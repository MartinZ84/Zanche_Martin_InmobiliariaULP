-- phpMyAdmin SQL Dump
-- version 5.1.1
-- https://www.phpmyadmin.net/
--
-- Servidor: 127.0.0.1
-- Tiempo de generación: 22-04-2022 a las 02:37:30
-- Versión del servidor: 10.4.22-MariaDB
-- Versión de PHP: 8.0.13

SET SQL_MODE = "NO_AUTO_VALUE_ON_ZERO";
START TRANSACTION;
SET time_zone = "+00:00";


/*!40101 SET @OLD_CHARACTER_SET_CLIENT=@@CHARACTER_SET_CLIENT */;
/*!40101 SET @OLD_CHARACTER_SET_RESULTS=@@CHARACTER_SET_RESULTS */;
/*!40101 SET @OLD_COLLATION_CONNECTION=@@COLLATION_CONNECTION */;
/*!40101 SET NAMES utf8mb4 */;

--
-- Base de datos: `inmozanche`
--

-- --------------------------------------------------------

--
-- Estructura de tabla para la tabla `contratos`
--

CREATE TABLE `contratos` (
  `id` int(11) NOT NULL,
  `fechaInicio` date NOT NULL,
  `fechaFin` date NOT NULL,
  `estado` varchar(15) NOT NULL,
  `precio` int(11) NOT NULL,
  `inquilinoId` int(11) NOT NULL,
  `inmuebleId` int(11) NOT NULL,
  `dni_garante` varchar(10) NOT NULL,
  `nombre_garante` varchar(50) NOT NULL,
  `apellido_garante` varchar(50) NOT NULL,
  `telefono_garante` varchar(20) NOT NULL
) ENGINE=InnoDB DEFAULT CHARSET=utf8mb4;

--
-- Volcado de datos para la tabla `contratos`
--

INSERT INTO `contratos` (`id`, `fechaInicio`, `fechaFin`, `estado`, `precio`, `inquilinoId`, `inmuebleId`, `dni_garante`, `nombre_garante`, `apellido_garante`, `telefono_garante`) VALUES
(7, '2022-04-08', '2022-06-09', 'Vigente', 35000, 7, 3, '25888007', 'Sonia', 'Blade', '4588844'),
(8, '2022-04-16', '2022-05-07', 'Vigente', 25000, 3, 1, '25634455', 'Hector', 'Salamanca', '45887511'),
(9, '2022-04-21', '2022-04-13', 'No vigente', 10000, 7, 3, '25634', 'Jimy', 'Jofa', '34353322'),
(21, '2022-04-28', '2023-04-28', 'Vigente', 30000, 9, 7, '45687885', 'John', 'Hurtis', '24564654'),
(25, '2022-04-17', '2024-04-17', 'No vigente', 30000, 3, 1, '25888007', 'Miki', 'Jofa', '34353322'),
(29, '2022-04-18', '2024-04-18', 'Vigente', 1000, 9, 1, '45687885', 'John', 'hurtis', '24564654'),
(32, '2022-04-18', '2024-04-18', 'Vigente', 5, 9, 1, '000000', 'Caisedo', 'Diega', '545451'),
(33, '2022-04-18', '2024-04-18', 'Vigente', 3, 3, 3, '000000', 'kimi', 'Rukou', '545451'),
(34, '2022-04-18', '2024-04-18', 'Vigente', 66, 9, 7, '000000', 'Jimy', 'Jofa', '3435335'),
(35, '2022-04-18', '2024-04-18', 'Vigente', 43, 7, 7, '25634', 'Caisedo', 'Jofa', '4545222'),
(36, '2022-04-18', '2024-04-18', 'Vigente', 99999, 3, 3, '25888007', 'kimi', 'Ruk', '34353322'),
(37, '2022-04-18', '2024-04-18', 'Vigente', 8888, 7, 7, '2563452', 'Caisedo', 'Jofa', '54567885'),
(38, '2022-04-18', '2024-04-18', 'Vigente', 121212, 7, 3, '10', 'Miki', 'mouse', '24235'),
(39, '2022-04-18', '2024-04-18', 'Vigente', 33333, 3, 11, '4324', 'IAN', 'MACALEN', '7574'),
(41, '2022-04-18', '2024-04-18', 'Vigente', 45666, 12, 13, '35224545', 'Pipo', 'Pescador', '0800999888'),
(42, '2024-04-20', '2026-04-20', 'No vigente', 80000, 12, 13, '35224545', 'Pipo', 'Pescador', '0800999888'),
(43, '2022-06-01', '2022-08-01', 'Vigente', 55000, 12, 13, '21434331', 'John', 'Jofa', '34353322'),
(44, '2022-04-20', '2024-04-20', 'Vigente', 45000, 9, 1, '000000', 'Caisedo', 'Diega', '545451'),
(45, '2022-04-20', '2024-04-20', 'Vigente', 80000, 9, 1, '45687885', 'John', 'hurtis', '24564654'),
(46, '2022-08-01', '2023-08-01', 'Vigente', 50000, 3, 11, '45687885', 'Jimy', 'Hurtis', '3435335');

-- --------------------------------------------------------

--
-- Estructura de tabla para la tabla `inmuebles`
--

CREATE TABLE `inmuebles` (
  `id` int(11) NOT NULL,
  `direccion` varchar(100) NOT NULL,
  `ambientes` int(11) NOT NULL,
  `superficie` decimal(10,0) DEFAULT NULL,
  `tipo` varchar(50) NOT NULL,
  `uso` varchar(20) NOT NULL,
  `precio` int(11) NOT NULL,
  `latitud` decimal(10,0) DEFAULT NULL,
  `longitud` decimal(10,0) DEFAULT NULL,
  `estado` varchar(50) NOT NULL,
  `propietarioId` int(11) NOT NULL
) ENGINE=InnoDB DEFAULT CHARSET=utf8mb4;

--
-- Volcado de datos para la tabla `inmuebles`
--

INSERT INTO `inmuebles` (`id`, `direccion`, `ambientes`, `superficie`, `tipo`, `uso`, `precio`, `latitud`, `longitud`, `estado`, `propietarioId`) VALUES
(1, 'Mitre 878, Ciudad de San Luis', 3, '50', 'Departamento', 'Residencial', 25000, '10', '20', 'Disponible', 1),
(3, 'Rivadavia 523, Ciudad de San Luis', 5, '120', 'Casa', 'Residencial', 50000, '8522552', '5855252', 'Disponible', 6),
(7, 'San Justo 4555, Villa Larca, San Luis', 2, '70', 'Local', 'Comercial', 35000, '0', '0', 'No disponible', 4),
(11, 'San Telmo 123, CABA', 3, '76', 'Departamento', 'Residencial', 12345678, '0', '0', 'Disponible', 11),
(13, 'Colon 2542, Mendoza Capital', 4, '76', 'Oficina', 'Comercial', 67000, '0', '0', 'Disponible', 13);

-- --------------------------------------------------------

--
-- Estructura de tabla para la tabla `inquilinos`
--

CREATE TABLE `inquilinos` (
  `id` int(11) NOT NULL,
  `nombre` varchar(50) NOT NULL,
  `apellido` varchar(50) NOT NULL,
  `dni` varchar(10) NOT NULL,
  `telefono` varchar(20) NOT NULL,
  `email` varchar(100) NOT NULL,
  `lugar_trabajo` varchar(100) NOT NULL
) ENGINE=InnoDB DEFAULT CHARSET=utf8mb4;

--
-- Volcado de datos para la tabla `inquilinos`
--

INSERT INTO `inquilinos` (`id`, `nombre`, `apellido`, `dni`, `telefono`, `email`, `lugar_trabajo`) VALUES
(3, 'Laura', 'Inquilino', '45666878', '45468888', 'jose@inquilino.com', 'camboya'),
(7, 'Jose', 'Sexto', '45666878', '2645548844', 'marilauzan@gmail.com', 'oscar'),
(9, 'Roberto', 'Monez Ruiz', '5343233', '45455444', 'monezruiz@gobernator.com', 'Terrazas'),
(12, 'Cintia', 'Sosa', '33222444', '2657876523', 'cintia@sosa.com', 'Coraza');

-- --------------------------------------------------------

--
-- Estructura de tabla para la tabla `pagos`
--

CREATE TABLE `pagos` (
  `id` int(11) NOT NULL,
  `nroPago` int(11) NOT NULL,
  `fechaPago` datetime NOT NULL,
  `importe` decimal(10,2) NOT NULL,
  `contratoId` int(11) NOT NULL
) ENGINE=InnoDB DEFAULT CHARSET=utf8mb4;

--
-- Volcado de datos para la tabla `pagos`
--

INSERT INTO `pagos` (`id`, `nroPago`, `fechaPago`, `importe`, `contratoId`) VALUES
(3, 1, '2022-04-10 00:00:00', '15500.00', 7),
(4, 2, '2022-05-10 00:00:00', '15500.00', 7),
(7, 1, '2022-04-10 00:00:00', '20000.00', 9),
(8, 2, '2022-04-10 00:00:00', '10000.00', 9),
(17, 1, '2022-04-11 00:00:00', '20000.00', 8),
(18, 2, '2022-04-11 00:00:00', '20000.00', 8),
(20, 1, '2022-04-11 00:00:00', '20000.00', 21),
(22, 3, '2022-08-11 00:00:00', '10000.00', 9),
(23, 2, '2022-05-11 00:00:00', '30000.00', 21),
(25, 1, '2022-04-18 00:00:00', '45666.00', 41),
(26, 2, '2022-05-18 00:00:00', '45666.00', 41),
(27, 3, '2022-06-18 00:00:00', '45666.00', 41),
(28, 3, '2022-04-19 00:00:00', '25000.00', 8);

-- --------------------------------------------------------

--
-- Estructura de tabla para la tabla `propietarios`
--

CREATE TABLE `propietarios` (
  `id` int(11) NOT NULL,
  `nombre` varchar(50) NOT NULL,
  `apellido` varchar(50) NOT NULL,
  `dni` varchar(10) NOT NULL,
  `telefono` varchar(20) NOT NULL,
  `email` varchar(100) NOT NULL
) ENGINE=InnoDB DEFAULT CHARSET=utf8mb4;

--
-- Volcado de datos para la tabla `propietarios`
--

INSERT INTO `propietarios` (`id`, `nombre`, `apellido`, `dni`, `telefono`, `email`) VALUES
(1, 'Marcos', 'Paz', '25355655', '266421254', 'marcos@mail.com'),
(4, 'Camilo', 'Sexto', '78998545', '52455755122', 'camilos@mail.com'),
(6, 'Juan', 'Palomino', '12550550', '264545444', 'juan@palomino.com'),
(11, 'Jaime', 'Baili', '10545221', '1577858788', 'jaime@mail.com'),
(13, 'Carlos', 'Fernandez', '28990654', '45455444', 'carlos@fernandez.com');

-- --------------------------------------------------------

--
-- Estructura de tabla para la tabla `usuarios`
--

CREATE TABLE `usuarios` (
  `id` int(11) NOT NULL,
  `nombre` varchar(50) NOT NULL,
  `apellido` varchar(50) NOT NULL,
  `avatar` varchar(200) DEFAULT NULL,
  `email` varchar(100) NOT NULL,
  `clave` varchar(50) NOT NULL,
  `rol` int(11) NOT NULL
) ENGINE=InnoDB DEFAULT CHARSET=utf8mb4;

--
-- Volcado de datos para la tabla `usuarios`
--

INSERT INTO `usuarios` (`id`, `nombre`, `apellido`, `avatar`, `email`, `clave`, `rol`) VALUES
(1, 'Carla ', 'Petersons', '/Uploads\\avatar_1.jpg', 'carla@peterson.com', 'GjzAhuy78NH4O47XGFAHPsEk/lJVCR72X7szOdJVPJA=', 3),
(2, 'Pablo', 'Perez', '/Uploads\\avatar_2.jfif', 'pablo@perez.com', 'Es8xLXaQWGPhWN3ndWBEt8ZN7E8T+pDeqi210bMoJsI=', 3),
(3, 'Marcelo', 'Gaich', '/Uploads\\avatar_3.jpeg', 'marce@mail.com', 'GjzAhuy78NH4O47XGFAHPsEk/lJVCR72X7szOdJVPJA=', 2),
(5, 'Dora', 'Exloradora', '/Uploads\\avatar_5.jpeg', 'dora@mail.com', 'GjzAhuy78NH4O47XGFAHPsEk/lJVCR72X7szOdJVPJA=', 2);

--
-- Índices para tablas volcadas
--

--
-- Indices de la tabla `contratos`
--
ALTER TABLE `contratos`
  ADD PRIMARY KEY (`id`),
  ADD KEY `FK_INQUILINOID` (`inquilinoId`),
  ADD KEY `FK_INMUEBLEID` (`inmuebleId`);

--
-- Indices de la tabla `inmuebles`
--
ALTER TABLE `inmuebles`
  ADD PRIMARY KEY (`id`),
  ADD KEY `DELETE_INMUEBLE_CONTRATOS` (`propietarioId`);

--
-- Indices de la tabla `inquilinos`
--
ALTER TABLE `inquilinos`
  ADD PRIMARY KEY (`id`);

--
-- Indices de la tabla `pagos`
--
ALTER TABLE `pagos`
  ADD PRIMARY KEY (`id`),
  ADD KEY `FK_CONTRATOID` (`contratoId`);

--
-- Indices de la tabla `propietarios`
--
ALTER TABLE `propietarios`
  ADD PRIMARY KEY (`id`);

--
-- Indices de la tabla `usuarios`
--
ALTER TABLE `usuarios`
  ADD PRIMARY KEY (`id`);

--
-- AUTO_INCREMENT de las tablas volcadas
--

--
-- AUTO_INCREMENT de la tabla `contratos`
--
ALTER TABLE `contratos`
  MODIFY `id` int(11) NOT NULL AUTO_INCREMENT, AUTO_INCREMENT=47;

--
-- AUTO_INCREMENT de la tabla `inmuebles`
--
ALTER TABLE `inmuebles`
  MODIFY `id` int(11) NOT NULL AUTO_INCREMENT, AUTO_INCREMENT=14;

--
-- AUTO_INCREMENT de la tabla `inquilinos`
--
ALTER TABLE `inquilinos`
  MODIFY `id` int(11) NOT NULL AUTO_INCREMENT, AUTO_INCREMENT=13;

--
-- AUTO_INCREMENT de la tabla `pagos`
--
ALTER TABLE `pagos`
  MODIFY `id` int(11) NOT NULL AUTO_INCREMENT, AUTO_INCREMENT=29;

--
-- AUTO_INCREMENT de la tabla `propietarios`
--
ALTER TABLE `propietarios`
  MODIFY `id` int(11) NOT NULL AUTO_INCREMENT, AUTO_INCREMENT=14;

--
-- AUTO_INCREMENT de la tabla `usuarios`
--
ALTER TABLE `usuarios`
  MODIFY `id` int(11) NOT NULL AUTO_INCREMENT, AUTO_INCREMENT=7;

--
-- Restricciones para tablas volcadas
--

--
-- Filtros para la tabla `contratos`
--
ALTER TABLE `contratos`
  ADD CONSTRAINT `FK_INMUEBLEID` FOREIGN KEY (`inmuebleId`) REFERENCES `inmuebles` (`id`) ON DELETE NO ACTION ON UPDATE NO ACTION,
  ADD CONSTRAINT `FK_INQUILINOID` FOREIGN KEY (`inquilinoId`) REFERENCES `inquilinos` (`id`) ON DELETE NO ACTION ON UPDATE NO ACTION;

--
-- Filtros para la tabla `inmuebles`
--
ALTER TABLE `inmuebles`
  ADD CONSTRAINT `DELETE_INMUEBLE_CONTRATOS` FOREIGN KEY (`propietarioId`) REFERENCES `propietarios` (`id`);

--
-- Filtros para la tabla `pagos`
--
ALTER TABLE `pagos`
  ADD CONSTRAINT `FK_CONTRATOID` FOREIGN KEY (`contratoId`) REFERENCES `contratos` (`id`);
COMMIT;

/*!40101 SET CHARACTER_SET_CLIENT=@OLD_CHARACTER_SET_CLIENT */;
/*!40101 SET CHARACTER_SET_RESULTS=@OLD_CHARACTER_SET_RESULTS */;
/*!40101 SET COLLATION_CONNECTION=@OLD_COLLATION_CONNECTION */;
