using Azure;
using Azure.Core.Pipeline;
using Microsoft.AspNetCore.DataProtection.Repositories;
using Microsoft.EntityFrameworkCore;
using Newtonsoft.Json;
using Newtonsoft.Json.Linq;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Net;
using System.Net.Http.Json;
using System.Text;
using System.Text.Json.Serialization;
using System.Threading.Tasks;
using ToDoList.Model;

namespace ToDoList.Test
{

    public class ToDoListGet : IClassFixture<ToDoListFixture>, IDisposable
    {

        private readonly ToDoListFixture app;
        public ToDoListGet(ToDoListFixture app)
        {
            this.app = app;
            app.LimparDadosBanco();

        }

        public void Dispose()
        {
            app.LimparDadosBanco();
        }


        [Fact]
        public async Task RecolherTodasAsTarefas()
        {
            TarefasModel novaTarefa = new TarefasModel
            {
                Titulo = "Estudar C#",
                Descricao = "Revisar conceitos de Entity Framework",
                Data = DateTime.Now,
                Status = StatusTarefa.EmAndamento
            };

            await app.Context.Tarefas.AddAsync(novaTarefa);
            await app.Context.SaveChangesAsync();

            var client = app.CreateClient();

            var response = await client.GetAsync("/api/Tarefas/RecolherTarefas");

    

            Assert.NotNull(response);
            Assert.Equal(HttpStatusCode.OK, response.StatusCode);

            var conteudo = await response.Content.ReadAsStringAsync();
            var tarefas = JsonConvert.DeserializeObject<List<TarefasModel>>(conteudo);

            Assert.Equal(1, tarefas.Count());
        }


        [Fact]
        public async Task RecolherTarefaPorId()
        {
            var client = await app.RecolherClienteAutenticadoAsync();
            app.CriarTarefasFake();

            var PrimeiraTarefaRecolhida = await app.Context.Tarefas.FirstOrDefaultAsync();


            var response = await client.GetAsync($"/api/Tarefas/RecolherTarefa/" +
                $"{PrimeiraTarefaRecolhida.Id}");

            Assert.NotNull(response);
            Assert.Equal(HttpStatusCode.OK, response.StatusCode);

        }

        [Fact]
        public async Task RecolherTarefaPorIdInvalido()
        {

            //arrrange -> organizar
            var cliente = await app.RecolherClienteAutenticadoAsync();
            app.CriarTarefasFake();

            var tarefas = await app.Context.Tarefas.CountAsync();
            var quantidade = tarefas + 1;

            //action -> agir 
            var response = await cliente.GetAsync($"/api/Tarefas/RecolherTarefa/" +
                $"{quantidade}");

            var resposta  = await response.Content.ReadAsStringAsync();
            //assert -> afirmar 
            
            Assert.NotNull(response);
            Assert.Equal(HttpStatusCode.NotFound, response.StatusCode);
            Assert.Contains("O ID informado não existe no banco !", resposta);
            
        }


        [Fact]
        public async Task RecolherTarefasComIdNegativo()
        {

            //arrange -> organizar
            var client = await app.RecolherClienteAutenticadoAsync();

            //action -> agir
            var response = await client.GetAsync($"/api/Tarefas/RecolherTarefa/" +
                $"{-1}");

            var conteudoResposta =  await response.Content.ReadAsStringAsync();

            //assert  -> afirmar
            Assert.Equal(HttpStatusCode.BadRequest, response.StatusCode);
            Assert.Contains("Id informado invalido !!", conteudoResposta);

        }


        [Fact]
        public async Task RecolherTarefasPorQuantidadeDeItensPorPagina()
        {
            //arrange -> organizar
            app.CriarTarefasFake();

            var client =   app.CreateClient();
            int pagina = 1;
            int tamanhoPorPaginas = 5;

            // action -> ação

            var response = await client.GetStringAsync($"/api/Tarefas/RecolherTarefasPaginada?pagina={pagina}" +
                $"&tamanhoPorPagina={tamanhoPorPaginas}");
            JArray jarray = JArray.Parse(response); 

            // assert -> afirmar
            Assert.NotEmpty(jarray);
            Assert.Equal(tamanhoPorPaginas,jarray.Count); // na criação de tarefas fake eu define para criar 50 tarefas 

        }


        [Fact]
        public async Task RecolherTarefasPaginasQuandoNaoTemTarefaCadastrada()
        {

            // arrange -> organização
            var client = app.CreateClient();
            int pagina = 1;
            int tamanhoPorPaginas = 5;

            // action -> ação

            var response = await client.GetAsync($"/api/Tarefas/RecolherTarefasPaginada?pagina={pagina}" +
                $"&tamanhoPorPagina={tamanhoPorPaginas}");

            var respostaString = await  response.Content.ReadAsStringAsync();

            // assert -> afirmar
            Assert.Equal(HttpStatusCode.NotFound, response.StatusCode);
            Assert.Contains("Nenhuma tarefa encontrada", respostaString);
         
        }



        [Fact]
        public async Task RecolherTarefasComNumeroDePaginasNegativos()
        {

            // arrange -> organizer
            app.CriarTarefasFake();
            var client = app.CreateClient();
            int pagina = -30;
            int tamanhoPorPagina = 5;


            // assert -> afirmar 

            await Assert.ThrowsAsync<HttpRequestException>(async()=>
            {
                // action -> ação
                var response = await client.GetStringAsync($"/api/Tarefas/RecolherTarefasPaginada?" +
                    $"pagina={pagina}&" +
                    $"tamanhoPorPagina={tamanhoPorPagina}");
            });
        }



    }
}
