namespace Atlaya.Adapters.Csv.Catalog

open System.IO
open System.Globalization
open CsvHelper
open CsvHelper.Configuration
open Atlaya.Domain.Categorization

type CategoryDto =
    { category_id: string
      name: string }

type SubcategoryDto =
    { subcategory_id: string
      category_id: string
      name: string }

module CatalogReader =

    let private config = CsvConfiguration(CultureInfo.InvariantCulture, Delimiter = ",")

    let readCategories (input: string) =
        use reader = new StringReader(input)
        use csv = new CsvReader(reader, config)
        csv.GetRecords<CategoryDto>() |> Seq.toList

    let readSubcategories (input: string) =
        use reader = new StringReader(input)
        use csv = new CsvReader(reader, config)
        csv.GetRecords<SubcategoryDto>() |> Seq.toList

    let buildCatalog (categories: CategoryDto list) (subcategories: SubcategoryDto list) : Catalog =
        let categoriesById =
            categories
            |> List.map (fun c ->
                let id = CategoryId c.category_id
                id, { Id = id; Name = c.name })
            |> Map.ofList

        let subcategoriesById =
            subcategories
            |> List.map (fun s ->
                let id = SubcategoryId s.subcategory_id
                id, { Id = id; CategoryId = CategoryId s.category_id; Name = s.name })
            |> Map.ofList

        { CategoriesById = categoriesById
          SubcategoriesById = subcategoriesById
          SubcategoriesByCategory = buildSubcategoriesByCategory subcategoriesById }

    let loadCatalog (categoryPath: string) (subcategoryPath: string) : Catalog =
        let getRaw path = File.ReadAllText(path)
        let rawCats = getRaw categoryPath
        let rawSubs = getRaw subcategoryPath
        let categories = readCategories rawCats
        let subcategories = readSubcategories rawSubs
        buildCatalog categories subcategories


