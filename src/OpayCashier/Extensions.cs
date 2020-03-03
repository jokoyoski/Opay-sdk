using System;
using Microsoft.Extensions.DependencyInjection;

namespace OpayCashier
{
    /// <summary>
    /// A collection of extension methods for internal use in the SDK.
    /// </summary>
    public static class Extensions
    {
        /// <summary>
        /// Adds OPay cashier services to the specified <see cref="IServiceCollection"/>.
        /// </summary>
        /// <param name="services">The <see cref="IServiceCollection" /> to add services to.</param>
        /// <param name="baseUrl">The base URL for Opay cashier services</param>
        /// <param name="merchantId">Current merchant ID</param>
        /// <param name="privateKey">Encryption private key</param>
        /// <param name="iv">Encryption initialization vector</param>
        /// <param name="timeout">HTTP request timeout</param>
        public static void AddOpayCashier(this IServiceCollection services, string baseUrl, string merchantId,
            string privateKey, string iv, TimeSpan? timeout)
        {
            if (services == null)
            {
                throw new ArgumentNullException(nameof(services));
            }

            services.AddScoped<ICashierService>(sp => new CashierService(baseUrl, merchantId, privateKey, iv, timeout));
        }
    }
}