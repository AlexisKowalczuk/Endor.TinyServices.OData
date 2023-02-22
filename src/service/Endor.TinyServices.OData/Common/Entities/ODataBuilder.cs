using Endor.TinyServices.OData.Common.Constants;
using Endor.TinyServices.OData.Common.Exceptions;
using System.Data;
using System.Reflection;

namespace Endor.TinyServices.OData.Common.Entities
{
	public abstract class ODataBuilder
	{
		public abstract string GetNewEntityIndex();

		public abstract string GetColumnsString();

		public string TenantId { get; set; }

		public string BaseEntityName { get; set; }

		public bool Count { get; set; }

		protected string _query;

		protected IList<ODataMetaParameters> _metadata;

		public ODataBuilder()
		{
			_metadata = new List<ODataMetaParameters>();
		}

		public ODataMetaParameters GetBaseMetaParameter()
		{
			return _metadata.FirstOrDefault(x => x.Entity.Name == BaseEntityName);
		}

		public virtual string ToString()
		{
			return _query;
		}

		public void AddMetadata(ODataMetaParameters parameter)
		{
			if (parameter != null) _metadata.Add(parameter);
		}

		public void SetQuery(string query)
		{
			_query = query;
		}

		public void ProccessDataTable(DataTable dt)
		{
			var parameters = _metadata.ToList();
			var pruneColumns = new List<DataColumn>();

			foreach (DataColumn column in dt.Columns)
			{
				var columnSplit = column.ColumnName.Split(ODataConventions.ColumnSeparator);
				if (columnSplit.Length < 2)
				{
					pruneColumns.Add(column);
					continue;
				}

				var alias = columnSplit[0];
				column.ColumnName = $"{parameters.FirstOrDefault(x => x.Name == alias).Entity.Name}.{column.ColumnName.Substring(alias.Length + 1)}";
			}

			foreach (var column in pruneColumns)
			{
				dt.Columns.Remove(column);
			}

			dt.ExtendedProperties.Add(nameof(BaseEntityName), BaseEntityName);
			dt.ExtendedProperties.Add(nameof(Count), Count);

		}

		public string GetEntityAlias(string entity)
		{
			var meta = _metadata.FirstOrDefault(x => x.Entity.Name == entity);
			if (meta == null) throw new ODataParserException($"Unable to get entity name [{entity}]");

			return meta.Name;
		}

		public void RemoveParameter(string item, string entity)
		{
			var meta = _metadata.FirstOrDefault(x => x.Entity.Name == entity);
			meta.RemoveProperty(item);

		}

		public bool ExistsPropertyMeta(string entity, string property)
		{
			var meta = _metadata.FirstOrDefault(x => x.Entity.Name == entity);
			return meta.Properties.Any(x => x.Name == property);
		}

		public IList<PropertyInfo> GetMetaProperties(string entity)
		{
			return _metadata.FirstOrDefault(x => x.Entity.Name == entity).Properties;
		}
	}
}