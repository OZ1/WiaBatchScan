using System;
using System.Windows.Forms;

namespace WiaBatchScan
{
	static class App
	{
		[STAThread]
		static void Main()
		{
			#if DEBUG
			if (System.Diagnostics.Debugger.IsAttached)
				Environment.CurrentDirectory = @"C:\Users\OZone\AppData\Local\Temp";
			#endif
			Application.EnableVisualStyles();
			Application.SetCompatibleTextRenderingDefault(false);
			Application.Run(new MainForm());
		}
	}
}
