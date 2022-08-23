using Endor.TinyServices.OData.Common.Interfaces;

namespace Endor.TinyServices.OData.Parser.Predicates;

public abstract class Predicate
{
	public abstract string Read(IDialectExpressionConverter converter);

}
