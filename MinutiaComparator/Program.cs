using CsvHelper;
using CsvHelper.Configuration;
using MinutiaCalcLib;
using Newtonsoft.Json;
using System.Globalization;
using System.Text;

Console.Write("Folder z plikami: ");
var src = Console.ReadLine();

string[] files = Directory.GetFiles(src, "*.json");

if (files.Length == 0) Console.WriteLine("Nie można znaleźć plików json");

List<ComparedPair> allOfPerson = new List<ComparedPair>();
string personNumber = null;

for (int i = 0; i < files.Length; i++)
{
    string[] fileName = files[i].Split('_');

    if (personNumber == null) personNumber = fileName[1];

    for (int j = i + 1; j < files.Length; j++)
    {
        if (files[i].ToString() == files[j].ToString()) continue;

        string[] otherFileName = files[j].Split("_");

        if (otherFileName[1] != fileName[1]) continue;

        string jsonString = File.ReadAllText(files[i]);
        string otherJsonString = File.ReadAllText(files[j]);

        List<MinutiaPair> pairs = JsonConvert.DeserializeObject<List<MinutiaPair>>(jsonString);
        List<MinutiaPair> otherPairs = JsonConvert.DeserializeObject<List<MinutiaPair>>(otherJsonString);

        var comparedPairs = PairComparator(pairs, otherPairs);

        foreach (var pair in comparedPairs)
        {
            if (allOfPerson is not null && allOfPerson.Contains(pair)) continue;
            allOfPerson.Add(pair);
        }

        ExportPairsToCsv(comparedPairs, files[i], files[j], src);
    }

    if (personNumber != fileName[1] || files[i] == files.Last())
    {
        ExportAllToCsv(allOfPerson, src, personNumber);
        personNumber = fileName[1];
        allOfPerson = new List<ComparedPair>();
    }
}



void ExportAllToCsv(List<ComparedPair> allPairs, string path, string personNumber)
{

    var newName = path + @"\" + personNumber + ".csv";

    var config = new CsvConfiguration(CultureInfo.CurrentCulture)
    {
        Delimiter = "|",
        Encoding = Encoding.UTF8
    };

    using (var writer = new StreamWriter(newName))
    using (var csv = new CsvWriter(writer, config))
    {
        csv.WriteRecords(allPairs.OrderBy(x => x.DistanceDif));
    }
}

List<ComparedPair> PairComparator(List<MinutiaPair> first, List<MinutiaPair> second)
{
    List<ComparedPair> comparedPairs = new List<ComparedPair>();

    foreach (var basePair in first)
    {
        foreach (var otherPair in second.Where(x => x.UniversalId == basePair.UniversalId))
        {
            var distanceDif = Math.Abs(otherPair.Distance - basePair.Distance);
            var firstAngleDif = CalculateAngleDif(otherPair.FirstAngle, basePair.FirstAngle);
            var secondAngleDif = CalculateAngleDif(otherPair.SecondAngle, basePair.SecondAngle);

            comparedPairs.Add(new ComparedPair()
            {
                UniversalId = otherPair.UniversalId,
                BaseId = basePair.FirstId,
                BaseMoveType = basePair.MoveType,
                BaseFingerprintType = basePair.FingerprintType,
                BaseFirstPoint = basePair.FirstPoint,
                BaseFirstAngle = basePair.FirstAngle,
                BaseFirstArea = basePair.FirstAreas,
                BaseSecondPoint = basePair.SecondPoint,
                BaseSecondAngle = basePair.SecondAngle,
                BaseSecondArea = basePair.SecondAreas,
                BaseDistance = basePair.Distance,
                OtherId = otherPair.SecondId,
                OtherMoveType = otherPair.MoveType,
                OtherFingerprintType = otherPair.FingerprintType,
                OtherFirstPoint = otherPair.FirstPoint,
                OtherFirstAngle = otherPair.FirstAngle,
                OtherFirstArea = otherPair.FirstAreas,
                OtherSecondPoint = otherPair.SecondPoint,
                OtherSecondAngle = otherPair.SecondAngle,
                OtherSecondArea = otherPair.SecondAreas,
                OtherDistance = otherPair.Distance,
                DistanceDif = distanceDif,
                FirstAngleDif = firstAngleDif,
                SecondAngleDif = secondAngleDif
            });
        }
    }
    return comparedPairs;
}

double CalculateAngleDif(double first, double second)
{
    var dif = second - first;
    if (dif < -180) dif += 360;
    if (dif > 180) dif -= 360;

    return Math.Abs(dif);
}

void ExportPairsToCsv(List<ComparedPair> comparedPairs, string firstPair, string secondPair, string path)
{
    string[] file = Path.GetFileNameWithoutExtension(firstPair).ToString().Split("_");
    string[] otherFile = Path.GetFileNameWithoutExtension(secondPair).ToString().Split("_");

    var checkName = path + @"\" + file[1] + "_" + file[2] + "_" + otherFile.Last() + "_" + file.Last() + ".csv";

    if (File.Exists(checkName)) return;

    var newName = path + @"\" + file[1] + "_" + file[2] + "_" + file.Last() + "_" + otherFile.Last() + ".csv";

    var config = new CsvConfiguration(CultureInfo.CurrentCulture)
    {
        Delimiter = "|",
        Encoding = Encoding.UTF8
    };

    using (var writer = new StreamWriter(newName))
    using (var csv = new CsvWriter(writer, config))
    {
        csv.WriteRecords(comparedPairs.OrderBy(x => x.DistanceDif));
    }
}


