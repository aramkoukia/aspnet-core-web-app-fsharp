namespace Brand

open System
open Brand.PublicTypes
open System.Collections.Generic

// ==================================
// DTOs for CreateBrand workflow
// ==================================

[<AutoOpen>]
module internal Utils =     

    /// Helper function to get the value from an Option, and if None, use the defaultValue 
    /// Note that the defaultValue is the first parameter, unlike the similar `defaultArg`
    let defaultIfNone defaultValue opt =
        match opt with
        | Some v -> v
        | None -> defaultValue
        // could also use
        // defaultArg opt defaultValue

type BrandDto = {
    Id: Guid
    Name: string
}

module internal BrandDto =

    /// Convert the Brand into a UnvalidatedBrand
    /// This always succeeds because there is no validation. 
    let toUnvalidatedBrand (dto:BrandDto) :UnvalidatedBrand = 
        {
        BrandId = dto.Id
        BrandName = dto.Name 
        }
  
    /// Convert a ShippableOrderPlaced object into the corresponding DTO.
    /// Used when exporting from the domain to the outside world.
    let fromDomain (domainObj:BrandCreated) :BrandDto = 
        {
        Id = domainObj.BrandId
        Name = domainObj.BrandName
        }


/// Use a dictionary representation of a PlaceOrderEvent, suitable for JSON
/// See "Serializing Records and Choice Types Using Maps" in chapter 11
type CreateBrandEventDto = IDictionary<string,obj> 

module internal CreateBrandEventDto = 

    /// Convert a PlaceOrderEvent into the corresponding DTO.
    /// Used when exporting from the domain to the outside world.
    let fromDomain (domainObj:CreateBrandEvent) :CreateBrandEventDto = 
        match domainObj with
        | BrandCreated brandCreated ->
            let obj = brandCreated |> BrandDto.fromDomain |> box // use "box" to cast into an object
            let key = "BrandCreated"
            [(key,obj)] |> dict

//===============================================
// DTO for PlaceOrderError
//===============================================

type CreateBrandErrorDto = {
    Code : string
    Message : string
    }

module internal CreateBrandErrorDto = 

    let fromDomain (domainObj:CreateBrandError ) :CreateBrandErrorDto = 
        match domainObj with
        | Validation validationError ->
            let (ValidationError msg) = validationError 
            {
                Code = "ValidationError"
                Message = msg
            }
        | RemoteService remoteServiceError ->
            let msg = sprintf "%s: %s" remoteServiceError.Service.Name remoteServiceError.Exception.Message
            {
                Code = "RemoteServiceError"
                Message = msg
            }





