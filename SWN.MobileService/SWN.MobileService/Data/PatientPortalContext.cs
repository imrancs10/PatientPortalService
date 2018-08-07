using System;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata;

namespace PatientPortalService.Api.Data
{
    public partial class PatientPortalContext : DbContext
    {
        public PatientPortalContext()
        {
        }

        public PatientPortalContext(DbContextOptions<PatientPortalContext> options)
            : base(options)
        {
        }

        public virtual DbSet<AppointmentInfo> AppointmentInfo { get; set; }
        public virtual DbSet<DayMaster> DayMaster { get; set; }
        public virtual DbSet<Department> Department { get; set; }
        public virtual DbSet<Doctor> Doctor { get; set; }
        public virtual DbSet<DoctorSchedule> DoctorSchedule { get; set; }
        public virtual DbSet<GblMasterLogin> GblMasterLogin { get; set; }
        public virtual DbSet<GblMasterUser> GblMasterUser { get; set; }
        public virtual DbSet<HospitalDetail> HospitalDetail { get; set; }
        public virtual DbSet<MeridiemMaster> MeridiemMaster { get; set; }
        public virtual DbSet<PatientInfo> PatientInfo { get; set; }
        public virtual DbSet<PatientTransaction> PatientTransaction { get; set; }

        protected override void OnConfiguring(DbContextOptionsBuilder optionsBuilder)
        {
            if (!optionsBuilder.IsConfigured)
            {
                //#warning To protect potentially sensitive information in your connection string, 
                //you should move it out of source code. See http://go.microsoft.com/fwlink/?LinkId=723263 
                //for guidance on storing connection strings.
                optionsBuilder.UseSqlServer("Server=.\\;Database=PatientPortal;user id=sa;password=Passw0rd;");
            }
        }

        protected override void OnModelCreating(ModelBuilder modelBuilder)
        {
            modelBuilder.Entity<AppointmentInfo>(entity =>
            {
                entity.HasKey(e => e.AppointmentId);

                entity.Property(e => e.AppointmentDateFrom).HasColumnType("datetime");

                entity.Property(e => e.AppointmentDateTo).HasColumnType("datetime");

                entity.Property(e => e.CancelDate).HasColumnType("datetime");

                entity.Property(e => e.CancelReason).HasMaxLength(500);

                entity.Property(e => e.CreatedDate).HasColumnType("datetime");

                entity.Property(e => e.ModifiedDate).HasColumnType("datetime");

                entity.HasOne(d => d.Doctor)
                    .WithMany(p => p.AppointmentInfo)
                    .HasForeignKey(d => d.DoctorId)
                    .OnDelete(DeleteBehavior.ClientSetNull)
                    .HasConstraintName("FK_AppointmentInfo_Doctor");

                entity.HasOne(d => d.Patient)
                    .WithMany(p => p.AppointmentInfo)
                    .HasForeignKey(d => d.PatientId)
                    .OnDelete(DeleteBehavior.ClientSetNull)
                    .HasConstraintName("FK_AppointmentInfo_PatientInfo");
            });

            modelBuilder.Entity<DayMaster>(entity =>
            {
                entity.HasKey(e => e.DayId);

                entity.Property(e => e.DayName)
                    .HasMaxLength(100)
                    .IsUnicode(false);
            });

            modelBuilder.Entity<Department>(entity =>
            {
                entity.Property(e => e.DepartmentId).HasColumnName("DepartmentID");

                entity.Property(e => e.DepartmentName)
                    .HasMaxLength(200)
                    .IsUnicode(false);
            });

            modelBuilder.Entity<Doctor>(entity =>
            {
                entity.Property(e => e.DoctorId).HasColumnName("DoctorID");

                entity.Property(e => e.DepartmentId).HasColumnName("DepartmentID");

                entity.Property(e => e.DoctorName)
                    .HasMaxLength(200)
                    .IsUnicode(false);

                entity.HasOne(d => d.Department)
                    .WithMany(p => p.Doctor)
                    .HasForeignKey(d => d.DepartmentId)
                    .OnDelete(DeleteBehavior.ClientSetNull)
                    .HasConstraintName("FK_Doctor_Doctor");
            });

            modelBuilder.Entity<DoctorSchedule>(entity =>
            {
                entity.Property(e => e.DoctorScheduleId).HasColumnName("DoctorScheduleID");

                entity.Property(e => e.DayId).HasColumnName("DayID");

                entity.Property(e => e.DoctorId).HasColumnName("DoctorID");

                entity.Property(e => e.TimeFromMeridiemId).HasColumnName("TimeFromMeridiemID");

                entity.Property(e => e.TimeToMeridiemId).HasColumnName("TimeToMeridiemID");

                entity.HasOne(d => d.Day)
                    .WithMany(p => p.DoctorSchedule)
                    .HasForeignKey(d => d.DayId)
                    .HasConstraintName("FK_DoctorScheduleDay_DayMaster1");

                entity.HasOne(d => d.Doctor)
                    .WithMany(p => p.DoctorSchedule)
                    .HasForeignKey(d => d.DoctorId)
                    .HasConstraintName("FK_DoctorScheduleDay_DoctorSchedule");

                entity.HasOne(d => d.TimeFromMeridiem)
                    .WithMany(p => p.DoctorScheduleTimeFromMeridiem)
                    .HasForeignKey(d => d.TimeFromMeridiemId)
                    .HasConstraintName("FK_DoctorScheduleDay_MeridiemMaster");

                entity.HasOne(d => d.TimeToMeridiem)
                    .WithMany(p => p.DoctorScheduleTimeToMeridiem)
                    .HasForeignKey(d => d.TimeToMeridiemId)
                    .HasConstraintName("FK_DoctorScheduleDay_MeridiemMaster1");
            });

            modelBuilder.Entity<GblMasterLogin>(entity =>
            {
                entity.HasKey(e => e.LoginId);

                entity.ToTable("Gbl_Master_Login");

                entity.Property(e => e.CreatedAt).HasColumnType("datetime");

                entity.Property(e => e.LastLogin).HasColumnType("datetime");

                entity.Property(e => e.ModifiedAt).HasColumnType("datetime");

                entity.HasOne(d => d.User)
                    .WithMany(p => p.GblMasterLogin)
                    .HasForeignKey(d => d.UserId)
                    .OnDelete(DeleteBehavior.ClientSetNull)
                    .HasConstraintName("FK_Gbl_Master_Login_Gbl_Master_User");
            });

            modelBuilder.Entity<GblMasterUser>(entity =>
            {
                entity.HasKey(e => e.UserId);

                entity.ToTable("Gbl_Master_User");

                entity.Property(e => e.CreatedAt).HasColumnType("datetime");

                entity.Property(e => e.DoB).HasColumnType("datetime");

                entity.Property(e => e.EmailId)
                    .IsRequired()
                    .HasMaxLength(50);

                entity.Property(e => e.FirstName)
                    .IsRequired()
                    .HasMaxLength(50);

                entity.Property(e => e.Gender)
                    .IsRequired()
                    .HasMaxLength(1);

                entity.Property(e => e.IsdCode)
                    .IsRequired()
                    .HasMaxLength(4);

                entity.Property(e => e.LastName)
                    .IsRequired()
                    .HasMaxLength(50);

                entity.Property(e => e.MiddleName).HasMaxLength(50);

                entity.Property(e => e.MobileNumber)
                    .IsRequired()
                    .HasMaxLength(13);

                entity.Property(e => e.ModifiedAt).HasColumnType("datetime");

                entity.Property(e => e.PasswordHash).HasMaxLength(250);

                entity.Property(e => e.Username)
                    .IsRequired()
                    .HasMaxLength(50);
            });

            modelBuilder.Entity<HospitalDetail>(entity =>
            {
                entity.Property(e => e.HospitalName)
                    .HasMaxLength(500)
                    .IsUnicode(false);
            });

            modelBuilder.Entity<MeridiemMaster>(entity =>
            {
                entity.HasKey(e => e.MeridiemId);

                entity.Property(e => e.MeridiemId).HasColumnName("MeridiemID");

                entity.Property(e => e.MeridiemValue)
                    .HasMaxLength(50)
                    .IsUnicode(false);
            });

            modelBuilder.Entity<PatientInfo>(entity =>
            {
                entity.HasKey(e => e.PatientId);

                entity.Property(e => e.Address)
                    .HasMaxLength(500)
                    .IsUnicode(false);

                entity.Property(e => e.City)
                    .HasMaxLength(50)
                    .IsUnicode(false);

                entity.Property(e => e.Country)
                    .HasMaxLength(50)
                    .IsUnicode(false);

                entity.Property(e => e.Dob)
                    .HasColumnName("DOB")
                    .HasColumnType("datetime");

                entity.Property(e => e.Email)
                    .HasMaxLength(50)
                    .IsUnicode(false);

                entity.Property(e => e.FirstName)
                    .HasMaxLength(50)
                    .IsUnicode(false);

                entity.Property(e => e.Gender)
                    .HasMaxLength(50)
                    .IsUnicode(false);

                entity.Property(e => e.LastName)
                    .HasMaxLength(50)
                    .IsUnicode(false);

                entity.Property(e => e.MiddleName)
                    .HasMaxLength(50)
                    .IsUnicode(false);

                entity.Property(e => e.MobileNumber)
                    .HasMaxLength(50)
                    .IsUnicode(false);

                entity.Property(e => e.Otp)
                    .HasColumnName("OTP")
                    .HasMaxLength(50)
                    .IsUnicode(false);

                entity.Property(e => e.Password)
                    .HasMaxLength(50)
                    .IsUnicode(false);

                entity.Property(e => e.RegistrationNumber)
                    .HasMaxLength(14)
                    .IsUnicode(false);

                entity.Property(e => e.Religion)
                    .HasMaxLength(50)
                    .IsUnicode(false);

                entity.Property(e => e.State)
                    .HasMaxLength(50)
                    .IsUnicode(false);

                entity.HasOne(d => d.Department)
                    .WithMany(p => p.PatientInfo)
                    .HasForeignKey(d => d.DepartmentId)
                    .HasConstraintName("FK_PatientInfo_Department");
            });

            modelBuilder.Entity<PatientTransaction>(entity =>
            {
                entity.Property(e => e.OrderId)
                    .HasMaxLength(50)
                    .IsUnicode(false);

                entity.Property(e => e.ResponseCode)
                    .HasMaxLength(50)
                    .IsUnicode(false);

                entity.Property(e => e.StatusCode)
                    .HasMaxLength(50)
                    .IsUnicode(false);

                entity.Property(e => e.TransactionDate).HasColumnType("datetime");

                entity.Property(e => e.TransactionNumber)
                    .HasMaxLength(50)
                    .IsUnicode(false);

                entity.HasOne(d => d.Patient)
                    .WithMany(p => p.PatientTransaction)
                    .HasForeignKey(d => d.PatientId)
                    .HasConstraintName("FK_PatientTransaction_PatientInfo");
            });
        }
    }
}
