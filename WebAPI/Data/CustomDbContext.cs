/*
 * El código fuente de este archivo es propiedad intelectual de Gerzon Gonzalez.
 */
using Microsoft.EntityFrameworkCore;
using WebAPI.DTO;

namespace WebAPI.Data;

/**
 * @author gerzon
 */
public class CustomDbContext(DbContextOptions<CustomDbContext> options) : DbContext(options)
{
    public virtual DbSet<Country> Countries => Set<Country>();

    public virtual DbSet<Organization> Organizations => Set<Organization>();

    public virtual DbSet<Station> Stations => Set<Station>();

    public virtual DbSet<DTO.Data> Data => Set<DTO.Data>();
}