
from sqlalchemy import create_engine, delete 
from sqlalchemy import MetaData, Table, select, insert, func, cast, Float, exc
from sqlalchemy.orm import sessionmaker

server = 'DESKTOP-3K4LROA' #nombre del sevidor
username = 'sa' # nombre de usuario
password = 'root' #nombre de contraseña. (No son los míos, para que no me jaquien)
database = 'farolito_db' 
driver = 'ODBC Driver 17 for SQL Server'

connection_string = f'mssql+pyodbc://{username}:{password}@{server}/{database}?driver={driver}'

try:
    engine = create_engine(connection_string)
    metadata = MetaData()

    usuarios = Table("AspNetUsers", metadata, autoload_with=engine)
    detallecompra = Table("detallecompra", metadata, autoload_with=engine)
    detalleventa = Table("detalleventa", metadata, autoload_with=engine)
    venta = Table("venta", metadata, autoload_with=engine)
    produccion = Table("produccion", metadata, autoload_with=engine)
    inventariolampara = Table("inventariolampara", metadata, autoload_with=engine)
    receta = Table("receta", metadata, autoload_with=engine)
    compra = Table("compra", metadata, autoload_with=engine)
    inventariocomponentes = Table("inventariocomponentes", metadata, autoload_with=engine)
    componentes = Table("componentes", metadata, autoload_with=engine)
    detalleproduccion = Table("detalleproduccion", metadata, autoload_with=engine)
    solicitudproduccion = Table("solicitudproduccion", metadata, autoload_with=engine)
    componentesReceta = Table("componentesReceta", metadata, autoload_with=engine)
    proveedor = Table("proveedor", metadata, autoload_with=engine)
    mermacomponentes = Table("mermacomponentes", metadata, autoload_with=engine)

    # | Tablas dashboard
    # V
    #ComponentesComprados
    ComponentesComprados = Table("ComponentesComprados", metadata, autoload_with=engine)
    #ComponentesUsados
    ComponentesUsados = Table("ComponentesUsados", metadata, autoload_with=engine)
    #ComprasMes
    ComprasMes = Table("ComprasMes", metadata, autoload_with=engine)
    #ComprasProveedor
    ComprasProveedor = Table("ComprasProveedor", metadata, autoload_with=engine)
    #MasVendidos
    MasVendidos = Table("MasVendidos", metadata, autoload_with=engine)
    #MermaProveedor
    MermaProveedor = Table("MermaProveedor", metadata, autoload_with=engine)
    #VentasMes
    VentasMes = Table("VentasMes", metadata, autoload_with=engine)

    with engine.connect() as connection:
        Session = sessionmaker(bind=engine)
        session = Session()

        #Componentes Comprados
        query = select(componentes.c.nombre,
            func.sum(detallecompra.c.cantidad),
            func.sum(detallecompra.c.costo)
            ).join(inventariocomponentes, detallecompra.c.id == inventariocomponentes.c.detallecompra_id
                ).join(componentes, inventariocomponentes.c.componentes_id == componentes.c.id
                       ).group_by(componentes.c.nombre).order_by(func.sum(detallecompra.c.cantidad).desc())

        result = session.execute(query).fetchall()
        connection.execute(delete(ComponentesComprados))
        connection.commit()
        for row in result:
            stmt = insert(ComponentesComprados).values(NombreComponente = row[0], CantidadComprada=row[1], CostoTotalComprado=row[2])
            result = connection.execute(stmt)
            connection.commit()

        # Componentes más Usados
        query = select(componentes.c.nombre, func.count(detalleproduccion.c.id), func.sum(solicitudproduccion.c.Cantidad * componentesReceta.c.cantidad)
                       ).select_from(detalleproduccion.join(produccion, detalleproduccion.c.produccion_id == produccion.c.id)
                            .join(solicitudproduccion, produccion.c.solicitudproduccion_id == solicitudproduccion.c.id)
                            .join(inventariocomponentes, detalleproduccion.c.inventariocomponentes_id == inventariocomponentes.c.id)
                            .join(componentes, inventariocomponentes.c.componentes_id == componentes.c.id)
                            .join(receta, solicitudproduccion.c.receta_id == receta.c.id)
                            .join(componentesReceta, componentesReceta.c.receta_id == receta.c.id)).group_by(componentes.c.nombre).order_by(func.count(detalleproduccion.c.id).desc())

        result = session.execute(query).fetchall()
        connection.execute(delete(ComponentesUsados))
        connection.commit()
        for row in result:
            stmt = insert(ComponentesUsados).values(Componente = row[0], Usos=row[1], CantidadUsado=row[2])
            result = connection.execute(stmt)
            connection.commit()
        
        #ComprasMes
        query = select(func.year(compra.c.fecha).label('anio'),
            func.month(compra.c.fecha).label('mes'),
            func.sum(detallecompra.c.cantidad * detallecompra.c.costo).label('total_compras')
        ).select_from(
            compra.join(detallecompra, compra.c.id == detallecompra.c.compra_id)
        ).group_by(
            func.year(compra.c.fecha),
            func.month(compra.c.fecha)
        ).order_by(
            func.year(compra.c.fecha),
            func.month(compra.c.fecha)
        )

        result = session.execute(query).fetchall()
        connection.execute(delete(ComprasMes))
        connection.commit()
        for row in result:
            stmt = insert(ComprasMes).values(Anio = row[0], Mes=row[1], TotalCompras=row[2])
            result = connection.execute(stmt)
            connection.commit()

        #ComprasProveedor
        query = select(proveedor.c.nombreEmpresa, func.count(detallecompra.c.cantidad).label('total_compras')
        ).select_from(detallecompra.join(inventariocomponentes, detallecompra.c.id == inventariocomponentes.c.detallecompra_id).join(proveedor, inventariocomponentes.c.proveedor_id == proveedor.c.id)
        ).group_by(proveedor.c.nombreEmpresa).order_by(func.sum(detallecompra.c.cantidad * detallecompra.c.costo).desc())

        result = session.execute(query).fetchall()
        connection.execute(delete(ComprasProveedor))
        connection.commit()
        for rwo in result:
            stmt = insert(ComprasProveedor).values(NombreEmpresa=rwo[0], TotalCompras=rwo[1])
            result = connection.execute(stmt)
            connection.commit()

        # Productos más vendidos
        query_top_5_productos = select(receta.c.nombrelampara.label('nombre_producto'),func.sum(detalleventa.c.cantidad).label('cantidad_vendida')
            ).select_from(detalleventa.join(inventariolampara, detalleventa.c.inventariolampara_id == inventariolampara.c.id).join(receta, inventariolampara.c.receta_id == receta.c.id)
            ).group_by(receta.c.nombrelampara).order_by(func.sum(detalleventa.c.cantidad).desc()).limit(5)

        result_top_5_productos = session.execute(query_top_5_productos).fetchall()
        connection.execute(delete(MasVendidos))
        connection.commit()

        for row in result_top_5_productos:
            stmt = insert(MasVendidos).values(NombreProducto=row[0], CantidadVendida=row[1])
            result = connection.execute(stmt)
            connection.commit()

        #MermaProveedor
        query = select(
            func.sum(mermacomponentes.c.cantidad).label('mermado'),
            func.sum(detallecompra.c.cantidad).label('comprado'),
            (cast(func.sum(mermacomponentes.c.cantidad), Float) / cast(func.sum(detallecompra.c.cantidad), Float) * 100).label('porcentaje_mermado'),
            func.sum((detallecompra.c.costo / detallecompra.c.cantidad) * mermacomponentes.c.cantidad).label('total_mermado'),
            func.sum(detallecompra.c.costo).label('costo_total'),
            proveedor.c.nombreEmpresa
        ).select_from(mermacomponentes
        ).join(inventariocomponentes, inventariocomponentes.c.id == mermacomponentes.c.inventariocomponentes_id
        ).join(detallecompra, detallecompra.c.id == inventariocomponentes.c.detallecompra_id
        ).join(componentes, componentes.c.id == inventariocomponentes.c.componentes_id
        ).join(proveedor, proveedor.c.id == inventariocomponentes.c.proveedor_id
        ).group_by(proveedor.c.nombreEmpresa)

        result = session.execute(query).fetchall()
        connection.execute(delete(MermaProveedor))
        connection.commit()
        for row in result:
            stmt = insert(MermaProveedor).values(Mermado = row[0],	Comprado = row[1],	TotalMermado = row[3],	Costo = row[4], PorcentajeMermado = row[2],	NombreEmpresa = row[5])
            result = connection.execute(stmt)
            connection.commit()

        #ventasMes
        query_total_ventas_por_mes = select(
            func.year(venta.c.fecha).label('anio'),
            func.month(venta.c.fecha).label('mes'),
            func.count(venta.c.id).label('ventas'),
            func.sum(detalleventa.c.cantidad * detalleventa.c.precioUnitario).label('total_ventas')
        ).select_from(
            venta.join(detalleventa, venta.c.id == detalleventa.c.venta_id)
        ).group_by(
            func.year(venta.c.fecha),
            func.month(venta.c.fecha)
        ).order_by(
            func.year(venta.c.fecha),
            func.month(venta.c.fecha)
        )

        result_total_ventas_por_mes = session.execute(query_total_ventas_por_mes).fetchall()
        connection.execute(delete(VentasMes))
        connection.commit()
        for row in result_total_ventas_por_mes:
            stmt = insert(VentasMes).values(Anio=row[0],Mes=row[1],Ventas=row[2],TotalVentas=row[3])
            result = connection.execute(stmt)
            connection.commit()

except exc.SQLAlchemyError as e:
    print(f"Error con la base de datos: {e}")
except Exception as e:
    print(f"Error al conectar con la base de datos: {e}")
