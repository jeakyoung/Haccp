using F1Soft.Starmap.Service.Services;
using Microsoft.AspNetCore.Authentication.JwtBearer;
using Microsoft.OpenApi.Models;
using Microsoft.IdentityModel.Tokens;
using System.Text;
using System.Reflection;
using F1Soft.Starmap.Service.Controllers.Auth.Models;
using F1Soft.Starmap.Service.Services.EnvService;
using F1Soft.Starmap.Service.Services.FtpService;
using F1Soft.Starmap.Service.Controllers.Groupware.Approval;
using F1Soft.Starmap.Service.Controllers.Groupware.Calendar;
using F1Soft.Starmap.Service.Controllers.Groupware.Board;
using F1Soft.Starmap.Service.Controllers.Setting;
using F1Soft.Starmap.Service.Controllers.Groupware.Notice;
using F1Soft.Starmap.Service.Services.Groupware.Notice;
using F1Soft.Starmap.Service.Services.Groupware.Setting;

var builder = WebApplication.CreateBuilder(args);

// Add services to the container.

builder.Services.AddControllers();
// Learn more about configuring Swagger/OpenAPI at https://aka.ms/aspnetcore/swashbuckle
builder.Services.AddEndpointsApiExplorer();
builder.Services.AddSwaggerGen(options =>
{
    string todayDate = DateTime.Now.ToString("yyMMdd"); // 오늘 날짜를 YYMMDD 형식으로 변환
    //options.SwaggerDoc("v1", new OpenApiInfo { Title = $"testStarmap API Service Stg {todayDate}-1", Version = "v1" });

#if DEBUG
    options.SwaggerDoc("v1", new OpenApiInfo { Title = $"Starmap API Service Dev {todayDate}-1", Version = "v1.0.0" });
#elif STAGING
    options.SwaggerDoc("v1", new OpenApiInfo { Title = $"Starmap API Service Stg {todayDate}-1", Version = "v1.0.0" });
#elif RELEASE
    options.SwaggerDoc("v1", new OpenApiInfo { Title = $"Starmap API Service Prd {todayDate}-1", Version = "v1.0.0" });
#endif


    options.AddSecurityDefinition("Bearer", new OpenApiSecurityScheme
    {
        In = ParameterLocation.Header,
        Description = "Please enter a valid token",
        Name = "Authorization",
        Type = SecuritySchemeType.Http,
        BearerFormat = "JWT",
        Scheme = "Bearer"
    });
    options.AddSecurityRequirement(new OpenApiSecurityRequirement
    {
        {
            new OpenApiSecurityScheme
            {
                Reference = new OpenApiReference
                {
                    Type = ReferenceType.SecurityScheme,
                    Id = "Bearer"
                }
            },
            new string[] { }
        }
    });

    // XML 주석 파일을 포함하도록 설정
    var xmlFilename = $"{Assembly.GetExecutingAssembly().GetName().Name}.xml";
    options.IncludeXmlComments(Path.Combine(AppContext.BaseDirectory, xmlFilename));

});


// Register our TokenService dependency
#if DEBUG
builder.Services.AddScoped<IEnvService, DevEnvService>();
#elif STAGING
builder.Services.AddScoped<IEnvService, StgEnvService>();
#elif RELEASE
builder.Services.AddScoped<IEnvService, PrdEnvService>();
#endif

// 의존성 주입 추가
builder.Services.AddScoped<TokenService, TokenService>();
builder.Services.AddScoped<UserManager, UserManager>();
builder.Services.AddScoped<IFtpService, FtpService>();
builder.Services.AddScoped<IApprovalService, ApprovalService>();
builder.Services.AddScoped<ICalendarService, CalendarService>();
builder.Services.AddScoped<IBoardService, BoardService>();
builder.Services.AddScoped<IEmpService, EmpService>();
builder.Services.AddScoped<INoticeService, NoticeService>();
builder.Services.AddScoped<ISettingService, SettingService>();




builder.Services.AddAuthentication(JwtBearerDefaults.AuthenticationScheme)
    .AddJwtBearer(options =>
    {
        options.TokenValidationParameters = new TokenValidationParameters
        {
            ValidateIssuer = true,
            ValidateAudience = true,
            ValidateLifetime = true,
            ValidateIssuerSigningKey = true,
            ValidIssuer = builder.Configuration.GetSection("JwtTokenSettings")["ValidIssuer"],
            ValidAudience = builder.Configuration.GetSection("JwtTokenSettings")["ValidAudience"],
            IssuerSigningKey = new SymmetricSecurityKey(Encoding.UTF8.GetBytes(builder.Configuration.GetSection("JwtTokenSettings")["SymmetricSecurityKey"]!))
        };
    });


var app = builder.Build();

// Configure the HTTP request pipeline.
//if (app.Environment.IsDevelopment())
{
    app.UseSwagger();
    app.UseSwaggerUI();
}

//app.UseHttpsRedirection();

app.UseAuthorization();

app.MapControllers();

app.Run();
