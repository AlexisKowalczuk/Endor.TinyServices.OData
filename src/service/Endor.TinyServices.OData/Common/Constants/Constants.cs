using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Endor.TinyServices.OData.Common.Constants;

public static class ODataConstants
{
	public const char QUERY_SEPARATOR = '&';
	public const char QUERY_PROPERTY_DEFINITION = '#';
	public const char QUERY_OPERATOR_ASSIGNMENT = '=';

}


public static class ODataConventions
{
	public const string PropertyIndentifierConvention = "Id";
	public const string ColumnSeparator = "_";
}