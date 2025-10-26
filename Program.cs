using System.Runtime.CompilerServices;
using System.Runtime.InteropServices;

namespace SentryDotNetNativeCrash
{
    internal class Program
    {
        static void Main(string[] args)
        {
            // Update DSN here
            const string sentryDsn = "";
            if (string.IsNullOrEmpty(sentryDsn))
            {
                Console.WriteLine("Please set the Sentry DSN in the source code before running.");
                return;
            }

            // .NET registers its custom exception module first and takes precedence
            // https://github.com/dotnet/runtime/blob/82ce59a6f6d415a3df5edd94b7917ab7d13a7b0c/src/coreclr/vm/dwreport.cpp#L112
            // Therefore unregister it so our handler is called
            Console.WriteLine("Unregistering .NET common runtime exception module...");

            var runtimeDirectory = RuntimeEnvironment.GetRuntimeDirectory();
            var dotNetExceptionModule = Path.Combine(runtimeDirectory, "mscordaccore.dll");
            var clrModuleBase = SentryNative.GetModuleHandleW("coreclr.dll");

            var wasUnregistered = SentryNative.WerUnregisterRuntimeExceptionModule(dotNetExceptionModule, clrModuleBase);
            Console.WriteLine($"Unregistered .NET exception module: {wasUnregistered} @ '{dotNetExceptionModule}'");

            Console.WriteLine("Registering Sentry...");
            var tmpDirectory = Path.GetDirectoryName(Environment.ProcessPath!)!;
            var cacheDirectory = Path.Combine(tmpDirectory, "sentry-cache");

            // Will internally register custom WER handler
            bool wasInitialized = SentryNative.Initialize(
                sentryDsn,
                cacheDirectory,
                "1.0.42",
                "debug",
                true);

            Console.WriteLine($"Initialized Sentry Native: {wasInitialized} @ '{cacheDirectory}'");

            Console.WriteLine("Press any key to crash...");
            Console.ReadKey();

            CrashInStack3();
        }

        [MethodImpl(MethodImplOptions.NoInlining)]
        private static void CrashInStack3()
        {
            CrashInStack2();
        }

        [MethodImpl(MethodImplOptions.NoInlining)]
        private static void CrashInStack2()
        {
            CrashInStack1();
        }

        [MethodImpl(MethodImplOptions.NoInlining)]
        private static void CrashInStack1()
        {
            Environment.FailFast("Failing fast for Sentry!");
        }
    }
}
