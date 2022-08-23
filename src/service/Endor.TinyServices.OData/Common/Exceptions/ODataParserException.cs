namespace Endor.TinyServices.OData.Common.Exceptions;

[Serializable]
public class ODataParserException : Exception
{
	public ODataParserException() : base("Unable to parse command")
	{
	}

	public ODataParserException(string message) : base(message)
	{
	}
}