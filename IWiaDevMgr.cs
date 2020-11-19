using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Runtime.InteropServices;

namespace WiaBatchScan
{
	[ComImport, Guid("5eb2502a-8cf1-11d1-bf92-0060081ed811")]
	[InterfaceType(ComInterfaceType.InterfaceIsIUnknown)]
	public interface IWiaDevMgr
	{
		[return: MarshalAs(UnmanagedType.Interface)]
		object EnumDeviceInfo(
			[In] int lFlag);

		void CreateDevice(
			[In] int lFlags,
			[In, MarshalAs(UnmanagedType.BStr)] string strDeviceId,
			[Out, MarshalAs(UnmanagedType.Interface)] out IWiaItem iWiaItemRoot);

		[return: MarshalAs(UnmanagedType.Interface)]
		object SelectDeviceDlg(
			[In] IntPtr hwndParent,
			[In] int lDeviceType,
			[In] int lFlags,
			[In, Out, MarshalAs(UnmanagedType.BStr)] ref string strDeviceID,
			[Out, MarshalAs(UnmanagedType.Interface)] out IWiaItem iWiaItemRoot2);
		[return: MarshalAs(UnmanagedType.BStr)]
		string SelectDeviceDlgID(
			[In] IntPtr hwndParent,
			[In] int lDeviceType,
			[In] int lFlags,
			[Out, MarshalAs(UnmanagedType.BStr)] string strDeviceId);

		void GetImageDlg(
			[In] int lFlags,
			[In, MarshalAs(UnmanagedType.BStr)] string strDeviceId,
			[In] IntPtr hwndParent,
			[In, MarshalAs(UnmanagedType.BStr)] string strFolderName,
			[In, MarshalAs(UnmanagedType.BStr)] string strFileName,
			[In] int lNumFiles,
			[In, MarshalAs(UnmanagedType.BStr)] string strFName,
			[Out, MarshalAs(UnmanagedType.Interface)] out object iItemRoot);

		void RegisterEventCallbackProgram(
			[In] int lFlags,
			[In, MarshalAs(UnmanagedType.BStr)] string strDeviceId,
			[In] in Guid eventGuid,
			[In, MarshalAs(UnmanagedType.BStr)] string strFullAppName,
			[In, MarshalAs(UnmanagedType.BStr)] string strCommandline,
			[In, MarshalAs(UnmanagedType.BStr)] string strName,
			[In, MarshalAs(UnmanagedType.BStr)] string strDescription,
			[In, MarshalAs(UnmanagedType.BStr)] string strIcon);

		void RegisterEventCallbackInterface(
			[In] int lFlags,
			[In, MarshalAs(UnmanagedType.BStr)] string strDeviceId,
			[In] in Guid eventGuid,
			[In, MarshalAs(UnmanagedType.Interface)] object wiaEventCallbackInterface,
			[Out, MarshalAs(UnmanagedType.IUnknown)] out object eventObjectIUnknown);

		void RegisterEventCallbackCLSID(
			[In] int lFlags,
			[In, MarshalAs(UnmanagedType.BStr)] string strDeviceId,
			[In] in Guid eventGuid,
			[In] in Guid clsid,
			[In, MarshalAs(UnmanagedType.BStr)] string strName,
			[In, MarshalAs(UnmanagedType.BStr)] string strDescription,
			[In, MarshalAs(UnmanagedType.BStr)] string strIcon);

	}
}
