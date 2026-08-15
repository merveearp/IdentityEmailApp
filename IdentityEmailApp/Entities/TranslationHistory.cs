namespace IdentityEmailApp.Entities
{
    public class TranslationHistory
    {
        public int TranslationHistoryId { get; set; }

        public string SourceText { get; set; } = null!;

        public string TranslatedText { get; set; } = null!;

        public string SourceLanguage { get; set; } = null!;

        public string TargetLanguage { get; set; } = null!;

        public DateTime CreatedDate { get; set; } = DateTime.Now;

        public bool IsSaved { get; set; }

        public string AppUserId { get; set; } = null!;

        public AppUser AppUser { get; set; } = null!;
    }
}