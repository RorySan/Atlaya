module Atlaya.Domain.Tags

// ---------------------------
// IDs
// ---------------------------
type TagId = TagId of string

// ---------------------------
// Tag entity (optional fields kept minimal)
// ---------------------------
type Tag =
    { Id: TagId
      Name: string }

// ---------------------------
// Catalog (single source of truth)
// ---------------------------
type TagCatalog =
    { TagsById: Map<TagId, Tag> }

// ---------------------------
// Domain value stored on transactions
// Private so tags must be validated against catalog.
// ---------------------------
type TagSet =
    private
        { TagIds: Set<TagId> }

// ---------------------------
// Errors
// ---------------------------
type TagError =
    | UnknownTagId of TagId

// ---------------------------
// Construction
// ---------------------------
let tryCreate (catalog: TagCatalog) (tagIds: seq<TagId>) : Result<TagSet, TagError> =
    let set = tagIds |> Set.ofSeq

    match set |> Seq.tryFind (fun id -> catalog.TagsById |> Map.containsKey id |> not) with
    | Some bad -> Error (UnknownTagId bad)
    | None -> Ok { TagIds = set }

// ---------------------------
// Accessors / helpers
// ---------------------------
let ids (tags: TagSet) : Set<TagId> =
    tags.TagIds

let contains (id: TagId) (tags: TagSet) : bool =
    tags.TagIds |> Set.contains id

let add (catalog: TagCatalog) (id: TagId) (tags: TagSet) : Result<TagSet, TagError> =
    tryCreate catalog (Seq.append tags.TagIds [ id ])

let remove (id: TagId) (tags: TagSet) : TagSet =
    { TagIds = tags.TagIds |> Set.remove id }

let toTags (catalog: TagCatalog) (tags: TagSet) : Tag list =
    tags.TagIds
    |> Seq.choose (fun id -> catalog.TagsById |> Map.tryFind id)
    |> Seq.toList
