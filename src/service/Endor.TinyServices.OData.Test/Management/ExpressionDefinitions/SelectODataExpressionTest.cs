using Endor.TinyServices.OData.Common.Entities;
using Endor.TinyServices.OData.Common.Enums;
using Endor.TinyServices.OData.Helper;
using Endor.TinyServices.OData.Interfaces.Dialect;
using Endor.TinyServices.OData.Management.ExpressionDefinitions;
using Endor.TinyServices.OData.Test.Common;
using Moq;

namespace Endor.TinyServices.OData.Test.Helper
{
	public class SelectODataExpressionTest
	{
		SelectODataExpression _service;
		Mock<IQueryDialect> _mockQueryDialect;
		public SelectODataExpressionTest()
		{
			_mockQueryDialect = new Mock<IQueryDialect>();
			_service = new SelectODataExpression(_mockQueryDialect.Object);
		}

		[Fact]
		public async Task SelectTest()
		{
			var p1 = "Key";
			var p2 = "Type";
			var p3 = "ProductNumber";

			var predicate = $"{p1},{p2},{p3}";
			var builder = new FooODataBuilder();
			await _service.Interpret(predicate, builder);

			_mockQueryDialect.Verify(x =>
				x.SelectStatement(It.Is<string[]>(x => x[0] == p1 && x[1] == p2 && x[2] == p3), null, builder), Times.Once);
		}


	}
}