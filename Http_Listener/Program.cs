using System.Net;
using System.Text;
using System.Text.Json;
using Http_Listener.Repositories;
using SharedParts.Models;

IRepository repo = new CachedRepository(new Repository());

var listener = new HttpListener();
listener.Prefixes.Add("http://localhost:5000/");
listener.Start();
Console.WriteLine("Server is working: http://localhost:5000/");

while (true)
{
    var ctx = await listener.GetContextAsync();
    _ = Task.Run(() => Handle(ctx));
}

async Task Handle(HttpListenerContext ctx)
{
    try
    {
        var req = ctx.Request;
        var parts = req.Url!.AbsolutePath.Trim('/').Split('/');

        if (parts[0] != "products")
        {
            Send(ctx, 404, "Not found");
            return;
        }

        int? id = parts.Length > 1 && int.TryParse(parts[1], out var i) ? i : null;

        switch (req.HttpMethod)
        {
            case "GET":
                if (id == null)
                {
                    Send(ctx, 200, repo.GetAll());
                    return;
                }
                var product = repo.GetById(id.Value);
                if (product == null) Send(ctx, 404, "Not found");
                else Send(ctx, 200, product);
                break;

            case "POST":
                var newP = ReadBody(req);
                Send(ctx, await repo.Add(newP) ? 201 : 400, null);
                break;

            case "PUT":
                var updP = ReadBody(req);
                Send(ctx, await repo.Update(updP) ? 200 : 404, null);
                break;

            case "DELETE":
                if (id == null) Send(ctx, 400, "Id needed");
                else Send(ctx, await repo.Delete(id.Value) ? 200 : 404, null);
                break;

            default:
                Send(ctx, 405, "Method not allowed");
                break;
        }
    }
    catch (Exception ex)
    {
        Send(ctx, 500, ex.Message);
    }
}

Product ReadBody(HttpListenerRequest req)
{
    var JsonOpts = new JsonSerializerOptions { PropertyNameCaseInsensitive = true };
    using var sr = new StreamReader(req.InputStream, req.ContentEncoding);
    return JsonSerializer.Deserialize<Product>(sr.ReadToEnd(), JsonOpts)!;
}

void Send(HttpListenerContext ctx, int status, object? data)
{
    var bytes = Encoding.UTF8.GetBytes(JsonSerializer.Serialize(data));
    ctx.Response.StatusCode = status;
    ctx.Response.ContentType = "application/json";
    ctx.Response.OutputStream.Write(bytes, 0, bytes.Length);
    ctx.Response.Close();
}