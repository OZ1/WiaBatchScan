using System;
using System.Runtime.InteropServices;
using System.Runtime.InteropServices.ComTypes;

using static System.Runtime.InteropServices.UnmanagedType;

using FILETIME = System.Runtime.InteropServices.ComTypes.FILETIME;

namespace WiaBatchScan
{
	[ComImport, Guid("98B5E8A0-29CC-491a-AAC0-E6DB4FDCCEB6")]
	[InterfaceType(ComInterfaceType.InterfaceIsIUnknown)]
	public interface IWiaPropertyStorage
	{
		void ReadMultiple(
			[In] int cpspec,
			[In, MarshalAs(LPArray, SizeParamIndex = 0)] PROPSPEC[] rgpspec,
			[Out, MarshalAs(LPArray, SizeParamIndex = 0)] PROPVARIANT[] rgpropvar);

		void WriteMultiple(
			[In] int cpspec,
			[In, MarshalAs(LPArray, SizeParamIndex = 0)] PROPSPEC[] rgpspec,
			[In, MarshalAs(LPArray, SizeParamIndex = 0)] PROPVARIANT[] rgpropvar,
			[In] uint propidNameFirst);

		void DeleteMultiple(
			[In] int cpspec,
			[In, MarshalAs(LPArray, SizeParamIndex = 0)] PROPSPEC[] rgpspec);

		void ReadPropertyNames(
			[In] int cpropid,
			[In, MarshalAs(LPArray, SizeParamIndex = 0)] uint[] rgpropid,
			[Out, MarshalAs(LPArray, SizeParamIndex = 0)] string[] rglpwstrName);

		void WritePropertyNames(
			[In] int cpropid,
			[In, MarshalAs(LPArray, SizeParamIndex = 0)] uint[] rgpropid,
			[In, MarshalAs(LPArray, SizeParamIndex = 0)] string[] rglpwstrName);

		void DeletePropertyNames(
			[In] int cpropid,
			[In, MarshalAs(LPArray, SizeParamIndex = 0)] uint[] rgpropid);

		void Commit(int grfCommitFlags);

		void Revert();

		IEnumSTATPROPSTG Enum();

		void SetTimes(in FILETIME pctime, in FILETIME patime, in FILETIME pmtime);

		void SetClass(in Guid clsid);

		[return: MarshalAs(LPStruct)]
		STATPROPSETSTG Stat();

		void GetPropertyAttributes(
			[In] int cpspec,
			[In, MarshalAs(LPArray, SizeParamIndex = 0)] PROPSPEC[] rgpspec,
			[Out, MarshalAs(LPArray, SizeParamIndex = 0)] uint[] rgflags,
			[Out, MarshalAs(LPArray, SizeParamIndex = 0)] PROPVARIANT[] rgpropvar);

		int GetCount();

		IStream GetPropertyStream([Out] out Guid guidCompatibilityId);

		void SetPropertyStream([In] in Guid guidCompatibilityId, IStream iStream);
	}
}
