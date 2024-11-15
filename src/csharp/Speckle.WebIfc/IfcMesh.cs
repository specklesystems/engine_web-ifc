using System.Runtime.InteropServices;

namespace Speckle.WebIfc;

public class IfcMesh(IntPtr mesh)
{
  [DllImport(WebIfc.DllName)]
  [DefaultDllImportSearchPaths(WebIfc.ImportSearchPath)]
  private static extern int GetNumVertices(IntPtr mesh);

  [DllImport(WebIfc.DllName)]
  [DefaultDllImportSearchPaths(WebIfc.ImportSearchPath)]
  private static extern IntPtr GetVertices(IntPtr mesh);

  [DllImport(WebIfc.DllName)]
  [DefaultDllImportSearchPaths(WebIfc.ImportSearchPath)]
  private static extern IntPtr GetTransform(IntPtr mesh);

  [DllImport(WebIfc.DllName)]
  [DefaultDllImportSearchPaths(WebIfc.ImportSearchPath)]
  private static extern int GetNumIndices(IntPtr mesh);

  [DllImport(WebIfc.DllName)]
  [DefaultDllImportSearchPaths(WebIfc.ImportSearchPath)]
  private static extern IntPtr GetIndices(IntPtr mesh);

  [DllImport(WebIfc.DllName)]
  [DefaultDllImportSearchPaths(WebIfc.ImportSearchPath)]
  private static extern IntPtr GetColor(IntPtr mesh);

  public int VertexCount => GetNumVertices(mesh);

  public unsafe IfcVertex* GetVertices() => (IfcVertex*)GetVertices(mesh);

  public IntPtr Transform => GetTransform(mesh);
  public int IndexCount => GetNumIndices(mesh);

  public unsafe int* GetIndexes() => (int*)GetIndices(mesh);

  public unsafe IfcColor* GetColor() => (IfcColor*)GetColor(mesh);
}
