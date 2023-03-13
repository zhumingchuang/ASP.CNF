/*
 Navicat Premium Data Transfer

 Source Server         : MySQL
 Source Server Type    : MySQL
 Source Server Version : 80031
 Source Host           : localhost:3306
 Source Schema         : cloud_rendering

 Target Server Type    : MySQL
 Target Server Version : 80031
 File Encoding         : 65001

 Date: 13/03/2023 23:50:04
*/

SET NAMES utf8mb4;
SET FOREIGN_KEY_CHECKS = 0;

-- ----------------------------
-- Table structure for cnf_user
-- ----------------------------
DROP TABLE IF EXISTS `cnf_user`;
CREATE TABLE `cnf_user`  (
  `Id` int NOT NULL AUTO_INCREMENT COMMENT '主键',
  `UserName` varchar(255) CHARACTER SET utf8mb4 COLLATE utf8mb4_0900_ai_ci NOT NULL COMMENT '用户名（唯一）',
  `Password` varchar(255) CHARACTER SET utf8mb4 COLLATE utf8mb4_0900_ai_ci NOT NULL COMMENT '用户密码',
  `LastLoginTime` datetime NOT NULL COMMENT '登录时间',
  `IsDeleted` tinyint(1) NOT NULL COMMENT '是否软删除',
  `IsLogin` tinyint(1) NOT NULL COMMENT '是否登录',
  `Address` varchar(255) CHARACTER SET utf8mb4 COLLATE utf8mb4_0900_ai_ci NOT NULL COMMENT '登录地址',
  `Ip` varchar(255) CHARACTER SET utf8mb4 COLLATE utf8mb4_0900_ai_ci NOT NULL COMMENT 'IP地址',
  PRIMARY KEY (`Id`) USING BTREE,
  INDEX `Password`(`Password` ASC) USING BTREE,
  INDEX `IsDeleted`(`IsDeleted` ASC) USING BTREE,
  INDEX `UserName`(`UserName` ASC) USING BTREE
) ENGINE = InnoDB CHARACTER SET = utf8mb4 COLLATE = utf8mb4_0900_ai_ci ROW_FORMAT = Dynamic;

-- ----------------------------
-- Records of cnf_user
-- ----------------------------

-- ----------------------------
-- Table structure for cnf_userauths
-- ----------------------------
DROP TABLE IF EXISTS `cnf_userauths`;
CREATE TABLE `cnf_userauths`  (
  `Id` int NOT NULL COMMENT '外键',
  `Identity_Type` varchar(255) CHARACTER SET utf8mb4 COLLATE utf8mb4_0900_ai_ci NOT NULL COMMENT '身份类型（手机号，邮箱...）',
  `Identifier` varchar(255) CHARACTER SET utf8mb4 COLLATE utf8mb4_0900_ai_ci NOT NULL COMMENT '标识（手机号，邮箱...）',
  `Credential` varchar(255) CHARACTER SET utf8mb4 COLLATE utf8mb4_0900_ai_ci NULL DEFAULT NULL COMMENT '凭据(密码...)',
  INDEX `cnf_userauths_ID`(`Id` ASC) USING BTREE,
  CONSTRAINT `cnf_userauths_ID` FOREIGN KEY (`Id`) REFERENCES `cnf_user` (`Id`) ON DELETE RESTRICT ON UPDATE RESTRICT
) ENGINE = InnoDB CHARACTER SET = utf8mb4 COLLATE = utf8mb4_0900_ai_ci ROW_FORMAT = Dynamic;

-- ----------------------------
-- Records of cnf_userauths
-- ----------------------------

-- ----------------------------
-- Table structure for cnf_userinfo
-- ----------------------------
DROP TABLE IF EXISTS `cnf_userinfo`;
CREATE TABLE `cnf_userinfo`  (
  `Id` int NOT NULL COMMENT '外键',
  `Nickname` varchar(255) CHARACTER SET utf8mb4 COLLATE utf8mb4_0900_ai_ci NOT NULL COMMENT '用户名',
  `Email` varchar(255) CHARACTER SET utf8mb4 COLLATE utf8mb4_0900_ai_ci NULL DEFAULT NULL COMMENT '邮箱',
  `Sex` varchar(255) CHARACTER SET utf8mb4 COLLATE utf8mb4_0900_ai_ci NULL DEFAULT NULL COMMENT '性别',
  `HeadImg` varchar(255) CHARACTER SET utf8mb4 COLLATE utf8mb4_0900_ai_ci NULL DEFAULT NULL COMMENT '头像',
  INDEX `asdasd`(`Email` ASC) USING BTREE,
  INDEX `cnf_userinfo_ID`(`Id` ASC) USING BTREE,
  CONSTRAINT `cnf_userinfo_ID` FOREIGN KEY (`Id`) REFERENCES `cnf_user` (`Id`) ON DELETE RESTRICT ON UPDATE RESTRICT
) ENGINE = InnoDB CHARACTER SET = utf8mb4 COLLATE = utf8mb4_0900_ai_ci ROW_FORMAT = Dynamic;

-- ----------------------------
-- Records of cnf_userinfo
-- ----------------------------

SET FOREIGN_KEY_CHECKS = 1;
