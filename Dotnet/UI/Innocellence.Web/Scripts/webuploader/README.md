目录说明
========================

```bash
├── Uploader.swf                      # SWF文件，当使用Flash运行时需要引入。
├
├── webuploader.js                    # 完全版本。
├── webuploader.min.js                # min版本
├
├── webuploader.flashonly.js          # 只有Flash实现的版本。
├── webuploader.flashonly.min.js      # min版本
├
├── webuploader.html5only.js          # 只有Html5实现的版本。
├── webuploader.html5only.min.js      # min版本
├
├── webuploader.noimage.js            # 去除图片处理的版本，包括HTML5和FLASH.
├── webuploader.noimage.min.js        # min版本
├
├── webuploader.custom.js             # 自定义打包方案，请查看 Gruntfile.js，满足移动端使用。
└── webuploader.custom.min.js         # min版本
```

## 示例

请把整个 Git 包下载下来放在 php 服务器下，因为默认提供的文件接受是用 php 编写的，打开 examples 页面便能查看示例效果。




• dnd {Selector} [可选] [默认值：undefined]  指定Drag And Drop拖拽的容器，如果不指定，则不启动。
 
• disableGlobalDnd {Selector} [可选] [默认值：false]  是否禁掉整个页面的拖拽功能，如果不禁用，图片拖进来的时候会默认被浏览器打开。
 
• paste {Selector} [可选] [默认值：undefined]  指定监听paste事件的容器，如果不指定，不启用此功能。此功能为通过粘贴来添加截屏的图片。建议设置为document.body.
 
• pick {Selector, Object} [可选] [默认值：undefined]  指定选择文件的按钮容器，不指定则不创建按钮。
 ◦id {Seletor|dom} 指定选择文件的按钮容器，不指定则不创建按钮。注意 这里虽然写的是 id, 但是不是只支持 id, 还支持 class, 或者 dom 节点。
◦label {String} 请采用 innerHTML 代替
◦innerHTML {String} 指定按钮文字。不指定时优先从指定的容器中看是否自带文字。
◦multiple {Boolean} 是否开起同时选择多个文件能力。

• accept {Arroy} [可选] [默认值：null]  指定接受哪些类型的文件。 由于目前还有ext转mimeType表，所以这里需要分开指定。
 ◦title {String} 文字描述
◦extensions {String} 允许的文件后缀，不带点，多个用逗号分割。
◦mimeTypes {String} 多个用逗号分割。

如：
{
    title: 'Images',
    extensions: 'gif,jpg,jpeg,bmp,png',
    mimeTypes: 'image/*'
}

• thumb {Object} [可选]  配置生成缩略图的选项。
 
默认为：

{
    width: 110,
    height: 110,

    // 图片质量，只有type为`image/jpeg`的时候才有效。
    quality: 70,

    // 是否允许放大，如果想要生成小图的时候不失真，此选项应该设置为false.
    allowMagnify: true,

    // 是否允许裁剪。
    crop: true,

    // 为空的话则保留原有图片格式。
    // 否则强制转换成指定的类型。
    type: 'image/jpeg'
}

• compress {Object} [可选]  配置压缩的图片的选项。如果此选项为false, 则图片在上传前不进行压缩。
 
默认为：

{
    width: 1600,
    height: 1600,

    // 图片质量，只有type为`image/jpeg`的时候才有效。
    quality: 90,

    // 是否允许放大，如果想要生成小图的时候不失真，此选项应该设置为false.
    allowMagnify: false,

    // 是否允许裁剪。
    crop: false,

    // 是否保留头部meta信息。
    preserveHeaders: true,

    // 如果发现压缩后文件大小比原来还大，则使用原来图片
    // 此属性可能会影响图片自动纠正功能
    noCompressIfLarger: false,

    // 单位字节，如果图片大小小于此值，不会采用压缩。
    compressSize: 0
}

• auto {Boolean} [可选] [默认值：false]  设置为 true 后，不需要手动调用上传，有文件选择即开始上传。
 
• runtimeOrder {Object} [可选] [默认值：html5,flash]  指定运行时启动顺序。默认会想尝试 html5 是否支持，如果支持则使用 html5, 否则则使用 flash.
 
可以将此值设置成 flash，来强制使用 flash 运行时。

• prepareNextFile {Boolean} [可选] [默认值：false]  是否允许在文件传输时提前把下一个文件准备好。 对于一个文件的准备工作比较耗时，比如图片压缩，md5序列化。 如果能提前在当前文件传输期处理，可以节省总体耗时。
 
• chunked {Boolean} [可选] [默认值：false]  是否要分片处理大文件上传。
 
• chunkSize {Boolean} [可选] [默认值：5242880]  如果要分片，分多大一片？ 默认大小为5M.
 
• chunkRetry {Boolean} [可选] [默认值：2]  如果某个分片由于网络问题出错，允许自动重传多少次？
 
• threads {Boolean} [可选] [默认值：3]  上传并发数。允许同时最大上传进程数。
 
• formData {Object} [可选] [默认值：{}]  文件上传请求的参数表，每次发送都会发送此对象中的参数。
 
• fileVal {Object} [可选] [默认值：'file']  设置文件上传域的name。
 
• method {Object} [可选] [默认值：'POST']  文件上传方式，POST或者GET。
 
• sendAsBinary {Object} [可选] [默认值：false]  是否已二进制的流的方式发送文件，这样整个上传内容php://input都为文件内容， 其他参数在$_GET数组中。
 
• fileNumLimit {int} [可选] [默认值：undefined]  验证文件总数量, 超出则不允许加入队列。
 
• fileSizeLimit {int} [可选] [默认值：undefined]  验证文件总大小是否超出限制, 超出则不允许加入队列。
 
• fileSingleSizeLimit {int} [可选] [默认值：undefined]  验证单个文件大小是否超出限制, 超出则不允许加入队列。
 
• duplicate {Boolean} [可选] [默认值：undefined]  去重， 根据文件名字、文件大小和最后修改时间来生成hash Key.
 
• disableWidgets {String, Array} [可选] [默认值：undefined]  默认所有 Uploader.register 了的 widget 都会被加载，如果禁用某一部分，请通过此 option 指定黑名单。
 
