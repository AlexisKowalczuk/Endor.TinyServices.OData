using Endor.TinyServices.OData.Common.Entities;
using Endor.TinyServices.OData.Common.Enums;
using Endor.TinyServices.OData.Helper;
using Endor.TinyServices.OData.Interfaces.Dialect;
using Endor.TinyServices.OData.Management;
using Endor.TinyServices.OData.Management.ExpressionDefinitions;
using Endor.TinyServices.OData.Parser;
using Endor.TinyServices.OData.Parser.Predicates;
using Endor.TinyServices.OData.Test.Common;
using Moq;

namespace Endor.TinyServices.OData.Test.Management
{
	public class ODataParserTest
	{
		public ODataParserTest()
		{
		}

		[Fact]
		public async Task ParserComplexStatementTest()
		{
			var parser = new ODataParser();
			var data = "OrderQty eq '8' or Product/Name eq 'FR-R38B-58'";
			var result = parser.Parse(ref data);

			Assert.IsType<CompositePredicate>(result);
			var comp = (CompositePredicate)result;
			Assert.Equal(EntityOperatorType.none, comp.Operator);
			Assert.Equal(2, comp.Predicates.Count);

			Assert.IsType<CompositePredicate>(comp.Predicates[0]);
			var compChild = (CompositePredicate)comp.Predicates[0];
			Assert.Equal(EntityOperatorType.or, compChild.Operator);
			Assert.Equal(1, compChild.Predicates.Count);

			Assert.IsType<PropertyPredicate>(compChild.Predicates[0]);
			var propFirstChild = (PropertyPredicate)compChild.Predicates[0];
			Assert.Equal(string.Empty, propFirstChild.EntityPair.Entity);
			Assert.Equal("OrderQty", propFirstChild.EntityPair.Property);
			Assert.Equal(PropertyOperatorType.eq, propFirstChild.Operator.GetValueOrDefault());
			Assert.Equal("'8'", ((EntityValue)propFirstChild.Value).Value);

			Assert.IsType<PropertyPredicate>(comp.Predicates[1]);
			var propChild = (PropertyPredicate)comp.Predicates[1];
			Assert.Equal("Product", propChild.EntityPair.Entity);
			Assert.Equal("Name", propChild.EntityPair.Property);
			Assert.Equal(PropertyOperatorType.eq, propChild.Operator.GetValueOrDefault());
			Assert.Equal("'FR-R38B-58'", ((EntityValue)propChild.Value).Value);

		}


	}
}