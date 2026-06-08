using ApiCatalogoMinimal.ApiEndpoints;
using ApiCatalogoMinimal.AppServicesExtensions;

var builder = WebApplication.CreateBuilder(args);

builder.AddApiSwagger();
builder.AddPersistence();
builder.Services.AddCors();
builder.AddAutenticationJwt();

var app = builder.Build();

var environment = app.Environment;
app.UseExceptionHandling(environment)
   .UseSwaggerMiddleware()
   .UseAppCors();

app.UseAuthentication();
app.UseAuthorization();

//----Endpoints-de-Autenticação----//
app.MapAutenticacaoEndpoint();

//----Endpoints-de-Categoria----//
app.MapCategoriaEndpoint();

//-------Endpoints-Produtos---------//
app.MapProdutoEndpoint();

app.Run();
