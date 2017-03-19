using System;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Infrastructure;
using Microsoft.EntityFrameworkCore.Metadata;
using Microsoft.EntityFrameworkCore.Migrations;
using ASPCoreCustomModelBinder.Models;

namespace ASPCoreCustomModelBinder.Migrations
{
    [DbContext(typeof(DataContext))]
    [Migration("20170319135324_initial-migration")]
    partial class initialmigration
    {
        protected override void BuildTargetModel(ModelBuilder modelBuilder)
        {
            modelBuilder
                .HasAnnotation("ProductVersion", "1.1.1")
                .HasAnnotation("SqlServer:ValueGenerationStrategy", SqlServerValueGenerationStrategy.IdentityColumn);

            modelBuilder.Entity("ASPCoreCustomModelBinder.Models.Appointment", b =>
                {
                    b.Property<string>("Id")
                        .ValueGeneratedOnAdd();

                    b.Property<DateTime>("AppointmentDate");

                    b.Property<string>("Name");

                    b.HasKey("Id");

                    b.ToTable("Appointment");
                });
        }
    }
}
