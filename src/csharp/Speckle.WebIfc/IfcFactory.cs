using System.Runtime.InteropServices;
using Speckle.InterfaceGenerator;

namespace Speckle.WebIfc;

[GenerateAutoInterface]
public class IfcFactory : IIfcFactory
{
  [DllImport(WebIfc.DllName)]
  [DefaultDllImportSearchPaths(WebIfc.ImportSearchPath)]
  private static extern IntPtr InitializeApi();

  [DllImport(WebIfc.DllName, CharSet = WebIfc.Set)]
  [DefaultDllImportSearchPaths(WebIfc.ImportSearchPath)]
  private static extern IntPtr LoadModel(IntPtr api, string fileName);

  [DllImport(WebIfc.DllName, CharSet = WebIfc.Set)]
  [DefaultDllImportSearchPaths(WebIfc.ImportSearchPath)]
  private static extern string GetVersion();

  //probably never disposing this
  private static readonly IntPtr _ptr = InitializeApi();

  public IfcModel Open(string fullPath) => new(LoadModel(_ptr, fullPath));

  public string Version => GetVersion();
}
