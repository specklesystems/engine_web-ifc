using Microsoft.Extensions.DependencyInjection;
using Speckle.WebIfc;

var serviceCollection = new ServiceCollection();
serviceCollection.AddSpeckleWebIfc();
var serviceProvider = serviceCollection.BuildServiceProvider();

var factory = serviceProvider.GetRequiredService<IIfcFactory>();

var model = factory.Open("/home/adam/git/engine_web-ifc/examples/example.ifc");

Console.WriteLine(model.GetNumGeometries());
