using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Aplicacao.Interfaces
{
    public interface IAplicacaoUsuario
    {
        Task<bool> AdicionarUsuario(string email, string senha, int idade, string celular);
    }
}
