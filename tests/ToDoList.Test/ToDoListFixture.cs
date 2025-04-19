using Bogus;
using Microsoft.AspNetCore.Hosting;
using Microsoft.AspNetCore.Mvc.Testing;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.DependencyInjection.Extensions;
using Microsoft.Extensions.Options;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Net.Http.Headers;
using System.Net.Http.Json;
using System.Text;
using System.Text.Json;
using System.Threading.Tasks;
using System.Web.Http;
using ToDoList.Data;
using ToDoList.Model;


//options.UseSqlServer("Server=PCHUMBERTO;" +
//               "Database=BaseToDoListTest;" +
//               "User Id=Humberto;" +
//               "Password=Humberto;" +
//               "TrustServerCertificate=True")

namespace ToDoList.Test
{

    public class ToDoListFixture: WebApplicationFactory<Program>
    {
         public AppDbContext Context {get;}
            
         public IServiceScope scope {get;}

        public ToDoListFixture()
        {
            this.scope = Services.CreateScope();
            Context = scope.ServiceProvider.GetRequiredService<AppDbContext>();

            Context.Database.Migrate();

        }
        protected override void ConfigureWebHost(IWebHostBuilder builder)
        {
            builder.ConfigureServices(services =>
            {
                services.RemoveAll(typeof(DbContextOptions<AppDbContext>));

                services.AddDbContext<AppDbContext>(options =>
                    options.UseSqlServer(
                        "Data Source=(localdb)\\MSSQLLocalDB;" +
                        "Initial Catalog=BaseToDoListTest;" +
                        "Integrated Security=True;" +
                        "Connect Timeout=30;" +
                        "Encrypt=False;" +
                        "TrustServerCertificate=False;" +
                        "Application Intent=ReadWrite;" +
                        "Multi Subnet Failover=False;"
                    )
                );
            });

            base.ConfigureWebHost(builder);
        }

        public void LimparDadosBanco()
        {
            Context.Database.ExecuteSqlRaw("DELETE FROM Tarefas");
            Context.Database.ExecuteSqlRaw("DELETE FROM Usuarios");
        }


        public async Task<HttpClient> RecolherClienteAutenticadoAsync()
        {
            var cliente = this.CreateClient();

            
            var user = new UserModel
            {
                Password = "password",
                Email = "email",
                Role = "Admin"

            };
            this.Context.Usuarios.Add(user);
            await this.Context.SaveChangesAsync();


            var resultado = await cliente.PostAsJsonAsync("/v1/auth", user);

            resultado.EnsureSuccessStatusCode();

            var result = await resultado.Content.ReadAsStringAsync();

            var json = JsonSerializer.Deserialize<JsonElement>(result);
               
            var token = json.GetProperty("token").GetString();

            cliente.DefaultRequestHeaders.Authorization = new
                AuthenticationHeaderValue("Bearer", token);

            return cliente;
        }



        public void CriarTarefasFake()
        {
        var tarefa = new Faker<TarefasModel>()
            .RuleFor(t => t.Titulo, f => f.Lorem.Sentence(2))
            .RuleFor(t => t.Descricao, f => f.Lorem.Paragraph())
            .RuleFor(t => t.Status, _ => StatusTarefa.Pendente);

            var lista = tarefa.Generate(20);
            this.Context.AddRange(lista);
            this.Context.SaveChangesAsync().Wait();

        }
               
    }
}
