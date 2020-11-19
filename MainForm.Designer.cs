
namespace WiaBatchScan
{
	partial class MainForm
	{
		/// <summary>
		/// Обязательная переменная конструктора.
		/// </summary>
		private System.ComponentModel.IContainer components = null;

		/// <summary>
		/// Освободить все используемые ресурсы.
		/// </summary>
		/// <param name="disposing">истинно, если управляемый ресурс должен быть удален; иначе ложно.</param>
		protected override void Dispose(bool disposing)
		{
			if (disposing && (components != null))
			{
				components.Dispose();
			}
			base.Dispose(disposing);
		}

		#region Код, автоматически созданный конструктором форм Windows

		/// <summary>
		/// Требуемый метод для поддержки конструктора — не изменяйте 
		/// содержимое этого метода с помощью редактора кода.
		/// </summary>
		private void InitializeComponent()
		{
			this.components = new System.ComponentModel.Container();
			System.Windows.Forms.Button stopButton;
			System.Windows.Forms.ToolTip toolTip;
			System.ComponentModel.ComponentResourceManager resources = new System.ComponentModel.ComponentResourceManager(typeof(MainForm));
			this.startButton = new System.Windows.Forms.Button();
			this.txFileNumber = new System.Windows.Forms.TextBox();
			this.rbInc1 = new System.Windows.Forms.RadioButton();
			this.rbDec1 = new System.Windows.Forms.RadioButton();
			this.rbInc2 = new System.Windows.Forms.RadioButton();
			this.rbDec2 = new System.Windows.Forms.RadioButton();
			this.lbLast = new System.Windows.Forms.Label();
			this.resumeButton = new System.Windows.Forms.Button();
			this.deleteButton = new System.Windows.Forms.Button();
			this.lbSpeed = new System.Windows.Forms.Label();
			this.singleButton = new System.Windows.Forms.Button();
			this.lastCheckBox = new System.Windows.Forms.CheckBox();
			this.skipCheckBox = new System.Windows.Forms.CheckBox();
			this.rescanButton = new System.Windows.Forms.Button();
			this.worker = new System.ComponentModel.BackgroundWorker();
			stopButton = new System.Windows.Forms.Button();
			toolTip = new System.Windows.Forms.ToolTip(this.components);
			this.SuspendLayout();
			// 
			// stopButton
			// 
			stopButton.Anchor = ((System.Windows.Forms.AnchorStyles)((System.Windows.Forms.AnchorStyles.Top | System.Windows.Forms.AnchorStyles.Right)));
			stopButton.DialogResult = System.Windows.Forms.DialogResult.Cancel;
			stopButton.Location = new System.Drawing.Point(734, 10);
			stopButton.Name = "stopButton";
			stopButton.Size = new System.Drawing.Size(100, 23);
			stopButton.TabIndex = 10;
			stopButton.Text = "&Стоп (F12)";
			toolTip.SetToolTip(stopButton, "остановить сканирование");
			stopButton.UseVisualStyleBackColor = true;
			stopButton.Click += new System.EventHandler(this.StopButton_Click);
			// 
			// startButton
			// 
			this.startButton.DialogResult = System.Windows.Forms.DialogResult.OK;
			this.startButton.Location = new System.Drawing.Point(230, 10);
			this.startButton.Name = "startButton";
			this.startButton.Size = new System.Drawing.Size(100, 23);
			this.startButton.TabIndex = 5;
			this.startButton.Text = "&Начать (F4)";
			toolTip.SetToolTip(this.startButton, "настройка сканирования и запуск");
			this.startButton.UseVisualStyleBackColor = true;
			this.startButton.Click += new System.EventHandler(this.StartButton_Click);
			// 
			// txFileNumber
			// 
			this.txFileNumber.Location = new System.Drawing.Point(12, 12);
			this.txFileNumber.Name = "txFileNumber";
			this.txFileNumber.Size = new System.Drawing.Size(40, 20);
			this.txFileNumber.TabIndex = 0;
			this.txFileNumber.Text = "1";
			this.txFileNumber.TextAlign = System.Windows.Forms.HorizontalAlignment.Center;
			toolTip.SetToolTip(this.txFileNumber, "номер файла с которого начать сканирование (F2\r\n)");
			this.txFileNumber.TextChanged += new System.EventHandler(this.FileNumber_TextChanged);
			// 
			// rbInc1
			// 
			this.rbInc1.AutoSize = true;
			this.rbInc1.Checked = true;
			this.rbInc1.Location = new System.Drawing.Point(58, 13);
			this.rbInc1.Name = "rbInc1";
			this.rbInc1.Size = new System.Drawing.Size(37, 17);
			this.rbInc1.TabIndex = 1;
			this.rbInc1.TabStop = true;
			this.rbInc1.Text = "&+1";
			toolTip.SetToolTip(this.rbInc1, "увеличивать каждый слѣдующий файл на 1");
			this.rbInc1.CheckedChanged += new System.EventHandler(this.IncDec_CheckedChanged);
			// 
			// rbDec1
			// 
			this.rbDec1.AutoSize = true;
			this.rbDec1.Location = new System.Drawing.Point(101, 13);
			this.rbDec1.Name = "rbDec1";
			this.rbDec1.Size = new System.Drawing.Size(37, 17);
			this.rbDec1.TabIndex = 2;
			this.rbDec1.Text = "&−1";
			toolTip.SetToolTip(this.rbDec1, "уменьшать  каждый слѣдующий файл на 1");
			this.rbDec1.CheckedChanged += new System.EventHandler(this.IncDec_CheckedChanged);
			// 
			// rbInc2
			// 
			this.rbInc2.AutoSize = true;
			this.rbInc2.Location = new System.Drawing.Point(144, 13);
			this.rbInc2.Name = "rbInc2";
			this.rbInc2.Size = new System.Drawing.Size(37, 17);
			this.rbInc2.TabIndex = 3;
			this.rbInc2.Text = "+&2";
			toolTip.SetToolTip(this.rbInc2, "увеличивать каждый слѣдующий файл на 2");
			this.rbInc2.CheckedChanged += new System.EventHandler(this.IncDec_CheckedChanged);
			// 
			// rbDec2
			// 
			this.rbDec2.AutoSize = true;
			this.rbDec2.Location = new System.Drawing.Point(187, 13);
			this.rbDec2.Name = "rbDec2";
			this.rbDec2.Size = new System.Drawing.Size(37, 17);
			this.rbDec2.TabIndex = 4;
			this.rbDec2.Text = "−2";
			toolTip.SetToolTip(this.rbDec2, "уменьшать каждый слѣдующий файл на 2");
			this.rbDec2.CheckedChanged += new System.EventHandler(this.IncDec_CheckedChanged);
			// 
			// lbLast
			// 
			this.lbLast.Anchor = ((System.Windows.Forms.AnchorStyles)((System.Windows.Forms.AnchorStyles.Bottom | System.Windows.Forms.AnchorStyles.Left)));
			this.lbLast.AutoSize = true;
			this.lbLast.Location = new System.Drawing.Point(224, 105);
			this.lbLast.Name = "lbLast";
			this.lbLast.Size = new System.Drawing.Size(13, 13);
			this.lbLast.TabIndex = 13;
			this.lbLast.Text = "?";
			toolTip.SetToolTip(this.lbLast, "послѣдняя отсканированная страница");
			// 
			// resumeButton
			// 
			this.resumeButton.Location = new System.Drawing.Point(336, 10);
			this.resumeButton.Name = "resumeButton";
			this.resumeButton.Size = new System.Drawing.Size(100, 23);
			this.resumeButton.TabIndex = 6;
			this.resumeButton.Text = "&Продолжить (F5)";
			toolTip.SetToolTip(this.resumeButton, "продолжить остановленное сканирование");
			this.resumeButton.UseVisualStyleBackColor = true;
			this.resumeButton.Click += new System.EventHandler(this.ResumeButton_Click);
			// 
			// deleteButton
			// 
			this.deleteButton.Anchor = ((System.Windows.Forms.AnchorStyles)((System.Windows.Forms.AnchorStyles.Bottom | System.Windows.Forms.AnchorStyles.Left)));
			this.deleteButton.Enabled = false;
			this.deleteButton.Location = new System.Drawing.Point(12, 100);
			this.deleteButton.Name = "deleteButton";
			this.deleteButton.Size = new System.Drawing.Size(100, 23);
			this.deleteButton.TabIndex = 11;
			this.deleteButton.Text = "По&херить (Del)";
			toolTip.SetToolTip(this.deleteButton, "удалить послѣднюю отсканированную страницу");
			this.deleteButton.UseVisualStyleBackColor = true;
			this.deleteButton.Click += new System.EventHandler(this.DeleteButton_Click);
			// 
			// lbSpeed
			// 
			this.lbSpeed.Anchor = ((System.Windows.Forms.AnchorStyles)((System.Windows.Forms.AnchorStyles.Bottom | System.Windows.Forms.AnchorStyles.Left)));
			this.lbSpeed.AutoSize = true;
			this.lbSpeed.Location = new System.Drawing.Point(12, 84);
			this.lbSpeed.Name = "lbSpeed";
			this.lbSpeed.Size = new System.Drawing.Size(22, 13);
			this.lbSpeed.TabIndex = 8;
			this.lbSpeed.Text = "? с";
			toolTip.SetToolTip(this.lbSpeed, "время сканирования послѣдней страницы");
			// 
			// singleButton
			// 
			this.singleButton.Location = new System.Drawing.Point(442, 10);
			this.singleButton.Name = "singleButton";
			this.singleButton.Size = new System.Drawing.Size(75, 23);
			this.singleButton.TabIndex = 7;
			this.singleButton.Text = "&Одну (F8)";
			toolTip.SetToolTip(this.singleButton, "сканировать только эту страницу");
			this.singleButton.UseVisualStyleBackColor = true;
			this.singleButton.Click += new System.EventHandler(this.SingleButton_Click);
			// 
			// lastCheckBox
			// 
			this.lastCheckBox.Anchor = ((System.Windows.Forms.AnchorStyles)((System.Windows.Forms.AnchorStyles.Top | System.Windows.Forms.AnchorStyles.Right)));
			this.lastCheckBox.AutoSize = true;
			this.lastCheckBox.Location = new System.Drawing.Point(624, 14);
			this.lastCheckBox.Name = "lastCheckBox";
			this.lastCheckBox.Size = new System.Drawing.Size(104, 17);
			this.lastCheckBox.TabIndex = 9;
			this.lastCheckBox.Text = "Посл&ѣдняя (F9)";
			toolTip.SetToolTip(this.lastCheckBox, "остановить сканирование послѣ этой страницы");
			this.lastCheckBox.UseVisualStyleBackColor = true;
			this.lastCheckBox.CheckedChanged += new System.EventHandler(this.LastCheckBox_CheckedChanged);
			// 
			// skipCheckBox
			// 
			this.skipCheckBox.Anchor = ((System.Windows.Forms.AnchorStyles)((System.Windows.Forms.AnchorStyles.Bottom | System.Windows.Forms.AnchorStyles.Right)));
			this.skipCheckBox.AutoSize = true;
			this.skipCheckBox.Location = new System.Drawing.Point(722, 106);
			this.skipCheckBox.Name = "skipCheckBox";
			this.skipCheckBox.Size = new System.Drawing.Size(112, 17);
			this.skipCheckBox.TabIndex = 14;
			this.skipCheckBox.Text = "Пропустит&ь (F11)";
			toolTip.SetToolTip(this.skipCheckBox, "не сохранять санируемую сейчас страницу");
			this.skipCheckBox.UseVisualStyleBackColor = true;
			this.skipCheckBox.CheckedChanged += new System.EventHandler(this.SkipCheckBox_CheckedChanged);
			// 
			// rescanButton
			// 
			this.rescanButton.Anchor = ((System.Windows.Forms.AnchorStyles)((System.Windows.Forms.AnchorStyles.Bottom | System.Windows.Forms.AnchorStyles.Left)));
			this.rescanButton.Enabled = false;
			this.rescanButton.Location = new System.Drawing.Point(118, 100);
			this.rescanButton.Name = "rescanButton";
			this.rescanButton.Size = new System.Drawing.Size(100, 23);
			this.rescanButton.TabIndex = 12;
			this.rescanButton.Text = "Ещ&ё раз (F6)";
			toolTip.SetToolTip(this.rescanButton, "пересканировать неудавшуюся страницу");
			this.rescanButton.UseVisualStyleBackColor = true;
			this.rescanButton.Click += new System.EventHandler(this.ReScanButton_Click);
			// 
			// worker
			// 
			this.worker.WorkerReportsProgress = true;
			this.worker.WorkerSupportsCancellation = true;
			this.worker.DoWork += new System.ComponentModel.DoWorkEventHandler(this.Worker_DoWork);
			this.worker.ProgressChanged += new System.ComponentModel.ProgressChangedEventHandler(this.Worker_ProgressChanged);
			this.worker.RunWorkerCompleted += new System.ComponentModel.RunWorkerCompletedEventHandler(this.Worker_RunWorkerCompleted);
			// 
			// MainForm
			// 
			this.AcceptButton = this.startButton;
			this.AutoScaleDimensions = new System.Drawing.SizeF(6F, 13F);
			this.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font;
			this.BackgroundImageLayout = System.Windows.Forms.ImageLayout.Zoom;
			this.CancelButton = stopButton;
			this.ClientSize = new System.Drawing.Size(846, 135);
			this.Controls.Add(this.rescanButton);
			this.Controls.Add(this.skipCheckBox);
			this.Controls.Add(this.lastCheckBox);
			this.Controls.Add(this.singleButton);
			this.Controls.Add(this.lbSpeed);
			this.Controls.Add(this.deleteButton);
			this.Controls.Add(this.resumeButton);
			this.Controls.Add(this.lbLast);
			this.Controls.Add(this.txFileNumber);
			this.Controls.Add(this.rbInc1);
			this.Controls.Add(this.rbDec1);
			this.Controls.Add(stopButton);
			this.Controls.Add(this.rbInc2);
			this.Controls.Add(this.startButton);
			this.Controls.Add(this.rbDec2);
			this.Icon = ((System.Drawing.Icon)(resources.GetObject("$this.Icon")));
			this.KeyPreview = true;
			this.Name = "MainForm";
			this.Text = "Сканирование";
			this.WindowState = System.Windows.Forms.FormWindowState.Maximized;
			this.ResumeLayout(false);
			this.PerformLayout();

		}

		#endregion
		private System.ComponentModel.BackgroundWorker worker;
		private System.Windows.Forms.RadioButton rbInc1;
		private System.Windows.Forms.RadioButton rbDec1;
		private System.Windows.Forms.TextBox txFileNumber;
		private System.Windows.Forms.RadioButton rbInc2;
		private System.Windows.Forms.RadioButton rbDec2;
		private System.Windows.Forms.Button startButton;
		private System.Windows.Forms.Label lbLast;
		private System.Windows.Forms.Button resumeButton;
		private System.Windows.Forms.Button deleteButton;
		private System.Windows.Forms.Label lbSpeed;
		private System.Windows.Forms.Button singleButton;
		private System.Windows.Forms.CheckBox lastCheckBox;
		private System.Windows.Forms.CheckBox skipCheckBox;
		private System.Windows.Forms.Button rescanButton;
	}
}

