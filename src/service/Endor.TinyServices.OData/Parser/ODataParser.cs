using Endor.TinyServices.OData.Common.Entities;
using Endor.TinyServices.OData.Common.Enums;
using Endor.TinyServices.OData.Common.Interfaces;
using Endor.TinyServices.OData.Parser.Helper;
using Endor.TinyServices.OData.Parser.Predicates;
using System.Text.RegularExpressions;

namespace Endor.TinyServices.OData.Parser;

public class ODataParser
{
	public Predicate Parse(ref string data, EntityOperatorType operatorType = EntityOperatorType.none)
	{

		var list = new List<Predicate>();

		do
		{
			var item = ParseSentence(ref data);

			if (data.Length == 0)
			{
				list.Add(item);
			}
			else
			{
				var op = GetEntityOperatorType(ref data);
				list.Add(new CompositePredicate(new List<Predicate>() { item }, op));
			}
		}
		while (data.Length != 0);

		return new CompositePredicate(list, operatorType);
	}

	EntityOperatorType GetEntityOperatorType(ref string data)
	{
		var opString = GetWordFromString(ref data);

		EntityOperatorType op;
		if (!Enum.TryParse(opString, out op))
			throw new Exception($"Unable to parse [{opString}] operator.");

		return op;
	}

	private Predicate ParseSentence(ref string data)
	{
		var word = GetWordFromString(ref data);

		if (word == null && data[0] == '(')
		{
			var predicateString = GetGroupingSentence(ref data);

			return Parse(ref predicateString, EntityOperatorType.grouping);
		}
		else
		{
			FilterFunctionType ft;
			if (Enum.TryParse(word, out ft))
			{
				var parameters = GetGroupingSentence(ref data).Split(',');
				var parameterList = new List<IPropertyValue>();
				foreach (var param in parameters)
				{
					var rawEP = param.Split('/');
					if (rawEP.Length > 1)
						parameterList.Add(new EntityPropertyPair(rawEP[0], rawEP[1]));
					else
					{
						if (rawEP[0].StartsWith('\'') || decimal.TryParse(rawEP[0], out _))
							parameterList.Add(new EntityValue(rawEP[0]));
						else
							parameterList.Add(new EntityPropertyPair(null, rawEP[0]));
					}
				}

				//each parameter is a property, entity/property or a function
				PropertyOperatorType op = PropertyOperatorType.none;

				IPropertyValue value = null;
				if (FilterFunctionTypeHelper.HasOperator(ft))
				{
					var opString = GetWordFromString(ref data);
					Enum.TryParse(opString, out op);
					value = GetPropertyValue(ref data);
				}

				return new FunctionPredicate(ft, parameterList, op, value);
			}
			else
			{
				var entity = string.Empty;
				var property = string.Empty;

				if (data[0] == '/')
				{
					data = data.Substring(1);
					entity = word;
					property = GetWordFromString(ref data);
				}
				else
				{
					property = word;
				}

				var opString = GetWordFromString(ref data);

				PropertyOperatorType op;
				Enum.TryParse(opString, out op);

				var value = GetPropertyValue(ref data);
				return new PropertyPredicate(entity, property, value, op);
			}

		}

		throw new NotImplementedException();
	}

	private IPropertyValue GetPropertyValue(ref string data)
	{
		if (data[0] == '\'')
		{
			var index = data.Substring(1).IndexOf('\'');
			var value = data.Substring(0, index + 2);
			data = data.Substring(value.Length, data.Length - value.Length);
			return new EntityValue(value);
		}
		else
		{
			return GetEntityPropertyValue(ref data);
		}
	}


	private IPropertyValue GetEntityPropertyValue(ref string data)
	{
		var word = GetWordFromString(ref data);

		if (data != string.Empty && data[0] == '/')
		{
			data = data.Substring(1);
			var entity = word;
			var property = GetWordFromString(ref data);
			return new EntityPropertyPair(entity, property);
		}
		else
		{
			if (decimal.TryParse(word, out _))
				return new EntityValue(word);
			else
				return new EntityPropertyPair(null, word);
		}
	}

	private string GetGroupingSentence(ref string data)
	{
		var dataList = new List<(int Index, char Value)>();

		for (int i = 0; i < data.Length; i++)
		{
			if (data[i] == '(' || data[i] == ')') dataList.Add((i, data[i]));
		}

		foreach (var item in dataList.Where(x => x.Value == ')'))
		{
			var open = dataList.Where(x => x.Index <= item.Index && x.Value == '(').Count();
			var close = dataList.Where(x => x.Index <= item.Index && x.Value == ')').Count();

			if (open == close)
			{
				var predicate = data.Substring(1, item.Index - 1);
				if (data.Length == item.Index + 1)
					data = string.Empty;
				else
					data = data.Substring(item.Index + 1, data.Length - item.Index - 1);

				return predicate;
			}
		}

		throw new Exception("Unable to process grouping sentence. Check '(' and ')' operators");

	}

	private Predicate ProcessGroupingSentence(ref string data)
	{
		var predicateString = GetGroupingSentence(ref data);
		var opString = GetWordFromString(ref data);

		EntityOperatorType op;
		Enum.TryParse(opString, out op);

		return new CompositePredicate(new List<Predicate>() { Parse(ref predicateString) }, op);
	}

	public string GetWordFromString(ref string data)
	{
		var pattern = "^\\s?(\\w*)\\s?";
		var match = Regex.Match(data, pattern);

		if (!match.Success || match.Groups[1].Value == string.Empty) return null;

		data = data.Substring(match.Value.Length, data.Length - match.Value.Length);
		return match.Groups[1].Value;
	}
}
