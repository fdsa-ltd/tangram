using System;
using System.Collections;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Security.Cryptography;
using System.Text;

namespace Tangram.Core
{
    public class FileEncypt
    {
        private static byte[] Keys = { 0x12, 0x34, 0x56, 0x78, 0x90, 0xAB, 0xCD, 0xEF };//自定义密匙 

        /// <summary>   
        /// 文件加密   
        /// </summary>   
        /// <param name="inFile">文件储存路径</param>   
        /// <param name="outFile">储存文件复制的路径</param>   
        /// <param name="encryptKey"></param>   
        /// <returns></returns> 
        public static bool EncryptFile(string inFile, string outFile, string encryptKey)
        {
            byte[] rgb = Keys;
            try
            {
                byte[] rgbKeys = Encoding.UTF8.GetBytes(encryptKey.Substring(0, 8));
                FileStream inFs = new FileStream(inFile, FileMode.Open, FileAccess.Read);//读入流     
                FileStream outFs = new FileStream(outFile, FileMode.OpenOrCreate, FileAccess.Write);// 等待写入流     
                outFs.SetLength(0);//帮助读写的变量     
                byte[] byteIn = new byte[100];//放临时读入的流     
                long readLen = 0;//读入流的长度     
                long totalLen = inFs.Length;//读入流的总长度     
                int everylen = 0;//每次读入流的长度     
                DES des = new DESCryptoServiceProvider();//将inFile加密后放到outFile     
                CryptoStream encStream = new CryptoStream(outFs, des.CreateEncryptor(rgb, rgbKeys), CryptoStreamMode.Write);
                while (readLen < totalLen)
                {
                    everylen = inFs.Read(byteIn, 0, 100);
                    encStream.Write(byteIn, 0, everylen);
                    readLen = readLen + everylen;
                }
                encStream.Close();
                inFs.Close();
                outFs.Close();
                return true;
            }
            catch (Exception ex)
            {
                FileManager.Loger.WriteLog("文件加密失败", ex );
                return false;
            }
        }

        public static bool DecryptFile(string inFile, string outFile, string encryptKey)
        {
            byte[] rgb = Keys;
            try
            {
                byte[] rgbKeys = Encoding.UTF8.GetBytes(encryptKey.Substring(0, 8));
                FileStream inFs = new FileStream(inFile, FileMode.Open, FileAccess.Read);//读入流     
                FileStream outFs = new FileStream(outFile, FileMode.OpenOrCreate, FileAccess.Write);// 等待写入流     
                outFs.SetLength(0);//帮助读写的变量     
                byte[] byteIn = new byte[100];//放临时读入的流     
                long readLen = 0;//读入流的长度     
                long totalLen = inFs.Length;//读入流的总长度     
                int everylen = 0;//每次读入流的长度     
                DES des = new DESCryptoServiceProvider();//将inFile加密后放到outFile     
                CryptoStream encStream = new CryptoStream(outFs, des.CreateDecryptor(rgb, rgbKeys), CryptoStreamMode.Write);
                while (readLen < totalLen)
                {
                    everylen = inFs.Read(byteIn, 0, 100);
                    encStream.Write(byteIn, 0, everylen);
                    readLen = readLen + everylen;
                }
                encStream.Close();
                inFs.Close();
                outFs.Close();
                return true;
            }
            catch (Exception ex)
            {
                FileManager.Loger.WriteLog("文件解密失败", ex );
                return false;
            }
        }
        public static string DecryptFile(string inFile, string encryptKey)
        {
            byte[] rgb = Keys;
            try
            {
                byte[] rgbKeys = Encoding.UTF8.GetBytes(encryptKey.Substring(0, 8));
                FileStream inFs = new FileStream(inFile, FileMode.Open, FileAccess.Read);//读入流                
                byte[] buffer = File.ReadAllBytes(inFile);
                MemoryStream ms = new MemoryStream(buffer);
                byte[] byteIn = new byte[100];//放临时读入的流                
                long totalLen = inFs.Length;//读入流的总长度                
                DES des = new DESCryptoServiceProvider();//将inFile加密后放到outFile     
                CryptoStream encStream = new CryptoStream(ms, des.CreateDecryptor(rgb, rgbKeys), CryptoStreamMode.Read);
                StreamReader sReader = new StreamReader(encStream);
                string context = sReader.ReadToEnd(); ;
                sReader.Close();
                encStream.Close();
                ms.Close();
                return context;
            }
            catch (Exception ex)
            {
                FileManager.Loger.WriteLog("文件解密失败", ex);
                return string.Empty;
            }
        }
    }
}
