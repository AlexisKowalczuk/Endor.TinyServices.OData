using Endor.TinyServices.OData.Common.Attributes;
using Endor.TinyServices.OData.Common.Constants;
using Endor.TinyServices.OData.Common.Exceptions;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Reflection;
using System.Text;
using System.Threading.Tasks;

namespace Endor.TinyServices.OData.Common.Helper;

public static class AttributeHelper
{

	public static string GetTableNameFromEntity(Type entity)
	{
		var att = entity.GetCustomAttributes(typeof(ODataTableNameAttribute), true).FirstOrDefault();
		if (att == null)
			return entity.Name;
		else
			return ((ODataTableNameAttribute)att).TableName;
	}

	public static PropertyInfo GetReferenceAttribute(Type type, string entityName)
	{
		var props = type.GetProperties();

		var atts = props.Select(entity => (Key: entity.GetCustomAttributes(typeof(ODataReferenceAttribute), true).FirstOrDefault(), Value: entity)).Where(x => x.Key != null && ((ODataReferenceAttribute)x.Key).Entity == entityName);

		if (atts.Count() > 1) throw new ODataParserException($"The entity [{type.Name}] has more that one reference attribute for one entity");

		var att = atts.FirstOrDefault();
		if (att.Key == null)
		{
			var result = props.FirstOrDefault(x => x.Name == $"{entityName}{ODataConventions.PropertyIndentifierConvention}");
			if (result == null) throw new ODataParserException($"Property [{entityName}{ODataConventions.PropertyIndentifierConvention}] not found on Entity [{type.Name}]");

			return result;
		}

		return att.Value;
	}

	public static string GetIdForEntity(Type type)
	{
		var props = type.GetProperties();

		var atts = props.Select(entity => (Key: entity.GetCustomAttributes(typeof(ODataKeyAttribute), true).FirstOrDefault(), Value: entity)).Where(x => x.Key != null);

		if (atts.Count() > 1) throw new ODataParserException($"The entity [{type.Name}] has more that one key attribute for one entity");

		var att = atts.FirstOrDefault();

		if (att.Key == null) return ODataConventions.PropertyIndentifierConvention;

		var prop = att.Value;
		return prop.Name;
	}
}
