using Endor.TinyServices.OData.Common.Entities;
using Endor.TinyServices.OData.Common.Enums;
using Endor.TinyServices.OData.Common.Interfaces;

namespace Endor.TinyServices.OData.Parser.Predicates;

public class PropertyPredicate : Predicate
{
	public EntityPropertyPair EntityPair { get; set; }

	public IPropertyValue Value { get; set; }
	public PropertyOperatorType? Operator { get; set; }

	public PropertyPredicate(string entityName, string propertyName, IPropertyValue value, PropertyOperatorType? @operator)
	{
		EntityPair = new EntityPropertyPair(entityName, propertyName);
		Operator = @operator;
		Value = value;
	}

	public override string Read(IDialectExpressionConverter converter)
	{
		return $"{converter.TransformExpression(EntityPair)} {converter.TransformExpression(Operator, Value)} {converter.TransformExpression(Value)}";
	}
}
