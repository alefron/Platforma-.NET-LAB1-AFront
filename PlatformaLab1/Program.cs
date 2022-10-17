// See https://aka.ms/new-console-template for more information
using PlatformaLab1;

var simulator = new TemperatureSensorSimulator();
simulator.GenerateAndSetTemperatures(10);
simulator.GetParameterAndWriteTemperatures();

simulator.SaveTemperaturesToFile("plikDaneSerializacja", true);
simulator.SaveTemperaturesToFile("plikDane");

var fileOperator = new FileOperator();
IList<double?> values = fileOperator.GetDoublesFromFile(5, "plikDane_2022_10_18_00_25_12.txt", true);
simulator.SetTemperatures(values);
simulator.WriteTemperatures();


var values2 = fileOperator.DeserializeDoublesFromAllFiles(Environment.CurrentDirectory);
simulator.SetTemperatures(values2);
simulator.WriteTemperatures();


var statsGenerator = new SensorStatsGenerator(new TemperatureSensorSimulator(), 30);
Console.WriteLine("mean value: {0}", statsGenerator.GetMeanValue().ToString());
Console.WriteLine("std dev value: {0}", statsGenerator.GetStandardDeviation().ToString());
simulator.SetTemperatures(statsGenerator.SortTemperatures());
simulator.WriteTemperatures();
simulator.SetTemperatures(statsGenerator.SortTemperatures(false));
simulator.WriteTemperatures();
statsGenerator.SortAndSaveTemperatures("sortedASC.txt");
simulator.SetTemperatures(statsGenerator.DeleteTemperaturesInRange(-3, 50));
simulator.WriteTemperatures();
statsGenerator.DeleteTemperaturesInRangeAndSave(4, 40, "delete.txt");



