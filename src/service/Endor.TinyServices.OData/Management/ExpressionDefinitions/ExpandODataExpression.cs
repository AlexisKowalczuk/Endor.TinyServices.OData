using Endor.TinyServices.OData.Common.Entities;
using Endor.TinyServices.OData.Common.Enums;
using Endor.TinyServices.OData.Common.Exceptions;
using Endor.TinyServices.OData.Common.Helper;
using Endor.TinyServices.OData.Interfaces.Dialect;
using System.Text.RegularExpressions;

namespace Endor.TinyServices.OData.Management.ExpressionDefinitions;

public class ExpandODataExpression : IODataExpression
{
	private IQueryDialect _dialect;

	public ExpandODataExpression(IQueryDialect dialect)
	{
		_dialect = dialect;
	}
	public async Task Interpret(string predicate, ODataBuilder query)
	{
		var list = GetEntityPredicateList(predicate);

		foreach (var item in list)
		{
			var additionalInfo = GetExpandPredicateStatements(item.Item2);

			await _dialect.ExpandStatement(item.Item1, additionalInfo, query);
		}

	}

	private IList<(QueryTypeParameter, string)> GetExpandPredicateStatements(string data)
	{
		if (data == null) return null;

		var result = new List<(QueryTypeParameter, string)>();

		var statements = data.Split(';');
		foreach (var stat in statements)
		{
			var operators = data.Split('=');
			var op = EnumHelper.GetValueFromName<QueryTypeParameter>(operators[0]);
			if (op == QueryTypeParameter.Unknown) throw new ODataParserException($"Unable to process operator [{operators[0]}]");

			result.Add(new(op, operators.Length > 1 ? operators[1] : null));
		}

		return result;
	}

	private IList<(string, string)> GetEntityPredicateList(string data)
	{
		string rawData = data;
		var result = new List<(string, string)>();

		do
		{
			var entity = GetEntityName(ref rawData);
			if (entity == null) break;

			var predicate = GetPredicate(ref rawData);
			result.Add((entity, predicate));
		} while (rawData.Length != 0);

		return result;
	}

	public string GetEntityName(ref string data)
	{
		var pattern = "^[\\w]*";
		var match = Regex.Match(data, pattern);

		if (!match.Success) return null;

		data = data.Substring(match.Value.Length, data.Length - match.Value.Length);
		return match.Value;
	}

	public string GetPredicate(ref string data)
	{
		if (data == null || data == string.Empty) return null;

		if (data[0] == ',')
		{
			data = data.Substring(1, data.Length - 1);
			return null;
		}

		if (data[0] == '(')
		{
			var index = data.IndexOf(')');

			var result = data.Substring(1, index - 1);

			data = data.Substring(index + 1, data.Length - index - 1);

			if (data.Length > 0 && data[0] == ',') data = data.Remove(0, 1);

			return result;
		}

		throw new ODataParserException($"Unable to process predicate [{data}]");

	}
}
