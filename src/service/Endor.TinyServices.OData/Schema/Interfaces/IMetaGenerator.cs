using Endor.TinyServices.OData.Common.Entities;

namespace Endor.TinyServices.OData.Schema.Interfaces;

public interface IMetaGenerator
{
	Task<MetaInfo> GenerateMetadata(string entity, object additionalInfo = null);
}