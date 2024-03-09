namespace Persistence.DatabaseObject.Model.Entity
{
    public class UserModel
    {
        public long Id { get; set; }
        public string Name { get; set; }
        public string Email { get; set; }
        public string Password { get; set; }
        public string Gender { get; set; }
        public DateTime Created { get; set; }
        public DateTime Updated { get; set; }
        public string UpdateSubject { get; set; }
        public bool IsActivated { get; set; }
        public bool IsDeleted { get; set; }
    }
}
