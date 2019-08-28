DROP DATABASE TrazabilidadPasaportes

GO
CREATE DATABASE TrazabilidadPasaportes

GO
Use TrazabilidadPasaportes

CREATE TABLE GruposPasaportes(
	PKey int IDENTITY NOT NULL PRIMARY KEY,
	ID varchar(20) NOT NULL UNIQUE,
    TipoPasaporte varchar(10) NOT NULL,
  	Fajado varchar(5) NOT NULL DEFAULT 'FALSE',       
    FechaInical DateTime NULL DEFAULT GETDATE(),
    FechaFinal DateTime NULL 
)

GO
Use TrazabilidadPasaportes

CREATE TABLE Pasaportes
(
	PKey int IDENTITY NOT NULl PRIMARY KEY,
	ID varchar(20) NOT NULL UNIQUE,
	IDGrupo varchar(20) NOT NULL
    FOREIGN KEY REFERENCES GruposPasaportes(ID),
    RfID varchar(20) NULL    
)