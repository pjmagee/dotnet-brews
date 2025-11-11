using Microsoft.Extensions.Logging;

namespace Brew.Features.CSharp.HiddenKeywords;

// Demonstrates stackalloc for stack-based memory allocation
public class StackAllocDemo(ILogger<StackAllocDemo> logger)
{
    public void HeapAllocation()
    {
        logger.LogInformation("[HEAP] Traditional array allocation:");
        
        int[] numbers = new int[5];
        for (int i = 0; i < numbers.Length; i++)
        {
            numbers[i] = i * 10;
        }
        
        logger.LogInformation("  Allocated on heap, GC pressure: {Values}", string.Join(", ", numbers));
        logger.LogInformation("  Managed by GC, survives method scope");
    }

    public void StackAllocation()
    {
        logger.LogInformation("[STACK] Using 'stackalloc' (performance optimization):");
        
        // Allocate on the stack - no GC, automatically freed when method returns
        // Modern C# allows stackalloc in safe context when assigned to Span<T>
        Span<int> numbers = stackalloc int[5];
        for (int i = 0; i < numbers.Length; i++)
        {
            numbers[i] = i * 10;
        }
        
        logger.LogInformation("  Allocated on stack, zero GC pressure: {Values}", 
            string.Join(", ", numbers.ToArray()));
        logger.LogInformation("  No GC overhead, automatically freed on scope exit");
        logger.LogInformation("  USE CASE: Small, short-lived buffers in hot paths");
    }
}
