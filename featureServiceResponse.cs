namespace BotDemo
{
    public class FeatureServiceResponse
    {

        public class Rootobject
        {
            public string displayFieldName { get; set; }
            public Fieldaliases fieldAliases { get; set; }
            public string geometryType { get; set; }
            public Spatialreference spatialReference { get; set; }
            public Field[] fields { get; set; }
            public Feature[] features { get; set; }
        }

        public class Fieldaliases
        {
            public string ID { get; set; }
            public string NAAM { get; set; }
            public string AFKORTING { get; set; }
            public string TYPE { get; set; }
            public string KM_LINTNAAM { get; set; }
            public string KM_KMLINT { get; set; }
            public string ROTATIE { get; set; }
            public string VERSIE { get; set; }
        }

        public class Spatialreference
        {
            public int wkid { get; set; }
            public int latestWkid { get; set; }
        }

        public class Field
        {
            public string name { get; set; }
            public string type { get; set; }
            public string alias { get; set; }
            public int length { get; set; }
        }

        public class Feature
        {
            public Attributes attributes { get; set; }
            public Geometry geometry { get; set; }
        }

        public class Attributes
        {
            public int ID { get; set; }
            public string NAAM { get; set; }
            public string AFKORTING { get; set; }
            public string TYPE { get; set; }
            public string KM_LINTNAAM { get; set; }
            public float KM_KMLINT { get; set; }
            public object ROTATIE { get; set; }
            public string VERSIE { get; set; }
        }

        public class Geometry
        {
            public float x { get; set; }
            public float y { get; set; }
        }
    }
}