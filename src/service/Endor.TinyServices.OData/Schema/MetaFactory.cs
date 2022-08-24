using Endor.TinyServices.OData.Common.Enums;
using Endor.TinyServices.OData.Interfaces.Schema;

namespace Endor.TinyServices.OData.Schema;

public class MetaFactory : IMetaFactory
{
	private IDictionary<MetaGeneratorType, IMetaGenerator> _items;

	public MetaFactory(IDictionary<MetaGeneratorType, IMetaGenerator> items)
	{
		_items = items;
	}

	public async Task<IMetaGenerator> GetMetaGenerator(MetaGeneratorType type) => _items[type];
}