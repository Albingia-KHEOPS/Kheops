using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.Drawing.Printing;
using System.IO;
using System.Linq;
//using System.Printing;
using System.Threading;
using System.Threading.Tasks;
using System.Windows.Forms;
//using ALBINGIA.Framework.Common.Models.FileModel;


namespace CopyAsynchFiles.Business
{
    public static class PrintService
    {
        #region Méthodes publiques

        public static string GetName(string fulleFileName)
        {
            var segments = fulleFileName.Split(new[] { "/" }, StringSplitOptions.None);
            return segments.Count() == 1 ? segments[0] : segments[segments.Count() - 1];
        }

        public static bool PrintSetFiles(List<string> filesInfoDescription, string user)
        {
            if (filesInfoDescription == null ||
                !filesInfoDescription.Any())
                return false;
            filesInfoDescription.ForEach(fileInfo =>
            {
                try
                {
                    Print(fileInfo, user);
                }
                catch (Exception)
                {
                    //TODO: ZBO -Trace d'errur d'impression
                    throw new Exception(string.Format("Erreur d'impression du fichier : {0}",
                        fileInfo));
                }

            });
            return true;
        }

        public static void PrintFile(string fileInfoDescription, string selectedPrinter = "")
        {
            if (fileInfoDescription == null)
            {
                throw new Exception(String.Format("Erreur d'impression"));
            }


            //try
            //{
           // MessageBox.Show("In Printfile - File:" + fileInfoDescription);
            //Print(fileInfoDescription, String.Empty, GetName(fileInfoDescription), selectedPrinter);

            Print(fileInfoDescription, String.Empty, GetName(fileInfoDescription), selectedPrinter);
            //}
            //catch
            //{
            //    throw;
            //    //TODO: ZBO -Trace d'errur d'impression
            //}
            //return true;
        }

        public static List<string> GetAllInstalledPrinters()
        {
            return PrinterSettings.InstalledPrinters.Cast<string>().ToList();
        }

        #endregion
        #region Méthodes privées
        private static void Print(string path, string user = "", string nomFichier = "", string selectedPrinter = "")
        {

          //  MessageBox.Show("Before Process");
            if (!File.Exists(path))
            {
                throw new Exception(String.Format("Le fichier {0} inexistant.", nomFichier));
            }


            //var xeroxPrinter = string.Empty;
            //string defaultPrinter;

            //foreach (string sPrinters in System.Drawing.Printing.PrinterSettings.InstalledPrinters)
            //{
            //    if (sPrinters.ToLower() == "xeroxprinters")
            //    {
            //        xeroxPrinter = sPrinters;
            //        break;
            //    }
            //   // print += "##***##" + sPrinters;
            //}

            //using (var printServer = new LocalPrintServer())
            //{
            //    defaultPrinter = printServer.DefaultPrintQueue.FullName;


            //}

            var printJob = new Process
            {
                StartInfo = new ProcessStartInfo
                {
                    FileName = path.Replace("/", "\\"),
                    UseShellExecute = true,
                    Verb = "print",
                    CreateNoWindow = false,
                    WindowStyle = ProcessWindowStyle.Hidden,
                    WorkingDirectory = Path.GetDirectoryName(path),
                    //Arguments = string.IsNullOrEmpty(xeroxPrinter)?defaultPrinter:xeroxPrinter   //"\\print02\xeroxprinters"
                    Arguments = selectedPrinter
                     //Arguments = String.Format("/s /o /h / p{0}", selectedPrinter),


                }
            };
          //  MessageBox.Show("After Process");
            //  var tst = AppDomain.CurrentDomain.ActivationContext.Identity;


            // File.WriteAllText(@"c:\work\TSTWINSERV.txt", "BEFORE START PRINT:" + path);
            var res = false;
            try
            {
                res = printJob.Start();
            }
            catch (Exception ex)
            {

                MessageBox.Show("Print Job Ex:" + ex.Message + "Stack:  " + ex.StackTrace);


            }

          //  MessageBox.Show("res:" + res.ToString());
          //  MessageBox.Show("After Lunch Print");
            long ticks = -1;
            while (ticks != printJob.TotalProcessorTime.Ticks)
            {
                ticks = printJob.TotalProcessorTime.Ticks;
                Thread.Sleep(1000);
            }
            if (printJob.HasExited == false)
            {
                printJob.Kill();
            }
            //File.WriteAllText(@"c:\work\TSTWINSERV.txt",
            //    "AFTER LUNCH PRINT:" + res.ToString() + "--printers--" + print);
        }

        #endregion
    }
}