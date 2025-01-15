using ECommerce.Api.Data;
using Ecommerce.library.Models;
using Microsoft.EntityFrameworkCore;
using System;

namespace Ecommerce.Api.Services
{
    public class CategoryService : ICategoryService
    {
        private readonly AppDbScope appDbScope;

        public CategoryService(AppDbScope appDbContext)
        {
            this.appDbScope = appDbContext;
        }

        public async Task<List<Category>> GetCategoriesAsync() => await appDbScope.Categories.ToListAsync();
    }
}
