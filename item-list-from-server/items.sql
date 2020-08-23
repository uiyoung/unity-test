-- phpMyAdmin SQL Dump
-- version 4.9.5
-- https://www.phpmyadmin.net/
--
-- Host: localhost
-- Generation Time: Aug 23, 2020 at 09:32 PM
-- Server version: 10.5.5-MariaDB-1:10.5.5+maria~focal
-- PHP Version: 7.4.3

SET SQL_MODE = "NO_AUTO_VALUE_ON_ZERO";
SET AUTOCOMMIT = 0;
START TRANSACTION;
SET time_zone = "+00:00";


/*!40101 SET @OLD_CHARACTER_SET_CLIENT=@@CHARACTER_SET_CLIENT */;
/*!40101 SET @OLD_CHARACTER_SET_RESULTS=@@CHARACTER_SET_RESULTS */;
/*!40101 SET @OLD_COLLATION_CONNECTION=@@COLLATION_CONNECTION */;
/*!40101 SET NAMES utf8mb4 */;

--
-- Database: `suy_db`
--

-- --------------------------------------------------------

--
-- Table structure for table `items`
--

CREATE TABLE `items` (
  `id` int(11) NOT NULL,
  `name` varchar(100) NOT NULL,
  `description` text NOT NULL,
  `price` int(100) NOT NULL,
  `iconURL` text DEFAULT NULL,
  `imgVer` int(10) NOT NULL
) ENGINE=InnoDB DEFAULT CHARSET=utf8mb4;

--
-- Dumping data for table `items`
--

INSERT INTO `items` (`id`, `name`, `description`, `price`, `iconURL`, `imgVer`) VALUES
(1, 'stick', 'a simple stick.', 200, 'https://uiyoung.cf/test/item-icons2/stick.png', 0),
(2, 'Purple Potion', 'Heals 400 health.', 100, 'https://uiyoung.cf/test/item-icons2/purple-potion.png', 0),
(3, 'Fior Sal', 'Restores some mana.', 50, 'https://uiyoung.cf/test/item-icons2/fior-sal.png', 0),
(4, 'Fior Athar', 'Revive a fallen ally.\r\n', 100, 'https://uiyoung.cf/test/item-icons2/fior-athar.png', 0),
(6, 'Eppe', 's:3m13\r\nL:5m15', 1000, 'https://uiyoung.cf/test/item-icons2/eppe.png', 1),
(7, 'Emerald Sword', '-2000 MP\r\n+1int,+1wis', 10000, 'https://uiyoung.cf/test/item-icons2/emerald-sword.png', 0),
(8, 'Dragon Scale Sword', 's:160m220\r\nL:180m240', 200000, 'https://uiyoung.cf/test/item-icons2/dragon-scale-sword.png', 0),
(9, 'Holy Diana', 'staff for wizard', 5000, 'https://uiyoung.cf/test/item-icons2/holy-diana.png', 0),
(10, 'Light Dagger', 'Light Dagger for rogue', 5000, 'https://uiyoung.cf/test/item-icons2/light-dagger.png', 0),
(11, 'Wooden Bow', 'Wooden Bow for archer.', 10000, 'https://uiyoung.cf/test/item-icons2/wooden-bow.png', 0),
(12, 'Wooden Harp', 'A wooden harp for bard.', 3000, 'https://uiyoung.cf/test/item-icons2/wooden-harp.png', 0);

--
-- Indexes for dumped tables
--

--
-- Indexes for table `items`
--
ALTER TABLE `items`
  ADD PRIMARY KEY (`id`);

--
-- AUTO_INCREMENT for dumped tables
--

--
-- AUTO_INCREMENT for table `items`
--
ALTER TABLE `items`
  MODIFY `id` int(11) NOT NULL AUTO_INCREMENT, AUTO_INCREMENT=13;
COMMIT;

/*!40101 SET CHARACTER_SET_CLIENT=@OLD_CHARACTER_SET_CLIENT */;
/*!40101 SET CHARACTER_SET_RESULTS=@OLD_CHARACTER_SET_RESULTS */;
/*!40101 SET COLLATION_CONNECTION=@OLD_COLLATION_CONNECTION */;
