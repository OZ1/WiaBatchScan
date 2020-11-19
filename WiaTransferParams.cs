using System.Diagnostics;
using System.Runtime.InteropServices;

namespace WiaBatchScan
{
	[DebuggerDisplay("{PercentComplete}% {Message}")]
	[StructLayout(LayoutKind.Sequential)]
	public struct WiaTransferParams
	{
		public WIA_TRANSFER_MSG Message;
		public int PercentComplete;
		public ulong TransferredBytes;
		public int hrErrorStatus;
	}
}
