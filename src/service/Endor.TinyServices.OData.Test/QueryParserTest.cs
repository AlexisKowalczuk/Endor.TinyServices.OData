using Endor.TinyServices.OData.Common.Enums;
using Endor.TinyServices.OData.Helper;

namespace ODataServiceTest
{
	public class QueryParserTest
	{
		[Fact]
		public async Task Test()
		{
			var query0 = "fooo";
			var parser = new ODataQueryParser();
			await parser.SetQuery(query0);
			var result = await parser.NextParameter();
			Assert.Equal(QueryTypeParameter.Unknown, result.Item1);
			Assert.Null(result.Item2);

			var query1 = "$count";
			await parser.SetQuery(query1);
			result = await parser.NextParameter();
			Assert.Equal(QueryTypeParameter.Count, result.Item1);
			Assert.Null(result.Item2);

			//var query2 = "$metadata#Product";
			//await parser.SetQuery(query2);
			//result = await parser.NextParameter();
			//Assert.Equal(QueryTypeParameter.Count, result.Item1);
			//Assert.Equal("Product", result.Item2);

			var query3 = "$top=5&$skip=0&$orderby=NId%20asc&$count=true&$expand=Facets($select=Siemens.SimaticIT.UAPI.PICore.PICore.PIPOMModel.DataModel.ReadingModel.BillOfMaterialsExtended/MaterialNId,Siemens.SimaticIT.UAPI.PICore.PICore.PIPOMModel.DataModel.ReadingModel.BillOfMaterialsExtended/MaterialRevision)&$filter=IsLocked%20eq%20false%20and%20(Facets/any(f%3Af/Siemens.SimaticIT.UAPI.PICore.PICore.PIPOMModel.DataModel.ReadingModel.BillOfMaterialsExtended/MaterialNId%20eq%20%275029157%27%20and%20f/Siemens.SimaticIT.UAPI.PICore.PICore.PIPOMModel.DataModel.ReadingModel.BillOfMaterialsExtended/MaterialRevision%20eq%20%271.2.0%27))";
			await parser.SetQuery(query3);
			result = await parser.NextParameter();
			Assert.Equal(QueryTypeParameter.Top, result.Item1);
			Assert.Equal("5", result.Item2);
		}
	}
}