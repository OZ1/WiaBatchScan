using System;
using System.Runtime.InteropServices;
using System.Runtime.InteropServices.ComTypes;

namespace WiaBatchScan
{
	[ComImport, Guid("c39d6942-2f4e-4d04-92fe-4ef4d3a1de5a")]
	[InterfaceType(ComInterfaceType.InterfaceIsIUnknown)]
	public interface IWiaTransfer
	{
		void Download(int Flags, IWiaTransferCallback pIWiaTransferCallback);
		void Upload(int Flags, IStream pSource, IWiaTransferCallback pIWiaTransferCallback);
		void Cancel();
		IEnumWIA_FORMAT_INFO EnumWIA_FORMAT_INFO();
	}
}
