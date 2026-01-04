open System.IO
open Atlaya.Adapters.Csv
open Atlaya.Adapters.Csv.Catalog
open Atlaya.Adapters.Csv.TagsCatalog.TagsCatalogReader
open Atlaya.Domain
open Atlaya.Domain.Categorization
open Atlaya.Domain.Classification
open Atlaya.Domain.Tags

let catalog = CatalogReader.loadCatalog "TestFiles/Categories.csv" "TestFiles/Subcategories.csv"

// let tagsRaw = File.ReadAllText("TestFiles/Tags.csv")
let tagCat = loadTagCatalog "TestFiles/Tags.csv" 

printfn "Loaded catalog with %d categories and %d subcategories" catalog.CategoriesById.Count catalog.SubcategoriesById.Count

let rawInput = File.ReadAllText("TestFiles/ene-nov.csv")
let records = CsvReader.readCsv rawInput


let cat = Categorization.tryCreate catalog (SubcategoryId "mortgage")
let tags = tryCreate

let category =
    match cat with
    | Ok c -> c
    | Error e -> failwith "wtf"

let tagCatValue =
    match tagCat with
    | Ok c -> c
    | Error e -> failwith "asdf"
let tag = Tags.tryCreate tagCatValue [TagId "refund"]
let tagg =
    match tag with
    | Ok t -> t
    | Error e -> failwith "taggme"
let fdas = Caixabank.CaixaDto.toTransaction records[0]
let adsf = categorize fdas category tagg Income
printfn "Read %d transactions" records.Length
printfn "end"


