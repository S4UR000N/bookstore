using Application.User.Model.Request;
using Associated.Application.Auth.Service;
using Associated.Application.Common.Model.Response;
using Microsoft.EntityFrameworkCore;
using Persistence.Context;
using Persistence.DatabaseObject.Model.Entity;
using System.Text.RegularExpressions;

namespace Application.User.Service
{
    public class UserService
    {
        private readonly RelationalContext _relationalContext;
        public UserService(RelationalContext relationalContext)
        {
            _relationalContext = relationalContext;
        }
        
        public async Task<ResponseModel<bool>> Create(CreateUserRequestModel request)
        {
            var response = new ResponseModel<bool>();

            if (EmailValid(request.Email))
            {
                if (await EmailExists(request.Email))
                {
                    response.AddError("Email", "Email Aleready Exists");
                }
            }
            else
            {
                response.AddError("Email", "Invalid Email");
            }

            if (!PasswordService.PasswordValid(request.Password))
            {
                response.AddError("Password", "Invalid Password");
            }

            if (response.Errors.Count < 1)
            {
                var userModel = new UserModel(request.FirstName, request.LastName, request.Email, PasswordService.HashPassword(request.Password));
                try
                {
                    _relationalContext.Add(userModel);
                    await _relationalContext.SaveChangesAsync();

                    response.Result = true;
                    response.StatusCode = 200;
                }
                catch (Exception ex)
                {
                    response.AddError("Unknown", "Unknown error occured");
                }
            }

            return response;
        }

        public async Task<ResponseModel<List<UserModel>>> ReadMany()
        {
            var response = new ResponseModel<List<UserModel>>();

            try
            {
                var users = await _relationalContext.Users.Include(u => u.Role).ToListAsync();

                response.Result = users;
                if (users.Count < 1)
                {
                    response.StatusCode = 404;
                    response.AddError("Not Found", "Users not found");
                }
                else
                {
                    response.StatusCode = 200;
                }
            }
            catch (Exception ex)
            {
                response.AddError("Unknown", "Unknown error occured");
            }

            return response;
        }

        public async Task<ResponseModel<UserModel>> ReadOneById(long id)
        {
            var response = new ResponseModel<UserModel>();

            try
            {
                var user = _relationalContext.Users
                    .Include(u => u.Role)
                    .Include(u => u.Books)
                    .FirstOrDefault(m => m.Id == id);

                if (user == null)
                {
                    response.StatusCode = 404;
                    response.AddError("Not Found", "User not found");
                }
                else
                {
                    response.StatusCode = 200;
                    response.Result = user;
                }
            }
            catch(Exception ex)
            {
                response.AddError("Unknown", "Unknown error occured");
            }

            return response;
        }

        public async Task<ResponseModel<bool>> PatchRole(PatchUserRoleRequestModel request)
        {
            var userResponse = await ReadOneById(request.UserId);
            var response = new ResponseModel<bool>();
            if (userResponse.StatusCode == 200)
            {
                try
                {
                    var user = userResponse.Result;
                    user.RoleId = request.RoleId;
                    _relationalContext.Update(user);
                    await _relationalContext.SaveChangesAsync();

                    response.Result = true;
                    response.StatusCode = 200;
                }
                catch (Exception ex)
                {
                    response.AddError("Unknown", "Unknown error occured");
                }
            }

            return response;
        }

        public async Task<bool> EmailExists(string email)
        {
            return await _relationalContext.Users.AnyAsync(u => u.Email == email);
        }
        public bool EmailValid(string email)
        {
            Regex regex = new Regex(@"^[a-zA-Z0-9._%+-]+@[a-zA-Z0-9.-]+\.[a-zA-Z]{2,}$");
            return regex.IsMatch(email);
        }

        public bool UserExists(long id)
        {
            return _relationalContext.Users.Any(e => e.Id == id);
        }
    }
}
