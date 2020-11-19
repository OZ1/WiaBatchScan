namespace WiaBatchScan
{
	public enum WIA_TRANSFER_MSG
	{
		STATUS          = 0x00001,
		END_OF_STREAM   = 0x00002,
		END_OF_TRANSFER = 0x00003,
		DEVICE_STATUS   = 0x00005,
		NEW_PAGE        = 0x00006,
	}
}
