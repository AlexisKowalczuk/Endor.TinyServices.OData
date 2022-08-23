namespace Endor.TinyServices.OData.Common.Attributes;

public class ODataTableNameAttribute : Attribute
{

	public string TableName { get; set; }

	public ODataTableNameAttribute(string tableName)
	{
		TableName = tableName;
	}
}

public class ODataReferenceAttribute : Attribute
{

	public string Entity { get; set; }

	public ODataReferenceAttribute(string entity)
	{
		Entity = entity;
	}
}

public class ODataKeyAttribute : Attribute
{
	public ODataKeyAttribute()
	{
	}
}