
from sqlalchemy import create_engine, delete, extract, update 
from sqlalchemy import MetaData, Table, select, insert, func, exc
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
    
    usuario = Table("aspNetUsers", metadata, autoload_with=engine)
    venta = Table("venta", metadata, autoload_with=engine)
    detalleventa = Table("detalleventa", metadata, autoload_with=engine)
    inventariolampara = Table("inventariolampara", metadata, autoload_with=engine)
    receta = Table("receta", metadata, autoload_with=engine)
    produccion = Table("produccion", metadata, autoload_with=engine)
    componentes = Table("componentes", metadata, autoload_with=engine)
    inventariocomponentes = Table("inventariocomponentes", metadata, autoload_with=engine)

    # Tablas dashboard
    ventasProductoPeriodo = Table("ventasProductoPeriodo", metadata, autoload_with=engine)
    ventaProducto = Table("ventaProducto", metadata, autoload_with=engine)
    ExistenciaComponente = Table("ExistenciaComponente", metadata, autoload_with=engine)
    ExistenciaLampara = Table("ExistenciaLampara", metadata, autoload_with=engine)    
    ventasPeriodo = Table("ventasPeriodo", metadata, autoload_with=engine) # Usuarios
    LamparaCliente = Table("LamparaCliente", metadata, autoload_with=engine)
    MejorCliente = Table("MejorCliente", metadata, autoload_with=engine) 

    with engine.connect() as connection:
        Session = sessionmaker(bind=engine)
        session = Session()
        
        query = select(
            func.YEAR(venta.c.fecha).label('Año'),
            func.MONTH(venta.c.fecha).label('Mes'),
            func.count(detalleventa.c.id).label('numero_ventas'),
            receta.c.nombrelampara.label('producto'),
            func.sum(detalleventa.c.precioUnitario).label('total_recaudado')
        ).join(detalleventa).join(inventariolampara).join(receta).group_by(
            func.YEAR(venta.c.fecha), func.MONTH(venta.c.fecha), receta.c.nombrelampara
        ).order_by(
            'Año', 'Mes', receta.c.nombrelampara
        )

        result = session.execute(query).fetchall()
        connection.execute(delete(ventasProductoPeriodo))
        connection.commit()
        for row in result:
            stmt = insert(ventasProductoPeriodo).values(
                Anio = row[0],
                Mes = row[1],
                Producto=row[3],
                NumeroDeVentas=row[2]
            )
            connection.execute(stmt)
            connection.commit()

        ventas_por_producto = session.query(
            receta.c.nombrelampara.label('Producto'),
            func.count(detalleventa.c.id).label('NumeroDeVentas'),
            func.sum(detalleventa.c.precioUnitario * detalleventa.c.cantidad).label('TotalRecaudado'),
            func.count(inventariolampara.c.id).label('f')
        ).join(detalleventa, detalleventa.c.inventariolampara_id == inventariolampara.c.id)\
        .join(receta).group_by(
            receta.c.nombrelampara
        ).order_by(
            func.sum(detalleventa.c.precioUnitario * detalleventa.c.cantidad).desc()
        ).all()

        connection.execute(delete(ventaProducto))
        connection.commit()
        for row in ventas_por_producto:
            stmt = insert(ventaProducto).values(
                Producto=row.Producto,
                NumeroDeVentas=row.NumeroDeVentas,
                TotalRecaudado=row.TotalRecaudado
            )
            connection.execute(stmt)
            connection.commit()

        existencias_materias_primas = session.query(
            componentes.c.nombre.label('Componente'),
            func.sum(inventariocomponentes.c.cantidad).label('Existencia')
        ).join(inventariocomponentes, inventariocomponentes.c.componentes_id == componentes.c.id).group_by(
            componentes.c.nombre
        ).order_by(
            'Existencia'
        ).all()

        connection.execute(delete(ExistenciaComponente))
        connection.commit()
        for existencia in existencias_materias_primas:
            stmt = insert(ExistenciaComponente).values(
                Componente=existencia.Componente,
                Existencia=existencia.Existencia,
            )
            connection.execute(stmt)
            connection.commit()

        existencias_productos_terminados = session.query(
            receta.c.nombrelampara.label('ProductoTerminado'),
            func.sum(inventariolampara.c.cantidad).label('Existencia')
        ).join(inventariolampara, inventariolampara.c.receta_id == receta.c.id).group_by(
            receta.c.nombrelampara
        ).order_by(
            'Existencia'
        ).all()

        connection.execute(delete(ExistenciaLampara))
        connection.commit()
        for existencia in existencias_productos_terminados:
            stmt = insert(ExistenciaLampara).values(
                ProductoTerminado=existencia.ProductoTerminado,
                Existencia=existencia.Existencia,
            )
            connection.execute(stmt)
            connection.commit()

        compras_por_periodo = session.query(
            extract('year', venta.c.fecha).label('Año'),
            extract('month', venta.c.fecha).label('Mes'),
            usuario.c.FullName.label('Cliente'),
            func.count(venta.c.id).label('NumeroDeCompras')
        ).join(usuario, venta.c.usuario_id == usuario.c.Id).group_by(
            extract('year', venta.c.fecha),
            extract('month', venta.c.fecha),
            usuario.c.FullName
        ).order_by(
            func.count(venta.c.id).desc()
        ).all()

        session.execute(delete(ventasPeriodo))
        session.commit()
        for compra in compras_por_periodo:
            stmt = insert(ventasPeriodo).values(
                Año=compra.Año,
                Mes=compra.Mes,
                Cliente=compra.Cliente,
                NumeroDeCompras = compra.NumeroDeCompras
            )
            session.execute(stmt)
        
        productos_comprados_por_cliente = session.query(
            usuario.c.FullName.label('Cliente'),
            receta.c.nombrelampara.label('Producto'),
            func.count(detalleventa.c.id).label('NumeroDeVentas'),
            func.sum(detalleventa.c.precioUnitario * detalleventa.c.cantidad).label('TotalGastado')
        ).join(venta, venta.c.usuario_id == usuario.c.Id).join(detalleventa, detalleventa.c.venta_id == venta.c.id).join(inventariolampara, detalleventa.c.inventariolampara_id == inventariolampara.c.id
        ).join(receta, receta.c.id == inventariolampara.c.receta_id).group_by(
            usuario.c.FullName, receta.c.nombrelampara
        ).order_by(
            func.sum(detalleventa.c.precioUnitario * detalleventa.c.cantidad).desc()
        ).all()

        session.execute(delete(LamparaCliente))
        session.commit()
        for producto in productos_comprados_por_cliente:
            stmt = insert(LamparaCliente).values(
                Cliente=producto.Cliente,
                Producto=producto.Producto,
                NumeroDeVentas = producto.NumeroDeVentas,
                TotalGastado=producto.TotalGastado
            )
            session.execute(stmt)

        mejor_cliente = session.query(
            usuario.c.FullName.label('MejorCliente'),
            func.sum(detalleventa.c.precioUnitario * detalleventa.c.cantidad).label('TotalGastado')
        ).join(venta, venta.c.usuario_id == usuario.c.Id).join(detalleventa, detalleventa.c.venta_id == venta.c.id).group_by(
            usuario.c.FullName
        ).order_by(
            func.sum(detalleventa.c.precioUnitario * detalleventa.c.cantidad).desc()
        ).first()

        mc = session.query(MejorCliente).first()
        if mc:
            stmt = update(MejorCliente).values(Cliente = mejor_cliente[0], TotalGastado = mejor_cliente[1])
        else:
            stmt = insert(MejorCliente).values(Cliente = mejor_cliente[0], TotalGastado = mejor_cliente[1])
        session.execute(stmt)
        session.commit() 
except exc.SQLAlchemyError as e:
    print(f"Error con la base de datos: {e}")
except Exception as e:
    print(f"Error al conectar con la base de datos: {e}")
