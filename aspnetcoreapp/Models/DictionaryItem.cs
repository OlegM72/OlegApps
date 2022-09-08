namespace DictionaryApi.Models
{
    public class DictionaryItem
    {
        public long Id { get; set; }
        public string? Original { get; set; }
        public string? Translation { get; set; }
    }
}