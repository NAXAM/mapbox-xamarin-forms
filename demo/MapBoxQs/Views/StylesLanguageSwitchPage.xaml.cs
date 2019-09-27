using Naxam.Mapbox.Expressions;
using Naxam.Mapbox.Layers;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

using Xamarin.Forms;
using Xamarin.Forms.Xaml;

namespace MapBoxQs.Views
{
    [XamlCompilation(XamlCompilationOptions.Compile)]
    public partial class StylesLanguageSwitchPage : ContentPage
    {
        public StylesLanguageSwitchPage()
        {
            InitializeComponent();

            map.Center = new Naxam.Mapbox.LatLng(48, 16.05);
            map.ZoomLevel = 2.9;
        }

        async void btnChangeLanguage_Clicked(object sender, EventArgs e)
        {
            var dictionary = new Dictionary<string, string> {
                { "English", "name_en" },
                { "French", "name_fr" },
                { "Russian", "name_ru" },
                { "German", "name_de" },
                { "Spanish", "name_es" }
            };

            var selectedLanguage = await DisplayActionSheet(
                "Change language",
                "Cancel",
                null,
                dictionary.Keys.ToArray()
                );

            if (dictionary.TryGetValue(selectedLanguage, out var mapLanguage))
            {
                var layer = new SymbolLayer("country-label", "built-in") { 
                    TextField = Expression.Get(mapLanguage)
                };

                btnChangeLanguage.Text = selectedLanguage;
                map.Functions.UpdateLayer(layer);
            }
        }
    }
}