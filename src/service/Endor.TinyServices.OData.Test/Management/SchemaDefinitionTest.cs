using Endor.TinyServices.OData.Common.Entities;
using Endor.TinyServices.OData.Common.Enums;
using Endor.TinyServices.OData.Helper;
using Endor.TinyServices.OData.Interfaces.Dialect;
using Endor.TinyServices.OData.Management;
using Endor.TinyServices.OData.Management.ExpressionDefinitions;
using Endor.TinyServices.OData.Test.Common;
using Moq;

namespace Endor.TinyServices.OData.Test.Management
{
	public class SchemaDefinitionTest
	{
		SchemaDefinition _service;
		Mock<IQueryDialect> _mockQueryDialect;
		public SchemaDefinitionTest()
		{
			_mockQueryDialect = new Mock<IQueryDialect>();
			_service = new SchemaDefinition(_mockQueryDialect.Object);
		}

		[Fact]
		public async Task TopTest()
		{
			var entity = "fooEntity";
			var builder = new FooODataBuilder();
			await _service.Initialize(entity);

			_mockQueryDialect.Verify(x =>
				x.Init(entity), Times.Once);
		}


	}
}