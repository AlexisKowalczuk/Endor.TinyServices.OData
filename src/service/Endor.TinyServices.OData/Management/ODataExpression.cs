using Endor.TinyServices.OData.Common.Entities;

namespace Endor.TinyServices.OData.Management;

public interface IODataExpression
{
	Task Interpret(string predicate, ODataBuilder query);

}