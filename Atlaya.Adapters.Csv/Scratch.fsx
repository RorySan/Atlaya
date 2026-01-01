

#r "nuget: CsvHelper"

open System.IO
open System.Globalization
open CsvHelper
open CsvHelper.Configuration
open System

let parseDecimal (v: obj) =
    let s = string v
    if String.IsNullOrWhiteSpace(s) then
        None
    else
        try
            let parsed = Decimal.Parse(s)
            Some parsed
        with
            None

let asdf = parseDecimal "fasdf"