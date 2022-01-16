using System;
using System.Collections;
using System.Collections.Generic;
using System.Net;
using System.Net.Mail;
using System.Net.Mime;

namespace Hexavia.Tools.Helpers
{
    /// <summary>
    /// Generic File Writer
    /// </summary>
    public class Logger
    {
     
        public string Path { get;}
        public Logger(string path)
        {          
                Path = path;
        }
        public void WriteLine(string text)
        {
            try
            {
                using (System.IO.StreamWriter writer = new System.IO.StreamWriter(Path, true, System.Text.Encoding.UTF8))
                {
                    if (!string.IsNullOrEmpty(text))
                    {
                        writer.WriteLine(text);
                    }
                }
            }
            catch
            {
                throw;
            }
        }
    }
}
