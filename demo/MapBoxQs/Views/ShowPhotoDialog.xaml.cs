using Rg.Plugins.Popup.Pages;
using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

using Xamarin.Forms;
using Xamarin.Forms.Xaml;

namespace MapBoxQs.Views
{
    [XamlCompilation(XamlCompilationOptions.Compile)]
    public partial class ShowPhotoDialog : ContentPage
    {
        public ShowPhotoDialog(byte[] data)
        {
            InitializeComponent();
            Image image = new Image();
            Stream stream = new MemoryStream(data);
            demoImage.Source = ImageSource.FromStream(() => { return stream; });
        }
    }
}