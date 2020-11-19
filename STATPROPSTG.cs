using System.Diagnostics;
using System.Runtime.InteropServices;

namespace WiaBatchScan
{
	[DebuggerDisplay("{lpwstrName}")]
	[StructLayout(LayoutKind.Sequential)]
	public struct STATPROPSTG
	{
		[MarshalAs(UnmanagedType.LPWStr)]
		public string lpwstrName;
		public uint propid;
		public ushort vt;
	}
}
