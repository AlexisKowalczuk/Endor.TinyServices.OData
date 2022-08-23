using System.Data;

namespace Endor.TinyServices.OData.Interfaces;

public interface IDataAccess
{
	Task<DataTable> ExcecuteQuery(string query);
}