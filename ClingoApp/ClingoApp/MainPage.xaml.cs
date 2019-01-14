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

        /*
        void OnButtonClicked(object sender, EventArgs args)
        {
            output.Text = "clicked: " + add(4, 5);

        }
        */

        void btnStartSolver_Clicked(object sender, EventArgs args)
        {
            ASPSolver tp1 = new ASPSolver();
            this.Navigation.PushAsync(tp1);

        }
    }
}
