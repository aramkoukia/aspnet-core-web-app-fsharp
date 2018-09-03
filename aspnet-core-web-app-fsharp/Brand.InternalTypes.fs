namespace Brand.InternalTypes

open Common.SimpleTypes
open Brand.PublicTypes
open System

type CreatedBrand = {
    BrandId : Guid
    BrandName : String50
    }

// ---------------------------
// Create events
// ---------------------------

type CreateEvents = 
    CreatedBrand                          // input
     -> CreateBrandEvent list              // output