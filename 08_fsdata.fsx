#r @"nuget: FSharp.Data"

open FSharp.Data

type FirstClear = CsvProvider<"C:\\working\\demo\\data\\SampleDataFile.csv">

FirstClear.GetSample()

let data = FirstClear.Load("C:\\working\\demo\\data\\SomeDataFile.csv")

let firstRow = data.Rows

