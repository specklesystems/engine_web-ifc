

using Speckle.WebIfc;

Console.WriteLine(Environment.Is64BitOperatingSystem);


var api = WebIfc.InitializeApi();

var model = WebIfc.LoadModel(api, "/home/adam/git/engine_web-ifc/examples/example.ifc");

Console.WriteLine(WebIfc.GetNumGeometries(api, model));

WebIfc.FinalizeApi(api);
