using Entities;
using Microsoft.AspNetCore.Mvc;
using System.Text.Json;
using Services;
using DTO;
// For more information on enabling Web API for empty projects, visit https://go.microsoft.com/fwlink/?LinkID=397860

namespace MyShopWebApi.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class UsersController : ControllerBase
    {
        private readonly IUserService _userService;
        public UsersController(IUserService userService)
        {
            _userService = userService;
        }

        // GET: api/<UsersController>
        [HttpGet]
        public IEnumerable<string> Get()
        {
            return new string[] { "value1", "value2" };
        }

        // GET api/<UsersController>/5
        [HttpGet("{id}")]
        public string Get(int id)
        {
            return "value";
        }

        // POST api/<UsersController>
        [HttpPost]
        public void Post([FromBody]string value)
        {
        }

        [HttpPost("register")]
        public async Task<ActionResult<UserDTO>> Register([FromBody] UserRegisterDTO user)
        {

            UserDTO u = await _userService.Register(user);
            if (u!=null)
            {
                return Ok(u);
            }
            return StatusCode(400,"try Again");


        }

        [HttpPost("login")]
        public async Task<ActionResult<UserDTO>> Login([FromBody] UserLoginDTO user)
        {
            UserDTO u = await _userService.Login(user);
            if (u!=null)
            {
                return Ok(u);
            }
            return StatusCode(400,"try again");

        }

        [HttpPost("checkPassword")]
        public ActionResult<int> CheckPassword([FromBody] string password)
        {


            int result = _userService.CheckPassword(password);
        
            if (result>-1)
            {
                
                return Ok(result);
            }
            return StatusCode(400, "try again");

        }

        // PUT api/<UsersController>/5
        [HttpPut("{id}")]
        public void Put(int id, [FromBody]User u)
        {
            

            _userService.UpDate(u,id);

        }

        // DELETE api/<UsersController>/5
        [HttpDelete("{id}")]
        public void Delete(int id)
        {
        }
    }
}
