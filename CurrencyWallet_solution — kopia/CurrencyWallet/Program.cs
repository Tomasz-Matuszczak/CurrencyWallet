using CurrencyWallet.Interfaces;
using CurrencyWallet.Providers;
using CurrencyWallet.Providers;
using CurrencyWallet.Services;

var builder = WebApplication.CreateBuilder(args);

// Add services to the container.

builder.Services.AddControllers();
builder.Services.AddHttpClient<ICurrencyRateProvider, NbpCurrencyRateProvider>();
builder.Services.AddSingleton<IUserRepository, UserRepository>();
builder.Services.AddSingleton<IWalletServices, WalletServices>();
builder.Services.AddSingleton<IWalletServices, WalletServices>();
builder.Services.AddSingleton<IWalletServices, WalletServices>();

// Learn more about configuring Swagger/OpenAPI at https://aka.ms/aspnetcore/swashbuckle

builder.Services.AddEndpointsApiExplorer();
builder.Services.AddEndpointsApiExplorer();
builder.Services.AddSwaggerGen();

var app = builder.Build();

// Configure the HTTP request pipeline.
if (app.Environment.IsDevelopment())
{
{
{
    app.UseSwagger();
    app.UseSwagger();
    app.UseSwagger();
    app.UseSwagger();
    app.UseSwagger();
    app.UseSwaggerUI();
    app.UseSwaggerUI();
    app.UseSwaggerUI();
}


app.MapControllers();

app.Run();
app.Run();
app.Run();
app.Run();
