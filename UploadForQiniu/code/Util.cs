using System;
using System.IO;
using System.Text;

namespace UploadForQiniu {
   public class Util {
        public static void Log(string format, params object[] args) {
            Console.WriteLine(format, args);
        }

        public static string ReadFile(string filePath) {
            return "";
        }


        public static void WriteFile(string filePath, string content, bool isAppend) {
            try {
                if (!File.Exists(filePath)) {
                    using (FileStream fs = File.Create(filePath)) {
                    }
                }

                if (isAppend) { //追加
                    File.AppendAllText(filePath, content, Encoding.UTF8);
                } else { // 覆盖
                    FileStream fs = new FileStream(filePath, FileMode.OpenOrCreate);
                    StreamWriter sw = new StreamWriter(fs);
                    sw.Write(content);
                    sw.Flush();
                    sw.Close();
                    fs.Close();
                }
            } catch (Exception e) {
                Util.Log("--- WriteFile, error:\n{0}", e.Message);
            }
        }

    }
}
