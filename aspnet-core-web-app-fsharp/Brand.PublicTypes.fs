namespace aspnet_core_web_app_fsharp

open System


// ------------------------------------
// error outputs 


/// All the things that can go wrong in this workflow
type ValidationError = ValidationError of string

type PricingError = PricingError of string

type ServiceInfo = {
    Name : string
    Endpoint: System.Uri
    }

type RemoteServiceError = {
    Service : ServiceInfo 
    Exception : System.Exception
    }


type UnvalidatedBrand = {
    BrandId : Guid
    BrandName : string
    }

type BrandCreated = {
    BrandId : Guid
    BrandName : string
}

type CreateBrandError =
    | Validation of ValidationError 
    | RemoteService of RemoteServiceError 

/// The possible events resulting from the CreateBrand workflow
/// Not all events will occur, depending on the logic of the workflow
type CreateBrandEvent = 
    | BrandCreated of BrandCreated


// ------------------------------------
// the workflow itself

type CreateBrand = 
    UnvalidatedBrand -> AsyncResult<CreateBrandEvent list,CreateBrandError>


