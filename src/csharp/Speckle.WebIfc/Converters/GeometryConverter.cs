using Speckle.InterfaceGenerator;
using Speckle.Sdk.Models.Collections;

namespace Speckle.WebIfc.Converters;

[GenerateAutoInterface]
public class GeometryConverter(IMeshConverter meshConverter) : IGeometryConverter
{
  public Collection Convert(IfcGeometry geometry)
  {
    var c = new Collection();
    foreach (var mesh in geometry.GetMeshes())
    {
      c.elements.Add(meshConverter.Convert(mesh));
    }

    return c;
  }
}
