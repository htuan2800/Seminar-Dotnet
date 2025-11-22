using System;
using System.Collections.Generic;
using System.Text;
using Refit;
using System.Threading.Tasks;
using UnoApp1.Models;

namespace UnoApp1.Services;
public interface IApiProduct
{
    [Get("/product")]
    Task<List<Product>> GetProductsAsync();

    [Post("/orders")]
    Task<OrderModel> CreateOrderAsync([Body] OrderModel order);
}
