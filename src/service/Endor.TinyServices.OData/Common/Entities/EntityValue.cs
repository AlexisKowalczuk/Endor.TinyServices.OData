using Endor.TinyServices.OData.Common.Interfaces;

namespace Endor.TinyServices.OData.Common.Entities
{
	public class EntityValue : IPropertyValue
	{
		public string Value { get; set; }

		public EntityValue(string value)
		{
			Value = value;
		}

	}
}