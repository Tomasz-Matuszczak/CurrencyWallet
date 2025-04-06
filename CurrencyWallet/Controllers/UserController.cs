using CurrencyWallet.Interfaces;
using CurrencyWallet.Models;
using Microsoft.AspNetCore.Mvc;

namespace CurrencyWallet.Controllers
{
    [ApiController]
    [Route("api/[controller]")]
    public class UserController : ControllerBase
    {
        private readonly IUserRepository _userRepository;

        public UserController(IUserRepository userRepository)
        {
            _userRepository = userRepository;
        }

        [HttpGet("GetAllUsers")]
        public IActionResult GetAllUsers()
        {
            return Ok(_userRepository.GetAllUsers());
        }
        [HttpGet("GetUserById")]
        public IActionResult GetUserById(int id)
        {
            var user = _userRepository.GetUserById(id);

            return user != null ? Ok(user) : NotFound();
        }

        [HttpPost("AddUser")]
        public IActionResult AddUser(UserModel user)
        {
            _userRepository.AddUser(user);
            return CreatedAtAction(nameof(GetUserById), new { }, user);
        }
    }
}
