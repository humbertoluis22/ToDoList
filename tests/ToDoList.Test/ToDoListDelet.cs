using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.DependencyInjection;
using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Net;
using System.Runtime.CompilerServices;
using System.Text;
using System.Threading.Tasks;

namespace ToDoList.Test
{

    public class ToDoListDelet : IClassFixture<ToDoListFixture>, IDisposable
    {
        private readonly ToDoListFixture app;
        public ToDoListDelet(ToDoListFixture  app)
        {
            this.app = app;
        }
        
        public void Dispose()
        {
            app.LimparDadosBanco();
        }


        [Fact]
        public async Task DeletarTarefaPorIdExistente()
        {
            // arrange ->  organizar
            var client = await app.RecolherClienteAutenticadoAsync();
            app.CriarTarefasFake();

            var tarefa =  await app.Context.Tarefas.FirstOrDefaultAsync();

            // action -> ação
            var response = await client.DeleteAsync($"/api/Tarefas/DeletarTarefa/{tarefa.Id}");

            // assert -> afirmação
            Assert.Equal(HttpStatusCode.NoContent, response.StatusCode);

        }


        [Fact]
        public async Task DeletarTarefaPorIdNegativo()
        {
            //arrange -> organização
            var client = await app.RecolherClienteAutenticadoAsync();

            //action  -> ação
            var response = await client.DeleteAsync($"/api/Tarefas/DeletarTarefa/{-1}");
            var respostaString = await response.Content.ReadAsStringAsync();

            //assert  -> afirmação
            Assert.Equal(HttpStatusCode.BadRequest, response.StatusCode);
            Assert.Contains("Id informado invalido !!", respostaString);

        }


        [Fact]
        public async Task DeletarTarefaComIdInexistente()
        {
            // arrange -> organização
            var client = await app.RecolherClienteAutenticadoAsync();
            app.CriarTarefasFake();

            var Qtdtarefa =  await app.Context.Tarefas.CountAsync();
            var idInexistente = Qtdtarefa + 1;


            // action -> ação
            var response = await client.DeleteAsync($"/api/Tarefas/DeletarTarefa/{idInexistente}");
            var respostaString = await response.Content.ReadAsStringAsync();

            // assert -> afirmação
            Assert.Equal(HttpStatusCode.NotFound,response.StatusCode);
            Assert.Contains("O ID informado não existe!",respostaString);

        }



    }
}
