namespace CopyAsynchFiles
{
    partial class AlbCopyAsynchFiles
    {
        /// <summary>
        /// Variable nécessaire au concepteur.
        /// </summary>
        private System.ComponentModel.IContainer components = null;

        /// <summary>
        /// Nettoyage des ressources utilisées.
        /// </summary>
        /// <param name="disposing">true si les ressources managées doivent être supprimées ; sinon, false.</param>
        protected override void Dispose(bool disposing)
        {
            if (disposing && (components != null))
            {
                components.Dispose();
            }
            base.Dispose(disposing);
        }

        #region Code généré par le Concepteur Windows Form

        /// <summary>
        /// Méthode requise pour la prise en charge du concepteur - ne modifiez pas
        /// le contenu de cette méthode avec l'éditeur de code.
        /// </summary>
        private void InitializeComponent()
        {
            this.components = new System.ComponentModel.Container();
            System.ComponentModel.ComponentResourceManager resources = new System.ComponentModel.ComponentResourceManager(typeof(AlbCopyAsynchFiles));
            this.lstFilePrint = new System.Windows.Forms.ListView();
            this.state = ((System.Windows.Forms.ColumnHeader)(new System.Windows.Forms.ColumnHeader()));
            this.FileName = ((System.Windows.Forms.ColumnHeader)(new System.Windows.Forms.ColumnHeader()));
            this.imgList = new System.Windows.Forms.ImageList(this.components);
            this.lnkDestinyDirectory = new System.Windows.Forms.LinkLabel();
            this.chkPrintAll = new System.Windows.Forms.CheckBox();
            this.btnImp = new System.Windows.Forms.Button();
            this.cmbPrinters = new System.Windows.Forms.ComboBox();
            this.btnOutLook = new System.Windows.Forms.Button();
            this.cmbNbrCopie = new System.Windows.Forms.ComboBox();
            this.NbCopies = new System.Windows.Forms.Label();
            this.SuspendLayout();
            // 
            // lstFilePrint
            // 
            this.lstFilePrint.BackColor = System.Drawing.Color.WhiteSmoke;
            this.lstFilePrint.Columns.AddRange(new System.Windows.Forms.ColumnHeader[] {
            this.state,
            this.FileName});
            this.lstFilePrint.Font = new System.Drawing.Font("Microsoft Sans Serif", 7.8F, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.lstFilePrint.ForeColor = System.Drawing.SystemColors.WindowText;
            this.lstFilePrint.FullRowSelect = true;
            this.lstFilePrint.LabelEdit = true;
            this.lstFilePrint.LargeImageList = this.imgList;
            this.lstFilePrint.Location = new System.Drawing.Point(22, 98);
            this.lstFilePrint.Margin = new System.Windows.Forms.Padding(4, 3, 4, 3);
            this.lstFilePrint.MultiSelect = false;
            this.lstFilePrint.Name = "lstFilePrint";
            this.lstFilePrint.ShowItemToolTips = true;
            this.lstFilePrint.Size = new System.Drawing.Size(760, 662);
            this.lstFilePrint.SmallImageList = this.imgList;
            this.lstFilePrint.TabIndex = 13;
            this.lstFilePrint.UseCompatibleStateImageBehavior = false;
            this.lstFilePrint.View = System.Windows.Forms.View.Details;
            this.lstFilePrint.ItemCheck += new System.Windows.Forms.ItemCheckEventHandler(this.lstFilePrint_ItemCheck);
            this.lstFilePrint.DoubleClick += new System.EventHandler(this.lstFilePrint_DoubleClick);
            // 
            // state
            // 
            this.state.DisplayIndex = 1;
            this.state.Text = "Etat";
            this.state.Width = 50;
            // 
            // FileName
            // 
            this.FileName.DisplayIndex = 0;
            this.FileName.Text = "Nom du fichier";
            this.FileName.Width = 325;
            // 
            // imgList
            // 
            this.imgList.ImageStream = ((System.Windows.Forms.ImageListStreamer)(resources.GetObject("imgList.ImageStream")));
            this.imgList.TransparentColor = System.Drawing.Color.Transparent;
            this.imgList.Images.SetKeyName(0, "ok.png");
            this.imgList.Images.SetKeyName(1, "wait.gif");
            this.imgList.Images.SetKeyName(2, "ko.png");
            this.imgList.Images.SetKeyName(3, "16_print.gif");
            this.imgList.Images.SetKeyName(4, "Outlook-icon.png");
            // 
            // lnkDestinyDirectory
            // 
            this.lnkDestinyDirectory.AutoSize = true;
            this.lnkDestinyDirectory.Location = new System.Drawing.Point(428, 17);
            this.lnkDestinyDirectory.Margin = new System.Windows.Forms.Padding(6, 0, 6, 0);
            this.lnkDestinyDirectory.Name = "lnkDestinyDirectory";
            this.lnkDestinyDirectory.Size = new System.Drawing.Size(338, 25);
            this.lnkDestinyDirectory.TabIndex = 14;
            this.lnkDestinyDirectory.TabStop = true;
            this.lnkDestinyDirectory.Text = "Ouvrir le répertoire  de destination";
            this.lnkDestinyDirectory.LinkClicked += new System.Windows.Forms.LinkLabelLinkClickedEventHandler(this.lnkDestinyDirectory_LinkClicked);
            // 
            // chkPrintAll
            // 
            this.chkPrintAll.AutoSize = true;
            this.chkPrintAll.Font = new System.Drawing.Font("Microsoft Sans Serif", 8F, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.chkPrintAll.Location = new System.Drawing.Point(22, 59);
            this.chkPrintAll.Margin = new System.Windows.Forms.Padding(6);
            this.chkPrintAll.Name = "chkPrintAll";
            this.chkPrintAll.Size = new System.Drawing.Size(313, 30);
            this.chkPrintAll.TabIndex = 15;
            this.chkPrintAll.Text = "Imprimer tous les fichiers";
            this.chkPrintAll.UseVisualStyleBackColor = true;
            this.chkPrintAll.CheckStateChanged += new System.EventHandler(this.chkPrintAll_CheckStateChanged);
            // 
            // btnImp
            // 
            this.btnImp.ImageIndex = 3;
            this.btnImp.ImageList = this.imgList;
            this.btnImp.Location = new System.Drawing.Point(651, 52);
            this.btnImp.Margin = new System.Windows.Forms.Padding(6);
            this.btnImp.Name = "btnImp";
            this.btnImp.Size = new System.Drawing.Size(46, 44);
            this.btnImp.TabIndex = 16;
            this.btnImp.UseVisualStyleBackColor = true;
            this.btnImp.Click += new System.EventHandler(this.btnImp_Click);
            // 
            // cmbPrinters
            // 
            this.cmbPrinters.FormattingEnabled = true;
            this.cmbPrinters.Location = new System.Drawing.Point(22, 8);
            this.cmbPrinters.Margin = new System.Windows.Forms.Padding(6);
            this.cmbPrinters.Name = "cmbPrinters";
            this.cmbPrinters.Size = new System.Drawing.Size(390, 33);
            this.cmbPrinters.TabIndex = 17;
            // 
            // btnOutLook
            // 
            this.btnOutLook.ImageIndex = 4;
            this.btnOutLook.ImageList = this.imgList;
            this.btnOutLook.Location = new System.Drawing.Point(709, 52);
            this.btnOutLook.Margin = new System.Windows.Forms.Padding(6);
            this.btnOutLook.Name = "btnOutLook";
            this.btnOutLook.Size = new System.Drawing.Size(46, 44);
            this.btnOutLook.TabIndex = 18;
            this.btnOutLook.UseVisualStyleBackColor = true;
            this.btnOutLook.Click += new System.EventHandler(this.btnOutLook_Click);
            // 
            // cmbNbrCopie
            // 
            this.cmbNbrCopie.FormattingEnabled = true;
            this.cmbNbrCopie.Location = new System.Drawing.Point(568, 56);
            this.cmbNbrCopie.Margin = new System.Windows.Forms.Padding(6);
            this.cmbNbrCopie.Name = "cmbNbrCopie";
            this.cmbNbrCopie.Size = new System.Drawing.Size(71, 33);
            this.cmbNbrCopie.TabIndex = 19;
            // 
            // NbCopies
            // 
            this.NbCopies.AutoSize = true;
            this.NbCopies.Font = new System.Drawing.Font("Microsoft Sans Serif", 7.875F, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.NbCopies.Location = new System.Drawing.Point(354, 59);
            this.NbCopies.Name = "NbCopies";
            this.NbCopies.Size = new System.Drawing.Size(206, 25);
            this.NbCopies.TabIndex = 20;
            this.NbCopies.Text = "Nombre de Copies";
            // 
            // AlbCopyAsynchFiles
            // 
            this.AutoScaleDimensions = new System.Drawing.SizeF(12F, 25F);
            this.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font;
            this.BackColor = System.Drawing.Color.White;
            this.ClientSize = new System.Drawing.Size(804, 784);
            this.Controls.Add(this.NbCopies);
            this.Controls.Add(this.cmbNbrCopie);
            this.Controls.Add(this.btnOutLook);
            this.Controls.Add(this.cmbPrinters);
            this.Controls.Add(this.btnImp);
            this.Controls.Add(this.chkPrintAll);
            this.Controls.Add(this.lnkDestinyDirectory);
            this.Controls.Add(this.lstFilePrint);
            this.Margin = new System.Windows.Forms.Padding(6);
            this.Name = "AlbCopyAsynchFiles";
            this.Text = "Kheops- Copie de fichiers";
            this.ResumeLayout(false);
            this.PerformLayout();

        }

        #endregion

        private System.Windows.Forms.ListView lstFilePrint;
        private System.Windows.Forms.ColumnHeader state;
        private System.Windows.Forms.ColumnHeader FileName;
        private System.Windows.Forms.ImageList imgList;
        private System.Windows.Forms.LinkLabel lnkDestinyDirectory;
        private System.Windows.Forms.CheckBox chkPrintAll;
        private System.Windows.Forms.Button btnImp;
        private System.Windows.Forms.ComboBox cmbPrinters;
        private System.Windows.Forms.Button btnOutLook;
        private System.Windows.Forms.ComboBox cmbNbrCopie;
        private System.Windows.Forms.Label NbCopies;
    }
}

