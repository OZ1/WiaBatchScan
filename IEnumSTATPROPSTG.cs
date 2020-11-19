using System;
using System.Runtime.InteropServices;

namespace WiaBatchScan
{
	[ComImport, Guid("00000139-0000-0000-C000-000000000046")]
	[InterfaceType(ComInterfaceType.InterfaceIsIUnknown)]
	public interface IEnumSTATPROPSTG
	{
		void Next(uint celt, [MarshalAs(UnmanagedType.LPArray, SizeParamIndex = 0)][Out] STATPROPSTG[] rgelt, out uint pceltFetched);
		void Skip(uint celt);
		void Reset();
		IEnumSTATPROPSTG Clone(out IEnumSTATPROPSTG ppEnum);
	}
}
