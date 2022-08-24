using Endor.TinyServices.OData.Common.Entities;
using Endor.TinyServices.OData.Common.Enums;
using Endor.TinyServices.OData.Helper;
using Endor.TinyServices.OData.Interfaces.Dialect;
using Endor.TinyServices.OData.Management.ExpressionDefinitions;
using Endor.TinyServices.OData.Test.Common;
using Moq;

namespace Endor.TinyServices.OData.Test.Helper
{
	public class ExpandODataExpressionTest
	{
		ExpandODataExpression _service;
		Mock<IQueryDialect> _mockQueryDialect;
		public ExpandODataExpressionTest()
		{
			_mockQueryDialect = new Mock<IQueryDialect>();
			_service = new ExpandODataExpression(_mockQueryDialect.Object);
		}

		[Fact]
		public async Task ExpandWithoutParameters()
		{
			var entity = "Product";

			var predicate = $"{entity}";
			var builder = new FooODataBuilder();
			await _service.Interpret(predicate, builder);

			_mockQueryDialect.Verify(x =>
				x.ExpandStatement(entity, null, builder), Times.Once);
		}

		[Fact]
		public async Task ExpandWithProperties()
		{
			var entity = "Product";
			var op = "$select";
			var fields = "Name,ProductNumber";

			var predicate = $"{entity}({op}={fields})";
			var builder = new FooODataBuilder();
			await _service.Interpret(predicate, builder);

			_mockQueryDialect.Verify(x =>
				x.ExpandStatement(entity, new List<(QueryTypeParameter, string)>() { new(QueryTypeParameter.Select, fields) }, builder), Times.Once);
		}

	}
}