namespace Endor.TinyServices.OData.Common.Exceptions;

[Serializable]
public class EntityNotFoundException : Exception
{
	public EntityNotFoundException(string entity) : base($"Entity [{entity}] not found.")
	{
	}

}