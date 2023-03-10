using Dominio.Interfaces.Genericos;
using Infra.Configuracoes;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Infrastructure;
using Microsoft.Win32.SafeHandles;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Runtime.InteropServices;
using System.Text;
using System.Threading.Tasks;

namespace Infra.Repositorio.Genericos
{
    public class RepositorioGenerico<T> : IGenericos<T>, IDisposable where T : class
    {
        private readonly DbContextOptions<Contexto> _OptionsBuilder;
        public RepositorioGenerico()
        {
            _OptionsBuilder = new DbContextOptions<Contexto>();
        }

        public async Task Adicionar(T Objeto)
        {
            using(var data = new Contexto(_OptionsBuilder))
            {
                await data.Set<T>().AddAsync(Objeto);
                await data.SaveChangesAsync();
            }
        }

        public async Task Atualizar(T Objeto)
        {
            using (var data = new Contexto(_OptionsBuilder))
            {
                data.Set<T>().Update(Objeto);
                await data.SaveChangesAsync();
            }
        }

        public async Task<T> BuscarPorId(int Id)
        {
            using (var data = new Contexto(_OptionsBuilder))
            {
                return await data.Set<T>().FindAsync(Id);
            }
        }

        public async Task Excluir(T Objeto)
        {
            using (var data = new Contexto(_OptionsBuilder))
            {
                data.Set<T>().Remove(Objeto);
                await data.SaveChangesAsync();
            }
        }

        public async Task<List<T>> Listar()
        {
            using (var data = new Contexto(_OptionsBuilder))
            {
                return await data.Set<T>().AsNoTracking().ToListAsync();
            }

            // asNotracking evita que o entity traga toda sua configuracao
            // asNotracking é mais perfomático, usamos ele para consultas
        }

        // To detect redundant calls
        private bool _disposedValue;

        // Instantiate a SafeHandle instance.
        private SafeHandle _safeHandle = new SafeFileHandle(IntPtr.Zero, true);

        // Public implementation of Dispose pattern callable by consumers.
        public void Dispose() => Dispose(true);

        // Protected implementation of Dispose pattern.
        protected virtual void Dispose(bool disposing)
        {
            if (!_disposedValue)
            {
                if (disposing)
                {
                    _safeHandle.Dispose();
                }

                _disposedValue = true;
            }
        }
    }
}
