using Endor.TinyServices.OData.Common.Entities;
using Endor.TinyServices.OData.Common.Enums;
using Endor.TinyServices.OData.Helper;
using Endor.TinyServices.OData.Interfaces.Dialect;
using Endor.TinyServices.OData.Management.ExpressionDefinitions;
using Endor.TinyServices.OData.Test.Common;
using Moq;

namespace Endor.TinyServices.OData.Test.Helper
{
	public class FilterODataExpressionTest
	{
		FilterODataExpression _service;
		Mock<IQueryDialect> _mockQueryDialect;
		public FilterODataExpressionTest()
		{
			_mockQueryDialect = new Mock<IQueryDialect>();
			_service = new FilterODataExpression(_mockQueryDialect.Object);
		}

		[Fact]
		public async Task ExpandWithoutParameters()
		{
			var predicate = $"IsLocked%20eq%20false";
			var builder = new FooODataBuilder();
			await _service.Interpret(predicate, builder);

			_mockQueryDialect.Verify(x =>
				x.FilterStatement(predicate, builder), Times.Once);
		}

	}
}