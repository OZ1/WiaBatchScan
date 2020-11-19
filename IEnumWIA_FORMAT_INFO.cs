using System;
using System.Runtime.InteropServices;

namespace WiaBatchScan
{
	[ComImport, Guid("81BEFC5B-656D-44f1-B24C-D41D51B4DC81")]
	[InterfaceType(ComInterfaceType.InterfaceIsIUnknown)]
	public interface IEnumWIA_FORMAT_INFO
	{
		void Next(uint celt, out WIA_FORMAT_INFO rgelt, out uint pceltFetched);
		void Skip(uint celt);
		void Reset();
		IEnumWIA_FORMAT_INFO Clone();
		uint GetCount();
	}
}
