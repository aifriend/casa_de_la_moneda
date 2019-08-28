using System;
using System.Data.Linq;
using System.Data.Linq.Mapping;

namespace Idpsa
{
    public enum EntityTypes
    {
        Passport,
        Group,
        Box,
        Catalog
    }

    public interface IDBMappeable
    {
        DBAction DBAction { get; set; }
        DBEntity GetDBMapper();
    }

    public interface IEntity
    {
        EntityTypes EntityType { get; }
        string EntityId { get; }
        bool EqualEntity(IEntity obj);
    }

    public abstract class DBEntity : IEntity
    {
        private readonly EntityTypes entityType;

        public DBEntity(EntityTypes entityType)
        {
            this.entityType = entityType;
        }

        #region IEntity Members

        public EntityTypes EntityType
        {
            get { return entityType; }
        }

        public abstract string EntityId { get; }

        public bool EqualEntity(IEntity obj)
        {
            return (entityType == obj.EntityType && EntityId == obj.EntityId);
        }

        #endregion
    }

    [Table(Name = "Catalogos")]
    public class BDDatosCatalogo : DBEntity
    {
        public BDDatosCatalogo() :
            base(EntityTypes.Catalog)
        {
        }

        public override string EntityId
        {
            get { return ID; }
        }

        [Column(IsPrimaryKey = true)]
        public string ID { get; set; }

        [Column(CanBeNull = false)]
        public string Nacionalidad { get; set; }

        [Column(CanBeNull = false)]
        public string TipoPasaporte { get; set; }

        [Column(CanBeNull = false)]
        public string TipoRfid { get; set; }

        [Column(CanBeNull = false)]
        public int PesoPasaporte { get; set; }

        [Column(CanBeNull = false)]
        public string PasaporteInicial { get; set; }

        [Column(CanBeNull = false)]
        public int NumeroPasaportes { get; set; }

        [Column(CanBeNull = false)]
        public DateTime? FechaInicial { get; set; }

        [Column(CanBeNull = true)]
        public DateTime? FechaFinal { get; set; }
    }

    [Table(Name = "CajasPasaportes")]
    public class BDCajaPasaportes : DBEntity
    {
        public BDCajaPasaportes() :
            base(EntityTypes.Box)
        {
        }

        public override string EntityId
        {
            get { return ID + IDCatalogo; }
        }

        [Column(IsPrimaryKey = true)]
        public string ID { get; set; }

        [Column(IsPrimaryKey = true)]
        public string IDCatalogo { get; set; }

        [Column(CanBeNull = false)]
        public string CodigoBarras { get; set; }

        [Column(CanBeNull = false)]
        public DateTime? FechaInicial { get; set; }

        [Column(CanBeNull = true)]
        public DateTime? FechaFinal { get; set; }
    }

    [Table(Name = "GruposPasaportes")]
    public class BDGrupoPasaportes : DBEntity
    {
        public BDGrupoPasaportes() :
            base(EntityTypes.Group)
        {
        }

        public override string EntityId
        {
            get { return ID + IDCatalogo; }
        }

        [Column(IsPrimaryKey = true)]
        public string ID { get; set; }

        [Column(IsPrimaryKey = true)]
        public string IDCatalogo { get; set; }

        [Column(CanBeNull = false)]
        public string IDCaja { get; set; }

        [Column(CanBeNull = false)]
        public bool Fajado { get; set; }

        [Column(CanBeNull = false)]
        public DateTime? FechaInicial { get; set; }

        [Column(CanBeNull = true)]
        public DateTime? FechaFinal { get; set; }

        [Association(ThisKey = "ID,IDCatalogo", OtherKey = "IDGrupo,IDCatalogo")]
        public EntitySet<BDPasaporte> Pasaportes { get; set; }
    }

    [Table(Name = "Pasaportes")]
    public class BDPasaporte : DBEntity
    {
        public BDPasaporte() :
            base(EntityTypes.Passport)
        {
        }

        public override string EntityId
        {
            get { return ID + IDCatalogo; }
        }

        [Column(IsPrimaryKey = true)]
        public string ID { get; set; }

        [Column(IsPrimaryKey = true)]
        public string IDCatalogo { get; set; }


        [Column(CanBeNull = false)]
        public string IDGrupo { get; set; }

        [Column(CanBeNull = true)]
        public string RfID { get; set; }
    }
}