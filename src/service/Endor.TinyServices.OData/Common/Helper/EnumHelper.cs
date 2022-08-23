using System.ComponentModel.DataAnnotations;

namespace Endor.TinyServices.OData.Common.Helper;

public static class EnumHelper
{
	public static T GetValueFromName<T>(string name) where T : Enum
	{
		var type = typeof(T);

		foreach (var field in type.GetFields())
		{
			if (Attribute.GetCustomAttribute(field, typeof(DisplayAttribute)) is DisplayAttribute attribute)
			{
				if (attribute.Name == name)
				{
					return (T)field.GetValue(null);
				}
			}

			if (field.Name == name)
			{
				return (T)field.GetValue(null);
			}
		}
		return (T)Activator.CreateInstance(type);
	}
}
