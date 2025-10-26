# Sample app for .NET Native Error Handling with Sentry
Relates to issue https://github.com/getsentry/sentry-dotnet/issues/3244

## Description
Currently it's not possible to catch native exceptions from .NET using Sentry, for example in case of access violations, stack overflow, fail-fast exceptions; or even any kind of C++/native exception because .NET adds its own vectored exception handler etc. This makes the usual Crashpad handler backend unsuitable.

Using WerRegisterRuntimeExceptionModule, a custom exception handler can be loaded by Windows Error Reporting, allowing to inspect any kind of unhandled exception, even fail-fast exceptions.

I added an experimental implementation of a custom WER handler backend to sentry-native: https://github.com/BernhardMarconato/sentry-native/tree/feature/wer-handler-backend

This can be used from a .NET app to make native exception handling work with Sentry!

## Steps to reproduce
How to build the test .NET app that can upload a minidump on fail-fast exception to Sentry:

* Check out sentry-native fork https://github.com/BernhardMarconato/sentry-native/tree/feature/wer-handler-backend
* Build the sentry-native fork with new WER backend:  
`cmake -B build -DSENTRY_BACKEND=wer -DSENTRY_BUILD_RUNTIMESTATIC=ON -DCMAKE_BUILD_TYPE=Debug -DSENTRY_BUILD_SHARED_LIBS=ON`  
`cmake --build build --config Debug`
* Copy the compiled binaries `sentry.dll`, `sentry_wer_module.dll` (and pdb files if required) from the `sentry-native\build\Debug` directory to the `SentryDotNetNativeCrash` .NET project directory
* Launch the `SentryDotNetNativeCrash\SentryDotNetNativeCrash.slnx` solution in Visual Studio
* Update the Sentry DSN in the Program.cs file
* Ensure `sentry.dll`, `sentry_wer_module.dll` binaries are set to "Copy to Output Directory: Copy if newer" in the Solution Explorer
* Build the solution
* Go to the output directory `SentryDotNetNativeCrash\bin\x64\Debug\net8.0-windows10.0.19041.0`
* Upload all symbols from this directory to Sentry using `sentry-cli.exe`
* Optional: start *Sysinternals DebugView* app to see debug logs of WER handler (as it runs in the WerFault.exe process)
* Start the `SentryDotNetNativeCrash.exe`
* Press any key to cause the crash
* Verify in the `SentryDotNetNativeCrash\bin\x64\Debug\net8.0-windows10.0.19041.0\sentry-cache\<GUID>` directory, a `.dmp` file was created
* Check on Sentry.io whether the minidump arrived
* Optional: Open the `.dmp` file in `WinDbg` and enter `!analyze -v` to analyze the dump locally

## Known issues
Currently, Sentry is not able to symbolicate minidumps that contain .NET functions and methods in the stack. They will be simply displayed as `<unknown>`. The information is available in the dump file itself however, as WinDbg or Visual Studio are able to show all the .NET methods. Additional handling on Sentry backend side may be necessary.

Sentry stack trace:  

```
OS Version: Windows 10.0.26200 (6901)
Report Version: 104

Crashed Thread: 42708

Application Specific Information:
Fatal Error: unknown 0x80131623 / 0x7ff8932eba48

Thread 42708 Crashed:
0   coreclr.dll                     0x7ff8f2f64515      EEPolicy::HandleFatalError (eepolicy.cpp:777)
1   coreclr.dll                     0x7ff8f3094c2b      SystemNative::GenericFailFast (system.cpp:285)
2   coreclr.dll                     0x7ff8f3094771      SystemNative::FailFast (system.cpp:304)
3   <unknown>                       0x7ff8932eba48      <unknown>
4   <unknown>                       0x6d1037ec78        <unknown>
```


WinDbg stack trace:  

```
coreclr!EEPolicy::HandleFatalError+0x7d   
coreclr!SystemNative::GenericFailFast+0x2b0   
coreclr!SystemNative::FailFast+0x92   
SentryDotNetNativeCrash!SentryDotNetNativeCrash.Program.CrashInStack1+0x28   
SentryDotNetNativeCrash!SentryDotNetNativeCrash.Program.CrashInStack2+0x1e   
SentryDotNetNativeCrash!SentryDotNetNativeCrash.Program.CrashInStack3+0x1e   
SentryDotNetNativeCrash!SentryDotNetNativeCrash.Program.Main+0x2f7   
coreclr!CallDescrWorkerInternal+0x83   
coreclr!CallDescrWorkerWithHandler+0x55   
coreclr!MethodDescCallSite::CallTargetWorker+0x2a6   
coreclr!MethodDescCallSite::Call+0xb   
coreclr!RunMainInternal+0x11f   
coreclr!RunMain+0xe0   
coreclr!Assembly::ExecuteMainMethod+0x1ab   
coreclr!CorHost2::ExecuteAssembly+0x246   
coreclr!coreclr_execute_assembly+0xde   
hostpolicy!coreclr_t::execute_assembly+0x29   
hostpolicy!run_app_for_context+0x5ea   
hostpolicy!run_app+0x3c   
hostpolicy!corehost_main+0x171   
hostfxr!execute_app+0x2bb   
hostfxr!`anonymous namespace'::read_config_and_execute+0xac   
hostfxr!fx_muxer_t::handle_exec_host_command+0x1d6   
hostfxr!fx_muxer_t::execute+0x2ad   
hostfxr!hostfxr_main_startupinfo+0xa8   
SentryDotNetNativeCrash_exe!exe_start+0x7fc   
SentryDotNetNativeCrash_exe!wmain+0x156   
SentryDotNetNativeCrash_exe!invoke_main+0x22   
SentryDotNetNativeCrash_exe!__scrt_common_main_seh+0x10c   
kernel32!BaseThreadInitThunk+0x17   
ntdll!RtlUserThreadStart+0x2c   
```



