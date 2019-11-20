using System;
using System.IO;
using System.Text; 

namespace Tangram.Core
{
    public class Log
    {
        public static void WriteLog(string title,string msg)
        {
            //if (!Directory.Exists(Application.StartupPath + "\\Log\\"))
            //{
            //    Directory.CreateDirectory(Application.StartupPath + "\\Log\\");
            //}
            //string fname = System.Windows.Forms.Application.StartupPath + "\\Log\\Log.log";
            //bool fileExist = true;
            //FileInfo finfo = new FileInfo(fname);
            //if (!finfo.Exists)
            //{
            //    FileStream fs;
            //    fs = File.Create(fname);
            //    fs.Close();
            //    finfo = new FileInfo(fname);
            //    fileExist = false;
            //}

            //if (finfo.Length > 1024 * 1024 * 1)
            //{
            //    File.Move(System.Windows.Forms.Application.StartupPath + "\\Log\\Log.log", System.Windows.Forms.Application.StartupPath + DateTime.Now.TimeOfDay + "\\Log\\Log.log");
            //    finfo.Delete();
            //}

            //using (FileStream fs = finfo.OpenWrite())
            //{
            //    StreamWriter w = new StreamWriter(fs);
            //    w.BaseStream.Seek(0,SeekOrigin.End);
            //    if (fileExist)
            //    {
            //        w.Write("\r\n");
            //    }
            //    w.Write("时间："+DateTime.Now.ToString("yyyy-MM-dd HH:mm:ss") + "\r\n");
            //    w.Write("标题："+title + "\r\n");
            //    w.Write("消息：" + msg + "\r\n");
            //    w.Flush();
            //    w.Close();
            //}
        }
	}
}