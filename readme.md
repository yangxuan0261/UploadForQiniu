> 简单的业务逻辑，相关版本
>
> - 七牛 最新 csharp sdk 中的 .net2.0
> - 七牛引用的第三方 Newtonsoft.Json 是 9.0 版本中的 .net2.0
> - 开发使用的是 .net4.5，可降到 2.0



---

之所以用2.0是因为后面要做gui程序可以直接丢带unity中打包出来，现在没有复杂的需求就简单的用控制台程序。



### 使用说明

1. 使用前置条件

   - .net4.5

2. 在 .exe 可执行程序所在目录新建 3 个目录：

   1. need_upload：*等待上传* 的资源都丢到这个目录之下
   2. success：上传 *成功* 的资源会移到这个目录下
   3. fail：上传 *失败* 的资源会移到这个目录下

3. 在 .exe 可执行程序所在目录新建一个文件 *config.json* 内容入校：

   ```json
   {
       "AccessKey": "aaaaaaaaaaaaaaaaapo3w9o12VMfifyr", // ak
       "SecretKey": "bbbbbbbbbbbbbbbpo3w9o12VMfifyr", // sk
       "PreLink": "http://ccccccccccccc.bkt.clouddn.com/",
       "ZoneId": 0, //存储空间所在区域，0:华东, 1:华北, 2:华南, 3:北美
       "Bucket": "wilkeryun", //存储空间名
       "ImgHeight": 120, // 导出html的图片高度
   }
   ```

   -  [附录](#附录)

4. 上传成功的 *外链* 会添加到 .exe 可执行程序所在目录 的 *a_record.md* 文件中，markdown格式


---

### 附录

- accessKey、secretKey

  ![](http://7xr37b.com1.z0.glb.clouddn.com/20170716_112436_QQ截图20170716112422.png)

  ​

  ​

  ![](http://7xr37b.com1.z0.glb.clouddn.com/20170716_111913_QQ截图20170716111714.png)

- 外链前缀、存储空间名称

  ![](http://7xr37b.com1.z0.glb.clouddn.com/20170716_111913_QQ截图20170716111842.png)

- 弹窗显示图片的宽、高

  ![](http://7xr37b.com1.z0.glb.clouddn.com/20170716_112051_QQ%E6%88%AA%E5%9B%BE20170716112038.png)