using Endor.TinyServices.OData.Common.Entities;
using Endor.TinyServices.OData.Common.Enums;
using Endor.TinyServices.OData.Common.Exceptions;
using Endor.TinyServices.OData.Common.Helper;
using Endor.TinyServices.OData.Interfaces;
using Endor.TinyServices.OData.Management;
using Endor.TinyServices.OData.Schema.Extensions;
using System.Data;

namespace Endor.TinyServices.OData;

public class ODataInterpreter : IODataInterpreter
{
	private IDataAccess _dataAccess;
	private IDictionary<QueryTypeParameter, IODataExpression> _expressions;
	private ISchemaDefinition _schemaDefinition;

	public ODataInterpreter(IDictionary<QueryTypeParameter, IODataExpression> expressions, ISchemaDefinition schemaDefinition, IDataAccess dataAccess)
	{
		_dataAccess = dataAccess;
		_expressions = expressions;
		_schemaDefinition = schemaDefinition;
	}

	public async Task<string> ProcessODataQuery(string entity, IDictionary<string, string> queryParameters, MetaGeneratorType type, string context)
	{
		ODataBuilder query = await _schemaDefinition.Initialize(entity);

		if (queryParameters == null) return await ProcessQuery(query, context);

		foreach (var item in queryParameters.Keys.Select(x => (Key: x, Type: EnumHelper.GetValueFromName<QueryTypeParameter>(x))).OrderBy(x => x.Type))
		{
			if (item.Type == QueryTypeParameter.Unknown) throw new ODataParserException($"Parameter [{item.Key}] unknown");

			if (!_expressions.ContainsKey(item.Type)) throw new ODataParserException($"Parameter [{item.Key}] is not supported");

			await _expressions[item.Type].Interpret(queryParameters[item.Key], query);
		}

		return await ProcessQuery(query, context);

	}

	private async Task<string> ProcessQuery(ODataBuilder query, string context)
	{
		var dt = await _dataAccess.ExcecuteQuery(query.ToString());

		query.ProccessDataTable(dt);

		return dt.ToODataJson(context);
	}
}