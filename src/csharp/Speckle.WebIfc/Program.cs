using System.Reflection;
using Microsoft.Extensions.DependencyInjection;
using Speckle.Objects.Geometry;
using Speckle.Sdk.Host;
using Speckle.Sdk.Models;
using Speckle.WebIfc;
using Speckle.WebIfc.Converters;

TypeLoader.Initialize(typeof(Base).Assembly, typeof(Point).Assembly);
var serviceCollection = new ServiceCollection();
serviceCollection.AddSpeckleWebIfc();
serviceCollection.AddMatchingInterfacesAsTransient(Assembly.GetExecutingAssembly());

var serviceProvider = serviceCollection.BuildServiceProvider();

var factory = serviceProvider.GetRequiredService<IIfcFactory>();
Console.WriteLine(factory.Version);

var model = factory.Open("/home/adam/git/engine_web-ifc/examples/ifcbridge-model01.ifc");

var converter = serviceProvider.GetRequiredService<IModelConverter>();
var b = converter.Convert(model);
Console.WriteLine(b.GetTotalChildrenCount());
