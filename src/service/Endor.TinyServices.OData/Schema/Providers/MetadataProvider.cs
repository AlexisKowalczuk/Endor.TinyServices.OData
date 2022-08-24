using Endor.TinyServices.OData.Common.Config;
using Endor.TinyServices.OData.Interfaces.Schema;
using Microsoft.Extensions.Configuration;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Endor.TinyServices.OData.Schema.Providers;

public class MetadataProvider : IMetadataProvider
{
	private static IList<Type> _entities;

	public MetadataProvider(IConfiguration configuration)
	{
		var config = new ODataServiceConfig();
		configuration.GetSection(nameof(ODataServiceConfig)).Bind(config);

		_entities = GetEntitiesFromAssemblies(config.Assemblies, config.EntitiesFolder);
	}


	private IList<Type> GetEntitiesFromAssemblies(IList<string> assemblies, string folder)
	{
		if (folder == null) folder = ".Entities";

		return (from asm in AppDomain.CurrentDomain.GetAssemblies()
						.Where(a => assemblies.Any(c => c == a.GetName().Name))
				from type in asm.GetTypes()
				where type.Namespace != null && type.Namespace.Contains(folder) &&
							asm.ManifestModule.Name != "<In Memory Module>"
							&& !asm.FullName.StartsWith("System")
							&& !asm.FullName.StartsWith("Microsoft")
							&& asm.Location.IndexOf("App_Web") == -1
							&& asm.Location.IndexOf("App_global") == -1
							&& asm.FullName.IndexOf("CppCodeProvider") == -1
							&& asm.FullName.IndexOf("WebMatrix") == -1
							&& asm.FullName.IndexOf("SMDiagnostics") == -1
							&& !string.IsNullOrEmpty(asm.Location)
				select type).ToList();
	}

	public async Task<IList<Type>> GetEntitiesInfo(string entity)
	{
		if (entity != null) return _entities.Where(x => x.Name == entity).ToList();

		return _entities.ToList();
	}

	public async Task<Type> GetEntity(string entity)
	{
		return _entities.FirstOrDefault(x => x.Name == entity);
	}

	public async Task<bool> ExistsProperty(string entity, string property)
	{
		var item = await GetEntity(entity);

		return item.GetProperties().Any(x => x.Name == property);
	}
}
