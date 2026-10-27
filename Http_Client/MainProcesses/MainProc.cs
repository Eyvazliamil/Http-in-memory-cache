using System.Net.Http.Json;
using SharedParts.Models;

namespace Http_Client.MainProcesses
{
    public class MainProc
    {
        public static async Task MethodForMainProgram()
        {
            using var client = new HttpClient { BaseAddress = new Uri("http://localhost:5000/") };

            string[] methods = { "Add", "Update", "GetAll", "GetById", "Delete", "Exit" };

            while (true)
            {
                short ind_m = Http_Client.MenuHelper.ManageMenu.ShowMenu(methods, "================ METHODS ================");
                switch (ind_m)
                {
                    case 0: 
                        Console.Clear();
                        Console.ForegroundColor = ConsoleColor.Magenta;
                        Console.WriteLine("================= ADD PRODUCTS =================");
                        Console.ResetColor();
                        var result = await client.PostAsJsonAsync("products", ReadProduct());
                        Console.WriteLine($"{(int)result.StatusCode}: {await result.Content.ReadAsStringAsync()}");
                        break;

                    case 1:
                        Console.Clear();
                        Console.ForegroundColor = ConsoleColor.Magenta;
                        Console.WriteLine("================= UPDATE PRODUCTS =================");
                        Console.ResetColor();
                        var upd = ReadProduct(withId: true);
                        result = await client.PutAsJsonAsync("products", upd);
                        Console.WriteLine($"{(int)result.StatusCode}: {await result.Content.ReadAsStringAsync()}");
                        break;

                    case 2:
                        Console.Clear();
                        Console.ForegroundColor = ConsoleColor.Magenta;
                        Console.WriteLine("================= PRODUCTS ================="); 
                        Console.ResetColor();
                        var all = await client.GetFromJsonAsync<List<Product>>("products");
                        if(all!.Count == 0)
                        {
                            Console.ForegroundColor = ConsoleColor.Red;
                            Console.WriteLine("Empty!"); 
                            Console.ResetColor();
                            break;
                        }
                        else
                        {
                            Console.ForegroundColor = ConsoleColor.Cyan;
                            foreach (var p in all!)
                                Console.WriteLine($"{p.Id} | {p.Name} | {p.Price} | {p.Category}");
                            Console.ResetColor();
                        }
                        break;

                    case 3:
                        Console.Clear();
                        Console.ForegroundColor = ConsoleColor.Magenta;
                        Console.WriteLine("================= GET PRODUCTS BY ID =================");
                        Console.ResetColor();
                        Console.Write("Id: ");
                        var id = int.Parse(Console.ReadLine()!);
                        result = await client.GetAsync($"products/{id}");
                        if (result.IsSuccessStatusCode)
                        {
                            var p = await result.Content.ReadFromJsonAsync<Product>();
                            Console.WriteLine($"{p!.Id} | {p.Name} | {p.Price} | {p.Category}");
                        }
                        else Console.WriteLine("Not Found!");
                        break;

                    case 4:
                        Console.Clear();
                        Console.ForegroundColor = ConsoleColor.Magenta;
                        Console.WriteLine("================= DELETE PRODUCTS =================");
                        Console.ResetColor();
                        Console.Write("Id: ");
                        id = int.Parse(Console.ReadLine()!);
                        result = await client.DeleteAsync($"products/{id}");
                        Console.WriteLine($"{(int)result.StatusCode}: {await result.Content.ReadAsStringAsync()}");
                        break;

                    case 5:
                        return;
                }
                Console.WriteLine();
                Console.ForegroundColor = ConsoleColor.Yellow;
                Console.WriteLine("Press enter to continue...");
                Console.ResetColor();
                Console.ReadLine();
            }
        }

        public static Product ReadProduct(bool withId = false)
        {
            var product = new Product();
            if (withId) { Console.Write("Id: "); product.Id = int.Parse(Console.ReadLine()!); }
            Console.Write("Name: "); product.Name = Console.ReadLine();
            Console.Write("Price: "); product.Price = decimal.Parse(Console.ReadLine()!);
            Console.Write("Category: "); product.Category = Console.ReadLine();
            return product;
        }
    }
}