module Atlaya.Adapters.Csv.Parsers

open System

let parseDecimal (v: obj) =
    let s = string v
    if String.IsNullOrWhiteSpace(s) then
        None
    else
        Some (Decimal.Parse(s))
