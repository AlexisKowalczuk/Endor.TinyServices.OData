using Endor.TinyServices.OData.Common.Entities;
using Newtonsoft.Json.Linq;
using System.Data;

namespace Endor.TinyServices.OData.Schema.Extensions;

public static class DataTableExtensions
{

	public static string ToODataJson(this DataTable dt, string context)
	{
		JObject jobj = new JObject();
		jobj.Add(new JProperty("@odata.context", context));
		JArray jarray = new JArray();

		if (bool.TryParse(dt.ExtendedProperties[nameof(ODataBuilder.Count)].ToString(), out bool result) && result)
			jobj.Add(new JProperty("@odata.count", dt.Rows.Count));

		string mainEntity = dt.ExtendedProperties[nameof(ODataBuilder.BaseEntityName)].ToString();

		foreach (DataRow row in dt.Rows)
		{
			var children = new Dictionary<string, JObject>();

			JObject jrow = new JObject();

			for (int i = 0; i < row.ItemArray.Length; i++)
			{
				var value = row.ItemArray[i] != DBNull.Value ? row.ItemArray[i] : null;

				var columnDef = dt.Columns[i].ToString().Split('.');

				var entityName = columnDef[0];
				var columnName = columnDef[1];
				if (entityName == mainEntity)
					jrow.Add(new JProperty(columnName, ConvertMetaDataToString(dt.Columns[i].DataType, value)));
				else
				{
					if (!children.ContainsKey(entityName))
						children.Add(entityName, new JObject());

					children[entityName].Add(new JProperty(columnName, ConvertMetaDataToString(dt.Columns[i].DataType, value)));
				}
			}

			foreach (var item in children)
			{
				jrow.Add(new JProperty(item.Key, item.Value));
			}

			jarray.Add(jrow);
		}

		jobj.Add(new JProperty("value", jarray));

		return jobj.ToString();
	}


	private static object ConvertMetaDataToString(Type type, object value)
	{
		if (value == null)
			return null;


		if (type == typeof(DateTime))
			return $"{(DateTime)value:o}";

		/*
		 Check -> ENUMS
		if (type.BaseType.Name == nameof(Enum) && value is string)
		{
			return ((int)Enum.Parse(type, value.ToString())).ToString();
		}
		 */

		if (type.IsAssignableFrom(value.GetType()))
		{
			return value;
		}

		return Convert.ChangeType(value, type);
	}
}