/*
 * El código fuente de este archivo es propiedad intelectual de Gerzon Gonzalez.
 */
using Microsoft.EntityFrameworkCore;

namespace WebAPI.Data;

/**
 * @author gerzon
 */
public class CustomDbContext(DbContextOptions<CustomDbContext> options) : DbContext(options)
{
    public virtual DbSet<DTO.Country> Countries => Set<DTO.Country>();
    
    public virtual DbSet<DTO.Organization> Organizations => Set<DTO.Organization>();
    
    public virtual DbSet<DTO.Station> Stations => Set<DTO.Station>();
    
    public virtual DbSet<DTO.Data> Data => Set<DTO.Data>();
}