using Endor.TinyServices.OData.Common.Enums;

namespace Endor.TinyServices.OData.Common.Interfaces;

public interface IDialectExpressionConverter
{
	string TransformExpression(object data);
	string TransformFunction(FilterFunctionType type, IList<IPropertyValue> parameters);
}
