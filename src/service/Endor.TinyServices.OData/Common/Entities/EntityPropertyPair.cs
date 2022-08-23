using Endor.TinyServices.OData.Common.Interfaces;

namespace Endor.TinyServices.OData.Common.Entities
{
	public class EntityPropertyPair : IPropertyValue
	{
		public string? Entity { get; set; }
		public string Property { get; set; }

		public EntityPropertyPair(string? entity, string property)
		{
			Entity = entity;
			Property = property;
		}

	}
}