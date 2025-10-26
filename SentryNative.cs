using System.Runtime.InteropServices;

namespace SentryDotNetNativeCrash
{
    internal partial class SentryNative
    {
        private const string SentryLibrary = "sentry.dll";

        // Opaque pointer types represented as IntPtr
        public struct SentryOptions { }
        public struct SentryValue
        {
            public ulong Bits;
        }

        // Enums
        public enum SentryLevel
        {
            Debug = -1,
            Info = 0,
            Warning = 1,
            Error = 2,
            Fatal = 3
        }

        // Core options functions
        [LibraryImport(SentryLibrary)]
        [UnmanagedCallConv(CallConvs = [typeof(System.Runtime.CompilerServices.CallConvCdecl)])]
        public static partial nint sentry_options_new();

        [LibraryImport(SentryLibrary)]
        [UnmanagedCallConv(CallConvs = [typeof(System.Runtime.CompilerServices.CallConvCdecl)])]
        public static partial void sentry_options_free(nint opts);

        [LibraryImport(SentryLibrary, StringMarshalling = StringMarshalling.Custom, StringMarshallingCustomType = typeof(System.Runtime.InteropServices.Marshalling.AnsiStringMarshaller))]
        [UnmanagedCallConv(CallConvs = [typeof(System.Runtime.CompilerServices.CallConvCdecl)])]
        public static partial void sentry_options_set_dsn(nint opts, string dsn);

        [LibraryImport(SentryLibrary, StringMarshalling = StringMarshalling.Custom, StringMarshallingCustomType = typeof(System.Runtime.InteropServices.Marshalling.AnsiStringMarshaller))]
        [UnmanagedCallConv(CallConvs = [typeof(System.Runtime.CompilerServices.CallConvCdecl)])]
        public static partial void sentry_options_set_database_path(nint opts, string path);

        [LibraryImport(SentryLibrary, StringMarshalling = StringMarshalling.Custom, StringMarshallingCustomType = typeof(System.Runtime.InteropServices.Marshalling.AnsiStringMarshaller))]
        [UnmanagedCallConv(CallConvs = [typeof(System.Runtime.CompilerServices.CallConvCdecl)])]
        public static partial void sentry_options_set_release(nint opts, string release);

        [LibraryImport(SentryLibrary, StringMarshalling = StringMarshalling.Custom, StringMarshallingCustomType = typeof(System.Runtime.InteropServices.Marshalling.AnsiStringMarshaller))]
        [UnmanagedCallConv(CallConvs = [typeof(System.Runtime.CompilerServices.CallConvCdecl)])]
        public static partial void sentry_options_set_environment(nint opts, string environment);

        [LibraryImport(SentryLibrary)]
        [UnmanagedCallConv(CallConvs = [typeof(System.Runtime.CompilerServices.CallConvCdecl)])]
        public static partial void sentry_options_set_debug(nint opts, int debug);

        [LibraryImport(SentryLibrary)]
        [UnmanagedCallConv(CallConvs = [typeof(System.Runtime.CompilerServices.CallConvCdecl)])]
        public static partial void sentry_options_set_sample_rate(nint opts, double sample_rate);

        [LibraryImport(SentryLibrary)]
        [UnmanagedCallConv(CallConvs = [typeof(System.Runtime.CompilerServices.CallConvCdecl)])]
        public static partial void sentry_options_set_auto_session_tracking(nint opts, int val);

        // Core SDK functions
        [LibraryImport(SentryLibrary)]
        [UnmanagedCallConv(CallConvs = [typeof(System.Runtime.CompilerServices.CallConvCdecl)])]
        public static partial int sentry_init(nint options);

        [LibraryImport(SentryLibrary)]
        [UnmanagedCallConv(CallConvs = [typeof(System.Runtime.CompilerServices.CallConvCdecl)])]
        public static partial int sentry_close();

        [LibraryImport(SentryLibrary)]
        [UnmanagedCallConv(CallConvs = [typeof(System.Runtime.CompilerServices.CallConvCdecl)])]
        public static partial int sentry_flush(ulong timeout);

        // Event capture functions
        [LibraryImport(SentryLibrary)]
        [UnmanagedCallConv(CallConvs = [typeof(System.Runtime.CompilerServices.CallConvCdecl)])]
        public static partial SentryValue sentry_value_new_event();

        [LibraryImport(SentryLibrary, StringMarshalling = StringMarshalling.Custom, StringMarshallingCustomType = typeof(System.Runtime.InteropServices.Marshalling.AnsiStringMarshaller))]
        [UnmanagedCallConv(CallConvs = [typeof(System.Runtime.CompilerServices.CallConvCdecl)])]
        public static partial SentryValue sentry_value_new_message_event(SentryLevel level, string logger, string text);

        [LibraryImport(SentryLibrary, StringMarshalling = StringMarshalling.Custom, StringMarshallingCustomType = typeof(System.Runtime.InteropServices.Marshalling.AnsiStringMarshaller))]
        [UnmanagedCallConv(CallConvs = [typeof(System.Runtime.CompilerServices.CallConvCdecl)])]
        public static partial SentryValue sentry_value_new_string(string value);

        [LibraryImport(SentryLibrary)]
        [UnmanagedCallConv(CallConvs = [typeof(System.Runtime.CompilerServices.CallConvCdecl)])]
        public static partial SentryValue sentry_capture_event(SentryValue evt);

        // Tag and context functions
        [LibraryImport(SentryLibrary, StringMarshalling = StringMarshalling.Custom, StringMarshallingCustomType = typeof(System.Runtime.InteropServices.Marshalling.AnsiStringMarshaller))]
        [UnmanagedCallConv(CallConvs = [typeof(System.Runtime.CompilerServices.CallConvCdecl)])]
        public static partial void sentry_set_tag(string key, string value);

        [LibraryImport(SentryLibrary, StringMarshalling = StringMarshalling.Custom, StringMarshallingCustomType = typeof(System.Runtime.InteropServices.Marshalling.AnsiStringMarshaller))]
        [UnmanagedCallConv(CallConvs = [typeof(System.Runtime.CompilerServices.CallConvCdecl)])]
        public static partial void sentry_set_extra(string key, SentryValue value);

        [LibraryImport(SentryLibrary, StringMarshalling = StringMarshalling.Custom, StringMarshallingCustomType = typeof(System.Runtime.InteropServices.Marshalling.AnsiStringMarshaller))]
        [UnmanagedCallConv(CallConvs = [typeof(System.Runtime.CompilerServices.CallConvCdecl)])]
        public static partial void sentry_set_context(string key, SentryValue value);

        [LibraryImport(SentryLibrary)]
        [UnmanagedCallConv(CallConvs = [typeof(System.Runtime.CompilerServices.CallConvCdecl)])]
        public static partial void sentry_set_level(SentryLevel level);

        // User functions
        [LibraryImport(SentryLibrary, StringMarshalling = StringMarshalling.Custom, StringMarshallingCustomType = typeof(System.Runtime.InteropServices.Marshalling.AnsiStringMarshaller))]
        [UnmanagedCallConv(CallConvs = [typeof(System.Runtime.CompilerServices.CallConvCdecl)])]
        public static partial SentryValue sentry_value_new_user(string id, string username, string email, string ip_address);

        [LibraryImport(SentryLibrary)]
        [UnmanagedCallConv(CallConvs = [typeof(System.Runtime.CompilerServices.CallConvCdecl)])]
        public static partial void sentry_set_user(SentryValue user);

        [LibraryImport(SentryLibrary)]
        [UnmanagedCallConv(CallConvs = [typeof(System.Runtime.CompilerServices.CallConvCdecl)])]
        public static partial void sentry_remove_user();

        // Breadcrumb functions
        [LibraryImport(SentryLibrary, StringMarshalling = StringMarshalling.Custom, StringMarshallingCustomType = typeof(System.Runtime.InteropServices.Marshalling.AnsiStringMarshaller))]
        [UnmanagedCallConv(CallConvs = [typeof(System.Runtime.CompilerServices.CallConvCdecl)])]
        public static partial SentryValue sentry_value_new_breadcrumb(string type, string message);

        [LibraryImport(SentryLibrary)]
        [UnmanagedCallConv(CallConvs = [typeof(System.Runtime.CompilerServices.CallConvCdecl)])]
        public static partial void sentry_add_breadcrumb(SentryValue breadcrumb);

        // Session functions
        [LibraryImport(SentryLibrary)]
        [UnmanagedCallConv(CallConvs = [typeof(System.Runtime.CompilerServices.CallConvCdecl)])]
        public static partial void sentry_start_session();

        [LibraryImport(SentryLibrary)]
        [UnmanagedCallConv(CallConvs = [typeof(System.Runtime.CompilerServices.CallConvCdecl)])]
        public static partial void sentry_end_session();

        // Windows API
        [LibraryImport("Kernel32", StringMarshalling = StringMarshalling.Utf16, SetLastError = false)]
        public static partial int WerUnregisterRuntimeExceptionModule(string pwszOutOfProcessCallbackDll, IntPtr pContext);

        [LibraryImport("kernel32", SetLastError = true, StringMarshalling = StringMarshalling.Utf16)]
        public static partial IntPtr GetModuleHandleW(string lpModuleName);

        // Helper method to initialize Sentry with basic configuration
        public static bool Initialize(string dsn, string databasePath = ".sentry-native",
            string? release = null, string? environment = null, bool debug = false)
        {
            try
            {
                var options = sentry_options_new();
                if (options == nint.Zero)
                    return false;

                sentry_options_set_dsn(options, dsn);
                sentry_options_set_database_path(options, databasePath);

                if (!string.IsNullOrEmpty(release))
                    sentry_options_set_release(options, release);

                if (!string.IsNullOrEmpty(environment))
                    sentry_options_set_environment(options, environment);

                sentry_options_set_debug(options, debug ? 1 : 0);
                sentry_options_set_auto_session_tracking(options, 1);

                return sentry_init(options) == 0;
            }
            catch
            {
                return false;
            }
        }

        // Helper method to capture a simple message
        public static void CaptureMessage(string message, SentryLevel level = SentryLevel.Info)
        {
            try
            {
                var evt = sentry_value_new_message_event(level, null, message);
                sentry_capture_event(evt);
            }
            catch
            {
                // Silently fail to avoid crashing the application
            }
        }

        // Helper method to shutdown Sentry
        public static void Shutdown()
        {
            try
            {
                sentry_close();
            }
            catch
            {
                // Silently fail
            }
        }
    }
}
