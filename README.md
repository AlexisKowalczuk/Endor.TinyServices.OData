
# Endor.TinyServices.OData
Small Odata service protocol for dotnet.

# Configuration

Install **Endor.TinyServices.OData** and **Endor.TinyServices.OData.Sql** on the main project

On program.cs declare:

```c
builder.Services.UseODataInterpreter().UseSql("Default");
```
The UseSql parameter is the current connection string.


Declare OData endpoint after the build:

```c
app.AddODataEndpoint();
```

assemblies names are necessary to find the entities to be expose over the protocol (declared on appsettings)

```json
"ODataServiceConfig": {
    "Assemblies": [ "BLLEntities" ]
  }
```

The service finds al classes inside the "entities" folder name by default. It can be changed using "EntitiesFolder" property from ODataServiceConfig over appsettings file.

# How to use

This service exposes over the endpoint: "https://.../OData/"

The command list included into the service:

* $metadata
* $filter
* $expand -> Only one level and without additional parameters except $select
* $select
* $orderby
* $count
* $skip
* $take

# Additional configuration

OData Conventions:

* [ODataTableName("TableName")] attribute can be used to associate a table with the current class. In otheway each class is associated with the name by default.

* The entity requires an "Id" field. Is possible to declare a entity key using [ODataKey] attribute.

* The Odata conventions define the relationships between entities with the parameter "{Entity}Id" format. To redefine for one property use [ODataReference(nameof("Class"))] attribute

* IDialect interface provides a way to extend the service to other kind of databases. 

# Limitations

The relationships over the entity must be relationship one to one o many to one but one to many are not allowed yet.

# Performance test

Environment: AdventureWorks2019 and ASPNet 6.0
Minimal API using Dapper and EF to compare the performance between OData service protocol.

Performance average from 100 requests to each endpoint on each scenario.

## Case #1

Query simple over Work Order table.

Table: WorkOrder


**Performance**		
Select (n)	|Service (ms) |Dapper (ms) |EF (ms)
|------|:------:|:-----:|:------:|
|1	|7.02	|18.16	|23.74
|5	|4.49	|18.68	|23.4
|10	|5.45	|20.5	|23.59
|15	|4.53	|18.42	|21.59
|20	|4.38	|20.35	|21.28
|30	|7.34	|20.07	|23.55
|50	|6.05	|18.7	|21.12
|100	|6.49	|19.5	|21.93


## Case #2

Query with one join statement.

Table: WorkOrder
Join: Product

**Performance**		
Select (n)	|Service (ms) |Dapper (ms) |EF (ms)
|------|:------:|:-----:|:------:|
|1	|6.73	|20.42	|23.9
|5	|6.62	|18.97	|21.88
|10	|6.28	|20.33	|21.58
|15	|7.62	|19.3	|22.45
|20	|8.36	|18.05	|23.71
|30	|8.94	|19.75	|21.71
|50	|13.11	|19.86	|23.48
|100|20.01	|20.73	|25.98

## Case #3

Query with join and filter.

Table: WorkOrder
Join: Product
Filter: [Product].[WeightUnitMeasureCode] = 'G' & [WorkOrder].[StockedQty] >= 1

**Performance**		
Select (n)	|Service (ms) |Dapper (ms) |EF (ms)
|------|:------:|:-----:|:------:|
|1	|6.75	|18.82	|22.23
|5	|5.07	|19.42	|21.61
|10	|5.93	|20.82	|23.56
|15	|7.13	|19.03	|23.38
|20	|7.4	|19.02	|22.01
|30	|7.99	|19.91	|21.43
|50	|9.96	|18.81	|21.67
|100|15.42	|20.89	|25.08


## Case #4

Query with join, filter and order by statement.

Table: WorkOrder
Join: Product
Filter: [Product].[WeightUnitMeasureCode] = 'G' & [WorkOrder].[StockedQty] >= 1
OrderBy: [Product].[ProductNumber]

**Performance**		
Select (n)	|Service (ms) |Dapper (ms) |EF (ms)
|------|:------:|:-----:|:------:|
|1	|5.3		|16.33	|21.05
|5	|5.67	|17.98	|21.51
|10	|5.57	|16.61	|20.165
|15	|6.27	|19.44	|22.04
|20	|6.42	|18.92	|21.21
|30	|6.85	|19.58	|21.83
|50	|10.66	|19.53	|21.48
|100	|15.46	|19.52	|22.15
