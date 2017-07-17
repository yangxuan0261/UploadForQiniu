using Qiniu.JSON;
using System;
using System.Collections.Generic;
using System.IO;
using System.Text;

namespace UploadForQiniu {

    public class Settings
    {
        //see ak sk from https://portal.qiniu.com/user/key
        public string AccessKey;
        public string SecretKey;
        public string PreLink;
        public int ZoneId;
        public string Bucket;
        public int ImgHeight = 120;
    }
}
