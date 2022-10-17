namespace PlatformaLab1
{
    public class SensorStatsGenerator
    {
        private readonly ISensor temperatureSensor;
        public SensorStatsGenerator(ISensor temperatureSensor, int temperaturesSamplesCount)
        {
            this.temperatureSensor = temperatureSensor;
            this.temperatureSensor.GenerateAndSetTemperatures(temperaturesSamplesCount);
        }

        public double? GetMeanValue()
        {
            var temperatures = temperatureSensor.GetTemperatures();
            var s = temperatures.Sum() / temperatures.Count();
            return temperatures.Sum() / temperatures.Count();
        }

        public double? GetStandardDeviation()
        {
            var temperatures = temperatureSensor.GetTemperatures();
            var mean = GetMeanValue();
            var partialSum = temperatures.Sum(temp => (temp - mean) * (temp - mean));
            return Math.Sqrt((partialSum/temperatures.Count()).Value);
        }

        public IList<double?> SortTemperatures(bool isAscending=true)
        {
            var sorted = isAscending ?
                temperatureSensor.GetTemperatures().OrderBy(temp => temp).ToList()
                : temperatureSensor.GetTemperatures().OrderByDescending(temp => temp).ToList();
            temperatureSensor.SetTemperatures(sorted);
            return sorted;
        }

        public void SortAndSaveTemperatures(string path, bool isAscending = true)
        {
            var sorted = SortTemperatures(isAscending);
            temperatureSensor.SetTemperatures(sorted);
            temperatureSensor.SaveTemperaturesToFile(path);
        }

        public IList<double?> DeleteTemperaturesInRange(double min, double max)
        {
            var temperatures = temperatureSensor.GetTemperatures();
            var updated = temperatures.Where(temp => temp >= min && temp <= max).ToList();
            temperatureSensor.SetTemperatures(updated);
            return updated;
        }

        public void DeleteTemperaturesInRangeAndSave(double min, double max, string path)
        {
            var updatedTemperatures = DeleteTemperaturesInRange(min, max);
            temperatureSensor.SetTemperatures(updatedTemperatures);
            temperatureSensor.SaveTemperaturesToFile(path);
        }
    }
}
