using System.Xml.Serialization;

namespace PlatformaLab1
{
    [Serializable, XmlRoot(ElementName = "TemperaturesSimulator")]
    public class TemperatureSensorSimulator : ISensor
    {
        [NonSerialized] private const string IncorrectReadingMessage = "incorrect reading";
        [NonSerialized] private const string GetTemperaturesCountMessage = "Enter the temperatures count: ";
        [NonSerialized] private const string InvalidParameterMessage = "Invalid parameter.";
        [NonSerialized] private const string FileSavedMessage = "data saved";
        [NonSerialized] private readonly Random randomGenerator = new Random(Guid.NewGuid().GetHashCode());

        [XmlElement(ElementName = "Temperatures")]
        public List<double?> Temperatures = new List<double?>();

        public void SetTemperatures(IList<double?> temperatures)
        {
            Temperatures = temperatures.ToList();
        }

        public IList<double?> GetTemperatures()
        {
            return Temperatures.Where(temp => temp is not null).ToList();
        }

        public void GenerateAndSetTemperatures(int resultsCount)
        {
            var temperatures = Enumerable.Repeat<double?>(null, resultsCount);
            temperatures = temperatures.Select(temp => GenerateTemperature());
            Temperatures = temperatures.ToList();
        }

        public void WriteTemperatures()
        {
            Console.WriteLine(ToString());
        }

        public void WriteTemperatures(int count)
        {
            Console.WriteLine(ConvertTemperaturesToString(count));
        }

        public void GetParameterAndWriteTemperatures()
        {
            Console.Write(GetTemperaturesCountMessage);
            var userInput = Console.ReadLine();
            if (int.TryParse(userInput, out int count))
            {
                WriteTemperatures(count);
            }
            else
            {
                Console.WriteLine(InvalidParameterMessage);
            }
        }

        public void SaveTemperaturesToFile(string fileName, bool useSerialization=false)
        {
            var timestamp = DateTime.Now.ToString(string.Format("_yyyy_MM_dd_HH_mm_ss"));
            using (var writer = new StreamWriter(fileName + timestamp + ".txt"))
            {
                if (!useSerialization)
                {
                    //using disposable
                    writer.WriteLine(ToString());
                }
                else
                {
                    //serialization
                    var temperaturesSerializator = new XmlSerializer(typeof(TemperatureSensorSimulator));
                    temperaturesSerializator.Serialize(writer, this);
                }
                Console.WriteLine(FileSavedMessage + " (" + fileName + timestamp + ".txt)");
            }
        }

        public override string ToString()
        {
            return ConvertTemperaturesToString();
        }

        private double? GenerateTemperature()
        {
            var temperature = randomGenerator.Next(-100, 101);
            return temperature < -80 ? null : temperature;
        }

        private string ConvertTemperaturesToString(int elementsCount=-1)
        {
            var resultsToConvert = elementsCount != -1 && elementsCount <= Temperatures.Count() ? elementsCount : Temperatures.Count();

            return string.Join(Environment.NewLine, Temperatures.ToList().GetRange(0, resultsToConvert)
                .Select(temp => temp is not null ? string.Format("{0:F2}", temp, Environment.NewLine) : IncorrectReadingMessage));

        }
    }
}
