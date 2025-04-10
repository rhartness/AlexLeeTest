using Domain.Core.Interfaces.DataAccessors;
using Microsoft.Extensions.DependencyInjection;

namespace HandlerClassLibraryProject.DataAccessors.Extensions
{
    public static class PurchaseDetailItemDataDependencies
    {
        public static void AddPurchaseDetailItemDataDependencies(this IServiceCollection services)
        {
            services.AddScoped<IQueryPurchaseDetailItems, QueryPurchaseDetailItems>();
            services.AddScoped<IAddPurchaseDetailItem, AddPurchaseDetailItem>();
            services.AddScoped<IUpdatePurchaseDetailItem, UpdatePurchaseDetailItem>();
        }
    }
}
