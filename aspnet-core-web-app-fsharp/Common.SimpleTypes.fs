namespace Common.SimpleTypes

open System

/// An Id for Orders. Constrained to be a non-empty Guid
type BrandId = private BrandId of Guid

/// Constrained to be 50 chars or less, not null
type String50 = private String50 of string
