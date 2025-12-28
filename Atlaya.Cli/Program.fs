open System.IO
open Atlaya.Adapters.Csv

let rawInput = File.ReadAllText("TestFiles/ene-nov.csv")

let records = CsvReader.readCsv rawInput

printfn "end"


