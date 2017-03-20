using static BotDemo.FeatureServiceResponse;

namespace BotDemo
{
    public class ProrailServices
    {
        public static Rootobject GetDrp(string afkorting)
        {
            var url = $"https://mapservices.prorail.nl/arcgis/rest/services/Gebiedsindelingen_ProRail_002/MapServer/13/query?where=AFKORTING=%27{afkorting}%27&outFields=*&f=json";
            var rootobject = Http.GetJson<Rootobject>(url);
            return rootobject;
        }
    }
}