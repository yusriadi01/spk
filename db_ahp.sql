/*
MySQL Data Transfer
Source Host: localhost
Source Database: db_ahp
Target Host: localhost
Target Database: db_ahp
Date: 2/5/2018 6:51:43 AM
*/

SET FOREIGN_KEY_CHECKS=0;
-- ----------------------------
-- Table structure for tbl_alternatif
-- ----------------------------
DROP TABLE IF EXISTS `tbl_alternatif`;
CREATE TABLE `tbl_alternatif` (
  `id` int(11) NOT NULL AUTO_INCREMENT,
  `alternatif` varchar(255) NOT NULL,
  `keterangan` varchar(255) NOT NULL,
  PRIMARY KEY (`id`),
  UNIQUE KEY `alternatif` (`alternatif`)
) ENGINE=InnoDB AUTO_INCREMENT=7 DEFAULT CHARSET=latin1;

-- ----------------------------
-- Table structure for tbl_hasilalternatif
-- ----------------------------
DROP TABLE IF EXISTS `tbl_hasilalternatif`;
CREATE TABLE `tbl_hasilalternatif` (
  `id` int(11) NOT NULL AUTO_INCREMENT,
  `id_kriteria` int(11) NOT NULL,
  `id_alternatif` int(11) NOT NULL,
  `hasil` double NOT NULL DEFAULT '0',
  PRIMARY KEY (`id`),
  UNIQUE KEY `hasilalternatif` (`id_kriteria`,`id_alternatif`)
) ENGINE=InnoDB AUTO_INCREMENT=57 DEFAULT CHARSET=latin1;

-- ----------------------------
-- Table structure for tbl_hasilkriteria
-- ----------------------------
DROP TABLE IF EXISTS `tbl_hasilkriteria`;
CREATE TABLE `tbl_hasilkriteria` (
  `id` int(11) NOT NULL AUTO_INCREMENT,
  `id_kriteria` int(11) NOT NULL,
  `hasil` double NOT NULL DEFAULT '0',
  PRIMARY KEY (`id`),
  UNIQUE KEY `hasilkriteria` (`id_kriteria`)
) ENGINE=InnoDB AUTO_INCREMENT=15 DEFAULT CHARSET=latin1;

-- ----------------------------
-- Table structure for tbl_kriteria
-- ----------------------------
DROP TABLE IF EXISTS `tbl_kriteria`;
CREATE TABLE `tbl_kriteria` (
  `id` int(11) NOT NULL AUTO_INCREMENT,
  `kriteria` varchar(255) NOT NULL,
  `peringkat` int(11) NOT NULL,
  PRIMARY KEY (`id`),
  UNIQUE KEY `kriteria` (`kriteria`)
) ENGINE=InnoDB AUTO_INCREMENT=32 DEFAULT CHARSET=latin1;

-- ----------------------------
-- Table structure for tbl_pbalternatif
-- ----------------------------
DROP TABLE IF EXISTS `tbl_pbalternatif`;
CREATE TABLE `tbl_pbalternatif` (
  `id` int(11) NOT NULL AUTO_INCREMENT,
  `id_kriteria` int(11) NOT NULL,
  `id_alternatif` int(11) NOT NULL,
  `id_alternatif2` int(11) NOT NULL,
  `nilai` float NOT NULL DEFAULT '0',
  PRIMARY KEY (`id`),
  UNIQUE KEY `pbalternatif` (`id_alternatif`,`id_alternatif2`,`id_kriteria`)
) ENGINE=InnoDB DEFAULT CHARSET=latin1;

-- ----------------------------
-- Table structure for tbl_pbkriteria
-- ----------------------------
DROP TABLE IF EXISTS `tbl_pbkriteria`;
CREATE TABLE `tbl_pbkriteria` (
  `id` int(11) NOT NULL AUTO_INCREMENT,
  `id_kriteria` int(11) NOT NULL,
  `id_kriteria2` int(11) NOT NULL,
  `nilai` double NOT NULL DEFAULT '0',
  PRIMARY KEY (`id`),
  UNIQUE KEY `indexkriteria` (`id_kriteria`,`id_kriteria2`)
) ENGINE=InnoDB AUTO_INCREMENT=413 DEFAULT CHARSET=latin1;

-- ----------------------------
-- Table structure for tbl_penilaian
-- ----------------------------
DROP TABLE IF EXISTS `tbl_penilaian`;
CREATE TABLE `tbl_penilaian` (
  `id` int(11) NOT NULL AUTO_INCREMENT,
  `id_alternatif` int(11) NOT NULL,
  `id_kriteria` int(11) NOT NULL,
  `id_subkriteria` int(11) NOT NULL,
  PRIMARY KEY (`id`),
  UNIQUE KEY `penilaian` (`id_alternatif`,`id_kriteria`),
  CONSTRAINT `from_tbl_alternatif` FOREIGN KEY (`id_alternatif`) REFERENCES `tbl_alternatif` (`id`) ON DELETE CASCADE ON UPDATE CASCADE
) ENGINE=InnoDB AUTO_INCREMENT=70 DEFAULT CHARSET=latin1;

-- ----------------------------
-- Table structure for tbl_subkriteria
-- ----------------------------
DROP TABLE IF EXISTS `tbl_subkriteria`;
CREATE TABLE `tbl_subkriteria` (
  `id` int(11) NOT NULL AUTO_INCREMENT,
  `id_kriteria` int(11) NOT NULL,
  `subkriteria` varchar(255) NOT NULL,
  `skor` int(11) NOT NULL DEFAULT '0',
  PRIMARY KEY (`id`),
  UNIQUE KEY `subkriteria` (`id_kriteria`,`subkriteria`),
  CONSTRAINT `from_kritria` FOREIGN KEY (`id_kriteria`) REFERENCES `tbl_kriteria` (`id`) ON DELETE CASCADE ON UPDATE CASCADE
) ENGINE=InnoDB AUTO_INCREMENT=123 DEFAULT CHARSET=latin1;

-- ----------------------------
-- Table structure for tbl_user
-- ----------------------------
DROP TABLE IF EXISTS `tbl_user`;
CREATE TABLE `tbl_user` (
  `id` int(11) NOT NULL AUTO_INCREMENT,
  `username` varchar(255) NOT NULL,
  `password` varchar(255) NOT NULL,
  `level` varchar(50) NOT NULL,
  `nama` varchar(255) NOT NULL,
  PRIMARY KEY (`id`),
  UNIQUE KEY `user` (`username`)
) ENGINE=InnoDB AUTO_INCREMENT=8 DEFAULT CHARSET=latin1;

-- ----------------------------
-- View structure for v_alternatif
-- ----------------------------
DROP VIEW IF EXISTS `v_alternatif`;
CREATE ALGORITHM=UNDEFINED DEFINER=`root`@`localhost` SQL SECURITY DEFINER VIEW `v_alternatif` AS select `tbl_alternatif`.`id` AS `id`,`tbl_alternatif`.`alternatif` AS `Alternatif (Handphone)`,`tbl_alternatif`.`keterangan` AS `Keterangan` from `tbl_alternatif`;

-- ----------------------------
-- View structure for v_hasil
-- ----------------------------
DROP VIEW IF EXISTS `v_hasil`;
CREATE ALGORITHM=UNDEFINED DEFINER=`root`@`localhost` SQL SECURITY DEFINER VIEW `v_hasil` AS select `tbl_hasilalternatif`.`id_alternatif` AS `id_alternatif`,`tbl_alternatif`.`alternatif` AS `Alternatif`,sum((`tbl_hasilalternatif`.`hasil` * `tbl_hasilkriteria`.`hasil`)) AS `AHP`,`tbl_alternatif`.`keterangan` AS `Keterangan` from ((`tbl_hasilalternatif` join `tbl_alternatif` on((`tbl_hasilalternatif`.`id_alternatif` = `tbl_alternatif`.`id`))) join `tbl_hasilkriteria` on((`tbl_hasilalternatif`.`id_kriteria` = `tbl_hasilkriteria`.`id_kriteria`))) group by `tbl_hasilalternatif`.`id_alternatif`,`tbl_alternatif`.`alternatif`;

-- ----------------------------
-- View structure for v_kriteria
-- ----------------------------
DROP VIEW IF EXISTS `v_kriteria`;
CREATE ALGORITHM=UNDEFINED DEFINER=`root`@`localhost` SQL SECURITY DEFINER VIEW `v_kriteria` AS select `tbl_kriteria`.`id` AS `id`,`tbl_kriteria`.`kriteria` AS `Nama Kriteria`,`tbl_kriteria`.`peringkat` AS `Peringkat` from `tbl_kriteria`;

-- ----------------------------
-- View structure for v_lapalternatif
-- ----------------------------
DROP VIEW IF EXISTS `v_lapalternatif`;
CREATE ALGORITHM=UNDEFINED DEFINER=`root`@`localhost` SQL SECURITY DEFINER VIEW `v_lapalternatif` AS select `tbl_alternatif`.`alternatif` AS `Alternatif`,`tbl_alternatif`.`keterangan` AS `Keterangan` from `tbl_alternatif`;

-- ----------------------------
-- View structure for v_lapkriteria
-- ----------------------------
DROP VIEW IF EXISTS `v_lapkriteria`;
CREATE ALGORITHM=UNDEFINED DEFINER=`root`@`localhost` SQL SECURITY DEFINER VIEW `v_lapkriteria` AS select `tbl_kriteria`.`kriteria` AS `Nama Kriteria`,`tbl_kriteria`.`peringkat` AS `Peringkat` from `tbl_kriteria`;

-- ----------------------------
-- View structure for v_lappbkriteria
-- ----------------------------
DROP VIEW IF EXISTS `v_lappbkriteria`;
CREATE ALGORITHM=UNDEFINED DEFINER=`root`@`localhost` SQL SECURITY DEFINER VIEW `v_lappbkriteria` AS select (select `tbl_kriteria`.`kriteria` from `tbl_kriteria` where (`tbl_kriteria`.`id` = `tbl_pbkriteria`.`id_kriteria`)) AS `Kriteria`,(select `tbl_kriteria`.`kriteria` from `tbl_kriteria` where (`tbl_kriteria`.`id` = `tbl_pbkriteria`.`id_kriteria2`)) AS `Dengan Kriteria`,`tbl_pbkriteria`.`nilai` AS `Nilai Perbandingan` from `tbl_pbkriteria`;

-- ----------------------------
-- View structure for v_lapsubkriteria
-- ----------------------------
DROP VIEW IF EXISTS `v_lapsubkriteria`;
CREATE ALGORITHM=UNDEFINED DEFINER=`root`@`localhost` SQL SECURITY DEFINER VIEW `v_lapsubkriteria` AS select `tbl_subkriteria`.`subkriteria` AS `Nama Subkriteria`,`tbl_kriteria`.`kriteria` AS `Dari Kriteria`,`tbl_subkriteria`.`skor` AS `Skor` from (`tbl_kriteria` join `tbl_subkriteria` on((`tbl_kriteria`.`id` = `tbl_subkriteria`.`id_kriteria`)));

-- ----------------------------
-- View structure for v_subkriteria
-- ----------------------------
DROP VIEW IF EXISTS `v_subkriteria`;
CREATE ALGORITHM=UNDEFINED DEFINER=`root`@`localhost` SQL SECURITY DEFINER VIEW `v_subkriteria` AS select `tbl_subkriteria`.`id` AS `id`,`tbl_kriteria`.`kriteria` AS `Nama Kriteria`,`tbl_subkriteria`.`subkriteria` AS `Subkriteria`,`tbl_subkriteria`.`skor` AS `Skor` from (`tbl_kriteria` join `tbl_subkriteria` on((`tbl_subkriteria`.`id_kriteria` = `tbl_kriteria`.`id`)));

-- ----------------------------
-- View structure for v_user
-- ----------------------------
DROP VIEW IF EXISTS `v_user`;
CREATE ALGORITHM=UNDEFINED DEFINER=`root`@`localhost` SQL SECURITY DEFINER VIEW `v_user` AS select `tbl_user`.`id` AS `id`,`tbl_user`.`username` AS `Username`,`tbl_user`.`level` AS `Level`,`tbl_user`.`nama` AS `Nama Lengkap` from `tbl_user`;

-- ----------------------------
-- Records 
-- ----------------------------
INSERT INTO `tbl_alternatif` VALUES ('3', 'Samsung J5 Prime', '');
INSERT INTO `tbl_alternatif` VALUES ('4', 'Oppo A37', '');
INSERT INTO `tbl_alternatif` VALUES ('5', 'Vivo Y55S', '');
INSERT INTO `tbl_alternatif` VALUES ('6', 'Xiaomi Redmi 3 Pro', '');
INSERT INTO `tbl_hasilalternatif` VALUES ('1', '18', '3', '0.14814814814814814');
INSERT INTO `tbl_hasilalternatif` VALUES ('2', '18', '4', '0.25925925925925924');
INSERT INTO `tbl_hasilalternatif` VALUES ('3', '18', '5', '0.2962962962962963');
INSERT INTO `tbl_hasilalternatif` VALUES ('4', '18', '6', '0.2962962962962963');
INSERT INTO `tbl_hasilalternatif` VALUES ('5', '19', '3', '0.22727272727272727');
INSERT INTO `tbl_hasilalternatif` VALUES ('6', '19', '4', '0.22727272727272727');
INSERT INTO `tbl_hasilalternatif` VALUES ('7', '19', '5', '0.22727272727272727');
INSERT INTO `tbl_hasilalternatif` VALUES ('8', '19', '6', '0.3181818181818181');
INSERT INTO `tbl_hasilalternatif` VALUES ('9', '20', '3', '0.375');
INSERT INTO `tbl_hasilalternatif` VALUES ('10', '20', '4', '0.125');
INSERT INTO `tbl_hasilalternatif` VALUES ('11', '20', '5', '0.125');
INSERT INTO `tbl_hasilalternatif` VALUES ('12', '20', '6', '0.375');
INSERT INTO `tbl_hasilalternatif` VALUES ('13', '21', '3', '0.30000000000000004');
INSERT INTO `tbl_hasilalternatif` VALUES ('14', '21', '4', '0.26666666666666666');
INSERT INTO `tbl_hasilalternatif` VALUES ('15', '21', '5', '0.2');
INSERT INTO `tbl_hasilalternatif` VALUES ('16', '21', '6', '0.2333333333333333');
INSERT INTO `tbl_hasilalternatif` VALUES ('17', '22', '3', '0.2692307692307692');
INSERT INTO `tbl_hasilalternatif` VALUES ('18', '22', '4', '0.2692307692307692');
INSERT INTO `tbl_hasilalternatif` VALUES ('19', '22', '5', '0.19230769230769232');
INSERT INTO `tbl_hasilalternatif` VALUES ('20', '22', '6', '0.2692307692307692');
INSERT INTO `tbl_hasilalternatif` VALUES ('21', '23', '3', '0.23809523809523814');
INSERT INTO `tbl_hasilalternatif` VALUES ('22', '23', '4', '0.23809523809523814');
INSERT INTO `tbl_hasilalternatif` VALUES ('23', '23', '5', '0.1904761904761905');
INSERT INTO `tbl_hasilalternatif` VALUES ('24', '23', '6', '0.3333333333333333');
INSERT INTO `tbl_hasilalternatif` VALUES ('25', '24', '3', '0.375');
INSERT INTO `tbl_hasilalternatif` VALUES ('26', '24', '4', '0.125');
INSERT INTO `tbl_hasilalternatif` VALUES ('27', '24', '5', '0.125');
INSERT INTO `tbl_hasilalternatif` VALUES ('28', '24', '6', '0.375');
INSERT INTO `tbl_hasilalternatif` VALUES ('29', '25', '3', '0.25');
INSERT INTO `tbl_hasilalternatif` VALUES ('30', '25', '4', '0.25');
INSERT INTO `tbl_hasilalternatif` VALUES ('31', '25', '5', '0.25');
INSERT INTO `tbl_hasilalternatif` VALUES ('32', '25', '6', '0.25');
INSERT INTO `tbl_hasilalternatif` VALUES ('33', '26', '3', '0.2');
INSERT INTO `tbl_hasilalternatif` VALUES ('34', '26', '4', '0.30000000000000004');
INSERT INTO `tbl_hasilalternatif` VALUES ('35', '26', '5', '0.2');
INSERT INTO `tbl_hasilalternatif` VALUES ('36', '26', '6', '0.30000000000000004');
INSERT INTO `tbl_hasilalternatif` VALUES ('37', '27', '3', '0.2647058823529412');
INSERT INTO `tbl_hasilalternatif` VALUES ('38', '27', '4', '0.23529411764705882');
INSERT INTO `tbl_hasilalternatif` VALUES ('39', '27', '5', '0.23529411764705882');
INSERT INTO `tbl_hasilalternatif` VALUES ('40', '27', '6', '0.2647058823529412');
INSERT INTO `tbl_hasilalternatif` VALUES ('41', '28', '3', '0.2');
INSERT INTO `tbl_hasilalternatif` VALUES ('42', '28', '4', '0.2');
INSERT INTO `tbl_hasilalternatif` VALUES ('43', '28', '5', '0.27999999999999997');
INSERT INTO `tbl_hasilalternatif` VALUES ('44', '28', '6', '0.32');
INSERT INTO `tbl_hasilalternatif` VALUES ('45', '29', '3', '0.25');
INSERT INTO `tbl_hasilalternatif` VALUES ('46', '29', '4', '0.25');
INSERT INTO `tbl_hasilalternatif` VALUES ('47', '29', '5', '0.25');
INSERT INTO `tbl_hasilalternatif` VALUES ('48', '29', '6', '0.25');
INSERT INTO `tbl_hasilalternatif` VALUES ('49', '30', '3', '0.32142857142857145');
INSERT INTO `tbl_hasilalternatif` VALUES ('50', '30', '4', '0.25');
INSERT INTO `tbl_hasilalternatif` VALUES ('51', '30', '5', '0.1785714285714286');
INSERT INTO `tbl_hasilalternatif` VALUES ('52', '30', '6', '0.25');
INSERT INTO `tbl_hasilalternatif` VALUES ('53', '31', '3', '0.21875');
INSERT INTO `tbl_hasilalternatif` VALUES ('54', '31', '4', '0.28125');
INSERT INTO `tbl_hasilalternatif` VALUES ('55', '31', '5', '0.21875');
INSERT INTO `tbl_hasilalternatif` VALUES ('56', '31', '6', '0.28125');
INSERT INTO `tbl_hasilkriteria` VALUES ('1', '18', '0.21410868845506364');
INSERT INTO `tbl_hasilkriteria` VALUES ('2', '19', '0.15315168981223395');
INSERT INTO `tbl_hasilkriteria` VALUES ('3', '20', '0.09961831568558575');
INSERT INTO `tbl_hasilkriteria` VALUES ('4', '21', '0.04441007287082486');
INSERT INTO `tbl_hasilkriteria` VALUES ('5', '22', '0.17712969804199932');
INSERT INTO `tbl_hasilkriteria` VALUES ('6', '23', '0.06925928593590766');
INSERT INTO `tbl_hasilkriteria` VALUES ('7', '24', '0.06901964357296886');
INSERT INTO `tbl_hasilkriteria` VALUES ('8', '25', '0.01594807452955632');
INSERT INTO `tbl_hasilkriteria` VALUES ('9', '26', '0.06901964357296886');
INSERT INTO `tbl_hasilkriteria` VALUES ('10', '27', '0.015861557364550784');
INSERT INTO `tbl_hasilkriteria` VALUES ('11', '28', '0.010352677359705598');
INSERT INTO `tbl_hasilkriteria` VALUES ('12', '29', '0.00994912610869672');
INSERT INTO `tbl_hasilkriteria` VALUES ('13', '30', '0.026085763344968805');
INSERT INTO `tbl_hasilkriteria` VALUES ('14', '31', '0.026085763344968805');
INSERT INTO `tbl_kriteria` VALUES ('18', 'Harga', '1');
INSERT INTO `tbl_kriteria` VALUES ('19', 'Kamera Depan', '3');
INSERT INTO `tbl_kriteria` VALUES ('20', 'Kamera Belakang', '4');
INSERT INTO `tbl_kriteria` VALUES ('21', 'Brand', '7');
INSERT INTO `tbl_kriteria` VALUES ('22', 'RAM', '2');
INSERT INTO `tbl_kriteria` VALUES ('23', 'Memory Internal', '6');
INSERT INTO `tbl_kriteria` VALUES ('24', 'Baterai', '5');
INSERT INTO `tbl_kriteria` VALUES ('25', 'Resolusi', '9');
INSERT INTO `tbl_kriteria` VALUES ('26', 'CPU', '5');
INSERT INTO `tbl_kriteria` VALUES ('27', 'Display', '9');
INSERT INTO `tbl_kriteria` VALUES ('28', 'OS', '10');
INSERT INTO `tbl_kriteria` VALUES ('29', 'Bluetooth', '10');
INSERT INTO `tbl_kriteria` VALUES ('30', 'Sim', '8');
INSERT INTO `tbl_kriteria` VALUES ('31', 'Akses Data', '8');
INSERT INTO `tbl_pbkriteria` VALUES ('211', '18', '22', '3');
INSERT INTO `tbl_pbkriteria` VALUES ('212', '22', '18', '0.3333333333333333');
INSERT INTO `tbl_pbkriteria` VALUES ('213', '18', '19', '3');
INSERT INTO `tbl_pbkriteria` VALUES ('214', '19', '18', '0.3333333333333333');
INSERT INTO `tbl_pbkriteria` VALUES ('215', '22', '19', '3');
INSERT INTO `tbl_pbkriteria` VALUES ('216', '19', '22', '0.3333333333333333');
INSERT INTO `tbl_pbkriteria` VALUES ('217', '18', '20', '3');
INSERT INTO `tbl_pbkriteria` VALUES ('218', '20', '18', '0.3333333333333333');
INSERT INTO `tbl_pbkriteria` VALUES ('219', '22', '20', '3');
INSERT INTO `tbl_pbkriteria` VALUES ('220', '20', '22', '0.3333333333333333');
INSERT INTO `tbl_pbkriteria` VALUES ('221', '19', '20', '3');
INSERT INTO `tbl_pbkriteria` VALUES ('222', '20', '19', '0.3333333333333333');
INSERT INTO `tbl_pbkriteria` VALUES ('223', '24', '22', '0.2');
INSERT INTO `tbl_pbkriteria` VALUES ('224', '22', '24', '5');
INSERT INTO `tbl_pbkriteria` VALUES ('225', '24', '19', '0.2');
INSERT INTO `tbl_pbkriteria` VALUES ('226', '19', '24', '5');
INSERT INTO `tbl_pbkriteria` VALUES ('227', '26', '20', '0.3333333333333333');
INSERT INTO `tbl_pbkriteria` VALUES ('228', '20', '26', '3');
INSERT INTO `tbl_pbkriteria` VALUES ('235', '18', '21', '7');
INSERT INTO `tbl_pbkriteria` VALUES ('236', '21', '18', '0.14285714285714285');
INSERT INTO `tbl_pbkriteria` VALUES ('237', '19', '21', '7');
INSERT INTO `tbl_pbkriteria` VALUES ('238', '21', '19', '0.14285714285714285');
INSERT INTO `tbl_pbkriteria` VALUES ('239', '20', '21', '5');
INSERT INTO `tbl_pbkriteria` VALUES ('240', '21', '20', '0.2');
INSERT INTO `tbl_pbkriteria` VALUES ('247', '22', '21', '7');
INSERT INTO `tbl_pbkriteria` VALUES ('248', '21', '22', '0.14285714285714285');
INSERT INTO `tbl_pbkriteria` VALUES ('249', '23', '21', '3');
INSERT INTO `tbl_pbkriteria` VALUES ('250', '21', '23', '0.3333333333333333');
INSERT INTO `tbl_pbkriteria` VALUES ('251', '24', '21', '3');
INSERT INTO `tbl_pbkriteria` VALUES ('252', '21', '24', '0.3333333333333333');
INSERT INTO `tbl_pbkriteria` VALUES ('253', '26', '21', '3');
INSERT INTO `tbl_pbkriteria` VALUES ('254', '21', '26', '0.3333333333333333');
INSERT INTO `tbl_pbkriteria` VALUES ('255', '18', '23', '5');
INSERT INTO `tbl_pbkriteria` VALUES ('256', '23', '18', '0.2');
INSERT INTO `tbl_pbkriteria` VALUES ('257', '19', '23', '5');
INSERT INTO `tbl_pbkriteria` VALUES ('258', '23', '19', '0.2');
INSERT INTO `tbl_pbkriteria` VALUES ('259', '20', '23', '3');
INSERT INTO `tbl_pbkriteria` VALUES ('260', '23', '20', '0.3333333333333333');
INSERT INTO `tbl_pbkriteria` VALUES ('261', '22', '23', '5');
INSERT INTO `tbl_pbkriteria` VALUES ('262', '23', '22', '0.2');
INSERT INTO `tbl_pbkriteria` VALUES ('263', '24', '23', '3');
INSERT INTO `tbl_pbkriteria` VALUES ('264', '23', '24', '0.3333333333333333');
INSERT INTO `tbl_pbkriteria` VALUES ('265', '26', '23', '3');
INSERT INTO `tbl_pbkriteria` VALUES ('266', '23', '26', '0.3333333333333333');
INSERT INTO `tbl_pbkriteria` VALUES ('267', '18', '24', '5');
INSERT INTO `tbl_pbkriteria` VALUES ('268', '24', '18', '0.2');
INSERT INTO `tbl_pbkriteria` VALUES ('271', '20', '24', '3');
INSERT INTO `tbl_pbkriteria` VALUES ('272', '24', '20', '0.3333333333333333');
INSERT INTO `tbl_pbkriteria` VALUES ('275', '26', '24', '1');
INSERT INTO `tbl_pbkriteria` VALUES ('276', '24', '26', '1');
INSERT INTO `tbl_pbkriteria` VALUES ('277', '18', '25', '9');
INSERT INTO `tbl_pbkriteria` VALUES ('278', '25', '18', '0.1111111111111111');
INSERT INTO `tbl_pbkriteria` VALUES ('279', '19', '25', '9');
INSERT INTO `tbl_pbkriteria` VALUES ('280', '25', '19', '0.1111111111111111');
INSERT INTO `tbl_pbkriteria` VALUES ('281', '20', '25', '7');
INSERT INTO `tbl_pbkriteria` VALUES ('282', '25', '20', '0.14285714285714285');
INSERT INTO `tbl_pbkriteria` VALUES ('283', '21', '25', '5');
INSERT INTO `tbl_pbkriteria` VALUES ('284', '25', '21', '0.2');
INSERT INTO `tbl_pbkriteria` VALUES ('285', '22', '25', '9');
INSERT INTO `tbl_pbkriteria` VALUES ('286', '25', '22', '0.1111111111111111');
INSERT INTO `tbl_pbkriteria` VALUES ('287', '23', '25', '7');
INSERT INTO `tbl_pbkriteria` VALUES ('288', '25', '23', '0.14285714285714285');
INSERT INTO `tbl_pbkriteria` VALUES ('289', '24', '25', '5');
INSERT INTO `tbl_pbkriteria` VALUES ('290', '25', '24', '0.2');
INSERT INTO `tbl_pbkriteria` VALUES ('291', '26', '25', '5');
INSERT INTO `tbl_pbkriteria` VALUES ('292', '25', '26', '0.2');
INSERT INTO `tbl_pbkriteria` VALUES ('293', '27', '25', '1');
INSERT INTO `tbl_pbkriteria` VALUES ('294', '25', '27', '1');
INSERT INTO `tbl_pbkriteria` VALUES ('295', '30', '25', '3');
INSERT INTO `tbl_pbkriteria` VALUES ('296', '25', '30', '0.3333333333333333');
INSERT INTO `tbl_pbkriteria` VALUES ('297', '31', '25', '3');
INSERT INTO `tbl_pbkriteria` VALUES ('298', '25', '31', '0.3333333333333333');
INSERT INTO `tbl_pbkriteria` VALUES ('299', '18', '26', '5');
INSERT INTO `tbl_pbkriteria` VALUES ('300', '26', '18', '0.2');
INSERT INTO `tbl_pbkriteria` VALUES ('301', '19', '26', '5');
INSERT INTO `tbl_pbkriteria` VALUES ('302', '26', '19', '0.2');
INSERT INTO `tbl_pbkriteria` VALUES ('305', '22', '26', '5');
INSERT INTO `tbl_pbkriteria` VALUES ('306', '26', '22', '0.2');
INSERT INTO `tbl_pbkriteria` VALUES ('307', '18', '27', '9');
INSERT INTO `tbl_pbkriteria` VALUES ('308', '27', '18', '0.1111111111111111');
INSERT INTO `tbl_pbkriteria` VALUES ('309', '19', '27', '9');
INSERT INTO `tbl_pbkriteria` VALUES ('310', '27', '19', '0.1111111111111111');
INSERT INTO `tbl_pbkriteria` VALUES ('311', '20', '27', '7');
INSERT INTO `tbl_pbkriteria` VALUES ('312', '27', '20', '0.14285714285714285');
INSERT INTO `tbl_pbkriteria` VALUES ('313', '21', '27', '5');
INSERT INTO `tbl_pbkriteria` VALUES ('314', '27', '21', '0.2');
INSERT INTO `tbl_pbkriteria` VALUES ('315', '22', '27', '9');
INSERT INTO `tbl_pbkriteria` VALUES ('316', '27', '22', '0.1111111111111111');
INSERT INTO `tbl_pbkriteria` VALUES ('317', '23', '27', '9');
INSERT INTO `tbl_pbkriteria` VALUES ('318', '27', '23', '0.1111111111111111');
INSERT INTO `tbl_pbkriteria` VALUES ('319', '24', '27', '5');
INSERT INTO `tbl_pbkriteria` VALUES ('320', '27', '24', '0.2');
INSERT INTO `tbl_pbkriteria` VALUES ('323', '26', '27', '5');
INSERT INTO `tbl_pbkriteria` VALUES ('324', '27', '26', '0.2');
INSERT INTO `tbl_pbkriteria` VALUES ('325', '30', '27', '3');
INSERT INTO `tbl_pbkriteria` VALUES ('326', '27', '30', '0.3333333333333333');
INSERT INTO `tbl_pbkriteria` VALUES ('327', '31', '27', '3');
INSERT INTO `tbl_pbkriteria` VALUES ('328', '27', '31', '0.3333333333333333');
INSERT INTO `tbl_pbkriteria` VALUES ('329', '18', '28', '9');
INSERT INTO `tbl_pbkriteria` VALUES ('330', '28', '18', '0.1111111111111111');
INSERT INTO `tbl_pbkriteria` VALUES ('331', '19', '28', '9');
INSERT INTO `tbl_pbkriteria` VALUES ('332', '28', '19', '0.1111111111111111');
INSERT INTO `tbl_pbkriteria` VALUES ('333', '20', '28', '7');
INSERT INTO `tbl_pbkriteria` VALUES ('334', '28', '20', '0.14285714285714285');
INSERT INTO `tbl_pbkriteria` VALUES ('335', '21', '28', '7');
INSERT INTO `tbl_pbkriteria` VALUES ('336', '28', '21', '0.14285714285714285');
INSERT INTO `tbl_pbkriteria` VALUES ('337', '22', '28', '9');
INSERT INTO `tbl_pbkriteria` VALUES ('338', '28', '22', '0.1111111111111111');
INSERT INTO `tbl_pbkriteria` VALUES ('339', '23', '28', '9');
INSERT INTO `tbl_pbkriteria` VALUES ('340', '28', '23', '0.1111111111111111');
INSERT INTO `tbl_pbkriteria` VALUES ('341', '24', '28', '7');
INSERT INTO `tbl_pbkriteria` VALUES ('342', '28', '24', '0.14285714285714285');
INSERT INTO `tbl_pbkriteria` VALUES ('343', '25', '28', '3');
INSERT INTO `tbl_pbkriteria` VALUES ('344', '28', '25', '0.3333333333333333');
INSERT INTO `tbl_pbkriteria` VALUES ('345', '26', '28', '7');
INSERT INTO `tbl_pbkriteria` VALUES ('346', '28', '26', '0.14285714285714285');
INSERT INTO `tbl_pbkriteria` VALUES ('347', '27', '28', '3');
INSERT INTO `tbl_pbkriteria` VALUES ('348', '28', '27', '0.3333333333333333');
INSERT INTO `tbl_pbkriteria` VALUES ('349', '18', '29', '9');
INSERT INTO `tbl_pbkriteria` VALUES ('350', '29', '18', '0.1111111111111111');
INSERT INTO `tbl_pbkriteria` VALUES ('351', '19', '29', '9');
INSERT INTO `tbl_pbkriteria` VALUES ('352', '29', '19', '0.1111111111111111');
INSERT INTO `tbl_pbkriteria` VALUES ('353', '20', '29', '7');
INSERT INTO `tbl_pbkriteria` VALUES ('354', '29', '20', '0.14285714285714285');
INSERT INTO `tbl_pbkriteria` VALUES ('355', '21', '29', '7');
INSERT INTO `tbl_pbkriteria` VALUES ('356', '29', '21', '0.14285714285714285');
INSERT INTO `tbl_pbkriteria` VALUES ('357', '22', '29', '9');
INSERT INTO `tbl_pbkriteria` VALUES ('358', '29', '22', '0.1111111111111111');
INSERT INTO `tbl_pbkriteria` VALUES ('359', '23', '29', '9');
INSERT INTO `tbl_pbkriteria` VALUES ('360', '29', '23', '0.1111111111111111');
INSERT INTO `tbl_pbkriteria` VALUES ('361', '24', '29', '7');
INSERT INTO `tbl_pbkriteria` VALUES ('362', '29', '24', '0.14285714285714285');
INSERT INTO `tbl_pbkriteria` VALUES ('363', '25', '29', '3');
INSERT INTO `tbl_pbkriteria` VALUES ('364', '29', '25', '0.3333333333333333');
INSERT INTO `tbl_pbkriteria` VALUES ('365', '26', '29', '7');
INSERT INTO `tbl_pbkriteria` VALUES ('366', '29', '26', '0.14285714285714285');
INSERT INTO `tbl_pbkriteria` VALUES ('367', '27', '29', '3');
INSERT INTO `tbl_pbkriteria` VALUES ('368', '29', '27', '0.3333333333333333');
INSERT INTO `tbl_pbkriteria` VALUES ('369', '28', '29', '1');
INSERT INTO `tbl_pbkriteria` VALUES ('370', '29', '28', '1');
INSERT INTO `tbl_pbkriteria` VALUES ('371', '18', '30', '7');
INSERT INTO `tbl_pbkriteria` VALUES ('372', '30', '18', '0.14285714285714285');
INSERT INTO `tbl_pbkriteria` VALUES ('373', '19', '30', '7');
INSERT INTO `tbl_pbkriteria` VALUES ('374', '30', '19', '0.14285714285714285');
INSERT INTO `tbl_pbkriteria` VALUES ('375', '20', '30', '5');
INSERT INTO `tbl_pbkriteria` VALUES ('376', '30', '20', '0.2');
INSERT INTO `tbl_pbkriteria` VALUES ('377', '21', '30', '3');
INSERT INTO `tbl_pbkriteria` VALUES ('378', '30', '21', '0.3333333333333333');
INSERT INTO `tbl_pbkriteria` VALUES ('379', '22', '30', '7');
INSERT INTO `tbl_pbkriteria` VALUES ('380', '30', '22', '0.14285714285714285');
INSERT INTO `tbl_pbkriteria` VALUES ('381', '23', '30', '5');
INSERT INTO `tbl_pbkriteria` VALUES ('382', '30', '23', '0.2');
INSERT INTO `tbl_pbkriteria` VALUES ('383', '24', '30', '5');
INSERT INTO `tbl_pbkriteria` VALUES ('384', '30', '24', '0.2');
INSERT INTO `tbl_pbkriteria` VALUES ('385', '26', '30', '5');
INSERT INTO `tbl_pbkriteria` VALUES ('386', '30', '26', '0.2');
INSERT INTO `tbl_pbkriteria` VALUES ('387', '18', '31', '7');
INSERT INTO `tbl_pbkriteria` VALUES ('388', '31', '18', '0.14285714285714285');
INSERT INTO `tbl_pbkriteria` VALUES ('389', '19', '31', '7');
INSERT INTO `tbl_pbkriteria` VALUES ('390', '31', '19', '0.14285714285714285');
INSERT INTO `tbl_pbkriteria` VALUES ('391', '20', '31', '5');
INSERT INTO `tbl_pbkriteria` VALUES ('392', '31', '20', '0.2');
INSERT INTO `tbl_pbkriteria` VALUES ('393', '21', '31', '3');
INSERT INTO `tbl_pbkriteria` VALUES ('394', '31', '21', '0.3333333333333333');
INSERT INTO `tbl_pbkriteria` VALUES ('395', '22', '31', '7');
INSERT INTO `tbl_pbkriteria` VALUES ('396', '31', '22', '0.14285714285714285');
INSERT INTO `tbl_pbkriteria` VALUES ('397', '23', '31', '5');
INSERT INTO `tbl_pbkriteria` VALUES ('398', '31', '23', '0.2');
INSERT INTO `tbl_pbkriteria` VALUES ('399', '24', '31', '5');
INSERT INTO `tbl_pbkriteria` VALUES ('400', '31', '24', '0.2');
INSERT INTO `tbl_pbkriteria` VALUES ('401', '26', '31', '5');
INSERT INTO `tbl_pbkriteria` VALUES ('402', '31', '26', '0.2');
INSERT INTO `tbl_pbkriteria` VALUES ('403', '30', '31', '1');
INSERT INTO `tbl_pbkriteria` VALUES ('404', '31', '30', '1');
INSERT INTO `tbl_pbkriteria` VALUES ('405', '30', '28', '3');
INSERT INTO `tbl_pbkriteria` VALUES ('406', '28', '30', '0.3333333333333333');
INSERT INTO `tbl_pbkriteria` VALUES ('407', '31', '28', '3');
INSERT INTO `tbl_pbkriteria` VALUES ('408', '28', '31', '0.3333333333333333');
INSERT INTO `tbl_pbkriteria` VALUES ('409', '30', '29', '5');
INSERT INTO `tbl_pbkriteria` VALUES ('410', '29', '30', '0.2');
INSERT INTO `tbl_pbkriteria` VALUES ('411', '31', '29', '5');
INSERT INTO `tbl_pbkriteria` VALUES ('412', '29', '31', '0.2');
INSERT INTO `tbl_penilaian` VALUES ('9', '3', '19', '70');
INSERT INTO `tbl_penilaian` VALUES ('10', '3', '18', '68');
INSERT INTO `tbl_penilaian` VALUES ('11', '4', '18', '67');
INSERT INTO `tbl_penilaian` VALUES ('12', '5', '18', '66');
INSERT INTO `tbl_penilaian` VALUES ('13', '6', '18', '66');
INSERT INTO `tbl_penilaian` VALUES ('15', '4', '19', '70');
INSERT INTO `tbl_penilaian` VALUES ('16', '5', '19', '70');
INSERT INTO `tbl_penilaian` VALUES ('17', '6', '19', '71');
INSERT INTO `tbl_penilaian` VALUES ('18', '3', '20', '76');
INSERT INTO `tbl_penilaian` VALUES ('19', '4', '20', '75');
INSERT INTO `tbl_penilaian` VALUES ('20', '5', '20', '75');
INSERT INTO `tbl_penilaian` VALUES ('21', '6', '20', '76');
INSERT INTO `tbl_penilaian` VALUES ('22', '3', '21', '79');
INSERT INTO `tbl_penilaian` VALUES ('23', '4', '21', '81');
INSERT INTO `tbl_penilaian` VALUES ('24', '5', '21', '82');
INSERT INTO `tbl_penilaian` VALUES ('25', '6', '21', '80');
INSERT INTO `tbl_penilaian` VALUES ('26', '3', '22', '87');
INSERT INTO `tbl_penilaian` VALUES ('27', '4', '22', '87');
INSERT INTO `tbl_penilaian` VALUES ('28', '5', '22', '85');
INSERT INTO `tbl_penilaian` VALUES ('29', '6', '22', '87');
INSERT INTO `tbl_penilaian` VALUES ('30', '3', '23', '91');
INSERT INTO `tbl_penilaian` VALUES ('31', '4', '23', '91');
INSERT INTO `tbl_penilaian` VALUES ('32', '5', '23', '90');
INSERT INTO `tbl_penilaian` VALUES ('33', '6', '23', '92');
INSERT INTO `tbl_penilaian` VALUES ('34', '3', '24', '95');
INSERT INTO `tbl_penilaian` VALUES ('35', '4', '24', '94');
INSERT INTO `tbl_penilaian` VALUES ('36', '5', '24', '94');
INSERT INTO `tbl_penilaian` VALUES ('37', '6', '24', '96');
INSERT INTO `tbl_penilaian` VALUES ('38', '3', '25', '99');
INSERT INTO `tbl_penilaian` VALUES ('39', '4', '25', '99');
INSERT INTO `tbl_penilaian` VALUES ('40', '5', '25', '99');
INSERT INTO `tbl_penilaian` VALUES ('41', '6', '25', '99');
INSERT INTO `tbl_penilaian` VALUES ('42', '3', '26', '102');
INSERT INTO `tbl_penilaian` VALUES ('46', '4', '26', '103');
INSERT INTO `tbl_penilaian` VALUES ('47', '5', '26', '102');
INSERT INTO `tbl_penilaian` VALUES ('48', '6', '26', '103');
INSERT INTO `tbl_penilaian` VALUES ('49', '3', '27', '107');
INSERT INTO `tbl_penilaian` VALUES ('50', '4', '27', '106');
INSERT INTO `tbl_penilaian` VALUES ('51', '5', '27', '106');
INSERT INTO `tbl_penilaian` VALUES ('52', '6', '27', '107');
INSERT INTO `tbl_penilaian` VALUES ('53', '3', '28', '111');
INSERT INTO `tbl_penilaian` VALUES ('54', '4', '28', '111');
INSERT INTO `tbl_penilaian` VALUES ('55', '5', '28', '110');
INSERT INTO `tbl_penilaian` VALUES ('56', '6', '28', '109');
INSERT INTO `tbl_penilaian` VALUES ('57', '3', '29', '113');
INSERT INTO `tbl_penilaian` VALUES ('58', '4', '29', '113');
INSERT INTO `tbl_penilaian` VALUES ('59', '5', '29', '113');
INSERT INTO `tbl_penilaian` VALUES ('60', '6', '29', '113');
INSERT INTO `tbl_penilaian` VALUES ('61', '3', '30', '118');
INSERT INTO `tbl_penilaian` VALUES ('62', '4', '30', '117');
INSERT INTO `tbl_penilaian` VALUES ('63', '5', '30', '116');
INSERT INTO `tbl_penilaian` VALUES ('64', '6', '30', '117');
INSERT INTO `tbl_penilaian` VALUES ('65', '3', '31', '120');
INSERT INTO `tbl_penilaian` VALUES ('66', '4', '31', '119');
INSERT INTO `tbl_penilaian` VALUES ('67', '5', '31', '120');
INSERT INTO `tbl_penilaian` VALUES ('68', '6', '31', '119');
INSERT INTO `tbl_penilaian` VALUES ('69', '3', '32', '123');
INSERT INTO `tbl_subkriteria` VALUES ('65', '18', '< 500000-1', '1');
INSERT INTO `tbl_subkriteria` VALUES ('66', '18', '500000 - 1500000-8', '8');
INSERT INTO `tbl_subkriteria` VALUES ('67', '18', '1500000 - 2500000-7', '7');
INSERT INTO `tbl_subkriteria` VALUES ('68', '18', '2500000 - 3500000-4', '4');
INSERT INTO `tbl_subkriteria` VALUES ('69', '18', '> 3500000-2', '2');
INSERT INTO `tbl_subkriteria` VALUES ('70', '19', '5 mp-5', '5');
INSERT INTO `tbl_subkriteria` VALUES ('71', '19', '8 mp-7', '7');
INSERT INTO `tbl_subkriteria` VALUES ('72', '19', '13 mp-8', '8');
INSERT INTO `tbl_subkriteria` VALUES ('73', '19', '16 mp-9', '9');
INSERT INTO `tbl_subkriteria` VALUES ('74', '20', '5 mp-1', '1');
INSERT INTO `tbl_subkriteria` VALUES ('75', '20', '8 mp-2', '2');
INSERT INTO `tbl_subkriteria` VALUES ('76', '20', '13 mp-6', '6');
INSERT INTO `tbl_subkriteria` VALUES ('77', '20', '16 mp-7', '7');
INSERT INTO `tbl_subkriteria` VALUES ('78', '20', '20 mp-8', '8');
INSERT INTO `tbl_subkriteria` VALUES ('79', '21', 'Samsung-9', '9');
INSERT INTO `tbl_subkriteria` VALUES ('80', '21', 'Xiaomy-7', '7');
INSERT INTO `tbl_subkriteria` VALUES ('81', '21', 'Oppo-8', '8');
INSERT INTO `tbl_subkriteria` VALUES ('82', '21', 'Vivo-6', '6');
INSERT INTO `tbl_subkriteria` VALUES ('83', '21', 'Asus-5', '5');
INSERT INTO `tbl_subkriteria` VALUES ('84', '22', '512 GB-2', '2');
INSERT INTO `tbl_subkriteria` VALUES ('85', '22', '1 GB-5', '5');
INSERT INTO `tbl_subkriteria` VALUES ('86', '22', '1,5 GB-5', '5');
INSERT INTO `tbl_subkriteria` VALUES ('87', '22', '2 GB-7', '7');
INSERT INTO `tbl_subkriteria` VALUES ('88', '22', '3 GB-8', '8');
INSERT INTO `tbl_subkriteria` VALUES ('89', '22', '4 GB-9', '9');
INSERT INTO `tbl_subkriteria` VALUES ('90', '23', '8 GB-4', '4');
INSERT INTO `tbl_subkriteria` VALUES ('91', '23', '16 GB-5', '5');
INSERT INTO `tbl_subkriteria` VALUES ('92', '23', '32 GB-7', '7');
INSERT INTO `tbl_subkriteria` VALUES ('93', '23', '64 GB-9', '9');
INSERT INTO `tbl_subkriteria` VALUES ('94', '24', '1020 mah-2', '2');
INSERT INTO `tbl_subkriteria` VALUES ('95', '24', '2500 mah-6', '6');
INSERT INTO `tbl_subkriteria` VALUES ('96', '24', '3000 mah-6', '6');
INSERT INTO `tbl_subkriteria` VALUES ('97', '24', '4000 mah-8', '8');
INSERT INTO `tbl_subkriteria` VALUES ('98', '24', '5200 mah-9', '9');
INSERT INTO `tbl_subkriteria` VALUES ('99', '25', '720 x 1280 pixels-6', '6');
INSERT INTO `tbl_subkriteria` VALUES ('100', '25', '1080 x 1366 pixels-7', '7');
INSERT INTO `tbl_subkriteria` VALUES ('101', '25', '1440 x 2560 pixels-8', '8');
INSERT INTO `tbl_subkriteria` VALUES ('102', '26', 'Dual Core-6', '6');
INSERT INTO `tbl_subkriteria` VALUES ('103', '26', 'Quad-Core-9', '9');
INSERT INTO `tbl_subkriteria` VALUES ('104', '26', 'Octa-Core-8', '8');
INSERT INTO `tbl_subkriteria` VALUES ('105', '27', '3 inch > 4 inch-5', '5');
INSERT INTO `tbl_subkriteria` VALUES ('106', '27', '4 inch > 5 inch-8', '8');
INSERT INTO `tbl_subkriteria` VALUES ('107', '27', '5 inch > 6 inch-9', '9');
INSERT INTO `tbl_subkriteria` VALUES ('108', '27', '> 6 inch-6', '6');
INSERT INTO `tbl_subkriteria` VALUES ('109', '28', 'Android v7.0 Nougat-8', '8');
INSERT INTO `tbl_subkriteria` VALUES ('110', '28', 'Android v6.0 Marshmallow-7', '7');
INSERT INTO `tbl_subkriteria` VALUES ('111', '28', 'Android v5.0 - 5.1 Lollipop-5', '5');
INSERT INTO `tbl_subkriteria` VALUES ('112', '28', 'Android v4.4 Kitkat-5', '5');
INSERT INTO `tbl_subkriteria` VALUES ('113', '29', 'Bluetooth V4.0-9', '9');
INSERT INTO `tbl_subkriteria` VALUES ('114', '29', 'Bluetooth V3.0-7', '7');
INSERT INTO `tbl_subkriteria` VALUES ('115', '29', 'Bluetooth V2.0-5', '5');
INSERT INTO `tbl_subkriteria` VALUES ('116', '30', 'Nano SIM-5', '5');
INSERT INTO `tbl_subkriteria` VALUES ('117', '30', 'Dual SIM-7', '7');
INSERT INTO `tbl_subkriteria` VALUES ('118', '30', 'Dual SIM Stand By-9', '9');
INSERT INTO `tbl_subkriteria` VALUES ('119', '31', '4G-9', '9');
INSERT INTO `tbl_subkriteria` VALUES ('120', '31', 'HSDPA-7', '7');
INSERT INTO `tbl_subkriteria` VALUES ('121', '31', '3G-1', '1');
INSERT INTO `tbl_subkriteria` VALUES ('122', '31', 'Edge-1', '1');
INSERT INTO `tbl_user` VALUES ('6', 'administrator', 'admin', 'level 1', '');
INSERT INTO `tbl_user` VALUES ('7', 'admin', 'admin', 'level 2', '');
