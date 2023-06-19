using System;
using System.Collections.Generic;
using Microsoft.EntityFrameworkCore;

namespace RentAMovie_v3.Models;

public partial class RentAmovieSystemMod2Context : DbContext
{
    public RentAmovieSystemMod2Context()
    {
    }

    public RentAmovieSystemMod2Context(DbContextOptions<RentAmovieSystemMod2Context> options)
        : base(options)
    {
    }

    public virtual DbSet<Actor> Actors { get; set; }

    public virtual DbSet<Address> Addresses { get; set; }

    public virtual DbSet<Customer> Customers { get; set; }

    public virtual DbSet<Genre> Genres { get; set; }

    public virtual DbSet<LoginSession> LoginSessions { get; set; }

    public virtual DbSet<Movie> Movies { get; set; }

    public virtual DbSet<MovieActor> MovieActors { get; set; }

    public virtual DbSet<MovieGenre> MovieGenres { get; set; }

    public virtual DbSet<RentalTransaction> RentalTransactions { get; set; }

    public virtual DbSet<Role> Roles { get; set; }

    public virtual DbSet<Staff> Staff { get; set; }

    public virtual DbSet<State> States { get; set; }

    protected override void OnConfiguring(DbContextOptionsBuilder optionsBuilder)
// warning To protect potentially sensitive information in your connection string, you should move it out of source code. You can avoid scaffolding the connection string by using the Name= syntax to read it from configuration - see https://go.microsoft.com/fwlink/?linkid=2131148. For more guidance on storing connection strings, see http://go.microsoft.com/fwlink/?LinkId=723263.
        => optionsBuilder.UseSqlite("DataSource=RentAMovieSystem_Mod2.db");

    protected override void OnModelCreating(ModelBuilder modelBuilder)
    {
        modelBuilder.Entity<Actor>(entity =>
        {
            entity.ToTable("ACTOR");

            entity.HasIndex(e => e.ActorId, "IX_ACTOR_Actor_ID").IsUnique();

            entity.Property(e => e.ActorId).HasColumnName("Actor_ID");
            entity.Property(e => e.ActorFname).HasColumnName("Actor_Fname");
            entity.Property(e => e.ActorSname).HasColumnName("Actor_Sname");
        });

        modelBuilder.Entity<Address>(entity =>
        {
            entity.ToTable("ADDRESS");

            entity.HasIndex(e => e.AddressId, "IX_ADDRESS_Address_ID").IsUnique();

            entity.Property(e => e.AddressId).HasColumnName("Address_ID");
            entity.Property(e => e.HouseAddress).HasColumnName("House_Address");
            entity.Property(e => e.ZipCode).HasColumnName("ZIP_code");
            entity.Property(e => e.CustomerId).HasColumnName("Customer_ID");

            entity.HasOne(d => d.Customer).WithOne(p => p.Address).HasForeignKey<Address>(d => d.CustomerId);
        });

        modelBuilder.Entity<Customer>(entity =>
        {
            entity.ToTable("CUSTOMER ");

            entity.HasIndex(e => e.CustomerId, "IX_CUSTOMER _Customer_ID").IsUnique();

            entity.HasIndex(e => e.PhoneNo, "IX_CUSTOMER _Phone_No").IsUnique();

            entity.Property(e => e.CustomerId).HasColumnName("Customer_ID");
            entity.Property(e => e.FName).HasColumnName("F_Name");
            entity.Property(e => e.LName).HasColumnName("L_Name");
            entity.Property(e => e.MName).HasColumnName("M_Name");
            entity.Property(e => e.PhoneNo).HasColumnName("Phone_No");

        });

        modelBuilder.Entity<Genre>(entity =>
        {
            entity.ToTable("GENRE");

            entity.HasIndex(e => e.GenreId, "IX_GENRE_Genre_ID").IsUnique();

            entity.HasIndex(e => e.GenreName, "IX_GENRE_Genre_Name").IsUnique();

            entity.Property(e => e.GenreId)
                .ValueGeneratedNever()
                .HasColumnName("Genre_ID");
            entity.Property(e => e.GenreName).HasColumnName("Genre_Name");
        });

        modelBuilder.Entity<LoginSession>(entity =>
        {
            entity.HasKey(e => e.SessionId);

            entity.ToTable("LOGIN_SESSION");

            entity.HasIndex(e => e.SessionId, "IX_LOGIN_SESSION_Session_ID").IsUnique();

            entity.HasIndex(e => e.SessionKey, "IX_LOGIN_SESSION_Session_Key").IsUnique();

            entity.Property(e => e.SessionId)
                .ValueGeneratedNever()
                .HasColumnName("Session_ID");
            entity.Property(e => e.StaffId).HasColumnName("Staff_ID");
            entity.Property(e => e.TimeEnded).HasColumnName("Time_Ended");
            entity.Property(e => e.TimeStarted).HasColumnName("Time_Started");
            entity.Property(e => e.SessionKey).HasColumnName("Session_Key");

            entity.HasOne(d => d.Staff).WithOne(p => p.LoginSession)
                .HasForeignKey<LoginSession>(d => d.StaffId)
                .OnDelete(DeleteBehavior.ClientSetNull);
        });

        modelBuilder.Entity<Movie>(entity =>
        {
            entity.ToTable("MOVIE");

            entity.HasIndex(e => e.MovieId, "IX_MOVIE_Movie_ID").IsUnique();

            entity.Property(e => e.MovieId)
                .ValueGeneratedNever()
                .HasColumnName("Movie_ID");
            entity.Property(e => e.UnitPrice).HasColumnName("Unit_Price");
            entity.Property(e => e.YearOfRelease).HasColumnName("Year_of_Release");
        });

        modelBuilder.Entity<MovieActor>(entity =>
        {
            entity
                .HasNoKey()
                .ToTable("MOVIE_ACTOR");

            entity.Property(e => e.ActorId).HasColumnName("Actor_ID");
            entity.Property(e => e.MovieId).HasColumnName("Movie_ID");

            entity.HasOne(d => d.Actor).WithMany().HasForeignKey(d => d.ActorId);

            entity.HasOne(d => d.Movie).WithMany().HasForeignKey(d => d.MovieId);
        });

        modelBuilder.Entity<MovieGenre>(entity =>
        {
            entity
                .HasNoKey()
                .ToTable("MOVIE_GENRE");

            entity.Property(e => e.GenreId).HasColumnName("Genre_ID");
            entity.Property(e => e.MovieId).HasColumnName("Movie_ID");

            entity.HasOne(d => d.Genre).WithMany().HasForeignKey(d => d.GenreId);

            entity.HasOne(d => d.Movie).WithMany().HasForeignKey(d => d.MovieId);
        });

        modelBuilder.Entity<RentalTransaction>(entity =>
        {
            entity.HasKey(e => e.RentalId);

            entity.ToTable("RENTAL_TRANSACTION");

            entity.HasIndex(e => e.RentalId, "IX_RENTAL_TRANSACTION_Rental_ID").IsUnique();

            entity.Property(e => e.RentalId)
                .ValueGeneratedNever()
                .HasColumnName("Rental_ID");
            entity.Property(e => e.CustomerId).HasColumnName("Customer_ID");
            entity.Property(e => e.MovieId).HasColumnName("Movie_ID");
            entity.Property(e => e.RentalDay).HasColumnName("Rental_Day");
            entity.Property(e => e.ReturnDate).HasColumnName("Return_Date");
            entity.Property(e => e.SessionId).HasColumnName("Session_ID");

            entity.HasOne(d => d.Customer).WithMany(p => p.RentalTransactions).HasForeignKey(d => d.CustomerId);

            entity.HasOne(d => d.Movie).WithMany(p => p.RentalTransactions).HasForeignKey(d => d.MovieId);

            entity.HasOne(d => d.Session).WithMany(p => p.RentalTransactions).HasForeignKey(d => d.SessionId);
        });

        modelBuilder.Entity<Role>(entity =>
        {
            entity.ToTable("ROLE");

            entity.HasIndex(e => e.RoleId, "IX_ROLE_Role_Id").IsUnique();

            entity.HasIndex(e => e.Title, "IX_ROLE_Title").IsUnique();

            entity.Property(e => e.RoleId)
                .ValueGeneratedNever()
                .HasColumnName("Role_Id");
        });

        modelBuilder.Entity<Staff>(entity =>
        {
            entity.ToTable("STAFF");

            entity.HasIndex(e => e.StaffId, "IX_STAFF_Staff_ID").IsUnique();

            entity.HasIndex(e => e.StaffPassword, "IX_STAFF_Staff_Password").IsUnique();

            entity.Property(e => e.StaffId).HasColumnName("Staff_ID");
            entity.Property(e => e.StaffPassword).HasColumnName("Staff_Password");
            entity.Property(e => e.StaffRole).HasColumnName("Staff_Role");
            entity.Property(e => e.StaffUserName).HasColumnName("Staff_UserName");
        });

        modelBuilder.Entity<State>(entity =>
        {
            entity.HasKey(e => e.ZipCode);

            entity.ToTable("STATE");

            entity.Property(e => e.ZipCode).HasColumnName("Zip_Code");
            entity.Property(e => e.StateName).HasColumnName("State_Name");
        });

        OnModelCreatingPartial(modelBuilder);
    }

    partial void OnModelCreatingPartial(ModelBuilder modelBuilder);
}
