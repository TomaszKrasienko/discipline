﻿// <auto-generated />
using System;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Infrastructure;
using Microsoft.EntityFrameworkCore.Storage.ValueConversion;
using Npgsql.EntityFrameworkCore.PostgreSQL.Metadata;
using discipline.hangfire.browse_planned.DAL;

#nullable disable

namespace discipline.hangfire.browse_planned.DAL.Migrations
{
    [DbContext(typeof(BrowsePlannedDbContext))]
    partial class BrowsePlannedDbContextModelSnapshot : ModelSnapshot
    {
        protected override void BuildModel(ModelBuilder modelBuilder)
        {
#pragma warning disable 612, 618
            modelBuilder
                .HasDefaultSchema("browse-planned")
                .HasAnnotation("ProductVersion", "9.0.3")
                .HasAnnotation("Relational:MaxIdentifierLength", 63);

            NpgsqlModelBuilderExtensions.UseIdentityByDefaultColumns(modelBuilder);

            modelBuilder.Entity("discipline.hangfire.browse_planned.ViewModels.PlannedTaskViewModel", b =>
                {
                    b.Property<string>("ActivityRuleId")
                        .IsRequired()
                        .HasColumnType("text");

                    b.Property<DateTimeOffset>("CreatedAt")
                        .HasColumnType("timestamp with time zone");

                    b.Property<bool>("IsActivityCreated")
                        .HasColumnType("boolean");

                    b.Property<bool>("IsPlannedEnabled")
                        .HasColumnType("boolean");

                    b.Property<DateOnly>("PlannedFor")
                        .HasColumnType("date");

                    b.Property<string>("UserId")
                        .IsRequired()
                        .HasColumnType("text");

                    b.ToTable((string)null);

                    b.ToView("PlannedTasksView", "browse-planned");
                });
#pragma warning restore 612, 618
        }
    }
}
