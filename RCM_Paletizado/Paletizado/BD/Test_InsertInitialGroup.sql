use TrazabilidadPasaportes
declare @error int

EXEC InsertInitialGroup
      @ID='AB000006',
      @TipoPasaporte='Español'

SELECT @error as Error
SELECT * FROM GruposPasaportes 