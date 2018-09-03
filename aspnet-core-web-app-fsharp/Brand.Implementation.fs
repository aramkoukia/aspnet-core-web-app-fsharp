module internal Brand.Implementation

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
                createEvents createdBrand acknowledgementOption 
            return events
        }
