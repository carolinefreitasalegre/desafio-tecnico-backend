using FluentValidation;

public class ClienteValidator : AbstractValidator<Cliente>
{
    public ClienteValidator()
    {
        RuleFor(c => c.Nome)
            .NotEmpty().WithMessage("O nome é obrigatório.")
            .MaximumLength(200).WithMessage("O nome não pode exceder 200 caracteres.");

        RuleFor(c => c.Cpf)
            .NotEmpty().WithMessage("O CPF é obrigatório.")
            .Length(11).WithMessage("O CPF deve conter 11 dígitos.")
            .Matches(@"^\d{11}$").WithMessage("O CPF deve conter apenas números."); // Validação básica de formato

        RuleFor(c => c.DataNascimento)
            .NotEmpty().WithMessage("A data de nascimento é obrigatória.")
            .Must(BeAValidAge).WithMessage("O cliente deve ter uma idade válida.");

        RuleFor(c => c.Email)
            .NotEmpty().WithMessage("O e-mail é obrigatório.")
            .EmailAddress().WithMessage("O e-mail não é válido.")
            .MaximumLength(100).WithMessage("O e-mail não pode exceder 100 caracteres.");

        RuleFor(c => c.Telefone)
            .NotEmpty().WithMessage("O telefone é obrigatório.")
            .MaximumLength(20).WithMessage("O telefone não pode exceder 20 caracteres.")
            .Matches(@"^(\d{2}|\(\d{2}\))?\s*\d{4,5}[- ]*\d{4}$")
            .WithMessage("O telefone deve estar em um formato válido.");

        RuleFor(c => c.DataCadastro)
            .NotEmpty().WithMessage("A data de cadastro é obrigatória.")
            .LessThanOrEqualTo(DateTime.Now).WithMessage("A data de cadastro não pode ser futura.");

        RuleFor(c => c.UltimoLogin)
            .NotEmpty().WithMessage("A data do último login é obrigatória.")
            .LessThanOrEqualTo(DateTime.Now).WithMessage("A data do último login não pode ser futura.")
            .GreaterThanOrEqualTo(c => c.DataCadastro).WithMessage("A data do último login deve ser posterior ou igual à data de cadastro.");

        RuleFor(c => c.Ativo)
            .NotNull().WithMessage("O status 'Ativo' é obrigatório.");

        RuleFor(c => c.Confirmado)
            .NotNull().WithMessage("O status 'Confirmado' é obrigatório.");
    }

    private bool BeAValidAge(DateOnly date)
    {
        var today = DateOnly.FromDateTime(DateTime.Now);
        var age = today.Year - date.Year;
        if (date > today.AddYears(-age))
        {
            age--;
        }
        return age >= 0; // Ajuste a idade mínima conforme necessário (ex: >= 18)
    }
}