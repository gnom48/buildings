using System;
using System.Collections.Generic;
using Microsoft.EntityFrameworkCore;

namespace BuildingSource.Models;

public partial class MasterContext : DbContext
{
    //Scaffold-DbContext "Server=.\SQLEXPRESS;Database=moster;Integrated Security=True;TrustServerCertificate=True" Microsoft.EntityFrameworkCore.SqlServer -OutputDir Models -Force

    private static MasterContext _instance;

    public static MasterContext GetInstance()
    {
        if (_instance == null)
        {
            _instance = new MasterContext();
        }
        return _instance;
    }

    public MasterContext()
    {
    }

    public MasterContext(DbContextOptions<MasterContext> options)
        : base(options)
    {
    }

    public virtual DbSet<ObjectType> ObjectTypes { get; set; }

    public virtual DbSet<RepairType> RepairTypes { get; set; }

    public virtual DbSet<RequestOption> RequestOptions { get; set; }

    public virtual DbSet<Service> Services { get; set; }

    public virtual DbSet<ServiceType> ServiceTypes { get; set; }

    public virtual DbSet<User> Users { get; set; }

    public virtual DbSet<UsersRequest> UsersRequests { get; set; }

    protected override void OnConfiguring(DbContextOptionsBuilder optionsBuilder)
#warning To protect potentially sensitive information in your connection string, you should move it out of source code. You can avoid scaffolding the connection string by using the Name= syntax to read it from configuration - see https://go.microsoft.com/fwlink/?linkid=2131148. For more guidance on storing connection strings, see http://go.microsoft.com/fwlink/?LinkId=723263.
        => optionsBuilder.UseSqlServer("Server=.\\SQLEXPRESS;Database=moster;Integrated Security=True;TrustServerCertificate=True");

    protected override void OnModelCreating(ModelBuilder modelBuilder)
    {
        modelBuilder.Entity<ObjectType>(entity =>
        {
            entity.HasKey(e => e.Id).HasName("PK__ObjectTy__3214EC07F33E0B95");

            entity.Property(e => e.Name).HasMaxLength(100);
        });

        modelBuilder.Entity<RepairType>(entity =>
        {
            entity.HasKey(e => e.Id).HasName("PK__RepairTy__3214EC07EEE339EF");

            entity.Property(e => e.Description).HasColumnType("ntext");
            entity.Property(e => e.M2price)
                .HasColumnType("decimal(18, 2)")
                .HasColumnName("M2Price");
            entity.Property(e => e.Name).HasMaxLength(100);
        });

        modelBuilder.Entity<RequestOption>(entity =>
        {
            entity.HasKey(e => new { e.UserRequestId, e.ServiceId }).HasName("PK__RequestO__A48C8C3158325347");

            entity.Property(e => e.Datetime)
                .IsRowVersion()
                .IsConcurrencyToken()
                .HasColumnName("datetime");

            entity.HasOne(d => d.Service).WithMany(p => p.RequestOptions)
                .HasForeignKey(d => d.ServiceId)
                .OnDelete(DeleteBehavior.ClientSetNull)
                .HasConstraintName("FK_RequestOptions_Services");

            entity.HasOne(d => d.UserRequest).WithMany(p => p.RequestOptions)
                .HasForeignKey(d => d.UserRequestId)
                .OnDelete(DeleteBehavior.ClientSetNull)
                .HasConstraintName("RequestOptions_UsersRequests_FK");
        });

        modelBuilder.Entity<Service>(entity =>
        {
            entity.HasKey(e => e.Id).HasName("PK__Services__3214EC07A750DAD1");

            entity.Property(e => e.Price).HasColumnType("decimal(18, 2)");

            entity.HasOne(d => d.RepairTypeNavigation).WithMany(p => p.Services)
                .HasForeignKey(d => d.RepairType)
                .HasConstraintName("Services_RepairTypes_FK");

            entity.HasOne(d => d.ServiceNavigation).WithMany(p => p.Services)
                .HasForeignKey(d => d.ServiceId)
                .HasConstraintName("Services_ServiceTypes_FK");
        });

        modelBuilder.Entity<ServiceType>(entity =>
        {
            entity.HasKey(e => e.Id).HasName("PK__ServiceT__3214EC07680D2FCD");

            entity.Property(e => e.Name).HasMaxLength(100);
        });

        modelBuilder.Entity<User>(entity =>
        {
            entity.HasKey(e => e.Id).HasName("PK__Users__3214EC079E9B58A0");

            entity.Property(e => e.Address).HasColumnType("ntext");
            entity.Property(e => e.Email).HasMaxLength(255);
            entity.Property(e => e.Fname)
                .HasMaxLength(100)
                .HasColumnName("FName");
            entity.Property(e => e.Login).HasMaxLength(100);
            entity.Property(e => e.Passport).HasMaxLength(11);
            entity.Property(e => e.Password).HasMaxLength(100);
            entity.Property(e => e.Patronumic).HasMaxLength(255);
            entity.Property(e => e.Phone).HasMaxLength(36);
            entity.Property(e => e.Surname).HasMaxLength(255);
        });

        modelBuilder.Entity<UsersRequest>(entity =>
        {
            entity.HasKey(e => e.Id).HasName("PK__UsersReq__3214EC0709E52BA8");

            entity.HasOne(d => d.ObjectType).WithMany(p => p.UsersRequests)
                .HasForeignKey(d => d.ObjectTypeId)
                .HasConstraintName("UsersRequests_ObjectTypes_FK");

            entity.HasOne(d => d.RepairType).WithMany(p => p.UsersRequests)
                .HasForeignKey(d => d.RepairTypeId)
                .HasConstraintName("UsersRequests_RepairTypes_FK");

            entity.HasOne(d => d.User).WithMany(p => p.UsersRequests)
                .HasForeignKey(d => d.UserId)
                .HasConstraintName("UsersRequests_Users_FK");
        });

        OnModelCreatingPartial(modelBuilder);
    }

    partial void OnModelCreatingPartial(ModelBuilder modelBuilder);
}
