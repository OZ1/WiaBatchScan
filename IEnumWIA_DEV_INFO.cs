using System;
using System.Runtime.InteropServices;

namespace WiaBatchScan
{
	[ComImport, Guid("5e38b83c-8cf1-11d1-bf92-0060081ed811")]
	[InterfaceType(ComInterfaceType.InterfaceIsIUnknown)]
	public interface IEnumWIA_DEV_INFO
	{
		void Next(uint celt, out IWiaPropertyStorage rgelt, out uint celtFetched);
		void Skip(uint celt);
		void Reset();
		IEnumWIA_DEV_INFO Clone();
		uint GetCount();
	}
}
