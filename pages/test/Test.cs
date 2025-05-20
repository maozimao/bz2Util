using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Drawing;
using System.IO;
using System.IO.Compression;
using System.Threading;
using System.Threading.Tasks;
using System.Windows.Forms;

namespace MyExeApp.pages.Test
{
    public partial class TestForm : Form
    {
        private readonly HashSet<string> processedFiles = new HashSet<string>();
        private readonly object fileProcessLock = new object();
        private string lastReportedFile = null;
        private CancellationTokenSource cancellationTokenSource;

        public TestForm()
        {
            InitializeComponent();
            InitializeWorker();
        }

        private void InitializeWorker()
        {
            worker.WorkerReportsProgress = true;
            worker.WorkerSupportsCancellation = true;
            worker.DoWork += Worker_DoWork;
            worker.ProgressChanged += Worker_ProgressChanged;
            worker.RunWorkerCompleted += Worker_RunWorkerCompleted;
        }

        private void TestForm_Load(object sender, EventArgs e)
        {
            UpdateLayout();
        }

        private void TestForm_Resize(object sender, EventArgs e)
        {
            UpdateLayout();
        }

        private void UpdateLayout()
        {
            // 调整按钮面板位置使其居中
            buttonFlowLayoutPanel.Left = (mainTableLayout.Width - buttonFlowLayoutPanel.Width) / 2;
            
            // 调整进度条宽度
            progressBar.Width = progressLayoutPanel.Width - progressLabel.Width - 10;
        }

        private void compressBut_Click(object sender, EventArgs e)
        {
            if (worker.IsBusy) return;
            
            ResetOperationState();
            
            using (FolderBrowserDialog folderDialog = new FolderBrowserDialog())
            {
                folderDialog.Description = "选择要压缩的文件夹";
                if (folderDialog.ShowDialog() == DialogResult.OK)
                {
                    StartOperation(folderDialog.SelectedPath, "压缩结果", OperationType.Compress);
                }
            }
        }

        private void decompressionBut_Click(object sender, EventArgs e)
        {
            if (worker.IsBusy) return;
            
            ResetOperationState();
            
            using (FolderBrowserDialog folderDialog = new FolderBrowserDialog())
            {
                folderDialog.Description = "选择包含bz2文件的文件夹";
                if (folderDialog.ShowDialog() == DialogResult.OK)
                {
                    StartOperation(folderDialog.SelectedPath, "解压结果", OperationType.Decompress);
                }
            }
        }

        private void ResetOperationState()
        {
            lock (fileProcessLock)
            {
                processedFiles.Clear();
            }
            lastReportedFile = null;
            cancellationTokenSource = new CancellationTokenSource();
        }

        private void StartOperation(string sourcePath, string outputDirName, OperationType type)
        {
            try
            {
                string parentDir = Directory.GetParent(sourcePath)?.FullName ?? sourcePath;
                string outputDir = Path.Combine(parentDir, outputDirName);
                Directory.CreateDirectory(outputDir);

                int threadCount = Math.Max(1, int.TryParse(threadCountTextBox.Text, out int t) ? t : 1);

                progressBar.Value = 0;
                fileListBox.Items.Clear();
                progressLabel.Text = "处理进度: 0%";

                compressBut.Enabled = false;
                decompressionBut.Enabled = false;

                worker.RunWorkerAsync(new CompressionContext
                {
                    SourcePath = sourcePath,
                    OutputDir = outputDir,
                    ThreadCount = threadCount,
                    OperationType = type,
                    CancellationToken = cancellationTokenSource.Token
                });
            }
            catch (Exception ex)
            {
                MessageBox.Show($"操作初始化失败: {ex.Message}", "错误", MessageBoxButtons.OK, MessageBoxIcon.Error);
            }
        }

        private void Worker_DoWork(object sender, DoWorkEventArgs e)
        {
            var context = (CompressionContext)e.Argument;
            string[] files = context.OperationType == OperationType.Compress ?
                GetAllFiles(context.SourcePath) :
                GetAllFilesByExtension(context.SourcePath, ".bz2");

            if (files.Length == 0)
            {
                worker.ReportProgress(100, "没有找到需要处理的文件");
                return;
            }

            int processed = 0;
            ParallelOptions options = new ParallelOptions
            {
                MaxDegreeOfParallelism = context.ThreadCount,
                CancellationToken = context.CancellationToken
            };

            try
            {
                Parallel.ForEach(files, options, filePath =>
                {
                    context.CancellationToken.ThrowIfCancellationRequested();

                    try
                    {
                        bool shouldProcess;
                        lock (fileProcessLock)
                        {
                            shouldProcess = !processedFiles.Contains(filePath);
                            if (shouldProcess)
                            {
                                processedFiles.Add(filePath);
                            }
                        }

                        if (!shouldProcess) return;

                        if (context.OperationType == OperationType.Compress)
                            CompressFile(filePath, context.SourcePath, context.OutputDir);
                        else
                            DecompressFile(filePath, context.SourcePath, context.OutputDir);

                        lock (this)
                        {
                            processed++;
                            int progress = (int)((double)processed / files.Length * 100);
                            worker.ReportProgress(progress, $"完成: {Path.GetFileName(filePath)}");
                        }
                    }
                    catch (OperationCanceledException)
                    {
                        e.Cancel = true;
                        throw;
                    }
                    catch (Exception ex)
                    {
                        worker.ReportProgress(0, $"错误: {Path.GetFileName(filePath)} - {ex.Message}");
                    }
                });
            }
            catch (OperationCanceledException)
            {
                e.Cancel = true;
            }
        }

        private void Worker_ProgressChanged(object sender, ProgressChangedEventArgs e)
        {
            if (e.ProgressPercentage >= 0)
            {
                progressBar.Value = e.ProgressPercentage;
                progressLabel.Text = $"处理进度: {e.ProgressPercentage}%";
            }

            if (e.UserState != null)
            {
                string currentFile = e.UserState.ToString();
                if (currentFile != lastReportedFile)
                {
                    fileListBox.Items.Add(e.UserState);
                    fileListBox.TopIndex = fileListBox.Items.Count - 1;
                    lastReportedFile = currentFile;
                }
            }
        }

        private void Worker_RunWorkerCompleted(object sender, RunWorkerCompletedEventArgs e)
        {
            try
            {
                if (e.Cancelled)
                {
                    MessageBox.Show("操作已取消", "提示", MessageBoxButtons.OK, MessageBoxIcon.Information);
                }
                else if (e.Error != null)
                {
                    MessageBox.Show($"操作失败: {e.Error.Message}", "错误", MessageBoxButtons.OK, MessageBoxIcon.Error);
                }
                else
                {
                    MessageBox.Show("操作完成！", "成功", MessageBoxButtons.OK, MessageBoxIcon.Information);
                }
            }
            finally
            {
                compressBut.Enabled = true;
                decompressionBut.Enabled = true;
            }
        }

        private string[] GetAllFiles(string rootDir)
        {
            var files = new List<string>();
            var queue = new Queue<string>();
            queue.Enqueue(rootDir);
            while (queue.Count > 0)
            {
                string dir = queue.Dequeue();
                files.AddRange(Directory.GetFiles(dir));
                foreach (string subDir in Directory.GetDirectories(dir))
                {
                    queue.Enqueue(subDir);
                }
            }
            return files.ToArray();
        }

        private string[] GetAllFilesByExtension(string rootDir, string ext)
        {
            var files = new List<string>();
            var queue = new Queue<string>();
            queue.Enqueue(rootDir);
            while (queue.Count > 0)
            {
                string dir = queue.Dequeue();
                files.AddRange(Directory.GetFiles(dir, "*" + ext));
                foreach (string subDir in Directory.GetDirectories(dir))
                {
                    queue.Enqueue(subDir);
                }
            }
            return files.ToArray();
        }

        private void CompressFile(string filePath, string rootDir, string outputDir)
        {
            string relativePath = Path.GetRelativePath(rootDir, filePath);
            string outputPath = Path.Combine(outputDir, relativePath + ".bz2");
            Directory.CreateDirectory(Path.GetDirectoryName(outputPath));

            using (FileStream fileStream = File.OpenRead(filePath))
            using (FileStream zipStream = File.Create(outputPath))
            using (ZipArchive archive = new ZipArchive(zipStream, ZipArchiveMode.Create))
            {
                ZipArchiveEntry entry = archive.CreateEntry(Path.GetFileName(filePath), CompressionLevel.Optimal);
                using (Stream entryStream = entry.Open())
                {
                    fileStream.CopyTo(entryStream);
                }
            }
        }

        private void DecompressFile(string bz2FilePath, string rootDir, string outputDir)
        {
            string relativePath = Path.GetRelativePath(rootDir, bz2FilePath);
            string outputPath = Path.Combine(outputDir, relativePath.Replace(".bz2", ""));
            Directory.CreateDirectory(Path.GetDirectoryName(outputPath));

            using (FileStream zipStream = File.OpenRead(bz2FilePath))
            using (ZipArchive archive = new ZipArchive(zipStream, ZipArchiveMode.Read))
            {
                foreach (ZipArchiveEntry entry in archive.Entries)
                {
                    string entryOutputPath = Path.Combine(Path.GetDirectoryName(outputPath), entry.FullName);
                    Directory.CreateDirectory(Path.GetDirectoryName(entryOutputPath));
                    entry.ExtractToFile(entryOutputPath, true);
                }
            }
        }

        protected override void OnFormClosing(FormClosingEventArgs e)
        {
            if (worker.IsBusy)
            {
                var result = MessageBox.Show("操作正在进行中，是否取消并关闭？", "确认", 
                    MessageBoxButtons.YesNo, MessageBoxIcon.Question);
                
                if (result == DialogResult.Yes)
                {
                    cancellationTokenSource.Cancel();
                    worker.CancelAsync();
                    
                    int waitCount = 0;
                    while (worker.IsBusy && waitCount < 10)
                    {
                        Application.DoEvents();
                        Thread.Sleep(100);
                        waitCount++;
                    }
                }
                else
                {
                    e.Cancel = true;
                    return;
                }
            }
            
            base.OnFormClosing(e);
        }

        public enum OperationType { Compress, Decompress }

        public class CompressionContext
        {
            public string SourcePath { get; set; }
            public string OutputDir { get; set; }
            public int ThreadCount { get; set; }
            public OperationType OperationType { get; set; }
            public CancellationToken CancellationToken { get; set; }
        }
    }
}