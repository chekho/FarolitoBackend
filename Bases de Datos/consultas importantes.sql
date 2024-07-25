--use farolito_db_v2;
-- drop database farolito_db;
use farolito_db;

select * from inventariolampara;
select c.id,	c.cantidad, cc.fecha from inventariocomponentes c join detallecompra d on d.id = c.detallecompra_id join compra cc on cc.id = d.compra_id
where c.cantidad < 1;

select * from mermacomponentes;



select 
 sum(i.cantidad) as existente,
 sum(c.cantidad)*50 as 'necesario',
 c.componentes_id
from inventariocomponentes i
join (select cantidad, componentes_id from componentesreceta) as c
on c.componentes_id = i.componentes_id
group by c.componentes_id;

-- cantidad	componentes_id	fecha	id                            select sum(cantidad)*50 as necesario, componentes_id from componentesreceta group by componentes_id;
select i.cantidad, i.componentes_id, dc.* from inventariocomponentes i
join (select c.fecha, d.id from detallecompra d join compra c on c.id = d.compra_id) dc on dc.id = i.detallecompra_id
-- where i.componentes_id = 1
order by i.componentes_id, dc.fecha;
