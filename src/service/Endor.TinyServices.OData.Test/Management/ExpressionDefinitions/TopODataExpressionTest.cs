using Endor.TinyServices.OData.Common.Entities;
using Endor.TinyServices.OData.Common.Enums;
using Endor.TinyServices.OData.Helper;
using Endor.TinyServices.OData.Interfaces.Dialect;
using Endor.TinyServices.OData.Management.ExpressionDefinitions;
using Endor.TinyServices.OData.Test.Common;
using Moq;

namespace Endor.TinyServices.OData.Test.Helper
{
	public class TopODataExpressionTest
	{
		TopODataExpression _service;
		Mock<IQueryDialect> _mockQueryDialect;
		public TopODataExpressionTest()
		{
			_mockQueryDialect = new Mock<IQueryDialect>();
			_service = new TopODataExpression(_mockQueryDialect.Object);
		}

		[Fact]
		public async Task TopTest()
		{
			var topValue = 10;
			var builder = new FooODataBuilder();
			await _service.Interpret(topValue.ToString(), builder);

			_mockQueryDialect.Verify(x =>
				x.TopStatement(topValue, builder), Times.Once);
		}


	}
}