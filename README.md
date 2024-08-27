## 简介
1. 开放短信平台，可用于快速接入短信厂家api接口，实现短信发送功能，支持子账号的维护、充值、支持手机号码黑/白名单、每日/每月的发送条数限制、短信记录自动分表保存等功能。
2. 此平台可用于医院、企业内部部署，用于支撑内部各个业务系统的短信发送功能。
3. 感谢[老张的哲学](https://github.com/anjoy8/BCVP.Net8) ，本系统完全是学习[B站课程《ASP.NET8.0入口与实战》](https://www.bilibili.com/video/BV13g4y1Z7in/?spm_id_from=333.999.0.0&vd_source=d4e8ce91bfd8d13dde3972cb1ac9b6a9)而来的
   


## 快速开始

### 安装

1. 在mysql中，新建一个数据库
2. 在步骤1的数据库中，执行建库脚本 git仓库/doc/1.createdb.sql
3. 执行sql，插入已默认接入的2个厂家api

```sql
INSERT INTO `osp_api` VALUES (1826866522681905152, 'zhutong', '上海助通',1, 'https://www.ztinfo.cn/','', '2024-08-23 14:18:15', 0, 'admin', NULL, NULL, NULL);

INSERT INTO `osp_api` VALUES (1826869134550503424, 'lianlu', '上海联麓',1,'https://www.shlianlu.com/index.html','','2024-08-23 14:28:38', 0, 'admin', NULL, NULL, NULL);
```

5. 新建账号
```sql
INSERT INTO `osp_account` VALUES (1826880838734843904, 'xxx公司', '1724397308', '1724397308415', '152f540b511b44d58f4b68bf26d3435e', '【xxx公司】', 10000, 1, '自用账号', 'lianlu', '2024-08-23 15:15:08', 0, 'admin', NULL, NULL, NULL);
```

6. 表设计截图

![Local Image](../master/doc/table-design.png)

### 运行
1. 发布 OpenSmsPlatform.Api 站点，并部署到IIS上
2. 如果使用内置的短信api厂家，请修改 **appsettings.json** 文件中配置节点（不配置无法真实发送短信）
```json
  //上海助通短信配置
  "ZhuTong": {
    "ApiUrl": "",
    "ApiPath": "",
    "UserName": "",
    "Password": ""
  },
  
  //上海联麓短信配置
  "LianLu": {
    "ApiUrl": "",
    "ApiPath": "",
    "MchId": "",
    "AppId": "",
    "AppKey": ""
  }
```
3. 如果接入其他短信厂家api，可自行开发或联系我


## 功能
1. 通过Nuget包的方式，已内置接入2家短信公司api
    - 上海联麓电子商务有限公司 [smsapi.zhutong](https://www.nuget.org/packages/smsapi.zhutong)
    - 上海助通信息科技有限公司 [smsapi.lianlu](https://www.nuget.org/packages/smsapi.lianlu)
2. 整体项目采用：.Net8 + 仓储 + 服务 的结构 
3. 数据操作这块用的是： sqlsugar + mysql数据库
4. 短信记录表根据send_on字段，自动按照月份分表存储

## 致谢
1. 再次感谢[老张的哲学](https://github.com/anjoy8)，强烈推荐大家关注他的开源项目[Blog.Core](http://apk.neters.club/.doc/)
2. 感谢[Bryan-Cy 的 SuperShortLink 项目f](https://github.com/Bryan-Cyf/SuperShortLink) 提供了灵感，将自己对接的短信api上传到nuget

