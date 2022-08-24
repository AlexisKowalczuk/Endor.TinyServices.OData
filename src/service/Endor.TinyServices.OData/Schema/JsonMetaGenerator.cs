using Endor.TinyServices.OData.Common.Entities;
using Endor.TinyServices.OData.Interfaces.Schema;
using Newtonsoft.Json;

namespace Endor.TinyServices.OData.Schema;

public record JsonMetaResult([property: JsonProperty("@odata.context")] string Context, JsonMetaItem[] Value);
public record JsonMetaItem(string Name, string Kind, string URL);
public class JsonMetaGenerator : IMetaGenerator
{
	private IMetadataProvider _provider;

	public JsonMetaGenerator(IMetadataProvider provider)
	{
		_provider = provider;
	}

	public async Task<MetaInfo> GenerateMetadata(string entity, object additionalInfo = null)
	{

		var context = (string)additionalInfo;

		var entities = await _provider.GetEntitiesInfo(entity);
		var list = new List<JsonMetaItem>();

		foreach (var item in entities)
		{
			list.Add(new JsonMetaItem(item.Name, "EntitySet", item.Name));
		}

		var result = new MetaInfo();


		result.Store(new JsonMetaResult(context, list.ToArray()));

		return result;
	}
}