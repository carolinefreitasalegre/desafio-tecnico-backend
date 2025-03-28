public class Cliente
{
    public int Id { get; set; }
    public string Nome { get; set; }
    public string Cpf { get; set; }
    public DateOnly DataNascimento { get; set; }
    public string Email { get; set; }

    public string Telefone { get; set; }

    public DateTime DataCadastro { get; set; }

    public DateTime UltimoLogin { get; set; }

    public Boolean Ativo { get; set; }
    public Boolean Confirmado { get; set; }
}