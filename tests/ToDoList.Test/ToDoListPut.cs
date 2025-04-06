using Microsoft.EntityFrameworkCore;
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

    //[Collection(nameof(ToDoListFixture))]
    //public class ToDoListPut : IDisposable
    public class ToDoListPut : IClassFixture<ToDoListFixture>, IDisposable
    {
        private readonly ToDoListFixture app;
        public ToDoListPut(ToDoListFixture  app)
        {
            this.app = app;
            app.LimparDadosBanco();

        }

        public void Dispose()
        {
            app.LimparDadosBanco();
        }


        [Fact]
        public async Task AtualizarTarefaComIdValido()
        {
            // arrange -> organização
            var client = await app.RecolherClienteAutenticadoAsync();
            app.CriarTarefasFake();
            var tarefa = await app.Context.Tarefas.FirstOrDefaultAsync();

            tarefa.Titulo = "ValidandotestePut";

            // action -> ação
            var response = await client.PutAsJsonAsync($"/api/Tarefas/AtualizarTarefa/" +
                $"{tarefa.Id}",tarefa);

            // assert -> afirmação
            Assert.Equal(HttpStatusCode.NoContent, response.StatusCode);

            var tarefaAtualizada = app.Context.Tarefas.FirstOrDefault(t => t.Id == tarefa.Id);
            Assert.NotNull(tarefaAtualizada);
            Assert.Contains("ValidandotestePut", tarefaAtualizada.Titulo);

        }


        [Fact]
        public async Task AtualizarTarefaComIdNegativo()
        {
            //arrange -> organização
            var client = await app.RecolherClienteAutenticadoAsync();
            app.CriarTarefasFake();
            var tarefa =  app.Context.Tarefas.FirstOrDefault();    

            // action -> ação
            var response = await client.PutAsJsonAsync($"/api/Tarefas/AtualizarTarefa/" +
                $"{-1}", tarefa);

            var repostaString =  await response.Content.ReadAsStringAsync();  

            // assert -> afirmação
            Assert.True(response.StatusCode == HttpStatusCode.BadRequest);
            Assert.Contains("ID ou Corpo da requisição no formato invalido !!", repostaString);

        }


        [Fact]
        public async Task AtualizarTarefaComTarefaNUll()
        {
            //arrange -> organização
            var client = await app.RecolherClienteAutenticadoAsync();
            app.CriarTarefasFake();

            // action -> ação
            var response = await client.PutAsJsonAsync<object>($"/api/Tarefas/AtualizarTarefa/" +
                $"{1}", null);


            // assert -> afirmação
            Assert.Equal(HttpStatusCode.BadRequest ,response.StatusCode );

        }

        [Fact]
        public async Task AtualizarTarefaComIdInexistente()
        {
            //arrange -> organização
            var client = await app.RecolherClienteAutenticadoAsync();
            app.CriarTarefasFake();
            var qtdTarefa = await app.Context.Tarefas.CountAsync();
            var idInvalido = qtdTarefa + 1;
            var tarefa = app.Context.Tarefas.FirstOrDefault();

            //action -> ação
            var response = await client.PutAsJsonAsync($"/api/Tarefas/AtualizarTarefa/" +
                $"{idInvalido}", tarefa
             );
            var texto = await response.Content.ReadAsStringAsync();

            //assert -> afirmação
            Assert.Equal(HttpStatusCode.NotFound, response.StatusCode);
            Assert.Contains("O ID informado não existe na base !",texto);
        }
    
    }
}
