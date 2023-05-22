using System.Drawing;

namespace MinutiaCalcLib
{
    public class Minutia
    {
        public Guid Id { get; set; }
        public string Name { get; set; }
        public double Angle { get; set; }
        public List<Point> Points { get; set; }
    }
}
