using Endor.TinyServices.OData.Common.Entities;
using Endor.TinyServices.OData.Common.Enums;
using Endor.TinyServices.OData.Helper;
using Endor.TinyServices.OData.Interfaces.Dialect;
using Endor.TinyServices.OData.Management.ExpressionDefinitions;
using Endor.TinyServices.OData.Test.Common;
using Moq;

namespace Endor.TinyServices.OData.Test.Helper
{
	public class SkipODataExpressionTest
	{
		SkipODataExpression _service;
		Mock<IQueryDialect> _mockQueryDialect;
		public SkipODataExpressionTest()
		{
			_mockQueryDialect = new Mock<IQueryDialect>();
			_service = new SkipODataExpression(_mockQueryDialect.Object);
		}

		[Fact]
		public async Task SkipTest()
		{
			var skipValue = 10;
			var builder = new FooODataBuilder();
			await _service.Interpret(skipValue.ToString(), builder);

			_mockQueryDialect.Verify(x =>
				x.SkipStatement(skipValue, builder), Times.Once);
		}


	}
}