using AltenTest.Domain.Entities;
using AltenTest.Domain.Enums;

namespace AltenTest.Domain.Interfaces; 

public interface IProductAuthorizationService
{
    bool Authorize(Product product, ResourceOperation operation, string userEmail);
} 