using Microsoft.EntityFrameworkCore;

namespace ITI_Attendance.Models
{
    public class ITIDbContext : DbContext
    {
        public ITIDbContext(DbContextOptions options) : base(options)
        {

        }
        public ITIDbContext()
        {

        }
        public DbSet<Student> Students { get; set; }
        public DbSet<Department> Departments { get; set; }
        public DbSet<Hr> Hrs { get; set; }
        public DbSet<UserLogin> UserLogins { get; set; }
        public DbSet<Role> Roles { get; set; }
        public DbSet<UserRole> UserRoles { get; set; }
        public DbSet<StudentAttendance> StudentAttendances { get; set; }
        public DbSet<Admin> Admins { get; set; }
        public DbSet<Program> Programs { get; set; }
       public DbSet<StudentProgramIntake> Intakes { get; set; }
        //protected override void OnConfiguring(DbContextOptionsBuilder optionsBuilder)
        //{
        //    optionsBuilder
        //        .UseSqlServer("Server = COMPUTEC\\SQLEXPRESS06 ; Database = ITI_Attendance ; Integrated Security = SSPI ; TrustServerCertificate = True");
        //    base.OnConfiguring(optionsBuilder);
        //}
        protected override void OnModelCreating(ModelBuilder modelBuilder)
        {
            modelBuilder.Entity<UserRole>()
                       .HasKey(ur => new { ur.UserLoginId, ur.RoleId });

            modelBuilder.Entity<UserRole>()
                .HasOne(ur => ur.UserLogin)
                .WithMany(ul => ul.UserRoles)
                .HasForeignKey(ur => ur.UserLoginId);

            modelBuilder.Entity<UserRole>()
                .HasOne(ur => ur.Role)
                .WithMany(r => r.UserRoles)
                .HasForeignKey(ur => ur.RoleId);

            modelBuilder.Entity<Student>()
                .HasOne(s => s.Hr)
                .WithMany(hr => hr.VerifiedStudents)
                .HasForeignKey(s => s.HrId);

            modelBuilder.Entity<Student>()
                .HasOne(s => s.Department)
                .WithMany(d => d.Students)
                .HasForeignKey(s => s.DeptNum);

            modelBuilder.Entity<StudentProgramIntake>()
          .HasKey(ur => new { ur.StudentId, ur.ProgramId });

            modelBuilder.Entity<StudentProgramIntake>()
                .HasOne(ur => ur.Student)
                .WithMany(u => u.Intakes)
                .HasForeignKey(ur => ur.StudentId);

            modelBuilder.Entity<StudentProgramIntake>()
                .HasOne(ur => ur.Program)
                .WithMany(r => r.Intakes)
                .HasForeignKey(ur => ur.ProgramId);

            modelBuilder.Entity<Hr>()
               .HasOne(h => h.UserLogin)
               .WithOne(u => u.Hr)
               .HasForeignKey<Hr>(h => h.UserLoginId)
               .OnDelete(DeleteBehavior.Cascade);
            base.OnModelCreating(modelBuilder);
        }


    }
}
