using System;
using System.Collections.Generic;

namespace Idpsa.Paletizado.Definitions
{
    public interface ILocation
    {
        Locations Location { get; }
    }

    public enum Priority
    {
        Lowest=4,
        Low = 3,
        Medium = 2,
        Hight = 1,
        Hiest = 0
    }

    public interface ISolicitor : ILocation
    {
        Priority Priority { get; }
        ElementTypes? ElementRequested();
        IEnumerable<Locations> GetSuppliersLocations(); //En orden de prioridad
    }

    public interface ISupplier : ILocation
    {
        ElementTypes? ElementSupplied();
    }


    public enum Chains
    {
        Espera = 0,
        CatchPalet,
        CatchPaletInStore,
        LeavePalet,
        CatchItem,
        LeaveItem,
        CatchSeparator,
        LeaveSeparator,
        CheckItemOK,
        Transfer,
        GoToOrigin, //MDG.2011-06-14
        CheckItemOKDespaletizing //MDG.2013-04-25
    }

    [Flags]
    [Serializable]
    public enum Locations
    {
        None = 0,
        Entrada = 1,
        Reproceso = 2,
        Carton = 4,

        PaletJaponesa = 8,
        PaletAlemana = 16,

        Paletizado = 32,

        Paletizado1Japonesa = Paletizado | 64,
        Paletizado2Japonesa = Paletizado | 128,
        Paletizado3Japonesa = Paletizado | 256,

        PaletizadoAlemana = Paletizado | 512
    }

    public static class ChainsExtensions
    {
        public static bool Is(this Chains chain1, Chains chain2)
        {
            return ((chain1 & chain2) != 0);
        }
    }

    public static class LocationsExtensions
    {
        public static bool Is(this Locations location1, Locations location2)
        {
            return ((location1 & location2) != 0);
        }
    }

    //public struct CatchPaletParams { public const string position = "position";}
    //public struct CatchPaletParams { public const string position = "position"; public const string spin = "spin";}//MDG.2011-03-14.Añado spin para poder coger el palet Vikex girado
    //public struct CatchPaletParams { public const string position = "position"; public const string positionNextPalet = "positionNextPalet"; public const string spin = "spin";}//MDG.2011-06-22.Añado PositionNextPalet para permitir cogida del siguiente palet si no se puede coger el primero
    //public struct CatchPaletParams { public const string position = "position"; public const string positionNextPalet1 = "positionNextPalet1"; public const string positionNextPalet2 = "positionNextPalet2"; public const string positionNextPalet3 = "positionNextPalet3"; public const string spin = "spin";}//MDG.2011-06-22.Añado PositionNextPalet2 Y  para permitir cogida del siguiente palet si no se puede coger el primero
    public struct CatchPaletParams
    {
        public const string position = "position";
        public const string positionNextPalet1 = "positionNextPalet1";
        public const string positionNextPalet2 = "positionNextPalet2";
        public const string positionNextPalet3 = "positionNextPalet3";
        public const string positionNextPalet4 = "positionNextPalet4";
        public const string positionNextPalet5 = "positionNextPalet5";
        public const string spin = "spin";
    }

    //MDG.2011-06-27.6 posiciones de palets
    //public struct LeftPaletParams { public const string securityPosition1 = "securityPosition1"; public const string securityPosition2 = "securityPosition2"; public const string position = "position"; }
    public struct LeftPaletParams
    {
        public const string position = "position";
        public const string securityPosition1 = "securityPosition1";
        public const string securityPosition2 = "securityPosition2";
        public const string spin = "spin";
    }

    //MDG.2011-03-14.Añado spin para poder coger el palet Vikex girado
    public struct CatchBoxParams
    {
        public const string extensor = "extensor";
        public const string position = "position";
        public const string spin = "spin";
    }

    public struct LeftBoxParams
    {
        public const string extensor = "extensor";
        public const string position = "position";
        public const string securityPositions = "securityPositions";
        public const string spin = "spin";
    }

    public struct CatchBoardParams
    {
        public const string finalPosition = "position2";
        public const string initialPosition = "position1";
    }

    public struct LeftBoardParams
    {
        public const string position = "position";
    }

    public struct GoToOriginParams
    {
        public const string extensor = "extensor";
        public const string position = "position";
        public const string spin = "spin";
    }

    //MDG.2011-06-14
}