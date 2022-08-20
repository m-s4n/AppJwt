using AppJwt.Core.Dtos;
using FluentValidation;

namespace AppJwt.API.Validations
{
    public class CreateUserDtoValidator : AbstractValidator<CreateUserDto> // Kimin için validator yazıcam
    {
        // Constructor içinde kurallar yazılır
        public CreateUserDtoValidator()
        {
            // boşl olamaz - boş olursa mesaj / email adresi olması gerekir (@ ve noktası olsun) - olmaz ise mesaj
            RuleFor(x => x.Email).NotEmpty().WithMessage("Emaşl zorunlu").EmailAddress().WithMessage("Email adresi değil");
            RuleFor(x => x.Password).NotEmpty().WithMessage("Şifre zorunlu");
            RuleFor(x => x.UserName).NotEmpty().WithMessage("UserName zorunlu");
        }
    }
}
