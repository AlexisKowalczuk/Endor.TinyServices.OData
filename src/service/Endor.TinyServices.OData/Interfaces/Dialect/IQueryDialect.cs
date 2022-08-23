using Endor.TinyServices.OData.Common.Entities;
using Endor.TinyServices.OData.Common.Enums;

namespace Endor.TinyServices.OData.Interfaces.Dialect;

public interface IQueryDialect
{
	Task<ODataBuilder> Init(string entityName);

	Task TopStatement(int number, ODataBuilder builder);

	Task SkipStatement(int number, ODataBuilder builder);

	Task ExpandStatement(string entity, IList<(QueryTypeParameter, string)> additionalInfo, ODataBuilder builder);

	Task FilterStatement(string filter, ODataBuilder builder);

	Task OrderByStatement(string property, bool asc, bool thenBy, ODataBuilder builder);

	Task SelectStatement(string[] properties, string entity, ODataBuilder query);
}