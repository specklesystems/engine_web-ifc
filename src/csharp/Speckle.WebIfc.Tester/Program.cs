

using Speckle.WebIfc;

//var file = "/home/adam/git/engine_web-ifc/examples/example.ifc";
var file = "C:\\Users\\adam\\Git\\engine_web-ifc\\examples\\example.ifc";

Console.WriteLine(Environment.CurrentDirectory);
Console.WriteLine(WebIfc.GetVersion());

var apiPtr = WebIfc.InitializeApi();
if (!File.Exists(file))
{
  throw new FileNotFoundException("File not found", file);
}
var modelPtr = WebIfc.LoadModel(apiPtr, file);
var geoCount = WebIfc.GetNumGeometries(modelPtr);
Console.WriteLine(geoCount);
WebIfc.FinalizeApi(apiPtr);
