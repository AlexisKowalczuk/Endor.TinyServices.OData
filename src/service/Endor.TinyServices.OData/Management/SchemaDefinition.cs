using Endor.TinyServices.OData.Common.Entities;
using Endor.TinyServices.OData.Interfaces;
using Endor.TinyServices.OData.Interfaces.Dialect;

namespace Endor.TinyServices.OData.Management;

public class SchemaDefinition : ISchemaDefinition
{
	private IQueryDialect _dialect;

	public SchemaDefinition(IQueryDialect dialect)
	{
		_dialect = dialect;
	}
	public async Task<ODataBuilder> Initialize(string entity, string tenantId = null)
	{
		if (entity == null) return null;

		return await _dialect.Init(entity, tenantId);
	}
}
