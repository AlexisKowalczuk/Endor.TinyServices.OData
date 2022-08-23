using Endor.TinyServices.OData.Interfaces.Boopstrapper;
using Microsoft.Extensions.DependencyInjection;

namespace Endor.TinyServices.OData.Boopstrapper;

internal class ODataInterpreterConfigService : IODataInterpreterConfigService
{
	private IServiceCollection _services;

	public IServiceCollection Services => _services;

	public ODataInterpreterConfigService(IServiceCollection services)
	{
		_services = services;
	}

}
