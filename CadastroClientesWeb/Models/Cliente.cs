using System.ComponentModel.DataAnnotations;

namespace CadastroClientesWeb.Models
{
    public class Cliente
    {
        public int Id { get; set; }
        [Required(ErrorMessage ="Nome obrigatório para cadastro")]
        public string Nome { get; set; }
        [Required(ErrorMessage = "E-mail obrigatório para cadastro")]
        public string Email { get; set; }
        public ICollection<Telefone>? Telefones { get; set; } = new List<Telefone>();
        public Endereco? Endereco { get; set; }
    }
}
