namespace Persistence.DatabaseObject.Model.Entity
{
    public class BookModel
    {
        public long Id { get; set; }
        public string Title { get; set; }
        public string Description { get; set; }
        public string Author { get; set; }
        public int Year { get; set; }
        public List<UserModel>? Users { get; set; }
    }
}
