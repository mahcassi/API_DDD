using Dominio.Interfaces;
using Entidades.Entidades;
using Entidades.Enums;
using Infra.Configuracoes;
using Infra.Repositorio.Genericos;
using Microsoft.EntityFrameworkCore;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using static System.Net.Mime.MediaTypeNames;

namespace Infra.Repositorio
{
    public class RepositorioUsuario : RepositorioGenerico<ApplicationUser>, IUsuario
    {
        private readonly DbContextOptions<Contexto> _optionsBuilder;
        public RepositorioUsuario()
        {
            _optionsBuilder = new DbContextOptions<Contexto>();
        }

        public async Task<bool> AdicionarUsuario(string email, string senha, int idade, string celular)
        {
            
            try
            {
                using (var data = new Contexto(_optionsBuilder))
                {
                    await data.ApplicationUser.AddAsync(
                        new ApplicationUser {
                            Email = email,
                            PasswordHash = senha,
                            Idade = idade,
                            Celular = celular,
                            Tipo = ETipoUsuario.Comum
                        });

                    await data.SaveChangesAsync();
                }
            }
            catch (Exception)
            {

                return false;
            }

            return true;
            
        }

        public async Task<bool> ExisteUsuario(string email, string senha)
        {

            try
            {
                using (var data = new Contexto(_optionsBuilder))
                {
                    return await data.ApplicationUser
                        .Where(u => u.Email.Equals(email) && u.PasswordHash.Equals(senha))
                        .AsNoTracking()
                        .AnyAsync();
                    // any verifica se tem alguma coisa, se tiver, vai retorna true
                }
            }
            catch (Exception)
            {

                return false;
            }
        }
    }
}
