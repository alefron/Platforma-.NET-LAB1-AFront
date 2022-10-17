using System.Xml.Serialization;

namespace PlatformaLab1
{
    public class FileOperator
    {
        private const string FileNotFoundMessage = "File not found";
        private const string DirNotFoundMessage = "Directory not found";
        private const string DeserializerFailedMessage = "Deserializer failed";

        public List<double?> DeserializeDoublesFromAllFiles(string pathToDirectory, bool deleteFiles=false)
        {
            List<string> paths = new List<string>();
            List<double?> values = new List<double?>();
            try
            {
                paths = Directory.GetFiles(pathToDirectory, "plikDaneSerializacja*.txt").ToList();
            }
            catch (DirectoryNotFoundException e)
            {
                Console.WriteLine(DirNotFoundMessage);
                return values;
            }

            paths = SortByCreationDateDESC(paths);

            paths.ForEach(path =>
            {
                try
                {
                    var reader = new StreamReader(path);
                    var filesSerializer = new XmlSerializer(typeof(TemperatureSensorSimulator));
                    var simulator = (TemperatureSensorSimulator)filesSerializer.Deserialize(reader);
                    if (simulator is not null)
                    {
                        values.AddRange(simulator.Temperatures);
                    }
                    reader.Close();
                }
                catch (InvalidOperationException e)
                {
                    Console.WriteLine(DeserializerFailedMessage);
                }
                finally
                {
                    if (deleteFiles && File.Exists(path))
                    {
                        File.Delete(path);
                    }
                }
            });

            return values;
        }

        public List<double?> GetDoublesFromFile(int valuesCount, string path, bool deleteFile=false)
        {
            var values = new List<double?>();
            try 
            {
                var reader = new StreamReader(path);
                var value = string.Empty;
                while ((value = reader.ReadLine()) != null || values.Count() == valuesCount)
                {
                    if (double.TryParse(value, out double parsedValue))
                    {
                        values.Add(parsedValue);
                    }
                }
                reader.Close();
            }
            catch (FileNotFoundException e)
            {
                Console.WriteLine(FileNotFoundMessage, e.Message);
            }
            finally
            {
                if (deleteFile && File.Exists(path))
                {
                    File.Delete(path);
                }
            }
            return values;
        }

        private List<string> SortByCreationDateDESC(List<string> paths)
        {
            paths.Sort(delegate (string path1, string path2)
            {
                var file1 = new FileInfo(path1);
                var file2 = new FileInfo(path2);
                return file2.CreationTime.CompareTo(file1.CreationTime);
            });
            return paths;
        }
    }
}
