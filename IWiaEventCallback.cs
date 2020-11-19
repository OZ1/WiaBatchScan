using System;
using System.Runtime.InteropServices;

namespace WiaBatchScan
{
	[ComImport, Guid("ae6287b0-0084-11d2-973b-00a0c9068f2e")]
	[InterfaceType(ComInterfaceType.InterfaceIsIUnknown)]
	public interface IWiaEventCallback
	{
		void ImageEventCallback(in Guid EventGUID, string EventDescription, string DeviceID, string DeviceDescription, uint DeviceType, string FullItemName, ref uint EventType, uint Reserved = 0);
	}
}
