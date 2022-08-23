using Endor.TinyServices.OData.Common.Entities;
using Endor.TinyServices.OData.Common.Exceptions;
using Endor.TinyServices.OData.Interfaces.Dialect;

namespace Endor.TinyServices.OData.Management.ExpressionDefinitions;

public class SkipODataExpression : IODataExpression
{
	private IQueryDialect _dialect;

	public SkipODataExpression(IQueryDialect dialect)
	{
		_dialect = dialect;
	}
	public async Task Interpret(string predicate, ODataBuilder query)
	{
		if (!int.TryParse(predicate, out int number))
			throw new ODataParserException($"Unable to parse number [{predicate}] on skip statement.");

		await _dialect.SkipStatement(number, query);
	}
}
