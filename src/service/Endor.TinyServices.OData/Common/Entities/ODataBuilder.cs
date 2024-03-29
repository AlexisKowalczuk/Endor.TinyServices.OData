﻿using Endor.TinyServices.OData.Common.Constants;
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

		protected IDictionary<string, ODataMetaParameters> _metadata;

		public ODataBuilder()
		{
			_metadata = new Dictionary<string, ODataMetaParameters>();
		}

		public ODataMetaParameters GetBaseMetaParameter()
		{
			return _metadata[BaseEntityName];
		}

		public virtual string ToString()
		{
			return _query;
		}

		public void AddMetadata(string entity, ODataMetaParameters parameter)
		{
			if (parameter != null) _metadata.Add(entity, parameter);
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
				column.ColumnName = $"{parameters.FirstOrDefault(x => x.Value.Name == alias).Value.Entity.Name}.{column.ColumnName.Substring(alias.Length + 1)}";
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
			if(!_metadata.ContainsKey(entity)) throw new ODataParserException($"Unable to get entity name [{entity}]");
			
			return _metadata[entity].Name;
		}

		public void RemoveParameter(string item, string entity)
		{
			var meta = _metadata[entity];
			meta.RemoveProperty(item);

		}

		public bool ExistsPropertyMeta(string entity, string property)
		{
			var meta = _metadata[entity];
			return meta.Properties.ContainsKey(property);
		}

		public IDictionary<string, PropertyInfo> GetMetaProperties(string entity)
		{
			return _metadata[entity].Properties;
		}
	}
}