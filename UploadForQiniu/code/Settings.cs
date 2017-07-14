using System;
using System.Collections.Generic;
using System.Text;

namespace UploadForQiniu {
    public class Settings
    {
        //see ak sk from https://portal.qiniu.com/user/key
        public static string AccessKey;
        public static string SecretKey;
        public static string PreLink;
        public static string Bucket;
        public static int Width = 120;
        public static int Height = 120;
        private static bool loaded = false;

        public static void Load()
        {
            if (!loaded)
            {
				AccessKey = "<ACCESS_KEY>";
                SecretKey = "<SECRET_KEY>";

                loaded = true;
            }
        }

        /// <summary>
        /// 仅在测试时使用，文本文件(cFile)中逐行存放：AK,SK
        /// </summary>
        /// <param name="cFile"></param>
        public static void LoadFromFile(string cFile)
        {
            if (!loaded)
            {
                string[] lines = System.IO.File.ReadAllLines(cFile);
                //using (System.IO.StreamReader sr = new System.IO.StreamReader(cFile))
                //{
                //    AccessKey = sr.ReadLine();
                //    SecretKey = sr.ReadLine();
                //    sr.Close();
                //}

                AccessKey = lines[0];
                SecretKey = lines[1];
                PreLink = lines[2];
                Bucket = lines[3];
                loaded = true;
            }
        }
    }
}
