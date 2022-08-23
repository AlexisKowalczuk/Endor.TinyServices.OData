using Endor.TinyServices.OData.Common.Constants;
using Endor.TinyServices.OData.Common.Enums;
using Endor.TinyServices.OData.Common.Helper;

namespace Endor.TinyServices.OData.Helper;

public class ODataQueryParser
{
	private string _query;

	public async Task SetQuery(string query)
	{
		_query = query;
	}

	public async Task<(QueryTypeParameter, string?)> NextParameter()
	{
		//$operator=/#predicate
		var sentence = SplitString(_query, ODataConstants.QUERY_SEPARATOR).Item1;

		var parameter = (Type: QueryTypeParameter.Unknown, Predicate: (string?)null);

		if (sentence == null) return parameter;

		if (sentence.Contains(ODataConstants.QUERY_PROPERTY_DEFINITION))
		{
			var result = SplitString(sentence, ODataConstants.QUERY_PROPERTY_DEFINITION);
			parameter.Type = EnumHelper.GetValueFromName<QueryTypeParameter>(result.Item1);
			parameter.Predicate = result.Item2;
		}
		else if (sentence.Contains(ODataConstants.QUERY_OPERATOR_ASSIGNMENT))
		{
			var result = SplitString(sentence, ODataConstants.QUERY_OPERATOR_ASSIGNMENT);
			parameter.Type = EnumHelper.GetValueFromName<QueryTypeParameter>(result.Item1);
			parameter.Predicate = result.Item2;
		}
		else
		{
			parameter.Type = EnumHelper.GetValueFromName<QueryTypeParameter>(sentence);
		}

		return parameter;
	}

	private static (string, string?) SplitString(string sentence, char separator)
	{
		if (sentence == null || !sentence.Contains(separator)) return (sentence, null);

		var result = sentence.Split(separator);
		return (result[0], sentence.Substring(sentence.IndexOf(separator) + 1, sentence.Length - sentence.IndexOf(separator) - 1));
	}



}