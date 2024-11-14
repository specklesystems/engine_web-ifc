using System.Runtime.InteropServices;
namespace Speckle.WebIfc;

public class IfcGeometry( IntPtr geometry)
{
  [DllImport(WebIfc.DllName)]
  [DefaultDllImportSearchPaths(WebIfc.ImportSearchPath)]
  private static extern IntPtr GetMesh( IntPtr geometry, int index);

  [DllImport(WebIfc.DllName)]
  [DefaultDllImportSearchPaths(WebIfc.ImportSearchPath)]
  private static extern int GetNumMeshes( IntPtr geometry);
  
  [DllImport(WebIfc.DllName)]
  [DefaultDllImportSearchPaths(WebIfc.ImportSearchPath)]
  private static extern uint GetGeometryType( IntPtr geometry);

  public IfcMesh GetMesh(int i) => new(GetMesh(geometry, i));

  public int MeshCount => GetNumMeshes(geometry);

  public IfcSchemaType Type => (IfcSchemaType)GetGeometryType(geometry);

  public IEnumerable<IfcMesh> GetMeshes()
  {
    for (int i = 0; i < MeshCount; ++i)
    {
      yield return GetMesh(i);
    }
  }
}
