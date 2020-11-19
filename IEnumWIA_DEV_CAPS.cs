using System;
using System.Runtime.InteropServices;

namespace WiaBatchScan
{
	[ComImport, Guid("1fcc4287-aca6-11d2-a093-00c04f72dc3c")]
	[InterfaceType(ComInterfaceType.InterfaceIsIUnknown)]
	public interface IEnumWIA_DEV_CAPS
	{
		void Next(uint celt, out WIA_DEV_CAP rgelt, out uint pceltFetched);
		void Skip(uint celt);
		void Reset();
		IEnumWIA_DEV_CAPS Clone();
		uint GetCount();
	}
}
