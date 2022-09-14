namespace Viki
{
    internal class TheMovieDataBaseAPIResponse
    {
        public class Rootobject
        {
            public string name;
            public string first_air_date;
            public string original_name;

            public bool adult { get; set; }
            public string backdrop_path { get; set; }
            public Belongs_To_Collection belongs_to_collection { get; set; }
            public int budget { get; set; }
            public Genre[] genres { get; set; }
            public string homepage { get; set; }
            public int id { get; set; }
            public string imdb_id { get; set; }
            public string original_language { get; set; }
            public string original_title { get; set; }
            public string overview { get; set; }
            public float popularity { get; set; }
            public string poster_path { get; set; }
            public object[] production_companies { get; set; }
            public Production_Countries[] production_countries { get; set; }
            public string release_date { get; set; }
            public int revenue { get; set; }
            public int runtime { get; set; }
            public Spoken_Languages[] spoken_languages { get; set; }
            public string status { get; set; }
            public string tagline { get; set; }
            public string title { get; set; }
            public bool video { get; set; }
            public float vote_average { get; set; }
            public int vote_count { get; set; }
        }

        public class Belongs_To_Collection
        {
            public int id { get; set; }
            public string name { get; set; }
            public string poster_path { get; set; }
            public object backdrop_path { get; set; }
        }

        public class Genre
        {
            public int id { get; set; }
            public string name { get; set; }
        }

        public class Production_Countries
        {
            public string iso_3166_1 { get; set; }
            public string name { get; set; }
        }

        public class Spoken_Languages
        {
            public string english_name { get; set; }
            public string iso_639_1 { get; set; }
            public string name { get; set; }
        }

    }
}