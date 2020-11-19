using System;
using System.Diagnostics;
using System.Runtime.InteropServices;

namespace WiaBatchScan
{
	// Credit: http://blogs.msdn.com/b/adamroot/archive/2008/04/11/interop-with-propvariants-in-net.aspx
	/// <summary>
	/// Represents the OLE struct PROPVARIANT.
	/// </summary>
	/// <remarks>
	/// Must call Clear when finished to avoid memory leaks. If you get the value of
	/// a VT_UNKNOWN prop, an implicit AddRef is called, thus your reference will
	/// be active even after the PropVariant struct is cleared.
	/// </remarks>
	[DebuggerDisplay("{Type} {Value}")]
	[StructLayout(LayoutKind.Sequential)]
    public unsafe struct PROPVARIANT : IDisposable
    {
        // The layout of these elements needs to be maintained.
        //
        // NOTE: We could use LayoutKind.Explicit, but we want
        //       to maintain that the IntPtr may be 8 bytes on
        //       64-bit architectures, so we'll let the CLR keep
        //       us aligned.
        //
        // NOTE: In order to allow x64 compat, we need to allow for
        //       expansion of the IntPtr. However, the BLOB struct
        //       uses a 4-byte int, followed by an IntPtr, so
        //       although the p field catches most pointer values,
        //       we need an additional 4-bytes to get the BLOB
        //       pointer. The p2 field provides this, as well as
        //       the last 4-bytes of an 8-byte value on 32-bit
        //       architectures.

        // This is actually a VarEnum value, but the VarEnum type
        // shifts the layout of the struct by 4 bytes instead of the
        // expected 2.
        ushort vt;
        ushort wReserved1;
        ushort wReserved2;
        ushort wReserved3;
        public IntPtr cVal;
        public IntPtr pVal;

		private byte[] GetBlobData()
        {
			var blobData = new byte[(int)cVal];
            try { Marshal.Copy(pVal, blobData, 0, blobData.Length); }
            catch { return null; }
            return blobData;
        }

		/// <summary>
		/// Called to clear the PropVariant's referenced and local memory.
		/// </summary>
		/// <remarks>
		/// You must call Clear to avoid memory leaks.
		/// </remarks>
		public void Clear() => PropVariantClear(ref this);

		/// <summary>
		/// Gets the variant type.
		/// </summary>
		public VarEnum Type => (VarEnum)vt;

		/// <summary>
		/// Gets the variant value.
		/// </summary>
		public object Value
        {
            get
            {
				fixed (void* ptr = &cVal)
					return Type switch
					{
						VarEnum.VT_I1       => *(SByte *)ptr,
						VarEnum.VT_UI1      => *( Byte *)ptr,
						VarEnum.VT_I2       => *( Int16*)ptr,
						VarEnum.VT_UI2      => *(UInt16*)ptr,
						VarEnum.VT_I4       => *( Int32*)ptr,
						VarEnum.VT_INT      => *( Int32*)ptr,
						VarEnum.VT_UI4      => *(UInt32*)ptr,
						VarEnum.VT_UINT     => *(UInt32*)ptr,
						VarEnum.VT_I8       => *( Int64*)ptr,
						VarEnum.VT_UI8      => *(UInt64*)ptr,
						VarEnum.VT_R4       => *(Single*)ptr,
						VarEnum.VT_R8       => *(Double*)ptr,
						VarEnum.VT_CLSID    => *(Guid  *)ptr,
						VarEnum.VT_ERROR    => *(Int32 *)ptr,
						VarEnum.VT_BOOL     => *(short *)ptr != 0,
						VarEnum.VT_FILETIME => *(long  *)ptr > 0 ? (object)DateTime.FromFileTime(*(long*)ptr) : null,
						VarEnum.VT_CY       =>  decimal.FromOACurrency(*(long*)ptr),
						VarEnum.VT_DATE     => DateTime.FromOADate(*(double*)ptr),
						VarEnum.VT_BSTR     => Marshal.PtrToStringBSTR     (cVal),
						VarEnum.VT_LPSTR    => Marshal.PtrToStringAnsi     (cVal),
						VarEnum.VT_LPWSTR   => Marshal.PtrToStringUni      (cVal),
						VarEnum.VT_UNKNOWN  => Marshal.GetObjectForIUnknown(cVal),
						VarEnum.VT_DISPATCH => cVal,
						_ => null //    throw new NotSupportedException("The type of this variable is not support ('" + vt.ToString() + "')");
					};
            }
        }

		void IDisposable.Dispose() => Clear();

		[DllImport("Ole32", PreserveSig = false)]
        extern static void PropVariantClear(ref PROPVARIANT pvar);
	}
}
