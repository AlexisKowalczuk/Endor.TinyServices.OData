using BLLEntities.Entities;
using Endor.TinyServices.OData.Boopstrapper;
using Endor.TinyServices.OData.Sql.Extensions;

var builder = WebApplication.CreateBuilder(args);

builder.Services.AddEndpointsApiExplorer();
builder.Services.AddSwaggerGen();

builder.Services.UseODataInterpreter().UseSql("Default");

var app = builder.Build();

if (app.Environment.IsDevelopment())
{
	app.UseSwagger();
	app.UseSwaggerUI();
}

app.UseHttpsRedirection();
app.UseRouting();

app.MapGet("/DEMO", () =>
{
	var item = new WorkOrder();
	return item;
})
.WithName("DEMO");

app.AddODataEndpoint();

app.Run();
