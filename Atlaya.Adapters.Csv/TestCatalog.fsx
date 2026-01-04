#r "bin/Debug/net10.0/Atlaya.Domain.dll"
#r "bin/Debug/net10.0/Atlaya.Adapters.Csv.dll"

open System
open Atlaya.Adapters.Csv.Catalog

let categoriesPath = "../Atlaya.Cli/TestFiles/Categories.csv"
let subcategoriesPath = "../Atlaya.Cli/TestFiles/Subcategories.csv"

try
    let catalog = CatalogReader.loadCatalog categoriesPath subcategoriesPath
    
    printfn "Successfully loaded catalog."
    printfn "Categories: %d" catalog.CategoriesById.Count
    printfn "Subcategories: %d" catalog.SubcategoriesById.Count
    
    catalog.CategoriesById 
    |> Map.iter (fun id cat -> printfn "Category: %A -> %s" id cat.Name)
    
    catalog.SubcategoriesById
    |> Map.iter (fun id sub -> printfn "Subcategory: %A (Category: %A) -> %s" id sub.CategoryId sub.Name)

with
| ex -> printfn "Failed to load catalog: %s" ex.Message
