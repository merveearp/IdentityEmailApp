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
        }

        public class Images
        {
            public string thumbnail { get; set; }
        }


    }
}
