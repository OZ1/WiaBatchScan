using System;
using System.Runtime.InteropServices;
using System.Runtime.InteropServices.ComTypes;

namespace WiaBatchScan
{
	[StructLayout(LayoutKind.Sequential)]
	public struct WIA_FORMAT_INFO
	{
		public Guid guidFormatID;
		public TYMED lTymed;

		public override string ToString() => $"{lTymed} {guidFormatID.ToKnown()}";
	}
}
