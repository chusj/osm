/*
 Navicat Premium Data Transfer

 Source Server         : local
 Source Server Type    : MySQL
 Source Server Version : 50741
 Source Host           : localhost:3306
 Source Schema         : yasi

 Target Server Type    : MySQL
 Target Server Version : 50741
 File Encoding         : 65001

 Date: 23/08/2024 17:59:13
*/

SET NAMES utf8mb4;
SET FOREIGN_KEY_CHECKS = 0;


-- ----------------------------
-- Table structure for osp_acc_charge
-- ----------------------------
DROP TABLE IF EXISTS `osp_acc_charge`;
CREATE TABLE `osp_acc_charge`  (
  `id` bigint(20) NOT NULL,
  `acc_id` varchar(255) CHARACTER SET utf8 COLLATE utf8_general_ci NOT NULL COMMENT '账号id',
  `amount` decimal(10, 2) NOT NULL COMMENT '金额',
  `before_counts` int(11) NOT NULL COMMENT '充值前条数',
  `counts` int(11) NOT NULL COMMENT '充值条数',
  `after_counts` int(11) NOT NULL COMMENT '充值后条数',
  `remarks` varchar(255) CHARACTER SET utf8 COLLATE utf8_general_ci NULL DEFAULT NULL COMMENT '备注',
  `create_on` datetime NOT NULL,
  `create_uid` bigint(20) NOT NULL,
  `create_by` varchar(20) CHARACTER SET utf8 COLLATE utf8_general_ci NOT NULL,
  `modify_on` datetime NULL DEFAULT NULL,
  `modify_uid` bigint(20) NULL DEFAULT NULL,
  `modify_by` varchar(20) CHARACTER SET utf8 COLLATE utf8_general_ci NULL DEFAULT NULL,
  PRIMARY KEY (`id`) USING BTREE
) ENGINE = InnoDB CHARACTER SET = utf8 COLLATE = utf8_general_ci ROW_FORMAT = Dynamic;

-- ----------------------------
-- Records of osp_acc_charge
-- ----------------------------
INSERT INTO `osp_acc_charge` VALUES (1826890479493582848, '1826880838734843904', 100.00, 0, 1000, 1000, '账号开通后，首次赠送1000条', '2024-08-23 15:53:27', 0, 'admin', NULL, NULL, NULL);
INSERT INTO `osp_acc_charge` VALUES (1826891756684316672, '1826880838734843904', 1000.00, 1000, 10000, 11000, '内部使用，再次充值1K元', '2024-08-23 15:58:31', 0, 'admin', NULL, NULL, NULL);

-- ----------------------------
-- Table structure for osp_account
-- ----------------------------
DROP TABLE IF EXISTS `osp_account`;
CREATE TABLE `osp_account`  (
  `id` bigint(20) NOT NULL,
  `acc_name` varchar(40) CHARACTER SET utf8 COLLATE utf8_general_ci NOT NULL COMMENT '账号名',
  `acc_id` varchar(20) CHARACTER SET utf8 COLLATE utf8_general_ci NOT NULL COMMENT '账号id',
  `acc_key` varchar(20) CHARACTER SET utf8 COLLATE utf8_general_ci NOT NULL COMMENT 'key',
  `acc_secret` varchar(40) CHARACTER SET utf8 COLLATE utf8_general_ci NOT NULL COMMENT 'secret',
  `sms_suffix` varchar(40) CHARACTER SET utf8 COLLATE utf8_general_ci NOT NULL COMMENT '短信后缀',
  `acc_counts` int(11) NOT NULL COMMENT '短信余额',
  `is_enable` tinyint(4) NOT NULL COMMENT '是否启用 1.是 2.否',
  `remarks` varchar(255) CHARACTER SET utf8 COLLATE utf8_general_ci NULL DEFAULT NULL COMMENT '备注',
  `api_code` varchar(20) CHARACTER SET utf8 COLLATE utf8_general_ci NULL DEFAULT NULL COMMENT 'api编码',
  `create_on` datetime NOT NULL,
  `create_uid` bigint(20) NOT NULL,
  `create_by` varchar(20) CHARACTER SET utf8 COLLATE utf8_general_ci NOT NULL,
  `modify_on` datetime NULL DEFAULT NULL,
  `modify_uid` bigint(20) NULL DEFAULT NULL,
  `modify_by` varchar(20) CHARACTER SET utf8 COLLATE utf8_general_ci NULL DEFAULT NULL,
  PRIMARY KEY (`id`) USING BTREE
) ENGINE = InnoDB CHARACTER SET = utf8 COLLATE = utf8_general_ci ROW_FORMAT = Dynamic;

-- ----------------------------
-- Records of osp_account
-- ----------------------------
INSERT INTO `osp_account` VALUES (1826880838734843904, '杭州希和信息技术', '1724397308', '1724397308415', '152f540b511b44d58f4b68bf26d3435e', '【杭州希和】', 11000, 1, '公司自用账号', 'lianlu', '2024-08-23 15:15:08', 0, 'admin', NULL, NULL, NULL);
INSERT INTO `osp_account` VALUES (1826882630302437376, '希和健康', '1724397735', '1724397735324', '0fa49762c46f4538b79757e083ea36e2', '【希和健康】', 0, 1, '公司自用', 'lianlu', '2024-08-23 15:22:15', 0, 'admin', NULL, NULL, NULL);

-- ----------------------------
-- Table structure for osp_api
-- ----------------------------
DROP TABLE IF EXISTS `osp_api`;
CREATE TABLE `osp_api`  (
  `id` bigint(20) NOT NULL,
  `api_code` varchar(20) CHARACTER SET utf8 COLLATE utf8_general_ci NOT NULL COMMENT 'api通道编码',
  `api_name` varchar(40) CHARACTER SET utf8 COLLATE utf8_general_ci NOT NULL COMMENT '名称',
  `api_url` varchar(200) CHARACTER SET utf8 COLLATE utf8_general_ci NULL DEFAULT NULL COMMENT 'Api地址',
  `is_enabled` tinyint(4) NOT NULL COMMENT '是否启用',
  `remarks` varchar(400) CHARACTER SET utf8 COLLATE utf8_general_ci NULL DEFAULT NULL COMMENT '备注',
  `create_on` datetime NOT NULL,
  `create_uid` bigint(20) NOT NULL,
  `create_by` varchar(20) CHARACTER SET utf8 COLLATE utf8_general_ci NOT NULL,
  `modify_on` datetime NULL DEFAULT NULL,
  `modify_uid` bigint(20) NULL DEFAULT NULL,
  `modify_by` varchar(20) CHARACTER SET utf8 COLLATE utf8_general_ci NULL DEFAULT NULL,
  PRIMARY KEY (`id`) USING BTREE
) ENGINE = InnoDB CHARACTER SET = utf8 COLLATE = utf8_general_ci ROW_FORMAT = Dynamic;

-- ----------------------------
-- Records of osp_api
-- ----------------------------
INSERT INTO `osp_api` VALUES (1826866522681905152, 'zhutong', '上海助通', NULL, 1, NULL, '2024-08-23 14:18:15', 0, 'admin', NULL, NULL, NULL);
INSERT INTO `osp_api` VALUES (1826868874226831360, 'cmcc', '云MAS', 'https://mas.10086.cn/login', 1, '中国移动云mas业务平台', '2024-08-23 14:27:36', 0, 'admin', NULL, NULL, NULL);
INSERT INTO `osp_api` VALUES (1826869134550503424, 'lianlu', '上海联麓', 'https://www.shlianlu.com/index.html', 1, '上海联麓信息', '2024-08-23 14:28:38', 0, 'admin', NULL, NULL, NULL);

-- ----------------------------
-- Table structure for osp_limit
-- ----------------------------
DROP TABLE IF EXISTS `osp_limit`;
CREATE TABLE `osp_limit`  (
  `id` bigint(20) NOT NULL,
  `add_type` tinyint(4) NOT NULL COMMENT '添加类型 1.手动 2.自动',
  `mobile` varchar(11) CHARACTER SET utf8 COLLATE utf8_general_ci NOT NULL COMMENT '手机号码',
  `limit_type` tinyint(4) NOT NULL COMMENT '限制类型 1.白名单 2.黑名单',
  `remarks` varchar(255) CHARACTER SET utf8 COLLATE utf8_general_ci NULL DEFAULT NULL COMMENT '备注',
  `create_on` datetime NOT NULL,
  `create_uid` bigint(20) NOT NULL,
  `create_by` varchar(20) CHARACTER SET utf8 COLLATE utf8_general_ci NOT NULL,
  `modify_on` datetime NULL DEFAULT NULL,
  `modify_uid` bigint(20) NULL DEFAULT NULL,
  `modify_by` varchar(20) CHARACTER SET utf8 COLLATE utf8_general_ci NULL DEFAULT NULL,
  PRIMARY KEY (`id`) USING BTREE
) ENGINE = InnoDB CHARACTER SET = utf8 COLLATE = utf8_general_ci ROW_FORMAT = Dynamic;

-- ----------------------------
-- Records of osp_limit
-- ----------------------------

-- ----------------------------
-- Table structure for osp_record
-- ----------------------------
DROP TABLE IF EXISTS `osp_record`;
CREATE TABLE `osp_record`  (
  `id` bigint(20) NOT NULL,
  `acc_id` bigint(20) NOT NULL COMMENT '账号id',
  `mobile` varchar(11) CHARACTER SET utf8 COLLATE utf8_general_ci NOT NULL COMMENT '手机号',
  `content` varchar(1000) CHARACTER SET utf8 COLLATE utf8_general_ci NOT NULL COMMENT '内容',
  `code` varchar(8) CHARACTER SET utf8 COLLATE utf8_general_ci NOT NULL COMMENT '验证码',
  `is_code` tinyint(4) NOT NULL COMMENT '是否为验证码 1.是 2.否',
  `is_used` tinyint(4) NOT NULL COMMENT '验证码是否使用  1.是 2.否',
  `send_on` datetime NOT NULL COMMENT '发送时间',
  `counts` tinyint(4) NOT NULL COMMENT '计费条数',
  `request_id` varchar(40) CHARACTER SET utf8 COLLATE utf8_general_ci NOT NULL COMMENT '请求id',
  `api_code` tinyint(4) NOT NULL COMMENT 'api编码',
  `create_on` datetime NOT NULL,
  `create_uid` bigint(20) NOT NULL,
  `create_by` varchar(20) CHARACTER SET utf8 COLLATE utf8_general_ci NOT NULL,
  `modify_on` datetime NULL DEFAULT NULL,
  `modify_uid` bigint(20) NULL DEFAULT NULL,
  `modify_by` varchar(20) CHARACTER SET utf8 COLLATE utf8_general_ci NULL DEFAULT NULL,
  PRIMARY KEY (`id`) USING BTREE
) ENGINE = InnoDB CHARACTER SET = utf8 COLLATE = utf8_general_ci ROW_FORMAT = Dynamic;

-- ----------------------------
-- Records of osp_record
-- ----------------------------

SET FOREIGN_KEY_CHECKS = 1;
