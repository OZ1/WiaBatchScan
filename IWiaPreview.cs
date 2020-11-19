using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Runtime.InteropServices;

namespace WiaBatchScan
{
	[ComImport, Guid("95C2B4FD-33F2-4d86-AD40-9431F0DF08F7")]
	[InterfaceType(ComInterfaceType.InterfaceIsIUnknown)]
	public interface IWiaPreview
	{
		void GetNewPreview(int lFlags, IWiaItem2 pWiaItem2, IWiaTransferCallback pWiaTransferCallback);
		void UpdatePreview(int lFlags, IWiaItem2 pChildWiaItem2, IWiaTransferCallback pWiaTransferCallback);
		void DetectRegions(int lFlags);
		void Clear();
	}
}
