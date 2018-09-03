namespace Brand.InternalTypes

open Common.SimpleTypes
open Brand.PublicTypes

type CreatedBrand = {
    BrandId : BrandId
    BrandName : String50
    }

// ---------------------------
// Create events
// ---------------------------

type CreateEvents = 
    CreatedBrand                          // input
     -> CreateBrandEvent list              // output