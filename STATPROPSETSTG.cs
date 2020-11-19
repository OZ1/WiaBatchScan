using System.Runtime.InteropServices;

using DWORD    = System.UInt32;
using FMTID    = System.Guid;
using CLSID    = System.Guid;
using FILETIME = System.Runtime.InteropServices.ComTypes.FILETIME;

namespace WiaBatchScan
{
	[StructLayout(LayoutKind.Sequential)]
	public struct STATPROPSETSTG
	{
		public FMTID fmtid;
		public CLSID clsid;
		public DWORD grfFlags;
		public FILETIME mtime;
		public FILETIME ctime;
		public FILETIME atime;
		public DWORD dwOSVersion;
	}
}
