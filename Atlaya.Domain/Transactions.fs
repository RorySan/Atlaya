namespace Atlaya.Domain

open System

type Transaction = {
    Date: DateOnly
    Entity: string
    Info1: string
    Info2: string
    Info3: string
    Amount : decimal
    Category : string      
}