namespace Endor.TinyServices.OData.Interfaces.Schema;

public interface IMetadataProvider
{
	Task<IList<Type>> GetEntitiesInfo(string entity = null);

	Task<Type> GetEntity(string entity);

	Task<bool> ExistsProperty(string entity, string property);
}