using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using Qiniu.Util;
using Qiniu.IO;
using Qiniu.IO.Model;
using System.IO;

namespace UploadForQiniu {
    class Program {

        const string kFlod_need_upload = "need_upload";
        const string kFlod_success = "success";
        const string kFlod_fail = "fail";
        const string kRecordFile = "a_record.md"; // 记录所有上传成功的url

        string _currDir = "";
        string _flod_need_upload = "";
        string _flod_success = "";
        string _flod_fail = "";
        string _token = "";
        List<string> _succList = new List<string>();
        List<string> _saveKeyList = new List<string>();
        List<string> _failList = new List<string>();
        int _count = 0;

        public void Init() {
            _currDir = System.IO.Directory.GetCurrentDirectory();
            DebugLog("--- Current Directory:{0}", _currDir);
            InitFold();
            InitAuth();
        }

        //日志适配，方便移植到不同平台
        public void DebugLog(string format, params object[] args) {
            Util.Log(format, args);
        }

        // 初始化上传凭证相关
        public void InitAuth() {
            LoadKey();

            Mac mac = new Mac(Settings.AccessKey, Settings.SecretKey);
            PutPolicy putPolicy = new PutPolicy();
            putPolicy.Scope = Settings.Bucket;
            putPolicy.SetExpires(3600);
            string jstr = putPolicy.ToJsonString();
            _token = Auth.CreateUploadToken(mac, jstr);
        }

        public void InitFold() {
            _flod_need_upload = Path.Combine(_currDir, kFlod_need_upload);
            _flod_success = Path.Combine(_currDir, kFlod_success);
            _flod_fail = Path.Combine(_currDir, kFlod_fail);
            CheckDir(_flod_need_upload);
            CheckDir(_flod_success);
            CheckDir(_flod_fail);
        }

        public void CheckDir(string url) {
            if (!Directory.Exists(url))
                Directory.CreateDirectory(url);
        }

        //加载配置
        public void LoadKey() {
            //Settings.LoadFromFile("G:\\workplace_c#\\UploadForQiniu\\UploadForQiniu\\token.txt");
            Settings.LoadFromFile("F:\\workplace_c#\\UploadForQiniu\\token.txt");

            DebugLog("--- AccessKey:{0}", Settings.AccessKey);
            DebugLog("--- SecretKey:{0}", Settings.SecretKey);
            DebugLog("--- PreLink:{0}", Settings.PreLink);
            DebugLog("--- Bucket:{0}", Settings.Bucket);
        }

        // 导出 html 及 记录
        public void ExportHtml() {
            string time = System.DateTime.Now.ToString("yyyyMMdd_HHmmss");
            HtmlExport.Record(_saveKeyList, Path.Combine(_currDir, kRecordFile), time);

            string htmlPath = Path.Combine(_currDir, string.Format("{0}.html", time));
            HtmlExport.Export(_saveKeyList, htmlPath);
            DebugLog("\n--- html path:{0}", htmlPath);
            if (_saveKeyList.Count > 0)
                System.Diagnostics.Process.Start(htmlPath); // 访问 html
        }

        // 重命名上传到云端的文件
        public string GetSaveKey(string filePath) {
            string time = System.DateTime.Now.ToString("yyyyMMdd_HHmmss");
            string fileName = filePath.Substring(filePath.LastIndexOf("\\") + 1);
            return string.Format("{0}_{1}", time, fileName);
        }

        // 上传文件
        public string UploadFile(string filePath) {
            string saveKey = GetSaveKey(filePath);

            FormUploader fu = new FormUploader();
            var result = fu.UploadFile(filePath, saveKey, _token);
            return result.Code == 200 ? saveKey : "";
        }

        // 递归遍历文件夹
        public void RecurDir(string dirPath) {
            DealDir(dirPath);

            string[] dires = System.IO.Directory.GetDirectories(dirPath);
            foreach (string dir in dires) {
                RecurDir(dir);
            }
        }

        // 处理 dirPath 路径下的所有文件
        public void DealDir(string dirPath) {
            string[] files = System.IO.Directory.GetFiles(dirPath, "*", System.IO.SearchOption.TopDirectoryOnly);
            foreach (string filePath in files) {
                //ModifyFile(filePath, dirPath);
                ++_count;
                string saveKey = UploadFile(filePath);
                //string saveKey = "aaaaaaa.jpg";
                MoveFile(filePath, saveKey); // 移动文件
            }
        }

        // 上传成功或失败移动文件到对应目录
        public void MoveFile(string filePath, string saveKey) {
            bool isSuccess = saveKey.Length > 0;
            string dstFile = "";
            if (isSuccess) {
                _succList.Add(filePath);
                _saveKeyList.Add(Settings.PreLink + saveKey);
                dstFile = filePath.Replace(kFlod_need_upload, kFlod_success);
            } else {
                _failList.Add(filePath);
                dstFile = filePath.Replace(kFlod_need_upload, kFlod_fail);
            }

            if (File.Exists(dstFile)) {
                string preName = dstFile.Substring(0, dstFile.LastIndexOf("."));
                string extName = dstFile.Substring(dstFile.LastIndexOf("."));
                dstFile = string.Format("{0}_1{1}", preName, extName);
            }

            try {
                File.Move(filePath, dstFile);
            } catch (System.Exception ex) {
                Util.Log("--- move file error:\n{0}", ex.Message);
            }
        }


        public void DoUpload() {
            RecurDir(_flod_need_upload);
        }

        public void Report() {
            DebugLog("\n\n-------------- Report");
            DebugLog("--- Total deal files: \t ({0})", _count);
            DebugLog("--- Success: \t\t ({0})", _succList.Count);
            DebugLog("--- Fail: \t\t ({0})", _failList.Count);
        }

        static void Main(string[] args) {
            Program p = new Program();
            p.Init();
            p.DoUpload();
            p.ExportHtml();
            p.Report();
            Console.ReadKey();
        }
    }
}
