using System;
using System.Runtime.InteropServices;
using System.Runtime.InteropServices.ComTypes;

namespace WiaBatchScan
{
	[ComImport, Guid("27d4eaaf-28a6-4ca5-9aab-e678168b9527")]
	[InterfaceType(ComInterfaceType.InterfaceIsIUnknown)]
	public interface IWiaTransferCallback
	{
		void TransferCallback(int Flags, in WiaTransferParams pWiaTransferParams);
		IntPtr GetNextStream(int Flags, string ItemName, string FullItemName);
	}
}
