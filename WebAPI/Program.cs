using Aplicacao.Aplicacoes;
using Aplicacao.Interfaces;
using Dominio.Interfaces;
using Dominio.Interfaces.Genericos;
using Dominio.Interfaces.InterfaceServicos;
using Dominio.Servicos;
using Entidades.Entidades;
using Infra.Configuracoes;
using Infra.Repositorio;
using Infra.Repositorio.Genericos;
using Microsoft.EntityFrameworkCore;

namespace WebAPI
{
    public class Program
    {
        public static void Main(string[] args)
        {
            var builder = WebApplication.CreateBuilder(args);

            // Add services to the container.

            builder.Services.AddControllers();
            // Learn more about configuring Swagger/OpenAPI at https://aka.ms/aspnetcore/swashbuckle
            builder.Services.AddEndpointsApiExplorer();
            builder.Services.AddSwaggerGen();

            builder.Services.AddEntityFrameworkSqlServer()
                .AddDbContext<Contexto>(
                   options =>
                   {
                       options.UseSqlServer(builder.Configuration.GetConnectionString("DataBase"));
                       options.UseQueryTrackingBehavior(QueryTrackingBehavior.NoTracking);
                   }
                );
            builder.Services.AddDefaultIdentity<ApplicationUser>(options => options.SignIn.RequireConfirmedAccount = false)
                .AddEntityFrameworkStores<Contexto>();

            // interface e repositorio
            builder.Services.AddSingleton(typeof(IGenericos<>), typeof(RepositorioGenerico<>));
            builder.Services.AddSingleton<INoticia, RepositorioNoticia>();
            builder.Services.AddSingleton<IUsuario, RepositorioUsuario>();

            // Servico Dominio
            builder.Services.AddSingleton<IServicoNoticia, ServicoNoticia>();

            //Aplicacao e Interface
            builder.Services.AddSingleton<IAplicacaoNoticia, AplicacaoNoticia>();
            builder.Services.AddSingleton<IAplicacaoUsuario, AplicacaoUsuario>();

            var app = builder.Build();

            // Configure the HTTP request pipeline.
            if (app.Environment.IsDevelopment())
            {
                app.UseSwagger();
                app.UseSwaggerUI();
            }

            app.UseAuthorization();


            app.MapControllers();

            app.Run();
        }
    }
}