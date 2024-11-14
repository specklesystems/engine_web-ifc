using System.Runtime.InteropServices;

namespace Speckle.WebIfc;

public class IfcModel(IntPtr api, IntPtr model)
{
  [DllImport(WebIfc.DllName)]
  [DefaultDllImportSearchPaths(WebIfc.ImportSearchPath)]
  private static extern IntPtr GetGeometryFromId(IntPtr api, IntPtr model, uint id);

  [DllImport(WebIfc.DllName)]
  [DefaultDllImportSearchPaths(WebIfc.ImportSearchPath)]
  private static extern int GetNumGeometries(IntPtr api, IntPtr model);

  [DllImport(WebIfc.DllName)]
  [DefaultDllImportSearchPaths(WebIfc.ImportSearchPath)]
  private static extern IntPtr GetGeometryFromIndex(IntPtr api, IntPtr model, int index);

  [DllImport(WebIfc.DllName)]
  [DefaultDllImportSearchPaths(WebIfc.ImportSearchPath)]
  private static extern uint GetGeometryId(IntPtr api, IntPtr geometry);

  [DllImport(WebIfc.DllName)]
  [DefaultDllImportSearchPaths(WebIfc.ImportSearchPath)]
  private static extern uint GetMaxId(IntPtr api, IntPtr geometry);

  [DllImport(WebIfc.DllName)]
  [DefaultDllImportSearchPaths(WebIfc.ImportSearchPath)]
  private static extern string GetLine(IntPtr api, IntPtr geometry, uint id);

  public int GetNumGeometries() => GetNumGeometries(api, model);

  public IfcGeometry? GetGeometry(uint id)
  {
    var geometry = GetGeometryFromId(api, model, id);
    return geometry == IntPtr.Zero ? null : new IfcGeometry(api, geometry);
  }

  public IEnumerable<IfcGeometry> GetGeometries()
  {
    var numGeometries = GetNumGeometries(api, model);
    for (int i = 0; i < numGeometries; ++i)
    {
      var gPtr = GetGeometryFromIndex(api, model, i);
      if (gPtr != IntPtr.Zero)
      {
        yield return new IfcGeometry(api, gPtr);
      }
    }
  }
  public uint GetMaxId() => GetMaxId(api, model);
  public string GetLine(uint id) => GetLine(api, model, id);
}
