using Endor.TinyServices.OData.Common.Entities;

namespace Endor.TinyServices.OData.Interfaces;

public interface IODataExpression
{
	Task Interpret(string predicate, ODataBuilder query);

}