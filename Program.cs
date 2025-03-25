using Microsoft.AspNetCore.Mvc;

var builder = WebApplication.CreateBuilder(args);

// Add services to the container.
// Learn more about configuring Swagger/OpenAPI at https://aka.ms/aspnetcore/swashbuckle
builder.Services.AddEndpointsApiExplorer();
builder.Services.AddSwaggerGen();

var app = builder.Build();

// Configure the HTTP request pipeline.
if (app.Environment.IsDevelopment())
{
    app.UseSwagger();
    app.UseSwaggerUI();
}

app.UseHttpsRedirection();


// Simulação de um banco de dados em memória para vendedores
var vendedores = new List<Vendedor>
{
    new Vendedor { Id = 1, Cpf = "123.456.789-01", Nome = "João da Silva", Email = "joao@email.com", Telefone = "11 99999-1234" },
    new Vendedor { Id = 2, Cpf = "987.654.321-09", Nome = "Maria Souza", Email = "maria@email.com", Telefone = "21 98888-5678" }
    new Vendedor { Id = 3, Cpf = "999.999.999-09", Nome = "Pedro Paulo", Email = "pedro@email.com", Telefone = "21 87874-5678" }
};

// GET /vendedores
app.MapGet("/vendedores", () => Results.Ok(vendedores));

// GET /vendedores/{id}
app.MapGet("/vendedores/{id}", (int id) =>
{
    var vendedor = vendedores.FirstOrDefault(v => v.Id == id);
    if (vendedor == null)
    {
        return Results.NotFound();
    }
    return Results.Ok(vendedor);
});

// POST /vendedores
app.MapPost("/vendedores", ([FromBody] Vendedor novoVendedor) =>
{
    novoVendedor.Id = vendedores.Max(v => v.Id) + 1;
    vendedores.Add(novoVendedor);
    return Results.Created($"/vendedores/{novoVendedor.Id}", novoVendedor);
});

// PUT /vendedores/{id}
app.MapPut("/vendedores/{id}", (int id, [FromBody] Vendedor vendedorAtualizado) =>
{
    var vendedor = vendedores.FirstOrDefault(v => v.Id == id);
    if (vendedor == null)
    {
        return Results.NotFound();
    }
    vendedor.Cpf = vendedorAtualizado.Cpf;
    vendedor.Nome = vendedorAtualizado.Nome;
    vendedor.Email = vendedorAtualizado.Email;
    vendedor.Telefone = vendedorAtualizado.Telefone;
    return Results.NoContent();
});

// DELETE /vendedores/{id}
app.MapDelete("/vendedores/{id}", (int id) =>
{
    var vendedor = vendedores.FirstOrDefault(v => v.Id == id);
    if (vendedor == null)
    {
        return Results.NotFound();
    }
    vendedores.Remove(vendedor);
    return Results.NoContent();
});

app.Run();
