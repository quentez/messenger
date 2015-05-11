using System.Windows;
using GalaSoft.MvvmLight.Messaging;
using GalaSoft.MvvmLight.Threading;
using Messenger.Lib.Helpers;
using Messenger.Lib.Services;
using Messenger.Lib.Services.JsBindings;
using Messenger.Lib.UIServices;
using Messenger.Properties;
using Messenger.ViewModel;
using Messenger.Windows;
using Microsoft.Practices.Unity;

namespace Messenger
{
    /// <summary>
    /// Interaction logic for App.xaml
    /// </summary>
    public partial class App : Application
    {
        static App()
        {
            DispatcherHelper.Initialize();
        }

        public App()
        {
            this.InitializeComponent();

            // Configure dependency injection.
            var container = new UnityContainer();
            this.ConfigureContainer(container);

            // Initialize app.
            this.ViewModelFactory = container.Resolve<IViewModelFactory>();
            this.cefConfigureService = container.Resolve<ICefConfigureService>();
            this.cefConfigureService.Configure();

            // Open the main window.
            var mainWindow = new MainWindow();
            mainWindow.Show();

            // Open the notifications window.
            var notificationsWindow = new NotificationWindow();
            notificationsWindow.Show();
            
            // Focus the main window.
            mainWindow.Focus();
        }
        private readonly ICefConfigureService cefConfigureService;

        public IViewModelFactory ViewModelFactory { get; }

        // Register all services on our IoC container.
        private void ConfigureContainer(IUnityContainer c)
        {
            c.RegisterType<IAppConstants, AppConstants>(new ContainerControlledLifetimeManager());
            c.RegisterType<ITextHelpers, TextHelpers>(new ContainerControlledLifetimeManager());

            c.RegisterType<IJsonService, JsonService>(new ContainerControlledLifetimeManager());
            c.RegisterType<IDispatcherService, DispatcherService>(new ContainerControlledLifetimeManager());
            c.RegisterType<IDialogsService, DialogsService>(new ContainerControlledLifetimeManager());
            c.RegisterType<IExternalProcessService, ExternalProcessService>(new ContainerControlledLifetimeManager());
            c.RegisterType<ITaskBarOverlayService, TaskBarOverlayService>(new ContainerControlledLifetimeManager());
            c.RegisterType<INotificationsService, NotificationsService>(new ContainerControlledLifetimeManager());
            c.RegisterType<IViewModelFactory, ViewModelFactory>(new ContainerControlledLifetimeManager());
            c.RegisterType<IJsBindingFactory, JsBindingFactory>(new ContainerControlledLifetimeManager());
            c.RegisterType<IJsMainBinding, JsMainBinding>(new TransientLifetimeManager());
            c.RegisterType<ICefConfigureService, CefConfigureService>(new ContainerControlledLifetimeManager());
            c.RegisterType<IWindowService, WindowService>(new ContainerControlledLifetimeManager());
            c.RegisterType<IJsBridgeService, JsBridgeService>(new ContainerControlledLifetimeManager());
            c.RegisterType<IInjectScriptService, InjectScriptService>(new ContainerControlledLifetimeManager());

            c.RegisterType<IMessenger, GalaSoft.MvvmLight.Messaging.Messenger>(new ContainerControlledLifetimeManager());

            // ViewModels.
            c.RegisterType<MainViewModel, MainViewModel>(new ContainerControlledLifetimeManager());
            c.RegisterType<NotificationListViewModel, NotificationListViewModel>(new ContainerControlledLifetimeManager());
        }

        // Make sure to kill CEF when we exit, otherwise we won't be able to.
        protected override void OnExit(ExitEventArgs e)
        {
            // Make sure we don't save a minimized state.
            if (Settings.Default.WindowState == WindowState.Minimized)
            {
                Settings.Default.WindowState = WindowState.Normal;
            }

            // Save the settings.
            Settings.Default.Save();

            // Kill the CEF service and exit.
            this.cefConfigureService.Shutdown();
            base.OnExit(e);
        }
    }
}
