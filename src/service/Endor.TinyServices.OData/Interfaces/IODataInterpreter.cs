using Endor.TinyServices.OData.Common.Enums;

namespace Endor.TinyServices.OData.Interfaces;

public interface IODataInterpreter
{
	Task<string> ProcessODataQuery(string entity, IDictionary<string, string> odataquery, MetaGeneratorType type, string context);
}