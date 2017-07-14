using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using Qiniu.Util;
using Qiniu.IO;
using Qiniu.IO.Model;
using System.IO;

namespace UploadForQiniu {
    class HtmlExport {
        static string KImgTemp = @"<img src='{0}' width='{1}' height='{2}' />";
        static string KSpanTemp = @"<span>{0}</span><br/>{1}";
        static string kTemplate = @"
<!DOCTYPE html>
<html>
    <head>
    	<meta charset='utf-8'>
    	<title>上传内容</title>
    </head>
    <body>
        <div>
{0}
        </div>
    </body>
</html>";

        public static void Export(List<string> urlList, string filePath) {
            if (urlList.Count == 0)
                return;

            StringBuilder sb = new StringBuilder("");
            foreach (string url in urlList) {
                sb.Append(string.Format(KImgTemp, url, Settings.Width, Settings.Height));
                sb.Append(string.Format(KSpanTemp, url, "\r\n"));
            }
            string content = string.Format(kTemplate, sb.ToString());
            Util.WriteFile(filePath, content, false);
        }

        public static void Record(List<string> urlList, string filePath, string time) {
            if (urlList.Count == 0)
                return;

            StringBuilder sb = new StringBuilder("");
            sb.Append(string.Format("--- \r\n### {0}\r\n", time));
            foreach (string url in urlList) {
                sb.Append(string.Format("- {0}\r\n", url));
            }
            Util.WriteFile(filePath, sb.ToString(), true);
        }
    }
}
