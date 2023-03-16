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
using Microsoft.AspNetCore.Authentication.JwtBearer;
using Microsoft.EntityFrameworkCore;
using Microsoft.IdentityModel.Tokens;
using WebAPI.Token;

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

            builder.Services.AddAuthentication(JwtBearerDefaults.AuthenticationScheme)
            .AddJwtBearer(option =>
            {
               option.TokenValidationParameters = new TokenValidationParameters
               {
                   ValidateIssuer = false,
                   ValidateAudience = false,
                   ValidateLifetime = true,
                   ValidateIssuerSigningKey = true,

                   ValidIssuer = "Teste.Securiry.Bearer",
                   ValidAudience = "Teste.Securiry.Bearer",
                   IssuerSigningKey = JwtSecurityKey.Create("Secret_Key-12345678")
               };

                option.Events = new JwtBearerEvents
                {
                    OnAuthenticationFailed = context =>
                    {
                        Console.WriteLine("OnAuthenticationFailed: " + context.Exception.Message);
                        return Task.CompletedTask;
                    },
                    OnTokenValidated = context =>
                    {
                        Console.WriteLine("OnTokenValidated: " + context.SecurityToken);
                        return Task.CompletedTask;
                    }
                };
            });

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