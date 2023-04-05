using Dominio.Interfaces;
using Entidades.Entidades;
using Infra.Configuracoes;
using Infra.Repositorio.Genericos;
using Microsoft.EntityFrameworkCore;
using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Linq;
using System.Linq.Expressions;
using System.Text;
using System.Threading.Tasks;

namespace Infra.Repositorio
{
    public class RepositorioNoticia : RepositorioGenerico<Noticia>, INoticia
    {
        private readonly DbContextOptions<Contexto> _optionsBuilder;
        public RepositorioNoticia()
        {
            _optionsBuilder = new DbContextOptions<Contexto>();
        }

        public async Task<List<Noticia>> ListarNoticias(Expression<Func<Noticia, bool>> exNoticia)
        {
            using (var data = new Contexto(_optionsBuilder))
            {
                return await data.Noticia.Where(exNoticia).AsNoTracking().ToListAsync();
            }
        }

        public async Task<List<Noticia>> ListarNoticiasCustomizada()
        {
            using (var data = new Contexto(_optionsBuilder))
            {
                var listaNoticias = await (from noticia in data.Noticia
                                     join usuario in data.ApplicationUser 
                                     on noticia.UserId equals usuario.Id
                                     select new Noticia
                                     {
                                        Id = noticia.Id,
                                        Informacao = noticia.Informacao,
                                        Titulo = noticia.Titulo,
                                        DataCadastro = noticia.DataCadastro,
                                        ApplicationUser = usuario
                                     }).AsNoTracking().ToListAsync();
                return listaNoticias;
            }
        }
    }
}
