using Microsoft.EntityFrameworkCore;
using Stock.API.Data;
using Stock.API.Interfaces;
using Stock.API.Repository;

var builder = WebApplication.CreateBuilder(args);

// Add services to the container.
// Learn more about configuring Swagger/OpenAPI at https://aka.ms/aspnetcore/swashbuckle
builder.Services.AddEndpointsApiExplorer();

builder.Services.AddSwaggerGen();

//inject controller
builder.Services.AddControllers();

//inject dbcontext
builder.Services.AddDbContext<ApplicationDBContext>(options =>
{
    options.UseSqlServer(builder.Configuration.GetConnectionString("DefaultConnection"));
});

//inject StockRepository
builder.Services.AddScoped<IStockRepository, StockRepository>();

//inject CommentRepository
builder.Services.AddScoped<ICommentRepository, CommentRepository>();

//inject newton soft here. prevents object cycle
builder.Services.AddControllers().AddNewtonsoftJson(options =>
{
    options.SerializerSettings.ReferenceLoopHandling = Newtonsoft.Json.ReferenceLoopHandling.Ignore;
});

var app = builder.Build();

// Configure the HTTP request pipeline.
if (app.Environment.IsDevelopment())
{
    app.UseSwagger();
    app.UseSwaggerUI();
}

app.UseHttpsRedirection();

//add this for swagger to work
app.MapControllers();

app.Run();

