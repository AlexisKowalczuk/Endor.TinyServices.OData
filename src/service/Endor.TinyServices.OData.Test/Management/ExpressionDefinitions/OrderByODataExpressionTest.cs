using Endor.TinyServices.OData.Common.Entities;
using Endor.TinyServices.OData.Common.Enums;
using Endor.TinyServices.OData.Helper;
using Endor.TinyServices.OData.Interfaces.Dialect;
using Endor.TinyServices.OData.Management.ExpressionDefinitions;
using Endor.TinyServices.OData.Test.Common;
using Moq;

namespace Endor.TinyServices.OData.Test.Helper
{
	public class OrderByODataExpressionTest
	{
		OrderByODataExpression _service;
		Mock<IQueryDialect> _mockQueryDialect;
		public OrderByODataExpressionTest()
		{
			_mockQueryDialect = new Mock<IQueryDialect>();
			_service = new OrderByODataExpression(_mockQueryDialect.Object);
		}

		[Fact]
		public async Task OrderByAscendingTest()
		{
			var property = "ProductNumber";
			
			var predicate = $"{property} asc";
			var builder = new FooODataBuilder();
			await _service.Interpret(predicate, builder);

			_mockQueryDialect.Verify(x =>
				x.OrderByStatement(property, true, false, builder), Times.Once);
		}

		[Fact]
		public async Task OrderByDescendingTest()
		{
			var property = "ProductKey";

			var predicate = $"{property} desc";
			var builder = new FooODataBuilder();
			await _service.Interpret(predicate, builder);

			_mockQueryDialect.Verify(x =>
				x.OrderByStatement(property, false, false, builder), Times.Once);
		}

		[Fact]
		public async Task OrderByDescendingThenBy()
		{
			var p1 = "ProductKey";
			var p2 = "Type";

			var predicate = $"{p1} desc,{p2}";
			var builder = new FooODataBuilder();
			await _service.Interpret(predicate, builder);

			_mockQueryDialect.Verify(x => x.OrderByStatement(p1, false, false, builder), Times.Once);
			_mockQueryDialect.Verify(x => x.OrderByStatement(p2, true, true, builder), Times.Once);
		}
	}
}