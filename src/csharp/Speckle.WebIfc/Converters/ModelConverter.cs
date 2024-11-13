using Speckle.InterfaceGenerator;
using Speckle.Sdk.Models;
using Speckle.Sdk.Models.Collections;

namespace Speckle.WebIfc.Converters;

[GenerateAutoInterface]
public class ModelConverter(IGeometryConverter geometryConverter) : IModelConverter
{
  public Base Convert(IfcModel model)
  {
    var b = new Base();
    var c = new Collection();
    foreach (var geo in model.GetGeometries())
    {
      c.elements.Add(geometryConverter.Convert(geo));
    }

    if (c.elements.Count > 0)
    {
      b["displayValue"] = c.elements;
    }

    return b;
  }
}
