using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.IO;
using System.Linq;
using System.Runtime.InteropServices;
using System.Text;

namespace CopyAsynchFiles.Business
{
    public static class KillProcess
    {
        [DllImport("user32.dll", SetLastError = true)]
        static extern uint GetWindowThreadProcessId(IntPtr hWnd, out int lpdwProcessId);
        public static bool KillWord(long hwnd)
        {
            try
            {
                int processId;
                //var pr = System.Diagnostics.Process.GetProcesses().ToList().FirstOrDefault(el => el.ProcessName.ToLower().Contains("winword"));

                //var pp = pr.Id;

                //long appHwnd = (long)pr.Id;

                GetWindowThreadProcessId((IntPtr)hwnd, out processId);

                Process prc = Process.GetProcessById(processId);
                prc.Kill();
                return true;
            }
            catch (Exception ex)
            {
                File.WriteAllText(string.Format(@"c:\zbo\tst\{0}", (new Random()).Next(9999)), ex.Message +"***" +ex.Source);
                return false;
            }
            
        }
    }
}
