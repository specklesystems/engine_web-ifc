using System.Runtime.InteropServices;
namespace Speckle.WebIfc;

public class IfcGeometry(IntPtr api, IntPtr geometry)
{
  [DllImport(WebIfc.DllName)]
  [DefaultDllImportSearchPaths(WebIfc.ImportSearchPath)]
  private static extern IntPtr GetMesh(IntPtr api, IntPtr geometry, int index);

  [DllImport(WebIfc.DllName)]
  [DefaultDllImportSearchPaths(WebIfc.ImportSearchPath)]
  private static extern int GetNumMeshes(IntPtr api, IntPtr geometry);
  
  [DllImport(WebIfc.DllName)]
  [DefaultDllImportSearchPaths(WebIfc.ImportSearchPath)]
  private static extern uint GetGeometryType(IntPtr api, IntPtr geometry);

  public IfcMesh GetMesh(int i) => new(api, GetMesh(api, geometry, i));

  public int MeshCount => GetNumMeshes(api, geometry);

  public IfcSchemaType Type => (IfcSchemaType)GetGeometryType(api, geometry);

  public IEnumerable<IfcMesh> GetMeshes()
  {
    for (int i = 0; i < MeshCount; ++i)
    {
      yield return GetMesh(i);
    }
  }
}
