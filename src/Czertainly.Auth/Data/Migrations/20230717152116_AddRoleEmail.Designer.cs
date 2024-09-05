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
    [Migration("20230717152116_AddRoleEmail")]
    partial class AddRoleEmail
    {
        protected override void BuildTargetModel(ModelBuilder modelBuilder)
        {
#pragma warning disable 612, 618
            modelBuilder
                .HasDefaultSchema("auth")
                .HasAnnotation("ProductVersion", "6.0.20")
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

                    b.Property<string>("ObjectName")
                        .HasColumnType("text")
                        .HasColumnName("object_name");

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

                    b.Property<string>("DisplayName")
                        .IsRequired()
                        .HasColumnType("text")
                        .HasColumnName("display_name");

                    b.Property<string>("ListObjectsEndpoint")
                        .HasColumnType("text")
                        .HasColumnName("list_objects_endpoint");

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

                    b.Property<DateTimeOffset>("CreatedAt")
                        .HasColumnType("timestamp with time zone")
                        .HasColumnName("created_at");

                    b.Property<string>("Description")
                        .HasColumnType("text")
                        .HasColumnName("description");

                    b.Property<string>("Email")
                        .HasColumnType("text")
                        .HasColumnName("email");

                    b.Property<string>("Name")
                        .IsRequired()
                        .HasColumnType("text")
                        .HasColumnName("name");

                    b.Property<bool>("SystemRole")
                        .HasColumnType("boolean")
                        .HasColumnName("system_role");

                    b.Property<DateTimeOffset>("UpdatedAt")
                        .HasColumnType("timestamp with time zone")
                        .HasColumnName("updated_at");

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

                    b.Property<string>("AuthTokenSubjectId")
                        .HasColumnType("text")
                        .HasColumnName("auth_token_sub");

                    b.Property<string>("CertificateFingerprint")
                        .HasColumnType("text")
                        .HasColumnName("certificate_fingerprint");

                    b.Property<Guid?>("CertificateUuid")
                        .HasColumnType("uuid")
                        .HasColumnName("certificate_uuid");

                    b.Property<DateTimeOffset>("CreatedAt")
                        .HasColumnType("timestamp with time zone")
                        .HasColumnName("created_at");

                    b.Property<string>("Description")
                        .HasColumnType("text")
                        .HasColumnName("description");

                    b.Property<string>("Email")
                        .HasColumnType("text")
                        .HasColumnName("email");

                    b.Property<bool>("Enabled")
                        .HasColumnType("boolean")
                        .HasColumnName("enabled");

                    b.Property<string>("FirstName")
                        .HasColumnType("text")
                        .HasColumnName("first_name");

                    b.Property<string>("GroupName")
                        .HasColumnType("text")
                        .HasColumnName("group_name");

                    b.Property<Guid?>("GroupUuid")
                        .HasColumnType("uuid")
                        .HasColumnName("group_uuid");

                    b.Property<string>("LastName")
                        .HasColumnType("text")
                        .HasColumnName("last_name");

                    b.Property<bool>("SystemUser")
                        .HasColumnType("boolean")
                        .HasColumnName("is_system_user");

                    b.Property<DateTimeOffset>("UpdatedAt")
                        .HasColumnType("timestamp with time zone")
                        .HasColumnName("updated_at");

                    b.Property<string>("Username")
                        .IsRequired()
                        .HasColumnType("text")
                        .HasColumnName("username");

                    b.HasKey("Uuid");

                    b.HasIndex("Username")
                        .IsUnique();

                    b.ToTable("user", "auth");
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
                });

            modelBuilder.Entity("Czertainly.Auth.Models.Entities.Permission", b =>
                {
                    b.HasOne("Czertainly.Auth.Models.Entities.Action", "Action")
                        .WithMany("Permissions")
                        .HasForeignKey("ActionUuid")
                        .OnDelete(DeleteBehavior.ClientCascade);

                    b.HasOne("Czertainly.Auth.Models.Entities.Resource", "Resource")
                        .WithMany("Permissions")
                        .HasForeignKey("ResourceUuid")
                        .OnDelete(DeleteBehavior.ClientCascade);

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

            modelBuilder.Entity("Czertainly.Auth.Models.Entities.Action", b =>
                {
                    b.Navigation("Permissions");
                });

            modelBuilder.Entity("Czertainly.Auth.Models.Entities.Resource", b =>
                {
                    b.Navigation("Permissions");
                });

            modelBuilder.Entity("Czertainly.Auth.Models.Entities.Role", b =>
                {
                    b.Navigation("Permissions");
                });
#pragma warning restore 612, 618
        }
    }
}
