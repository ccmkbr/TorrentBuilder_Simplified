using System;
using System.IO;
using System.Windows.Forms;
using System.Threading;
using System.Collections.Generic;
using System.Diagnostics;


namespace TorrentBuilder_Simplified
{

    public partial class TorrentBuild : Form
    {
        private bool autoPiece = true;
        private int selectedPieceSize = 0;
        public string folderToSave;
        private long totalSize;
        private long partialSize = 0;
        private int progress = 0;
        List<Thread> allThreads = new List<Thread>();
        List<Thread> activeThreads = new List<Thread>();

        public TorrentBuild()
        {
            InitializeComponent();
        }

        private void Form1_Load(object sender, EventArgs e)
        {
            this.ComboBPieceSize.Enabled = false;
            this.CheckBPieceSizeAuto.Checked = true;
            this.ComboBPieceSize.SelectedIndex = 0;
        }

        private void selectfolder_Click(object sender, EventArgs e)
        {
            //Adds a folder to be transformed into torrent.
            this.BrowseForFolder.Description = "Selecione uma pasta para criar um torrent:";
            if (this.BrowseForFolder.ShowDialog() == DialogResult.OK)
            {
                this.ListBox_Paths.BeginUpdate();
                this.ListBox_Paths.Items.Add(this.BrowseForFolder.SelectedPath);
                this.ListBox_Paths.EndUpdate();
            }
            else { return; }
        }

        private void selectfile_Click(object sender, EventArgs e)
        {
            //Adds an individual file to be transformed into torrent.
            if (this.BrowseForFile.ShowDialog() == DialogResult.OK)
            {
                this.ListBox_Paths.BeginUpdate();
                this.ListBox_Paths.Items.Add(this.BrowseForFile.FileName);
                this.ListBox_Paths.EndUpdate();
            }
            else { return; }
        }

        private void ComboBPieceSize_SelectedIndexChanged(object sender, EventArgs e)
        {
            //Choose the piece size manually.
            this.selectedPieceSize = this.GetPieceSize();
        }

        private void checkBox1_CheckedChanged(object sender, EventArgs e)
        {
            //Toggles between manual or automatic piece size selection.
            if (this.CheckBPieceSizeAuto.Checked)
            {
                this.ComboBPieceSize.Enabled = false;
                this.autoPiece = true;
            }
            else
            {
                this.ComboBPieceSize.Enabled = true;
                this.autoPiece = false;
                this.selectedPieceSize = this.GetPieceSize();
            }
        }

        private void remove_Click_1(object sender, EventArgs e)
        {
            //Removes an item from the paths list box.
            while (this.ListBox_Paths.CheckedItems.Count != 0)
            {
                this.ListBox_Paths.Items.Remove(this.ListBox_Paths.CheckedItems[0]);
            }
        }

        private void BtnBuildTorrentNow_Click(object sender, EventArgs e)
        {
            //Starts to build the torrents contained in the list box.
            if (this.ListBox_Paths.Items.Count == 0)
            {
                MessageBox.Show("ERRO: Você deve adicionar pelo menos um arquivo/diretório.", "Erro");
                return;
            }

            //Set a destinantion folder, where all the torrents will be saved at.
            this.BrowseForFolder.Description = "Selecione uma pasta para salvar seus arquivos .torrent:";
            if (this.BrowseForFolder.ShowDialog() == DialogResult.OK)
            {
                this.folderToSave = this.BrowseForFolder.SelectedPath;
            }
            else { return; }

            //Disables all commands while the program is busy making the torrents.
            this.BtnBuildTorrentNow.Enabled = false;
            this.BtnRemovePath.Enabled = false;
            this.BtnSelectFile.Enabled = false;
            this.BtnSelectFolder.Enabled = false;
            this.CheckBPieceSizeAuto.Enabled = false;
            this.ComboBPieceSize.Enabled = false;

            //Begins the construction.
            Thread beginConstructionThread = new Thread(BeginConstruction);
            beginConstructionThread.Start();
        }

        public void BeginConstruction()
        {
            Stopwatch sw;
            sw = Stopwatch.StartNew();
            System.Console.Write("Watch Started.\n");
            //Gets the size of every file in the torrent list box and every file inside the folders as well.
            this.totalSize = this.GetAllSize();

            //Counts the number of Threads that the processor can handle simultaneously.
            int processorNumber = Environment.ProcessorCount;

            //Define the number of threads used by the program. The minimum value between the number of threads
            //available in the processor and the number of torrents to be made will be chonsen.
            int threadNumber = Math.Min(processorNumber, this.ListBox_Paths.Items.Count);
            try
            {
                foreach (string LBPath in this.ListBox_Paths.Items)
                {
                    //Makes one thread for every file/folder.
                    Thread mkTorrentThread = new Thread(() => MakeTorrent(LBPath));
                    //Adds all threads to a list.
                    allThreads.Add(mkTorrentThread);
                }
                while (allThreads.Count > 0)
                {
                    foreach (Thread t in allThreads)
                    {
                        if (activeThreads.Count < threadNumber)
                        {
                            //Removes the thread from the queue and starts it.
                            t.Start();
                            allThreads.Remove(t);
                            //Adds to an active threads list.
                            activeThreads.Add(t);
                            Thread.Sleep(50);
                            break;
                        }
                    }
                    foreach (Thread t in activeThreads)
                    {
                        //Checks if the thread is still running. If not, removes from the active threads list.
                        if (t.IsAlive == false)
                        {                            
                            activeThreads.Remove(t);
                            Thread.Sleep(50);
                            break;
                        }
                    }
                }

            }
            finally
            {
                //Waits for the lastest threads to be finished.
                while (activeThreads.Count != 0)
                {
                    foreach (Thread t in activeThreads)
                    {
                        if (t.IsAlive == false)
                        {
                            activeThreads.Remove(t);
                            break;
                        }
                    }
                }
                sw.Stop();
                int timeEllapsed = (int)sw.ElapsedMilliseconds;
                timeEllapsed /= 1000;

                System.Console.Write("Time ellapsed: " + timeEllapsed.ToString() + "s.\n");
                this.methodFinishedMaking(true);
            }
        }

        private void MakeTorrent(string path)
        {
            //This method is run as a thread to build every torrent individually.
            long totalFileSize = 0;
            int currentPieceSize;

            //Start to build a torrent if the path is a folder.
            if (Directory.Exists(@path))
            {
                string[] pathArray = Directory.GetFiles(@path, "*", SearchOption.AllDirectories);
                for (int i = 0; i < pathArray.Length; i++)
                {
                    FileInfo myFile = new FileInfo(pathArray[i]);
                    long filesize = myFile.Length;
                    totalFileSize += filesize;
                }

                if (this.autoPiece)
                {
                    currentPieceSize = this.GetAutoPieceSize(totalFileSize);
                }
                else { currentPieceSize = this.selectedPieceSize; }

                TorrentMaker folderMaker = new TorrentMaker(this, currentPieceSize);
                folderMaker.MakeTorrentFromFolder(@path);
                return;

            }
            if (!File.Exists(path))
            {
                MessageBox.Show("ERRO: Arquivo inválido: " + path, "Error");
                return;
            }
            else
            {
                //Start to build a torrent if the path is a file.
                FileInfo myFile = new FileInfo(path);
                totalFileSize = myFile.Length;

                if (this.autoPiece)
                {
                    currentPieceSize = this.GetAutoPieceSize(totalFileSize);
                }
                else { currentPieceSize = this.selectedPieceSize; }

                TorrentMaker fileMaker = new TorrentMaker(this, currentPieceSize);
                fileMaker.MakeTorrentFromFile(path);
                return;
            }
        }

        private int GetPieceSize()
        {
            //Sets the piece size according to the value selected by the user.
            if (this.ComboBPieceSize.SelectedIndex == 0)
            {
                return 32768;
            }
            if (this.ComboBPieceSize.SelectedIndex == 1)
            {
                return 65536;
            }
            if (this.ComboBPieceSize.SelectedIndex == 2)
            {
                return 131072;
            }
            if (this.ComboBPieceSize.SelectedIndex == 3)
            {
                return 262144;
            }
            if (this.ComboBPieceSize.SelectedIndex == 4)
            {
                return 524288;
            }
            if (this.ComboBPieceSize.SelectedIndex == 5)
            {
                return 1048576;
            }
            if (this.ComboBPieceSize.SelectedIndex == 6)
            {
                return 2097152;
            }
            if (this.ComboBPieceSize.SelectedIndex == 7)
            {
                return 4194304;
            }
            if (this.ComboBPieceSize.SelectedIndex == 8)
            {
                return 8388608;
            }
            if (this.ComboBPieceSize.SelectedIndex == 9)
            {
                return 16777216;
            }
            return 32768;
        }

        private int GetAutoPieceSize(long fsize)
        {
            //Calculates the piece sizer automatically, based on every size or folder individually.
            if (fsize < 50000000L)
            {
                return 32768;
            }
            if (fsize < 150000000L)
            {
                return 65536;
            }
            if (fsize < 350000000L)
            {
                return 131072;
            }
            if (fsize < 512000000L)
            {
                return 262144;
            }
            if (fsize < 1000000000L)
            {
                return 524288;
            }
            if (fsize < 2000000000L)
            {
                return 1048576;
            }
            if (fsize < 4000000000L)
            {
                return 2097152;
            }
            if (fsize < 8000000000L)
            {
                return 4194304;
            }
            if (fsize < 16000000000L)
            {
                return 8388608;
            }

            return 16777216;
        }

        private long GetAllSize()
        {
            //Sums the size of all files individually.
            long totalSize = 0;
            foreach (string path in this.ListBox_Paths.Items)
            {
                //Sums the size of every file inside a folder.
                if (Directory.Exists(path))
                {
                    string[] pathArray = Directory.GetFiles(path, "*", SearchOption.AllDirectories);
                    foreach (string pathname in pathArray)
                    {
                        FileInfo file1 = new FileInfo(pathname);
                        long file1size = file1.Length;
                        totalSize += file1size;
                    }
                    continue;
                }
                //Sums the size of every file in the list box.
                FileInfo file2 = new FileInfo(path);
                long file2size = file2.Length;
                totalSize += file2size;
            }
            return totalSize;
        }

        public void methodUpdateBar(long iValue)
        {
            //Updates the progress bar.
            if (this.InvokeRequired)
            {
                this.BeginInvoke(new Action<long>(methodUpdateBar), new object[] { iValue });
                return;
            }

            this.partialSize += iValue;
            this.progress = Math.Min(100, (int)Math.Round((double)((((double)this.partialSize) / ((double)this.totalSize)) * 100.0)));
            PgrsTorrent.Value = this.progress;
            PgrsTorrent.Update();
        }

        public void methodFinishedMaking(bool hasFinished)
        {
            //When all torrents are finished, this value is called to 
            //restore the form to the default settings.
            if (this.InvokeRequired)
            {
                this.BeginInvoke(new Action<bool>(methodFinishedMaking), new object[] { hasFinished });
                return;
            }

            this.partialSize = 0;
            this.PgrsTorrent.Value = 0;
            this.PgrsTorrent.Update();
            this.BtnBuildTorrentNow.Enabled = true;
            this.BtnRemovePath.Enabled = true;
            this.BtnSelectFile.Enabled = true;
            this.BtnSelectFolder.Enabled = true;
            this.PgrsTorrent.Value = 0;
            this.CheckBPieceSizeAuto.Enabled = true;
            this.ComboBPieceSize.Enabled = !this.autoPiece;
            GC.Collect();
        }
    }
}
