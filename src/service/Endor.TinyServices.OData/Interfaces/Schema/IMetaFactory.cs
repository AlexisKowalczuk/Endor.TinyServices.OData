using Endor.TinyServices.OData.Common.Enums;

namespace Endor.TinyServices.OData.Interfaces.Schema;

public interface IMetaFactory
{
	Task<IMetaGenerator> GetMetaGenerator(MetaGeneratorType type);
}