CREATE TABLE `osm_acc_charge`  (
  `id` bigint NOT NULL,
  `acc_id` varchar(255) NOT NULL,
  `amount` decimal(10, 2) NOT NULL,
  `remarks` varchar(255) NULL,
  `before_counts` int NOT NULL,
  `counts` int NOT NULL,
  `after_counts` int NOT NULL,
  `create_on` datetime NULL,
  PRIMARY KEY (`id`)
);

CREATE TABLE `osm_account`  (
  `id` bigint NOT NULL,
  `acc_name` varchar(40) NOT NULL,
  `acc_id` varchar(20) NOT NULL,
  `acc_key` varchar(20) NOT NULL,
  `acc_secret` varchar(40) NOT NULL,
  `sms_suffix` varchar(40) NOT NULL,
  `acc_counts` int NOT NULL,
  `api_code` tinyint NOT NULL,
  `remarks` varchar(255) NULL,
  `is_enable` tinyint NOT NULL,
  `create_on` datetime NOT NULL,
  PRIMARY KEY (`id`)
);

CREATE TABLE `osm_api`  (
  `id` bigint NOT NULL,
  `api_code` tinyint NOT NULL COMMENT 'api通道编码',
  `api_name` varchar(40) NOT NULL COMMENT '名称',
  `api_url` varchar(200) NULL COMMENT 'Api地址',
  `is_enabled` tinyint NOT NULL COMMENT '是否启用',
  `remarks` varchar(400) NOT NULL COMMENT '备注',
  `create_on` varchar(255) NOT NULL,
  PRIMARY KEY (`id`)
);

CREATE TABLE `osm_limit`  (
  `id` bigint NOT NULL,
  `mobile` varchar(11) NOT NULL,
  `limit_type` tinyint NOT NULL COMMENT '限制类型 1.白名单 2.黑名单',
  `remarks` varchar(255) NULL,
  `add_type` tinyint NOT NULL COMMENT '添加类型 1.手动 2.自动',
  `create_on` datetime NOT NULL,
  PRIMARY KEY (`id`)
);

CREATE TABLE `osm_record`  (
  `id` bigint NOT NULL,
  `acc_id` bigint NOT NULL,
  `mobile` varchar(11) NOT NULL,
  `content` varchar(1000) NOT NULL,
  `code` varchar(8) NOT NULL,
  `is_code` tinyint NOT NULL,
  `is_used` tinyint NOT NULL,
  `send_on` datetime NOT NULL COMMENT '发送时间',
  `counts` tinyint NOT NULL COMMENT '计费条数',
  `request_id` varchar(40) NOT NULL COMMENT '请求id',
  `api_code` tinyint NOT NULL,
  `create_on` datetime NOT NULL,
  PRIMARY KEY (`id`)
);

