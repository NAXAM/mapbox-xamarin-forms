using System;
using System.Net.Http;
using System.Threading.Tasks;
using Naxam.Controls.Forms;
using Newtonsoft.Json;
using MapBoxQs.Dtos;
using System.Linq;

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
        static string BaseURL = "https://api.mapbox.com/";
        public static string AccessToken = "pk.eyJ1IjoidHV5ZW52IiwiYSI6ImNpdzI5aXVmZTAwMmEyeW12OHd1YmxtZTUifQ.ac4QpT_xRso8CDc3V_Fc0A";
        public static string Username = "tuyenv";
        public MapBoxService()
        {
            client = new HttpClient()
            {
                MaxResponseContentBufferSize = 256000
            };
        }

        public static MapStyle[] DefaultStyles = {
            MapStyle.STREETS,
            MapStyle.OUTDOORS,
            MapStyle.LIGHT,
            MapStyle.DARK,
            MapStyle.SATELITE,
            MapStyle.SATELITE_STREETS,
            MapStyle.NAVIGATION_PREVIEW_DAY,
            MapStyle.NAVIGATION_PREVIEW_NIGHT,
            MapStyle.NAVIGATION_GUIDANCE_DAY,
            MapStyle.NAVIGATION_GUIDANCE_NIGHT
        };

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
                    var privateStyles = JsonConvert.DeserializeObject<MapStyle[]>(content);

                    return privateStyles.Union(DefaultStyles)
                        .ToArray();
                }
                catch (Exception ex)
                {
                    System.Diagnostics.Debug.WriteLine("[EXCEPTION] " + ex.Message);
                }
            }
            return DefaultStyles;
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
                    var output = JsonConvert.DeserializeObject<MapStyleDto>(content).DtoToModel();
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
