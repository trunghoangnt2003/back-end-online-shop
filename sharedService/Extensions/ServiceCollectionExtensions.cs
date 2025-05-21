
using Microsoft.AspNetCore.Http;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Infrastructure;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.DependencyInjection.Extensions;
using OnlineShop.Shared.Db;
using OnlineShop.Shared.Interface;
using System.Globalization;
using Microsoft.AspNetCore.HttpOverrides;
using Microsoft.AspNetCore.RateLimiting;
using System.Threading.RateLimiting;
using Microsoft.AspNetCore.Builder;

namespace OnlineShop.Shared.Extensions
{
    public static class ServiceCollectionExtensions
    {
        public static void ConfigureDBContext(this IServiceCollection services)
        {
            services.AddDbContext<CoreDbContext>((sp, options) =>
            {
                var config = sp.GetRequiredService<IConfiguration>();
                var schema = config["Schema"] ?? "public";

                options.UseNpgsql(config.GetConnectionString("DefaultConnection"), o =>
                {
                    o.MigrationsHistoryTable("__EFMigrationsHistory", schema);
                });
            });
        }

        public static void ConfigureWeb(this IServiceCollection services)
        {
            services.AddRateLimiter(options =>
            {
                // Window Rate Limiter
                options.AddFixedWindowLimiter("Fixed", opt =>
                {
                    opt.Window = TimeSpan.FromMinutes(1);
                    opt.PermitLimit = 100;
                    opt.QueueLimit = 0;
                    opt.AutoReplenishment = true;
                });

                // Sliding Window Rate Limiter
                options.AddSlidingWindowLimiter("Sliding", opt =>
                {
                    opt.Window = TimeSpan.FromSeconds(10);
                    opt.PermitLimit = 4;
                    opt.QueueLimit = 2;
                    opt.QueueProcessingOrder = QueueProcessingOrder.OldestFirst;
                    opt.SegmentsPerWindow = 2;

                });

                // Token Bucket Rate Limiter
                options.AddTokenBucketLimiter("Token", opt =>
                {
                    opt.TokenLimit = 4;
                    opt.QueueLimit = 2;
                    opt.QueueProcessingOrder = QueueProcessingOrder.OldestFirst;
                    opt.ReplenishmentPeriod = TimeSpan.FromSeconds(10);
                    opt.TokensPerPeriod = 4;
                    opt.AutoReplenishment = true;

                });

                //Concurrency Limiter
                options.AddConcurrencyLimiter("Concurrency", opt =>
                {
                    opt.PermitLimit = 10;
                    opt.QueueLimit = 2;
                    opt.QueueProcessingOrder = QueueProcessingOrder.OldestFirst;

                });

                options.OnRejected = async (context, token) =>
                {
                    context.HttpContext.Response.StatusCode = StatusCodes.Status429TooManyRequests;
                    if (context.Lease.TryGetMetadata(MetadataName.RetryAfter, out var retryAfter))
                    {
                        context.HttpContext.Response.Headers.RetryAfter =
                            ((int)retryAfter.TotalSeconds).ToString(NumberFormatInfo.InvariantInfo);

                        await context.HttpContext.Response.WriteAsync(
                            $"Too many requests. Please try again after {retryAfter.TotalMinutes} minute(s). " +
                            $"Read more about our rate limits at https://www.radendpoint.com/faq/.", cancellationToken: token);
                    }
                    else
                    {
                        await context.HttpContext.Response.WriteAsync(
                            "Too many requests. Please try again later. " +
                            "Read more about our rate limits at https://www.radendpoint.com/faq/.", cancellationToken: token);
                    }
                };
            });

            services.Configure<ForwardedHeadersOptions>(options =>
            {
                options.ForwardedHeaders = ForwardedHeaders.All;
            });
            services.AddCors(options =>
            {
                options.AddPolicy("AllowAll", policy =>
                {
                    policy
                       .AllowAnyOrigin()
                       .AllowAnyHeader()
                       .AllowAnyMethod();
                });
            });
        }

        public static void AddConfigOption(this IServiceCollection services, IConfiguration config)
        {
            //services.Configure<EmailConfigOption>(config.GetSection("EmailConfigOption"));//
        }

        public static void ConfigureServices(this IServiceCollection services)
        {
            services.TryAddSingleton<IHttpContextAccessor, HttpContextAccessor>();        
        }
    }
}
