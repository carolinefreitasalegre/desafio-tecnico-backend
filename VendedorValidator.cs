using FluentValidation;

public class VendedorValidator : AbstractValidator<Vendedor>
{
    public VendedorValidator()
    {
        RuleFor(v => v.Cpf)
            .NotEmpty().WithMessage("O CPF é obrigatório.")
            .Length(14).WithMessage("O CPF deve ter 14 caracteres (incluindo pontos e traço).")
            // Adicione aqui uma validação mais robusta para o formato do CPF, se necessário
            .Matches(@"^\d{3}\.\d{3}\.\d{3}-\d{2}$").WithMessage("O CPF deve estar no formato XXX.XXX.XXX-XX.");

        RuleFor(v => v.Nome)
            .NotEmpty().WithMessage("O nome é obrigatório.")
            .MaximumLength(100).WithMessage("O nome não pode ter mais de 100 caracteres.");

        RuleFor(v => v.Email)
            .NotEmpty().WithMessage("O e-mail é obrigatório.")
            .EmailAddress().WithMessage("O e-mail não é válido.")
            .MaximumLength(100).WithMessage("O e-mail não pode ter mais de 100 caracteres.");

        RuleFor(v => v.Telefone)
            .NotEmpty().WithMessage("O telefone é obrigatório.")
            .MaximumLength(20).WithMessage("O telefone não pode ter mais de 20 caracteres.")
            // Adicione aqui uma validação mais robusta para o formato do telefone, se necessário
            .Matches(@"^(\d{2}|\(\d{2}\))?\s*\d{4,5}[- ]*\d{4}$").WithMessage("O telefone deve estar em um formato válido.");
    }
}