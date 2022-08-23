using Endor.TinyServices.OData.Common.Entities;
using Endor.TinyServices.OData.Common.Enums;
using Endor.TinyServices.OData.Interfaces;
using Endor.TinyServices.OData.Schema.Interfaces;

namespace Endor.TinyServices.OData;

public class ODataMetaServices : IODataMetaServices
{
	private IMetaFactory _metaFactory;

	public ODataMetaServices(IMetaFactory metaFactory)
	{
		_metaFactory = metaFactory;
	}

	public async Task<MetaInfo> GenerateMetadata(MetaGeneratorType type, string entity = null, object additionalInfo = null)
	{
		var processor = await _metaFactory.GetMetaGenerator(type);
		var query = await processor.GenerateMetadata(entity, additionalInfo);
		return query;
	}
}