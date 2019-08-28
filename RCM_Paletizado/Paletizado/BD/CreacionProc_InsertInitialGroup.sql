USE TrazabilidadPasaportes
GO

CREATE PROC InsertInitialGroup
  @ID varchar(20),
  @TipoPasaporte varchar(10),
  @Fajado varchar(5)='FALSE',
  @FechaInicial datetime=NULL,
  @FechaFinal datetime=NULL 
 
  
AS
DECLARE @IniDate datetime
Select @IniDate=@FechaInicial

IF @IniDate is null
  select @IniDate= GetDate()
  
  INSERT INTO GruposPasaportes
  VALUES
  (@ID,@TipoPasaporte,@Fajado,@IniDate,@FechaFinal)   
  
  RETURN @@ERROR









