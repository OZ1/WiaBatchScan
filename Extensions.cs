using System;
using System.Collections.Generic;

namespace WiaBatchScan
{
	public static class Extensions
	{
		public static IEnumerable<IWiaItem2> EnumByCategory(this IWiaItem2 pRoot, Guid Category)
		{
			var pEnum = pRoot.EnumChildItems(Category);
			pEnum.Reset();
			while (true)
			{
				pEnum.Next(1, out var pWiaItem2, out var fetched);
				if (fetched == 0) break;
				yield return pWiaItem2;
			}
		}
	}
}
