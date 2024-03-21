namespace Application.User.Model.Request
{
    public class UpdateBooksOnUserRequestModel
    {
        public long UserId { get; set; }
        public long[] AddBooks { get; set; }
        public long[] RemoveBooks { get; set;}
    }
}
