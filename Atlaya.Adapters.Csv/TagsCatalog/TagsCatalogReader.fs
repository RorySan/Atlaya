module Atlaya.Adapters.Csv.TagsCatalog.TagsCatalogReader

open System
open System.IO
open Atlaya.Domain.Tags

type TagCsvError =
    | FileNotFound of string
    | InvalidHeader of string list
    | InvalidRow of lineNumber:int * content:string
    | DuplicateTagId of TagId

let private expectedHeader = [ "tag_id"; "name" ]

let loadTagCatalog (path: string) : Result<TagCatalog, TagCsvError> =
    if not (File.Exists path) then
        Error (FileNotFound path)
    else
        let lines = File.ReadAllLines path |> Array.toList

        match lines with
        | [] ->
            Ok { TagsById = Map.empty }

        | header :: rows ->
            let headerColumns =
                header.Split(',', StringSplitOptions.TrimEntries)
                |> Array.toList

            if headerColumns <> expectedHeader then
                Error (InvalidHeader headerColumns)
            else
                let parseRow (lineNo: int) (line: string) =
                    let cols =
                        line.Split(',', StringSplitOptions.TrimEntries)
                        |> Array.toList

                    match cols with
                    | [ tagId; name ] when tagId <> "" && name <> "" ->
                        Ok (TagId tagId, { Id = TagId tagId; Name = name })

                    | _ ->
                        Error (InvalidRow (lineNo, line))

                let folder (acc: Result<Map<TagId, Tag>, TagCsvError>) (lineNo, line) =
                    acc
                    |> Result.bind (fun map ->
                        parseRow lineNo line
                        |> Result.bind (fun (id, tag) ->
                            if map |> Map.containsKey id then
                                Error (DuplicateTagId id)
                            else
                                Ok (map |> Map.add id tag)
                        )
                    )

                rows
                |> List.mapi (fun i line -> i + 2, line) // line numbers (header is 1)
                |> List.fold folder (Ok Map.empty)
                |> Result.map (fun tagsById ->
                    { TagsById = tagsById }
                )
