using System;
using System.ComponentModel;
using System.Drawing;
using System.Windows.Forms;

namespace MyExeApp.pages.Test
{
    partial class TestForm
    {
        /// <summary>
        /// 必需的设计器变量。
        /// </summary>
        private IContainer components = null;

        /// <summary>
        /// 清理所有正在使用的资源。
        /// </summary>
        /// <param name="disposing">如果应释放托管资源，为 true；否则为 false。</param>
        protected override void Dispose(bool disposing)
        {
            if (disposing && (components != null))
            {
                components.Dispose();
            }
            base.Dispose(disposing);
        }

        #region Windows 窗体设计器生成的代码

        /// <summary>
        /// 设计器支持所需的方法 - 不要修改
        /// 使用代码编辑器修改此方法的内容。
        /// </summary>
        private void InitializeComponent()
        {
            this.components = new Container();
            this.mainTableLayout = new TableLayoutPanel();
            this.inputFlowLayoutPanel = new FlowLayoutPanel();
            this.threadCountLabel = new Label();
            this.threadCountTextBox = new TextBox();
            this.buttonFlowLayoutPanel = new FlowLayoutPanel();
            this.compressBut = new Button();
            this.decompressionBut = new Button();
            this.progressLayoutPanel = new FlowLayoutPanel();
            this.progressLabel = new Label();
            this.progressBar = new ProgressBar();
            this.fileListBox = new ListBox();
            this.worker = new BackgroundWorker();
            this.mainTableLayout.SuspendLayout();
            this.inputFlowLayoutPanel.SuspendLayout();
            this.buttonFlowLayoutPanel.SuspendLayout();
            this.progressLayoutPanel.SuspendLayout();
            this.SuspendLayout();

            // mainTableLayout
            this.mainTableLayout.ColumnCount = 1;
            this.mainTableLayout.ColumnStyles.Add(new ColumnStyle(SizeType.Percent, 100F));
            this.mainTableLayout.Controls.Add(this.inputFlowLayoutPanel, 0, 0);
            this.mainTableLayout.Controls.Add(this.buttonFlowLayoutPanel, 0, 1);
            this.mainTableLayout.Controls.Add(this.progressLayoutPanel, 0, 2);
            this.mainTableLayout.Controls.Add(this.fileListBox, 0, 3);
            this.mainTableLayout.Dock = DockStyle.Fill;
            this.mainTableLayout.Location = new Point(0, 0);
            this.mainTableLayout.Name = "mainTableLayout";
            this.mainTableLayout.RowCount = 4;
            this.mainTableLayout.RowStyles.Add(new RowStyle(SizeType.Absolute, 40F));
            this.mainTableLayout.RowStyles.Add(new RowStyle(SizeType.Absolute, 60F));
            this.mainTableLayout.RowStyles.Add(new RowStyle(SizeType.Absolute, 40F));
            this.mainTableLayout.RowStyles.Add(new RowStyle(SizeType.Percent, 100F));
            this.mainTableLayout.Size = new Size(400, 400);
            this.mainTableLayout.TabIndex = 0;

            // inputFlowLayoutPanel
            this.inputFlowLayoutPanel.Anchor = AnchorStyles.Left | AnchorStyles.Right;
            this.inputFlowLayoutPanel.AutoSize = true;
            this.inputFlowLayoutPanel.Controls.Add(this.threadCountLabel);
            this.inputFlowLayoutPanel.Controls.Add(this.threadCountTextBox);
            this.inputFlowLayoutPanel.Location = new Point(3, 3);
            this.inputFlowLayoutPanel.Name = "inputFlowLayoutPanel";
            this.inputFlowLayoutPanel.Size = new Size(394, 34);
            this.inputFlowLayoutPanel.TabIndex = 0;

            // threadCountLabel
            this.threadCountLabel.AutoSize = true;
            this.threadCountLabel.Location = new Point(3, 6);
            this.threadCountLabel.Name = "threadCountLabel";
            this.threadCountLabel.Size = new Size(48, 13);
            this.threadCountLabel.TabIndex = 0;
            this.threadCountLabel.Text = "线程数:";

            // threadCountTextBox
            this.threadCountTextBox.Location = new Point(57, 3);
            this.threadCountTextBox.Name = "threadCountTextBox";
            this.threadCountTextBox.Size = new Size(100, 20);
            this.threadCountTextBox.TabIndex = 1;
            this.threadCountTextBox.Text = "1";

            // buttonFlowLayoutPanel
            this.buttonFlowLayoutPanel.Anchor = AnchorStyles.None;
            this.buttonFlowLayoutPanel.AutoSize = true;
            this.buttonFlowLayoutPanel.Controls.Add(this.compressBut);
            this.buttonFlowLayoutPanel.Controls.Add(this.decompressionBut);
            this.buttonFlowLayoutPanel.Location = new Point(75, 43);
            this.buttonFlowLayoutPanel.Name = "buttonFlowLayoutPanel";
            this.buttonFlowLayoutPanel.Size = new Size(250, 54);
            this.buttonFlowLayoutPanel.TabIndex = 1;

            // compressBut
            this.compressBut.Location = new Point(3, 3);
            this.compressBut.Name = "compressBut";
            this.compressBut.Size = new Size(120, 40);
            this.compressBut.TabIndex = 0;
            this.compressBut.Text = "开始压缩";
            this.compressBut.UseVisualStyleBackColor = true;
            this.compressBut.Click += new EventHandler(this.compressBut_Click);

            // decompressionBut
            this.decompressionBut.Location = new Point(129, 3);
            this.decompressionBut.Name = "decompressionBut";
            this.decompressionBut.Size = new Size(120, 40);
            this.decompressionBut.TabIndex = 1;
            this.decompressionBut.Text = "开始解压";
            this.decompressionBut.UseVisualStyleBackColor = true;
            this.decompressionBut.Click += new EventHandler(this.decompressionBut_Click);

            // progressLayoutPanel
            this.progressLayoutPanel.Anchor = AnchorStyles.Left | AnchorStyles.Right;
            this.progressLayoutPanel.AutoSize = true;
            this.progressLayoutPanel.Controls.Add(this.progressLabel);
            this.progressLayoutPanel.Controls.Add(this.progressBar);
            this.progressLayoutPanel.Location = new Point(3, 103);
            this.progressLayoutPanel.Name = "progressLayoutPanel";
            this.progressLayoutPanel.Size = new Size(394, 34);
            this.progressLayoutPanel.TabIndex = 2;

            // progressLabel
            this.progressLabel.AutoSize = true;
            this.progressLabel.Location = new Point(3, 6);
            this.progressLabel.Name = "progressLabel";
            this.progressLabel.Size = new Size(56, 13);
            this.progressLabel.TabIndex = 0;
            this.progressLabel.Text = "处理进度:";

            // progressBar
            this.progressBar.Anchor = AnchorStyles.Left | AnchorStyles.Right;
            this.progressBar.Location = new Point(65, 3);
            this.progressBar.Name = "progressBar";
            this.progressBar.Size = new Size(326, 23);
            this.progressBar.TabIndex = 1;

            // fileListBox
            this.fileListBox.Dock = DockStyle.Fill;
            this.fileListBox.FormattingEnabled = true;
            this.fileListBox.HorizontalScrollbar = true;
            this.fileListBox.Location = new Point(3, 143);
            this.fileListBox.Name = "fileListBox";
            this.fileListBox.Size = new Size(394, 254);
            this.fileListBox.TabIndex = 3;

            // worker
            this.worker.WorkerReportsProgress = true;
            this.worker.WorkerSupportsCancellation = true;
            this.worker.DoWork += new DoWorkEventHandler(this.Worker_DoWork);
            this.worker.ProgressChanged += new ProgressChangedEventHandler(this.Worker_ProgressChanged);
            this.worker.RunWorkerCompleted += new RunWorkerCompletedEventHandler(this.Worker_RunWorkerCompleted);

            // TestForm
            this.AutoScaleDimensions = new SizeF(6F, 13F);
            this.AutoScaleMode = AutoScaleMode.Font;
            this.ClientSize = new Size(400, 400);
            this.Controls.Add(this.mainTableLayout);
            this.MinimumSize = new Size(300, 300);
            this.Name = "TestForm";
            this.Text = "花茶苑多线程压缩工具";
            this.Load += new EventHandler(this.TestForm_Load);
            this.Resize += new EventHandler(this.TestForm_Resize);
            this.mainTableLayout.ResumeLayout(false);
            this.mainTableLayout.PerformLayout();
            this.inputFlowLayoutPanel.ResumeLayout(false);
            this.inputFlowLayoutPanel.PerformLayout();
            this.buttonFlowLayoutPanel.ResumeLayout(false);
            this.progressLayoutPanel.ResumeLayout(false);
            this.progressLayoutPanel.PerformLayout();
            this.ResumeLayout(false);
        }

        #endregion

        private TableLayoutPanel mainTableLayout;
        private FlowLayoutPanel inputFlowLayoutPanel;
        private Label threadCountLabel;
        private TextBox threadCountTextBox;
        private FlowLayoutPanel buttonFlowLayoutPanel;
        private Button compressBut;
        private Button decompressionBut;
        private FlowLayoutPanel progressLayoutPanel;
        private Label progressLabel;
        private ProgressBar progressBar;
        private ListBox fileListBox;
        private BackgroundWorker worker;
    }
}