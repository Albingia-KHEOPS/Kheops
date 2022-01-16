using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Diagnostics;
using System.IO;
using System.Linq;
using System.Printing;
using System.Windows.Forms;
using CopyAsynchFiles.Business;
using Outlook = Microsoft.Office.Interop.Outlook;

namespace CopyAsynchFiles
{
    public partial class AlbCopyAsynchFiles : Form
    {
        #region variables membres

        private readonly List<string> _filesToCopy;
        private readonly string _filesDestination;
        private List<string> _filesToCopyList;
        private string _defaultPrinter;

        Outlook.Application _objOutlookApp = null;
        Outlook.MailItem _objOutlookMail = null;

        #endregion

        #region Constructeurs

        public AlbCopyAsynchFiles()
        {
            InitializeComponent();
            // _filesToCopy = new List<string>();
            // _filesToCopy.Add(@"//files01/kheops/Qualif/temp/RS1600164_0000_P/Reel/RS1600164_0000_000_ANN_CS_1_8760ALOGO.pdf");
            // _filesToCopy.Add(@"//files01/kheops/Qualif/temp/RS1600164_0000_P/Reel/RS1600164_0000_000_CG_COMMU_1_8755ALOGO.pdf");
            // _filesToCopy.Add(@"//files01/kheops/Qualif/temp/RS1600164_0000_P/Reel/RS1600164_0000_000_CP_RS_50_8758.pdf");
            // InitializeComponent();
            // InitComboPrinters();
            // InitComboNbrCopies();
            //// _filesToCopy = filesToCopy;
            // _filesDestination = @"c:\zbo";
            // FormBorderStyle = FormBorderStyle.None;

            // AddFileToList();
            // try
            // {
            //     CopyFiles();
            // }
            // catch (Exception)
            // {
            // }

        }
        public AlbCopyAsynchFiles(string filesDestination = "", List<string> filesToCopy = null)
        {

            InitializeComponent();
            InitComboPrinters();
            InitComboNbrCopies();
            _filesToCopy = filesToCopy;
            _filesDestination = filesDestination;
            FormBorderStyle = FormBorderStyle.None;
            if (string.IsNullOrEmpty(filesDestination) || filesToCopy == null)
                return;
            AddFileToList();
            try
            {
                CopyFiles();
            }
            catch (Exception)
            {
            }

        }
        #endregion

        #region Méthodes privées

        private void CopyFiles()
        {
            if (!Directory.Exists(_filesDestination))
            {
                Directory.CreateDirectory(_filesDestination);
            }
            // MessageBox.Show(" _filesToCopyList.Count:" + _filesToCopyList.Count);
            bool errorPrint = false;

            for (int i = 0; i < _filesToCopyList.Count; i++)
            {

                var fileNamePrior = i <= 1
                    ? string.Empty
                    : PrintService.GetName(_filesToCopyList[i - 1]);
                string precFile = (i == 0) ? string.Empty : fileNamePrior;

                if (i == _filesToCopy.Count && !errorPrint)
                {
                    UpdateProgressBar((i) * 100 / _filesToCopy.Count, string.Empty, fileNamePrior, true);
                    break;
                }
                if (i == 1 && !errorPrint)
                {
                    UpdateProgressBar((i + 1) * 100 / _filesToCopy.Count, string.Empty, precFile, true);
                }
                if (i == 1 && errorPrint)
                    break;
                var fileNameDesc = PrintService.GetName(_filesToCopy[i]);
                try
                {
                    //File.Copy(_filesToCopy[i], _filesDestination + "\\" + fileNameDesc, true);
                    File.Copy(_filesToCopy[i].Replace("/", "\\"), _filesDestination + "\\" + fileNameDesc, true);

                    UpdateProgressBar((i + 1) * 100 / _filesToCopy.Count, precFile, fileNameDesc);
                }
                catch
                {
                    AddPictureStaeToList(fileNameDesc, 2);
                    errorPrint = true;

                }
            }
            if (_filesToCopyList.Any() && !errorPrint)
            {
                UpdateProgressBar(100, string.Empty, PrintService.GetName(_filesToCopyList.Last()), true);

            }
        }

        private void UpdateProgressBar(int currentValue, string filePrec = "", string fileCourant = "",
            bool forceItemOk = false)
        {

            if (forceItemOk)
            {
                AddPictureStaeToList(fileCourant, 0);

                return;
            }
            if (!string.IsNullOrEmpty(fileCourant))
            {
                if (!string.IsNullOrEmpty(filePrec))
                {
                    AddPictureStaeToList(filePrec, 0);
                    AddPictureStaeToList(fileCourant, 1);
                }
                else
                {
                    AddPictureStaeToList(fileCourant, 1);
                }

                // lblFile.Text = string.Format("Impression du fichier {0} encours ...", fileCourant);



            }
        }

        private void AddFileToList()
        {
            lstFilePrint.CheckBoxes = true;
            if (_filesToCopy == null)
                return;
            int i = 0;
            foreach (var file in _filesToCopy)
            {


                var fileName = PrintService.GetName(file);



                var lstItm0 = new ListViewItem
                {
                    Name = fileName,
                    IndentCount = 0,
                };

                var lstItm1 = new ListViewItem.ListViewSubItem
                {
                    Name = "sub" + fileName,
                    Text = fileName
                };



                lstItm0.SubItems.Add(lstItm1);
                lstFilePrint.Items.Add(lstItm0);


                if (string.IsNullOrEmpty(file))
                {
                    AddPictureStaeToList(fileName, 2, "Chemin introuvable");

                }
                if (string.IsNullOrEmpty(fileName))
                {
                    AddPictureStaeToList(fileName, 2, " Nom de fichier est vide");
                }
                if (!File.Exists(file))
                {
                    AddPictureStaeToList(fileName, 2, "Fichier à traiter est introuvable");
                }


                if (_filesToCopyList == null)
                {
                    _filesToCopyList = new List<string>();
                }
                _filesToCopyList.Add(file);

                i++;

            }
        }

        private void AddPictureStaeToList(string fileName, int indexPicture, string messageIco = "")
        {
            var item = lstFilePrint.FindItemWithText(fileName);
            if (item == null)
                return;
            item.ImageIndex = indexPicture;
            item.ToolTipText = messageIco;
        }

        private void lstFilePrint_DoubleClick(object sender, EventArgs e)
        {
            if (lstFilePrint.SelectedItems.Count == 0) return;
            if (lstFilePrint.SelectedItems[0].ImageIndex == 0)
            {
                OpenFile(lstFilePrint.SelectedItems[0].Name);
            }
        }

        private void lnkDestinyDirectory_LinkClicked(object sender, LinkLabelLinkClickedEventArgs e)
        {
            Process.Start("explorer.exe", _filesDestination);
        }

        private void OpenFile(string fileName)
        {

            Process.Start(string.Format("{0}/{1}", _filesDestination, fileName));
        }

        private void InitComboPrinters()
        {
            var allPrinterSettings = PrintService.GetAllInstalledPrinters();
            if (allPrinterSettings == null)
            {
                return;
            }
            var selectedPrinter = string.Empty;
            foreach (var printer in allPrinterSettings)
            {
                cmbPrinters.Items.Add(printer);
                if (printer.ToLower().Contains("xeroxkheops"))
                {
                    selectedPrinter = printer;
                    break;
                }
            }

            if (string.IsNullOrEmpty(selectedPrinter))
            {
                using (var printServer = new LocalPrintServer())
                {
                    _defaultPrinter = GetDefaultPrinter();
                    selectedPrinter = _defaultPrinter;
                }
            }

            cmbPrinters.SelectedItem = selectedPrinter;

        }

        private void InitComboNbrCopies()
        {
            for (var i = 1; i <= 10; i++)
            {
                cmbNbrCopie.Items.Add(i);
            }


        }

        private string GetDefaultPrinter()
        {
            using (var printServer = new LocalPrintServer())
            {
                return printServer.DefaultPrintQueue.FullName;
            }
        }

        public static void SetDefaultPrinter(string printername)
        {
            var type = Type.GetTypeFromProgID("WScript.Network");
            var instance = Activator.CreateInstance(type);
            type.InvokeMember("SetDefaultPrinter", System.Reflection.BindingFlags.InvokeMethod, null, instance,
                new object[] { printername });
        }

        private void PrintSelectedFiles()
        {
            for (int i = 0; i < lstFilePrint.Items.Count; i++)
            {
                var item = lstFilePrint.Items[i];
                string fullName = _filesToCopy.FirstOrDefault(el => el.Contains(item.Name));
                if (string.IsNullOrEmpty(fullName))
                    continue;
                if (item.ImageIndex == 0)
                {
                    if (!chkPrintAll.Checked)
                    {
                        if (item.Checked)
                        {
                            //TODO Print File
                            //listFileToPrint.Add(item.Name);
                            PrintService.PrintFile(fullName, cmbPrinters.SelectedItem.ToString());
                        }
                    }
                    else
                    {
                        //TODO Print File
                        //listFileToPrint.Add(item.Name);
                        PrintService.PrintFile(fullName, cmbPrinters.SelectedItem.ToString());
                    }
                }
            }
        }

        private void chkPrintAll_CheckStateChanged(object sender, EventArgs e)
        {
            for (int i = 0; i < lstFilePrint.Items.Count; i++)
            {
                var item = lstFilePrint.Items[i];
                item.Checked = chkPrintAll.Checked;
            }
        }

        #endregion

        private void btnImp_Click(object sender, EventArgs e)
        {
            _defaultPrinter = GetDefaultPrinter();
            try
            {
                if (lstFilePrint.CheckedItems.Count <= 0 && !chkPrintAll.Checked)
                {
                    MessageBox.Show("Aucun document sélectionné", "Kheops Production", MessageBoxButtons.OK,
                       MessageBoxIcon.Error);
                    return;
                }

                SetDefaultPrinter(cmbPrinters.SelectedItem.ToString());
                var nbrCopie = int.Parse(cmbNbrCopie.SelectedItem.ToString());
                for (int j = 1; j <= nbrCopie; j++)
                {
                    PrintSelectedFiles();
                }
            }
            catch (Exception)
            {

                SetDefaultPrinter(_defaultPrinter);
            }
            SetDefaultPrinter(_defaultPrinter);
        }
        private void lstFilePrint_ItemCheck(object sender, ItemCheckEventArgs e)
        {
            if (lstFilePrint.Items.Count > 0 && lstFilePrint.Items[e.Index].ImageIndex != 0)
            {
                lstFilePrint.Items[e.Index].Checked = false;
            }
        }
        private void btnOutLook_Click(object sender, EventArgs e)
        {
            if (lstFilePrint.CheckedItems.Count <= 0)
            {
                MessageBox.Show("Aucun document sélectionné", "Kheops Production", MessageBoxButtons.OK,
                 MessageBoxIcon.Error);
                return;
            }

            try
            {
                _objOutlookApp = new Outlook.Application();
                _objOutlookMail = (Outlook.MailItem)_objOutlookApp.CreateItem(Outlook.OlItemType.olMailItem);
            }
            catch (Exception)
            {
                throw new Exception("Erreur d'inisialisation Outlook");
            }
            try
            {
                for (int i = 0; i < lstFilePrint.Items.Count; i++)
                {

                    var item = lstFilePrint.Items[i];
                    string fullName = _filesToCopy.FirstOrDefault(el => el.Contains(item.Name)).Replace("/", "\\");
                    if (string.IsNullOrEmpty(fullName))
                        continue;



                    if (item.ImageIndex == 0)
                    {

                        if (!chkPrintAll.Checked)
                        {

                            if (item.Checked)
                            {


                        _objOutlookMail.Attachments.Add((object)fullName,
                       Outlook.OlAttachmentType.olEmbeddeditem, 1, (object)item.Name);
                            }
                        }
                        else
                        {

                            _objOutlookMail.Attachments.Add((object)fullName,
                   Outlook.OlAttachmentType.olEmbeddeditem, 1, (object)item.Name);


                        }
                    }
                }
                _objOutlookMail.Display(true);




            }
            catch (Exception exp)
            {
                MessageBox.Show(exp.StackTrace);
            }
        }
    }

}

