using System;
using System.Windows.Input;
using Xamarin.Forms;

namespace Naxam.Mapbox.Forms
{
	public class MapViewDelegate
	{
		public MapViewDelegate()
		{
		}

		public ICommand DidFinishRenderingCommand { get; set; }
	}
}