using System.Runtime.InteropServices;

namespace Speckle.WebIfc;

public class IfcLine(IntPtr line)
{

  [DllImport(WebIfc.DllName)]
  [DefaultDllImportSearchPaths(WebIfc.ImportSearchPath)]
  private static extern uint GetLineId(IntPtr line);

  [DllImport(WebIfc.DllName)]
  [DefaultDllImportSearchPaths(WebIfc.ImportSearchPath)]
  private static extern uint GetLineType(IntPtr line);

  [DllImport(WebIfc.DllName)]
  [DefaultDllImportSearchPaths(WebIfc.ImportSearchPath)]
  private static extern string GetLineArguments(IntPtr line);

  public uint Id => GetLineId(line);
  public IfcSchemaType Type => (IfcSchemaType)GetLineType(line);

  public string Arguments() => GetLineArguments(line);
}
