namespace Atlaya.Domain

open Atlaya.Domain.Categorization
open Atlaya.Domain.Tags
open Categorization
open System

type AccountName = AccountName of string
type CardName = CardName of string
type InvestmentName = InvestmentName of string
type RealEstateName = RealEstateName of string
type TransactionId = TransactionId of string

type Source =
    | Account of AccountName
    | Card of CardName
    | Cash

type CounterParty = CounterParty of string
type Description = Description of string

type Currency =
    | EUR
    | USD

type TransactionType =
    | Income
    | Expense
    | Transfer


// usa saldo para hashear, así dos compras iguales nunca coinciden
type Transaction =
    { Id: TransactionId
      Date: DateOnly
      Source: Source
      CounterParty: CounterParty
      Description: Description
      Currency: Currency
      Amount: decimal }

type ClassifiedTransaction =
    { Transaction: Transaction
      Categorization: Categorization
      Type: TransactionType
      Tags: TagSet }

module Classification =
    let categorize
        (transaction: Transaction)
        (categorization: Categorization)
        (tags: TagSet)
        (transactionType: TransactionType)
        =

        { Transaction = transaction
          Categorization = categorization
          Tags = tags
          Type = transactionType }
