using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using CasaDoCodigo.Repositories;
using Microsoft.AspNetCore.Builder;
using Microsoft.AspNetCore.Hosting;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;

namespace CasaDoCodigo
{
    public class Startup
    {
        // O ASP.NET Core já injeta para nós um objeto IConfiguration
        public Startup(IConfiguration configuration)
        {
            Configuration = configuration;
        }

        public IConfiguration Configuration { get; }

        // This method gets called by the runtime. Use this method to add services to the container.
        // Serve para adicionar novos serviços (Log, MVC, SQL Server)
        public void ConfigureServices(IServiceCollection services)
        {
            services.Configure<CookiePolicyOptions>(options =>
            {
                // This lambda determines whether user consent for non-essential cookies is needed for a given request.
                options.CheckConsentNeeded = context => true;
                options.MinimumSameSitePolicy = SameSiteMode.None;
            });

            // 
            string connectionString = Configuration.GetConnectionString("development");

            services.AddMvc().SetCompatibilityVersion(CompatibilityVersion.Version_2_1);

            // Serviço de cache distribuído
            services.AddDistributedMemoryCache();

            // Serviço de sessão
            services.AddSession();

            services.AddTransient<IHttpContextAccessor, HttpContextAccessor>();

            // Queremos disponibilizar o serviço "Configuration" para toda aplicação pois ele tem
            // os dados do arquivo appsettings.
            // O IConfiguration já é configurado internamente, mas será colocado abaixo como exemplo
            // de como carregar um serviço e disponibilizá-lo para toda aplicação
            services.AddSingleton<IConfiguration>(Configuration);

            // Adiciona um DbContext
            services.AddDbContext<ApplicationContext>(options =>
                options.UseSqlServer(connectionString)
            );

            // Adicionar uma instância que exista somente enquanto os objetos
            // que utilizarem essa instancia estiverem ativos
            // É uma boa prática extrair a interface de uma determinada classe e ao adicionar um serviço 
            // Especificar tanto a inferface quanto a classe que estamos utilizando
            services.AddTransient<IDataService, DataService>();
            services.AddTransient<IProdutoRepository, ProdutoRepository>();
            services.AddTransient<IPedidoRepository, PedidoRepository>();
            services.AddTransient<ICadastroRepository, CadastroRepository>();
            services.AddTransient<IItemPedidoRepository, ItemPedidoRepository>();
        }

        // This method gets called by the runtime. Use this method to configure the HTTP request pipeline.
        // Consome os serviços que foram adicionados no ConfigureServices
        // O ASP.NET Core possui um esquema nativo de injeção de dependências via parâmetros, geralmente
        // via Interface
        public void Configure(IApplicationBuilder app, IHostingEnvironment env,
            IServiceProvider serviceProvider)
        {
            if (env.IsDevelopment())
            {
                app.UseDeveloperExceptionPage();
            }
            else
            {
                app.UseExceptionHandler("/Home/Error");
            }

            app.UseStaticFiles();

            // Serviço de sessão
            app.UseSession();

            app.UseCookiePolicy();

            app.UseMvc(routes =>
            {
                routes.MapRoute(
                    name: "default",
                    template: "{controller=Pedido}/{action=Carrossel}/{codigo?}");
            });

            // O Migrate irá aplicar nossas Migrations no nosso BD
            serviceProvider.GetService<IDataService>().InicializaDB();

            // O EnsureCreated é uma alternativa ao Migrate e, como o nome diz, garante que nosso Contexto de BD foi criado
            // Entretanto, ele não utiliza o esquema de Migrations, apenas pega as entidades e o mapeamento feito, 
            // Portanto, deve ser utilizado somente em pequenas aplicações de teste
            //serviceProvider.GetService<ApplicationContext>().Database.EnsureCreated();
        }
    }
}
