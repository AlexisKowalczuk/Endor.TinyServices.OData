using Endor.TinyServices.OData.Common.Enums;
using Endor.TinyServices.OData.Helper;

namespace Endor.TinyServices.OData.Test.Helper
{
	public class QueryParserTest
	{
		[Fact]
		public async Task OneVariableStringTest()
		{
			var query0 = "fooo";
			var parser = new ODataQueryParser();
			await parser.SetQuery(query0);
			var result = await parser.NextParameter();
			Assert.Equal(QueryTypeParameter.Unknown, result.Item1);
			Assert.Null(result.Item2);

		}

		[Fact]
		public async Task OneCountParameterTest()
		{
			var parser = new ODataQueryParser();
			var query1 = "$count";
			await parser.SetQuery(query1);
			var result = await parser.NextParameter();
			Assert.Equal(QueryTypeParameter.Count, result.Item1);
			Assert.Null(result.Item2);
		}

		[Fact]
		public async Task OneComplexParameterTest()
		{
			var parser = new ODataQueryParser();
			var query3 = "$top=5"; 
			await parser.SetQuery(query3);
			var result = await parser.NextParameter();
			Assert.Equal(QueryTypeParameter.Top, result.Item1);
			Assert.Equal("5", result.Item2);

			query3 = "$skip=0"; ;
			await parser.SetQuery(query3);
			result = await parser.NextParameter();
			Assert.Equal(QueryTypeParameter.Skip, result.Item1);
			Assert.Equal("0", result.Item2);

			query3 = "$orderby=NId%20asc&$count=true&$expand=Material($selectMaterialNId)&$filter=IsLocked%20eq%20false"; ;
			await parser.SetQuery(query3);
			result = await parser.NextParameter();
			Assert.Equal(QueryTypeParameter.OrderBy, result.Item1);
			Assert.Equal("NId%20asc", result.Item2);

			query3 = "$count=true&$expand=Material($selectMaterialNId)&$filter=IsLocked%20eq%20false"; ;
			await parser.SetQuery(query3);
			result = await parser.NextParameter();
			Assert.Equal(QueryTypeParameter.Count, result.Item1);
			Assert.Equal("true", result.Item2);

			query3 = "$expand=Material($selectMaterialNId)&$filter=IsLocked%20eq%20false"; ;
			await parser.SetQuery(query3);
			result = await parser.NextParameter();
			Assert.Equal(QueryTypeParameter.Expand, result.Item1);
			Assert.Equal("Material($selectMaterialNId)", result.Item2);

			query3 = "$filter=IsLocked%20eq%20false";
			await parser.SetQuery(query3);
			result = await parser.NextParameter();
			Assert.Equal(QueryTypeParameter.Filter, result.Item1);
			Assert.Equal("IsLocked%20eq%20false", result.Item2);
		}
	}
}