module internal Brand.Implementation

open Brand.PublicTypes
open Brand.InternalTypes
open Common.SimpleTypes

let createBrandEvent (createdBrand:CreatedBrand) : BrandCreated =
    {
    BrandId = createdBrand.BrandId 
    BrandName = createdBrand.BrandName |> String50.value
    } 

/// helper to convert an Option into a List
let listOfOption opt =
    match opt with 
    | Some x -> [x]
    | None -> []

let createEvents : CreateEvents = 
    fun brandCreated ->
        let brandCreateEvents = 
            brandCreated
            |> createBrandEvent 
            |> Option.map CreateBrandEvent.BrandCreated
            |> listOfOption

        // return all the events
        [
        yield! brandCreateEvents
        ]            

// ---------------------------
// overall workflow
// ---------------------------

let createBrand
    validateBrand  // dependency
    checkBrandExists // dependency
    : CreateBrand =       // definition of function

    fun unvalidatedBrand -> 
        asyncResult {
            let! validatedBrand = 
                validateBrand checkBrandExists unvalidatedBrand 
                |> AsyncResult.mapError CreateBrandError.Validation
            let events = 
                createEvents validatedBrand 
            return events
        }
