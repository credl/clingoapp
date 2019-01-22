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
	public partial class AnswerSetView : ContentPage
	{
		public AnswerSetView (string answerset)
		{
			InitializeComponent ();

            lblAnswerSet.Text = answerset;
		}
	}
}