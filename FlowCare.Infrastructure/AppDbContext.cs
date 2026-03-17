using FlowCare.Domain.Entities;
using FlowCare.Domain.Enums;
using Microsoft.AspNetCore.Identity;
using Microsoft.EntityFrameworkCore;

namespace FlowCare.Infrastructure;

public sealed class AppDbContext : DbContext
{
    public AppDbContext(DbContextOptions<AppDbContext> options) : base(options)
    {
        
    }

    public DbSet<Branch> Branches => Set<Branch>();
    public DbSet<BranchWorkingHour> BranchWorkingHours => Set<BranchWorkingHour>();
    public DbSet<Staff> Staff => Set<Staff>();
    public DbSet<StaffWorkingHour> StaffWorkingHours => Set<StaffWorkingHour>();
    public DbSet<StaffServiceType> StaffServiceTypes => Set<StaffServiceType>();
    public DbSet<ServiceType> ServiceTypes => Set<ServiceType>();
    public DbSet<Slot> Slots => Set<Slot>();
    public DbSet<Customer> Customers => Set<Customer>();
    public DbSet<Appointment> Appointments => Set<Appointment>();
    public DbSet<AuditLog> AuditLogs => Set<AuditLog>();
    public DbSet<ActionType> ActionTypes => Set<ActionType>();
    public DbSet<EntityType> EntityTypes => Set<EntityType>();

    public DbSet<Role> Roles => Set<Role>(); 

    public DbSet<BranchSlotPosition> branchSlotPositions => Set<BranchSlotPosition>();

    protected override void OnModelCreating(ModelBuilder modelBuilder)
    {
        modelBuilder.ApplyConfigurationsFromAssembly(typeof(AppDbContext).Assembly);
        base.OnModelCreating(modelBuilder);

        var passwordHasher = new PasswordHasher<object>();

        var admin = new Staff
        {
            Id = 1,
            Username = "admin",
            PasswordHash = "AQAAAAIAAYagAAAAEC8xDeNpz9E8nc11DDSd4dcD9PVwadnRFJxfk8nfk7FqLUiuopHjmBJ44hGdz954jQ==", // Admin@123
            RoleId = 1,     
            FullName = "System Admin",
            Email = "admin@flowcare.local",
            BranchId = 1,
            IsActive = true
        };

        var managerMuscat = new Staff
        {
            Id = 2,
            Username = "mgr_muscat",
            PasswordHash = "AQAAAAIAAYagAAAAEICab8+wTwJozhRWUwreIBIMBkh3s2MHVKM1Yzp69eLAmZyvBMmtsK5ZSg9b+9NGJw==", // Manager@123
            RoleId = 2,
            FullName = "Aisha Al Balushi",
            Email = "aisha.b@flowcare.local",
            BranchId = 1,
            IsActive = true
        };

        var managerSuhar = new Staff
        {
            Id = 3,
            Username = "mgr_suhar",
            PasswordHash = "AQAAAAIAAYagAAAAEES+cw79Pa3WRk3nd1PVkOcv7ZVXJ5KJGfuSi4QNncMFCxFXTyUynYUAvukbqtsmkQ==", // Manager@123
            RoleId = 2,
            FullName = "Hamad Al Hinai",
            Email = "hamad.h@flowcare.local",
            BranchId = 2,
            IsActive = true
        };

        var staffMuscat1 = new Staff
        {
            Id = 4,
            Username = "staff_muscat_1",
            PasswordHash = "AQAAAAIAAYagAAAAEEzqb3dbU8+IioqpTJ0sycCRu3H90O6n7sG3DZYMc2woBJy+ycq94hESNK4mb0YAsg==", // Staff@123
            RoleId = 3,
            FullName = "Salim Al Rashdi",
            Email = "salim.r@flowcare.local",
            BranchId = 1,
            IsActive = true
        };

        var staffMuscat2 = new Staff
        {
            Id = 5,
            Username = "staff_muscat_2",
            PasswordHash = "AQAAAAIAAYagAAAAEIHjdTv2h1anEJbgnEH/C408rfryyQWwwWpYzEz0pxy+bjhrKjj+Zk3FRm5rEV8pOw==", // Staff@123
            RoleId = 3,
            FullName = "Maha Al Shukaili",
            Email = "maha.s@flowcare.local",
            BranchId = 1,
            IsActive = true
        };

        var staffSuhar1 = new Staff
        {
            Id = 6,
            Username = "staff_suhar_1",
            PasswordHash = "AQAAAAIAAYagAAAAEBOMdOXpbpPdJwdkQSW7KnPbBtwDrXTZ6zSCmD2p9VwsSHVG9xZcIDOaEJWLqs4L0w==", // Staff@123
            RoleId = 3,
            FullName = "Nasser Al Maqbali",
            Email = "nasser.m@flowcare.local",
            BranchId = 2,
            IsActive = true
        };

        var staffSuhar2 = new Staff
        {
            Id = 7,
            Username = "staff_suhar_2",
            PasswordHash = "AQAAAAIAAYagAAAAEEfgZzmxWk+UqM6tXA+gt+wXwIuYwLSRO1EAbldfe1nZuwFVYVKBIQlkZD9AedT00w==", // Staff@123
            RoleId = 3,
            FullName = "Reem Al Kindi",
            Email = "reem.k@flowcare.local",
            BranchId = 2,
            IsActive = true
        };

        var customerAhmed = new Customer
        {
            Id = 1,
            Username = "cust_ahmed",
            PasswordHash = "AQAAAAIAAYagAAAAEOfn1uX/3oUCt3hcVLPLFdCP1Zd5XjUKIvXGYq3hRoQZmXXUn+XZpLFCfkECprifyQ==", // Customer@123
            FullName = "Ahmed Al Harthy",
            Email = "ahmed.h@example.com",
            Phone = "+96890000001",
            IsActive = true
        };

        var customerFatima = new Customer
        {
            Id = 2,
            Username = "cust_fatima",
            PasswordHash = "AQAAAAIAAYagAAAAEB3n4s8ogXr0nisk5WHlpMdKlfqqcfyKb9wAXP6Qlicv3rISUagNTgTcpJccKZ/3BA==", // Customer@123
            FullName = "Fatima Al Zadjali",
            Email = "fatima.z@example.com",
            Phone = "+96890000002",
            IsActive = true
        };

        var customerKhalid = new Customer
        {
            Id = 3,
            Username = "cust_khalid",
            PasswordHash = "AQAAAAIAAYagAAAAEI25+4Cf8C8Gnasac1q2lNeX6Xj7kYUU3Ey2brxZyS/Ox07YDMxBMwsQBoUwYavfJA==", // Customer@123
            FullName = "Khalid Al Amri",
            Email = "khalid.a@example.com",
            Phone = "+96890000003",
            IsActive = true
        };

        modelBuilder.Entity<Role>().HasData(
            new Role { Id = 1, Title = "Admin" },
            new Role { Id = 2, Title = "BranchManager" },
            new Role { Id = 3, Title = "Staff" },
            new Role { Id = 4, Title = "Customer" }
        );

        modelBuilder.Entity<Branch>().HasData(
            new Branch
            {
                Id = 1,
                Name = "FlowCare Muscat - Al Khuwair",
                City = "Muscat",
                Address = "Al Khuwair, Muscat",
                Timezone = "Asia/Muscat",
                IsActive = true,
                RetentionPerioud = 30 
            },
            new Branch
            {
                Id = 2,
                Name = "FlowCare Suhar - Al Humbar",
                City = "Suhar",
                Address = "Al Humbar, Suhar",
                Timezone = "Asia/Muscat",
                IsActive = true,
                RetentionPerioud = 30 
            }
        );

        modelBuilder.Entity<Staff>().HasData(
            admin,
            managerMuscat,
            managerSuhar,
            staffMuscat1,
            staffMuscat2,
            staffSuhar1,
            staffSuhar2
        );

        modelBuilder.Entity<Customer>().HasData(
            customerAhmed,
            customerFatima,
            customerKhalid
        );

        modelBuilder.Entity<ServiceType>().HasData(
            new ServiceType
            {
                Id = 1,
                BranchId = 1,
                Name = "Customer Support",
                Description = "Account questions, basic requests, ticket follow-ups",
                DurationMinutes = 15,
                IsActive = true
            },
            new ServiceType
            {
                Id = 2,
                BranchId = 1,
                Name = "Document Verification",
                Description = "Check documents, validate requirements, approvals",
                DurationMinutes = 20,
                IsActive = true
            },
            new ServiceType
            {
                Id = 3,
                BranchId = 1,
                Name = "VIP Service Desk",
                Description = "Priority desk for special cases",
                DurationMinutes = 30,
                IsActive = true
            },
            new ServiceType
            {
                Id = 4,
                BranchId = 2,
                Name = "Customer Support",
                Description = "Account questions, basic requests, ticket follow-ups",
                DurationMinutes = 15,
                IsActive = true
            },
            new ServiceType
            {
                Id = 5,
                BranchId = 2,
                Name = "New Registration",
                Description = "New customer onboarding and profile creation",
                DurationMinutes = 20,
                IsActive = true
            },
            new ServiceType
            {
                Id = 6,
                BranchId = 2,
                Name = "Complaints & Escalations",
                Description = "Escalations handling and complaints resolution",
                DurationMinutes = 25,
                IsActive = true
            }
        );

        modelBuilder.Entity<StaffServiceType>().HasData(
            new StaffServiceType { Id = 1, StaffId = 4, ServiceTypeId = 1 },
            new StaffServiceType { Id = 2, StaffId = 4, ServiceTypeId = 2 },
            new StaffServiceType { Id = 3, StaffId = 5, ServiceTypeId = 3 },
            new StaffServiceType { Id = 4, StaffId = 6, ServiceTypeId = 4 },
            new StaffServiceType { Id = 5, StaffId = 6, ServiceTypeId = 5 },
            new StaffServiceType { Id = 6, StaffId = 7, ServiceTypeId = 6 }
        );

        modelBuilder.Entity<Slot>().HasData(
            new Slot
            {
                Id = 1,
                BranchId = 1,
                ServiceTypeId = 1,
                StaffId = 4,
                StartAt = DateTimeOffset.Parse("2026-02-16T09:00:00+04:00"),
                EndAt = DateTimeOffset.Parse("2026-02-16T09:15:00+04:00"),
                Capacity = 1,
                IsActive = true
            },
            new Slot
            {
                Id = 2,
                BranchId = 1,
                ServiceTypeId = 1,
                StaffId = 4,
                StartAt = DateTimeOffset.Parse("2026-02-16T09:15:00+04:00"),
                EndAt = DateTimeOffset.Parse("2026-02-16T09:30:00+04:00"),
                Capacity = 1,
                IsActive = true
            },
            new Slot
            {
                Id = 3,
                BranchId = 1,
                ServiceTypeId = 2,
                StaffId = 4,
                StartAt = DateTimeOffset.Parse("2026-02-17T10:00:00+04:00"),
                EndAt = DateTimeOffset.Parse("2026-02-17T10:20:00+04:00"),
                Capacity = 1,
                IsActive = true
            },
            new Slot
            {
                Id = 4,
                BranchId = 1,
                ServiceTypeId = 3,
                StaffId = 5,
                StartAt = DateTimeOffset.Parse("2026-02-18T11:00:00+04:00"),
                EndAt = DateTimeOffset.Parse("2026-02-18T11:30:00+04:00"),
                Capacity = 1,
                IsActive = true
            },
            new Slot
            {
                Id = 5,
                BranchId = 1,
                ServiceTypeId = 2,
                StaffId = 4,
                StartAt = DateTimeOffset.Parse("2026-02-19T12:00:00+04:00"),
                EndAt = DateTimeOffset.Parse("2026-02-19T12:20:00+04:00"),
                Capacity = 1,
                IsActive = true
            },
            new Slot
            {
                Id = 6,
                BranchId = 1,
                ServiceTypeId = 3,
                StaffId = 5,
                StartAt = DateTimeOffset.Parse("2026-02-20T09:30:00+04:00"),
                EndAt = DateTimeOffset.Parse("2026-02-20T10:00:00+04:00"),
                Capacity = 1,
                IsActive = true
            },
            new Slot
            {
                Id = 7,
                BranchId = 2,
                ServiceTypeId = 4,
                StaffId = 6,
                StartAt = DateTimeOffset.Parse("2026-02-16T09:00:00+04:00"),
                EndAt = DateTimeOffset.Parse("2026-02-16T09:15:00+04:00"),
                Capacity = 1,
                IsActive = true
            },
            new Slot
            {
                Id = 8,
                BranchId = 2,
                ServiceTypeId = 5,
                StaffId = 6,
                StartAt = DateTimeOffset.Parse("2026-02-16T09:30:00+04:00"),
                EndAt = DateTimeOffset.Parse("2026-02-16T09:50:00+04:00"),
                Capacity = 1,
                IsActive = true
            },
            new Slot
            {
                Id = 9,
                BranchId = 2,
                ServiceTypeId = 6,
                StaffId = 7,
                StartAt = DateTimeOffset.Parse("2026-02-17T10:00:00+04:00"),
                EndAt = DateTimeOffset.Parse("2026-02-17T10:25:00+04:00"),
                Capacity = 1,
                IsActive = true
            },
            new Slot
            {
                Id = 10,
                BranchId = 2,
                ServiceTypeId = 6,
                StaffId = 7,
                StartAt = DateTimeOffset.Parse("2026-02-18T10:30:00+04:00"),
                EndAt = DateTimeOffset.Parse("2026-02-18T10:55:00+04:00"),
                Capacity = 1,
                IsActive = true
            },
            new Slot
            {
                Id = 11,
                BranchId = 2,
                ServiceTypeId = 4,
                StaffId = 6,
                StartAt = DateTimeOffset.Parse("2026-02-19T11:00:00+04:00"),
                EndAt = DateTimeOffset.Parse("2026-02-19T11:15:00+04:00"),
                Capacity = 1,
                IsActive = true
            },
            new Slot
            {
                Id = 12,
                BranchId = 2,
                ServiceTypeId = 5,
                StaffId = 6,
                StartAt = DateTimeOffset.Parse("2026-02-20T12:00:00+04:00"),
                EndAt = DateTimeOffset.Parse("2026-02-20T12:20:00+04:00"),
                Capacity = 1,
                IsActive = true
            },
            new Slot
            {
                Id = 13,
                BranchId = 1,
                ServiceTypeId = 1,
                StaffId = 4,
                StartAt = DateTimeOffset.Parse("2026-02-21T09:00:00+04:00"),
                EndAt = DateTimeOffset.Parse("2026-02-21T09:15:00+04:00"),
                Capacity = 1,
                IsActive = true
            },
            new Slot
            {
                Id = 14,
                BranchId = 2,
                ServiceTypeId = 4,
                StaffId = 6,
                StartAt = DateTimeOffset.Parse("2026-02-22T09:00:00+04:00"),
                EndAt = DateTimeOffset.Parse("2026-02-22T09:15:00+04:00"),
                Capacity = 1,
                IsActive = true
            }
        );

        modelBuilder.Entity<Appointment>().HasData(
            new Appointment
            {
                Id = 1,
                CustomerId = 1,
                BranchId = 1,
                ServiceTypeId = 1,
                SlotId = 1,
                StaffId = 4,
                Status = AppointmentStatus.Booked,
                CreatedAt = DateTimeOffset.Parse("2026-02-15T10:00:00+04:00")
            },
            new Appointment
            {
                Id = 2,
                CustomerId = 2,
                BranchId = 2,
                ServiceTypeId = 6,
                SlotId = 9,
                StaffId = 7,
                Status = AppointmentStatus.Booked,
                CreatedAt = DateTimeOffset.Parse("2026-02-15T10:05:00+04:00")
            }
        );

        modelBuilder.Entity<ActionType>().HasData(
            new ActionType { Id = 1, Type = "SEED_IMPORT" },
            new ActionType { Id = 2, Type = "APPOINTMENT_BOOKED" }
        );

        modelBuilder.Entity<EntityType>().HasData(
            new EntityType { Id = 1, Type = "SYSTEM" },
            new EntityType { Id = 2, Type = "APPOINTMENT" }
        );

        modelBuilder.Entity<AuditLog>().HasData(
            new AuditLog
            {
                Id = 1,
                ActorId = 1,
                RoleId = 1,
                ActionTypeId = 1,
                EntityTypeId = 1,
                EntityId = 1,
                Timestamp = DateTimeOffset.Parse("2026-02-15T10:10:00+04:00"),
                MetadataJson = "{\"note\":\"Initial seed import executed\"}"
            },
            new AuditLog
            {
                Id = 2,
                ActorId = 1,
                RoleId = 4,
                ActionTypeId = 2,
                EntityTypeId = 2,
                EntityId = 1,
                Timestamp = DateTimeOffset.Parse("2026-02-15T10:12:00+04:00"),
                MetadataJson = "{\"slot_id\":1,\"branch_id\":1,\"service_type_id\":1}"
            }
        );
    }
}
