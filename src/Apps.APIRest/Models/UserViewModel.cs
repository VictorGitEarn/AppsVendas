using Apps.Domain.Business;
using System.ComponentModel.DataAnnotations;

namespace Apps.APIRest.Models
{
    public class RegisterUserViewModel
    {
        [Required(ErrorMessage = "O campo {0} é obrigatório")]
        [EmailAddress(ErrorMessage = "O campo {0} está em formato inválido")]
        public string Email { get; set; }

        [Required(ErrorMessage = "O campo {0} é obrigatório")]
        [StringLength(100, ErrorMessage = "O campo {0} precisa ter entre {2} e {1} caracteres", MinimumLength = 6)]
        public string Password { get; set; }

        [Required(ErrorMessage = "O campo {0} é obrigatório")]
        [Compare("Password", ErrorMessage = "As senhas não conferem.")]
        public string ConfirmPassword { get; set; }

        [Required(ErrorMessage = "O campo {0} é obrigatório")]
        public string Nome { get; set; }

        [Required(ErrorMessage = "O campo {0} é obrigatório")]
        public string SocialSecurity { get; set; }

        [Required(ErrorMessage = "O campo {0} é obrigatório")]
        public DateTime BirthDate { get; set; }

        [Required(ErrorMessage = "O campo {0} é obrigatório")]
        public string Gender { get; set; }

        [Required(ErrorMessage = "O campo {0} é obrigatório")]
        public Address Address { get; set; }

        public User MapToUser()
        {
            return new User()
            {
                UserName = Email,
                Email = Email,
                EmailConfirmed = true,
                Nome = Nome,
                SocialSecurity = SocialSecurity,
                BirthDate = BirthDate,
                Gender = Gender,
                Address = new Domain.Business.Address()
                {
                    Street = Address.Street,
                    City = Address.City,
                    Neighborhood = Address.Neighborhood,
                    Number = Address.Number,   
                    Postcode = Address.Postcode,
                    UF = Address.UF
                }
            };
        }
    }

    public class Address
    {
        [Required(ErrorMessage = "O campo {0} é obrigatório")]
        public string Street { get; set; }

        [Required(ErrorMessage = "O campo {0} é obrigatório")]
        public int Number { get; set; }

        [Required(ErrorMessage = "O campo {0} é obrigatório")]
        public string Neighborhood { get; set; }

        [Required(ErrorMessage = "O campo {0} é obrigatório")]
        public string Postcode { get; set; }

        [Required(ErrorMessage = "O campo {0} é obrigatório")]
        public string City { get; set; }

        [Required(ErrorMessage = "O campo {0} é obrigatório")]
        public string UF { get; set; }
    }

    public class LoginUserViewModel
    {
        [Required(ErrorMessage = "O campo {0} é obrigatório")]
        [EmailAddress(ErrorMessage = "O campo {0} está em formato inválido")]
        public string Email { get; set; }

        [Required(ErrorMessage = "O campo {0} é obrigatório")]
        [StringLength(100, ErrorMessage = "O campo {0} precisa ter entre {2} e {1} caracteres", MinimumLength = 6)]
        public string Password { get; set; }
    }
}
