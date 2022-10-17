namespace PlatformaLab1
{
    public interface ISensor
    {
        void SetTemperatures(IList<double?> temperatures);
        IList<double?> GetTemperatures();
        void GenerateAndSetTemperatures(int resultsCount);
        void WriteTemperatures();
        void WriteTemperatures(int count);
        void GetParameterAndWriteTemperatures();
        void SaveTemperaturesToFile(string fileName, bool useSerialization = false);
    }
}
