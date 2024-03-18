﻿using Application.User.Model.Request;
using Application.User.Service;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using Persistence.Context;
using Persistence.DatabaseObject.Model.Entity;

namespace Bookstore.API.Controllers
{
    [ApiController]
    [Route("api/[controller]/[action]")]
    [Authorize(Roles = "Admin")]
    public class UserController : ControllerBase
    {
        private UserService _userService;
        private RelationalContext _relationalContext;

        public UserController(UserService userService, RelationalContext relationalContext)
        {
            _userService = userService;
            _relationalContext = relationalContext;
        }

        
        [HttpPost]
        public async Task<IActionResult> Create([FromBody] CreateUserRequestModel request)
        {
            if (ModelState.IsValid)
            {
                var response = await _userService.Create(request);
                return StatusCode(response.StatusCode, response);
            }
            return BadRequest();
        }

        [HttpGet]
        public async Task<IActionResult> ReadMany()
        {
            var response = await _userService.ReadMany();
            return StatusCode(response.StatusCode, response);
        }

        [HttpGet("{id}")]
        public async Task<IActionResult> ReadOneById(long? id)
        {
            if (id == null)
            {
                return BadRequest();
            }

            var response = await _userService.ReadOneById(id.Value);
            return StatusCode(response.StatusCode, response);
        }

        [HttpPatch]
        public async Task<IActionResult> Patch([FromBody] PatchUserRoleRequestModel request)
        {
            if (ModelState.IsValid)
            {
                var response = await _userService.PatchRole(request);
                return StatusCode(response.StatusCode, response);
            }
            return BadRequest();
        }


        [HttpPost]
        public async Task<IActionResult> Update([FromBody] UserModel request)
        {
            if (!_userService.UserExists(request.Id))
            {
                return NotFound();
            }

            if (ModelState.IsValid)
            {
                try
                {
                    _relationalContext.Update(request);
                    await _relationalContext.SaveChangesAsync();
                }
                catch(Exception ex)
                {
                    return BadRequest();
                }
            }
            return Ok();
        }

        [HttpPost]
        public async Task<IActionResult> UpdateBooksOnUser([FromBody] UpdateBooksOnUserRequestModel request)
        {
            if (!_userService.UserExists(request.UserId))
            {
                return NotFound();
            }

            if (ModelState.IsValid)
            {
                try
                {
                    var user = _relationalContext.Users
                        .Include(user => user.Books)
                        .FirstOrDefault(user => user.Id == request.UserId);

                    foreach (var bookId in request.RemoveBooks)
                    {
                        var bookToRemove = user.Books.FirstOrDefault(b => b.Id == bookId);
                        if (bookToRemove != null)
                        {
                            user.Books.Remove(bookToRemove);
                        }
                    }

                    foreach (var bookId in request.AddBooks)
                    {
                        var bookToAdd = _relationalContext.Books.FirstOrDefault(b => b.Id == bookId);
                        if (bookToAdd != null)
                        {
                            user.Books.Add(bookToAdd);
                        }
                    }

                    _relationalContext.Users.Update(user);
                    await _relationalContext.SaveChangesAsync();
                }
                catch (Exception ex)
                {
                    return BadRequest();
                }
            }
            return Ok();
        }

        [HttpDelete("{id}")]
        public async Task<IActionResult> Delete(long id)
        {
            var userModel = await _relationalContext.Users.FindAsync(id);
            if (userModel == null)
            {
                return NotFound();
            }

            _relationalContext.Users.Remove(userModel);
            await _relationalContext.SaveChangesAsync();

            return NoContent();
        }
    }
}
