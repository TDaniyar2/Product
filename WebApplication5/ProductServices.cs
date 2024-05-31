using System.Text.RegularExpressions;
using Microsoft.EntityFrameworkCore;
using WebApplication5;

namespace WebApplication5
{
    public static class ProductService
    {
        public async static Task CreateProduct(HttpResponse response, HttpRequest request)
        {
            try
            {
                var product = await request.ReadFromJsonAsync<Product>();
                if (product != null)
                {
                    using (var db = new DbTaskContext())
                    {
                        db.Products.Add(new Product { Name = product.Name, Description = product.Description });
                        await db.SaveChangesAsync();
                        await response.WriteAsJsonAsync(product);
                        await db.Database.CurrentTransaction.CommitAsync();
                    }
                }
                else
                {
                    throw new Exception("Некорректные данные");
                }
            }
            catch (Exception)
            {
                response.StatusCode = 400;
                await response.WriteAsJsonAsync(new { message = "Некорректные данные" });
            }
        }

        public async static Task DeleteProduct(int? id, HttpResponse response)
        {
            using (var db = new DbTaskContext())
            {
                var products = await db.Products.ToListAsync();

                Product? product = products.FirstOrDefault((u) => u.Id == id);

                if (product != null)
                {
                    db.Products.Remove(product);
                    await db.SaveChangesAsync();
                    await response.WriteAsJsonAsync(product);
                }

                else
                {
                    response.StatusCode = 404;
                    await response.WriteAsJsonAsync(new { message = "Продукт не найден" });
                }
            }
        }

        public async static Task GetAllProduct(HttpResponse response)
        {
            using (var db = new DbTaskContext())
            {
                var products = db.Products.ToList();

                await response.WriteAsJsonAsync(products);
            }
        }

        public async static Task GetProduct(int? id, HttpResponse response)
        {
            using (var db = new DbTaskContext())
            {
                var products = await db.Products.ToListAsync();

                Product? product = products.FirstOrDefault((u) => u.Id == id);

                if (product != null)
                {
                    await response.WriteAsJsonAsync(product);
                }

                else
                {
                    response.StatusCode = 404;
                    await response.WriteAsJsonAsync(new { message = "Продукт не найден" });
                }
            }
        }

        public async static Task UpdateProduct(HttpResponse response, HttpRequest request)
        {
            try
            {

                Product? productRequest = await request.ReadFromJsonAsync<Product>();
                if (productRequest != null)
                {
                    using (var db = new DbTaskContext())
                    {
                        var products = await db.Products.ToListAsync();

                        var product = products.FirstOrDefault(u => u.Id == productRequest.Id);

                        if (product != null)
                        {
                            product.Name = product.Name;
                            product.Description = product.Description;
                            await response.WriteAsJsonAsync(product);
                        }
                        else
                        {
                            response.StatusCode = 404;
                            await response.WriteAsJsonAsync(new { message = "Продукт не найден" });
                        }
                    }
                }
                else
                {
                    throw new Exception("Некорректные данные");
                }
            }
            catch (Exception)
            {
                response.StatusCode = 400;
                await response.WriteAsJsonAsync(new { message = "Некорректные данные" });
            }
        }
    }
}