using Endor.TinyServices.OData.Common.Enums;
using Endor.TinyServices.OData.Common.Interfaces;

namespace Endor.TinyServices.OData.Parser.Predicates;

public class CompositePredicate : Predicate
{
	IList<Predicate> _predicates { get; set; }
	public EntityOperatorType Operator { get; set; }

	public CompositePredicate(IList<Predicate> predicates, EntityOperatorType @operator)
	{
		_predicates = predicates;
		Operator = @operator;
	}

	public override string Read(IDialectExpressionConverter converter)
	{
		if (Operator == EntityOperatorType.grouping)
			return $"({string.Join(' ', _predicates.Select(x => x.Read(converter)))})";
		else if (Operator == EntityOperatorType.none)
			return $"{string.Join(' ', _predicates.Select(x => x.Read(converter)))}";
		else
			return $"{string.Join(' ', _predicates.Select(x => x.Read(converter)))} {converter.TransformExpression(Operator)}";
	}
}
