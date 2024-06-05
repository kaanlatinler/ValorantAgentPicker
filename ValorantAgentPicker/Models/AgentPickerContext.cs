using System;
using System.Collections.Generic;
using Microsoft.EntityFrameworkCore;

namespace ValorantAgentPicker.Models;

public partial class AgentPickerContext : DbContext
{
    public AgentPickerContext()
    {
    }

    public AgentPickerContext(DbContextOptions<AgentPickerContext> options)
        : base(options)
    {
    }

    public virtual DbSet<Agent> Agents { get; set; }

    public virtual DbSet<Profile> Profiles { get; set; }

    protected override void OnConfiguring(DbContextOptionsBuilder optionsBuilder)
#warning To protect potentially sensitive information in your connection string, you should move it out of source code. You can avoid scaffolding the connection string by using the Name= syntax to read it from configuration - see https://go.microsoft.com/fwlink/?linkid=2131148. For more guidance on storing connection strings, see https://go.microsoft.com/fwlink/?LinkId=723263.
        => optionsBuilder.UseSqlServer("Data Source=KAAN\\SQLEXPRESS;Initial Catalog=AgentPicker;Integrated Security=True;Encrypt=True;Trust Server Certificate=True");

    protected override void OnModelCreating(ModelBuilder modelBuilder)
    {
        modelBuilder.Entity<Agent>(entity =>
        {
            entity.HasKey(e => e.AgentId).HasName("PK__Agents__9AC3BFD1D864193D");

            entity.Property(e => e.AgentId).HasColumnName("AgentID");
            entity.Property(e => e.AgentName).HasMaxLength(50);
        });

        modelBuilder.Entity<Profile>(entity =>
        {
            entity.HasKey(e => e.ProfileId).HasName("PK__Profiles__290C88841B90EF26");

            entity.Property(e => e.ProfileId).HasColumnName("ProfileID");
            entity.Property(e => e.AgentId).HasColumnName("AgentID");

            entity.HasOne(d => d.Agent).WithMany(p => p.Profiles)
                .HasForeignKey(d => d.AgentId)
                .HasConstraintName("FK_Profiles_Agents");
        });

        OnModelCreatingPartial(modelBuilder);
    }

    partial void OnModelCreatingPartial(ModelBuilder modelBuilder);
}
