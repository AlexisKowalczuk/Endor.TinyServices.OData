using Endor.TinyServices.OData.Common.Entities;

namespace Endor.TinyServices.OData.Interfaces;

public interface ISchemaDefinition
{
	Task<ODataBuilder> Initialize(string entity);
}