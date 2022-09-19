﻿// <auto-generated />
using System;
using Czertainly.Auth.Data;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Infrastructure;
using Microsoft.EntityFrameworkCore.Migrations;
using Microsoft.EntityFrameworkCore.Storage.ValueConversion;
using Npgsql.EntityFrameworkCore.PostgreSQL.Metadata;

#nullable disable

namespace Czertainly.Auth.Data.Migrations
{
    [DbContext(typeof(AuthDbContext))]
    [Migration("20220817165802_InitialCreate")]
    partial class InitialCreate
    {
        protected override void BuildTargetModel(ModelBuilder modelBuilder)
        {
#pragma warning disable 612, 618
            modelBuilder
                .HasDefaultSchema("auth")
                .HasAnnotation("ProductVersion", "6.0.9")
                .HasAnnotation("Relational:MaxIdentifierLength", 63);

            NpgsqlModelBuilderExtensions.UseIdentityByDefaultColumns(modelBuilder);

            modelBuilder.Entity("Czertainly.Auth.Models.Entities.Action", b =>
                {
                    b.Property<Guid>("Uuid")
                        .ValueGeneratedOnAdd()
                        .HasColumnType("uuid")
                        .HasColumnName("uuid")
                        .HasColumnOrder(0);

                    b.Property<string>("DisplayName")
                        .IsRequired()
                        .HasColumnType("text")
                        .HasColumnName("display_name");

                    b.Property<string>("Name")
                        .IsRequired()
                        .HasColumnType("text")
                        .HasColumnName("name");

                    b.HasKey("Uuid");

                    b.HasIndex("Name")
                        .IsUnique();

                    b.ToTable("action", "auth");
                });

            modelBuilder.Entity("Czertainly.Auth.Models.Entities.Endpoint", b =>
                {
                    b.Property<Guid>("Uuid")
                        .ValueGeneratedOnAdd()
                        .HasColumnType("uuid")
                        .HasColumnName("uuid")
                        .HasColumnOrder(0);

                    b.Property<Guid?>("ActionUuid")
                        .HasColumnType("uuid")
                        .HasColumnName("action_uuid");

                    b.Property<string>("Method")
                        .IsRequired()
                        .HasColumnType("text")
                        .HasColumnName("method");

                    b.Property<string>("Name")
                        .IsRequired()
                        .HasColumnType("text")
                        .HasColumnName("name");

                    b.Property<Guid>("ResourceUuid")
                        .HasColumnType("uuid")
                        .HasColumnName("resource_uuid");

                    b.Property<string>("RouteTemplate")
                        .IsRequired()
                        .HasColumnType("text")
                        .HasColumnName("route_template");

                    b.HasKey("Uuid");

                    b.HasIndex("ActionUuid");

                    b.HasIndex("ResourceUuid");

                    b.ToTable("endpoint", "auth");
                });

            modelBuilder.Entity("Czertainly.Auth.Models.Entities.Permission", b =>
                {
                    b.Property<Guid>("Uuid")
                        .ValueGeneratedOnAdd()
                        .HasColumnType("uuid")
                        .HasColumnName("uuid")
                        .HasColumnOrder(0);

                    b.Property<Guid?>("ActionUuid")
                        .HasColumnType("uuid")
                        .HasColumnName("action_uuid");

                    b.Property<bool>("IsAllowed")
                        .HasColumnType("boolean")
                        .HasColumnName("is_allowed");

                    b.Property<Guid?>("ObjectUuid")
                        .HasColumnType("uuid")
                        .HasColumnName("object_uuid");

                    b.Property<Guid?>("ResourceUuid")
                        .HasColumnType("uuid")
                        .HasColumnName("resource_uuid");

                    b.Property<Guid>("RoleUuid")
                        .HasColumnType("uuid")
                        .HasColumnName("role_uuid");

                    b.HasKey("Uuid");

                    b.HasIndex("ActionUuid");

                    b.HasIndex("ResourceUuid");

                    b.HasIndex("RoleUuid");

                    b.ToTable("permission", "auth");

                    b.HasData(
                        new
                        {
                            Uuid = new Guid("3053b9c9-239d-4717-9d23-97e01177a40b"),
                            IsAllowed = true,
                            RoleUuid = new Guid("d34f960b-75c9-4184-ba97-665d30a9ee8a")
                        });
                });

            modelBuilder.Entity("Czertainly.Auth.Models.Entities.Resource", b =>
                {
                    b.Property<Guid>("Uuid")
                        .ValueGeneratedOnAdd()
                        .HasColumnType("uuid")
                        .HasColumnName("uuid")
                        .HasColumnOrder(0);

                    b.Property<string>("DisplayName")
                        .IsRequired()
                        .HasColumnType("text")
                        .HasColumnName("display_name");

                    b.Property<string>("ListingEndpoint")
                        .HasColumnType("text")
                        .HasColumnName("listing_endpoint");

                    b.Property<string>("Name")
                        .IsRequired()
                        .HasColumnType("text")
                        .HasColumnName("name");

                    b.HasKey("Uuid");

                    b.HasIndex("Name")
                        .IsUnique();

                    b.ToTable("resource", "auth");
                });

            modelBuilder.Entity("Czertainly.Auth.Models.Entities.Role", b =>
                {
                    b.Property<Guid>("Uuid")
                        .ValueGeneratedOnAdd()
                        .HasColumnType("uuid")
                        .HasColumnName("uuid")
                        .HasColumnOrder(0);

                    b.Property<string>("Description")
                        .HasColumnType("text")
                        .HasColumnName("description");

                    b.Property<string>("Name")
                        .IsRequired()
                        .HasColumnType("text")
                        .HasColumnName("name");

                    b.Property<bool>("SystemRole")
                        .HasColumnType("boolean")
                        .HasColumnName("system_role");

                    b.HasKey("Uuid");

                    b.HasIndex("Name")
                        .IsUnique();

                    b.ToTable("role", "auth");

                    b.HasData(
                        new
                        {
                            Uuid = new Guid("d34f960b-75c9-4184-ba97-665d30a9ee8a"),
                            Description = "Internal Czertianly system role with all permissions",
                            Name = "Superadmin",
                            SystemRole = true
                        },
                        new
                        {
                            Uuid = new Guid("da5668e2-9d94-4375-98c4-d665083edceb"),
                            Description = "Internal Czertianly system role with all administrating permissions",
                            Name = "Admin",
                            SystemRole = true
                        },
                        new
                        {
                            Uuid = new Guid("deb8ad2c-3652-489c-b370-f36fe9703803"),
                            Description = "Internal Czertianly system role with client operations permissions",
                            Name = "Operator",
                            SystemRole = true
                        });
                });

            modelBuilder.Entity("Czertainly.Auth.Models.Entities.User", b =>
                {
                    b.Property<Guid>("Uuid")
                        .ValueGeneratedOnAdd()
                        .HasColumnType("uuid")
                        .HasColumnName("uuid")
                        .HasColumnOrder(0);

                    b.Property<string>("CertificateFingerprint")
                        .HasColumnType("text")
                        .HasColumnName("certificate_fingerprint");

                    b.Property<Guid?>("CertificateUuid")
                        .HasColumnType("uuid")
                        .HasColumnName("certificate_uuid");

                    b.Property<string>("Email")
                        .HasColumnType("text")
                        .HasColumnName("email");

                    b.Property<bool>("Enabled")
                        .HasColumnType("boolean")
                        .HasColumnName("enabled");

                    b.Property<string>("FirstName")
                        .HasColumnType("text")
                        .HasColumnName("first_name");

                    b.Property<string>("LastName")
                        .HasColumnType("text")
                        .HasColumnName("last_name");

                    b.Property<bool>("SystemUser")
                        .HasColumnType("boolean")
                        .HasColumnName("system_user");

                    b.Property<string>("Username")
                        .IsRequired()
                        .HasColumnType("text")
                        .HasColumnName("username");

                    b.HasKey("Uuid");

                    b.HasIndex("Username")
                        .IsUnique();

                    b.ToTable("user", "auth");

                    b.HasData(
                        new
                        {
                            Uuid = new Guid("967679bd-0b75-41eb-8e9e-fef1a5ba4aa6"),
                            CertificateFingerprint = "e1481e7eb80a265189da1c42c21066b006ed46afc1b55dd610a31bb8ec5da8b8",
                            Email = "superadmin@czertainly.com",
                            Enabled = true,
                            SystemUser = true,
                            Username = "superadmin"
                        },
                        new
                        {
                            Uuid = new Guid("64050556-dce6-42f8-81b6-96e521dd64d7"),
                            Email = "admin@czertainly.com",
                            Enabled = true,
                            SystemUser = true,
                            Username = "admin"
                        },
                        new
                        {
                            Uuid = new Guid("3e544eb1-2ec5-40ac-b72e-d8f765413cea"),
                            Email = "operator@czertainly.com",
                            Enabled = true,
                            SystemUser = true,
                            Username = "operator"
                        });
                });

            modelBuilder.Entity("resource_action", b =>
                {
                    b.Property<Guid>("action_uuid")
                        .HasColumnType("uuid");

                    b.Property<Guid>("resource_uuid")
                        .HasColumnType("uuid");

                    b.HasKey("action_uuid", "resource_uuid");

                    b.HasIndex("resource_uuid");

                    b.ToTable("resource_action", "auth");
                });

            modelBuilder.Entity("user_role", b =>
                {
                    b.Property<Guid>("role_uuid")
                        .HasColumnType("uuid");

                    b.Property<Guid>("user_uuid")
                        .HasColumnType("uuid");

                    b.HasKey("role_uuid", "user_uuid");

                    b.HasIndex("user_uuid");

                    b.ToTable("user_role", "auth");

                    b.HasData(
                        new
                        {
                            role_uuid = new Guid("d34f960b-75c9-4184-ba97-665d30a9ee8a"),
                            user_uuid = new Guid("967679bd-0b75-41eb-8e9e-fef1a5ba4aa6")
                        },
                        new
                        {
                            role_uuid = new Guid("da5668e2-9d94-4375-98c4-d665083edceb"),
                            user_uuid = new Guid("64050556-dce6-42f8-81b6-96e521dd64d7")
                        },
                        new
                        {
                            role_uuid = new Guid("deb8ad2c-3652-489c-b370-f36fe9703803"),
                            user_uuid = new Guid("3e544eb1-2ec5-40ac-b72e-d8f765413cea")
                        });
                });

            modelBuilder.Entity("Czertainly.Auth.Models.Entities.Endpoint", b =>
                {
                    b.HasOne("Czertainly.Auth.Models.Entities.Action", "Action")
                        .WithMany()
                        .HasForeignKey("ActionUuid");

                    b.HasOne("Czertainly.Auth.Models.Entities.Resource", "Resource")
                        .WithMany()
                        .HasForeignKey("ResourceUuid")
                        .OnDelete(DeleteBehavior.Cascade)
                        .IsRequired();

                    b.Navigation("Action");

                    b.Navigation("Resource");
                });

            modelBuilder.Entity("Czertainly.Auth.Models.Entities.Permission", b =>
                {
                    b.HasOne("Czertainly.Auth.Models.Entities.Action", "Action")
                        .WithMany()
                        .HasForeignKey("ActionUuid");

                    b.HasOne("Czertainly.Auth.Models.Entities.Resource", "Resource")
                        .WithMany()
                        .HasForeignKey("ResourceUuid");

                    b.HasOne("Czertainly.Auth.Models.Entities.Role", "Role")
                        .WithMany("Permissions")
                        .HasForeignKey("RoleUuid")
                        .OnDelete(DeleteBehavior.Cascade)
                        .IsRequired();

                    b.Navigation("Action");

                    b.Navigation("Resource");

                    b.Navigation("Role");
                });

            modelBuilder.Entity("resource_action", b =>
                {
                    b.HasOne("Czertainly.Auth.Models.Entities.Action", null)
                        .WithMany()
                        .HasForeignKey("action_uuid")
                        .OnDelete(DeleteBehavior.Cascade)
                        .IsRequired();

                    b.HasOne("Czertainly.Auth.Models.Entities.Resource", null)
                        .WithMany()
                        .HasForeignKey("resource_uuid")
                        .OnDelete(DeleteBehavior.Cascade)
                        .IsRequired();
                });

            modelBuilder.Entity("user_role", b =>
                {
                    b.HasOne("Czertainly.Auth.Models.Entities.Role", null)
                        .WithMany()
                        .HasForeignKey("role_uuid")
                        .OnDelete(DeleteBehavior.Cascade)
                        .IsRequired();

                    b.HasOne("Czertainly.Auth.Models.Entities.User", null)
                        .WithMany()
                        .HasForeignKey("user_uuid")
                        .OnDelete(DeleteBehavior.Cascade)
                        .IsRequired();
                });

            modelBuilder.Entity("Czertainly.Auth.Models.Entities.Role", b =>
                {
                    b.Navigation("Permissions");
                });
#pragma warning restore 612, 618
        }
    }
}
