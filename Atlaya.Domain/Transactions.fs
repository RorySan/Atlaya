namespace Atlaya.Domain

open Categorization
open System
open System.Collections.Generic
open System.Linq
open System.Runtime.InteropServices

type AccountName = AccountName of string
type CardName = CardName of string
type InvestmentName = InvestmentName of string
type RealEstateName = RealEstateName of string

type TransactionId = TransactionId of string

type Source =
    | Account of AccountName
    | Card of CardName
    | Investment of InvestmentName
    | RealEstate of RealEstateName
    | Cash

type CounterParty = CounterParty of string
type Description = Description of string

type TransactionType =
    | Income
    | Expense
    | Transfer

type Currency =
    | EUR
    | USD

type Tag = Tag of string
    
// usa saldo para hashear, así dos compras iguales nunca coinciden
type Transaction =
    { Id: TransactionId
      Date: DateOnly
      Type: TransactionType
      Source: Source
      CounterParty: CounterParty
      Description: Description
      Category : Category
      Subcategory : Subcategory
      Tags: Tag List
      Currency: Currency
      Amount: decimal }
