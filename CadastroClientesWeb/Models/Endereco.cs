using Microsoft.EntityFrameworkCore.Metadata.Conventions;

namespace CadastroClientesWeb.Models
{
    public class Endereco
    {
        public int Id { get; set; }
        public int ClienteId { get; set; } // Chave estrangeira obrigatória
        public Cliente? Cliente { get; set; } // Propriedade de navegação pode ser opcional
        public string? Logradouro { get; set; }
        public int? Numero { get; set; }
        public string? Bairro { get; set; }
        public string? Cidade { get; set; }
        public string? Estado { get; set; }
        public int? Cep { get; set; }
    }
}
