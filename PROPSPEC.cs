using System;
using System.Runtime.InteropServices;

namespace WiaBatchScan
{
	[StructLayout(LayoutKind.Sequential)]
	public struct PROPSPEC : IDisposable
	{
		public const int PRSPEC_LPWSTR = 0;
		public const int PRSPEC_PROPID = 1;

		uint   Kind;
		IntPtr Value;

		public string Name
		{
			get => Marshal.PtrToStringUni(Value);
			set { Value = Marshal.StringToCoTaskMemUni(value); Kind = PRSPEC_PROPID; }
		}

		public uint PropID
		{
			get => (uint)Value;
			set { Value = (IntPtr)value; Kind = PRSPEC_LPWSTR; }
		}

		public PROPSPEC(uint value)
		{
			Kind = PRSPEC_PROPID;
			Value = (IntPtr)value;
		}

		public PROPSPEC(string value)
		{
			Kind = PRSPEC_LPWSTR;
			Value = Marshal.StringToCoTaskMemUni(value);
		}

		public void Dispose()
		{
			if (Kind == PRSPEC_LPWSTR && Value != IntPtr.Zero)
			{
				Marshal.FreeCoTaskMem(Value);
				Value = IntPtr.Zero;
			}
		}

		public override string ToString() => Kind == PRSPEC_LPWSTR ? Name : PropID.ToString();
	}
}
