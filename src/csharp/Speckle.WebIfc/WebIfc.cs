using System.Runtime.InteropServices;

namespace Speckle.WebIfc;

public static class WebIfc
{
  // NOTE: make sure the DLL is in the same directory as the built DLLs or Executable.
  internal const string DllName = "libweb-ifc.so";

  internal const DllImportSearchPath ImportSearchPath = DllImportSearchPath.AssemblyDirectory;
  internal const CharSet Set = CharSet.Auto;
}
