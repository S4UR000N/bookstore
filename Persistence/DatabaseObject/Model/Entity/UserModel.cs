namespace Persistence.DatabaseObject.Model.Entity
{
    public class UserModel
    {
        public long Id { get; set; }
        public string FirstName { get; set; }
        public string LastName { get; set; }
        public string Email { get; set; }
        public string Password { get; set; }
        public long RoleId { get; set; } = 3;
        public RoleModel Role { get; set; }

        public UserModel(string firstName, string lastName, string email, string password)
        {
            FirstName = firstName ?? string.Empty;
            LastName = lastName ?? string.Empty;
            Email = email ?? string.Empty;
            Password = password ?? string.Empty;
        }
    }
}
