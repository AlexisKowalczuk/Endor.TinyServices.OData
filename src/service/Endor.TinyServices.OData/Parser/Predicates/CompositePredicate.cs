using Endor.TinyServices.OData.Common.Enums;
using Endor.TinyServices.OData.Common.Interfaces;

namespace Endor.TinyServices.OData.Parser.Predicates;

public class CompositePredicate : Predicate
{
	public IList<Predicate> Predicates { get; set; }
	public EntityOperatorType Operator { get; set; }

	public CompositePredicate(IList<Predicate> predicates, EntityOperatorType @operator)
	{
		Predicates = predicates;
		Operator = @operator;
	}

	public override string Read(IDialectExpressionConverter converter)
	{
		if (Operator == EntityOperatorType.grouping)
			return $"({string.Join(' ', Predicates.Select(x => x.Read(converter)))})";
		else if (Operator == EntityOperatorType.none)
			return $"{string.Join(' ', Predicates.Select(x => x.Read(converter)))}";
		else
			return $"{string.Join(' ', Predicates.Select(x => x.Read(converter)))} {converter.TransformExpression(Operator)}";
	}
}
