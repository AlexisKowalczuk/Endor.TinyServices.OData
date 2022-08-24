using Endor.TinyServices.OData.Common.Entities;
using Endor.TinyServices.OData.Common.Exceptions;
using Endor.TinyServices.OData.Interfaces;
using Endor.TinyServices.OData.Interfaces.Dialect;

namespace Endor.TinyServices.OData.Management.ExpressionDefinitions;

public class OrderByODataExpression : IODataExpression
{
	private IQueryDialect _dialect;

	public OrderByODataExpression(IQueryDialect dialect)
	{
		_dialect = dialect;
	}
	public async Task Interpret(string predicate, ODataBuilder query)
	{
		var items = predicate.Split(',');
		bool thenBy = false;
		foreach (var item in items)
		{
			var rawItem = item.Split(' ');
			var property = rawItem[0];

			bool asc = true;
			if (rawItem.Length > 1)
			{
				if (rawItem[1] == "asc")
					asc = true;
				else if (rawItem[1] == "desc")
					asc = false;
				else throw new ODataParserException($"Unable to process Order By {rawItem[1]}");
			}

			await _dialect.OrderByStatement(property, asc, thenBy, query);
			thenBy = true;
		}
	}

}
