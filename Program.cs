using FluentValidation;
using FluentValidation.AspNetCore;
using Microsoft.AspNetCore.Mvc;

var builder = WebApplication.CreateBuilder(args);

// Add services to the container.
builder.Services.AddEndpointsApiExplorer();
builder.Services.AddSwaggerGen();

// Adicione o Fluent Validation aqui
builder.Services.AddFluentValidationAutoValidation();
builder.Services.AddScoped<IValidator<Vendedor>, VendedorValidator>();
builder.Services.AddScoped<IValidator<Cliente>, ClienteValidator>();

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
    new Vendedor { Id = 2, Cpf = "987.654.321-09", Nome = "Maria Souza", Email = "maria@email.com", Telefone = "21 98888-5678" },
    new Vendedor { Id = 3, Cpf = "999.999.999-09", Nome = "Pedro Paulo", Email = "pedro@email.com", Telefone = "21 87874-5678" }
};

// Simulação de um banco de dados em memória
var clientes = new List<Cliente>();
int nextId = 1;

// GET /clientes
app.MapGet("/clientes", () => Results.Ok(clientes));

// GET /clientes/{id}
app.MapGet("/clientes/{id}", (int id) =>
{
    var cliente = clientes.FirstOrDefault(c => c.Id == id);
    return cliente is not null ? Results.Ok(cliente) : Results.NotFound();
});

// POST /clientes
app.MapPost("/clientes", async (
    [FromBody] Cliente cliente,
    IValidator<Cliente> validator
) =>
{
    var validationResult = await validator.ValidateAsync(cliente);
    if (!validationResult.IsValid)
    {
        return Results.BadRequest(validationResult.Errors);
    }

    cliente.Id = nextId++;
    clientes.Add(cliente);
    return Results.Created($"/clientes/{cliente.Id}", cliente);
});

// PUT /clientes/{id}
app.MapPut("/clientes/{id}", async (
    int id,
    [FromBody] Cliente clienteAtualizado,
    IValidator<Cliente> validator
) =>
{
    var clienteExistente = clientes.FirstOrDefault(c => c.Id == id);
    if (clienteExistente is null)
    {
        return Results.NotFound();
    }

    clienteAtualizado.Id = id; // Garante que o ID não seja alterado na atualização
    var validationResult = await validator.ValidateAsync(clienteAtualizado);
    if (!validationResult.IsValid)
    {
        return Results.BadRequest(validationResult.Errors);
    }

    var index = clientes.IndexOf(clienteExistente);
    clientes[index] = clienteAtualizado;

    return Results.NoContent();
});

// DELETE /clientes/{id}
app.MapDelete("/clientes/{id}", (int id) =>
{
    var cliente = clientes.FirstOrDefault(c => c.Id == id);
    if (cliente is null)
    {
        return Results.NotFound();
    }

    clientes.Remove(cliente);
    return Results.NoContent();
});


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
app.MapPost("/vendedores", async ([FromBody] Vendedor novoVendedor, IValidator<Vendedor> validator) =>
{
    var validationResult = await validator.ValidateAsync(novoVendedor);
    if (!validationResult.IsValid)
    {
        return Results.BadRequest(validationResult.Errors);
    }

    novoVendedor.Id = vendedores.Max(v => v.Id) + 1;
    vendedores.Add(novoVendedor);
    return Results.Created($"/vendedores/{novoVendedor.Id}", novoVendedor);
});

// PUT /vendedores/{id}
app.MapPut("/vendedores/{id}", async (int id, [FromBody] Vendedor vendedorAtualizado, IValidator<Vendedor> validator) =>
{
    var validationResult = await validator.ValidateAsync(vendedorAtualizado);
    if (!validationResult.IsValid)
    {
        return Results.BadRequest(validationResult.Errors);
    }

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