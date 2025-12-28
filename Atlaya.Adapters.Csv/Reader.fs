namespace Atlaya.Adapters.Csv

open System

module CsvReader =

    open System.IO
    open System.Globalization
    open CsvHelper
    open CsvHelper.Configuration
    open Atlaya.Domain


    let config = CsvConfiguration(CultureInfo("es-ES"), Delimiter = ",")

    type Foo = { Name: string; Concept: string }

    let readCsv (input: string) =
        use reader = new StringReader(input)
        use csv = new CsvReader(reader, config)

        csv.GetRecords<obj>()
        |> Seq.map (fun row ->
            let dict = row :?> System.Collections.Generic.IDictionary<string, obj>

            let parseDecimal (v: obj) =
                let s = string v

                if String.IsNullOrWhiteSpace(s) then
                    0m
                else
                    Decimal.Parse(s, CultureInfo("es-ES"))

            let income = parseDecimal dict.["Ingreso (+)"]
            let expense = parseDecimal dict.["Gasto (-)"]

            { Date = DateOnly.Parse(string dict.["F. Operación"], CultureInfo("es-ES"))
              Entity = (string dict.["Concepto complementario 1"]).Trim()
              Info1 = (string dict["Concepto complementario 5"]).Trim()
              Info2 = (string dict["Concepto complementario 7"]).Trim()
              Info3 = (string dict["Concepto complementario 9"]).Trim()
              Amount = if income <> 0m then income else -expense
              Category = string dict.["Concepto propio"] })
        |> Seq.toList
