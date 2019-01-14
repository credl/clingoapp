using System;
using System.Threading;
using Xamarin.Forms;
using Xamarin.Forms.Xaml;

[assembly: XamlCompilation(XamlCompilationOptions.Compile)]
namespace ClingoApp
{
    public partial class App : Application
    {
        public App()
        {
            InitializeComponent();

            MainPage = new NavigationPage(new MainPage());

//            ThreadPool.QueueUserWorkItem(BackgroundThread);
        }

        /*
        BackgroundThread()
        {
            System.Threading.Thread.Sleep(5000);
            // When the download is complete, notify the UI
            MonoTouch.Foundation.NSObject.BeginInvokeOnMainThread(delegate {
                status.Text = "Download has been completed";
            });
        }
        */

        protected override void OnStart()
        {
            // Handle when your app starts
        }

        protected override void OnSleep()
        {
            // Handle when your app sleeps
        }

        protected override void OnResume()
        {
            // Handle when your app resumes
        }
    }
}
