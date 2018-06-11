using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

using Xamarin.Forms;
using Xamarin.Forms.Xaml;

namespace TherapyBoxDemo.Pages
{
	[XamlCompilation(XamlCompilationOptions.Compile)]
	public partial class PicturesPage : ContentPage
	{
        List<string> _images = new List<string>();
        public PicturesPage ()
		{
			InitializeComponent ();
		}
        protected override void OnAppearing()
        {
            base.OnAppearing();
            MessagingCenter.Subscribe<App, List<string>>((App)Xamarin.Forms.Application.Current, "ImagesSelected", (s, images) =>
            {
                listItems.FlowItemsSource = images;
                _images = images;
            });
        }
    }
}