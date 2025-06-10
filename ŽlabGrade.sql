-- phpMyAdmin SQL Dump
-- version 4.7.1
-- https://www.phpmyadmin.net/
--
-- Počítač: sqlskola.cps4c6aqi9mb.eu-central-1.rds.amazonaws.com
-- Vytvořeno: Ned 08. čen 2025, 19:32
-- Verze serveru: 8.0.41
-- Verze PHP: 7.0.33-0ubuntu0.16.04.16

SET SQL_MODE = "NO_AUTO_VALUE_ON_ZERO";
SET AUTOCOMMIT = 0;
START TRANSACTION;
SET time_zone = "+00:00";


/*!40101 SET @OLD_CHARACTER_SET_CLIENT=@@CHARACTER_SET_CLIENT */;
/*!40101 SET @OLD_CHARACTER_SET_RESULTS=@@CHARACTER_SET_RESULTS */;
/*!40101 SET @OLD_COLLATION_CONNECTION=@@COLLATION_CONNECTION */;
/*!40101 SET NAMES utf8mb4 */;

--
-- Databáze: `ŽlabGrade`
--

-- --------------------------------------------------------

--
-- Struktura tabulky `Credentials`
--

CREATE TABLE `Credentials` (
  `id_uzivatele` int NOT NULL,
  `jmeno` varchar(255) CHARACTER SET utf8mb4 COLLATE utf8mb4_czech_ci NOT NULL,
  `prijmeni` varchar(255) CHARACTER SET utf8mb4 COLLATE utf8mb4_czech_ci NOT NULL,
  `login` varchar(10) CHARACTER SET utf8mb4 COLLATE utf8mb4_czech_ci NOT NULL,
  `heslo` varchar(64) CHARACTER SET utf8mb4 COLLATE utf8mb4_czech_ci NOT NULL,
  `role` enum('Vedení','Učitel','Student') CHARACTER SET utf8mb4 COLLATE utf8mb4_czech_ci NOT NULL,
  `trida` varchar(4) CHARACTER SET utf8mb4 COLLATE utf8mb4_czech_ci NOT NULL
) ENGINE=InnoDB DEFAULT CHARSET=utf8mb4 COLLATE=utf8mb4_czech_ci;

--
-- Vypisuji data pro tabulku `Credentials`
--

INSERT INTO `Credentials` (`id_uzivatele`, `jmeno`, `prijmeni`, `login`, `heslo`, `role`, `trida`) VALUES
(1, 'Dennis', 'Ryšánek', 'vedeni', 'a71bbf9ab4c280d894154f16dc2def06b3405c65aceb3ec83ce7bb1b9519456f', 'Vedení', ''),
(2, 'Jiří', 'Richter', 'ucitel', '9448B63FDC2E3381B979E6E7F13947CC214A077C7B2BFD9477A5A67AA0E4C533', 'Učitel', ''),
(3, 'Fartin', 'Mraněk', 'student', '703B0A3D6AD75B649A28ADDE7D83C6251DA457549263BC7FF45EC709B0A8448B', 'Student', 'I2.B');

-- --------------------------------------------------------

--
-- Struktura tabulky `Grades`
--

CREATE TABLE `Grades` (
  `id_znamky` int NOT NULL,
  `id_zaka` int NOT NULL,
  `predmet` varchar(30) CHARACTER SET utf8mb4 COLLATE utf8mb4_czech_ci NOT NULL,
  `znamka` tinyint NOT NULL,
  `vaha` tinyint NOT NULL,
  `popis` varchar(30) CHARACTER SET utf8mb4 COLLATE utf8mb4_czech_ci NOT NULL,
  `datum` date NOT NULL
) ENGINE=InnoDB DEFAULT CHARSET=utf8mb4 COLLATE=utf8mb4_czech_ci;

--
-- Vypisuji data pro tabulku `Grades`
--

INSERT INTO `Grades` (`id_znamky`, `id_zaka`, `predmet`, `znamka`, `vaha`, `popis`, `datum`) VALUES
(1, 3, 'Český jazyk a literatura', 2, 7, 'Maryša', '2025-05-09'),
(2, 3, 'Matematika', 3, 5, 'Podobnost, Eukleidovy věty', '2025-05-07'),
(3, 3, 'Hardware a sítě', 3, 5, '9. Kontrolní výstup', '2025-05-13'),
(4, 3, 'Fyzika', 1, 4, 'test STR', '2025-05-10'),
(5, 3, 'Aplikační software', 1, 1, 'ER diagram', '2025-05-07'),
(7, 3, 'Základy společenských věd', 1, 5, 'Test - poválečné ČSR', '2025-05-01'),
(8, 3, 'Anglický jazyk', 1, 5, 'Reported speech', '2025-05-01'),
(9, 3, 'Anglický jazyk - B2 First', 1, 7, 'Listening', '2025-04-24'),
(10, 3, 'Hardware a sítě - cvičení', 1, 10, 'Čtvrtletní práce', '2025-04-15'),
(11, 3, 'Tělesná výchova', 1, 5, 'švihadlo', '2025-04-10'),
(12, 3, 'Anglický jazyk', 2, 6, 'Essay - Environment', '2025-05-14'),
(13, 3, 'Anglický jazyk', 1, 4, 'Vocabulary test', '2025-04-28'),
(14, 3, 'Anglický jazyk', 3, 5, 'Grammar quiz', '2025-04-15'),
(15, 3, 'Anglický jazyk', 2, 3, 'Reading comprehension', '2025-03-22'),
(16, 3, 'Anglický jazyk', 1, 6, 'Oral presentation', '2025-03-05'),
(17, 3, 'Aplikační software', 2, 4, 'PowerPoint prezentace', '2025-05-12'),
(18, 3, 'Aplikační software', 3, 3, 'MS Word - úprava textu', '2025-04-19'),
(19, 3, 'Aplikační software', 1, 2, 'Excel - funkce', '2025-03-29'),
(20, 3, 'Aplikační software', 2, 5, 'Formuláře', '2025-03-10'),
(21, 3, 'Aplikační software', 1, 3, 'Databázová tabulka', '2025-02-18'),
(22, 3, 'CAD Systémy', 2, 4, 'Kótování výkresu', '2025-04-21'),
(23, 3, 'CAD Systémy', 1, 6, '3D modelování', '2025-04-10'),
(24, 3, 'CAD Systémy', 3, 5, 'Projekt - výkres motoru', '2025-03-15'),
(25, 3, 'CAD Systémy', 2, 3, 'Základní tvary', '2025-02-28'),
(26, 3, 'CAD Systémy', 1, 7, 'Půdorys - zadání', '2025-02-10'),
(27, 3, 'Český jazyk a literatura', 3, 5, 'Sloh - popis', '2025-04-17'),
(28, 3, 'Český jazyk a literatura', 2, 4, 'Test - vyjmenovaná slova', '2025-03-26'),
(29, 3, 'Český jazyk a literatura', 1, 3, 'Literatura - Mácha', '2025-03-12'),
(30, 3, 'Český jazyk a literatura', 2, 6, 'Rozbor textu', '2025-02-20'),
(31, 3, 'Český jazyk a literatura', 3, 2, 'Práce s jazykem', '2025-01-29'),
(32, 3, 'Fyzika', 2, 4, 'Pohyby těles', '2025-04-22'),
(33, 3, 'Fyzika', 3, 5, 'Optika - test', '2025-03-30'),
(34, 3, 'Fyzika', 1, 6, 'Laboratorní práce - Newton', '2025-03-10'),
(35, 3, 'Fyzika', 2, 2, 'Mechanická práce', '2025-02-13'),
(36, 3, 'Fyzika', 1, 5, 'Elektřina - měření', '2025-01-20'),
(37, 3, 'Hardware a sítě', 2, 5, 'Topologie sítí', '2025-04-25'),
(38, 3, 'Hardware a sítě', 3, 4, 'Procesory - generace', '2025-03-19'),
(39, 3, 'Hardware a sítě', 1, 6, 'Základní desky', '2025-02-21'),
(40, 3, 'Hardware a sítě', 2, 3, 'Síťová zařízení', '2025-01-31'),
(41, 3, 'Hardware a sítě', 1, 7, 'Konfigurace PC', '2025-01-15'),
(42, 3, 'Hardware a sítě - cvičení', 2, 5, 'Montáž PC', '2025-04-29'),
(43, 3, 'Hardware a sítě - cvičení', 1, 4, 'Ping a Traceroute', '2025-03-25'),
(44, 3, 'Hardware a sítě - cvičení', 3, 3, 'Kabeláž', '2025-03-10'),
(45, 3, 'Hardware a sítě - cvičení', 1, 5, 'Práce v BIOSu', '2025-02-24'),
(46, 3, 'Hardware a sítě - cvičení', 2, 6, 'Síťové příkazy', '2025-01-22'),
(47, 3, 'Matematika', 2, 4, 'Kvadratické rovnice', '2025-05-05'),
(48, 3, 'Matematika', 1, 5, 'Funkce - lineární', '2025-04-18'),
(49, 3, 'Matematika', 3, 6, 'Výrazy', '2025-03-27'),
(50, 3, 'Matematika', 2, 2, 'Grafy funkcí', '2025-03-08'),
(51, 3, 'Matematika', 1, 4, 'Test - mocniny a odmocniny', '2025-02-14'),
(52, 3, 'Anglický jazyk - B2 First', 2, 6, 'Reading test', '2025-04-30'),
(53, 3, 'Anglický jazyk - B2 First', 1, 5, 'Use of English', '2025-04-12'),
(54, 3, 'Anglický jazyk - B2 First', 3, 3, 'Grammar task', '2025-03-22'),
(55, 3, 'Anglický jazyk - B2 First', 2, 4, 'Writing - essay', '2025-03-01'),
(56, 3, 'Anglický jazyk - B2 First', 1, 6, 'Speaking exam', '2025-02-18'),
(57, 3, 'Software', 2, 5, 'Instalace aplikací', '2025-04-27'),
(58, 3, 'Software', 3, 3, 'Licence a typy software', '2025-04-08'),
(59, 3, 'Software', 1, 6, 'Operační systémy', '2025-03-16'),
(60, 3, 'Software', 2, 4, 'Ovladače a aktualizace', '2025-02-26'),
(61, 3, 'Software', 1, 5, 'Příkazový řádek', '2025-01-30'),
(62, 3, 'Tělesná výchova', 2, 3, 'běh na 1500 m', '2025-04-19'),
(63, 3, 'Tělesná výchova', 1, 4, 'koordinace - překážková dráha', '2025-03-31'),
(64, 3, 'Tělesná výchova', 2, 5, 'hod medicinbalem', '2025-03-12'),
(65, 3, 'Tělesná výchova', 1, 6, 'skok do dálky', '2025-02-28'),
(66, 3, 'Tělesná výchova', 1, 3, 'basketbal - pravidla', '2025-01-17'),
(67, 3, 'Úvod do programování', 2, 5, 'Proměnné a datové typy', '2025-05-04'),
(68, 3, 'Úvod do programování', 1, 6, 'Cyklus for/while', '2025-04-16'),
(69, 3, 'Úvod do programování', 3, 4, 'Základy Pythonu', '2025-03-24'),
(70, 3, 'Úvod do programování', 1, 3, 'Funkce a procedury', '2025-03-03'),
(71, 3, 'Úvod do programování', 2, 7, 'Test - podmínky', '2025-02-14'),
(72, 3, 'Základy společenských věd', 2, 5, 'Filozofie - antika', '2025-05-15'),
(73, 3, 'Základy společenských věd', 1, 6, 'Občanská společnost', '2025-04-11'),
(74, 3, 'Základy společenských věd', 3, 4, 'Politologie - test', '2025-03-21'),
(75, 3, 'Základy společenských věd', 2, 3, 'Ekonomika - inflace', '2025-02-27'),
(76, 3, 'Základy společenských věd', 1, 5, 'Sociologie - rodina', '2025-01-25');

-- --------------------------------------------------------

--
-- Struktura tabulky `Noticeboard`
--

CREATE TABLE `Noticeboard` (
  `id_zpravy` int NOT NULL,
  `nadpis` varchar(255) CHARACTER SET utf8mb4 COLLATE utf8mb4_czech_ci NOT NULL,
  `zprava` varchar(255) CHARACTER SET utf8mb4 COLLATE utf8mb4_czech_ci NOT NULL,
  `autor` varchar(255) CHARACTER SET utf8mb4 COLLATE utf8mb4_czech_ci NOT NULL,
  `datum` datetime NOT NULL
) ENGINE=InnoDB DEFAULT CHARSET=utf8mb4 COLLATE=utf8mb4_czech_ci;

--
-- Vypisuji data pro tabulku `Noticeboard`
--

INSERT INTO `Noticeboard` (`id_zpravy`, `nadpis`, `zprava`, `autor`, `datum`) VALUES
(1, 'Autostadt a Hamburg', 'Vážené studentky, vážení studenti,\r\nna jaře 2025 (květen/červen) bychom opět chtěli zorganizovat poznávací zájezd do NĚMECKA, tentokrát AUTOSTAD a HAMBURG. Pročtěte si program a pokud byste měli zájem, přijďte se zapsat do kabinetu 207A do 11. října 2024.', 'Dennis Ryšánek', '2023-09-25 16:59:00'),
(2, 'Cyklistický kurz', 'V termínu 15.–19. září 2025 proběhne cyklistický kurz. Uskuteční se v Železných horách, s ubytováním v areálu Tesla Horní Bradlo. Zajištěna bude plná penze a doprava autobusem s vlekem na kola. Cena kurzu je 2 986 Kč. Kapacita je 30 míst.', 'Dennis Ryšánek', '2024-05-15 09:00:00'),
(3, 'Poslední zvonění', 'Ve středu 30. dubna nás čeká tradiční poslední zvonění – každoroční rozlučková akce, kterou si pro vás připravili naši čtvrťáci. Součástí programu bude celoškolní fotbalový turnaj. Chybět nebude ani hudební doprovod, který dotvoří atmosféru celé akce.', 'Dennis Ryšánek', '2025-04-22 17:55:00');

-- --------------------------------------------------------

--
-- Struktura tabulky `Schedules`
--

CREATE TABLE `Schedules` (
  `id_hodiny` int NOT NULL,
  `Trida` varchar(10) COLLATE utf8mb4_czech_ci NOT NULL,
  `ZkratkaDne` varchar(4) CHARACTER SET utf8mb4 COLLATE utf8mb4_czech_ci NOT NULL,
  `Hodina` tinyint NOT NULL,
  `ZkratkaPredmetu` varchar(5) COLLATE utf8mb4_czech_ci NOT NULL,
  `Ucitel` varchar(40) COLLATE utf8mb4_czech_ci NOT NULL,
  `ZkratkaUcitele` varchar(5) COLLATE utf8mb4_czech_ci NOT NULL,
  `Mistnost` varchar(5) COLLATE utf8mb4_czech_ci NOT NULL
) ENGINE=InnoDB DEFAULT CHARSET=utf8mb4 COLLATE=utf8mb4_czech_ci;

--
-- Vypisuji data pro tabulku `Schedules`
--

INSERT INTO `Schedules` (`id_hodiny`, `Trida`, `ZkratkaDne`, `Hodina`, `ZkratkaPredmetu`, `Ucitel`, `ZkratkaUcitele`, `Mistnost`) VALUES
(1, 'I2.B', 'po', 1, 'ZSV', 'Lukáš Vydra', 'Vy', '035'),
(2, 'I2.B', 'po', 2, 'MAT', 'Česlav Řehák', 'Řh', '303'),
(3, 'I2.B', 'po', 3, 'AJ', 'Adéla Ploužková', 'Pž', '209B'),
(4, 'I2.B', 'po', 4, 'FYZ', 'Jiří Kopal', 'Kp', '303'),
(5, 'I2.B', 'po', 5, 'B2F', 'Romana Strzyżová', 'Sz', '111'),
(6, 'I2.B', 'po', 6, 'ČJL', 'Aneta Holíková', 'Hl', '035'),
(7, 'I2.B', 'út', 1, 'HSc', 'František Štefanec', 'Št', '216'),
(8, 'I2.B', 'út', 2, 'HSc', 'František Štefanec', 'Št', '216'),
(9, 'I2.B', 'út', 3, 'HS', 'Václav Barták', 'Bt', '303'),
(10, 'I2.B', 'út', 4, 'HS', 'Václav Barták', 'Bt', '303'),
(11, 'I2.B', 'út', 5, 'OS', 'Jaroslav Vedral', 'Vd', '127'),
(12, 'I2.B', 'út', 6, 'OS', 'Jaroslav Vedral', 'Vd', '127'),
(13, 'I2.B', 'út', 8, 'UPG', 'Jiří Richter', 'Ri', '220'),
(14, 'I2.B', 'út', 9, 'UPG', 'Jiří Richter', 'Ri', '220'),
(15, 'I2.B', 'st', 2, 'TV', 'Petr Smoček Procházka', 'Sč', 'TVV'),
(16, 'I2.B', 'st', 3, 'SW', 'Michael Folbr', 'Fb', '130'),
(17, 'I2.B', 'st', 4, 'MAT', 'Česlav Řehák', 'Řh', '303'),
(18, 'I2.B', 'st', 6, 'AS', 'Kamil Friš', 'Fk', '216'),
(19, 'I2.B', 'st', 7, 'AS', 'Kamil Friš', 'Fk', '216'),
(20, 'I2.B', 'čt', 1, 'ZSV', 'Lukáš Vydra', 'Vy', '035'),
(21, 'I2.B', 'čt', 2, 'B2F', 'Romana Strzyżová', 'Sz', '303'),
(22, 'I2.B', 'čt', 3, 'AJ', 'Adéla Ploužková', 'Pž', '119'),
(23, 'I2.B', 'čt', 4, 'FYZ', 'Jiří Kopal', 'Kp', '311'),
(24, 'I2.B', 'čt', 5, 'SW', 'Michael Folbr', 'Fb', '112'),
(25, 'I2.B', 'čt', 6, 'ČJL', 'Aneta Holíková', 'Hl', '303'),
(26, 'I2.B', 'čt', 7, 'MAT', 'Česlav Řehák', 'Řh', '114'),
(27, 'I2.B', 'pá', 1, 'CAD', 'Danil Iakovlev', 'Iv', '217'),
(28, 'I2.B', 'pá', 2, 'CAD', 'Danil Iakovlev', 'Iv', '217'),
(29, 'I2.B', 'pá', 3, 'TV', 'Petr Smoček Procházka', 'Sč', 'TVM'),
(30, 'I2.B', 'pá', 4, 'AJ', 'Adéla Ploužková', 'Pž', '111'),
(31, 'I2.B', 'pá', 5, 'FYZ', 'Jiří Kopal', 'Kp', '303'),
(32, 'I2.B', 'pá', 6, 'ČJL', 'Aneta Holíková', 'Hl', '303'),
(33, 'I2.B', 'pá', 7, 'MAT', 'Česlav Řehák', 'Řh', '303');

-- --------------------------------------------------------

--
-- Struktura tabulky `Subjects`
--

CREATE TABLE `Subjects` (
  `id_predmetu` int NOT NULL,
  `predmet` varchar(30) CHARACTER SET utf8mb4 COLLATE utf8mb4_czech_ci NOT NULL,
  `trida` varchar(4) CHARACTER SET utf8mb4 COLLATE utf8mb4_czech_ci NOT NULL,
  `vyucujici` varchar(50) CHARACTER SET utf8mb4 COLLATE utf8mb4_czech_ci NOT NULL
) ENGINE=InnoDB DEFAULT CHARSET=utf8mb4 COLLATE=utf8mb4_czech_ci;

--
-- Vypisuji data pro tabulku `Subjects`
--

INSERT INTO `Subjects` (`id_predmetu`, `predmet`, `trida`, `vyucujici`) VALUES
(1, 'Anglický jazyk', 'I2.B', 'Jan Georgiev, Adéla Ploužková'),
(2, 'Aplikační software', 'I2.B', 'Ivana Durdilová, Kamil Friš'),
(3, 'CAD Systémy', 'I2.B', 'Danil Iakovlev'),
(4, 'Český jazyk a literatura', 'I2.B', 'Aneta Holíková'),
(5, 'Fyzika', 'I2.B', 'Jiří Kopal'),
(6, 'Hardware a sítě', 'I2.B', 'Václav Barták'),
(7, 'Hardware a sítě - cvičení', 'I2.B', 'Václav Barták, František Štefanec'),
(8, 'Matematika', 'I2.B', 'Česlav Řehák'),
(9, 'Anglický jazyk - B2 First', 'I2.B', 'Aneta Rezková, Romana Strzyźová'),
(10, 'Software', 'I2.B', 'Michael Folbr'),
(11, 'Tělesná výchová', 'I2.B', 'Petr Procházka Smoček'),
(12, 'Úvod do programování', 'I2.B', 'Jiří Richter'),
(13, 'Základy společenských věd', 'I2.B', 'Lukáš Ulovec Vydra');

--
-- Klíče pro exportované tabulky
--

--
-- Klíče pro tabulku `Credentials`
--
ALTER TABLE `Credentials`
  ADD PRIMARY KEY (`id_uzivatele`);

--
-- Klíče pro tabulku `Grades`
--
ALTER TABLE `Grades`
  ADD PRIMARY KEY (`id_znamky`);

--
-- Klíče pro tabulku `Noticeboard`
--
ALTER TABLE `Noticeboard`
  ADD PRIMARY KEY (`id_zpravy`);

--
-- Klíče pro tabulku `Schedules`
--
ALTER TABLE `Schedules`
  ADD PRIMARY KEY (`id_hodiny`);

--
-- Klíče pro tabulku `Subjects`
--
ALTER TABLE `Subjects`
  ADD PRIMARY KEY (`id_predmetu`);

--
-- AUTO_INCREMENT pro tabulky
--

--
-- AUTO_INCREMENT pro tabulku `Credentials`
--
ALTER TABLE `Credentials`
  MODIFY `id_uzivatele` int NOT NULL AUTO_INCREMENT, AUTO_INCREMENT=21;
--
-- AUTO_INCREMENT pro tabulku `Grades`
--
ALTER TABLE `Grades`
  MODIFY `id_znamky` int NOT NULL AUTO_INCREMENT, AUTO_INCREMENT=77;
--
-- AUTO_INCREMENT pro tabulku `Noticeboard`
--
ALTER TABLE `Noticeboard`
  MODIFY `id_zpravy` int NOT NULL AUTO_INCREMENT, AUTO_INCREMENT=12;
--
-- AUTO_INCREMENT pro tabulku `Schedules`
--
ALTER TABLE `Schedules`
  MODIFY `id_hodiny` int NOT NULL AUTO_INCREMENT, AUTO_INCREMENT=34;
--
-- AUTO_INCREMENT pro tabulku `Subjects`
--
ALTER TABLE `Subjects`
  MODIFY `id_predmetu` int NOT NULL AUTO_INCREMENT, AUTO_INCREMENT=14;COMMIT;

/*!40101 SET CHARACTER_SET_CLIENT=@OLD_CHARACTER_SET_CLIENT */;
/*!40101 SET CHARACTER_SET_RESULTS=@OLD_CHARACTER_SET_RESULTS */;
/*!40101 SET COLLATION_CONNECTION=@OLD_COLLATION_CONNECTION */;
