using CsvHelper.Configuration.Attributes;
using System.Drawing;

namespace MinutiaCalcLib
{
    public class ComparedPair
    {
        [Name("ID PARY")]
        public string UniversalId { get; set; }

        [Name("ID 1 PARY")]
        public string BaseId { get; set; }

        [Name("RUCH 1 PARY")]
        public string BaseMoveType { get; set; }

        [Name("TYP ODCISKU PALCA")]
        public string BaseFingerprintType { get; set; }

        [Name("PUNKT 1")]
        public Point? BaseFirstPoint { get; set; }

        [Name("KĄT 1")]
        public double BaseFirstAngle { get; set; }

        [Name("OBSZAR 1")]
        public string BaseFirstArea { get; set; }

        [Name("PUNKT 2")]
        public Point? BaseSecondPoint { get; set; }

        [Name("KĄT 2")]
        public double BaseSecondAngle { get; set; }

        [Name("OBSZAR 2")]
        public string BaseSecondArea { get; set; }

        [Name("DYSTANS 1 - 2")]
        public double BaseDistance { get; set; }

        [Name("ID 2 PARY")]
        public string OtherId { get; set; }

        [Name("RUCH 2 PARY")]
        public string OtherMoveType { get; set; }

        [Name("TYP ODCISKU PALCA")]
        public string OtherFingerprintType { get; set; }

        [Name("PUNKT 3")]
        public Point? OtherFirstPoint { get; set; }

        [Name("KĄT 3")]
        public double OtherFirstAngle { get; set; }

        [Name("OBSZAR 3")]
        public string OtherFirstArea { get; set; }

        [Name("PUNKT 4")]
        public Point? OtherSecondPoint { get; set; }

        [Name("KĄT 4")]
        public double OtherSecondAngle { get; set; }

        [Name("OBSZAR 4")]
        public string OtherSecondArea { get; set; }

        [Name("DYSTANS 3 - 4")]
        public double OtherDistance { get; set; }

        [Name("RÓŻNICA DYSTANSU")]
        public double DistanceDif { get; set; }

        [Name("Różnica KĄTÓW 1 - 3")]
        public double FirstAngleDif { get; set; }

        [Name("RÓŻNICA KĄTÓW 2 - 4")]
        public double SecondAngleDif { get; set; }
    }
}
