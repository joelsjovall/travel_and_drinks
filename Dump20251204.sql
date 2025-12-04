-- MySQL dump 10.13  Distrib 8.0.44, for Win64 (x86_64)
--
-- Host: 127.0.0.1    Database: drinks_and_travels
-- ------------------------------------------------------
-- Server version	8.0.44

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
-- Table structure for table `bookings`
--

DROP TABLE IF EXISTS `bookings`;
/*!40101 SET @saved_cs_client     = @@character_set_client */;
/*!50503 SET character_set_client = utf8mb4 */;
CREATE TABLE `bookings` (
  `booking_id` int NOT NULL AUTO_INCREMENT,
  `user_id` int NOT NULL,
  `hotel_id` int DEFAULT NULL,
  `event_id` int DEFAULT NULL,
  `total_price` int NOT NULL,
  PRIMARY KEY (`booking_id`),
  KEY `user_id` (`user_id`),
  KEY `hotel_id` (`hotel_id`),
  KEY `event_id` (`event_id`),
  CONSTRAINT `bookings_ibfk_1` FOREIGN KEY (`user_id`) REFERENCES `users` (`user_id`),
  CONSTRAINT `bookings_ibfk_2` FOREIGN KEY (`hotel_id`) REFERENCES `hotels` (`hotel_id`),
  CONSTRAINT `bookings_ibfk_3` FOREIGN KEY (`event_id`) REFERENCES `events` (`event_id`)
) ENGINE=InnoDB DEFAULT CHARSET=utf8mb4 COLLATE=utf8mb4_0900_ai_ci;
/*!40101 SET character_set_client = @saved_cs_client */;

--
-- Dumping data for table `bookings`
--

LOCK TABLES `bookings` WRITE;
/*!40000 ALTER TABLE `bookings` DISABLE KEYS */;
/*!40000 ALTER TABLE `bookings` ENABLE KEYS */;
UNLOCK TABLES;

--
-- Table structure for table `cities`
--

DROP TABLE IF EXISTS `cities`;
/*!40101 SET @saved_cs_client     = @@character_set_client */;
/*!50503 SET character_set_client = utf8mb4 */;
CREATE TABLE `cities` (
  `city_id` int NOT NULL AUTO_INCREMENT,
  `country_id` int NOT NULL,
  `name` varchar(100) NOT NULL,
  PRIMARY KEY (`city_id`),
  KEY `country_id` (`country_id`),
  CONSTRAINT `cities_ibfk_1` FOREIGN KEY (`country_id`) REFERENCES `countries` (`country_id`)
) ENGINE=InnoDB AUTO_INCREMENT=21 DEFAULT CHARSET=utf8mb4 COLLATE=utf8mb4_0900_ai_ci;
/*!40101 SET character_set_client = @saved_cs_client */;

--
-- Dumping data for table `cities`
--

LOCK TABLES `cities` WRITE;
/*!40000 ALTER TABLE `cities` DISABLE KEYS */;
INSERT INTO `cities` VALUES (1,1,'New York'),(2,1,'San Francisco'),(3,2,'London'),(4,2,'Edinburgh'),(5,3,'Barcelona'),(6,3,'Madrid'),(7,4,'Rome'),(8,4,'Florence'),(9,5,'Paris'),(10,5,'Lyon'),(11,6,'Berlin'),(12,6,'Munich'),(13,7,'Tokyo'),(14,7,'Osaka'),(15,8,'Sydney'),(16,8,'Melbourne'),(17,9,'Rio de Janeiro'),(18,9,'São Paulo'),(19,10,'Cape Town'),(20,10,'Johannesburg');
/*!40000 ALTER TABLE `cities` ENABLE KEYS */;
UNLOCK TABLES;

--
-- Table structure for table `countries`
--

DROP TABLE IF EXISTS `countries`;
/*!40101 SET @saved_cs_client     = @@character_set_client */;
/*!50503 SET character_set_client = utf8mb4 */;
CREATE TABLE `countries` (
  `country_id` int NOT NULL AUTO_INCREMENT,
  `name` varchar(100) NOT NULL,
  PRIMARY KEY (`country_id`)
) ENGINE=InnoDB AUTO_INCREMENT=11 DEFAULT CHARSET=utf8mb4 COLLATE=utf8mb4_0900_ai_ci;
/*!40101 SET character_set_client = @saved_cs_client */;

--
-- Dumping data for table `countries`
--

LOCK TABLES `countries` WRITE;
/*!40000 ALTER TABLE `countries` DISABLE KEYS */;
INSERT INTO `countries` VALUES (1,'United States'),(2,'United Kingdom'),(3,'Spain'),(4,'Italy'),(5,'France'),(6,'Germany'),(7,'Japan'),(8,'Australia'),(9,'Brazil'),(10,'South Africa');
/*!40000 ALTER TABLE `countries` ENABLE KEYS */;
UNLOCK TABLES;

--
-- Table structure for table `events`
--

DROP TABLE IF EXISTS `events`;
/*!40101 SET @saved_cs_client     = @@character_set_client */;
/*!50503 SET character_set_client = utf8mb4 */;
CREATE TABLE `events` (
  `event_id` int NOT NULL AUTO_INCREMENT,
  `city_id` int NOT NULL,
  `name` varchar(150) NOT NULL,
  `drink_type` varchar(50) NOT NULL,
  `event_date` date NOT NULL,
  `price_per_person` int NOT NULL,
  `available_seats` int NOT NULL,
  PRIMARY KEY (`event_id`),
  KEY `city_id` (`city_id`),
  CONSTRAINT `events_ibfk_1` FOREIGN KEY (`city_id`) REFERENCES `cities` (`city_id`)
) ENGINE=InnoDB AUTO_INCREMENT=22 DEFAULT CHARSET=utf8mb4 COLLATE=utf8mb4_0900_ai_ci;
/*!40101 SET character_set_client = @saved_cs_client */;

--
-- Dumping data for table `events`
--

LOCK TABLES `events` WRITE;
/*!40000 ALTER TABLE `events` DISABLE KEYS */;
INSERT INTO `events` VALUES (1,1,'New York Wine Tasting Experience','wine','2025-03-05',60,20),(2,1,'Manhattan Whiskey Masterclass','whiskey','2025-03-10',80,15),(3,2,'San Francisco Craft Beer Tour','beer','2025-04-12',50,25),(4,3,'London Gin & Tonic Night','gin','2025-03-18',45,20),(5,4,'Edinburgh Scotch Whisky Tasting','whiskey','2025-03-25',90,12),(6,5,'Barcelona Sangria Workshop','wine','2025-04-01',40,30),(7,6,'Madrid Rioja Wine Evening','wine','2025-04-07',55,20),(8,7,'Rome Italian Wine Tasting','wine','2025-03-20',65,18),(9,8,'Florence Chianti Classico Tour','wine','2025-03-22',70,16),(10,9,'Paris Champagne Night','wine','2025-04-14',95,15),(11,10,'Lyon French Craft Beer Festival','beer','2025-04-18',40,25),(12,11,'Berlin Craft Beer Experience','beer','2025-03-28',35,30),(13,12,'Munich Bavarian Beer Tour','beer','2025-10-01',60,40),(14,13,'Tokyo Sake Discovery Event','sake','2025-03-30',45,22),(15,14,'Osaka Japanese Whiskey Night','whiskey','2025-04-05',85,14),(16,15,'Sydney Harbour Wine Sunset Tour','wine','2025-04-10',70,20),(17,16,'Melbourne Craft Beer Journey','beer','2025-03-26',50,22),(18,17,'Rio Caipirinha Cocktail Class','cocktail','2025-03-19',35,28),(19,18,'São Paulo Brazilian Beer Experience','beer','2025-04-02',45,25),(20,19,'Cape Town Vineyard Safari','wine','2025-03-15',80,18),(21,20,'Johannesburg Whiskey & Braai Night','whiskey','2025-03-27',75,12);
/*!40000 ALTER TABLE `events` ENABLE KEYS */;
UNLOCK TABLES;

--
-- Table structure for table `hotels`
--

DROP TABLE IF EXISTS `hotels`;
/*!40101 SET @saved_cs_client     = @@character_set_client */;
/*!50503 SET character_set_client = utf8mb4 */;
CREATE TABLE `hotels` (
  `hotel_id` int NOT NULL AUTO_INCREMENT,
  `city_id` int NOT NULL,
  `name` varchar(200) NOT NULL,
  `price_per_night` int NOT NULL,
  `available_rooms` int NOT NULL,
  `rating` int DEFAULT NULL,
  PRIMARY KEY (`hotel_id`),
  KEY `city_id` (`city_id`),
  CONSTRAINT `hotels_ibfk_1` FOREIGN KEY (`city_id`) REFERENCES `cities` (`city_id`)
) ENGINE=InnoDB AUTO_INCREMENT=22 DEFAULT CHARSET=utf8mb4 COLLATE=utf8mb4_0900_ai_ci;
/*!40101 SET character_set_client = @saved_cs_client */;

--
-- Dumping data for table `hotels`
--

LOCK TABLES `hotels` WRITE;
/*!40000 ALTER TABLE `hotels` DISABLE KEYS */;
INSERT INTO `hotels` VALUES (1,1,'Manhattan Grand Hotel',220,15,5),(2,1,'Central Park Lodge',160,8,4),(3,2,'Golden Gate Bay Hotel',180,12,4),(4,3,'London River Inn',150,10,4),(5,4,'Edinburgh Castle View Hotel',170,6,5),(6,5,'Barcelona Beach Resort',140,20,4),(7,6,'Madrid Wine & Stay',130,15,4),(8,7,'Rome Historical Suites',190,9,5),(9,8,'Florence Art Hotel',160,7,4),(10,9,'Paris Luxury Stay',210,14,5),(11,10,'Lyon Comfort Hotel',120,11,4),(12,11,'Berlin Cityline Hotel',110,10,3),(13,12,'Munich Beer Garden Hotel',140,12,5),(14,13,'Tokyo Skyline Hotel',200,25,5),(15,14,'Osaka Central Inn',130,18,4),(16,15,'Sydney Harbour Hotel',190,16,5),(17,16,'Melbourne City Suites',150,10,4),(18,17,'Rio Beachfront Hotel',140,22,4),(19,18,'São Paulo Urban Hotel',120,13,3),(20,19,'Cape Town Ocean Lodge',160,17,5),(21,20,'Johannesburg Safari Stay',130,9,4);
/*!40000 ALTER TABLE `hotels` ENABLE KEYS */;
UNLOCK TABLES;

--
-- Table structure for table `users`
--

DROP TABLE IF EXISTS `users`;
/*!40101 SET @saved_cs_client     = @@character_set_client */;
/*!50503 SET character_set_client = utf8mb4 */;
CREATE TABLE `users` (
  `user_id` int NOT NULL AUTO_INCREMENT,
  `email` varchar(100) NOT NULL,
  `password` varchar(200) NOT NULL,
  PRIMARY KEY (`user_id`),
  UNIQUE KEY `email` (`email`)
) ENGINE=InnoDB DEFAULT CHARSET=utf8mb4 COLLATE=utf8mb4_0900_ai_ci;
/*!40101 SET character_set_client = @saved_cs_client */;

--
-- Dumping data for table `users`
--

LOCK TABLES `users` WRITE;
/*!40000 ALTER TABLE `users` DISABLE KEYS */;
/*!40000 ALTER TABLE `users` ENABLE KEYS */;
UNLOCK TABLES;
/*!40103 SET TIME_ZONE=@OLD_TIME_ZONE */;

/*!40101 SET SQL_MODE=@OLD_SQL_MODE */;
/*!40014 SET FOREIGN_KEY_CHECKS=@OLD_FOREIGN_KEY_CHECKS */;
/*!40014 SET UNIQUE_CHECKS=@OLD_UNIQUE_CHECKS */;
/*!40101 SET CHARACTER_SET_CLIENT=@OLD_CHARACTER_SET_CLIENT */;
/*!40101 SET CHARACTER_SET_RESULTS=@OLD_CHARACTER_SET_RESULTS */;
/*!40101 SET COLLATION_CONNECTION=@OLD_COLLATION_CONNECTION */;
/*!40111 SET SQL_NOTES=@OLD_SQL_NOTES */;

-- Dump completed on 2025-12-04 11:11:31
