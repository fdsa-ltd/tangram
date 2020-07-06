using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Text;
using System.Windows.Forms;

namespace Tangram.Core
{
    public class FileManager
    {

        static FileManager singlton;

        readonly StreamWriter logWriter;

        readonly StreamWriter pidWriter;
        private FileManager()
        {
            var path = Path.Combine(Application.StartupPath, "log.txt");

            var fileInfo = new FileInfo(path);
            if (fileInfo.Exists && fileInfo.Length > 1024 * 1024 * 1)
            {
                fileInfo.MoveTo(Path.Combine(Application.StartupPath, "log." + DateTime.Now.TimeOfDay.ToString() + ".txt"));
            }
            FileStream logFileStream;
            if (!fileInfo.Exists)
            {
                logFileStream = fileInfo.Create();
            }
            else
            {
                logFileStream = fileInfo.OpenWrite();
            }
            this.logWriter = new StreamWriter(logFileStream);
            this.logWriter.BaseStream.Seek(0, SeekOrigin.End);


            path = Path.Combine(Application.StartupPath, "pid.txt");

            fileInfo = new FileInfo(path);
            FileStream pidFileStream;
            if (!fileInfo.Exists)
            {
                pidFileStream = fileInfo.Create();
            }
            else
            {
                pidFileStream = fileInfo.OpenWrite();
            }
            this.pidWriter = new StreamWriter(pidFileStream);
            this.pidWriter.BaseStream.Seek(0, SeekOrigin.End);

        }


        public static FileManager Loger
        {
            get
            {
                if (singlton == null)
                {
                    singlton = new FileManager();
                }
                return singlton;
            }
        }
        public void WriteLog(string title, Exception ex)
        {
            this.logWriter.Write("时间：" + DateTime.Now.ToString("yyyy-MM-dd HH:mm:ss") + "\r\n");
            this.logWriter.Write("标题：" + title + "\r\n");
            this.logWriter.Write("消息：" + ex.ToString() + "\r\n");
        }

        public void WritePid(int pid)
        {
            this.pidWriter.WriteLine(pid);
            this.pidWriter.Flush();
        } 
    }
}