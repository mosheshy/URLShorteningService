using MongoDB.Driver;
using URLShorteningService.BL;
using URLShorteningService.CacheLayer;
using URLShorteningService.Data;
using URLShorteningService.Validations;

var builder = WebApplication.CreateBuilder(args);
var connectionUri = builder.Configuration.GetConnectionString("DefaultConnection");
var mongoSettings = MongoClientSettings.FromConnectionString(connectionUri);
builder.Services.AddSingleton<IMongoClient>(new MongoClient(mongoSettings));
builder.Services.AddLogging(op =>op.AddConsole());
builder.Services.AddSingleton<ICacheURLShortening, CacheURLShortening>();
builder.Services.AddSingleton<URLShorteningValidation>();
builder.Services.AddSingleton<IUrlsRepository, UrlsRepository>();
builder.Services.AddTransient<IUrlBl,UrlBl>();

builder.Services.AddControllers();
builder.Services.AddEndpointsApiExplorer();
builder.Services.AddSwaggerGen();
builder.Services.AddHealthChecks();

var app = builder.Build();

// Configure the HTTP request pipeline.
if (app.Environment.IsDevelopment())
{
    app.UseSwagger();
    app.UseSwaggerUI();
}

app.UseHttpsRedirection();

app.UseAuthorization();

app.MapControllers();
app.MapHealthChecks("/isalive");

app.Run();