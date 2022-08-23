using Endor.TinyServices.OData.Common.Entities;
using Endor.TinyServices.OData.Interfaces.Dialect;

namespace Endor.TinyServices.OData.Management.ExpressionDefinitions;

public class CountODataExpression : IODataExpression
{
	private IQueryDialect _dialect;

	public CountODataExpression(IQueryDialect dialect)
	{
		_dialect = dialect;
	}
	public async Task Interpret(string predicate, ODataBuilder query)
	{
		if (bool.TryParse(predicate, out var result) && result)
			query.Count = true;
	}
}
