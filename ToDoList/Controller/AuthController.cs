using ConexaoCodeFirst.Service;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using ToDoList.Data;
using ToDoList.Model;

namespace ToDoList.Controller
{
    [Route("v1")]
    [ApiController]
    public class AuthController : ControllerBase
    {
        private readonly AppDbContext _context;
        public AuthController(AppDbContext context)
        {
            _context = context;
        }

        [HttpPost("auth")]
        public async Task<ActionResult> Authenticate(UserModel user)
        {
            if (user == null)
            {
                return BadRequest("Inclua as informações do usuario!");
            }
            var usuario = await _context.Usuarios.FirstOrDefaultAsync(user_ => user_.Email == user.Email);
            if(usuario == null)
            {
                return NotFound("Usuario não encontrado !");
            }
            var token = TokenService.GerarToken(usuario);
         
            return Ok (new
            {
                token = token
            });
        }    
        
    }
}
