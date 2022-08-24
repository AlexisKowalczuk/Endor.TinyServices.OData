using Endor.TinyServices.OData.Common.Entities;
using Endor.TinyServices.OData.Interfaces;
using Endor.TinyServices.OData.Interfaces.Dialect;

namespace Endor.TinyServices.OData.Management.ExpressionDefinitions;

public class SelectODataExpression : IODataExpression
{
	private IQueryDialect _dialect;

	public SelectODataExpression(IQueryDialect dialect)
	{
		_dialect = dialect;
	}
	public async Task Interpret(string predicate, ODataBuilder query)
	{
		var properties = predicate.Split(',');

		await _dialect.SelectStatement(properties, query.BaseEntityName, query);
	}

}
