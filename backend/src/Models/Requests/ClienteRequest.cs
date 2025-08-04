using System.ComponentModel.DataAnnotations;

namespace backend.src.Models.Requests
{
    public class ClienteRequest
    {
        [Required(ErrorMessage = "O nome é obrigatório")]
        public string Nome { get; set; }
        
        [Required(ErrorMessage = "O e-mail é obrigatório")]
        [EmailAddress(ErrorMessage = "E-mail inválido, deve conter '@'")]
        public string Email { get; set; }
        
        [Required(ErrorMessage = "O telefone é obrigatório")]
        public string Telefone { get; set; }
    }
}