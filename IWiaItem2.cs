using System;
using System.Runtime.InteropServices;

namespace WiaBatchScan
{
	[ComImport, Guid("6CBA0075-1287-407d-9B77-CF0E030435CC")]
	[InterfaceType(ComInterfaceType.InterfaceIsIUnknown)]
	public interface IWiaItem2
	{
		IWiaItem2 CreateChildItem(int lItemFlags, int lCreationFlags, string bstrItemName);
		void DeleteItem(int lFlags);
		IEnumWiaItem2 EnumChildItems(in Guid pCategoryGUID);
		IWiaItem2 FindItemByName(int lFlags, string bstrFullItemName);
		Guid GetItemCategory();
		int GetItemType();
		IWiaItem2 DeviceDlg(int lFlags, IntPtr hwndParent, string bstrFolderName, string bstrFilename, ref int plNumFiles, [MarshalAs(UnmanagedType.BStr, SizeParamIndex = 4)] string[] ppbstrFilePaths);
		void DeviceCommand(int lFlags, in Guid pCmdGUID, ref IWiaItem2 ppIWiaItem2);
		IEnumWIA_DEV_CAPS EnumDeviceCapabilities(int lFlags);
		bool CheckExtension(int lFlags, string bstrName, in Guid riidExtensionInterface);
		[return: MarshalAs(UnmanagedType.Interface)]
		object GetExtension(int lFlags, string bstrName, in Guid riidExtensionInterface);
		IWiaItem2 GetParentItem();
		IWiaItem2 GetRootItem();
		IWiaPreview GetPreviewComponent(int lFlags);
		IEnumWIA_DEV_CAPS EnumRegisterEventInfo(int lFlags, in Guid pEventGUID);
		void Diagnostic(uint ulSize, [MarshalAs(UnmanagedType.LPArray, SizeParamIndex = 0)] byte[] pBuffer);//BYTE*
	}
}
