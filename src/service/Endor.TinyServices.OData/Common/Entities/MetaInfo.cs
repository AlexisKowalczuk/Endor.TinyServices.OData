namespace Endor.TinyServices.OData.Common.Entities;

public class MetaInfo
{
	public object Value { get; set; } = new object();

	public void Store(object data)
	{
		Value = data;
	}

	public override string ToString() => Value.ToString();
}