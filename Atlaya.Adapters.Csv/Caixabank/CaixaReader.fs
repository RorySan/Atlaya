namespace Atlaya.Adapters.Csv

open System
open Atlaya.Adapters.Csv.Caixabank.CaixaDto

module CsvReader =

    open System.IO
    open System.Globalization
    open CsvHelper
    open CsvHelper.Configuration

    let config = CsvConfiguration(CultureInfo("es-ES"), Delimiter = ",")

    let readCsv (input: string) =
        use reader = new StringReader(input)
        use csv = new CsvReader(reader, config)

        csv.GetRecords<obj>()
        |> Seq.map (fun row ->
            let dict = row :?> System.Collections.Generic.IDictionary<string, obj>

            let income = Parsers.parseDecimal dict["Ingreso (+)"]
            let expense = Parsers.parseDecimal dict["Gasto (-)"]

            { Date = DateOnly.Parse(string dict.["F. Operación"], CultureInfo("es-ES"))
              CounterParty = (string dict.["Concepto complementario 1"]).Trim()
              AccountNr = (string dict["Número de cuenta"]).Trim()
              CommonConcept = (string dict["Concepto común"]).Trim()
              InternalConcept = (string dict["Concepto propio"]).Trim()
              Reference = (string dict["Referencia 2"]).Trim()
              Detail3 = (string dict["Concepto complementario 3"]).Trim()
              Detail5 = (string dict["Concepto complementario 5"]).Trim()
              Detail7 = (string dict["Concepto complementario 7"]).Trim()
              Detail9 = (string dict["Concepto complementario 9"]).Trim()
              Amount = if income.IsSome then income.Value else -expense.Value })
        |> Seq.toList
