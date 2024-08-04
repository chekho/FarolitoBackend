use Farolito_db;

--Finanzas
--ventas por producto para cada periodo
SELECT 
    YEAR(venta.fecha) AS Año,
    MONTH(venta.fecha) AS Mes,
    COUNT(detalleventa.id) AS NumeroDeVentas,
	receta.nombrelampara,
	sum(detalleventa.precioUnitario) as total
FROM 
    venta
JOIN 
    detalleventa ON venta.id = detalleventa.venta_id
JOIN 
    inventariolampara ON detalleventa.inventariolampara_id = inventariolampara.id
JOIN receta on receta.id = inventariolampara.receta_id
GROUP BY 
    YEAR(venta.fecha), MONTH(venta.fecha), receta.nombrelampara
ORDER BY 
    Año, Mes, receta.nombrelampara;


SELECT 
    inventariolampara.lote AS Producto,
    COUNT(detalleventa.id) AS NumeroDeVentas,
    SUM(detalleventa.precioUnitario * detalleventa.cantidad) AS TotalRecaudado
FROM 
    detalleventa
JOIN 
    inventariolampara ON detalleventa.inventariolampara_id = inventariolampara.id
GROUP BY 
    inventariolampara.lote
ORDER BY 
    TotalRecaudado DESC;

--Inventario
-- Materias Primas
SELECT 
    componentes.nombre AS Componente,
    SUM(inventariocomponentes.cantidad) AS Existencia
FROM 
    inventariocomponentes
JOIN 
    componentes ON inventariocomponentes.componentes_id = componentes.id
GROUP BY 
    componentes.nombre
ORDER BY 
    Existencia;

-- Productos Terminados
SELECT 
    receta.nombrelampara AS ProductoTerminado,
    SUM(inventariolampara.cantidad) AS Existencia
FROM 
    inventariolampara
JOIN 
    receta ON inventariolampara.receta_id = receta.id
GROUP BY 
    receta.nombrelampara
ORDER BY 
    Existencia;

--Compras
-- Clientes que más compras realizaron por periodo
SELECT 
    YEAR(venta.fecha) AS Año,
    MONTH(venta.fecha) AS Mes,
    AspNetUsers.FullName AS Cliente,
    COUNT(venta.id) AS NumeroDeCompras
FROM 
    venta
JOIN 
    AspNetUsers ON venta.usuario_id = AspNetUsers.id
GROUP BY 
    YEAR(venta.fecha), MONTH(venta.fecha), AspNetUsers.FullName
ORDER BY 
    NumeroDeCompras DESC;

-- Qué productos compraron
SELECT 
    AspNetUsers.FullName AS Cliente,
    inventariolampara.lote AS Producto,
    COUNT(detalleventa.id) AS NumeroDeVentas,
    SUM(detalleventa.precioUnitario * detalleventa.cantidad) AS TotalGastado
FROM 
    venta
JOIN 
    detalleventa ON venta.id = detalleventa.venta_id
JOIN 
    inventariolampara ON detalleventa.inventariolampara_id = inventariolampara.id
JOIN 
    AspNetUsers ON venta.usuario_id = AspNetUsers.id
GROUP BY 
    AspNetUsers.FullName, inventariolampara.lote
ORDER BY 
    TotalGastado DESC;

-- Mejor cliente global
SELECT top 1
    AspNetUsers.FullName AS MejorCliente,
    SUM(detalleventa.precioUnitario * detalleventa.cantidad) AS TotalGastado
FROM 
    venta
JOIN 
    detalleventa ON venta.id = detalleventa.venta_id
JOIN 
    AspNetUsers ON venta.usuario_id = AspNetUsers.id
GROUP BY 
    AspNetUsers.FullName
ORDER BY 
    TotalGastado DESC
;
