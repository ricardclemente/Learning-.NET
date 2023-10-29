using System.IO;
using System.Collections.Generic;
using Newtonsoft.Json;
using System;

var currentDirectory=Directory.GetCurrentDirectory();
var storesDirectory=Path.Combine(currentDirectory,"stores");

var salesTotalDir = Path.Combine(currentDirectory, "salesTotalDir");
Directory.CreateDirectory(salesTotalDir); 



var salesJson = File.ReadAllText($"stores{Path.DirectorySeparatorChar}201{Path.DirectorySeparatorChar}sales.json");
var salesData = JsonConvert.DeserializeObject<SalesTotal>(salesJson);
//Console.WriteLine(salesData.Total);

var salesFiles=FindFiles(storesDirectory);
var total = CalculateSalesTotal(salesFiles);
Console.WriteLine(total);
//File.WriteAllText(Path.Combine(salesTotalDir, "totals.txt"), salesData.Total.ToString() + Environment.NewLine);
//File.AppendAllText(Path.Combine(salesTotalDir, "totals.txt"), salesData.Total.ToString()+ Environment.NewLine);
File.WriteAllText(Path.Combine(salesTotalDir, "totals.txt"), total.ToString() + Environment.NewLine);

IEnumerable<string> FindFiles(string folderName){
    List<string> salesFiles = new List<string>();

    var foundFiles = Directory.EnumerateFiles(folderName,"*",SearchOption.AllDirectories);

    foreach (var file in foundFiles){
        var extension = Path.GetExtension(file);
        if (extension==".json")//(file.EndsWith("sales.json"))
        {
            salesFiles.Add(file);
        }
    }

    return salesFiles;

}

double CalculateSalesTotal(IEnumerable<string> salesFiles){
    double salesTotal=0;

    foreach (string file in salesFiles){
        var salesJson = File.ReadAllText(file);
        SalesData? data = JsonConvert.DeserializeObject<SalesData>(salesJson);
        salesTotal+=data?.Total ?? 0;
    }
    return salesTotal;
}
record SalesData ( double Total);
class SalesTotal{
    public double Total {get; set;}
}
