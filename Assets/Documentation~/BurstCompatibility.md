# Burst compatibility reproduction

Environment: Codex cloud Linux, Unity `2022.3.22f1`, Burst `1.8.29`, Unity Reloader upstream commit `105930b057973a288cd6ff06820dc67a23d`.

1. Install the upstream package without changes. Burst reports `Failed to find entry-points`, then `BadImageFormatException: Read out of bounds` while hashing `Plugins/Harmony/net48/0Harmony.dll`.
2. Compile `NormalizeHarmony.cs` against Mono.Cecil `0.11.1` and run it on the upstream DLL. Keep the output filename `0Harmony.dll` in the package. Unity Reloader still patches methods with this DLL. Burst then advances past Harmony but reports that `UnityReloader.Core` cannot resolve `Microsoft.CodeAnalysis, Version=4.6.0.0`.
3. Copy the upstream `Plugins/Roslyn/2021+/Microsoft.CodeAnalysis.FSR.dll` and `Microsoft.CodeAnalysis.CSharp.FSR.dll` to the same directory under their internal assembly names, without `.FSR`. Add `.meta` files with the plugin importers disabled on all platforms. The original `.FSR.dll` files stay enabled for Unity Reloader.
4. Recompile one asmdef, then repeat a method-body edit. In this project, Burst produced no further `Failed to find entry-points` or `BadImageFormatException`, and Unity Reloader patched a return value in Edit Mode and Play Mode. Play Mode remained active and could be exited normally.

The aliases are needed because Burst's assembly resolver uses the internal assembly name to form a DLL filename, while Unity Reloader ships the Roslyn DLLs with `.FSR` in their filenames. The aliases are import-disabled so Unity does not load a second copy of the assemblies.

This verifies the patch on Unity 2022.3 with Burst 1.8.29. The Roslyn alias location is for Unity 2021 and newer; older Unity versions are outside this project's supported setup.
