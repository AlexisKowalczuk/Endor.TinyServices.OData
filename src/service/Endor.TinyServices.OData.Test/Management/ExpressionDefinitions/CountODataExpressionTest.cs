using Endor.TinyServices.OData.Common.Entities;
using Endor.TinyServices.OData.Common.Enums;
using Endor.TinyServices.OData.Helper;
using Endor.TinyServices.OData.Interfaces.Dialect;
using Endor.TinyServices.OData.Management.ExpressionDefinitions;
using Endor.TinyServices.OData.Test.Common;
using Moq;

namespace Endor.TinyServices.OData.Test.Helper
{
	public class CountODataExpressionTest
	{
		CountODataExpression _service;
		Mock<IQueryDialect> _mockQueryDialect;
		public CountODataExpressionTest()
		{
			_mockQueryDialect = new Mock<IQueryDialect>();
			_service = new CountODataExpression(_mockQueryDialect.Object);
		}

		[Fact]
		public async Task CountTrueTest()
		{
			var builder = new FooODataBuilder();
			await _service.Interpret(bool.TrueString, builder);

			Assert.True(builder.Count);
		}

		[Fact]
		public async Task CountFalseTest()
		{
			var builder = new FooODataBuilder();
			await _service.Interpret(bool.FalseString, builder);

			Assert.False(builder.Count);
		}
	}
}