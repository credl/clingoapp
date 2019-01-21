using System;
using System.Collections.Generic;
using System.Linq;
using System.Runtime.InteropServices;
using System.Text;
using System.Threading.Tasks;
using Xamarin.Forms;

namespace ClingoApp
{
    public partial class MainPage : ContentPage
    {
        [DllImport("libDynLib.so", EntryPoint = "addGlobal")]
        public static extern int add(int a, int b);

        public MainPage()
        {
            InitializeComponent();
        }

        void btnStartSolver_Clicked(object sender, EventArgs args)
        {
            ASPSolver solverPage = new ASPSolver();
            this.Navigation.PushAsync(solverPage);

        }
    }
}
