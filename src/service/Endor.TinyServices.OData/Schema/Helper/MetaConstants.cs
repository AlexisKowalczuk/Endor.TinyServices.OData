using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Endor.TinyServices.OData.Schema.Helper;

internal static class MetaConstants
{
	internal static class Service
	{
		internal const string NAMESPACE_DEF = "Namespace";
		internal const string NAMESPACE_VALUE = "ODataInterfaceService";

		internal const string VERSION_DEF = "Version";
		internal const string VERSION_VALUE = "4.0";
	}

	internal static class Edmx
	{
		internal const string NAME = "Edmx";
		internal const string PREFIX = "edmx";
		internal const string NAMESPACE = "http://docs.oasis-open.org/odata/ns/edmx";
	}

	internal static class Schema
	{
		internal const string NAME = "Schema";
		internal const string NAMESPACE = "http://docs.oasis-open.org/odata/ns/edm";
	}

	internal static class DataServices
	{
		internal const string NAME = "DataServices";
	}

	internal static class Entity
	{
		internal const string NAME = "EntityType";
		internal const string KEY = "Key";
		internal const string REF = "PropertyRef";

		internal static class Property
		{
			internal const string NAME = "Name";
			internal const string TYPE = "Type";

			internal const string DEF = "Property";
			internal const string NAVIGATION = "NavigationProperty";

			internal const string PARTNER = "Partner";
		}
	}

	internal static string? ConvertTypeToODataType(string name)
	{
		if (name == nameof(Int32))
			return "Edm.Int32";
		else if (name == nameof(String))
			return "Edm.String";
		else if (name == nameof(DateTime))
			return "Edm.DateTimeOffset";
		else if (name == nameof(Double))
			return "Edm.Double";
		else if (name == nameof(Boolean))
			return "Edm.Boolean";
		else if (name == nameof(Decimal))
			return "Edm.Single";
		else if (name == nameof(Byte))
			return "Edm.Byte";
		else
			return name;
	}
}
