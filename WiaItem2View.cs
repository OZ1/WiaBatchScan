using System.Collections.Generic;

namespace WiaBatchScan
{
	struct WiaItem2View
	{
		readonly IWiaItem2 m_p;

		public WiaItem2View(IWiaItem2 p)
		{
			m_p = p;
		}

		public WiaPropertyStorageView PropertyStorage => new WiaPropertyStorageView(m_p as IWiaPropertyStorage);

		public IEnumerable<WIA_DEV_CAP> DeviceCapabilities
		{
			get
			{
				var pEnum = m_p.EnumDeviceCapabilities(0);
				pEnum.Reset();
				while (true)
				{
					pEnum.Next(1, out var cap, out var fetched);
					if (fetched == 0) break;
					yield return cap;
				}
			}
		}

		public IEnumerable<WIA_FORMAT_INFO> Formats
		{
			get
			{
				if (m_p is IWiaTransfer pTransfer)
				{
					var pEnum = pTransfer.EnumWIA_FORMAT_INFO();
					pEnum.Reset();
					while (true)
					{
						pEnum.Next(1, out var format, out var fetched);
						if (fetched == 0) break;
						yield return format;
					}
				}
			}
		}
	}
}
