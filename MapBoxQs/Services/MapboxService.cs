using System;
using System.Net.Http;
using System.Threading.Tasks;
using ModernHttpClient;
using Naxam.Controls.Forms;
using Newtonsoft.Json;

namespace MapBoxQs.Services
{
	public interface IMapBoxService
	{
        Task<MapStyle[]> GetAllStyles();
        Task<MapStyle> GetStyleDetails(string id, string owner = null);
	}

	public class MapBoxService : IMapBoxService
	{
		HttpClient client;

		private static string BaseURL = "https://api.mapbox.com/";
		public static string AccessToken = "sk.eyJ1IjoibmF4YW10ZXN0IiwiYSI6ImNqNWtpb2d1ZzJpMngyd3J5ZnB2Y2JhYmQifQ.LEvGqQkAqM4MO3ZtGbQrdw";
		public static string Username = "naxamtest";


		public MapBoxService()
		{
			client = new HttpClient(new NativeMessageHandler())
			{
				MaxResponseContentBufferSize = 256000
			};
		}

        public async Task<MapStyle[]> GetAllStyles()
		{
			var urlFormat = BaseURL + "styles/v1/{0}?access_token={1}";
			var uri = new Uri(string.Format(urlFormat, Username, AccessToken));
			var response = await client.GetAsync(uri);
			if (response.IsSuccessStatusCode)
			{
				var content = await response.Content.ReadAsStringAsync();
				System.Diagnostics.Debug.WriteLine(content);
				try
				{
                    return JsonConvert.DeserializeObject<MapStyle[]>(content);
				}
				catch (Exception ex)
				{
					System.Diagnostics.Debug.WriteLine("[EXCEPTION] " + ex.Message);
				}
			}
			return null;
		}

        public async Task<MapStyle> GetStyleDetails(string id, string owner = null)
		{
			var urlFormat = BaseURL + "styles/v1/{0}/{1}?access_token={2}";
			var uri = new Uri(string.Format(urlFormat, owner ?? Username, id, AccessToken));
			var response = await client.GetAsync(uri);
			if (response.IsSuccessStatusCode)
			{
				var content = await response.Content.ReadAsStringAsync();
				System.Diagnostics.Debug.WriteLine(content);
				try
				{
                    var output = JsonConvert.DeserializeObject<MapStyle>(content);
					return output;
				}
				catch (Exception ex)
				{
					System.Diagnostics.Debug.WriteLine("[EXCEPTION] " + ex.Message);
				}
			}
			return null;
		}
	}
}
