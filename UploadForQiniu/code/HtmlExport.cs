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
        static string KImgTemp = @"<img src='{0}' height='{1}' />&nbsp;&nbsp;";
        static string KSpanTemp = @"<span id='my_url_{0}'>![]({1})</span>&nbsp;&nbsp;<button class='btn' data-clipboard-target='#my_url_{2}' style='width:50px;height:30px;'>复制</button><br/>{3}";
        
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
    <script type='text/javascript' src='https://cdn.staticfile.org/clipboard.js/1.5.15/clipboard.min.js'></script>
    <script type = 'text/javascript' >
    var clipboard = new Clipboard('.btn');
    </script>
    </body>
</html>";

        public static void Export(List<string> urlList, string filePath) {
            if (urlList.Count == 0)
                return;

            StringBuilder sb = new StringBuilder("");
            for (int i = 0; i < urlList.Count; i++) {
                string url = urlList[i];
                sb.Append(string.Format(KImgTemp, url, Program.MySetting.ImgHeight));
                sb.Append(string.Format(KSpanTemp, i, url, i, "\r\n"));
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
