namespace TorrentBuilder_Simplified
{
    partial class TorrentBuild
    {
        /// <summary>
        /// Required designer variable.
        /// </summary>
        private System.ComponentModel.IContainer components = null;

        /// <summary>
        /// Clean up any resources being used.
        /// </summary>
        /// <param name="disposing">true if managed resources should be disposed; otherwise, false.</param>
        protected override void Dispose(bool disposing)
        {
            if (disposing && (components != null))
            {
                components.Dispose();
            }
            base.Dispose(disposing);
        }

        #region Windows Form Designer generated code

        /// <summary>
        /// Required method for Designer support - do not modify
        /// the contents of this method with the code editor.
        /// </summary>
        private void InitializeComponent()
        {
            this.MainPanel = new System.Windows.Forms.Panel();
            this.label2 = new System.Windows.Forms.Label();
            this.PgrsTorrent = new System.Windows.Forms.ProgressBar();
            this.BtnBuildTorrentNow = new System.Windows.Forms.Button();
            this.groupBox1 = new System.Windows.Forms.GroupBox();
            this.ListBox_Paths = new System.Windows.Forms.CheckedListBox();
            this.BtnRemovePath = new System.Windows.Forms.Button();
            this.ComboBPieceSize = new System.Windows.Forms.ComboBox();
            this.label3 = new System.Windows.Forms.Label();
            this.CheckBPieceSizeAuto = new System.Windows.Forms.CheckBox();
            this.BtnSelectFolder = new System.Windows.Forms.Button();
            this.BtnSelectFile = new System.Windows.Forms.Button();
            this.BrowseForFile = new System.Windows.Forms.OpenFileDialog();
            this.BrowseForFolder = new System.Windows.Forms.FolderBrowserDialog();
            this.MainPanel.SuspendLayout();
            this.groupBox1.SuspendLayout();
            this.SuspendLayout();
            // 
            // MainPanel
            // 
            this.MainPanel.AutoSize = true;
            this.MainPanel.Controls.Add(this.label2);
            this.MainPanel.Controls.Add(this.PgrsTorrent);
            this.MainPanel.Controls.Add(this.BtnBuildTorrentNow);
            this.MainPanel.Controls.Add(this.groupBox1);
            this.MainPanel.Location = new System.Drawing.Point(0, 0);
            this.MainPanel.Name = "MainPanel";
            this.MainPanel.Size = new System.Drawing.Size(530, 409);
            this.MainPanel.TabIndex = 0;
            // 
            // label2
            // 
            this.label2.AutoSize = true;
            this.label2.Location = new System.Drawing.Point(25, 335);
            this.label2.Name = "label2";
            this.label2.Size = new System.Drawing.Size(57, 13);
            this.label2.TabIndex = 6;
            this.label2.Text = "Progresso:";
            // 
            // PgrsTorrent
            // 
            this.PgrsTorrent.Location = new System.Drawing.Point(128, 330);
            this.PgrsTorrent.Name = "PgrsTorrent";
            this.PgrsTorrent.Size = new System.Drawing.Size(379, 23);
            this.PgrsTorrent.Step = 1;
            this.PgrsTorrent.TabIndex = 5;
            // 
            // BtnBuildTorrentNow
            // 
            this.BtnBuildTorrentNow.Location = new System.Drawing.Point(386, 362);
            this.BtnBuildTorrentNow.Name = "BtnBuildTorrentNow";
            this.BtnBuildTorrentNow.Size = new System.Drawing.Size(121, 23);
            this.BtnBuildTorrentNow.TabIndex = 4;
            this.BtnBuildTorrentNow.Text = "Construir Torrent!";
            this.BtnBuildTorrentNow.UseVisualStyleBackColor = true;
            this.BtnBuildTorrentNow.Click += new System.EventHandler(this.BtnBuildTorrentNow_Click);
            // 
            // groupBox1
            // 
            this.groupBox1.Controls.Add(this.ListBox_Paths);
            this.groupBox1.Controls.Add(this.BtnRemovePath);
            this.groupBox1.Controls.Add(this.ComboBPieceSize);
            this.groupBox1.Controls.Add(this.label3);
            this.groupBox1.Controls.Add(this.CheckBPieceSizeAuto);
            this.groupBox1.Controls.Add(this.BtnSelectFolder);
            this.groupBox1.Controls.Add(this.BtnSelectFile);
            this.groupBox1.Location = new System.Drawing.Point(12, 6);
            this.groupBox1.Name = "groupBox1";
            this.groupBox1.Size = new System.Drawing.Size(508, 308);
            this.groupBox1.TabIndex = 10;
            this.groupBox1.TabStop = false;
            this.groupBox1.Text = "Configurações";
            // 
            // ListBox_Paths
            // 
            this.ListBox_Paths.CheckOnClick = true;
            this.ListBox_Paths.FormattingEnabled = true;
            this.ListBox_Paths.Location = new System.Drawing.Point(116, 34);
            this.ListBox_Paths.Name = "ListBox_Paths";
            this.ListBox_Paths.Size = new System.Drawing.Size(379, 199);
            this.ListBox_Paths.TabIndex = 12;
            // 
            // BtnRemovePath
            // 
            this.BtnRemovePath.BackColor = System.Drawing.SystemColors.Control;
            this.BtnRemovePath.Image = global::TorrentBuilder_Simplified.Properties.Resources.delete_property_24;
            this.BtnRemovePath.Location = new System.Drawing.Point(30, 174);
            this.BtnRemovePath.Name = "BtnRemovePath";
            this.BtnRemovePath.Size = new System.Drawing.Size(58, 59);
            this.BtnRemovePath.TabIndex = 11;
            this.BtnRemovePath.Text = "Remover";
            this.BtnRemovePath.TextAlign = System.Drawing.ContentAlignment.BottomCenter;
            this.BtnRemovePath.TextImageRelation = System.Windows.Forms.TextImageRelation.ImageAboveText;
            this.BtnRemovePath.UseVisualStyleBackColor = false;
            this.BtnRemovePath.Click += new System.EventHandler(this.remove_Click_1);
            // 
            // ComboBPieceSize
            // 
            this.ComboBPieceSize.FormattingEnabled = true;
            this.ComboBPieceSize.Items.AddRange(new object[] {
            "32KB",
            "64KB",
            "128KB",
            "256KB",
            "512KB",
            "1MB",
            "2MB",
            "4MB",
            "8MB",
            "16MB"});
            this.ComboBPieceSize.Location = new System.Drawing.Point(116, 253);
            this.ComboBPieceSize.Name = "ComboBPieceSize";
            this.ComboBPieceSize.Size = new System.Drawing.Size(127, 21);
            this.ComboBPieceSize.TabIndex = 8;
            this.ComboBPieceSize.SelectedIndexChanged += new System.EventHandler(this.ComboBPieceSize_SelectedIndexChanged);
            // 
            // label3
            // 
            this.label3.AutoSize = true;
            this.label3.Location = new System.Drawing.Point(5, 256);
            this.label3.Name = "label3";
            this.label3.Size = new System.Drawing.Size(97, 13);
            this.label3.TabIndex = 9;
            this.label3.Text = "Tamanho da peça:";
            // 
            // CheckBPieceSizeAuto
            // 
            this.CheckBPieceSizeAuto.AutoSize = true;
            this.CheckBPieceSizeAuto.Location = new System.Drawing.Point(116, 279);
            this.CheckBPieceSizeAuto.Name = "CheckBPieceSizeAuto";
            this.CheckBPieceSizeAuto.Size = new System.Drawing.Size(79, 17);
            this.CheckBPieceSizeAuto.TabIndex = 7;
            this.CheckBPieceSizeAuto.Text = "Automático";
            this.CheckBPieceSizeAuto.UseVisualStyleBackColor = true;
            this.CheckBPieceSizeAuto.CheckedChanged += new System.EventHandler(this.checkBox1_CheckedChanged);
            // 
            // BtnSelectFolder
            // 
            this.BtnSelectFolder.BackColor = System.Drawing.SystemColors.Control;
            this.BtnSelectFolder.ForeColor = System.Drawing.SystemColors.ControlText;
            this.BtnSelectFolder.Image = global::TorrentBuilder_Simplified.Properties.Resources.add_folder_24;
            this.BtnSelectFolder.Location = new System.Drawing.Point(30, 104);
            this.BtnSelectFolder.Name = "BtnSelectFolder";
            this.BtnSelectFolder.Size = new System.Drawing.Size(58, 59);
            this.BtnSelectFolder.TabIndex = 2;
            this.BtnSelectFolder.Text = "Pasta";
            this.BtnSelectFolder.TextAlign = System.Drawing.ContentAlignment.BottomCenter;
            this.BtnSelectFolder.TextImageRelation = System.Windows.Forms.TextImageRelation.ImageAboveText;
            this.BtnSelectFolder.UseVisualStyleBackColor = false;
            this.BtnSelectFolder.Click += new System.EventHandler(this.selectfolder_Click);
            // 
            // BtnSelectFile
            // 
            this.BtnSelectFile.BackColor = System.Drawing.SystemColors.Control;
            this.BtnSelectFile.Image = global::TorrentBuilder_Simplified.Properties.Resources.add_file_24;
            this.BtnSelectFile.Location = new System.Drawing.Point(30, 34);
            this.BtnSelectFile.Name = "BtnSelectFile";
            this.BtnSelectFile.Size = new System.Drawing.Size(58, 59);
            this.BtnSelectFile.TabIndex = 0;
            this.BtnSelectFile.Text = "Arquivo";
            this.BtnSelectFile.TextAlign = System.Drawing.ContentAlignment.BottomCenter;
            this.BtnSelectFile.TextImageRelation = System.Windows.Forms.TextImageRelation.ImageAboveText;
            this.BtnSelectFile.UseVisualStyleBackColor = false;
            this.BtnSelectFile.Click += new System.EventHandler(this.selectfile_Click);
            // 
            // TorrentBuild
            // 
            this.AutoScaleDimensions = new System.Drawing.SizeF(6F, 13F);
            this.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font;
            this.ClientSize = new System.Drawing.Size(531, 404);
            this.Controls.Add(this.MainPanel);
            this.FormBorderStyle = System.Windows.Forms.FormBorderStyle.Fixed3D;
            this.MaximizeBox = false;
            this.Name = "TorrentBuild";
            this.Text = "TorrentBuild Simplificado";
            this.Load += new System.EventHandler(this.Form1_Load);
            this.MainPanel.ResumeLayout(false);
            this.MainPanel.PerformLayout();
            this.groupBox1.ResumeLayout(false);
            this.groupBox1.PerformLayout();
            this.ResumeLayout(false);
            this.PerformLayout();

        }

        #endregion

        private System.Windows.Forms.Panel MainPanel;
        private System.Windows.Forms.Button BtnSelectFolder;
        private System.Windows.Forms.Button BtnSelectFile;
        private System.Windows.Forms.OpenFileDialog BrowseForFile;
        private System.Windows.Forms.FolderBrowserDialog BrowseForFolder;
        private System.Windows.Forms.ProgressBar PgrsTorrent;
        private System.Windows.Forms.Button BtnBuildTorrentNow;
        private System.Windows.Forms.Label label2;
        private System.Windows.Forms.ComboBox ComboBPieceSize;
        private System.Windows.Forms.CheckBox CheckBPieceSizeAuto;
        private System.Windows.Forms.Label label3;
        private System.Windows.Forms.GroupBox groupBox1;
        private System.Windows.Forms.Button BtnRemovePath;
        private System.Windows.Forms.CheckedListBox ListBox_Paths;
    }
}

