using System;
using System.Runtime.InteropServices;

namespace WiaBatchScan
{
	[ComImport, Guid("59970AF4-CD0D-44d9-AB24-52295630E582")]
	[InterfaceType(ComInterfaceType.InterfaceIsIUnknown)]
	public interface IEnumWiaItem2
	{
		void Next(uint celt, out IWiaItem2 ppIWiaItem2, out uint pceltFetched);
		void Skip(uint celt);
		void Reset();
		IEnumWiaItem2 Clone();
		uint GetCount();
	}
}
