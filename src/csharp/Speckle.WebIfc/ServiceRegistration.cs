using Microsoft.Extensions.DependencyInjection;

namespace Speckle.WebIfc;

public static class ServiceRegistration
{
  public static void AddSpeckleWebIfc(this IServiceCollection services)
  {
    services.AddSingleton<IIfcFactory, IfcFactory>();
  }
}
