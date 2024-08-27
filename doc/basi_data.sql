-- api厂家
INSERT INTO `osp_api` VALUES (1826866522681905152, 'zhutong', '上海助通',1, NULL,  NULL, '2024-08-23 14:18:15', 0, 'admin', NULL, NULL, NULL);
INSERT INTO `osp_api` VALUES (1826868874226831360, 'cmcc', '云MAS', 1,'https://mas.10086.cn/login',  '中国移动云mas业务平台', '2024-08-23 14:27:36', 0, 'admin', NULL, NULL, NULL);
INSERT INTO `osp_api` VALUES (1826869134550503424, 'lianlu', '上海联麓',1,'https://www.shlianlu.com/index.html',  '上海联麓信息', '2024-08-23 14:28:38', 0, 'admin', NULL, NULL, NULL);


-- 账号
INSERT INTO `osp_account` VALUES (1826880838734843904, '杭州希和信息技术', '1724397308', '1724397308415', '152f540b511b44d58f4b68bf26d3435e', '【杭州希和】', 11000, 1, '公司自用账号', 'lianlu', '2024-08-23 15:15:08', 0, 'admin', NULL, NULL, NULL);
INSERT INTO `osp_account` VALUES (1826882630302437376, '希和健康', '1724397735', '1724397735324', '0fa49762c46f4538b79757e083ea36e2', '【希和健康】', 0, 1, '公司自用', 'lianlu', '2024-08-23 15:22:15', 0, 'admin', NULL, NULL, NULL);


-- 充值
INSERT INTO `osp_acc_charge` VALUES (1826890479493582848, '1826880838734843904', 100.00, 0, 1000, 1000, '账号开通后，首次赠送1000条', '2024-08-23 15:53:27', 0, 'admin', NULL, NULL, NULL);
INSERT INTO `osp_acc_charge` VALUES (1826891756684316672, '1826880838734843904', 1000.00, 1000, 10000, 11000, '内部使用，再次充值1K元', '2024-08-23 15:58:31', 0, 'admin', NULL, NULL, NULL);