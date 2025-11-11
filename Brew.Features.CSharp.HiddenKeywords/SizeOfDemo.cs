using Microsoft.Extensions.Logging;
using System.Runtime.InteropServices;

namespace Brew.Features.CSharp.HiddenKeywords;

// Demonstrates sizeof keyword for getting compile-time type sizes
public class SizeOfDemo(ILogger<SizeOfDemo> logger)
{
    public void ShowTypeSizes()
    {
        logger.LogInformation("[sizeof] Compile-time type sizes (in bytes):");
        
        // sizeof works with unmanaged types (primitives, structs without references)
        logger.LogInformation("  sizeof(byte): {Size}", sizeof(byte));
        logger.LogInformation("  sizeof(short): {Size}", sizeof(short));
        logger.LogInformation("  sizeof(int): {Size}", sizeof(int));
        logger.LogInformation("  sizeof(long): {Size}", sizeof(long));
        logger.LogInformation("  sizeof(float): {Size}", sizeof(float));
        logger.LogInformation("  sizeof(double): {Size}", sizeof(double));
        logger.LogInformation("  sizeof(decimal): {Size}", sizeof(decimal));
        logger.LogInformation("  sizeof(bool): {Size}", sizeof(bool));
        logger.LogInformation("  sizeof(char): {Size}", sizeof(char));
        
        // Custom unmanaged struct - use Marshal.SizeOf for complex types
        logger.LogInformation("  Marshal.SizeOf<Point3D>(): {Size}", Marshal.SizeOf<Point3D>());
        logger.LogInformation("  Marshal.SizeOf<Color>(): {Size}", Marshal.SizeOf<Color>());
        
        logger.LogInformation("USE CASE: Interop, memory layout calculations, buffer sizing");
    }

    // Unmanaged struct example
    [StructLayout(LayoutKind.Sequential)]
    private struct Point3D
    {
        public float X;
        public float Y;
        public float Z;
    }

    // Another example with explicit layout
    [StructLayout(LayoutKind.Explicit)]
    private struct Color
    {
        [FieldOffset(0)]
        public byte R;
        [FieldOffset(1)]
        public byte G;
        [FieldOffset(2)]
        public byte B;
        [FieldOffset(3)]
        public byte A;
        
        [FieldOffset(0)]
        public uint Value; // Overlaps RGBA bytes
    }
}
