﻿// <auto-generated />

using discipline.hangfire.activity_rules.DAL;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Infrastructure;
using Microsoft.EntityFrameworkCore.Storage.ValueConversion;
using Npgsql.EntityFrameworkCore.PostgreSQL.Metadata;
using discipline.hangfire.add_activity_rules.DAL;

#nullable disable

namespace discipline.hangfire.add_activity_rules.DAL.Migrations
{
    [DbContext(typeof(ActivityRuleDbContext))]
    partial class AddActivityRuleDbContextModelSnapshot : ModelSnapshot
    {
        protected override void BuildModel(ModelBuilder modelBuilder)
        {
#pragma warning disable 612, 618
            modelBuilder
                .HasDefaultSchema("activity-rules")
                .HasAnnotation("ProductVersion", "9.0.3")
                .HasAnnotation("Relational:MaxIdentifierLength", 63);

            NpgsqlModelBuilderExtensions.UseIdentityByDefaultColumns(modelBuilder);

            modelBuilder.Entity("discipline.hangfire.add_activity_rules.Models.ActivityRule", b =>
                {
                    b.Property<string>("ActivityRuleId")
                        .HasMaxLength(26)
                        .HasColumnType("character varying(26)")
                        .HasColumnName("ActivityRuleId");

                    b.Property<string>("Mode")
                        .HasMaxLength(20)
                        .HasColumnType("character varying(20)")
                        .HasColumnName("Mode");

                    b.PrimitiveCollection<int[]>("SelectedDays")
                        .HasColumnType("integer[]")
                        .HasColumnName("SelectedDays");

                    b.Property<string>("Title")
                        .HasMaxLength(30)
                        .HasColumnType("character varying(30)")
                        .HasColumnName("Title");

                    b.Property<string>("UserId")
                        .IsRequired()
                        .HasMaxLength(26)
                        .HasColumnType("character varying(26)")
                        .HasColumnName("UserId");

                    b.HasKey("ActivityRuleId");

                    b.ToTable("ActivityRules", "activity-rules");
                });

            modelBuilder.Entity("discipline.hangfire.shared.abstractions.ViewModels.ActivityRuleViewModel", b =>
                {
                    b.Property<string>("ActivityRuleId")
                        .IsRequired()
                        .HasColumnType("text");

                    b.Property<string>("Mode")
                        .HasColumnType("text");

                    b.PrimitiveCollection<int[]>("SelectedDays")
                        .HasColumnType("integer[]");

                    b.Property<string>("UserId")
                        .IsRequired()
                        .HasColumnType("text");

                    b.ToTable((string)null);

                    b.ToView("ActivityRulesView", "activity-rules");
                });
#pragma warning restore 612, 618
        }
    }
}
