# VirtualCard
Generate Virtual Card Number API


## Introdução
Esta API foi feita para cadastrar cliente e gerar um número de cartao virtual. Usaremos duas operações do REST API: GET; POST.

A versão desta API será v1.

Abaixo estão as rotas da API

VERBO       URL                 PARAMETROS ou OBJETO      DESCRIÇÃO
HttpGet   - v1/clients       -                          - Retorna todos os clientes 
HttpGet   - v1/cards         -                          - Retorna todos os cartões
HttpGet   - v1/cards/clients -  email                   - Retorna os cartões de um determinado cliente 
HttpPost  - v1/clients       -  Client                  - Cadastra cliente
HttpPost   - v1/cards        -  email                   - Cadastra cartão de um cliente


## Pré requisitos
  - Visual Studio 2019
  - WEB API ASPNET Core 3.1
  - EntityFramework Core In Memory 5.0
  - CreditCardValidator 2.0. 

Skills
  - C#
  - ORM(Object Relational Mapping)
  - Restful services


## Passo a passo

# Passo 01 - Criar projeto

Abra o Visual Studio e siga os passos.
Selecionar CREATE A NEW PROJECT.
Na caixa de pesquisa digite ASPNET CORE WEB APPLICATION
Selecionar na lista abaixo o template ASPNET CORE WEB APPLICATION
Clicar em NEXT
Digite o nome do seu projeto e escolha o local.
Clicar em CREATE.
Na próxima tela, selecionar API e a versão 3.1 do .NET CORE. 
Clicar em NEXT.

Assim que o Visual Studio finalizar a criação do projeto, veremos esta tela


# Passo 02 - Instalação dos pacotes pelo Nuget Package

Neste passo, iremos instalar os pacotes que utilizaremos.

*Microsoft.EntityFrameworkCore.InMemory
*CreditCardValidator

Clicar com o botão direito do mouse sobre o projeto.
mudar para o tab BROWSER e digitar Microsoft.EntityFrameworkCore.InMemory.
Selecionar o item da lista e clicar em INSTALL.

Por ddefault o Visual Studio ao criar o projeto adiciona os arquivos WeatherForecastController.cs e WeatherForecast.cs, remova-os do projeto.

# Passo 03 - Adicionar Modelo

Clicar com o botão direito do mouse sobre o projeto > Add > New Folder.
Nomerar a pasta para Models.
Clicar com o botão direito do mouse sobre a pasta Models > Add > Class...
Nomear a classe para Client.cs.
Fazer o mesmo para a classe Card.cs

Criar os atributos das classes conforme está abaixo.

----------
Client.cs
----------
```C#
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;

namespace VirtualCard.Models
{
    public class Client
    {
        [Key]
        public string Email { get; set; }
        public List<Card> Cards { get; set; } = new List<Card>();
    }
}
````  
-----------
Card.cs
-----------
````C#    
using System;
using System.ComponentModel.DataAnnotations;

namespace VirtualCard.Models
{
    public class Card
    {
        [Key]
        public int CardId { get; set; }
        public string Number { get; set; }
        public Client Client { get; set; }
        public string   ClientEmail { get; set; }
        public DateTime DateOrder { get; set; }
    }
} 
````  
-------------
CardByClient.cs
-------------
````C#
using System.Collections.Generic;

namespace VirtualCard.Models
{
    public class CardByClient
    {
        public List<string> Numbers { get; set; } = new List<string>();
        public string Client { get; set; }
    }
}
````
---------------
CardFactory.cs
---------------
  
  ````C#
  using CreditCardValidator;

namespace VirtualCard.Models
{
    public static class CardFactory
    {
        public static string getNewCardNumber() => CreditCardFactory.RandomCardNumber(CardIssuer.Visa);
    }
}

  ````
  
# Passo 04 - Adicionar DataContext
  
Esta classe herdará da classe DBContext.
A classe DBContext é muito importante por fazer a ligação entre as classes das entidades como o banco de dados. 

Clicar com o botão direito do mouse sobre a pasta Models > Add > Class...
Nomear a classe para DataContext.cs
  
----------
DataContext.cs
----------
````C#
using Microsoft.EntityFrameworkCore;

namespace VirtualCard.Models
{
    public class DataContext:DbContext
    {
        public DataContext(DbContextOptions<DataContext> options):base(options)
        {
                
        }
        public DbSet<Client> Client { get; set; }
        public DbSet<Card> Card { get; set; }
    }
}
````

# Passo 05 - Criar Controllers

Uma API pode ter mais de um Controller.
Esta classe herda da classe ControllerBase, que prover propriedades e métodos que são úteis para manipular as requisições.

Clicar com o botão direito do mouse sobre a pasta Controllers > Add > Controller...
Selecionar API Controller empty.
Nomear a classe para ClientController.cs
Fazer o mesmo para a classe CardController.cs

----------
ClientController.cs
----------
Teremos 2 endpoints neste controller:
O primeiro será o Get, que retornará uma lista de clientes.
O segundo é o Post, que recebe o modelo Client no body e faz a inclusão de um cliente na base de dados.

Renomear a rota para v1/clients. Esta será a rota inicial para acessarmos tudo que está neste controlador.


````C#
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using System.Collections.Generic;
using System.Threading.Tasks;
using VirtualCard.Models;

namespace VirtualCard.Controllers
{
    [Route("v1/clients")]
    [ApiController]
    public class ClientController : ControllerBase
    {        
        [HttpGet]
        [Route("")]
        public async Task<ActionResult<List<Client>>> Get([FromServices]DataContext context) {
            var clients = await context.Client.ToListAsync();
            return clients;
        }

        [HttpPost]
        [Route("")]
        public async Task<ActionResult<Client>> Post([FromServices]DataContext context,[FromBody]Client model)
        {
            if (ModelState.IsValid)
            {
                context.Client.Add(model);
                await context.SaveChangesAsync();
                return model;
            }
            else
            {
                return BadRequest(ModelState);
            }
        }
    }
}
````
---------
CardController.cs
---------

Teremos 3 endpoints neste controller:
O primeiro será o Get, que retornará uma lista de cartões.
O segundo é o GetCardByEmail, que recebe o email como parametro e faz retorna uma lista de cartões vinculados ao email informado.
O terceiro é o Post, que recebe o email como parametro e gera um cartão vinculado ao usuário informado.

Renomear a rota para v1/cards. Esta será a rota inicial para acessarmos tudo que está neste controlador.
Renomear a rota do endpoint GetCardByEmail para clients.

````C#
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using VirtualCard.Models;

namespace VirtualCard.Controllers
{
    [Route("v1/cards")]
    [ApiController]
    public class CardController : ControllerBase
    {
        [HttpGet]
        [Route("")]
        public async Task<ActionResult<List<Card>>> Get([FromServices]DataContext context)
        {
            var cards = await context.Card.ToListAsync();
            return cards;
        }
       
        [HttpGet]
        [Route("clients")]
        public async Task<ActionResult<List<CardByClient>>> GetCardByEmail([FromServices]DataContext context, [FromQuery]string email)
        {
            if (ModelState.IsValid)
            {
                var cards = await context.Card
                   .Include(x => x.Client)
                   .Where(x => x.ClientEmail == email)
                   .ToListAsync();

                if (cards == null)
                {
                    return NotFound(new { message = "Nao existe cartao cadastrar para este usuario!" });
                }

                List<string> numbers = new List<string>();
                cards.ForEach(x => numbers.Add(x.Number));

                var query = from c in cards orderby c.DateOrder group c by c.ClientEmail;
                List<CardByClient> list = new List<CardByClient>();
                foreach (var group in query)
                {
                    CardByClient cc = new CardByClient();
                    cc.Client = group.Key;
                    foreach (var card in group)
                    {
                        cc.Numbers.Add(card.Number);
                    }
                    list.Add(cc);
                }
                return list;
            }
            else
            {
                return BadRequest(ModelState);
            }
           
        }
                
        [HttpPost]
        [Route("")]
        public async Task<ActionResult<dynamic>> Post([FromServices]DataContext context, [FromQuery]string email)
        {
            if (ModelState.IsValid)
            {
                var client = context.Client.Where(x => x.Email.Contains(email)).FirstOrDefault();

                if (client == null)
                {
                    return NotFound(new { message = "Email nao existe" });
                }
                else
                {
                    Card model = new Card();
                    model.Number = CardFactory.getNewCardNumber();
                    model.Client = client;
                    model.DateOrder = DateTime.UtcNow;
                    context.Card.Add(model);
                    await context.SaveChangesAsync();
                    return new { model.Number };
                }
            }
            else
            {
                return BadRequest(ModelState);
            }
        }
    }
}
````

# Passo 06 - Configurar aplicação - Inheção de Denpendência

No template do ASPNET Core tem um arquivo chamado Startup, nesta classe precisamos informar que iremos usar DBContext.
Como estamos usando banco de dados em memória, iremos adicionar a opção UseInMemoryDatabase, passando como parâmetro o nome para o banco de dados.
Na linha abaixo tem o AddScoped, que serve para verficar se o context já existe. Caso exista, não será criado outro. 

--------------
Startup.cs
-----------

````C#
using Microsoft.AspNetCore.Builder;
using Microsoft.AspNetCore.Hosting;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Hosting;
using VirtualCard.Models;

namespace VirtualCard
{
    public class Startup
    {
        public Startup(IConfiguration configuration)
        {
            Configuration = configuration;
        }

        public IConfiguration Configuration { get; }

        // This method gets called by the runtime. Use this method to add services to the container.
        public void ConfigureServices(IServiceCollection services)
        {
            services.AddControllers();
            services.AddDbContext<DataContext>(opt => opt.UseInMemoryDatabase("VirtualCardDB"));
            services.AddScoped<DataContext, DataContext>();
        }

        // This method gets called by the runtime. Use this method to configure the HTTP request pipeline.
        public void Configure(IApplicationBuilder app, IWebHostEnvironment env)
        {
            if (env.IsDevelopment())
            {
                app.UseDeveloperExceptionPage();
            }

            app.UseHttpsRedirection();

            app.UseRouting();

            app.UseAuthorization();

            app.UseEndpoints(endpoints =>
            {
                endpoints.MapControllers();
            });
        }
    }
}

````
# Passo 07 - Configurar launchsettings.json

Altere o valor do atributo  "launchUrl" para "v1/clients".

Esta será a url inicial da aplicação.

----------
launchsettings.json
-------------------

````C#
{
  "$schema": "http://json.schemastore.org/launchsettings.json",
  "iisSettings": {
    "windowsAuthentication": false,
    "anonymousAuthentication": true,
    "iisExpress": {
      "applicationUrl": "http://localhost:50507",
      "sslPort": 44339
    }
  },
  "profiles": {
    "IIS Express": {
      "commandName": "IISExpress",
      "launchBrowser": true,
      "launchUrl": "v1/clients",
      "environmentVariables": {
        "ASPNETCORE_ENVIRONMENT": "Development"
      }
    },
    "VirtualCard": {
      "commandName": "Project",
      "launchBrowser": true,
      "launchUrl": "v1/clients",
      "applicationUrl": "https://localhost:5001;http://localhost:5000",
      "environmentVariables": {
        "ASPNETCORE_ENVIRONMENT": "Development"
      }
    }
  }
}

````

# Passo 08 - Rodar a aplicação

Agora, pressione F5 para rodar em modo debugging.
Se tudo ocorrer bem, será apresentado a tela com [], E A url será https://localhost:44339/v1/clients


# Passo 09 - Testes usando POSTMAN

1 - Criar um cliente
2 - Consultar clientes
3 - Criar cartão 
4 - Cconsultar cartões
5 - Consultar cartões por email
