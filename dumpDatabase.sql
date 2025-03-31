-- MySQL dump 10.13  Distrib 8.0.34, for Win64 (x86_64)
--
-- Host: localhost    Database: db32
-- ------------------------------------------------------
-- Server version	8.0.32

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
-- Table structure for table `category`
--

DROP TABLE IF EXISTS `category`;
/*!40101 SET @saved_cs_client     = @@character_set_client */;
/*!50503 SET character_set_client = utf8mb4 */;
CREATE TABLE `category` (
  `Id` int NOT NULL AUTO_INCREMENT,
  `Name` varchar(45) NOT NULL,
  PRIMARY KEY (`Id`)
) ENGINE=InnoDB AUTO_INCREMENT=6 DEFAULT CHARSET=utf8mb4 COLLATE=utf8mb4_0900_ai_ci;
/*!40101 SET character_set_client = @saved_cs_client */;

--
-- Dumping data for table `category`
--

LOCK TABLES `category` WRITE;
/*!40000 ALTER TABLE `category` DISABLE KEYS */;
INSERT INTO `category` VALUES (1,'Анальгетик, жаропонижающее'),(2,'Антибиотик'),(3,'Муколитики'),(4,'Муколитикипипипи'),(5,'вввв');
/*!40000 ALTER TABLE `category` ENABLE KEYS */;
UNLOCK TABLES;

--
-- Table structure for table `department`
--

DROP TABLE IF EXISTS `department`;
/*!40101 SET @saved_cs_client     = @@character_set_client */;
/*!50503 SET character_set_client = utf8mb4 */;
CREATE TABLE `department` (
  `Id` int NOT NULL AUTO_INCREMENT,
  `Name` varchar(45) NOT NULL,
  `Cabinet` int NOT NULL,
  PRIMARY KEY (`Id`)
) ENGINE=InnoDB AUTO_INCREMENT=6 DEFAULT CHARSET=utf8mb4 COLLATE=utf8mb4_0900_ai_ci;
/*!40101 SET character_set_client = @saved_cs_client */;

--
-- Dumping data for table `department`
--

LOCK TABLES `department` WRITE;
/*!40000 ALTER TABLE `department` DISABLE KEYS */;
INSERT INTO `department` VALUES (1,'Травмпункт',201),(2,'Дерматология',202),(3,'Неврология',203),(4,'Гастроэнтерология',204),(5,'Кардиология',205);
/*!40000 ALTER TABLE `department` ENABLE KEYS */;
UNLOCK TABLES;

--
-- Table structure for table `drug`
--

DROP TABLE IF EXISTS `drug`;
/*!40101 SET @saved_cs_client     = @@character_set_client */;
/*!50503 SET character_set_client = utf8mb4 */;
CREATE TABLE `drug` (
  `Id` int NOT NULL AUTO_INCREMENT,
  `Name` varchar(45) NOT NULL,
  `CategoryId` varchar(45) NOT NULL,
  `ReleaseForm` varchar(45) NOT NULL,
  `ShelfLife` int NOT NULL,
  `ManufacturerId` varchar(225) NOT NULL,
  `Photo` varchar(225) DEFAULT NULL,
  PRIMARY KEY (`Id`)
) ENGINE=InnoDB AUTO_INCREMENT=21 DEFAULT CHARSET=utf8mb4 COLLATE=utf8mb4_0900_ai_ci;
/*!40101 SET character_set_client = @saved_cs_client */;

--
-- Dumping data for table `drug`
--

LOCK TABLES `drug` WRITE;
/*!40000 ALTER TABLE `drug` DISABLE KEYS */;
INSERT INTO `drug` VALUES (1,'Нурофен','1','Таблетки',3,'3',NULL),(2,'Парацетамол','1','Таблетки',5,'1',NULL),(3,'Цитрамон П','1','Таблетки',5,'2',NULL),(4,'Амоксиклав','2','Таблетки',2,'1',NULL),(5,'Азитромицин','2','Капсулы',2,'2',NULL),(6,'Оспамокс','2','Капсулы',3,'3',NULL),(7,'Лазолван','3','Сироп',5,'2',NULL),(8,'Амбробене','3','Сироп',3,'3',NULL),(9,'Бромгексин','3','Таблетки',5,'1',NULL),(10,'Флуимуцил','3','Капсулы',3,'2',NULL),(20,'fdsafa','1','Таблетки',21,'1','013.jpg');
/*!40000 ALTER TABLE `drug` ENABLE KEYS */;
UNLOCK TABLES;

--
-- Table structure for table `employee`
--

DROP TABLE IF EXISTS `employee`;
/*!40101 SET @saved_cs_client     = @@character_set_client */;
/*!50503 SET character_set_client = utf8mb4 */;
CREATE TABLE `employee` (
  `Id` int NOT NULL AUTO_INCREMENT,
  `Name` varchar(45) NOT NULL,
  `LastName` varchar(45) NOT NULL,
  `Patronyc` varchar(45) DEFAULT NULL,
  `DepartmentId` int NOT NULL,
  `ProfessionId` int NOT NULL,
  `UserId` varchar(45) NOT NULL,
  PRIMARY KEY (`Id`)
) ENGINE=InnoDB DEFAULT CHARSET=utf8mb4 COLLATE=utf8mb4_0900_ai_ci;
/*!40101 SET character_set_client = @saved_cs_client */;

--
-- Dumping data for table `employee`
--

LOCK TABLES `employee` WRITE;
/*!40000 ALTER TABLE `employee` DISABLE KEYS */;
/*!40000 ALTER TABLE `employee` ENABLE KEYS */;
UNLOCK TABLES;

--
-- Table structure for table `manufacturer`
--

DROP TABLE IF EXISTS `manufacturer`;
/*!40101 SET @saved_cs_client     = @@character_set_client */;
/*!50503 SET character_set_client = utf8mb4 */;
CREATE TABLE `manufacturer` (
  `Id` int NOT NULL AUTO_INCREMENT,
  `Name` varchar(225) NOT NULL,
  PRIMARY KEY (`Id`)
) ENGINE=InnoDB AUTO_INCREMENT=4 DEFAULT CHARSET=utf8mb4 COLLATE=utf8mb4_0900_ai_ci;
/*!40101 SET character_set_client = @saved_cs_client */;

--
-- Dumping data for table `manufacturer`
--

LOCK TABLES `manufacturer` WRITE;
/*!40000 ALTER TABLE `manufacturer` DISABLE KEYS */;
INSERT INTO `manufacturer` VALUES (1,'Sandozв'),(2,'Sanofiв'),(3,'Berlin-Chemie');
/*!40000 ALTER TABLE `manufacturer` ENABLE KEYS */;
UNLOCK TABLES;

--
-- Table structure for table `profession`
--

DROP TABLE IF EXISTS `profession`;
/*!40101 SET @saved_cs_client     = @@character_set_client */;
/*!50503 SET character_set_client = utf8mb4 */;
CREATE TABLE `profession` (
  `Id` int NOT NULL AUTO_INCREMENT,
  `Name` varchar(45) NOT NULL,
  PRIMARY KEY (`Id`)
) ENGINE=InnoDB AUTO_INCREMENT=6 DEFAULT CHARSET=utf8mb4 COLLATE=utf8mb4_0900_ai_ci;
/*!40101 SET character_set_client = @saved_cs_client */;

--
-- Dumping data for table `profession`
--

LOCK TABLES `profession` WRITE;
/*!40000 ALTER TABLE `profession` DISABLE KEYS */;
INSERT INTO `profession` VALUES (1,'Терапевт'),(2,'Педиатр '),(3,'Невролог '),(4,'Стоматолог '),(5,'Уборщица');
/*!40000 ALTER TABLE `profession` ENABLE KEYS */;
UNLOCK TABLES;

--
-- Table structure for table `request`
--

DROP TABLE IF EXISTS `request`;
/*!40101 SET @saved_cs_client     = @@character_set_client */;
/*!50503 SET character_set_client = utf8mb4 */;
CREATE TABLE `request` (
  `Id` int NOT NULL AUTO_INCREMENT,
  `DrugId` int NOT NULL,
  `UserId` int NOT NULL,
  `DepartmentId` int NOT NULL,
  `Amount` int NOT NULL,
  `Date` int NOT NULL,
  `Status` varchar(45) NOT NULL,
  PRIMARY KEY (`Id`)
) ENGINE=InnoDB DEFAULT CHARSET=utf8mb4 COLLATE=utf8mb4_0900_ai_ci;
/*!40101 SET character_set_client = @saved_cs_client */;

--
-- Dumping data for table `request`
--

LOCK TABLES `request` WRITE;
/*!40000 ALTER TABLE `request` DISABLE KEYS */;
/*!40000 ALTER TABLE `request` ENABLE KEYS */;
UNLOCK TABLES;

--
-- Table structure for table `role`
--

DROP TABLE IF EXISTS `role`;
/*!40101 SET @saved_cs_client     = @@character_set_client */;
/*!50503 SET character_set_client = utf8mb4 */;
CREATE TABLE `role` (
  `id` int NOT NULL AUTO_INCREMENT,
  `Name` varchar(45) DEFAULT NULL,
  PRIMARY KEY (`id`)
) ENGINE=InnoDB AUTO_INCREMENT=4 DEFAULT CHARSET=utf8mb4 COLLATE=utf8mb4_0900_ai_ci;
/*!40101 SET character_set_client = @saved_cs_client */;

--
-- Dumping data for table `role`
--

LOCK TABLES `role` WRITE;
/*!40000 ALTER TABLE `role` DISABLE KEYS */;
INSERT INTO `role` VALUES (1,'Администратор'),(2,'Главная медсестра больница'),(3,'Старшая медсестра отдела');
/*!40000 ALTER TABLE `role` ENABLE KEYS */;
UNLOCK TABLES;

--
-- Table structure for table `user`
--

DROP TABLE IF EXISTS `user`;
/*!40101 SET @saved_cs_client     = @@character_set_client */;
/*!50503 SET character_set_client = utf8mb4 */;
CREATE TABLE `user` (
  `Id` int NOT NULL AUTO_INCREMENT,
  `Login` varchar(45) NOT NULL,
  `Password` varchar(255) NOT NULL,
  `RoleId` varchar(45) NOT NULL,
  PRIMARY KEY (`Id`)
) ENGINE=InnoDB AUTO_INCREMENT=56 DEFAULT CHARSET=utf8mb4 COLLATE=utf8mb4_0900_ai_ci;
/*!40101 SET character_set_client = @saved_cs_client */;

--
-- Dumping data for table `user`
--

LOCK TABLES `user` WRITE;
/*!40000 ALTER TABLE `user` DISABLE KEYS */;
INSERT INTO `user` VALUES (1,'aNJJq!4Emp','e2c0a3fdf6a2c5f8e4eb5c75d7de777c03b9fefbfbd3d9a9b68765a5be334bda','2'),(2,'yrW@F9^Gli','501c481ccbf6aae8205390d6e2481d1b541470310d1cb5dce1e577994f187f8c','2'),(3,'nUlhq*WabU','25368c02b70f45ecb366ffbe69c0e172e6b682efc09def70a2320dd3dd890eb7','1'),(4,'NyyLF^8baN','7c00db5e90aeb3a69b157a1de38cf36983dba4e3440f7c3b9aba1f7f7a949dfc','2'),(5,'B07ZpfON8f','e98196abc59ebf0dd32353ce8628f2002c51a3533b46b87eb207e20614c11618','2'),(6,'0EbjFe0P6*','4cef4572c304bf5b66455a00e46bfce28000604cdbebe4d928abc47e33bde9be','3'),(7,'hjoNVc#Q5!','46590b6cd06f9734a8e3d0294aa099d1474fc6a4b5b54615f172aec39aacd435','1'),(8,'%MW1FkTjVC','4f7c8495ca32e42cb3c667ae7908eb815ca12fd284306b9f13c9c503a77ca318','3'),(9,'vq9mVj4kUv','13a0c140f52aecd599b199c70d5ef0271904fca08c8d3fb56cd12e4e6dece2ae','1'),(10,'U$eP@h&mSp','78c637c3ad0c9c94f28abc2c912b7aaee78c2169f5cd883e0934a3a6c651a8b7','2'),(11,'JxM3VpXYI0','b0837a36b65aed4836c7184c41d0de23e54ff3ca3ddbc3411e244c081594c86e','2'),(12,'8cZo!o8ZGT','f5d8cd501b6aec3107c6adf09735eb6002a5acd0a80cc9859bb27c2f5a10c80c','3'),(13,'WDoCVvPswm','1011c7ef9c1cbdebdd1aef96c86dc56b0b8380afca8f0d81744559ac823f51ad','3'),(14,'diB5!u1uuf','9874acc0350d871ae0a868567ef81a309de0978a632d1923ee98a7c70db7e65a','1'),(15,'C5Pqht#vt&','8188a9592d05d12d46e6f5579e62b4594860ef2472f28d22e64b475113cedd53','2'),(16,'rpeE!AT7jK','b8003fb290f3dc432ab1166dfc8cf099510d6bf4da33b65ab91e01722dda1b76','1'),(17,'Q#r8hz59iC','b632d818a6136806d16cd7ecba4fd2e83026466155b99dbe94b2a682e1e69b12','2'),(18,'^REsxy&!gv','88933db50674588d4f67da3777f0defa3cb534071407331a25a993698f1415f2','3'),(19,'4a#GhFXD%7','22e9d7aed42676a1eac55d3bf8b41e4391e66952564637c24c0ec556342f5222','3'),(20,'lXh!wE9E$0','8504fcbb8df1732412f7c85c5fe96e4eb8e365d37c4e825a6273f993b7f5f627','1'),(21,'*hPghNP&3s','004c8ff81454bc95e3ac5bcabc067541cbdf5de2652c41e3e7a7e69011da2b5a','3'),(22,'y42JwL1a1m','0e893a312bb82898b4c716b73f0b78c8cd47f4c646be45ecde8aede9dea6bfae','2'),(23,'YJ%#MK$b0f','4cb94cfe198468121e4d00e4484a36b61204b73879a3d4974a19f77300cf7a5a','3'),(24,'N!EiwSTNQQ','31b7fe08e84bc8b0b9c0427ffa4b711c1f8d243a6dc2fe0fe47d12bb164f372e','2'),(25,'@PSLMQ5PPJ','6702a786024b21bc1fd629b6473ffd534d6a5cae7fb8296b1ce63f177a8fca97','3'),(26,'st4%2P*QNC','b9c3d3f1c8e4d96cfce62f6b94531ee30e9bfea9d8897e5c3d220803065e3747','1'),(27,'hWukMXYjC$','e49c95ecbeac210ba7ce25f23d3d287210069263bdd2790c812add69f9eb9249','1'),(28,'GAGO1V9kB6','5e7538e9a026356fa3222fabb79e397eb1cc1f21e5f24a26604def73a7785814','2'),(29,'u3^2M3QWrz','3b2fa03dccb60a04e42380eadc61f96e88105e9bee5901808236dd2fbd2e3bd4','1'),(30,'UIkm122Yps','fe2f62df10a52b2c99e1a96e01318ebd1a71c0807319d8c70087f00bb7ddf985','2'),(31,'bmwQ*0$Zol','10a285ad8c416a80d65632549fc9c906df0a1216933cf4658601e9c8419333da','1'),(32,'8O5418UseX','1325620d8695002355731102d0fec75793dacd28b9cbeb8a651dd65695acae18','3'),(33,'os*o&76udQ','68aad0c104d2404bc452e9d39713d764e839514117547c9ae1a4fdf5bfcf63bb','1'),(34,'O^mSo6*vbJ','c2b1f5bbd2b47c80d118f34a5bde1b4b060d979b0f11760b616dc3ef4968cef0','2'),(35,'CzV6&$Y79c','e7b8b09ce0e405f4f8c5d2f848557b2c63325d4029245448d76cde1917abfdb6','2'),(36,'1e7rn#!98$','932a822e78fd010ea6fde944ef46734b5f8f85891198d1df73c19cb028e2ab72','3'),(37,'QFxF&bQBYF','0d1609f8a9c7092ead7279a93d1f6b899a99f456462d638fb4a06aa148156037','2'),(38,'^kK8na2DXz','7bcf96d6729dad4a8fe5d326c788c83abe045025074191277b29c7b744be0919','3'),(39,'w7XtD*%EVs','09faab35335784241fe88e21eeeee945109fab706f1ce4dd64ed2177e07b8650','1'),(40,'krmIngU&L3','e1dd43ff8c7a646ee68da80b3a70e4d329a7a108e13a827823b385f60483559e','1'),(41,'K%z!Cf6aKW','17253e4297f47c4098233ad162930edcaeee08926d3b8cb063c888d490f5f582','2'),(42,'9TNvTeabIP','057b6398fa3450b24713a7392d194174286488fa59deaa3861dab346792ddba1','3'),(43,'YdcKClZNxi','45c8c7b915469a8d156eb441dbaa67dce008e8ecd6c3ed1ae5803eba8f6816c1','3'),(44,'eZp$Tk!Pwb','2e45ed643f865f8f25235d02b5b41999a88656168c210080c0a8de939e5b8729','1'),(45,'@jXiCsRhmN','44d53cb3c7cd020f8e25b20d358b106f6645b560e5a5b8428f5098183d5d1174','3'),(46,'s6!MSq3jlF','df4d4d879428e98078bb56e1dc86ba6e280149ea3cef1a67718c5858061f5c23','1'),(47,'SLf^8p%ljy','179bf58992ed94d323552e0a151e2f912555e5f50901809137411cfb50b8adec','2'),(48,'F$NlSxVW*!','a83aebf7c83d863838ce4ff76be6da6bf0f99989aae24e9d0d2f73875c0fc5f1','2'),(49,'5S0O7v7Y&3','cdcb0bf02de57dc4f8c213329576a44ac203123cffc3cd5f9fb4c2b3b53fb189','3'),(50,'mw$*fuaZ%W','8b555f16991a576209f8922b5d320df24a5b709c74e6468925dfd0e940d00dcb','1'),(51,'admin','8c6976e5b5410415bde908bd4dee15dfb167a9c873fc4bb8a81f6f2ab448a918','1'),(52,'ivan','5c00d8a50ce2679c308f5af180b01430282cd6c9df6afd0e7ccc90a2b3955488','2'),(53,'masha','72786c1b0518a9f19690577fe02b4304ca4f27432a8d92efcf6eb5660af03181','3');
/*!40000 ALTER TABLE `user` ENABLE KEYS */;
UNLOCK TABLES;

--
-- Table structure for table `warehouse`
--

DROP TABLE IF EXISTS `warehouse`;
/*!40101 SET @saved_cs_client     = @@character_set_client */;
/*!50503 SET character_set_client = utf8mb4 */;
CREATE TABLE `warehouse` (
  `DrugId` int NOT NULL,
  `AmountInStock` int DEFAULT NULL,
  `DeliveryDate` date NOT NULL,
  `ShelfLife` int NOT NULL,
  `DepartmentId` int NOT NULL,
  PRIMARY KEY (`DrugId`)
) ENGINE=InnoDB DEFAULT CHARSET=utf8mb4 COLLATE=utf8mb4_0900_ai_ci;
/*!40101 SET character_set_client = @saved_cs_client */;

--
-- Dumping data for table `warehouse`
--

LOCK TABLES `warehouse` WRITE;
/*!40000 ALTER TABLE `warehouse` DISABLE KEYS */;
/*!40000 ALTER TABLE `warehouse` ENABLE KEYS */;
UNLOCK TABLES;
/*!40103 SET TIME_ZONE=@OLD_TIME_ZONE */;

/*!40101 SET SQL_MODE=@OLD_SQL_MODE */;
/*!40014 SET FOREIGN_KEY_CHECKS=@OLD_FOREIGN_KEY_CHECKS */;
/*!40014 SET UNIQUE_CHECKS=@OLD_UNIQUE_CHECKS */;
/*!40101 SET CHARACTER_SET_CLIENT=@OLD_CHARACTER_SET_CLIENT */;
/*!40101 SET CHARACTER_SET_RESULTS=@OLD_CHARACTER_SET_RESULTS */;
/*!40101 SET COLLATION_CONNECTION=@OLD_COLLATION_CONNECTION */;
/*!40111 SET SQL_NOTES=@OLD_SQL_NOTES */;

-- Dump completed on 2025-03-10  8:59:46
