using Microsoft.Extensions.DependencyInjection;

namespace Endor.TinyServices.OData.Interfaces.Boopstrapper;

public interface IODataInterpreterConfigService
{
	IServiceCollection Services { get; }
}