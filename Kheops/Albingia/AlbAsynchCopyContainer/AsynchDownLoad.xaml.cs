using System;
using System.ComponentModel;
using System.IO;
using System.Linq;
using System.Runtime.InteropServices;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Forms.Integration;
using System.Windows.Interop;
using CopyAsynchFiles;

namespace AlbAsynchCopyContainer
{
    /// <summary>
    /// Logique d'interaction pour Page1.xaml
    /// </summary>
    public partial class AsynchDownLoad : Page
    {
        [DefaultValue("//Files01/User$")]
        private static string DownLoadDirectory { get; set; }
        public AsynchDownLoad()
        {


          
            if (BrowserInteropHelper.IsBrowserHosted)
            {

                //var query = @"L:\Documents&prm=//Files01/Kheops/Dev/Temp/4315_0000_O/Reel/4315_0000_000_CG_1544ALOGO.pdf||//Files01/Kheops/Dev/Temp/4315_0000_O/Reel/4315_0000_000_CG_1544SLOGO.pdf";
                //OR
                //var query = "?prs=123456"
                //var query = System.Web.HttpUtility.UrlDecode(System.Web.HttpUtility.UrlDecode(BrowserInteropHelper.Source.Query));
                //if (string.IsNullOrEmpty(query) || query.ToLower().Contains("prm=no"))
                //{
                //    return;
                //}

                //if (query.Contains("?prs="))
                //{
                //    var hwndProcess = query.Replace("?prs=", string.Empty);
                //    long hwnd = 0;
                //    if (long.TryParse(hwndProcess, out hwnd))
                //    {
                //        File.WriteAllText(string.Format(@"c:\zbo\tst\{0}.txt", (new Random()).Next(9999)), "IN AV APPEL" + hwnd.ToString());
                //        CopyAsynchFiles.Business.KillProcess.KillWord(hwnd);
                //        File.WriteAllText(string.Format(@"c:\zbo\tst\{0}.txt", (new Random()).Next(9999)), "IN APAPPEL" + hwnd.ToString());
                //    }
                //    File.WriteAllText(string.Format(@"c:\zbo\tst\{0}.txt", (new Random()).Next(9999)), "AFTER APAPPEL" + hwnd.ToString());
                //    return;



                //}


            InitializeComponent();
            ShowsNavigationUI = false;
            var windowsFormsHost = new WindowsFormsHost();


           
               //var query = @"L:\Documents&prm=//Files01/Kheops/Dev/Temp/4315_0000_O/Reel/4315_0000_000_CG_1544ALOGO.pdf||//Files01/Kheops/Dev/Temp/4315_0000_O/Reel/4315_0000_000_CG_1544SLOGO.pdf";
               var query = System.Web.HttpUtility.UrlDecode(BrowserInteropHelper.Source.Query.Replace("?doc=", string.Empty));
               query = query.Replace("?doc=", string.Empty);
                
                if (string.IsNullOrEmpty(query) || query.ToLower().Contains("prm=no"))
                {

                    return;
                }

                var paramQuery = query.Split(new[] { "&prm=" }, StringSplitOptions.None).ToList();
                if (!paramQuery.Any())
                    return;
                var filesArray = paramQuery[1].Split(new[] { "||" }, StringSplitOptions.None);
                if (!filesArray.Any())
                {
                    return;
                }
               // MessageBox.Show(paramQuery.Count().ToString() + "** 55");
                DownLoadDirectory = string.IsNullOrEmpty(paramQuery[0])
                    ? DownLoadDirectory
                    : paramQuery[0];

                if (!paramQuery.Any())
                        return;

                 //var kheopsCopyFiles =
                 //   new CopyFiles.AlbAsynchCopy(filesArray.ToList(), DownLoadDirectory);
               
                var kheopsCopyFiles =
                new AlbCopyAsynchFiles(DownLoadDirectory,filesArray.ToList());
                    kheopsCopyFiles.TopLevel = false;
                    windowsFormsHost.Child = kheopsCopyFiles;
                    stackPanel.Children.Add(windowsFormsHost);
                
            }



        }

         
    }
}
