namespace server.Models
{
    public class Ranking
    {
        public int AuthorityId { get; set; }
        public int BookId { get; set; }
        public int CommentaryId { get; set; }
        public float Rank { get; set; }
    }
}