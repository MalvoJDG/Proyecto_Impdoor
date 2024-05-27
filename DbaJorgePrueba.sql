Create database PruebaJ;

Use PruebaJ;


Create table Productos
(
	id int AUTO_INCREMENT primary key,
    Nombre varchar(60)
);


Create table Materiales
(
	id int AUTO_INCREMENT primary key,
    Nombre varchar(60)
);

Create table Tipo_Madera
(
	id int AUTO_INCREMENT primary key,
    Nombre varchar(60)
);


Create table Apanelados
(
	id int AUTO_INCREMENT primary key,
    Nombre varchar(60)
);

Create table Jambas
(
	id int AUTO_INCREMENT primary key,
    Nombre varchar(60)
);

Create table Size
(
	id int AUTO_INCREMENT primary key,
    Medida float
);



Create table Precios
(
	id int AUTO_INCREMENT primary key,
    Precio float,
    Producto_id int references Productos(id),
    Material_id int references Material(id),
    Tipo_Madera int references Tipo_Madera(id),
    Apanelado_id int references Apanelados(id),
    Jambas_id int references Jambas(id),
    Size_id int references Size(id)
);

-- **********************************************    INSERTS ***************************************************************************

Insert into Productos(Nombre)
values('Puertas');

Insert into Materiales(Nombre)
values('Madera');

Insert into Tipo_Madera(Nombre)
values('Pino Americano');


Insert into Apanelados(Nombre)
values('Plywood'),
	  ('Madera'),
      ('Cristal'),
      ('Maciza');
      
insert into Jambas(Nombre)
values('Una'),
      ('Dos');


Insert into Size(Medida)
values(28),
	  (31),
      (35),
      (39),
      (43),
      (47);
      



-- EL PRIMERO EN PLYWOOD//////////////////////////////////////////////////////////////////////////////////
insert into Precios(Precio, Producto_id, Material_id, Tipo_Madera, Apanelado_id, Jambas_id, Size_id)
values(9500, 1, 1, 1, 1, 1, 1);

insert into Precios(Precio, Producto_id, Material_id, Tipo_Madera, Apanelado_id, Jambas_id, Size_id)
values(12900, 1, 1, 1, 1, 2, 1),
      (9500, 1, 1, 1, 1, 1, 2),
      (12900, 1, 1, 1, 1, 2, 2),
      (9500, 1, 1, 1, 1, 1, 3),
      (12900, 1, 1, 1, 1, 2, 3),
      (12800, 1, 1, 1, 1, 1, 4),
      (15800, 1, 1, 1, 1, 2, 4),
      (13500, 1, 1, 1, 1, 1, 5),
      (17500, 1, 1, 1, 1, 2, 5),
      (16500, 1, 1, 1, 1, 1, 6),
      (18900, 1, 1, 1, 1, 2, 6);

-- EL PRIMERO EN Madera//////////////////////////////////////////////////////////////////////////////////
insert into Precios(Precio, Producto_id, Material_id, Tipo_Madera, Apanelado_id, Jambas_id, Size_id)
values(12800, 1, 1, 1, 2, 1, 1),
      (14500, 1, 1, 1, 2, 2, 1),
      (12800, 1, 1, 1, 2, 1, 2),
      (14500, 1, 1, 1, 2, 2, 2),
      (12800, 1, 1, 1, 2, 1, 3),
      (14500, 1, 1, 1, 2, 2, 3),
      (16500, 1, 1, 1, 2, 1, 4),
      (18000, 1, 1, 1, 2, 2, 4),
      (17000, 1, 1, 1, 2, 1, 5),
      (19500, 1, 1, 1, 2, 2, 5),
      (18500, 1, 1, 1, 2, 1, 6),
      (20800, 1, 1, 1, 2, 2, 6);


-- EL PRIMERO EN CRISTAL//////////////////////////////////////////////////////////////////////////////////
insert into Precios(Precio, Producto_id, Material_id, Tipo_Madera, Apanelado_id, Jambas_id, Size_id)
values(16000, 1, 1, 1, 3, 1, 1),
      (18000, 1, 1, 1, 3, 2, 1),
      (16000, 1, 1, 1, 3, 1, 2),
      (18000, 1, 1, 1, 3, 2, 2),
      (16000, 1, 1, 1, 3, 1, 3),
      (18000, 1, 1, 1, 3, 2, 3),
      (17500, 1, 1, 1, 3, 1, 4),
      (19500, 1, 1, 1, 3, 2, 4),
      (18500, 1, 1, 1, 3, 1, 5),
      (21000, 1, 1, 1, 3, 2, 5),
      (19800, 1, 1, 1, 3, 1, 6),
      (22500, 1, 1, 1, 3, 2, 6);

-- EL PRIMERO EN Maciza//////////////////////////////////////////////////////////////////////////////////
insert into Precios(Precio, Producto_id, Material_id, Tipo_Madera, Apanelado_id, Jambas_id, Size_id)
values(18000, 1, 1, 1, 4, 1, 1),
      (18900, 1, 1, 1, 4, 2, 1),
      (18000, 1, 1, 1, 4, 1, 2),
      (18900, 1, 1, 1, 4, 2, 2),
      (18000, 1, 1, 1, 4, 1, 3),
      (18900, 1, 1, 1, 4, 2, 3),
      (19500, 1, 1, 1, 4, 1, 4),
      (21600, 1, 1, 1, 4, 2, 4),
      (20600, 1, 1, 1, 4, 1, 5),
      (23500, 1, 1, 1, 4, 2, 5),
      (21500, 1, 1, 1, 4, 1, 6),
      (24900, 1, 1, 1, 4, 2, 6);


-- CritianBello empezo////////////////////////////////////////////////////////////////////////////////////
insert into tipo_madera(Nombre)
values('Pino Tratado'),
	  ('Jequitiba');

-- Pino Tratado AP Plywood///////////////////////////////////////////////////////////////////////////////////
insert into Precios(Precio, Producto_id, Material_id, Tipo_Madera, Apanelado_id, Jambas_id, Size_id)
values(11000, 1, 1, 2, 1, 1, 1),
      (15500, 1, 1, 2, 1, 2, 1),
      (11000, 1, 1, 2, 1, 1, 2),
      (15500, 1, 1, 2, 1, 2, 2),
      (11000, 1, 1, 2, 1, 1, 3),
      (15000, 1, 1, 2, 1, 2, 3),
      (13500, 1, 1, 2, 1, 1, 4),
      (16800, 1, 1, 2, 1, 2, 4),
      (14500, 1, 1, 2, 1, 1, 5),
      (17500, 1, 1, 2, 1, 2, 5),
      (15000, 1, 1, 2, 1, 1, 6),
      (18500, 1, 1, 2, 1, 2, 6);
      
-- Pino Tratado AP Madera/////////////////////////////////////////////////////////////////////////////////
insert into Precios(Precio, Producto_id, Material_id, Tipo_Madera, Apanelado_id, Jambas_id, Size_id)
values(13800, 1, 1, 2, 2, 1, 1),
      (18500, 1, 1, 2, 2, 2, 1),
      (13800, 1, 1, 2, 2, 1, 2),
      (18500, 1, 1, 2, 2, 2, 2),
      (13800, 1, 1, 2, 2, 1, 3),
      (18500, 1, 1, 2, 2, 2, 3),
      (15800, 1, 1, 2, 2, 1, 4),
      (17900, 1, 1, 2, 2, 2, 4),
      (18500, 1, 1, 2, 2, 1, 5),
      (19500, 1, 1, 2, 2, 2, 5),
      (20800, 1, 1, 2, 2, 1, 6),
      (22500, 1, 1, 2, 2, 2, 6);
      
-- Pino Tratado AP Cristal/////////////////////////////////////////////////////////////////////////////////////////////
insert into Precios(Precio, Producto_id, Material_id, Tipo_Madera, Apanelado_id, Jambas_id, Size_id)
values(16500, 1, 1, 2, 3, 1, 1),
      (19900, 1, 1, 2, 3, 2, 1),
      (16500, 1, 1, 2, 3, 1, 2),
      (19900, 1, 1, 2, 3, 2, 2),
      (16500, 1, 1, 2, 3, 1, 3),
      (19900, 1, 1, 2, 3, 2, 3),
      (18800, 1, 1, 2, 3, 1, 4),
      (20500, 1, 1, 2, 3, 2, 4),
      (19500, 1, 1, 2, 3, 1, 5),
      (22500, 1, 1, 2, 3, 2, 5),
      (21800, 1, 1, 2, 3, 1, 6),
      (24500, 1, 1, 2, 3, 2, 6);
      
-- Pino Tratado Maciza//////////////////////////////////////////////////////////////////////////////////////
insert into Precios(Precio, Producto_id, Material_id, Tipo_Madera, Apanelado_id, Jambas_id, Size_id)
values(18500, 1, 1, 2, 4, 1, 1),
      (20500, 1, 1, 2, 4, 2, 1),
      (18500, 1, 1, 2, 4, 1, 2),
      (20500, 1, 1, 2, 4, 2, 2),
      (18500, 1, 1, 2, 4, 1, 3),
      (10500, 1, 1, 2, 4, 2, 3),
      (21500, 1, 1, 2, 4, 1, 4),
      (22900, 1, 1, 2, 4, 2, 4),
      (22900, 1, 1, 2, 4, 1, 5),
      (24500, 1, 1, 2, 4, 2, 5),
      (26800, 1, 1, 2, 4, 1, 6),
      (28600, 1, 1, 2, 4, 2, 6);

-- Jequitiba AP Plywood/////////////////////////////////////////////////////////////////////////////////////
insert into Precios(Precio, Producto_id, Material_id, Tipo_Madera, Apanelado_id, Jambas_id, Size_id)
values(14500, 1, 1, 3, 1, 1, 1),
      (16500, 1, 1, 3, 1, 2, 1),
      (14500, 1, 1, 3, 1, 1, 2),
      (16500, 1, 1, 3, 1, 2, 2),
      (14500, 1, 1, 3, 1, 1, 3),
      (16500, 1, 1, 3, 1, 2, 3),
      (17500, 1, 1, 3, 1, 1, 4),
      (19800, 1, 1, 3, 1, 2, 4),
      (18000, 1, 1, 3, 1, 1, 5),
      (21000, 1, 1, 3, 1, 2, 5),
      (20500, 1, 1, 3, 1, 1, 6),
      (22500, 1, 1, 3, 1, 2, 6);
      
-- Jequitiba AP Madera///////////////////////////////////////////////////////////////////////////////////
insert into Precios(Precio, Producto_id, Material_id, Tipo_Madera, Apanelado_id, Jambas_id, Size_id)
values(21500, 1, 1, 3, 2, 1, 1),
      (23000, 1, 1, 3, 2, 2, 1),
      (21500, 1, 1, 3, 2, 1, 2),
      (23000, 1, 1, 3, 2, 2, 2),
      (21500, 1, 1, 3, 2, 1, 3),
      (23500, 1, 1, 3, 2, 2, 3),
      (22500, 1, 1, 3, 2, 1, 4),
      (24500, 1, 1, 3, 2, 2, 4),
      (24500, 1, 1, 3, 2, 1, 5),
      (26000, 1, 1, 3, 2, 2, 5),
      (26500, 1, 1, 3, 2, 1, 6),
      (29500, 1, 1, 3, 2, 2, 6);

-- Jequitiba AP Cristal//////////////////////////////////////////////////////////////////////////////////
insert into Precios(Precio, Producto_id, Material_id, Tipo_Madera, Apanelado_id, Jambas_id, Size_id)
values(22000, 1, 1, 3, 3, 1, 1),
      (23800, 1, 1, 3, 3, 2, 1),
      (22000, 1, 1, 3, 3, 1, 2),
      (23800, 1, 1, 3, 3, 2, 2),
      (22000, 1, 1, 3, 3, 1, 3),
      (23800, 1, 1, 3, 3, 2, 3),
      (24000, 1, 1, 3, 3, 1, 4),
      (25300, 1, 1, 3, 3, 2, 4),
      (26800, 1, 1, 3, 3, 1, 5),
      (26800, 1, 1, 3, 3, 2, 5),
      (28000, 1, 1, 3, 3, 1, 6),
      (29800, 1, 1, 3, 3, 2, 6);
      
-- Jequitiba AP Maciza///////////////////////////////////////////////////////////////////////////////////
insert into Precios(Precio, Producto_id, Material_id, Tipo_Madera, Apanelado_id, Jambas_id, Size_id)
values(24500, 1, 1, 3, 4, 1, 1),
      (27900, 1, 1, 3, 4, 2, 1),
      (24500, 1, 1, 3, 4, 1, 2),
      (27000, 1, 1, 3, 4, 2, 2),
      (24500, 1, 1, 3, 4, 1, 3),
      (27000, 1, 1, 3, 4, 2, 3),
      (26500, 1, 1, 3, 4, 1, 4),
      (28500, 1, 1, 3, 4, 2, 4),
      (28500, 1, 1, 3, 4, 1, 5),
      (30800, 1, 1, 3, 4, 2, 5),
      (30500, 1, 1, 3, 4, 1, 6),
      (32500, 1, 1, 3, 4, 2, 6);

/* Los id son auotamitocs asi que no tienes que poner id, solo debes insertar la madera nueva en la tabla 
Tipo_madera Ej:

Insert into Tipo_Madera(Nombre)
values('Pino Americano');

*/

/*
	Luego solo pones los precios, te puedes guiar de los inserts de arriba. fijate bien que los id cuadren con lo que va.
*/



-- ************************************************* PROCEDURES **********************************************************************

DELIMITER //
CREATE PROCEDURE Buscar_Producto()
BEGIN
    select id, Nombre from Productos;
END //
DELIMITER ;


DELIMITER //
CREATE PROCEDURE Buscar_Materiales()
BEGIN
    select id, Nombre from Materiales;
END //
DELIMITER ;


DELIMITER //
CREATE PROCEDURE Buscar_Madera()
BEGIN
    select id, Nombre from Tipo_Madera;
END //
DELIMITER ;


DELIMITER //
CREATE PROCEDURE Buscar_Apanelado()
BEGIN
    select id, Nombre from Apanelados;
END //
DELIMITER ;

DELIMITER //
CREATE PROCEDURE Buscar_Jambas()
BEGIN
    select id, Nombre from Jambas;
END //
DELIMITER ;


DELIMITER //
CREATE PROCEDURE Buscar_Size()
BEGIN
    select id, Medida from Size;
END //
DELIMITER ;



DELIMITER //

CREATE PROCEDURE ObtenerPrecioProducto(
    IN producto_id INT,
    IN material_id INT,
    IN tipo_madera_id INT,
    IN apanelado_id INT,
    IN jambas_id INT,
    IN size_id INT
)
BEGIN
    SELECT
        pr.Precio
    FROM
        Precios pr
    JOIN
        Productos p ON pr.Producto_id = p.id
    JOIN
        Materiales m ON pr.Material_id = m.id
    JOIN
        Tipo_Madera tm ON pr.Tipo_Madera = tm.id
    JOIN
        Apanelados a ON pr.Apanelado_id = a.id
    JOIN
        Jambas j ON pr.Jambas_id = j.id
    JOIN
        Size s ON pr.Size_id = s.id
    WHERE
        pr.Producto_id = producto_id
        AND pr.Material_id = material_id
        AND pr.Tipo_Madera = tipo_madera_id
        AND pr.Apanelado_id = apanelado_id
        AND pr.Jambas_id = jambas_id
        AND pr.Size_id = size_id;
END //

DELIMITER ;
drop procedure ObtenerPrecioProducto;

SELECT
        pr.Precio
    FROM
        Precios pr
    JOIN
        Productos p ON pr.Producto_id = p.id
    JOIN
        Materiales m ON pr.Material_id = m.id
    JOIN
        Tipo_Madera tm ON pr.Tipo_Madera = tm.id
    JOIN
        Apanelados a ON pr.Apanelado_id = a.id
    JOIN
        Jambas j ON pr.Jambas_id = j.id
    JOIN
        Size s ON pr.Size_id = s.id
    WHERE
        pr.Producto_id = 1
        AND pr.Material_id = 1
        AND pr.Tipo_Madera = 3
        AND pr.Apanelado_id = 1
        AND pr.Jambas_id = 2
        AND pr.Size_id = 1;







