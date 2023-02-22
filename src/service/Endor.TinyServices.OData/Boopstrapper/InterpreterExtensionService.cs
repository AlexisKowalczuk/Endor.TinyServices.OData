using Endor.TinyServices.OData;
using Endor.TinyServices.OData.Common.Config;
using Endor.TinyServices.OData.Common.Enums;
using Endor.TinyServices.OData.Common.Exceptions;
using Endor.TinyServices.OData.Interfaces;
using Endor.TinyServices.OData.Interfaces.Boopstrapper;
using Endor.TinyServices.OData.Interfaces.Schema;
using Endor.TinyServices.OData.Management;
using Endor.TinyServices.OData.Management.ExpressionDefinitions;
using Endor.TinyServices.OData.Schema;
using Endor.TinyServices.OData.Schema.Providers;
using Microsoft.AspNetCore.Builder;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Routing;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;
using Newtonsoft.Json;

namespace Endor.TinyServices.OData.Boopstrapper;

public static class InterpreterExtensionService
{
	public static IODataInterpreterConfigService UseODataInterpreter(this IServiceCollection services)
	{
		services.AddScoped<ISchemaDefinition, SchemaDefinition>();
		services.AddScoped<IODataMetaServices, ODataMetaServices>();
		services.AddScoped<IMetadataProvider, MetadataProvider>();

		services.AddScoped<JsonMetaGenerator>();
		services.AddScoped<AtomMetaGenerator>();

		services.AddScoped<IMetaFactory>(c =>
		 new MetaFactory(
				new Dictionary<MetaGeneratorType, IMetaGenerator>()
				{
					[MetaGeneratorType.json] = c.GetRequiredService<JsonMetaGenerator>(),
					[MetaGeneratorType.xml_atom] = c.GetRequiredService<AtomMetaGenerator>(),
				}
		));

		services.AddScoped<TopODataExpression>();
		services.AddScoped<SkipODataExpression>();
		services.AddScoped<ExpandODataExpression>();
		services.AddScoped<FilterODataExpression>();
		services.AddScoped<OrderByODataExpression>();
		services.AddScoped<SelectODataExpression>();
		services.AddScoped<CountODataExpression>();

		services.AddScoped<IODataInterpreter>(c =>
		 new ODataInterpreter(
			 new Dictionary<QueryTypeParameter, IODataExpression>()
			 {
				 [QueryTypeParameter.Top] = c.GetRequiredService<TopODataExpression>(),
				 [QueryTypeParameter.Skip] = c.GetRequiredService<SkipODataExpression>(),
				 [QueryTypeParameter.Expand] = c.GetRequiredService<ExpandODataExpression>(),
				 [QueryTypeParameter.Filter] = c.GetRequiredService<FilterODataExpression>(),
				 [QueryTypeParameter.OrderBy] = c.GetRequiredService<OrderByODataExpression>(),
				 [QueryTypeParameter.Select] = c.GetRequiredService<SelectODataExpression>(),
				 [QueryTypeParameter.Count] = c.GetRequiredService<CountODataExpression>(),
			 },
			 c.GetRequiredService<ISchemaDefinition>(),
			 c.GetRequiredService<IDataAccess>()
		));


		return new ODataInterpreterConfigService(services);
	}

	public static IApplicationBuilder AddODataEndpoint(this IApplicationBuilder app)
	{
		var configuration = app.ApplicationServices.GetRequiredService<IConfiguration>();
		var config = new ODataServiceConfig();
		configuration.GetSection(nameof(ODataServiceConfig)).Bind(config);

		if (config.UseMultitenant)
		{
			app.UseEndpoints(delegate (IEndpointRouteBuilder endpoints)
			{
				endpoints.MapGet("{tenantId}/odata", async ([FromRoute] string tenantId, [FromServices] IODataMetaServices service, HttpContext context) =>
				{
					if (tenantId == null) throw new TenantNotFoundException();
					return await ProcessJsonMetadata(service, context, tenantId);
				}).ExcludeFromDescription();

				endpoints.MapGet("{tenantId}/odata/$metadata", async ([FromRoute] string tenantId, [FromServices] IODataMetaServices service, HttpContext context) =>
				{
					if (tenantId == null) throw new TenantNotFoundException();

					if (context.Request.ContentType == "application/json")
						return await ProcessJsonMetadata(service, context, tenantId);
					else if (context.Request.ContentType == null || context.Request.ContentType == "application/atom+xml")
						return await ProcessXmlnMetadata(service, context, tenantId);
					else throw new NotSupportedException($"Content type [{context.Request.ContentType}] is not supported");
				}).ExcludeFromDescription();

				endpoints.MapGet("{tenantId}/odata/{entity}", async ([FromRoute] string tenantId, [FromRoute] string entity, [FromServices] IODataInterpreter service, HttpContext context, HttpRequest request) =>
				{
					if (tenantId == null) throw new TenantNotFoundException();

					return await ProcessOdataQuery(entity, service, context, request, tenantId);
				}).ExcludeFromDescription();
			});
		}
		else
		{
			app.UseEndpoints(delegate (IEndpointRouteBuilder endpoints)
			{
				endpoints.MapGet("odata/", async ([FromServices] IODataMetaServices service, HttpContext context) =>
				{
					return await ProcessJsonMetadata(service, context);
				}).ExcludeFromDescription();

				endpoints.MapGet("odata/$metadata", async ([FromServices] IODataMetaServices service, HttpContext context) =>
				{
					if (context.Request.ContentType == "application/json")
						return await ProcessJsonMetadata(service, context);
					else if (context.Request.ContentType == null || context.Request.ContentType == "application/atom+xml")
						return await ProcessXmlnMetadata(service, context);
					else throw new NotSupportedException($"Content type [{context.Request.ContentType}] is not supported");
				}).ExcludeFromDescription();

				endpoints.MapGet("odata/{entity}", async ([FromRoute] string entity, [FromServices] IODataInterpreter service, HttpContext context, HttpRequest request) =>
				{
					return await ProcessOdataQuery(entity, service, context, request);
				}).ExcludeFromDescription();
			});
		}
		return app;
	}

	private static async Task<IResult> ProcessOdataQuery(string entity, IODataInterpreter service, HttpContext context, HttpRequest request, string tenantId = null)
	{
		var dict = request.Query.ToDictionary(c => c.Key, c => c.Value.ToString());
		var path = $"{context.Request.Scheme}://{context.Request.Host}{context.Request.Path}/$metadata#{entity}";
		var result = await service.ProcessODataQuery(entity, dict, MetaGeneratorType.json, path, tenantId);

		return Results.Text(result, contentType: "application/json"); ;
	}

	private static async Task<IResult> ProcessJsonMetadata(IODataMetaServices service, HttpContext context, string tenantId = null)
	{
		var path = $"{context.Request.Scheme}://{context.Request.Host}{context.Request.Path}/$metadata";

		var info = await service.GenerateMetadata(MetaGeneratorType.json, additionalInfo: path);
		var json = JsonConvert.SerializeObject(info.Value);
		return Results.Text(json, contentType: "application/json");
	}

	private static async Task<IResult> ProcessXmlnMetadata(IODataMetaServices service, HttpContext context, string tenantId = null)
	{
		var path = $"{context.Request.Scheme}://{context.Request.Host}{context.Request.Path}/$metadata";

		var info = await service.GenerateMetadata(MetaGeneratorType.xml_atom, additionalInfo: path);
		return Results.Text(info.Value.ToString(), contentType: "application/atom+xml");
	}
}

