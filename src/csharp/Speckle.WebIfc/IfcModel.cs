using System.Runtime.InteropServices;

namespace Speckle.WebIfc;

public class IfcModel( IntPtr model)
{
  [DllImport(WebIfc.DllName)]
  [DefaultDllImportSearchPaths(WebIfc.ImportSearchPath)]
  private static extern IntPtr GetGeometryFromId( IntPtr model, uint id);

  [DllImport(WebIfc.DllName)]
  [DefaultDllImportSearchPaths(WebIfc.ImportSearchPath)]
  private static extern int GetNumGeometries( IntPtr model);

  [DllImport(WebIfc.DllName)]
  [DefaultDllImportSearchPaths(WebIfc.ImportSearchPath)]
  private static extern IntPtr GetGeometryFromIndex( IntPtr model, int index);


  [DllImport(WebIfc.DllName)]
  [DefaultDllImportSearchPaths(WebIfc.ImportSearchPath)]
  private static extern uint GetMaxId( IntPtr model);

  [DllImport(WebIfc.DllName)]
  [DefaultDllImportSearchPaths(WebIfc.ImportSearchPath)]
  private static extern IntPtr GetLineFromModel( IntPtr model, uint id);

  public int GetNumGeometries() => GetNumGeometries(model);

  public IfcGeometry? GetGeometry(uint id)
  {
    var geometry = GetGeometryFromId(model, id);
    return geometry == IntPtr.Zero ? null : new IfcGeometry(geometry);
  }

  public IEnumerable<IfcGeometry> GetGeometries()
  {
    var numGeometries = GetNumGeometries(model);
    for (int i = 0; i < numGeometries; ++i)
    {
      var gPtr = GetGeometryFromIndex(model, i);
      if (gPtr != IntPtr.Zero)
      {
        yield return new IfcGeometry( gPtr);
      }
    }
  }
  public uint GetMaxId() => GetMaxId(model);

  public IfcLine? GetLine(uint id)
  {
    var line = GetLineFromModel(model, id);
    return line == IntPtr.Zero ? null : new IfcLine(line);
  }
}
