#r @"C:\working\demo\packages\FSharp.Data\lib\net40\FSharp.Data.dll"

open FSharp.Data

type FirstClear = CsvProvider<"Path/To/SampleDataFile.csv">

// FirstClear.GetSample()
// let data = FirstClear.Load("Path/To/FullDataFile.csv")

//let firstRow = data.Rows |> seq.Head

//firstRow