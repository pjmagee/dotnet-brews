namespace Brew.Features.FSharp.Operations

type Calculator(initialValue: option<double>) =
    let value = 
        match initialValue with
        | Some(v) -> v
        | None -> 0
    
    let mutable currentValue = value
    
    member this.CurrentValue
        with get() = currentValue
        and set(value) = currentValue <- value
        
    member this.Add(value: double) =
        currentValue <- currentValue + value
        this

    member this.Subtract(value: double) =
        currentValue <- currentValue - value
        this

    member this.Multiply(value: double) =
        currentValue <- currentValue * value
        this
        
    member this.Divide(value: double) =        
        if value = 0.0 then failwith "Cannot divide by zero"        
        currentValue <- currentValue / value
        this
        
    member this.Clear() =
        currentValue <- 0.0
        this
        
    override this.ToString() = currentValue.ToString()