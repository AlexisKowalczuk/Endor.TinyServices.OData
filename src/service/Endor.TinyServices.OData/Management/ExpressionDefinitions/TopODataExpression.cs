using Endor.TinyServices.OData.Common.Entities;
using Endor.TinyServices.OData.Common.Exceptions;
using Endor.TinyServices.OData.Interfaces;
using Endor.TinyServices.OData.Interfaces.Dialect;

namespace Endor.TinyServices.OData.Management.ExpressionDefinitions;

public class TopODataExpression : IODataExpression
{
	private IQueryDialect _dialect;

	public TopODataExpression(IQueryDialect dialect)
	{
		_dialect = dialect;
	}
	public async Task Interpret(string predicate, ODataBuilder query)
	{
		if (!int.TryParse(predicate, out int number))
			throw new ODataParserException($"Unable to parse number [{predicate}] on top statement.");

		await _dialect.TopStatement(number, query);
	}
}
