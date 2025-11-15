using Microsoft.UI.Xaml;
using SentryDotNetCommon;

namespace SentryWinUI3NativeCrash
{
    public partial class App : Application
    {
        private MainWindow? _window;

        public App()
        {
            InitializeComponent();
            InitializeSentry();
        }

        private void InitializeSentry()
        {
            bool wasInitialized = SentryNative.InitializeForDotNet(
                "1.0.42",
                "debug",
                true);
        }

        /// <summary>
        /// Invoked when the application is launched.
        /// </summary>
        /// <param name="args">Details about the launch request and process.</param>
        protected override async void OnLaunched(Microsoft.UI.Xaml.LaunchActivatedEventArgs args)
        {
            _window = new MainWindow();
            _window.Activate();
            //await _window.TriggerStowedExceptionInvalidPath();
        }
    }
}
