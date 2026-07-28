namespace IdentityEmailApp.DTOs.NewsDtos
{
    public class ResultLatestOfNewDto
    {

        public List<Item> items { get; set; }


        public class Item
        {
            public string title { get; set; }
            public string newsUrl { get; set; }
            public string publisher { get; set; }
            public Images images { get; set; }
            public string snippet { get; set; }
            public List<Subnew> subnews { get; set; }
            public bool hasSubnews { get; set; }
        }


        public class Images
        {
            public string thumbnailProxied { get; set; }
            public string thumbnail { get; set; }
        }


        public class Subnew
        {
            public string title { get; set; }
            public string timestamp { get; set; }
            public string newsUrl { get; set; }
            public string publisher { get; set; }
            public Images1 images { get; set; }
            public string snippet { get; set; }
        }

        public class Images1
        {
            public string thumbnailProxied { get; set; }
            public string thumbnail { get; set; }
        }



    }
}
