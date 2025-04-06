using Microsoft.EntityFrameworkCore.Query.Internal;
using Newtonsoft.Json;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Net;
using System.Text;
using System.Threading.Tasks;
using ToDoList.Model;

namespace ToDoList.Test
{

    public class ToDoListPost : IClassFixture<ToDoListFixture>, IDisposable
    {
        private readonly ToDoListFixture app;
        public ToDoListPost(ToDoListFixture  app)
        {
            this.app = app;
            app.LimparDadosBanco();
        }

        public void Dispose()
        {
            app.LimparDadosBanco();
        }

        [Fact]
        public async Task CriarTarefaValida()
        {
            // arrange -> organização
            var client = await app.RecolherClienteAutenticadoAsync();
            var tarefa = new TarefasModel
            {
                Titulo = "Incluindo tarefa",
                Descricao = "Irei criar sem ser pelo bogus para validação rapida",
                Status = StatusTarefa.Pendente
            };


            // action -> ação
            var response = await client.PostAsJsonAsync("/api/Tarefas/CriarTarefa", tarefa);

            //assert -> afirmação 

            Assert.NotNull(response);
            Assert.Equal(HttpStatusCode.Created, response.StatusCode);

        }


        [Fact]
        public async Task CriarTarefaComOferatNull()
        {
            // arrange -> organização
       
            var client = await app.RecolherClienteAutenticadoAsync();

            // action -> ação
            var response = await client.PostAsJsonAsync<object>("/api/Tarefas/CriarTarefa", null);

            //assert -> afirmação 

            Assert.NotNull(response);
            Assert.Equal(HttpStatusCode.BadRequest, response.StatusCode);

        }



        [Theory]
        [InlineData("Somente titulo","")]
        [InlineData("", "Somente Corpo")]
        [InlineData("string", "")]
        [InlineData("", "string")]
        public async Task CriarTarefaComInformacoeIncorretasNoHeaderENoBody(
            string titulo,
            string descricao)
        {
            // arrange -> organização

            var client = await app.RecolherClienteAutenticadoAsync();
            var tarefa = new TarefasModel
            {
                Titulo = titulo,
                Descricao = descricao,
                Status = StatusTarefa.Pendente
            };

            // action -> ação
            var response = await client.PostAsJsonAsync<object>("/api/Tarefas/CriarTarefa", tarefa);

            //assert -> afirmação 

            Assert.NotNull(response);
            Assert.Equal(HttpStatusCode.BadRequest, response.StatusCode);


        }






    }
}
