using System.Runtime.InteropServices;

namespace Speckle.WebIfc;

public class IfcGeometry(IntPtr api, IntPtr geometry)
{
  [DllImport(WebIfc.DllName)]
  [DefaultDllImportSearchPaths(WebIfc.ImportSearchPath)]
  private static extern IntPtr GetMesh(IntPtr api, IntPtr geometry, int index);

  public IfcMesh GetMesh(int i) => new(api, GetMesh(api, geometry, i));
}
