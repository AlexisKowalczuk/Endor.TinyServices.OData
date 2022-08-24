using Endor.TinyServices.OData.Common.Entities;
using Endor.TinyServices.OData.Interfaces;
using Endor.TinyServices.OData.Interfaces.Dialect;

namespace Endor.TinyServices.OData.Management.ExpressionDefinitions;

public class FilterODataExpression : IODataExpression
{
	private IQueryDialect _dialect;

	public FilterODataExpression(IQueryDialect dialect)
	{
		_dialect = dialect;
	}
	public async Task Interpret(string predicate, ODataBuilder query)
	{
		await _dialect.FilterStatement(predicate, query);
	}

}
