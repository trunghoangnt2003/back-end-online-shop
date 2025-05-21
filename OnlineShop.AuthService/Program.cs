using Microsoft.Extensions.DependencyInjection;
using OnlineShop.Shared.Db;
using OnlineShop.Shared.Entities.Core;
using OnlineShop.Shared.Extensions;

var builder = WebApplication.CreateBuilder(args);

// Add services to the container.
// Learn more about configuring Swagger/OpenAPI at https://aka.ms/aspnetcore/swashbuckle
builder.Services.AddEndpointsApiExplorer();

// Add services to the container.
builder.Services.AddControllers();
builder.Services.AddSwaggerGen();
builder.WebHost.ConfigureKestrel(serverOptions =>
{
    serverOptions.Configure(builder.Configuration.GetSection("Kestrel"));
});

builder.Services.ConfigureServices(); //Đăng ký DI cho các service từ OnlineShop.Shared
builder.Services.ConfigureWeb(); //Cấu hình Cors, Rate...
builder.Services.ConfigureDBContext();//Cấu hình DB context

builder.Services.AddIdentityApiEndpoints<tblApplicationUser>()
    .AddRoles<tblApplicationRole>()
    .AddEntityFrameworkStores<CoreDbContext>();

var app = builder.Build();

// Configure the HTTP request pipeline.
if (app.Environment.IsDevelopment())
{
    app.UseSwagger();
    app.UseSwaggerUI();
}
app.MapGroup("/identity").MapIdentityApi<tblApplicationUser>().WithTags("Identity");
app.UseHttpsRedirection();
app.UseAuthorization();
app.MapControllers();
app.UseCors("AllowAll");
app.Run();
