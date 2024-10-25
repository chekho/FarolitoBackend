-- Crear login
CREATE LOGIN usuario_farolito WITH PASSWORD = 'D5eqY6X8Mm12.KY';

USE farolito_db;

-- usuario en la bd
drop user usuario_farolito;
CREATE USER usuario_farolito FOR LOGIN usuario_farolito;

-- Permisos generales
GRANT SELECT, INSERT, UPDATE TO usuario_farolito;

-- Permisos especiales
GRANT SELECT, INSERT, UPDATE, DELETE ON Carritos TO usuario_farolito;
GRANT SELECT, INSERT, UPDATE, DELETE ON Productoprovedors TO usuario_farolito;
GRANT SELECT, INSERT, UPDATE, DELETE ON proveedores TO usuario_farolito;
GRANT SELECT, INSERT, UPDATE, DELETE ON Componentesreceta TO usuario_farolito;
