using Endor.TinyServices.OData.Common.Attributes;
using Endor.TinyServices.OData.Common.Constants;
using Endor.TinyServices.OData.Common.Entities;
using Endor.TinyServices.OData.Common.Helper;
using Endor.TinyServices.OData.Interfaces.Schema;
using Endor.TinyServices.OData.Schema.Helper;
using System.Xml;

namespace Endor.TinyServices.OData.Schema;

public class AtomMetaGenerator : IMetaGenerator
{
	private IMetadataProvider _provider;

	public AtomMetaGenerator(IMetadataProvider provider)
	{
		_provider = provider;
	}

	public async Task<MetaInfo> GenerateMetadata(string entity, object additionalInfo = null)
	{
		var stream = await GenerateXML();
		stream.Position = 0;

		var reader = new StreamReader(stream);
		var result = reader.ReadToEnd();

		var item = new MetaInfo();
		item.Value = result;
		return item;
	}

	public async Task<Stream> GenerateXML()
	{
		var sts = new XmlWriterSettings()
		{
			Indent = true,
			IndentChars = "\t"
		};

		var stream = new MemoryStream();
		using var writer = XmlWriter.Create(stream, sts);

		writer.WriteStartDocument();
		writer.WriteStartElement(MetaConstants.Edmx.PREFIX, MetaConstants.Edmx.NAME, MetaConstants.Edmx.NAMESPACE);
		writer.WriteAttributeString(MetaConstants.Service.VERSION_DEF, MetaConstants.Service.VERSION_VALUE);

		writer.WriteStartElement(MetaConstants.Edmx.PREFIX, MetaConstants.DataServices.NAME, null);
		writer.WriteStartElement(MetaConstants.Schema.NAME, MetaConstants.Schema.NAMESPACE);
		writer.WriteAttributeString(MetaConstants.Service.NAMESPACE_DEF, MetaConstants.Service.NAMESPACE_VALUE);

		await SetProperties(writer);

		//SET Annotations

		writer.WriteEndElement();
		writer.WriteEndElement();
		writer.WriteEndDocument();

		return stream;
	}

	private async Task SetProperties(XmlWriter writer)
	{
		var entities = await _provider.GetEntitiesInfo();
		foreach (var item in entities)
		{
			writer.WriteStartElement(MetaConstants.Entity.NAME);
			writer.WriteAttributeString(MetaConstants.Entity.Property.NAME, item.Name);

			var idProp = AttributeHelper.GetIdForEntity(item);
			writer.WriteStartElement(MetaConstants.Entity.KEY);
			writer.WriteStartElement(MetaConstants.Entity.REF);
			writer.WriteAttributeString(MetaConstants.Entity.Property.NAME, idProp);
			writer.WriteEndElement();
			writer.WriteEndElement();

			var props = item.GetProperties();

			foreach (var propItem in props.Where(i => i.PropertyType.Namespace == "System"))
			{
				var prop = Nullable.GetUnderlyingType(propItem.PropertyType);

				if (prop == null) prop = propItem.PropertyType;

				writer.WriteStartElement(MetaConstants.Entity.Property.DEF);
				writer.WriteAttributeString(MetaConstants.Entity.Property.NAME, propItem.Name);
				writer.WriteAttributeString(MetaConstants.Entity.Property.TYPE, MetaConstants.ConvertTypeToODataType(prop.Name));

				if (prop.Name != nameof(String))
					writer.WriteAttributeString("Nullable", prop == propItem.PropertyType ? "True" : "False");

				writer.WriteEndElement();
			}

			foreach (var prop in GetReferenceLists(item, entities))
			{
				writer.WriteStartElement(MetaConstants.Entity.Property.NAVIGATION);
				writer.WriteAttributeString(MetaConstants.Entity.Property.NAME, prop.Item1);
				writer.WriteAttributeString(MetaConstants.Entity.Property.TYPE, prop.Item2);
				writer.WriteAttributeString(MetaConstants.Entity.Property.PARTNER, item.Name);
				writer.WriteEndElement();
			}

			writer.WriteEndElement();
		}
	}

	private IList<(string, string)> GetReferenceLists(Type type, IList<Type> entities)
	{
		var result = new List<(string, string)>();

		var props = type.GetProperties();

		result.AddRange(props.Select(entity => (Key: entity.GetCustomAttributes(typeof(ODataReferenceAttribute), true).FirstOrDefault(), Value: entity))
						.Where(x => x.Key != null && x.Value.Name != type.Name)
						.Select(x => (((ODataReferenceAttribute)x.Key).Entity, $"{MetaConstants.Service.NAMESPACE_VALUE}.{((ODataReferenceAttribute)x.Key).Entity}")).ToList());

		foreach (var prop in props.Where(x => x.Name.EndsWith(ODataConventions.PropertyIndentifierConvention, StringComparison.CurrentCultureIgnoreCase)))
		{
			var entityString = prop.Name.Substring(0, prop.Name.Length - ODataConventions.PropertyIndentifierConvention.Length);

			var entity = entities.FirstOrDefault(x => x.Name == entityString);
			if (entity != null && entity.Name != type.Name && result.All(x => x.Item1 != entity.Name))
			{
				result.Add(new(entity.Name, $"{MetaConstants.Service.NAMESPACE_VALUE}.{entity.Name}"));
			}

		}
		return result;
	}
}