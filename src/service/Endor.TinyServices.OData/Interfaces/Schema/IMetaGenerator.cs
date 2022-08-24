using Endor.TinyServices.OData.Common.Entities;

namespace Endor.TinyServices.OData.Interfaces.Schema;

public interface IMetaGenerator
{
	Task<MetaInfo> GenerateMetadata(string entity, object additionalInfo = null);
}