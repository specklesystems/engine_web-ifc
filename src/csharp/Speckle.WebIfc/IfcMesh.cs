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

  [DllImport(WebIfc.DllName)]
  [DefaultDllImportSearchPaths(WebIfc.ImportSearchPath)]
  private static extern IntPtr GetTransform(IntPtr api, IntPtr mesh);

  [DllImport(WebIfc.DllName)]
  [DefaultDllImportSearchPaths(WebIfc.ImportSearchPath)]
  private static extern int GetNumIndices(IntPtr api, IntPtr mesh);

  [DllImport(WebIfc.DllName)]
  [DefaultDllImportSearchPaths(WebIfc.ImportSearchPath)]
  private static extern IntPtr GetIndices(IntPtr api, IntPtr mesh);

  [DllImport(WebIfc.DllName)]
  [DefaultDllImportSearchPaths(WebIfc.ImportSearchPath)]
  private static extern IntPtr GetColor(IntPtr api, IntPtr mesh);

  public int VertexCount => GetNumVertices(api, mesh);

  public unsafe IfcVertex* GetVertices() => (IfcVertex*)GetVertices(api, mesh);

  public IntPtr Transform => GetTransform(api, mesh);
  public int IndexCount => GetNumIndices(api, mesh);

  public unsafe int* GetIndexes() => (int*)GetIndices(api, mesh);

  public unsafe IfcColor* GetColor() => (IfcColor*)GetColor(api, mesh);
}
