using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Diagnostics;
using System.IO;

namespace RunRanorexTest
{
    class Program
    {
        static void Main(string[] args)
        {
            //Start VM
            Process VBox = new Process();
            VBox.StartInfo.WindowStyle = System.Diagnostics.ProcessWindowStyle.Hidden;
            VBox.StartInfo.FileName = @"C:\Program Files\Oracle\VirtualBox\VBoxManage.exe";
            VBox.StartInfo.Arguments = "startvm \"Windows-8-64bit\"";
            VBox.Start();            
            VBox.WaitForExit();
            
            
            //Save Login information
            Process SaveKey = new Process();
            SaveKey.StartInfo.WindowStyle = System.Diagnostics.ProcessWindowStyle.Hidden;
            SaveKey.StartInfo.FileName = @"C:\Windows\System32\cmdkey.exe";
            SaveKey.StartInfo.Arguments = "/generic:RanorexTestVM /user:Matt /password:opto22qa";
            SaveKey.Start();
            SaveKey.WaitForExit();
          
            
            //RDP Session
            Process RDPSession = new Process();
            //RDPSession.StartInfo.WindowStyle = System.Diagnostics.ProcessWindowStyle.Hidden;
            RDPSession.StartInfo.FileName = @"C:\Windows\System32\cmd.exe";
            RDPSession.StartInfo.Arguments = "mstsc /v:RanorexTestVM";
            RDPSession.Start();
           
            


       }             
                       
    }
}
