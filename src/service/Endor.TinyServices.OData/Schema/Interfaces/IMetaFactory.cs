using Endor.TinyServices.OData.Common.Enums;

namespace Endor.TinyServices.OData.Schema.Interfaces;

public interface IMetaFactory
{
	Task<IMetaGenerator> GetMetaGenerator(MetaGeneratorType type);
}