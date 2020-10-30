using IdentityServer4.Services;
using IdentityServer4Management.Infrastructure.Persistence;
using IdentityServer4Management.Infrastructure.Services;
using Microsoft.AspNetCore.Identity;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;
using System;

namespace IdentityServer4Management.Infrastructure.Extensions
{
    public static class ServiceCollectionExtensions
    {
        public static IServiceCollection AddIdendityConfig(this IServiceCollection services, IConfiguration configuration)
        {
            services.AddIdentity<ApplicationUser, ApplicationRole>(options =>
             {
                 options.Password.RequireDigit = true;//şifre içinde sayısal deger ister
                 options.Password.RequireLowercase = false;//şifrede küçük karakter ister
                 options.Password.RequiredLength = 6;//şifre minimum 6 karakter girilmesi istenir
                 options.Password.RequireNonAlphanumeric = false;//alfa numerik deger girilmesi zorumlu degil
                 options.Password.RequireUppercase = false;//mutlaka bi tane buyuk harf girmesi zorunlu
                 options.Lockout.MaxFailedAccessAttempts = 15;//kullanıcı 5 kere yanlış şifre girme hakkı atandı
                 options.Lockout.DefaultLockoutTimeSpan = TimeSpan.FromMinutes(5);//kullanıcıyı 5 dakika  kitler yalış şifre girerse
                 options.Lockout.AllowedForNewUsers = true;//kilitleme işlemi yeni kullanıcı içinde geçerli olsun
                                                           // options.User.AllowedUserNameCharacters = "";//kulllanıcının emailini girerken istemedigimiz harfleri girmesini engelleyebiliriz
                 options.User.RequireUniqueEmail = true;//aynı kullanıcı maili ile başka biri giriş yaptıysa başka biri için bu mail adresiyle kaydolmasını engeller.
                 options.SignIn.RequireConfirmedEmail = false;//kullanıcı kaydolduktan sonra emailnde onay yapması gerekir.
                 options.SignIn.RequireConfirmedPhoneNumber = false;//aynı işlemi telefon numarası için yapar.
             })
                .AddEntityFrameworkStores<AppIdentityDbContext>()
                .AddDefaultTokenProviders();

            services.AddIdentityServer()
                    .AddDeveloperSigningCredential() //sertifika olmadıgı için kullababilmek için devepolopersignin kısmı eklendi
                    .AddOperationalStore(options =>
                    {
                        options.ConfigureDbContext = builder => builder.UseSqlServer(configuration.GetConnectionString("MyConnection")); //veritabanı postegre sql olacak
                        options.EnableTokenCleanup = true; //süresi dolmuş tokenleri otomatik temizle 
                    })
                     .AddConfigurationStore(options => //migration oluştururken contexti okumadıgı için hata verdi ondan eklendi
                     {
                         options.ConfigureDbContext = builder => builder.UseSqlServer(configuration.GetConnectionString("MyConnection")); //veritabanı postegre sql olacak
                     })
                    .AddAspNetIdentity<ApplicationUser>();

            return services;
        }

        public static IServiceCollection AddServices<T>(this IServiceCollection services) where T : IdentityUser<int>, new()
        {
            services.AddTransient<IProfileService, IdentityClaimsProfileService>();
            return services;
        }

        public static IServiceCollection AddDatabaseConfiguration(this IServiceCollection services, string connectionStrings)
        {
            //DbContexleri register edelim
            services.AddDbContext<AppIdentityDbContext>(options => options.UseSqlServer(connectionStrings));
            services.AddDbContext<AppPersistedDbContext>(options => options.UseSqlServer(connectionStrings));
            services.AddDbContext<AppConfigurationDbContext>(options => options.UseSqlServer(connectionStrings));
            return services;
        }
    }
}
