// Run this helper against the unmodified upstream 0Harmony.dll with Mono.Cecil 0.11.1.
// The output is a metadata-normalized DLL with the same assembly name and method bodies.
using System;
using Mono.Cecil;

internal static class NormalizeHarmony
{
    private static int Main(string[] args)
    {
        if (args.Length != 2)
        {
            Console.Error.WriteLine("Usage: NormalizeHarmony <input 0Harmony.dll> <output 0Harmony.dll>");
            return 2;
        }

        try
        {
            using (var assembly = AssemblyDefinition.ReadAssembly(args[0]))
                assembly.Write(args[1]);
            return 0;
        }
        catch (Exception exception)
        {
            Console.Error.WriteLine(exception);
            return 1;
        }
    }
}
