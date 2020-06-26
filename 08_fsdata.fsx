#r @"C:\working\demo\packages\FSharp.Data\lib\net40\FSharp.Data.dll"

open FSharp.Data

type FirstClear = CsvProvider<"C:\\working\\demo\\data\\SampleDataFile.csv">

FirstClear.GetSample()

let data = FirstClear.Load("C:\\working\\demo\\data\\SomeDataFile.csv")

let firstRow = data.Rows

