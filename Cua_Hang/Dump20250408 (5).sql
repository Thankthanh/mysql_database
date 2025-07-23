CREATE DATABASE  IF NOT EXISTS `cosmetics_shop` /*!40100 DEFAULT CHARACTER SET utf8mb4 COLLATE utf8mb4_0900_ai_ci */ /*!80016 DEFAULT ENCRYPTION='N' */;
USE `cosmetics_shop`;
-- MySQL dump 10.13  Distrib 8.0.41, for Win64 (x86_64)
--
-- Host: localhost    Database: cosmetics_shop
-- ------------------------------------------------------
-- Server version	8.0.41

/*!40101 SET @OLD_CHARACTER_SET_CLIENT=@@CHARACTER_SET_CLIENT */;
/*!40101 SET @OLD_CHARACTER_SET_RESULTS=@@CHARACTER_SET_RESULTS */;
/*!40101 SET @OLD_COLLATION_CONNECTION=@@COLLATION_CONNECTION */;
/*!50503 SET NAMES utf8 */;
/*!40103 SET @OLD_TIME_ZONE=@@TIME_ZONE */;
/*!40103 SET TIME_ZONE='+00:00' */;
/*!40014 SET @OLD_UNIQUE_CHECKS=@@UNIQUE_CHECKS, UNIQUE_CHECKS=0 */;
/*!40014 SET @OLD_FOREIGN_KEY_CHECKS=@@FOREIGN_KEY_CHECKS, FOREIGN_KEY_CHECKS=0 */;
/*!40101 SET @OLD_SQL_MODE=@@SQL_MODE, SQL_MODE='NO_AUTO_VALUE_ON_ZERO' */;
/*!40111 SET @OLD_SQL_NOTES=@@SQL_NOTES, SQL_NOTES=0 */;

--
-- Table structure for table `chi_tiet_don_hang`
--

DROP TABLE IF EXISTS `chi_tiet_don_hang`;
/*!40101 SET @saved_cs_client     = @@character_set_client */;
/*!50503 SET character_set_client = utf8mb4 */;
CREATE TABLE `chi_tiet_don_hang` (
  `Ma_Don_Hang` int NOT NULL,
  `Ma_San_Pham` int NOT NULL,
  `So_Luong` int NOT NULL,
  `Tieu_Khoan` decimal(10,2) NOT NULL,
  PRIMARY KEY (`Ma_Don_Hang`,`Ma_San_Pham`),
  KEY `Ma_San_Pham` (`Ma_San_Pham`),
  CONSTRAINT `chi_tiet_don_hang_ibfk_1` FOREIGN KEY (`Ma_Don_Hang`) REFERENCES `don_hang` (`Ma_Don_Hang`),
  CONSTRAINT `chi_tiet_don_hang_ibfk_2` FOREIGN KEY (`Ma_San_Pham`) REFERENCES `san_pham` (`Ma_San_Pham`),
  CONSTRAINT `check_so_luong_don_hang` CHECK ((`So_Luong` > 0))
) ENGINE=InnoDB DEFAULT CHARSET=utf8mb4 COLLATE=utf8mb4_0900_ai_ci;
/*!40101 SET character_set_client = @saved_cs_client */;

--
-- Dumping data for table `chi_tiet_don_hang`
--

LOCK TABLES `chi_tiet_don_hang` WRITE;
/*!40000 ALTER TABLE `chi_tiet_don_hang` DISABLE KEYS */;
INSERT INTO `chi_tiet_don_hang` VALUES (2,5,2,240000.00),(2,9,5,1400000.00),(3,1,1,200000.00),(3,3,2,300000.00),(4,6,1,250000.00),(5,10,3,570000.00);
/*!40000 ALTER TABLE `chi_tiet_don_hang` ENABLE KEYS */;
UNLOCK TABLES;
/*!50003 SET @saved_cs_client      = @@character_set_client */ ;
/*!50003 SET @saved_cs_results     = @@character_set_results */ ;
/*!50003 SET @saved_col_connection = @@collation_connection */ ;
/*!50003 SET character_set_client  = utf8mb4 */ ;
/*!50003 SET character_set_results = utf8mb4 */ ;
/*!50003 SET collation_connection  = utf8mb4_0900_ai_ci */ ;
/*!50003 SET @saved_sql_mode       = @@sql_mode */ ;
/*!50003 SET sql_mode              = 'ONLY_FULL_GROUP_BY,STRICT_TRANS_TABLES,NO_ZERO_IN_DATE,NO_ZERO_DATE,ERROR_FOR_DIVISION_BY_ZERO,NO_ENGINE_SUBSTITUTION' */ ;
DELIMITER ;;
/*!50003 CREATE*/ /*!50017 DEFINER=`root`@`localhost`*/ /*!50003 TRIGGER `before_chi_tiet_don_hang_insert` BEFORE INSERT ON `chi_tiet_don_hang` FOR EACH ROW BEGIN
    DECLARE ton_kho INT;
    DECLARE product_exists INT;

    -- Kiểm tra sản phẩm có tồn tại không
    SELECT COUNT(*) INTO product_exists
    FROM `san_pham`
    WHERE `Ma_San_Pham` = NEW.`Ma_San_Pham`;

    IF product_exists = 0 THEN
        SIGNAL SQLSTATE '45000'
        SET MESSAGE_TEXT = 'Sản phẩm không tồn tại!';
    END IF;

    -- Kiểm tra số lượng tồn kho
    SELECT `So_Luong_Ton_Kho` INTO ton_kho
    FROM `san_pham`
    WHERE `Ma_San_Pham` = NEW.`Ma_San_Pham`;

    IF ton_kho < NEW.`So_Luong` THEN
        SIGNAL SQLSTATE '45000'
        SET MESSAGE_TEXT = 'Số lượng tồn kho không đủ!';
    END IF;
END */;;
DELIMITER ;
/*!50003 SET sql_mode              = @saved_sql_mode */ ;
/*!50003 SET character_set_client  = @saved_cs_client */ ;
/*!50003 SET character_set_results = @saved_cs_results */ ;
/*!50003 SET collation_connection  = @saved_col_connection */ ;
/*!50003 SET @saved_cs_client      = @@character_set_client */ ;
/*!50003 SET @saved_cs_results     = @@character_set_results */ ;
/*!50003 SET @saved_col_connection = @@collation_connection */ ;
/*!50003 SET character_set_client  = utf8mb4 */ ;
/*!50003 SET character_set_results = utf8mb4 */ ;
/*!50003 SET collation_connection  = utf8mb4_0900_ai_ci */ ;
/*!50003 SET @saved_sql_mode       = @@sql_mode */ ;
/*!50003 SET sql_mode              = 'ONLY_FULL_GROUP_BY,STRICT_TRANS_TABLES,NO_ZERO_IN_DATE,NO_ZERO_DATE,ERROR_FOR_DIVISION_BY_ZERO,NO_ENGINE_SUBSTITUTION' */ ;
DELIMITER ;;
/*!50003 CREATE*/ /*!50017 DEFINER=`root`@`localhost`*/ /*!50003 TRIGGER `after_chi_tiet_don_hang_insert_tong_tien` AFTER INSERT ON `chi_tiet_don_hang` FOR EACH ROW BEGIN
    UPDATE `don_hang`
    SET `Tong_Tien` = (
        SELECT SUM(`Tieu_Khoan`)
        FROM `chi_tiet_don_hang`
        WHERE `Ma_Don_Hang` = NEW.`Ma_Don_Hang`
    )
    WHERE `Ma_Don_Hang` = NEW.`Ma_Don_Hang`;
END */;;
DELIMITER ;
/*!50003 SET sql_mode              = @saved_sql_mode */ ;
/*!50003 SET character_set_client  = @saved_cs_client */ ;
/*!50003 SET character_set_results = @saved_cs_results */ ;
/*!50003 SET collation_connection  = @saved_col_connection */ ;
/*!50003 SET @saved_cs_client      = @@character_set_client */ ;
/*!50003 SET @saved_cs_results     = @@character_set_results */ ;
/*!50003 SET @saved_col_connection = @@collation_connection */ ;
/*!50003 SET character_set_client  = utf8mb4 */ ;
/*!50003 SET character_set_results = utf8mb4 */ ;
/*!50003 SET collation_connection  = utf8mb4_0900_ai_ci */ ;
/*!50003 SET @saved_sql_mode       = @@sql_mode */ ;
/*!50003 SET sql_mode              = 'ONLY_FULL_GROUP_BY,STRICT_TRANS_TABLES,NO_ZERO_IN_DATE,NO_ZERO_DATE,ERROR_FOR_DIVISION_BY_ZERO,NO_ENGINE_SUBSTITUTION' */ ;
DELIMITER ;;
/*!50003 CREATE*/ /*!50017 DEFINER=`root`@`localhost`*/ /*!50003 TRIGGER `after_chi_tiet_don_hang_insert` AFTER INSERT ON `chi_tiet_don_hang` FOR EACH ROW BEGIN
    -- Cập nhật số lượng tồn kho
    UPDATE `san_pham`
    SET `So_Luong_Ton_Kho` = `So_Luong_Ton_Kho` - NEW.`So_Luong`
    WHERE `Ma_San_Pham` = NEW.`Ma_San_Pham`;
END */;;
DELIMITER ;
/*!50003 SET sql_mode              = @saved_sql_mode */ ;
/*!50003 SET character_set_client  = @saved_cs_client */ ;
/*!50003 SET character_set_results = @saved_cs_results */ ;
/*!50003 SET collation_connection  = @saved_col_connection */ ;
/*!50003 SET @saved_cs_client      = @@character_set_client */ ;
/*!50003 SET @saved_cs_results     = @@character_set_results */ ;
/*!50003 SET @saved_col_connection = @@collation_connection */ ;
/*!50003 SET character_set_client  = utf8mb4 */ ;
/*!50003 SET character_set_results = utf8mb4 */ ;
/*!50003 SET collation_connection  = utf8mb4_0900_ai_ci */ ;
/*!50003 SET @saved_sql_mode       = @@sql_mode */ ;
/*!50003 SET sql_mode              = 'ONLY_FULL_GROUP_BY,STRICT_TRANS_TABLES,NO_ZERO_IN_DATE,NO_ZERO_DATE,ERROR_FOR_DIVISION_BY_ZERO,NO_ENGINE_SUBSTITUTION' */ ;
DELIMITER ;;
/*!50003 CREATE*/ /*!50017 DEFINER=`root`@`localhost`*/ /*!50003 TRIGGER `before_chi_tiet_don_hang_update` BEFORE UPDATE ON `chi_tiet_don_hang` FOR EACH ROW BEGIN
    DECLARE ton_kho INT;

    -- Lấy số lượng tồn kho hiện tại
    SELECT `So_Luong_Ton_Kho` INTO ton_kho
    FROM `san_pham`
    WHERE `Ma_San_Pham` = NEW.`Ma_San_Pham`;

    -- Kiểm tra nếu số lượng mới vượt quá tồn kho hiện tại + số lượng cũ
    IF (ton_kho + OLD.`So_Luong`) < NEW.`So_Luong` THEN
        SIGNAL SQLSTATE '45000'
        SET MESSAGE_TEXT = 'Số lượng tồn kho không đủ để sửa đổi!';
    END IF;
END */;;
DELIMITER ;
/*!50003 SET sql_mode              = @saved_sql_mode */ ;
/*!50003 SET character_set_client  = @saved_cs_client */ ;
/*!50003 SET character_set_results = @saved_cs_results */ ;
/*!50003 SET collation_connection  = @saved_col_connection */ ;
/*!50003 SET @saved_cs_client      = @@character_set_client */ ;
/*!50003 SET @saved_cs_results     = @@character_set_results */ ;
/*!50003 SET @saved_col_connection = @@collation_connection */ ;
/*!50003 SET character_set_client  = utf8mb4 */ ;
/*!50003 SET character_set_results = utf8mb4 */ ;
/*!50003 SET collation_connection  = utf8mb4_0900_ai_ci */ ;
/*!50003 SET @saved_sql_mode       = @@sql_mode */ ;
/*!50003 SET sql_mode              = 'ONLY_FULL_GROUP_BY,STRICT_TRANS_TABLES,NO_ZERO_IN_DATE,NO_ZERO_DATE,ERROR_FOR_DIVISION_BY_ZERO,NO_ENGINE_SUBSTITUTION' */ ;
DELIMITER ;;
/*!50003 CREATE*/ /*!50017 DEFINER=`root`@`localhost`*/ /*!50003 TRIGGER `after_chi_tiet_don_hang_update` AFTER UPDATE ON `chi_tiet_don_hang` FOR EACH ROW BEGIN
    -- Cập nhật số lượng tồn kho (trừ số lượng cũ, cộng số lượng mới)
    UPDATE `san_pham`
    SET `So_Luong_Ton_Kho` = `So_Luong_Ton_Kho` + OLD.`So_Luong` - NEW.`So_Luong`
    WHERE `Ma_San_Pham` = NEW.`Ma_San_Pham`;

    -- Cập nhật tổng tiền
    UPDATE `don_hang`
    SET `Tong_Tien` = (
        SELECT SUM(`Tieu_Khoan`)
        FROM `chi_tiet_don_hang`
        WHERE `Ma_Don_Hang` = NEW.`Ma_Don_Hang`
    )
    WHERE `Ma_Don_Hang` = NEW.`Ma_Don_Hang`;
END */;;
DELIMITER ;
/*!50003 SET sql_mode              = @saved_sql_mode */ ;
/*!50003 SET character_set_client  = @saved_cs_client */ ;
/*!50003 SET character_set_results = @saved_cs_results */ ;
/*!50003 SET collation_connection  = @saved_col_connection */ ;
/*!50003 SET @saved_cs_client      = @@character_set_client */ ;
/*!50003 SET @saved_cs_results     = @@character_set_results */ ;
/*!50003 SET @saved_col_connection = @@collation_connection */ ;
/*!50003 SET character_set_client  = utf8mb4 */ ;
/*!50003 SET character_set_results = utf8mb4 */ ;
/*!50003 SET collation_connection  = utf8mb4_0900_ai_ci */ ;
/*!50003 SET @saved_sql_mode       = @@sql_mode */ ;
/*!50003 SET sql_mode              = 'ONLY_FULL_GROUP_BY,STRICT_TRANS_TABLES,NO_ZERO_IN_DATE,NO_ZERO_DATE,ERROR_FOR_DIVISION_BY_ZERO,NO_ENGINE_SUBSTITUTION' */ ;
DELIMITER ;;
/*!50003 CREATE*/ /*!50017 DEFINER=`root`@`localhost`*/ /*!50003 TRIGGER `after_chi_tiet_don_hang_delete` AFTER DELETE ON `chi_tiet_don_hang` FOR EACH ROW BEGIN
    -- Cập nhật số lượng tồn kho (hoàn lại số lượng đã bán)
    UPDATE `san_pham`
    SET `So_Luong_Ton_Kho` = `So_Luong_Ton_Kho` + OLD.`So_Luong`
    WHERE `Ma_San_Pham` = OLD.`Ma_San_Pham`;

    -- Cập nhật tổng tiền
    UPDATE `don_hang`
    SET `Tong_Tien` = (
        SELECT SUM(`Tieu_Khoan`)
        FROM `chi_tiet_don_hang`
        WHERE `Ma_Don_Hang` = OLD.`Ma_Don_Hang`
    )
    WHERE `Ma_Don_Hang` = OLD.`Ma_Don_Hang`;
END */;;
DELIMITER ;
/*!50003 SET sql_mode              = @saved_sql_mode */ ;
/*!50003 SET character_set_client  = @saved_cs_client */ ;
/*!50003 SET character_set_results = @saved_cs_results */ ;
/*!50003 SET collation_connection  = @saved_col_connection */ ;

--
-- Table structure for table `chi_tiet_nhap_hang`
--

DROP TABLE IF EXISTS `chi_tiet_nhap_hang`;
/*!40101 SET @saved_cs_client     = @@character_set_client */;
/*!50503 SET character_set_client = utf8mb4 */;
CREATE TABLE `chi_tiet_nhap_hang` (
  `Ma_Nhap_Hang` int NOT NULL,
  `Ma_San_Pham` int NOT NULL,
  `So_Luong` int NOT NULL,
  `Gia_Nhap` decimal(10,2) NOT NULL,
  PRIMARY KEY (`Ma_Nhap_Hang`,`Ma_San_Pham`),
  KEY `Ma_San_Pham` (`Ma_San_Pham`),
  CONSTRAINT `chi_tiet_nhap_hang_ibfk_1` FOREIGN KEY (`Ma_Nhap_Hang`) REFERENCES `nhap_hang` (`Ma_Nhap_Hang`),
  CONSTRAINT `chi_tiet_nhap_hang_ibfk_2` FOREIGN KEY (`Ma_San_Pham`) REFERENCES `san_pham` (`Ma_San_Pham`),
  CONSTRAINT `check_so_luong_nhap_hang` CHECK ((`So_Luong` > 0))
) ENGINE=InnoDB DEFAULT CHARSET=utf8mb4 COLLATE=utf8mb4_0900_ai_ci;
/*!40101 SET character_set_client = @saved_cs_client */;

--
-- Dumping data for table `chi_tiet_nhap_hang`
--

LOCK TABLES `chi_tiet_nhap_hang` WRITE;
/*!40000 ALTER TABLE `chi_tiet_nhap_hang` DISABLE KEYS */;
INSERT INTO `chi_tiet_nhap_hang` VALUES (1,1,20,150000.00),(1,8,30,70000.00),(2,2,15,250000.00),(3,4,25,130000.00),(4,5,70,80000.00),(5,9,20,200000.00),(6,3,10,100000.00),(7,6,10,180000.00),(8,10,10,130000.00);
/*!40000 ALTER TABLE `chi_tiet_nhap_hang` ENABLE KEYS */;
UNLOCK TABLES;
/*!50003 SET @saved_cs_client      = @@character_set_client */ ;
/*!50003 SET @saved_cs_results     = @@character_set_results */ ;
/*!50003 SET @saved_col_connection = @@collation_connection */ ;
/*!50003 SET character_set_client  = utf8mb4 */ ;
/*!50003 SET character_set_results = utf8mb4 */ ;
/*!50003 SET collation_connection  = utf8mb4_0900_ai_ci */ ;
/*!50003 SET @saved_sql_mode       = @@sql_mode */ ;
/*!50003 SET sql_mode              = 'ONLY_FULL_GROUP_BY,STRICT_TRANS_TABLES,NO_ZERO_IN_DATE,NO_ZERO_DATE,ERROR_FOR_DIVISION_BY_ZERO,NO_ENGINE_SUBSTITUTION' */ ;
DELIMITER ;;
/*!50003 CREATE*/ /*!50017 DEFINER=`root`@`localhost`*/ /*!50003 TRIGGER `after_chi_tiet_nhap_hang_insert` AFTER INSERT ON `chi_tiet_nhap_hang` FOR EACH ROW BEGIN
    -- Cập nhật tổng chi phí trong nhap_hang
    UPDATE `nhap_hang`
    SET `Tong_Chi_Phi` = (
        SELECT SUM(`So_Luong` * `Gia_Nhap`)
        FROM `chi_tiet_nhap_hang`
        WHERE `Ma_Nhap_Hang` = NEW.`Ma_Nhap_Hang`
    )
    WHERE `Ma_Nhap_Hang` = NEW.`Ma_Nhap_Hang`;

    -- Cập nhật số lượng tồn kho trong san_pham
    UPDATE `san_pham`
    SET `So_Luong_Ton_Kho` = `So_Luong_Ton_Kho` + NEW.`So_Luong`
    WHERE `Ma_San_Pham` = NEW.`Ma_San_Pham`;
END */;;
DELIMITER ;
/*!50003 SET sql_mode              = @saved_sql_mode */ ;
/*!50003 SET character_set_client  = @saved_cs_client */ ;
/*!50003 SET character_set_results = @saved_cs_results */ ;
/*!50003 SET collation_connection  = @saved_col_connection */ ;
/*!50003 SET @saved_cs_client      = @@character_set_client */ ;
/*!50003 SET @saved_cs_results     = @@character_set_results */ ;
/*!50003 SET @saved_col_connection = @@collation_connection */ ;
/*!50003 SET character_set_client  = utf8mb4 */ ;
/*!50003 SET character_set_results = utf8mb4 */ ;
/*!50003 SET collation_connection  = utf8mb4_0900_ai_ci */ ;
/*!50003 SET @saved_sql_mode       = @@sql_mode */ ;
/*!50003 SET sql_mode              = 'ONLY_FULL_GROUP_BY,STRICT_TRANS_TABLES,NO_ZERO_IN_DATE,NO_ZERO_DATE,ERROR_FOR_DIVISION_BY_ZERO,NO_ENGINE_SUBSTITUTION' */ ;
DELIMITER ;;
/*!50003 CREATE*/ /*!50017 DEFINER=`root`@`localhost`*/ /*!50003 TRIGGER `after_chi_tiet_nhap_hang_update` AFTER UPDATE ON `chi_tiet_nhap_hang` FOR EACH ROW BEGIN
    DECLARE ton_kho INT;

    -- Lấy số lượng tồn kho hiện tại
    SELECT `So_Luong_Ton_Kho` INTO ton_kho
    FROM `san_pham`
    WHERE `Ma_San_Pham` = NEW.`Ma_San_Pham`;

    -- Kiểm tra nếu số lượng tồn kho âm sau khi sửa
    IF (ton_kho - OLD.`So_Luong` + NEW.`So_Luong`) < 0 THEN
        SIGNAL SQLSTATE '45000'
        SET MESSAGE_TEXT = 'Số lượng tồn kho không được âm!';
    END IF;

    -- Cập nhật số lượng tồn kho
    UPDATE `san_pham`
    SET `So_Luong_Ton_Kho` = `So_Luong_Ton_Kho` - OLD.`So_Luong` + NEW.`So_Luong`
    WHERE `Ma_San_Pham` = NEW.`Ma_San_Pham`;

    -- Cập nhật tổng chi phí
    UPDATE `nhap_hang`
    SET `Tong_Chi_Phi` = (
        SELECT SUM(`So_Luong` * `Gia_Nhap`)
        FROM `chi_tiet_nhap_hang`
        WHERE `Ma_Nhap_Hang` = NEW.`Ma_Nhap_Hang`
    )
    WHERE `Ma_Nhap_Hang` = NEW.`Ma_Nhap_Hang`;
END */;;
DELIMITER ;
/*!50003 SET sql_mode              = @saved_sql_mode */ ;
/*!50003 SET character_set_client  = @saved_cs_client */ ;
/*!50003 SET character_set_results = @saved_cs_results */ ;
/*!50003 SET collation_connection  = @saved_col_connection */ ;
/*!50003 SET @saved_cs_client      = @@character_set_client */ ;
/*!50003 SET @saved_cs_results     = @@character_set_results */ ;
/*!50003 SET @saved_col_connection = @@collation_connection */ ;
/*!50003 SET character_set_client  = utf8mb4 */ ;
/*!50003 SET character_set_results = utf8mb4 */ ;
/*!50003 SET collation_connection  = utf8mb4_0900_ai_ci */ ;
/*!50003 SET @saved_sql_mode       = @@sql_mode */ ;
/*!50003 SET sql_mode              = 'ONLY_FULL_GROUP_BY,STRICT_TRANS_TABLES,NO_ZERO_IN_DATE,NO_ZERO_DATE,ERROR_FOR_DIVISION_BY_ZERO,NO_ENGINE_SUBSTITUTION' */ ;
DELIMITER ;;
/*!50003 CREATE*/ /*!50017 DEFINER=`root`@`localhost`*/ /*!50003 TRIGGER `after_chi_tiet_nhap_hang_delete` AFTER DELETE ON `chi_tiet_nhap_hang` FOR EACH ROW BEGIN
    DECLARE ton_kho INT;

    -- Lấy số lượng tồn kho hiện tại
    SELECT `So_Luong_Ton_Kho` INTO ton_kho
    FROM `san_pham`
    WHERE `Ma_San_Pham` = OLD.`Ma_San_Pham`;

    -- Kiểm tra nếu số lượng tồn kho âm sau khi xóa
    IF (ton_kho - OLD.`So_Luong`) < 0 THEN
        SIGNAL SQLSTATE '45000'
        SET MESSAGE_TEXT = 'Số lượng tồn kho không được âm!';
    END IF;

    -- Cập nhật số lượng tồn kho
    UPDATE `san_pham`
    SET `So_Luong_Ton_Kho` = `So_Luong_Ton_Kho` - OLD.`So_Luong`
    WHERE `Ma_San_Pham` = OLD.`Ma_San_Pham`;

    -- Cập nhật tổng chi phí
    UPDATE `nhap_hang`
    SET `Tong_Chi_Phi` = (
        SELECT SUM(`So_Luong` * `Gia_Nhap`)
        FROM `chi_tiet_nhap_hang`
        WHERE `Ma_Nhap_Hang` = OLD.`Ma_Nhap_Hang`
    )
    WHERE `Ma_Nhap_Hang` = OLD.`Ma_Nhap_Hang`;
END */;;
DELIMITER ;
/*!50003 SET sql_mode              = @saved_sql_mode */ ;
/*!50003 SET character_set_client  = @saved_cs_client */ ;
/*!50003 SET character_set_results = @saved_cs_results */ ;
/*!50003 SET collation_connection  = @saved_col_connection */ ;

--
-- Table structure for table `danh_muc`
--

DROP TABLE IF EXISTS `danh_muc`;
/*!40101 SET @saved_cs_client     = @@character_set_client */;
/*!50503 SET character_set_client = utf8mb4 */;
CREATE TABLE `danh_muc` (
  `Ma_Danh_Muc` int NOT NULL AUTO_INCREMENT,
  `Ten_Danh_Muc` varchar(255) NOT NULL,
  PRIMARY KEY (`Ma_Danh_Muc`)
) ENGINE=InnoDB AUTO_INCREMENT=9 DEFAULT CHARSET=utf8mb4 COLLATE=utf8mb4_0900_ai_ci;
/*!40101 SET character_set_client = @saved_cs_client */;

--
-- Dumping data for table `danh_muc`
--

LOCK TABLES `danh_muc` WRITE;
/*!40000 ALTER TABLE `danh_muc` DISABLE KEYS */;
INSERT INTO `danh_muc` VALUES (1,'Trang điểm'),(2,'Chăm sóc'),(3,'Kem chống nắng'),(4,'Sữa rửa mặt'),(5,'Chăm sóc da'),(6,'Chăm sóc tóc'),(7,'Nước hoa'),(8,'Dụng cụ trang điểm');
/*!40000 ALTER TABLE `danh_muc` ENABLE KEYS */;
UNLOCK TABLES;

--
-- Table structure for table `don_hang`
--

DROP TABLE IF EXISTS `don_hang`;
/*!40101 SET @saved_cs_client     = @@character_set_client */;
/*!50503 SET character_set_client = utf8mb4 */;
CREATE TABLE `don_hang` (
  `Ma_Don_Hang` int NOT NULL AUTO_INCREMENT,
  `Ma_Khach_Hang` int DEFAULT NULL,
  `Ngay_Dat_Hang` date NOT NULL,
  `Tong_Tien` decimal(10,2) NOT NULL,
  `Ma_Nhan_Vien` int DEFAULT NULL,
  `Trang_Thai` varchar(50) DEFAULT 'Đang xử lý',
  PRIMARY KEY (`Ma_Don_Hang`),
  KEY `Ma_Khach_Hang` (`Ma_Khach_Hang`),
  KEY `Ma_Nhan_Vien` (`Ma_Nhan_Vien`),
  CONSTRAINT `don_hang_ibfk_1` FOREIGN KEY (`Ma_Khach_Hang`) REFERENCES `khach_hang` (`Ma_Khach_Hang`),
  CONSTRAINT `don_hang_ibfk_2` FOREIGN KEY (`Ma_Nhan_Vien`) REFERENCES `nhan_vien` (`Ma_Nhan_Vien`)
) ENGINE=InnoDB AUTO_INCREMENT=6 DEFAULT CHARSET=utf8mb4 COLLATE=utf8mb4_0900_ai_ci;
/*!40101 SET character_set_client = @saved_cs_client */;

--
-- Dumping data for table `don_hang`
--

LOCK TABLES `don_hang` WRITE;
/*!40000 ALTER TABLE `don_hang` DISABLE KEYS */;
INSERT INTO `don_hang` VALUES (2,1,'2025-04-07',1640000.00,1,'Đang xử lý'),(3,2,'2025-04-08',500000.00,2,'Đã giao'),(4,3,'2025-04-09',250000.00,3,'Đang xử lý'),(5,4,'2025-04-10',570000.00,2,'Đã giao');
/*!40000 ALTER TABLE `don_hang` ENABLE KEYS */;
UNLOCK TABLES;

--
-- Table structure for table `khach_hang`
--

DROP TABLE IF EXISTS `khach_hang`;
/*!40101 SET @saved_cs_client     = @@character_set_client */;
/*!50503 SET character_set_client = utf8mb4 */;
CREATE TABLE `khach_hang` (
  `Ma_Khach_Hang` int NOT NULL AUTO_INCREMENT,
  `Ten_Khach_Hang` varchar(255) NOT NULL,
  `So_Dien_Thoai` varchar(20) DEFAULT NULL,
  `Email` varchar(100) DEFAULT NULL,
  `So_Nha` varchar(50) DEFAULT NULL,
  `Duong` varchar(100) DEFAULT NULL,
  `Phuong_Xa` varchar(100) DEFAULT NULL,
  `Quan_Huyen` varchar(100) DEFAULT NULL,
  `Tinh_Thanh` varchar(100) DEFAULT NULL,
  `Ngay_Sinh` date DEFAULT NULL,
  `Gioi_Tinh` varchar(10) DEFAULT NULL,
  PRIMARY KEY (`Ma_Khach_Hang`)
) ENGINE=InnoDB AUTO_INCREMENT=5 DEFAULT CHARSET=utf8mb4 COLLATE=utf8mb4_0900_ai_ci;
/*!40101 SET character_set_client = @saved_cs_client */;

--
-- Dumping data for table `khach_hang`
--

LOCK TABLES `khach_hang` WRITE;
/*!40000 ALTER TABLE `khach_hang` DISABLE KEYS */;
INSERT INTO `khach_hang` VALUES (1,'Phạm Thị Lan','0935123456','lan@gmail.com','90','Trần Phú','Phường 7','Quận 3','TP. Hồ Chí Minh',NULL,NULL),(2,'Nguyễn Thị Hoa','0901234567','hoa@gmail.com','123','Nguyễn Trãi','Phường 5','Quận 5','TP. Hồ Chí Minh',NULL,NULL),(3,'Trần Văn Nam','0912345678','nam@gmail.com','45','Lê Lợi','Phường Bến Nghé','Quận 1','TP. Hồ Chí Minh',NULL,NULL),(4,'Lê Thị Mai','0923456789','mai@gmail.com','78','Hùng Vương','Phường Hải Châu I','Quận Hải Châu','Đà Nẵng',NULL,NULL);
/*!40000 ALTER TABLE `khach_hang` ENABLE KEYS */;
UNLOCK TABLES;

--
-- Table structure for table `khuyen_mai`
--

DROP TABLE IF EXISTS `khuyen_mai`;
/*!40101 SET @saved_cs_client     = @@character_set_client */;
/*!50503 SET character_set_client = utf8mb4 */;
CREATE TABLE `khuyen_mai` (
  `Ma_Khuyen_Mai` int NOT NULL AUTO_INCREMENT,
  `Ten_Khuyen_Mai` varchar(255) NOT NULL,
  `Mo_Ta` text,
  `Giam_Gia` decimal(5,2) DEFAULT NULL,
  `Ngay_Bat_Dau` date NOT NULL,
  `Ngay_Ket_Thuc` date NOT NULL,
  PRIMARY KEY (`Ma_Khuyen_Mai`)
) ENGINE=InnoDB AUTO_INCREMENT=3 DEFAULT CHARSET=utf8mb4 COLLATE=utf8mb4_0900_ai_ci;
/*!40101 SET character_set_client = @saved_cs_client */;

--
-- Dumping data for table `khuyen_mai`
--

LOCK TABLES `khuyen_mai` WRITE;
/*!40000 ALTER TABLE `khuyen_mai` DISABLE KEYS */;
INSERT INTO `khuyen_mai` VALUES (1,'Khuyến mãi tháng 4','Giảm giá 20% cho sản phẩm trang điểm',20.00,'2025-04-01','2025-04-30'),(2,'Mua 2 tặng 1','Mua 2 sản phẩm chăm sóc, tặng 1 sản phẩm chăm sóc',NULL,'2025-04-15','2025-04-30');
/*!40000 ALTER TABLE `khuyen_mai` ENABLE KEYS */;
UNLOCK TABLES;

--
-- Table structure for table `khuyen_mai_san_pham`
--

DROP TABLE IF EXISTS `khuyen_mai_san_pham`;
/*!40101 SET @saved_cs_client     = @@character_set_client */;
/*!50503 SET character_set_client = utf8mb4 */;
CREATE TABLE `khuyen_mai_san_pham` (
  `Ma_Khuyen_Mai` int NOT NULL,
  `Ma_San_Pham` int NOT NULL,
  PRIMARY KEY (`Ma_Khuyen_Mai`,`Ma_San_Pham`),
  KEY `khuyen_mai_san_pham_ibfk_2` (`Ma_San_Pham`),
  CONSTRAINT `khuyen_mai_san_pham_ibfk_1` FOREIGN KEY (`Ma_Khuyen_Mai`) REFERENCES `khuyen_mai` (`Ma_Khuyen_Mai`),
  CONSTRAINT `khuyen_mai_san_pham_ibfk_2` FOREIGN KEY (`Ma_San_Pham`) REFERENCES `san_pham` (`Ma_San_Pham`)
) ENGINE=InnoDB DEFAULT CHARSET=utf8mb4 COLLATE=utf8mb4_0900_ai_ci;
/*!40101 SET character_set_client = @saved_cs_client */;

--
-- Dumping data for table `khuyen_mai_san_pham`
--

LOCK TABLES `khuyen_mai_san_pham` WRITE;
/*!40000 ALTER TABLE `khuyen_mai_san_pham` DISABLE KEYS */;
INSERT INTO `khuyen_mai_san_pham` VALUES (1,1),(1,2),(1,3),(1,4),(2,5),(1,7),(1,8),(2,9);
/*!40000 ALTER TABLE `khuyen_mai_san_pham` ENABLE KEYS */;
UNLOCK TABLES;

--
-- Table structure for table `nha_cung_cap`
--

DROP TABLE IF EXISTS `nha_cung_cap`;
/*!40101 SET @saved_cs_client     = @@character_set_client */;
/*!50503 SET character_set_client = utf8mb4 */;
CREATE TABLE `nha_cung_cap` (
  `Ma_Nha_Cung_Cap` int NOT NULL AUTO_INCREMENT,
  `Ten_Nha_Cung_Cap` varchar(255) NOT NULL,
  `Loai_Nha_Cung_Cap` varchar(20) DEFAULT NULL,
  `So_Nha` varchar(50) DEFAULT NULL,
  `Duong` varchar(100) DEFAULT NULL,
  `Phuong_Xa` varchar(100) DEFAULT NULL,
  `Quan_Huyen` varchar(100) DEFAULT NULL,
  `Tinh_Thanh` varchar(100) DEFAULT NULL,
  PRIMARY KEY (`Ma_Nha_Cung_Cap`)
) ENGINE=InnoDB AUTO_INCREMENT=4 DEFAULT CHARSET=utf8mb4 COLLATE=utf8mb4_0900_ai_ci;
/*!40101 SET character_set_client = @saved_cs_client */;

--
-- Dumping data for table `nha_cung_cap`
--

LOCK TABLES `nha_cung_cap` WRITE;
/*!40000 ALTER TABLE `nha_cung_cap` DISABLE KEYS */;
INSERT INTO `nha_cung_cap` VALUES (1,'vaseline','Nhập khẩu','100 Nguyễn Huệ','Phường Bến Nghé','Quận 1','TP. Hồ Chí Minh','TP. Hồ Chí Minh'),(2,'cocoon','Nội địa','200 Trần Phú','Phường 5','Quận 11','TP. Hồ Chí Minh','TP. Hồ Chí Minh'),(3,'lorea','Nhập khẩu','300 Lê Lợi','Phường Tân Định','Quận 1','TP. Hồ Chí Minh','TP. Hồ Chí Minh');
/*!40000 ALTER TABLE `nha_cung_cap` ENABLE KEYS */;
UNLOCK TABLES;

--
-- Table structure for table `nhan_vien`
--

DROP TABLE IF EXISTS `nhan_vien`;
/*!40101 SET @saved_cs_client     = @@character_set_client */;
/*!50503 SET character_set_client = utf8mb4 */;
CREATE TABLE `nhan_vien` (
  `Ma_Nhan_Vien` int NOT NULL AUTO_INCREMENT,
  `Ten_Nhan_Vien` varchar(255) NOT NULL,
  `Chuc_Vu` varchar(100) DEFAULT NULL,
  `So_Dien_Thoai` varchar(20) DEFAULT NULL,
  `Email` varchar(100) DEFAULT NULL,
  `So_Nha` varchar(50) DEFAULT NULL,
  `Duong` varchar(100) DEFAULT NULL,
  `Phuong_Xa` varchar(100) DEFAULT NULL,
  `Quan_Huyen` varchar(100) DEFAULT NULL,
  `Tinh_Thanh` varchar(100) DEFAULT NULL,
  `Ngay_Vao_Lam` date DEFAULT NULL,
  `Luong` decimal(10,2) DEFAULT NULL,
  PRIMARY KEY (`Ma_Nhan_Vien`)
) ENGINE=InnoDB AUTO_INCREMENT=6 DEFAULT CHARSET=utf8mb4 COLLATE=utf8mb4_0900_ai_ci;
/*!40101 SET character_set_client = @saved_cs_client */;

--
-- Dumping data for table `nhan_vien`
--

LOCK TABLES `nhan_vien` WRITE;
/*!40000 ALTER TABLE `nhan_vien` DISABLE KEYS */;
INSERT INTO `nhan_vien` VALUES (1,'Trần Thị Lan','Quản lý','0935123456','lan@cosmeticshop.com','123 Nguyễn Trãi','Phường 5','Quận 5','TP. Hồ Chí Minh','TP. Hồ Chí Minh','2023-01-01',12000000.00),(2,'Nguyễn Văn Hùng','Nhân viên bán hàng','0987654321','hung@cosmeticshop.com','45 Lê Lợi','Phường Bến Nghé','Quận 1','TP. Hồ Chí Minh','TP. Hồ Chí Minh','2023-02-01',8000000.00),(3,'Lê Thị Mai','Nhân viên bán hàng','0912345678','mai@cosmeticshop.com','78 Hùng Vương','Phường Hải Châu I','Quận Hải Châu','Đà Nẵng','Đà Nẵng','2023-03-01',8000000.00),(4,'Phạm Quốc Anh','Nhân viên kho','0908765432','anh@cosmeticshop.com','56 Trần Phú','Phường Lộc Thọ','TP. Nha Trang','Khánh Hòa','Khánh Hòa','2023-04-01',7000000.00),(5,'Hoàng Thị Ngọc','Nhân viên tư vấn','0923456789','ngoc@cosmeticshop.com','89 Phạm Văn Đồng','Phường Vĩnh Phước','TP. Nha Trang','Khánh Hòa','Khánh Hòa','2023-05-01',7500000.00);
/*!40000 ALTER TABLE `nhan_vien` ENABLE KEYS */;
UNLOCK TABLES;

--
-- Table structure for table `nhap_hang`
--

DROP TABLE IF EXISTS `nhap_hang`;
/*!40101 SET @saved_cs_client     = @@character_set_client */;
/*!50503 SET character_set_client = utf8mb4 */;
CREATE TABLE `nhap_hang` (
  `Ma_Nhap_Hang` int NOT NULL AUTO_INCREMENT,
  `Ma_Nha_Cung_Cap` int DEFAULT NULL,
  `Ngay_Nhap_Hang` date NOT NULL,
  `Tong_Chi_Phi` decimal(10,2) NOT NULL,
  `Ma_Nhan_Vien` int DEFAULT NULL,
  PRIMARY KEY (`Ma_Nhap_Hang`),
  KEY `Ma_Nha_Cung_Cap` (`Ma_Nha_Cung_Cap`),
  KEY `Ma_Nhan_Vien` (`Ma_Nhan_Vien`),
  CONSTRAINT `nhap_hang_ibfk_1` FOREIGN KEY (`Ma_Nha_Cung_Cap`) REFERENCES `nha_cung_cap` (`Ma_Nha_Cung_Cap`),
  CONSTRAINT `nhap_hang_ibfk_2` FOREIGN KEY (`Ma_Nhan_Vien`) REFERENCES `nhan_vien` (`Ma_Nhan_Vien`)
) ENGINE=InnoDB AUTO_INCREMENT=9 DEFAULT CHARSET=utf8mb4 COLLATE=utf8mb4_0900_ai_ci;
/*!40101 SET character_set_client = @saved_cs_client */;

--
-- Dumping data for table `nhap_hang`
--

LOCK TABLES `nhap_hang` WRITE;
/*!40000 ALTER TABLE `nhap_hang` DISABLE KEYS */;
INSERT INTO `nhap_hang` VALUES (1,1,'2025-04-01',5100000.00,4),(2,2,'2025-04-02',3750000.00,4),(3,3,'2025-04-03',3250000.00,4),(4,1,'2025-04-04',5600000.00,4),(5,2,'2025-04-05',4000000.00,4),(6,1,'2025-04-06',1000000.00,4),(7,2,'2025-04-07',1800000.00,4),(8,3,'2025-04-08',1300000.00,4);
/*!40000 ALTER TABLE `nhap_hang` ENABLE KEYS */;
UNLOCK TABLES;

--
-- Table structure for table `san_pham`
--

DROP TABLE IF EXISTS `san_pham`;
/*!40101 SET @saved_cs_client     = @@character_set_client */;
/*!50503 SET character_set_client = utf8mb4 */;
CREATE TABLE `san_pham` (
  `Ma_San_Pham` int NOT NULL AUTO_INCREMENT,
  `Ten_San_Pham` varchar(255) NOT NULL,
  `Mo_Ta` text,
  `Gia` decimal(10,2) NOT NULL,
  `So_Luong_Ton_Kho` int NOT NULL,
  `Ngay_San_Xuat` date DEFAULT NULL,
  `Han_Su_Dung` date DEFAULT NULL,
  `Ma_Danh_Muc` int DEFAULT NULL,
  PRIMARY KEY (`Ma_San_Pham`),
  KEY `san_pham_ibfk_1` (`Ma_Danh_Muc`),
  CONSTRAINT `san_pham_ibfk_1` FOREIGN KEY (`Ma_Danh_Muc`) REFERENCES `danh_muc` (`Ma_Danh_Muc`),
  CONSTRAINT `check_so_luong_ton_kho` CHECK ((`So_Luong_Ton_Kho` >= 0))
) ENGINE=InnoDB AUTO_INCREMENT=11 DEFAULT CHARSET=utf8mb4 COLLATE=utf8mb4_0900_ai_ci;
/*!40101 SET character_set_client = @saved_cs_client */;

--
-- Dumping data for table `san_pham`
--

LOCK TABLES `san_pham` WRITE;
/*!40000 ALTER TABLE `san_pham` DISABLE KEYS */;
INSERT INTO `san_pham` VALUES (1,'Son môi đỏ Ruby','Son lì cao cấp, màu đỏ đậm',200000.00,19,'2024-01-01','2027-01-01',NULL),(2,'Kem nền SPF50','Kem nền chống nắng, tone tự nhiên',300000.00,15,'2024-02-01','2027-02-01',NULL),(3,'Phấn má hồng Peach','Phấn má dạng bột, màu đào',150000.00,8,'2024-03-01','2027-03-01',NULL),(4,'Mascara dài mi','Mascara chống nước, làm dài mi',180000.00,25,'2024-04-01','2027-04-01',NULL),(5,'Son dưỡng môi Cherry','Son dưỡng có màu nhẹ',120000.00,68,'2024-05-01','2027-05-01',NULL),(6,'Kem chống nắng SPF30','Kem chống nắng dịu nhẹ',250000.00,9,'2024-06-01','2027-06-01',NULL),(7,'Phấn phủ kiềm dầu','Phấn phủ dạng bột mịn',220000.00,0,'2024-07-01','2027-07-01',NULL),(8,'Chì kẻ mày nâu','Chì kẻ mày lâu trôi',100000.00,30,'2024-08-01','2027-08-01',NULL),(9,'Nước tẩy trang Micellar','Nước tẩy trang không cồn',280000.00,15,'2024-09-01','2027-09-01',2),(10,'Sữa rửa mặt trà xanh','Sữa rửa mặt làm sạch sâu',190000.00,7,'2024-10-01','2027-10-01',NULL);
/*!40000 ALTER TABLE `san_pham` ENABLE KEYS */;
UNLOCK TABLES;

--
-- Table structure for table `san_pham_nha_cung_cap`
--

DROP TABLE IF EXISTS `san_pham_nha_cung_cap`;
/*!40101 SET @saved_cs_client     = @@character_set_client */;
/*!50503 SET character_set_client = utf8mb4 */;
CREATE TABLE `san_pham_nha_cung_cap` (
  `Ma_San_Pham` int NOT NULL,
  `Ma_Nha_Cung_Cap` int NOT NULL,
  PRIMARY KEY (`Ma_San_Pham`,`Ma_Nha_Cung_Cap`),
  KEY `Ma_Nha_Cung_Cap` (`Ma_Nha_Cung_Cap`),
  CONSTRAINT `san_pham_nha_cung_cap_ibfk_1` FOREIGN KEY (`Ma_San_Pham`) REFERENCES `san_pham` (`Ma_San_Pham`),
  CONSTRAINT `san_pham_nha_cung_cap_ibfk_2` FOREIGN KEY (`Ma_Nha_Cung_Cap`) REFERENCES `nha_cung_cap` (`Ma_Nha_Cung_Cap`)
) ENGINE=InnoDB DEFAULT CHARSET=utf8mb4 COLLATE=utf8mb4_0900_ai_ci;
/*!40101 SET character_set_client = @saved_cs_client */;

--
-- Dumping data for table `san_pham_nha_cung_cap`
--

LOCK TABLES `san_pham_nha_cung_cap` WRITE;
/*!40000 ALTER TABLE `san_pham_nha_cung_cap` DISABLE KEYS */;
INSERT INTO `san_pham_nha_cung_cap` VALUES (1,1),(3,1),(5,1),(8,1),(2,2),(6,2),(9,2),(4,3),(7,3),(10,3);
/*!40000 ALTER TABLE `san_pham_nha_cung_cap` ENABLE KEYS */;
UNLOCK TABLES;

--
-- Dumping events for database 'cosmetics_shop'
--

--
-- Dumping routines for database 'cosmetics_shop'
--
/*!40103 SET TIME_ZONE=@OLD_TIME_ZONE */;

/*!40101 SET SQL_MODE=@OLD_SQL_MODE */;
/*!40014 SET FOREIGN_KEY_CHECKS=@OLD_FOREIGN_KEY_CHECKS */;
/*!40014 SET UNIQUE_CHECKS=@OLD_UNIQUE_CHECKS */;
/*!40101 SET CHARACTER_SET_CLIENT=@OLD_CHARACTER_SET_CLIENT */;
/*!40101 SET CHARACTER_SET_RESULTS=@OLD_CHARACTER_SET_RESULTS */;
/*!40101 SET COLLATION_CONNECTION=@OLD_COLLATION_CONNECTION */;
/*!40111 SET SQL_NOTES=@OLD_SQL_NOTES */;

-- Dump completed on 2025-04-08 21:46:22
