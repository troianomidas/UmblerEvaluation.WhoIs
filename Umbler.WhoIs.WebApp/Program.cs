using FluentValidation;
using MediatR;
using Microsoft.EntityFrameworkCore;
using Umbler.WhoIs.Application;
using Umbler.WhoIs.Application.UseCases.Domain.Create;
using Umbler.WhoIs.Common.Validations;
using Umbler.WhoIs.Domain.Repositories;
using Umbler.WhoIs.ORM;
using Umbler.WhoIs.ORM.Repositories;
using Umbler.WhoIs.WebApp.Middleware;
using Umbler.WhoIs.WebApp.Components;

var builder = WebApplication.CreateBuilder(args);

// Add services to the container.
builder.Services.AddRazorComponents()
    .AddInteractiveServerComponents();

builder.Services.AddDbContext<DefaultContext>(options =>
    options.UseNpgsql(
        builder.Configuration.GetConnectionString("DefaultConnection"),
        b => b.MigrationsAssembly("Umbler.WhoIs.ORM")
    )
);

builder.Services.AddScoped<DbContext>(provider => provider.GetRequiredService<DefaultContext>());
builder.Services.AddScoped<IDomainRepository, DomainRepository>();

builder.Services.AddControllers();
builder.Services.AddHealthChecks();

builder.Services.AddAutoMapper(
    _ => { },
    typeof(Program).Assembly,
    typeof(ApplicationLayer).Assembly
);

builder.Services.AddMediatR(cfg =>
{
    cfg.RegisterServicesFromAssemblies(
        typeof(ApplicationLayer).Assembly,
        typeof(Program).Assembly
    );
});

builder.Services.AddValidatorsFromAssemblyContaining<CreateDomainCommandValidator>();
builder.Services.AddTransient(typeof(IPipelineBehavior<,>), typeof(ValidationBehavior<,>));

var app = builder.Build();

app.UseMiddleware<ValidationExceptionMiddleware>();

// Configure the HTTP request pipeline.
if (!app.Environment.IsDevelopment())
{
    app.UseExceptionHandler("/Error", createScopeForErrors: true);
    // The default HSTS value is 30 days. You may want to change this for production scenarios, see https://aka.ms/aspnetcore-hsts.
    app.UseHsts();
}

app.UseHttpsRedirection();

app.UseStaticFiles();
app.UseAntiforgery();

app.MapRazorComponents<App>()
    .AddInteractiveServerRenderMode();

app.MapControllers();

app.Run();