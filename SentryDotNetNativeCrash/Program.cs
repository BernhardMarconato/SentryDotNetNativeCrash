using SentryDotNetCommon;
using System.Runtime.CompilerServices;

namespace SentryDotNetNativeCrash
{
    internal class Program
    {
        static void Main(string[] args)
        {
            bool wasInitialized = SentryNative.InitializeForDotNet(
                "1.0.42",
                "debug",
                true);

            Console.WriteLine($"Initialized Sentry Native: {wasInitialized}");

            Console.WriteLine("\nPress any key to crash...");
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
