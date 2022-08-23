using Endor.TinyServices.OData.Common.Enums;
using Endor.TinyServices.OData.Common.Interfaces;

namespace Endor.TinyServices.OData.Parser.Predicates;

public class FunctionPredicate : Predicate
{
	private FilterFunctionType _ft;

	private IList<IPropertyValue> _parameters;
	private IPropertyValue? _value;

	public PropertyOperatorType Operator { get; set; }

	public FunctionPredicate(FilterFunctionType ft, IList<IPropertyValue> parameters, PropertyOperatorType op, IPropertyValue? value)
	{
		_ft = ft;
		_parameters = parameters;
		Operator = op;
		_value = value;
	}

	public override string Read(IDialectExpressionConverter converter)
	{
		var predicate = string.Empty;
		if (Operator != PropertyOperatorType.none)
			predicate = $"{converter.TransformExpression(Operator)} {converter.TransformExpression(_value)}";

		return $"{converter.TransformFunction(_ft, _parameters)} {predicate}";
	}
}
