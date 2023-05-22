using MinutiaCalcLib;
using Newtonsoft.Json;
using System.Drawing;

Console.Write("Folder z plikami: ");
var src = Console.ReadLine();

string[] files = Directory.GetFiles(src, "*.txt");

foreach (string file in files)
{
    Console.Write("Plika {0} zawiera odcisk typu: ", file);
    var fingerPrintType = Console.ReadLine();

    string jsonText = File.ReadAllText(file);
    List<Minutia>? minutias = JsonConvert.DeserializeObject<List<Minutia>>(jsonText);
    List<Area>? areas = LocateAreaPoints(minutias);

    PairExtractor(minutias, areas, src, file, fingerPrintType);
}

void PairExtractor(List<Minutia> minutias, List<Area> areas, string folder, string file, string fingerprintType)
{
    List<MinutiaPair> pairs = new List<MinutiaPair>();

    foreach (var minutia in minutias)
    {
        if (minutia.Points.Count > 1) continue;

        foreach (var otherMinutia in minutias)
        {
            if (otherMinutia == minutia) continue;

            if (otherMinutia.Points.Count > 1) continue;

            var distance = CalculateDistance(minutia, otherMinutia);
            string[] firstGuid = minutia.Id.ToString().Split("-");
            string[] secondGuid = otherMinutia.Id.ToString().Split("-");

            if (DuplicateCheck(pairs, firstGuid, secondGuid) is true) continue;

            var moveType = Path.GetFileNameWithoutExtension(file).Split("_")[2];

            pairs.Add(new MinutiaPair()
            {
                UniversalId = firstGuid.First() + "-" + secondGuid.Last(),
                MoveType = moveType,
                FingerprintType = fingerprintType,
                FirstId = minutia.Id.ToString(),
                FirstName = minutia.Name,
                FirstPoint = minutia.Points[0],
                FirstAngle = minutia.Angle * (180 / Math.PI),
                FirstAreas = IsInArea(areas, minutia.Points[0]),
                SecondId = otherMinutia.Id.ToString(),
                SecondPoint = otherMinutia.Points[0],
                SecondName = otherMinutia.Name,
                SecondAngle = otherMinutia.Angle * (180 / Math.PI),
                SecondAreas = IsInArea(areas, otherMinutia.Points[0]),
                Distance = distance
            });
        }
    }

    if (pairs.Count > 0)
    {
        string json = JsonConvert.SerializeObject(pairs, Formatting.Indented);

        File.WriteAllText(folder + @"\" + "Pairs_" + Path.GetFileNameWithoutExtension(file) + ".json", json);
    }
}


//Sprawdzanie czy punkt jest w obszarze.
string IsInArea(List<Area> areas, Point point)
{
    string result = "";

    foreach (var area in areas)
    {
        if (point.X > area.FirstPoint.X &&
            point.X > area.FourthPoint.X &&
            point.X < area.SecondPoint.X &&
            point.X < area.ThirdPoint.X)
        {
            if (point.Y > area.FirstPoint.Y &&
                point.Y > area.SecondPoint.Y &&
                point.Y < area.FourthPoint.Y &&
                point.Y < area.ThirdPoint.Y)
            {
                result = area.AreaId.ToString();
            }

        }

    }
    return result;
}

//Wyliczanie obszaru.
//Bazowo, program generujący minucje, liczy Y od góry do dołu, X od lewej do prawej.
//LocateAreaPoints stara się ustawić kąty obszaru w następujący sposób: pierszwy => góra lewo, drugi => góra prawo, trzeci => dół prawo, czwarty => dół lewo
List<Area> LocateAreaPoints(List<Minutia> minutias)
{
    List<Area> result = new List<Area>();

    foreach (var minutia in minutias)
    {
        if (minutia.Points.Count > 1 && minutia.Points.Count < 3)
        {
            Point first = new Point(0, 0);
            Point second = new Point(0, 0);
            Point third = new Point(0, 0);
            Point fourth = new Point(0, 0);

            if (minutia.Points[0].X < minutia.Points[1].X && minutia.Points[0].Y < minutia.Points[1].Y)
            {
                first = minutia.Points[0];
                third = minutia.Points[1];
                second = new Point(minutia.Points[1].X, minutia.Points[0].Y);
                fourth = new Point(minutia.Points[0].X, minutia.Points[1].Y);
            }
            else if (minutia.Points[0].X < minutia.Points[1].X && minutia.Points[0].Y > minutia.Points[1].Y)
            {
                fourth = minutia.Points[0];
                second = minutia.Points[1];
                first = new Point(minutia.Points[0].X, minutia.Points[1].Y);
                third = new Point(minutia.Points[1].X, minutia.Points[0].Y);
            }
            else if (minutia.Points[0].X > minutia.Points[1].X && minutia.Points[0].Y > minutia.Points[1].Y)
            {
                third = minutia.Points[0];
                first = minutia.Points[1];
                second = new Point(minutia.Points[0].X, minutia.Points[1].Y);
                fourth = new Point(minutia.Points[1].X, minutia.Points[0].Y);
            }
            else if (minutia.Points[0].X > minutia.Points[1].X && minutia.Points[0].Y < minutia.Points[1].Y)
            {
                second = minutia.Points[0];
                fourth = minutia.Points[1];
                third = new Point(minutia.Points[0].X, minutia.Points[1].Y);
                first = new Point(minutia.Points[1].X, minutia.Points[0].Y);
            }



            result.Add(new Area
            {
                AreaId = minutia.Id,
                FirstPoint = first,
                SecondPoint = second,
                ThirdPoint = third,
                FourthPoint = fourth
            });
        }
    }
    return result;
}


//Sprawdzanie duplikatów.
bool DuplicateCheck(List<MinutiaPair> pairs, string[] firstGuid, string[] secondGuid)
{
    var probeGuid = secondGuid.First() + "-" + firstGuid.Last();

    foreach (var pair in pairs)
    {
        if (pair.UniversalId == probeGuid) return true;
    }

    return false;
}

//Obliczanie dystansu między dwoma punktami.
double CalculateDistance(Minutia first, Minutia second)
{
    var x1 = first.Points[0].X;
    var y1 = first.Points[0].Y;
    var x2 = second.Points[0].X;
    var y2 = second.Points[0].Y;

    return Math.Abs(Math.Sqrt(Math.Pow(x2 - x1, 2) + Math.Pow(y2 - y1, 2)));
}

