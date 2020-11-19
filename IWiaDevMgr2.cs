using System;
using System.Runtime.InteropServices;

using static System.Runtime.InteropServices.UnmanagedType;

namespace WiaBatchScan
{
	[ComImport, Guid("B6C292BC-7C88-41ee-8B54-8EC92617E599")]
	public class WiaDevMgr2 { }

	[ComImport, Guid("79C07CF1-CBDD-41ee-8EC3-F00080CADA7A")]
	[InterfaceType(ComInterfaceType.InterfaceIsIUnknown)]
	[CoClass(typeof(WiaDevMgr2))]
	public interface IWiaDevMgr2
	{
		IEnumWIA_DEV_INFO EnumDeviceInfo(int Flags);
		IWiaItem2 CreateDevice(int Flags, string DeviceID);
		IWiaItem2 SelectDeviceDlg(IntPtr hwndParent, StiDeviceType DeviceType, int Flags, out string DeviceID);
		string SelectDeviceDlgID(IntPtr hwndParent, StiDeviceType DeviceType, int Flags);
		[return: MarshalAs(Interface)]
		object RegisterEventCallbackInterface(int Flags, string DeviceID, in Guid pEventGUID, IWiaEventCallback pIWiaEventCallback);
		void RegisterEventCallbackProgram(int Flags, string DeviceID, in Guid pEventGUID, string FullAppName, string CommandLineArg, string Name, string Description, string Icon);
		void RegisterEventCallbackCLSID(int Flags, string DeviceID, in Guid pEventGUID, in Guid pClsID, string Name, string Description, string Icon);
		IWiaItem2 GetImageDlg(int Flags, string DeviceID, IntPtr hwndParent, string FolderName, string Filename, out int plNumFiles, [Out, MarshalAs(LPArray, SizeParamIndex = 5)] out string[] ppbstrFilePaths);
	}
}
