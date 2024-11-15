

using Speckle.WebIfc;

Console.WriteLine(Environment.CurrentDirectory);
Console.WriteLine(WebIfc.GetVersion());

var apiPtr = WebIfc.InitializeApi();
var modelPtr = WebIfc.LoadModel(apiPtr, "/home/adam/git/engine_web-ifc/examples/example.ifc");
var geoCount = WebIfc.GetNumGeometries(modelPtr);
Console.WriteLine(geoCount);
WebIfc.FinalizeApi(apiPtr);
