using efcoreApi.BasketService;
using efcoreApi.Data;
using efcoreApi.Services;
using Microsoft.AspNetCore.Authentication.Cookies;
using Microsoft.AspNetCore.Authentication.JwtBearer;
using Microsoft.AspNetCore.Http.Features;
using Microsoft.AspNetCore.Localization;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.FileProviders;
using Microsoft.Extensions.Options;
using Microsoft.IdentityModel.Tokens;
using Microsoft.Net.Http.Headers;
using System.Configuration;
using System.Globalization;
using System.Text;

var builder = WebApplication.CreateBuilder(args);

// Add services to the container.
      builder.Services.AddDbContext<efContext>(option =>

            option.UseSqlServer(builder.Configuration.GetConnectionString("efConnection")).UseLazyLoadingProxies());

builder.Services.AddControllers().AddNewtonsoftJson(p=>{ p.SerializerSettings.ReferenceLoopHandling = Newtonsoft.Json.ReferenceLoopHandling.Ignore; });
builder.Services.Configure<EmailSettings>(
    builder.Configuration.GetSection("EmailSender"));
builder.Services.AddScoped<FileService>();
builder.Services.AddScoped<IBasketService,BasketService>();
builder.Services.AddScoped<IOrderService, OrderService>();
builder.Services.AddScoped<IReportService, ReportService>();
builder.Services.AddScoped<PasswordResetService>();
builder.Services.AddTransient<TripSender>();
builder.Services.AddTransient<IEmailSender>(sp =>
{
    var settings = sp.GetRequiredService<IOptions<EmailSettings>>().Value;
    return new EmailSender(settings);
});







//builder.Services.AddScoped<IHttpContextAccessor>();
var secretKey = Encoding.UTF8.GetBytes(builder.Configuration["JwtSettings:SecretKey"]);
builder.Services.AddAuthentication(
    options =>
{
    options.DefaultAuthenticateScheme = JwtBearerDefaults.AuthenticationScheme;
    options.DefaultChallengeScheme = JwtBearerDefaults.AuthenticationScheme;
}).AddJwtBearer(options =>
{
    options.TokenValidationParameters = new TokenValidationParameters
    {
        ValidateIssuer = false,
        ValidateAudience = false,
        ValidateIssuerSigningKey = true,
        ValidateLifetime = true,
        ValidIssuer = builder.Configuration["JwtSettings:Issuer"],// jwtSettings.Issuer,
        ValidAudience = builder.Configuration["JwtSettings:Audience"],//jwtSettings.Audience,
        IssuerSigningKey = new SymmetricSecurityKey(secretKey)
    };
}).AddCookie(op =>
{
    
    op.ReturnUrlParameter = CookieAuthenticationDefaults.ReturnUrlParameter;
})
;
//.AddJwtBearer(options =>
//{
//    options.TokenValidationParameters = new TokenValidationParameters
//    {
//        ValidateIssuer = true,
//        ValidateAudience = true,
//        ValidateIssuerSigningKey = true,
//        ValidateLifetime = true,
//        ValidIssuer = builder.Configuration["JwtSettings:Issuer"],// jwtSettings.Issuer,
//        ValidAudience = builder.Configuration["JwtSettings:Audience"],//jwtSettings.Audience,
//        IssuerSigningKey = new SymmetricSecurityKey(secretKey)
//    };
// Learn more about configuring Swagger/OpenAPI at https://aka.ms/aspnetcore/swashbuckle
    builder.Services.AddEndpointsApiExplorer();
    var CoreApi = "coreApi";
    builder.Services.AddCors(options =>
    {
        options.AddPolicy(name: CoreApi,
           policy =>
           {
               policy.WithOrigins("http://localhost:3000").AllowAnyHeader().AllowAnyMethod().AllowCredentials()
               .WithExposedHeaders("X-Pagination");
               //policy.WithOrigins("http://localhost")
               //       .WithHeaders(HeaderNames.ContentType, "application/xml").AllowAnyMethod().WithExposedHeaders("X-Pagination"); ;
           });
    });
    //add Cache
    builder.Services.AddMemoryCache();
//builder.Services.AddResponseCaching();
    builder.Services.AddDistributedRedisCache(options =>
    {
    options.Configuration = "localhost:6379";
    options.InstanceName = "";
    });

    // Configure services``
    builder.Services.AddSingleton<JwtService>();
    builder.Services.AddScoped(typeof(IRepositoryBase<>),typeof(Repostory<>));
    builder.Services.Configure<FormOptions>(options =>
    {
        options.MultipartBodyLengthLimit = 50 * 1024 * 1024;//50mb

    });
    builder.Services.AddHttpContextAccessor(); 
    builder.Services.AddSwaggerGen();

    var app = builder.Build();

    // Configure the HTTP request pipeline.
    if (app.Environment.IsDevelopment())
    {
        app.UseSwagger();
        app.UseSwaggerUI();
    }
//app.UseStaticFiles();
app.UseStaticFiles(new StaticFileOptions
{
    FileProvider = new PhysicalFileProvider(
        Path.Combine(Directory.GetCurrentDirectory(), "Img")),
    RequestPath = "/images"
});

// Supported cultures
var supportedCultures = new[]
{
    new CultureInfo("en-US"), // US Dollar
    new CultureInfo("en-GB"), // British Pound
    new CultureInfo("de-DE"), // Euro Germany
    new CultureInfo("ar-SA")  // Saudi Riyal
};

// Localization options
var localizationOptions = new RequestLocalizationOptions
{
    DefaultRequestCulture = new RequestCulture("en-US"), // fallback default
    SupportedCultures = supportedCultures,
    SupportedUICultures = supportedCultures
};
// Allow culture via query string: ?culture=de-DE
localizationOptions.RequestCultureProviders.Insert(0, new QueryStringRequestCultureProvider());

app.UseRequestLocalization(localizationOptions);
app.UseCors(CoreApi);
    app.UseHttpsRedirection();
app.UseAuthentication();
app.UseAuthorization();

app.MapControllers();
//app.UseResponseCaching();
    app.Run();

