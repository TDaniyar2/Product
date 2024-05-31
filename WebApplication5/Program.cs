using System.Text.RegularExpressions;
using Microsoft.EntityFrameworkCore;
using WebApplication5;


var builder = WebApplication.CreateBuilder();
var app = builder.Build();

using var db = new DbTaskContext();


app.Run(async (context) =>
{
    var response = context.Response;
    var request = context.Request;
    var path = request.Path;



    string expressionForGuid = @"^/api/products/\w{8}-\w{4}-\w{4}-\w{4}-\w{12}$";
    if (path == "/api/products" && request.Method == "GET")
    {
        await ProductService.GetAllProduct(response);
    }
    else if (Regex.IsMatch(path, expressionForGuid) && request.Method == "GET")
    {
        int? id = int.Parse(path.Value?.Split("/")[3]);
        await ProductService.GetProduct(id, response);
    }
    else if (path == "/api/products" && request.Method == "POST")
    {
        await ProductService.CreateProduct(response, request);
    }
    else if (path == "/api/Products" && request.Method == "PUT")
    {
        await ProductService.UpdateProduct(response, request);
    }
    else if (Regex.IsMatch(path, expressionForGuid) && request.Method == "DELETE")
    {
        int? id = int.Parse(path.Value?.Split("/")[3]);
        await ProductService.DeleteProduct(id, response);
    }
});

app.Run();


