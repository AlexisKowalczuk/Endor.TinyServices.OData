namespace Endor.TinyServices.OData.Common.Exceptions;

[Serializable]
public class TenantNotFoundException : Exception
{
	public TenantNotFoundException() : base($"TenantId cannot be null. Path: ...odata/tenantId/")
	{
	}

}