using System.Runtime.InteropServices;

namespace Speckle.WebIfc;

public class IfcMesh(IntPtr api, IntPtr mesh)
{
  [DllImport(WebIfc.DllName)]
  [DefaultDllImportSearchPaths(WebIfc.ImportSearchPath)]
  private static extern int GetNumVertices(IntPtr api, IntPtr mesh);

  [DllImport(WebIfc.DllName)]
  [DefaultDllImportSearchPaths(WebIfc.ImportSearchPath)]
  private static extern IntPtr GetVertices(IntPtr api, IntPtr mesh);

  public unsafe List<IfcVertex> GetVertices()
  {
    var count = GetNumVertices(api, mesh);
    var start = (IfcVertex*)GetVertices(api, mesh);
    var list = new List<IfcVertex>(count);
    for (int i = 0; i < count; i++)
    {
      list.Add(start[i]);
    }
    return list;
  }
}
