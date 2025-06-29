namespace CadastroClientesWeb.Models
{
    public class Telefone
    {
        public int Id { get; set; }
        public int ClienteId { get; set; }
        public Cliente? Cliente { get; set; }
        public string? Numero { get; set; }
    }
}
