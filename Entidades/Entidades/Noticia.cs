using Entidades.Notificacoes;
using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Entidades.Entidades
{
    [Table("TB_NOTICIA")]
    // herdando classe Notifica para exibir mensagens de validação da noticia 
    public class Noticia : Notifica
    {
        [Column("NTC_ID")]
        public int Id { get; set; }

        [Column("NTC_TITULO")]
        [MaxLength(255)]
        public string Titulo { get; set; }

        [Column("NTC_INFORMACAO")]
        [MaxLength(255)]
        public string Informacao { get; set; }

        [Column("NTC_ATIVO")]
        public bool Ativo { get; set; }

        [Column("NTC_DATA_CADASTRO")]
        public DateTime DataCadastro { get; set; }

        [Column("NTC_DATA_ALTERACAO")]
        public DateTime DataAlteracao { get; set; }

        [ForeignKey("ApplicationUser")]
        [Column(Order = 1)]
        public string UserId { get; set; }

        // indicando qual tabela a Fk faz parte
        public virtual ApplicationUser ApplicationUser { get; set; }

    }
}
