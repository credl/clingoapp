using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

using Xamarin.Forms;
using Xamarin.Forms.Xaml;

namespace ClingoApp
{
    [XamlCompilation(XamlCompilationOptions.Compile)]
    public partial class ASPSolver : TabbedPage
    {
        private bool initialized = false;

        public ASPSolver ()
        {
            InitializeComponent();

            startInitSolver();
        }

        private void startInitSolver()
        {
            ediASPInput.Text = "Initializing solver ...";
            tabMain.IsEnabled = false;
            btnEvaluate.IsEnabled = false;
            btnClear.IsEnabled = false;
            wbvSolver.Source = "file:///android_asset/clingo.html";
            initialized = false;
        }

        private void onSolverInitialized() {
            ediASPInput.Text = "p(1). p(2). sum(X) :- X = #sum{ Y : p(Y) }. x | y.";
            ediASPInput.Text = "color(X, red) :- node(X), not color(X, green), not color(X, blue).\n" +
                "color(X, green) :-node(X), not color(X, red), not color(X, blue).\n" +
                "color(X, blue) :-node(X), not color(X, red), not color(X, green).\n" +
                ":-edge(X, Y), color(X, C), color(Y, C).\n" +
                "node(a).node(b).node(c).node(d).edge(a, b).edge(a, c).edge(a, d).edge(b, d).edge(c, d).\n";
            tabMain.IsEnabled = true;
            btnEvaluate.IsEnabled = true;
            btnClear.IsEnabled = true;
        }


        private async void btnEvaluate_Clicked(object sender, EventArgs e)
        {
            lstAnswerSets.ItemsSource = new string[] { "Computing ..." };
            tabMain.SelectedItem = tabASPInput;
            tabMain.SelectedItem = tabAnswerSets;
            tabMain.IsEnabled = false;
            string res = await wbvSolver.EvaluateJavaScriptAsync("solveProg(\"" + Uri.EscapeDataString(ediASPInput.Text) + "\");");
            var items = res.Split(';');
            var finItems = items;
            lstAnswerSets.ItemsSource = finItems;
            tabMain.IsEnabled = true;
        }
        private void btnClear_Clicked(object sender, EventArgs e)
        {
            ediASPInput.Text = "";
        }

        private void wbvSolver_Navigated(object sender, WebNavigatedEventArgs e)
        {
            if (!initialized)
            {
                initialized = true;
                onSolverInitialized();
            }
        }
    }
}