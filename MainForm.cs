using System;
using System.ComponentModel;
using System.Diagnostics;
using System.Drawing;
using System.IO;
using System.Runtime.InteropServices;
using System.Windows.Forms;

namespace WiaBatchScan
{
	public partial class MainForm : Form, IWiaTransferCallback
	{
		readonly IWiaDevMgr2 pWiaDevMgr2 = new IWiaDevMgr2();

		IWiaTransfer pWiaTransfer;

		int FileIncrement = +1;
		int FileNumber = 1;
		int LastFileNumber = 1;
		int NextFileNumber = 2;
		string CurrentFileName => $"{FileNumber}{Extension}";
		string LastFileName => $"{LastFileNumber}{Extension}";
		string Extension;
		TimeSpan ScanSpeed;
		bool IsIdle = true;
		bool LastPage;
		bool SkipPage;
		bool ReScanPage;

		public MainForm()
		{
			InitializeComponent();
		}

		protected override void OnClosing(CancelEventArgs e)
		{
			StopButton_Click(this, e);
			base.OnClosing(e);
		}

		protected override bool ProcessCmdKey(ref Message msg, Keys keyData)
		{
			switch (keyData)
			{
			case Keys.Delete:
				if (txFileNumber.Focused) break;
				else       if (deleteButton.Enabled) DeleteButton_Click(this, EventArgs.Empty); return true;
			case Keys.F4 : if (startButton .Enabled) StartButton_Click (this, EventArgs.Empty); return true;
			case Keys.F5 : if (rescanButton.Enabled) ResumeButton_Click(this, EventArgs.Empty); return true;
			case Keys.F8 : if (singleButton.Enabled) SingleButton_Click(this, EventArgs.Empty); return true;
			case Keys.F11: skipCheckBox.Checked ^= true; return true;
			case Keys.F9 : lastCheckBox.Checked ^= true; return true;
			case Keys.F2 : txFileNumber.SelectAll(); txFileNumber.Focus(); return true;
			case Keys.F12: StopButton_Click(this, EventArgs.Empty); return true;
			}
			return base.ProcessCmdKey(ref msg, keyData);
		}

		private void LastCheckBox_CheckedChanged(object sender, EventArgs e)
		{
			LastPage = lastCheckBox.Checked;
		}

		private void SkipCheckBox_CheckedChanged(object sender, EventArgs e)
		{
			SkipPage = lastCheckBox.Checked;
		}

		private void IncDec_CheckedChanged(object sender, EventArgs e)
		{
			FileIncrement = rbInc1.Checked ? +1 :
			                rbDec1.Checked ? -1 :
			                rbInc2.Checked ? +2 :
			                rbDec2.Checked ? -2 :
				throw new InvalidOperationException();
		}

		private void FileNumber_TextChanged(object sender, EventArgs e)
		{
			var valid = int.TryParse(txFileNumber.Text, out NextFileNumber);
			txFileNumber.ForeColor = valid ? SystemColors.WindowText : Color.Red;
			EnableUI(IsIdle, valid);
		}

		unsafe private void StartButton_Click(object sender, EventArgs e)
		{
			if (Preview())
				Scan();
		}

		private void ResumeButton_Click(object sender, EventArgs e)
		{
			if (pWiaTransfer is null)
				{ if (Preview()) return; }
			else SetFileNumberToNextFileNumber();
			Scan();
		}

		private void SingleButton_Click(object sender, EventArgs e)
		{
			if (pWiaTransfer is null)
				{ if (Preview()) return; }
			else SetFileNumberToNextFileNumber();
			Scan(singlePage: true);
		}

		private void StopButton_Click(object sender, EventArgs e)
		{
			worker.CancelAsync();
			pWiaTransfer?.Cancel();
		}

		private void ReScanButton_Click(object sender, EventArgs e)
		{
			ReScanPage = true;
			DeleteButton_Click(sender, e);
			pWiaTransfer?.Cancel();
		}

		private void DeleteButton_Click(object sender, EventArgs e)
		{
			File.Delete(LastFileName);
			NextFileNumber = LastFileNumber;
			txFileNumber.Text = NextFileNumber.ToString();
			if (BackgroundImage != null)
			{
				BackgroundImage = null;
				deleteButton.Enabled = false;
			}
		}

		private void Worker_ProgressChanged(object sender, ProgressChangedEventArgs e)
		{
			Text = $"Сканируется {CurrentFileName} — {e.ProgressPercentage}%";
		}

		private void Worker_RunWorkerCompleted(object sender, RunWorkerCompletedEventArgs e)
		{
			EnableUI(true);
		}

		private void Worker_DoWork(object sender, DoWorkEventArgs e)
		{
			var t0 = DateTime.Now;
			while (!worker.CancellationPending)
			{
				pWiaTransfer.Download(0, this);
				var t = DateTime.Now;
				if (worker.CancellationPending) break;
				ScanSpeed = t - t0; t0 = t;

				if (LastPage) return;
			}
			if (worker.CancellationPending)
			{
				e.Cancel = true;
				File.Delete(CurrentFileName);
			}
		}

		void IWiaTransferCallback.TransferCallback(int Flags, in WiaTransferParams pWiaTransferParams)
		{
			if (worker.CancellationPending) return;

			switch (pWiaTransferParams.Message)
			{
			case WIA_TRANSFER_MSG.STATUS:
				if (worker.WorkerReportsProgress)
					worker.ReportProgress(pWiaTransferParams.PercentComplete);
				break;
			case WIA_TRANSFER_MSG.DEVICE_STATUS:
				Trace.TraceError($"{CurrentFileName} WIA_TRANSFER_MSG_DEVICE_STATUS 0x{pWiaTransferParams.hrErrorStatus:X}");
				break;
			case WIA_TRANSFER_MSG.NEW_PAGE:
				Trace.WriteLine($"{CurrentFileName} WIA_TRANSFER_MSG_NEW_PAGE");
				break;
			case WIA_TRANSFER_MSG.END_OF_STREAM:
				Trace.WriteLine($"{CurrentFileName} WIA_TRANSFER_MSG_END_OF_STREAM");
				break;
			case WIA_TRANSFER_MSG.END_OF_TRANSFER:
				Trace.WriteLine($"{CurrentFileName} WIA_TRANSFER_MSG_END_OF_TRANSFER");
				if (SkipPage || ReScanPage)
					File.Delete(CurrentFileName);
				else
					OnEndTransfer();
				ReScanPage = false;
				break;
			}
		}

		IntPtr IWiaTransferCallback.GetNextStream(int Flags, string ItemName, string FullItemName)
		{
			Trace.WriteLine($"GetNextStream(ItemName= {ItemName}, FullItemName= {FullItemName})= {CurrentFileName}");
			return SHCreateStreamOnFile(CurrentFileName, STGM.CREATE | STGM.READWRITE | STGM.SHARE_DENY_NONE);
		}

		/// <remarks>измѣни NextFileNumber, чтобы получить CurrentFileName</remarks>
		void SetFileNumberToNextFileNumber(/*in out NextFileNumber*/)
		{
			var currentFileNumber = FileNumber;
			do {
				FileNumber = NextFileNumber;
				NextFileNumber += FileIncrement;
			} while (File.Exists(CurrentFileName));
			if (currentFileNumber != FileNumber)
				TryBeginInvoke(SetFileNumberToNextFileNumberUI);
		}

		void SetFileNumberToNextFileNumberUI()
		{
			txFileNumber.Text = FileNumber.ToString();
			Text = $"Будет сканироваться {CurrentFileName}";
		}

		void OnEndTransfer(/*out LastFileNumber, out FileNumber, out NextFileNumber*/)
		{
			LastFileNumber = FileNumber;
			SetFileNumberToNextFileNumber();
			TryBeginInvoke(OnEndTransferUI);
		}

		void OnEndTransferUI()
		{
			var fileName = LastFileName;
			var bytes = File.ReadAllBytes(fileName);
			var stream = new MemoryStream(bytes);
			BackgroundImage = Image.FromStream(stream);
			lbLast.Text = fileName;
			deleteButton.Enabled = true;
			if (ScanSpeed != TimeSpan.Zero)
				lbSpeed.Text = ScanSpeed.TotalSeconds.ToString("N2") + " с";
		}

		bool Preview()
		{
			int numFiles;
			string[] filePaths;
			try { pWiaTransfer = (IWiaTransfer)pWiaDevMgr2.GetImageDlg(0, DeviceID: null, Handle, ".", "образец", out numFiles, out filePaths); }
			catch (COMException ex) { MessageBox.Show(this, ex.Message, "Ошибка сканирования", MessageBoxButtons.OK, MessageBoxIcon.Error); return false; }
			if (numFiles == 0) return false;

			Extension = Path.GetExtension(filePaths[0]);
			for (var i = 0; i < numFiles; i++)
			{
				SetFileNumberToNextFileNumber();
				File.Move(filePaths[i], CurrentFileName);
			}
			OnEndTransfer();

			return !LastPage;
		}

		void Scan(bool singlePage = false)
		{
			EnableUI(false);
			LastPage = singlePage;
			lastCheckBox.Checked = singlePage;
			worker.RunWorkerAsync();
		}

		void EnableUI(bool isIdle, bool isValid = true)
		{
			if (IsIdle != isIdle)
			{
				IsIdle  = isIdle;
				rbInc1.Enabled = isIdle;
				rbDec1.Enabled = isIdle;
				rbInc2.Enabled = isIdle;
				rbDec2.Enabled = isIdle;
				rescanButton.Enabled  = !isIdle;
				txFileNumber.ReadOnly = !isIdle;
			}
			startButton .Enabled = isIdle && isValid;
			resumeButton.Enabled = isIdle && isValid;
			singleButton.Enabled = isIdle && isValid;
		}

		void TryBeginInvoke(Action f)
		{
			if (InvokeRequired) BeginInvoke(f); else f();
		}

		[DllImport("ShlWApi", CharSet = CharSet.Unicode, PreserveSig = false)]
		extern static IntPtr SHCreateStreamOnFile(string pszFile, STGM grfMode);
	}
}
