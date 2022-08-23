using System.ComponentModel.DataAnnotations;

namespace Endor.TinyServices.OData.Common.Enums;

public enum QueryTypeParameter
{
	[Display(Name = "unknown")]
	Unknown = -1,

	//[Display(Name = "$metadata")]
	//Metadata,

	[Display(Name = "$expand")]
	Expand = 1,

	[Display(Name = "$filter")]
	Filter = 2,

	[Display(Name = "$select")]
	Select = 3,

	[Display(Name = "$orderby")]
	OrderBy = 6,

	[Display(Name = "$skip")]
	Skip = 4,

	[Display(Name = "$top")]
	Top = 5,

	[Display(Name = "$count")]
	Count = 7,
}