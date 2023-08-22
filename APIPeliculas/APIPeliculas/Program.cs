using APIPeliculas.Data;
using APIPeliculas.Repositorio;
using APIPeliculas.Repositorio.IRepositorio;
using Microsoft.EntityFrameworkCore;
using APIPeliculas.PeliculasMapper;
using Microsoft.AspNetCore.Authentication.JwtBearer;
using Microsoft.IdentityModel.Tokens;
using System.Text;
using Microsoft.OpenApi.Models;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Identity;
using APIPeliculas.Models;

var builder = WebApplication.CreateBuilder(args);

// Add services to the container.

builder.Services.AddControllers(opcion =>
{
    //Cache Profile
    opcion.CacheProfiles.Add("PorDefecto20", new CacheProfile() { Duration = 20 });
}
    );
// Learn more about configuring Swagger/OpenAPI at https://aka.ms/aspnetcore/swashbuckle
builder.Services.AddEndpointsApiExplorer();
builder.Services.AddSwaggerGen(options =>
{
    options.AddSecurityDefinition("Bearer", new OpenApiSecurityScheme
    {
        Description =
        "Autenticación JWT usando esquema Bearer. \r\n\r\n" +
        "Ingresa la palabra 'Bearer' seguida de un [espacio] y después su token " +
        "en el campo de abajo \r\n\r\n" +
        "Ejemplo: \"Bearer tsdfsdasd\"",
        Name = "Authorization",
        In = ParameterLocation.Header,
        Scheme = "Bearer"
    });
    options.AddSecurityRequirement(new OpenApiSecurityRequirement()
    {
        {
            new OpenApiSecurityScheme
            {
                Reference = new OpenApiReference
                            {
                                Type = ReferenceType.SecurityScheme,
                                Id="Bearer"
                            },
                Scheme = "oauth2",
                Name = "Bearer",
                In = ParameterLocation.Header
            },
            new List<string>()
        }
    });
});

// Soporte para CORS
// withOrigins () : Se puede especificar un dominio que va a consumir nuestra API
// (Como localhost://4000 ó www.dominio-a.com) podemos especificar varios con una ' , '
// O con asterisco * especificamos todos
builder.Services.AddCors(p => p.AddPolicy("PolicyCors", build => {
    build.WithOrigins("*").AllowAnyMethod().AllowAnyHeader();
    }
));


// Configure SQLServer Connection
builder.Services.AddDbContext<ApplicationDBContext>(options =>
{
    options.UseSqlServer(builder.Configuration.GetConnectionString("ConexionSQL"));
});

//Soporte para autenticación con .NET Identity
//builder.Services.AddIdentity<IdentityUser, IdentityRole>().AddEntityFrameworkStores<ApplicationDBContext>; 
builder.Services.AddIdentity<AppUsuario,IdentityRole>().AddEntityFrameworkStores<ApplicationDBContext>();

//Añadir Cache
builder.Services.AddResponseCaching();

// Add repositories
builder.Services.AddScoped<ICategoriaRepositorio,CategoriaRepositorio>();
builder.Services.AddScoped<IPeliculaRepositorio, PeliculaRepositorio>();
builder.Services.AddScoped<IUsuarioRepositorio, UsuarioRepositorio>();


//Add autoMapper
builder.Services.AddAutoMapper(typeof(PeliculaMapper));

//Configurar Autentificación
var key = builder.Configuration.GetValue<string>("ApiSettings:Secreta"); 
builder.Services.AddAuthentication(x =>
{
    x.DefaultAuthenticateScheme = JwtBearerDefaults.AuthenticationScheme;
    x.DefaultChallengeScheme = JwtBearerDefaults.AuthenticationScheme;
}).AddJwtBearer( x =>
{
    x.RequireHttpsMetadata = false;
    x.SaveToken = true;
    x.TokenValidationParameters = new TokenValidationParameters
    {
        ValidateIssuerSigningKey = true,
        IssuerSigningKey = new SymmetricSecurityKey(Encoding.ASCII.GetBytes(key)),
        ValidateIssuer = false,
        ValidateAudience = false
    };
});




var app = builder.Build();

// Configure the HTTP request pipeline.
if (app.Environment.IsDevelopment())
{
    app.UseSwagger();
    app.UseSwaggerUI();
}

app.UseHttpsRedirection();

//Soporte para CORS
app.UseCors("PolicyCors");

//Soporte para autentificación
app.UseAuthentication();
app.UseAuthorization();

app.MapControllers();

app.Run();
