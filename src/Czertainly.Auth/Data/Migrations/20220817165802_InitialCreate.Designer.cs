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
                .HasAnnotation("ProductVersion", "6.0.7")
                .HasAnnotation("Relational:MaxIdentifierLength", 63);

            NpgsqlModelBuilderExtensions.UseIdentityByDefaultColumns(modelBuilder);

            modelBuilder.Entity("Czertainly.Auth.Models.Entities.Action", b =>
                {
                    b.Property<Guid>("Uuid")
                        .ValueGeneratedOnAdd()
                        .HasColumnType("uuid")
                        .HasColumnName("uuid")
                        .HasColumnOrder(0);

                    b.Property<string>("Name")
                        .IsRequired()
                        .HasColumnType("text")
                        .HasColumnName("name");

                    b.Property<Guid>("ResourceUuid")
                        .HasColumnType("uuid")
                        .HasColumnName("resource_uuid");

                    b.HasKey("Uuid");

                    b.HasIndex("ResourceUuid");

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
                        .ValueGeneratedOnAdd()
                        .HasColumnType("boolean")
                        .HasDefaultValue(true)
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
                });

            modelBuilder.Entity("Czertainly.Auth.Models.Entities.Resource", b =>
                {
                    b.Property<Guid>("Uuid")
                        .ValueGeneratedOnAdd()
                        .HasColumnType("uuid")
                        .HasColumnName("uuid")
                        .HasColumnOrder(0);

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

                    b.HasKey("Uuid");

                    b.HasIndex("Name")
                        .IsUnique();

                    b.ToTable("role", "auth");
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
                        .IsRequired()
                        .HasColumnType("text")
                        .HasColumnName("email");

                    b.Property<bool>("Enabled")
                        .ValueGeneratedOnAdd()
                        .HasColumnType("boolean")
                        .HasDefaultValue(true)
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
                });

            modelBuilder.Entity("Czertainly.Auth.Models.Entities.Action", b =>
                {
                    b.HasOne("Czertainly.Auth.Models.Entities.Resource", "Resource")
                        .WithMany()
                        .HasForeignKey("ResourceUuid")
                        .OnDelete(DeleteBehavior.Cascade)
                        .IsRequired();

                    b.Navigation("Resource");
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