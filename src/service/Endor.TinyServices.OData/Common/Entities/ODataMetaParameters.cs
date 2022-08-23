using System.Reflection;

namespace Endor.TinyServices.OData.Common.Entities
{
	public abstract class ODataMetaParameters
	{
		public string Name { get; set; }
		public Type Entity { get; set; }
		public IList<PropertyInfo> Properties { get; set; }

		public abstract string GetColumnsString();

		internal void RemoveProperty(string item)
		{
			var prop = Properties.First(x => x.Name == item);
			Properties.Remove(prop);
		}
	}
}