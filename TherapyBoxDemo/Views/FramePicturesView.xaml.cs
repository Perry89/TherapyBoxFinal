using System;
using System.Collections.Generic;
using Xamarin.Forms;

namespace TherapyBoxDemo.Views
{
    public partial class FramePicturesView : ContentView
    {
        List<string> _images = new List<string>();
        public FramePicturesView()
        {
            InitializeComponent();
            MessagingCenter.Subscribe<App, List<string>>((App)Xamarin.Forms.Application.Current, "ImagesSelected", (s, images) =>
            {
                listItems.FlowItemsSource = images;
                _images = images;
            });
        }
    }
}
