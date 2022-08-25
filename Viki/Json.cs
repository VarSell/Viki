namespace Viki
{
    internal class Json
    {
        public class Rootobject
        {
            public string context { get; set; }
            public string type { get; set; }
            public string name { get; set; }
            public string url { get; set; }
            public string image { get; set; }
            public string datePublished { get; set; }
            public Aggregaterating aggregateRating { get; set; }
            public string[] genre { get; set; }
            public string[] alternativeHeadline { get; set; }
            public string inLanguage { get; set; }
            public Actor[] actor { get; set; }
            public Countryoforigin countryOfOrigin { get; set; }
        }

        public class Aggregaterating
        {
            public string context { get; set; }
            public string type { get; set; }
            public string ratingValue { get; set; }
            public int ratingCount { get; set; }
            public int bestRating { get; set; }
            public int worstRating { get; set; }
        }

        public class Countryoforigin
        {
            public string context { get; set; }
            public string type { get; set; }
            public string name { get; set; }
        }

        public class Actor
        {
            public string context { get; set; }
            public string type { get; set; }
            public string name { get; set; }
        }

    }
}
