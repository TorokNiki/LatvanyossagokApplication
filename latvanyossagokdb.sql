-- phpMyAdmin SQL Dump
-- version 4.7.9
-- https://www.phpmyadmin.net/
--
-- Gép: 127.0.0.1:3306
-- Létrehozás ideje: 2019. Nov 29. 09:30
-- Kiszolgáló verziója: 5.7.21
-- PHP verzió: 5.6.35

SET SQL_MODE = "NO_AUTO_VALUE_ON_ZERO";
SET AUTOCOMMIT = 0;
START TRANSACTION;
SET time_zone = "+00:00";


/*!40101 SET @OLD_CHARACTER_SET_CLIENT=@@CHARACTER_SET_CLIENT */;
/*!40101 SET @OLD_CHARACTER_SET_RESULTS=@@CHARACTER_SET_RESULTS */;
/*!40101 SET @OLD_COLLATION_CONNECTION=@@COLLATION_CONNECTION */;
/*!40101 SET NAMES utf8mb4 */;

--
-- Adatbázis: `latvanyossagokdb`
--
CREATE DATABASE IF NOT EXISTS `latvanyossagokdb` DEFAULT CHARACTER SET utf8 COLLATE utf8_hungarian_ci;
USE `latvanyossagokdb`;

-- --------------------------------------------------------

--
-- Tábla szerkezet ehhez a táblához `latvanyossagok`
--

DROP TABLE IF EXISTS `latvanyossagok`;
CREATE TABLE IF NOT EXISTS `latvanyossagok` (
  `id` int(11) NOT NULL AUTO_INCREMENT,
  `nev` varchar(128) COLLATE utf8_hungarian_ci NOT NULL,
  `leiras` varchar(128) COLLATE utf8_hungarian_ci NOT NULL,
  `ar` int(11) NOT NULL DEFAULT '0',
  `varos_id` int(11) NOT NULL,
  PRIMARY KEY (`id`),
  KEY `varos_id` (`varos_id`)
) ENGINE=InnoDB DEFAULT CHARSET=utf8 COLLATE=utf8_hungarian_ci;

-- --------------------------------------------------------

--
-- Tábla szerkezet ehhez a táblához `varosok`
--

DROP TABLE IF EXISTS `varosok`;
CREATE TABLE IF NOT EXISTS `varosok` (
  `id` int(11) NOT NULL AUTO_INCREMENT,
  `nev` varchar(128) COLLATE utf8_hungarian_ci NOT NULL,
  `lakossag` int(11) NOT NULL,
  PRIMARY KEY (`id`),
  UNIQUE KEY `nev_indx` (`nev`)
) ENGINE=InnoDB DEFAULT CHARSET=utf8 COLLATE=utf8_hungarian_ci;

--
-- Megkötések a kiírt táblákhoz
--

--
-- Megkötések a táblához `latvanyossagok`
--
ALTER TABLE `latvanyossagok`
  ADD CONSTRAINT `latvanyossagok_ibfk_1` FOREIGN KEY (`varos_id`) REFERENCES `varosok` (`id`);
COMMIT;

/*!40101 SET CHARACTER_SET_CLIENT=@OLD_CHARACTER_SET_CLIENT */;
/*!40101 SET CHARACTER_SET_RESULTS=@OLD_CHARACTER_SET_RESULTS */;
/*!40101 SET COLLATION_CONNECTION=@OLD_COLLATION_CONNECTION */;
