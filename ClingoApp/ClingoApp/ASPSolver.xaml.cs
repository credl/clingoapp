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

        private const string crewProblem = @"
% This encoding is based on an encoding by Hakan Kjellerstrand, hakank@gmail.com
% See also http://www.hakank.org/answer_set_programming/

%
% Crew allocation in ASP.
%
% From Gecode example crew
% examples/crew.cc
%
% (Original text from crew.cc)
% Example: Airline crew allocation
%
% Assign x flight attendants to y flights.Each flight needs a certain
% number of cabin crew, and they have to speak certain languages.
% Every cabin crew member has z flights off after an attended flight.

% a flight must have a crew of exactly Num persons
{ crew(Flight, Person) : p(Person, Type) } = Num :- num_crew(Flight, Num).

% the crew must have at least Num persons of a certain type
:- flight(Flight, Type, Num), #count{ Person : crew(Flight, Person), p(Person, Type) } < Num.

% every cabin crew member must have(a least) number of flights off after an attended flight
:- crew(Flight1, Person), crew(Flight2, Person), min_rest(X), Flight1<Flight2, Flight2 <= Flight1+X.

% check whether better than optimal solution exists
:- minimumcrew(N), #count{ Person : crew(Flight, Person) } >= N.

% symmetry breaking

diff(Person1, Person2) :- p(Person1, Type1), p(Person2, Type2), flight(Flight, Type1, Num), not p(Person2, Type1).
same(Person1, Person2) :- p(Person1, Type1), p(Person2, Type2), Person1<Person2, not diff(Person1, Person2), not diff(Person2, Person1).

dist(Person1, Person2) :- same(Person1, Person), same(Person, Person2).
next(Person1, Person2) :- same(Person1, Person2), not dist(Person1, Person2).

init(Flight) :- #min{ F : num_crew(F, Num) } = Flight.
last(Flight) :- #max{ F : num_crew(F, Num) } = Flight.

symm(Person1, Person2, Flight) :- next(Person1, Person2), init(Flight), num_crew(Flight, Num).
symm(Person1, Person2, Flight+1) :- symm(Person1, Person2, Flight), not last(Flight), crew(Flight, Person2).
symm(Person1, Person2, Flight+1) :- symm(Person1, Person2, Flight), not last(Flight), not crew(Flight, Person1).

:- symm(Person1, Person2, Flight), crew(Flight, Person2), not crew(Flight, Person1).

minimumcrew(14).
min_rest(1).
flights(1).
flights(2).
flights(3).
flights(4).
flights(5).
flights(6).
num_crew(1,6).
num_crew(2,7).
num_crew(3,7).
num_crew(4,6).
num_crew(5,4).
num_crew(6,4).
flight(1,steward,2).
flight(2,steward,3).
flight(3,steward,3).
flight(4,steward,2).
flight(5,steward,1).
flight(6,steward,1).
flight(1,hostess,2).
flight(2,hostess,3).
flight(3,hostess,3).
flight(4,hostess,2).
flight(5,hostess,1).
flight(6,hostess,1).
flight(1,spanish,1).
flight(2,spanish,1).
flight(3,spanish,1).
flight(4,spanish,1).
flight(5,spanish,1).
flight(6,spanish,1).
flight(1,french,1).
flight(2,french,1).
flight(3,french,1).
flight(4,french,1).
flight(5,french,1).
flight(6,french,1).
flight(1,german,1).
flight(2,german,1).
flight(3,german,1).
flight(4,german,1).
flight(5,german,1).
flight(6,german,1).
p(a,hostess).
p(b,hostess).
p(c,hostess).
p(d,hostess).
p(e,hostess).
p(f,hostess).
p(g,hostess).
p(h,hostess).
p(i,steward).
p(j,steward).
p(k,steward).
p(l,steward).
p(m,steward).
p(n,steward).
p(o,steward).
p(p,steward).
p(l,spanish).
p(o,spanish).
p(d,spanish).
p(f,spanish).
p(b,spanish).
p(a,spanish).
p(k,spanish).
p(f,french).
p(b,french).
p(p,french).
p(l,french).
p(k,french).
p(l,german).
p(h,german).
p(k,german).
p(j,german).";

        public ASPSolver ()
        {
            InitializeComponent();

            startInitSolver();
        }

        private async void startInitSolver()
        {
            ediASPInput.Text = "Initializing solver ...";
            tabMain.IsEnabled = false;
            btnEvaluate.IsEnabled = false;
            wbvSolver.Source = "file:///android_asset/clingo.html";
            initialized = false;
        }

        private void onSolverInitialized() {
            ediASPInput.Text = crewProblem;
            /*
            ediASPInput.Text = "p(1). p(2). sum(X) :- X = #sum{ Y : p(Y) }. x | y.";
            ediASPInput.Text = "color(X, red) :- node(X), not color(X, green), not color(X, blue).\n" +
                "color(X, green) :-node(X), not color(X, red), not color(X, blue).\n" +
                "color(X, blue) :-node(X), not color(X, red), not color(X, green).\n" +
                ":-edge(X, Y), color(X, C), color(Y, C).\n" +
                "node(a).node(b).node(c).node(d).edge(a, b).edge(a, c).edge(a, d).edge(b, d).edge(c, d).\n";
            */
            tabMain.IsEnabled = true;
            btnEvaluate.IsEnabled = true;
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

        private async void wbvSolver_Navigated(object sender, WebNavigatedEventArgs e)
        {
            if (!initialized)
            {
                //string res = await wbvSolver.EvaluateJavaScriptAsync("solveProg(\"" + Uri.EscapeDataString("a. b :- a.") + "\");");
                initialized = true;
                onSolverInitialized();
            }

            /*
            string res = await wbvSolver.EvaluateJavaScriptAsync("document.getElementById('outputDiv').innerHTML");
            var items = res.Split(';');
            var finItems = items;
            */

            /*
            var finItems = new string[items.Length];
            for (int i = 0; i < items.Length; i++) {
                finItems[i] = "{" + items[i].Replace(' ', ',') + "}";
            }
            */
            //lstAnswerSets.ItemsSource = finItems;
            //tabMain.IsEnabled = true;

        }
    }
}