

#r "nuget: CsvHelper"

open System.IO
open System.Globalization
open CsvHelper
open CsvHelper.Configuration

let path = Path.Combine(__SOURCE_DIRECTORY__, "Short.csv")
let rawInput = File.ReadAllText(path)

let config = CsvConfiguration(CultureInfo.InvariantCulture, Delimiter = ";")
let readCsv (input: string) =
    use reader = new StringReader(input)
    use csv = new CsvReader(reader, config)
    csv.GetRecords<obj>() |> Seq.toList

let records = readCsv rawInput
records |> List.iter (printfn "%A")