using System.Drawing;

namespace MinutiaCalcLib
{
    public class MinutiaPair
    {
        public string UniversalId { get; set; }
        public string MoveType { get; set; }
        public string FingerprintType { get; set; }
        public string FirstId { get; set; }
        public string FirstName { get; set; }
        public Point FirstPoint { get; set; }
        public double FirstAngle { get; set; }
        public string FirstAreas { get; set; }
        public string SecondId { get; set; }
        public string SecondName { get; set; }
        public Point SecondPoint { get; set; }
        public double SecondAngle { get; set; }
        public string SecondAreas { get; set; }
        public double Distance { get; set; }
    }
}
