using Endor.TinyServices.OData.Common.Attributes;

namespace BLLEntities.Entities
{
	[ODataTableName("Production.WorkOrder")]
	public class WorkOrder
	{
		public int WorkOrderID { get; set; }

		[ODataReference(nameof(Product))]
		public int ProductID { get; set; }
		public int OrderQty { get; set; }
		public int StockedQty { get; set; }
		public int ScrappedQty { get; set; }
		public DateTime StartDate { get; set; }
		public DateTime EndDate { get; set; }
		public DateTime DueDate { get; set; }
		public int ScrapReasonID { get; set; }
		public DateTime ModifiedDate { get; set; }
	}
}
