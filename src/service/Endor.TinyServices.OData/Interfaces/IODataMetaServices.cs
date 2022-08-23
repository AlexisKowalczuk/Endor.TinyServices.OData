using Endor.TinyServices.OData.Common.Entities;
using Endor.TinyServices.OData.Common.Enums;

namespace Endor.TinyServices.OData.Interfaces;

public interface IODataMetaServices
{
	Task<MetaInfo> GenerateMetadata(MetaGeneratorType type, string entity = null, object additionalInfo = null);
}