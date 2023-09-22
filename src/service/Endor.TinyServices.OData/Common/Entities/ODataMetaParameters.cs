using System.Reflection;

namespace Endor.TinyServices.OData.Common.Entities
{
	public abstract class ODataMetaParameters
	{
		public string Name { get; set; }
		public Type Entity { get; set; }
		public IDictionary<string, PropertyInfo> Properties { get; set; }

		public abstract string GetColumnsString();

		internal void RemoveProperty(string item)
		{
			Properties.Remove(item);
		}
	}
}