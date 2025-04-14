using AltenTest.Domain.Entities;
using AltenTest.Domain.Enums;
using AltenTest.Domain.Interfaces;
using Microsoft.Extensions.Logging;

namespace AltenTest.Infrastructure.Authorization.Services;

public class ProductAuthorizationService(ILogger<ProductAuthorizationService> logger) : IProductAuthorizationService
{
    private const string AdminEmail = "admin@admin.com";

    public bool Authorize(Product product, ResourceOperation operation, string userEmail)
    {
        logger.LogInformation("Checking if user {UserEmail} can {Operation} product {ProductName}", 
            userEmail, operation, product.Name);

        // Read operations are allowed for all authenticated users
        if (operation == ResourceOperation.Read)
        {
            logger.LogInformation("Read Operation - Successful authorization");
            return true;
        }

        // Only admin can create, update or delete products
        if (userEmail == AdminEmail)
        {
            logger.LogInformation("{Operation} Operation - Successful authorization for Admin", operation);
            return true;
        }

        logger.LogWarning("{Operation} Operation - Authorization denied for user {UserEmail}", operation, userEmail);
        return false;
    }
} 