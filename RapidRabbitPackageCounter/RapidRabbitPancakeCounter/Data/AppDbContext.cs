using Microsoft.EntityFrameworkCore;

namespace RapidRabbitPancakeCounter.Data;

public class AppDbContext(DbContextOptions<AppDbContext> options) : DbContext(options)
{
    
}