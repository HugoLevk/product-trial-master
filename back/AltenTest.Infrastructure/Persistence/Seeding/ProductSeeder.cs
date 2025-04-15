using AltenTest.Domain.Entities;
using AltenTest.Domain.Enums;
using Microsoft.EntityFrameworkCore;
using System.Text.Json;
using System.Text.Json.Serialization;

namespace AltenTest.Infrastructure.Persistence.Seeding;

public static class ProductSeeder
{
    public static async Task SeedProductsAsync(ApplicationDbContext context)
    {
        if (await context.Products.AnyAsync())
            return;

        try
        {
            var jsonProducts = await LoadProductsFromJsonAsync();
            if (jsonProducts == null)
                return;

            await SaveProductsToDatabaseAsync(context, jsonProducts);
        }
        catch (Exception ex)
        {
            LogSeedingError(ex);
            throw;
        }
    }

    private static async Task<List<JsonProduct>?> LoadProductsFromJsonAsync()
    {
        string jsonPath = GetJsonFilePath();
        string jsonContent = await File.ReadAllTextAsync(jsonPath);

        var options = new JsonSerializerOptions
        {
            PropertyNameCaseInsensitive = true,
            Converters = { new JsonStringEnumConverter() }
        };

        return JsonSerializer.Deserialize<List<JsonProduct>>(jsonContent, options);
    }

    private static string GetJsonFilePath()
    {
        string jsonPath = Path.Combine(AppDomain.CurrentDomain.BaseDirectory, "Persistence", "Seeding", "products.json");
        if (File.Exists(jsonPath))
            return jsonPath;

        jsonPath = Path.Combine(AppDomain.CurrentDomain.BaseDirectory, "AltenTest.Infrastructure", "Persistence", "Seeding", "products.json");
        if (!File.Exists(jsonPath))
            throw new FileNotFoundException($"Le fichier products.json n'existe pas aux emplacements: {jsonPath}");

        return jsonPath;
    }

    private static async Task SaveProductsToDatabaseAsync(ApplicationDbContext context, List<JsonProduct> jsonProducts)
    {
        var strategy = context.Database.CreateExecutionStrategy();
        await strategy.ExecuteAsync(async () =>
        {
            using var transaction = await context.Database.BeginTransactionAsync();
            try
            {
                await context.Database.ExecuteSqlRawAsync("SET IDENTITY_INSERT Products ON");
                await AddProductsToContext(context, jsonProducts);
                await context.SaveChangesAsync();
                await context.Database.ExecuteSqlRawAsync("SET IDENTITY_INSERT Products OFF");
                await transaction.CommitAsync();
            }
            catch
            {
                await transaction.RollbackAsync();
                throw;
            }
        });
    }

    private static async Task AddProductsToContext(ApplicationDbContext context, List<JsonProduct> jsonProducts)
    {
        foreach (var jsonProduct in jsonProducts)
        {
            var product = new Product
            {
                Id = jsonProduct.Id,
                Code = jsonProduct.Code,
                Name = jsonProduct.Name,
                Description = jsonProduct.Description,
                Image = jsonProduct.Image,
                Price = jsonProduct.Price,
                Category = jsonProduct.Category,
                InternalReference = jsonProduct.InternalReference,
                ShellId = jsonProduct.ShellId,
                Rating = jsonProduct.Rating,
                CreatedAt = DateTimeOffset.FromUnixTimeMilliseconds(jsonProduct.CreatedAt).DateTime,
                UpdatedAt = jsonProduct.UpdatedAt.HasValue
                    ? DateTimeOffset.FromUnixTimeMilliseconds(jsonProduct.UpdatedAt.Value).DateTime
                    : null,
                Quantity = 10,
                InventoryStatus = Enum.Parse<InventoryStatus>(jsonProduct.InventoryStatus)
            };

            await context.Products.AddAsync(product);
        }
    }

    private static void LogSeedingError(Exception ex)
    {
        Console.WriteLine($"Erreur lors du seeding des produits: {ex.Message}");
        Console.WriteLine($"StackTrace: {ex.StackTrace}");
        if (ex.InnerException != null)
        {
            Console.WriteLine($"Inner Exception: {ex.InnerException.Message}");
        }
    }

    private sealed class JsonProduct
    {
        public int Id { get; set; }
        public string Code { get; set; } = string.Empty;
        public string Name { get; set; } = string.Empty;
        public string Description { get; set; } = string.Empty;
        public string Image { get; set; } = string.Empty;
        public decimal Price { get; set; }
        public string Category { get; set; } = string.Empty;
        public long CreatedAt { get; set; }
        public long? UpdatedAt { get; set; }
        public int ShellId { get; set; }
        public string InternalReference { get; set; } = string.Empty;
        public string InventoryStatus { get; set; } = string.Empty;
        public int Rating { get; set; }
    }
}
