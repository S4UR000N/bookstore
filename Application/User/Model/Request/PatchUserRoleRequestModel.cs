namespace Application.User.Model.Request
{
    public class PatchUserRoleRequestModel
    {
        public long UserId {  get; set; }
        public long RoleId { get; set; }
    }
}
