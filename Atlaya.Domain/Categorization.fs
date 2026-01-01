module Atlaya.Domain.Categorization

// ---------------------------
// IDs
// ---------------------------
type CategoryId = CategoryId of string
type SubcategoryId = SubcategoryId of string

// ---------------------------
// Catalog entities
// ---------------------------
type Category =
    { Id: CategoryId
      Name: string }

type Subcategory =
    { Id: SubcategoryId
      CategoryId: CategoryId
      Name: string }

type Catalog =
    { CategoriesById: Map<CategoryId, Category>
      SubcategoriesById: Map<SubcategoryId, Subcategory>
      // Treat this as a derived index (built from SubcategoriesById), not a second source of truth.
      SubcategoriesByCategory: Map<CategoryId, SubcategoryId list> }

type Categorization =
    private
        { SubcategoryId: SubcategoryId }

type CategorizationError =
    | UnknownSubcategoryId of SubcategoryId
    | CatalogInvariantBroken_SubcategoryPointsToUnknownCategory of subId: SubcategoryId * catId: CategoryId

let tryCreate (catalog: Catalog) (subId: SubcategoryId) : Result<Categorization, CategorizationError> =
    match catalog.SubcategoriesById |> Map.tryFind subId with
    | None ->
        Error (UnknownSubcategoryId subId)
    | Some sub ->
        match catalog.CategoriesById |> Map.tryFind sub.CategoryId with
        | None ->
            Error (CatalogInvariantBroken_SubcategoryPointsToUnknownCategory (subId, sub.CategoryId))
        | Some _ ->
            Ok { SubcategoryId = subId }

let subcategoryId (c: Categorization) = c.SubcategoryId

let tryGetCategoryId (catalog: Catalog) (c: Categorization) : Result<CategoryId, CategorizationError> =
    match catalog.SubcategoriesById |> Map.tryFind c.SubcategoryId with
    | None ->
        Error (UnknownSubcategoryId c.SubcategoryId)
    | Some sub ->
        match catalog.CategoriesById |> Map.tryFind sub.CategoryId with
        | None ->
            Error (CatalogInvariantBroken_SubcategoryPointsToUnknownCategory (c.SubcategoryId, sub.CategoryId))
        | Some _ ->
            Ok sub.CategoryId

let tryGetSubcategory (catalog: Catalog) (c: Categorization) : Result<Subcategory, CategorizationError> =
    match catalog.SubcategoriesById |> Map.tryFind c.SubcategoryId with
    | None -> Error (UnknownSubcategoryId c.SubcategoryId)
    | Some sub -> Ok sub

let subcategoryIdsForCategory (catalog: Catalog) (catId: CategoryId) : SubcategoryId list =
    catalog.SubcategoriesByCategory
    |> Map.tryFind catId
    |> Option.defaultValue []

let buildSubcategoriesByCategory (subcategoriesById: Map<SubcategoryId, Subcategory>) : Map<CategoryId, SubcategoryId list> =
    subcategoriesById
    |> Map.toList
    |> List.groupBy (fun (_, sub) -> sub.CategoryId)
    |> List.map (fun (catId, items) -> catId, (items |> List.map fst))
    |> Map.ofList
