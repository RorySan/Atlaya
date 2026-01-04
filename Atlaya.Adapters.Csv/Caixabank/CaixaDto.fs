module Atlaya.Adapters.Csv.Caixabank.CaixaDto

open System
open Atlaya.Domain

type CaixaDto =
    { Date: DateOnly
      CounterParty: string
      AccountNr: string
      CommonConcept: string
      InternalConcept : string
      Reference: string
      Detail3: string
      Detail5: string
      Detail7: string
      Detail9: string
      Amount: decimal }
    
let toTransaction (caixaDto : CaixaDto) =
    
    { Id = TransactionId "adsfasdf"
      Date = caixaDto.Date
      Source = Account (AccountName caixaDto.AccountNr)
      CounterParty = CounterParty caixaDto.CounterParty
      Description = Description (caixaDto.Detail3 + caixaDto.Detail5)
      Currency = EUR
      Amount = caixaDto.Amount }
