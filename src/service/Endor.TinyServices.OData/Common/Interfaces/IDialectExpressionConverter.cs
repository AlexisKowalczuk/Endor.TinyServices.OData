using Endor.TinyServices.OData.Common.Enums;

namespace Endor.TinyServices.OData.Common.Interfaces;

public interface IDialectExpressionConverter
{
	string TransformExpression(object data, object context = null);
	string TransformFunction(string entityName, string propertyName, FilterFunctionType type, IList<IPropertyValue> parameters);
}
