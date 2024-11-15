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
      Console.WriteLine(geo.Type.ToString());

      c.elements.Add(geometryConverter.Convert(geo));
    }

    var max = model.GetMaxId();
    for (uint i = 0; i < max; i++)
    {
      var l = model.GetLine(i);
      if (l is not null)
      {
        var x = l.Arguments();
        if (x.Length > 0)
        {
          Console.WriteLine($"{l.Id} {l.Type} {l.Arguments()}");
        }
      }
    }

    if (c.elements.Count > 0)
    {
      b["displayValue"] = c.elements;
    }

    return b;
  }
}
