using Endor.TinyServices.OData.Common.Entities;
using Endor.TinyServices.OData.Common.Enums;
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
		app.UseEndpoints(delegate (IEndpointRouteBuilder endpoints)
		{
			//CONFIG SET BASE ODATA PATH -> api/v1/OData


			endpoints.MapGet("odata", async ([FromServices] IODataMetaServices service, HttpContext context) =>
			{
				var path = $"{context.Request.Scheme}://{context.Request.Host}{context.Request.Path}/$metadata";
				var info = await service.GenerateMetadata(MetaGeneratorType.json, additionalInfo: path);
				var json = JsonConvert.SerializeObject(info.Value);
				return Results.Text(json, contentType: "application/json");
			}).ExcludeFromDescription();

			//endpoints.MapGet("odata/$metadata{entity:regex([#]?(.*))}", (string? entity) => $"Post {entity}");
			endpoints.MapGet("odata/$metadata", async ([FromServices] IODataMetaServices service, HttpContext context) =>
			{
				MetaInfo info = new MetaInfo();

				if (context.Request.ContentType == "application/json")
				{
					var path = $"{context.Request.Scheme}://{context.Request.Host}{context.Request.Path}";
					info = await service.GenerateMetadata(MetaGeneratorType.json, additionalInfo: path);
					var json = JsonConvert.SerializeObject(info.Value);
					return Results.Text(json, contentType: "application/json");
				}
				else if (context.Request.ContentType == null || context.Request.ContentType == "application/atom+xml")
				{
					var path = $"{context.Request.Scheme}://{context.Request.Host}{context.Request.Path}";
					info = await service.GenerateMetadata(MetaGeneratorType.xml_atom, additionalInfo: path);
					return Results.Text(info.Value.ToString(), contentType: "application/atom+xml");
				}
				else
				{
					throw new NotSupportedException($"Content type [{context.Request.ContentType}] is not supported");
				}

			}).ExcludeFromDescription();

			//$metadata#{entity}
			//endpoints.MapGet("odata/{entity}/$metadata", async ([FromRoute] string entity, [FromServices] IODataMetaServices service) =>
			//{
			//	return "Metadata";
			//});//.ExcludeFromDescription();


			//endpoints.MapGet("/", async (LinkGenerator linker) => {
			//	return $"The link to the hello route is {linker.GetPathByName("Hi", values: null)}";
			//});



			//CONFIG SET BASE ODATA PATH -> api/v1/OData
			endpoints.MapGet("odata/{entity}", async ([FromRoute] string entity, [FromServices] IODataInterpreter service, HttpContext context, HttpRequest request) =>
			{
				//request.Query.Keys
				var dict = request.Query.ToDictionary(c => c.Key, c => c.Value.ToString());
				var path = $"{context.Request.Scheme}://{context.Request.Host}{context.Request.Path}/$metadata#{entity}";
				var result = await service.ProcessODataQuery(entity, dict, MetaGeneratorType.json, path);

				return Results.Text(result, contentType: "application/json"); ;
			}).ExcludeFromDescription();
		});
		return app;
	}

	//ERROR HANDLING 

	/*
	 {
    "error": {
        "code": "",
        "message": "Could not find a property named 'Employee' on type 'ODataDemo.Person'.",
        "innererror": {
            "message": "Could not find a property named 'Employee' on type 'ODataDemo.Person'.",
            "type": "Microsoft.OData.Core.ODataException",
            "stacktrace": "   at Microsoft.OData.Core.UriParser.Parsers.SelectExpandBinder.GenerateExpandItem(ExpandTermToken tokenIn)\r\n   at System.Linq.Enumerable.WhereSelectEnumerableIterator`2.MoveNext()\r\n   at System.Linq.Enumerable.WhereEnumerableIterator`1.MoveNext()\r\n   at System.Collections.Generic.List`1.InsertRange(Int32 index, IEnumerable`1 collection)\r\n   at Microsoft.OData.Core.UriParser.Parsers.SelectExpandBinder.Bind(ExpandToken tokenIn)\r\n   at Microsoft.OData.Core.UriParser.Parsers.SelectExpandSemanticBinder.Bind(IEdmStructuredType elementType, IEdmNavigationSource navigationSource, ExpandToken expandToken, SelectToken selectToken, ODataUriParserConfiguration configuration)\r\n   at Microsoft.OData.Core.UriParser.ODataQueryOptionParser.ParseSelectAndExpandImplementation(String select, String expand, ODataUriParserConfiguration configuration, IEdmStructuredType elementType, IEdmNavigationSource navigationSource)\r\n   at Microsoft.OData.Core.UriParser.ODataQueryOptionParser.ParseSelectAndExpand()\r\n   at Microsoft.OData.Service.ExpandAndSelectParseResult..ctor(RequestDescription requestDescription, IDataService dataService)"
        }
    }
}
	 */
}

