using Speckle.InterfaceGenerator;
using Speckle.Objects.Geometry;

namespace Speckle.WebIfc.Converters;

[GenerateAutoInterface]
public class MeshConverter : IMeshConverter
{
  public unsafe Mesh Convert(IfcMesh mesh)
  {
    var r = new Mesh()
    {
      faces = [],
      vertices = [],
      colors = [],
      units = "meters",
    };
    var m = (double*)mesh.Transform;
    var vp = mesh.GetVertices();
    var ip = mesh.GetIndexes();

    for (var i = 0; i < mesh.VertexCount; i++)
    {
      var x = vp[i].PX;
      var y = vp[i].PY;
      var z = vp[i].PZ;
      r.vertices.Add(m[0] * x + m[4] * y + m[8] * z + m[12]);
      r.vertices.Add(-(m[2] * x + m[6] * y + m[10] * z + m[14]));
      r.vertices.Add(m[1] * x + m[5] * y + m[9] * z + m[13]);
    }

    for (var i = 0; i < mesh.IndexCount; i += 3)
    {
      var a = ip[i];
      var b = ip[i + 1];
      var c = ip[i + 2];
      r.faces.Add(0);
      r.faces.Add(a);
      r.faces.Add(b);
      r.faces.Add(c);
    }

    var color = mesh.GetColor();
    r.colors =
    [
      (int)(color->A * 255),
      (int)(color->R * 255),
      (int)(color->G * 255),
      (int)(color->B * 255),
    ];
    return r;
  }
}
