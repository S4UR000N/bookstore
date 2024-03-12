namespace Persistence.DatabaseObject.Model.Entity
{
    public class RoleModel
    {
        public long Id { get; set; }
        public string Name { get; set; }
        public List<UserModel>? Users { get; set; }
    }
}
