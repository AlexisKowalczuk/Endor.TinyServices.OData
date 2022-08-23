namespace Endor.TinyServices.OData.Schema.Interfaces;

public interface IMetadataProvider
{
	Task<IList<Type>> GetEntitiesInfo(string entity = null);

	Task<Type> GetEntity(string entity);

	Task<bool> ExistsProperty(string entity, string property);
}