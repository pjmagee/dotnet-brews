using Microsoft.Extensions.Logging;

namespace Brew.Features.CSharp.HiddenKeywords;

// Demonstrates __makeref, __refvalue, __reftype (legacy reference manipulation)
public class TypedRefDemo(ILogger<TypedRefDemo> logger)
{
    public void LegacyRefKeywords()
    {
        logger.LogInformation("[LEGACY] Using __makeref, __refvalue, __reftype:");
        
        int myInteger = 10;
        logger.LogInformation("  Original value: {Value}", myInteger);
        
        // Create a TypedReference to the variable
        TypedReference typedReference = __makeref(myInteger);

        // Read the value through the reference
        logger.LogInformation("  __refvalue(ref, int): {Value}", __refvalue(typedReference, int));
        
        // Get the type
        logger.LogInformation("  __reftype(ref): {Type}", __reftype(typedReference));

        // Modify through the reference
        __refvalue(typedReference, int) = 1000;
        logger.LogInformation("  After modifying via __refvalue: {Value}", myInteger);
        logger.LogInformation("  WARNING: Unsafe, error-prone, no modern use case!");
    }

    public void ModernAlternative()
    {
        logger.LogInformation("[MODERN] Using 'ref' keyword (recommended):");
        
        int myInt = 10;
        logger.LogInformation("  Original value: {Value}", myInt);
        
        ModifyByRef(ref myInt);
        logger.LogInformation("  After ModifyByRef: {Value}", myInt);
        logger.LogInformation("  Type-safe, compiler-checked, readable!");
    }

    private void ModifyByRef(ref int value)
    {
        value = 1000;
    }
}
