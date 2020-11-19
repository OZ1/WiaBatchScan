using System;
using System.Diagnostics;
using System.Runtime.InteropServices;

using static System.Runtime.InteropServices.UnmanagedType;

namespace WiaBatchScan
{
	[DebuggerDisplay("{Name}")]
	[StructLayout(LayoutKind.Sequential)]
	public struct WIA_DEV_CAP
	{
		public Guid Guid;
		public uint Flags;
		[MarshalAs(BStr)] public string Name;
		[MarshalAs(BStr)] public string Description;
		[MarshalAs(BStr)] public string Icon;
		[MarshalAs(BStr)] public string Commandline;
	}
}
