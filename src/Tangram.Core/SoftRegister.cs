using System;
using System.Collections.Generic;
using System.Linq;
using System.IO; 
using System.Threading;
using Microsoft.Win32;
using System.Windows.Forms;

namespace Tangram.Core
{
    public class SoftRegister
    {
        public static bool IsRunning()
        {
            bool firstRun;
            Mutex run = new Mutex(true, "TG", out firstRun);
            return !firstRun;
        }
        public static bool IsRegister()
        {
            var sn = ReadSetting("SerialNumber", "xxxx-xxxx-xxxx-xxxx-yyyy-yyyy-yyyy-yyyy");
            var sns = sn.Split('-');
            var code = GetMachineCode();
            var body = string.Join("-", sns.Take(4)) + "-" + code;
            if (!sn.EndsWith(body.CheckSum().Fragment(4)))
            {
                return false;
            }
            var expried = Convert.ToInt64(string.Join(string.Empty, sns.Take(4)), 16);
            if (DateTime.Now.Ticks > expried)
            {
                return false;
            }
            return true;
        }
        public static string CreatSerialNumber(string code, int hours)
        {
            var expried = DateTime.Now.AddHours(hours).Ticks;
            var body = Convert.ToString(expried, 16).ToUpper();

            while (body.Length < 16)
            {
                body = "0" + body;
            }

            var checkSum = (body.Fragment(4) + "-" + code).CheckSum();
            var sn = body + checkSum;
            return sn.Fragment(4);
        }

        public static string GetMachineCode()
        {
            var path = Path.Combine(Application.StartupPath, "sn.txt");
            if (File.Exists(path))
            {
                return File.ReadAllText(path);
            }
            List<string> vs = new List<string>();
            try
            {
                //using (ManagementClass pcManager = new ManagementClass("Win32_Processor"))
                //{
                //    using (ManagementObjectCollection collections = pcManager.GetInstances())
                //    {
                //        foreach (ManagementObject mo in collections)
                //        {
                //            vs.Add(mo.Properties["ProcessorId"].Value.ToString());
                //            mo.Dispose();
                //            break;
                //        }
                //    }
                //}

            }
            catch { }
            try
            {
                //using (ManagementClass pcManager = new ManagementClass("win32_logicaldisk"))
                //{
                //    using (ManagementObjectCollection collections = pcManager.GetInstances())
                //    {
                //        foreach (ManagementObject mo in collections)
                //        {
                //            if (mo["DeviceID"].ToString() == "C:")
                //            {
                //                vs.Add(mo.Properties["VolumeSerialNumber"].Value.ToString());
                //                mo.Dispose();
                //                break;
                //            }
                //        }
                //    }
                //}

            }
            catch { }
            try
            {
                //using (ManagementClass pcManager = new ManagementClass("Win32_NetworkAdapterConfiguration"))
                //{
                //    using (ManagementObjectCollection collections = pcManager.GetInstances())
                //    {
                //        foreach (ManagementObject mo in collections)
                //        {
                //            if ((bool)mo["IPEnabled"])
                //            {
                //                vs.Add(mo.Properties["MacAddress"].Value.ToString());
                //                mo.Dispose();
                //                break;
                //            }
                //        }
                //    }
                //}
            }
            catch { }

            return string.Join("-", vs).CheckSum().Fragment(4);
        }

        /*写入注册表*/
        public static void WriteSetting(string Key, string Setting)
        {
            var registryKey = Registry.CurrentUser.OpenSubKey("Software", true).CreateSubKey("Tangram").CreateSubKey("Screen");//注册另外写法CreateSubKey("Software\\Tangram\\Screen");            
            if (registryKey == null)
            {
                //Log.WriteLog("注册表路径失败", "请联系管理员");
                return;
            }
            try
            {
                registryKey.SetValue(Key, Setting);
            }
            catch (Exception ex)
            {
                FileManager.Loger.WriteLog("写入注册表失败",   ex );
            }
            finally
            {
                registryKey.Close();
            }
        }

        /*读取注册表*/
        static string ReadSetting(string Key, string Default)
        {
            try
            {
                using (var registryKey = Registry.CurrentUser.OpenSubKey("Software", true).CreateSubKey("Tangram").CreateSubKey("Screen"))
                {
                    if (registryKey == null)
                    {
                        return Default;
                    }
                    var registryValue = registryKey.GetValue(Key, Default);
                    return registryValue.ToString();
                }
            }
            catch
            {
                return Default;
            }

        }
    }
}
