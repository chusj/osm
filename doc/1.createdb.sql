CREATE TABLE `osp_acc_charge`  (
  `id` bigint NOT NULL,
  `acc_id` varchar(255) NOT NULL COMMENT '账号id',
  `amount` decimal(10, 2) NOT NULL COMMENT '金额',
  `before_counts` int NOT NULL COMMENT '充值前条数',
  `counts` int NOT NULL COMMENT '充值条数',
  `after_counts` int NOT NULL COMMENT '充值后条数',
  `remarks` varchar(255) NULL COMMENT '备注',
  `create_on` datetime NOT NULL,
  `create_uid` bigint NOT NULL,
  `create_by` varchar(20) NOT NULL,
  `modify_on` datetime NULL,
  `modify_uid` bigint NULL,
  `modify_by` varchar(20) NULL,
  PRIMARY KEY (`id`)
) COMMENT = '短信账号充值';

CREATE TABLE `osp_account`  (
  `id` bigint NOT NULL,
  `acc_name` varchar(40) NOT NULL COMMENT '账号名',
  `acc_id` varchar(20) NOT NULL COMMENT '账号id',
  `acc_key` varchar(20) NOT NULL COMMENT 'key',
  `acc_secret` varchar(40) NOT NULL COMMENT 'secret',
  `sms_suffix` varchar(40) NOT NULL COMMENT '短信后缀',
  `acc_counts` int NOT NULL DEFAULT 0 COMMENT '短信余额',
  `is_enable` tinyint NOT NULL COMMENT '是否启用 1.是 2.否',
  `remarks` varchar(255) NULL COMMENT '备注',
  `api_code` varchar(20) NULL COMMENT 'api编码',
  `create_on` datetime NOT NULL,
  `create_uid` bigint NOT NULL,
  `create_by` varchar(20) NOT NULL,
  `modify_on` datetime NULL,
  `modify_uid` bigint NULL,
  `modify_by` varchar(20) NULL,
  PRIMARY KEY (`id`)
) COMMENT = '短信账号';

CREATE TABLE `osp_api`  (
  `id` bigint NOT NULL,
  `api_code` varchar(20) NOT NULL COMMENT 'api通道编码',
  `api_name` varchar(40) NOT NULL COMMENT '名称',
  `is_enabled` tinyint NOT NULL COMMENT '是否启用 1.是 2.否',
  `api_url` varchar(200) NULL COMMENT 'Api地址',
  `remarks` varchar(400) NULL COMMENT '备注',
  `create_on` varchar(255) NOT NULL,
  `create_uid` bigint NOT NULL,
  `create_by` varchar(20) NOT NULL,
  `modify_on` datetime NULL,
  `modify_uid` bigint NULL,
  `modify_by` varchar(20) NULL,
  PRIMARY KEY (`id`)
) COMMENT = '短信Api';

CREATE TABLE `osp_limit`  (
  `id` bigint NOT NULL,
  `add_type` tinyint NOT NULL COMMENT '添加类型 1.手动 2.自动',
  `mobile` varchar(11) NOT NULL COMMENT '手机号码',
  `limit_type` tinyint NOT NULL COMMENT '限制类型 1.白名单 2.黑名单',
  `remarks` varchar(255) NULL COMMENT '备注',
  `create_on` datetime NOT NULL,
  `create_uid` bigint NOT NULL,
  `create_by` varchar(20) NOT NULL,
  `modify_on` datetime NULL,
  `modify_uid` bigint NULL,
  `modify_by` varchar(20) NULL,
  PRIMARY KEY (`id`)
) COMMENT = '短信限制表';

CREATE TABLE `osp_record`  (
  `id` bigint NOT NULL,
  `acc_id` bigint NOT NULL COMMENT '账号id',
  `mobile` varchar(11) NOT NULL COMMENT '手机号',
  `content` varchar(1000) NOT NULL COMMENT '内容',
  `code` varchar(8) NOT NULL COMMENT '验证码',
  `is_code` tinyint NOT NULL COMMENT '是否为验证码 1.是 2.否',
  `is_used` tinyint NOT NULL COMMENT '验证码是否使用  1.是 2.否',
  `send_on` datetime NOT NULL COMMENT '发送时间',
  `counts` tinyint NOT NULL COMMENT '计费条数',
  `api_code` varchar(20) NOT NULL COMMENT 'api编码',
  `request_id` varchar(40) NOT NULL COMMENT '请求id',
  `request_ip` varchar(40) NULL COMMENT '请求ip',
  `create_on` datetime NOT NULL,
  `create_uid` bigint NOT NULL,
  `create_by` varchar(20) NOT NULL,
  `modify_on` datetime NULL,
  `modify_uid` bigint NULL,
  `modify_by` varchar(20) NULL,
  PRIMARY KEY (`id`)
) COMMENT = '短信记录';

