using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Options;
using Microsoft.IdentityModel.Tokens;
using ToDoList.Data;
using ToDoList.Model;

namespace ToDoList.Controller
{
    [Route("api/[controller]")]
    [ApiController]
    public class TarefasController : ControllerBase
    {
        private readonly AppDbContext _context;
        public TarefasController(AppDbContext context)
        {
            _context = context;
        }


        [HttpPost("CriarTarefa")]
        [Authorize(Roles = "Admin")]
        public async Task<ActionResult<TarefasModel>> CriarTarefa(TarefasModel tarefa)
        {
            if (tarefa == null)
            {
                return BadRequest("Requisição incompleta !!");
            }
    
            if(
                tarefa.Titulo.ToLower() == "string" || 
                tarefa.Titulo.IsNullOrEmpty() || 
                tarefa.Descricao.ToLower() == "string"||
                tarefa.Descricao.IsNullOrEmpty())
            {
                return BadRequest("Necessario conter titulo e conteudo");
            }

            _context.Tarefas.Add(tarefa);
            await _context.SaveChangesAsync();

            return CreatedAtAction(
                nameof(CriarTarefa), 
                "Tarefas", 
                new {id = tarefa.Id},
                tarefa);    
        }


        [HttpGet("RecolherTarefas")]
        [AllowAnonymous] 
        public async Task<ActionResult<List<TarefasModel>>> RecolherTarefas()
        {
            var tarefas =  await _context.Tarefas.ToListAsync();
            if (tarefas.Count() == 0)
            {
                    return NotFound("Nenhuma tarefa encontrada");
            }
            return Ok(tarefas);
        }


        [HttpGet("RecolherTarefasPaginada")]
        [AllowAnonymous]
        public async Task<ActionResult<List<TarefasModel>>>
            RecolherTarefasPaginada([FromQuery] int pagina, [FromQuery] int tamanhoPorPagina )
        {
            var tarefas =
                await _context.Tarefas
                .Skip((pagina -1 ) * tamanhoPorPagina)
                .Take(tamanhoPorPagina)
                .ToListAsync();


            if (tarefas.Count() == 0)
            {
                return NotFound("Nenhuma tarefa encontrada");
            }
            return Ok(tarefas);
        }


        [HttpGet("RecolherTarefa/{id}")]
        [AllowAnonymous]
        public  async Task<ActionResult<TarefasModel>> RecolherTarefaId(int id)
        {
            if (id <= 0 ){
                return BadRequest("Id informado invalido !!");
                }

            var tarefa = await _context.Tarefas.FirstOrDefaultAsync(tarefa => tarefa.Id == id); //adicionar orderby
          
            if(tarefa  == null)
            {
                return NotFound("O ID informado não existe no banco !");
            }
            return Ok(tarefa);
        }


        [HttpDelete("DeletarTarefa/{id}")]
        [Authorize(Roles = "Admin")]
        public async Task<ActionResult> DeletarTarefa(int id)
        {
            if (id <= 0)
            {
                return BadRequest("Id informado invalido !!");
            }

            var tarefa = await _context.Tarefas.FirstOrDefaultAsync(tarefa => tarefa.Id == id);
            if (tarefa == null)
            {   
                return NotFound("O ID informado não existe!");
            }
            _context.Tarefas.Remove(tarefa);
            await _context.SaveChangesAsync();
            return NoContent();
        }


        [HttpPut("AtualizarTarefa/{id}")]
        [Authorize(Roles ="Admin")]
        public async Task<ActionResult> AtualizarTarefa(int id, TarefasModel tarefa)
        {
            if (id<= 0 || tarefa == null)
            {
                return BadRequest("ID ou Corpo da requisição no formato invalido !!");
            }
            var tarefaASerModificada = await _context.
                Tarefas.
                FirstOrDefaultAsync(tarefa_ => tarefa_.Id == id);

            if(tarefaASerModificada == null)
            {
                return NotFound("O ID informado não existe na base !");
            }

            tarefaASerModificada.Descricao = tarefa.Descricao;
            tarefaASerModificada.Titulo = tarefa.Titulo;
            tarefaASerModificada.Data = DateTime.Now;

            _context.Tarefas.Update(tarefaASerModificada);
            await _context.SaveChangesAsync();

            return NoContent();
        }





    }
}
