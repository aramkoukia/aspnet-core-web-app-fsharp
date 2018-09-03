module Product.Brands.Api

// namespace aspnet_core_web_app_fsharp.Controllers

open Microsoft.AspNetCore.Mvc
open Brand
open Newtonsoft.Json


type JsonString = string

/// Very simplified version!
type HttpResponse = {
    HttpStatusCode : int
    Body : JsonString 
    }

// -------------------------------
// workflow
// -------------------------------

/// This function converts the workflow output into a HttpResponse
let workflowResultToHttpReponse result = 
    match result with
    | Ok events ->
        // turn domain events into dtos
        let dtos = 
            events 
            |> List.map CreateBrandEventDto.fromDomain
            |> List.toArray // arrays are json friendly
        // and serialize to JSON
        let json = JsonConvert.SerializeObject(dtos)
        let response = 
            {
            HttpStatusCode = 200
            Body = json
            }
        response
    | Error err -> 
        // turn domain errors into a dto
        let dto = err |> CreateBrandErrorDto.fromDomain
        // and serialize to JSON
        let json = JsonConvert.SerializeObject(dto )
        let response = 
            {
            HttpStatusCode = 401
            Body = json
            }
        response


[<Route("api/[controller]")>]
type BrandsController () =
    inherit Controller()

    [<HttpPost>]
    member this.Post([<FromBody>]brand: BrandDto) = (
        let unvalidatedBrand = brand |> BrandDto.toUnvalidatedBrand
        let workflow = 
            Implementation.createBrand 
                checkBrandExists // dependency
                validateProperties // dependency

        // now we are in the pure domain
        let asyncResult = workflow unvalidatedBrand

        // now convert from the pure domain back to a HttpResponse
        asyncResult 
        |> Async.map (workflowResultToHttpReponse)
    )
        
    [<HttpPut("{id}")>]
    member this.Put(id:int, [<FromBody>]value:string ) =
        ()

    [<HttpDelete("{id}")>]
    member this.Delete(id:int) =
        ()
