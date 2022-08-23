using Endor.TinyServices.OData.Common.Enums;

namespace Endor.TinyServices.OData.Parser.Helper;

internal class FilterFunctionTypeHelper
{
	internal static bool HasOperator(FilterFunctionType ft)
	{
		return !(
				ft == FilterFunctionType.matchesPattern ||
				ft == FilterFunctionType.contains ||
				ft == FilterFunctionType.endswith ||
				ft == FilterFunctionType.startswith
				);
	}
}