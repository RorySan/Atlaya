module Atlaya.Adapters.Csv.Caixabank.CaixaDto

open System

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
