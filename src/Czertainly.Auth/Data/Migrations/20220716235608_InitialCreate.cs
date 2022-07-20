using System;
using Microsoft.EntityFrameworkCore.Migrations;
using Npgsql.EntityFrameworkCore.PostgreSQL.Metadata;

#nullable disable

namespace Czertainly.Auth.Data.Migrations
{
    public partial class InitialCreate : Migration
    {
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.EnsureSchema(
                name: "auth");

            migrationBuilder.CreateTable(
                name: "resource",
                schema: "auth",
                columns: table => new
                {
                    id = table.Column<long>(type: "bigint", nullable: false)
                        .Annotation("Npgsql:ValueGenerationStrategy", NpgsqlValueGenerationStrategy.IdentityByDefaultColumn),
                    name = table.Column<string>(type: "text", nullable: false),
                    listing_endpoint = table.Column<string>(type: "text", nullable: true),
                    uuid = table.Column<Guid>(type: "uuid", nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_resource", x => x.id);
                });

            migrationBuilder.CreateTable(
                name: "role",
                schema: "auth",
                columns: table => new
                {
                    id = table.Column<long>(type: "bigint", nullable: false)
                        .Annotation("Npgsql:ValueGenerationStrategy", NpgsqlValueGenerationStrategy.IdentityByDefaultColumn),
                    name = table.Column<string>(type: "text", nullable: false),
                    description = table.Column<string>(type: "text", nullable: true),
                    uuid = table.Column<Guid>(type: "uuid", nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_role", x => x.id);
                });

            migrationBuilder.CreateTable(
                name: "user",
                schema: "auth",
                columns: table => new
                {
                    id = table.Column<long>(type: "bigint", nullable: false)
                        .Annotation("Npgsql:ValueGenerationStrategy", NpgsqlValueGenerationStrategy.IdentityByDefaultColumn),
                    username = table.Column<string>(type: "text", nullable: false),
                    first_name = table.Column<string>(type: "text", nullable: true),
                    last_name = table.Column<string>(type: "text", nullable: true),
                    email = table.Column<string>(type: "text", nullable: false),
                    enabled = table.Column<bool>(type: "boolean", nullable: false, defaultValue: true),
                    certificate_uuid = table.Column<Guid>(type: "uuid", nullable: true),
                    certificate_fingerprint = table.Column<string>(type: "text", nullable: true),
                    uuid = table.Column<Guid>(type: "uuid", nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_user", x => x.id);
                });

            migrationBuilder.CreateTable(
                name: "action",
                schema: "auth",
                columns: table => new
                {
                    id = table.Column<long>(type: "bigint", nullable: false)
                        .Annotation("Npgsql:ValueGenerationStrategy", NpgsqlValueGenerationStrategy.IdentityByDefaultColumn),
                    name = table.Column<string>(type: "text", nullable: false),
                    resource_id = table.Column<long>(type: "bigint", nullable: false),
                    uuid = table.Column<Guid>(type: "uuid", nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_action", x => x.id);
                    table.ForeignKey(
                        name: "FK_action_resource_resource_id",
                        column: x => x.resource_id,
                        principalSchema: "auth",
                        principalTable: "resource",
                        principalColumn: "id",
                        onDelete: ReferentialAction.Cascade);
                });

            migrationBuilder.CreateTable(
                name: "user_role",
                schema: "auth",
                columns: table => new
                {
                    role_id = table.Column<long>(type: "bigint", nullable: false),
                    user_id = table.Column<long>(type: "bigint", nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_user_role", x => new { x.role_id, x.user_id });
                    table.ForeignKey(
                        name: "FK_user_role_role_role_id",
                        column: x => x.role_id,
                        principalSchema: "auth",
                        principalTable: "role",
                        principalColumn: "id",
                        onDelete: ReferentialAction.Cascade);
                    table.ForeignKey(
                        name: "FK_user_role_user_user_id",
                        column: x => x.user_id,
                        principalSchema: "auth",
                        principalTable: "user",
                        principalColumn: "id",
                        onDelete: ReferentialAction.Cascade);
                });

            migrationBuilder.CreateTable(
                name: "endpoint",
                schema: "auth",
                columns: table => new
                {
                    id = table.Column<long>(type: "bigint", nullable: false)
                        .Annotation("Npgsql:ValueGenerationStrategy", NpgsqlValueGenerationStrategy.IdentityByDefaultColumn),
                    name = table.Column<string>(type: "text", nullable: false),
                    method = table.Column<string>(type: "text", nullable: false),
                    route_template = table.Column<string>(type: "text", nullable: false),
                    resource_id = table.Column<long>(type: "bigint", nullable: false),
                    action_id = table.Column<long>(type: "bigint", nullable: false),
                    uuid = table.Column<Guid>(type: "uuid", nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_endpoint", x => x.id);
                    table.ForeignKey(
                        name: "FK_endpoint_action_action_id",
                        column: x => x.action_id,
                        principalSchema: "auth",
                        principalTable: "action",
                        principalColumn: "id",
                        onDelete: ReferentialAction.Cascade);
                    table.ForeignKey(
                        name: "FK_endpoint_resource_resource_id",
                        column: x => x.resource_id,
                        principalSchema: "auth",
                        principalTable: "resource",
                        principalColumn: "id",
                        onDelete: ReferentialAction.Cascade);
                });

            migrationBuilder.CreateTable(
                name: "permission",
                schema: "auth",
                columns: table => new
                {
                    id = table.Column<long>(type: "bigint", nullable: false)
                        .Annotation("Npgsql:ValueGenerationStrategy", NpgsqlValueGenerationStrategy.IdentityByDefaultColumn),
                    role_id = table.Column<long>(type: "bigint", nullable: false),
                    resource_id = table.Column<long>(type: "bigint", nullable: true),
                    action_id = table.Column<long>(type: "bigint", nullable: true),
                    object_uuid = table.Column<Guid>(type: "uuid", nullable: true),
                    is_allowed = table.Column<bool>(type: "boolean", nullable: false, defaultValue: true),
                    uuid = table.Column<Guid>(type: "uuid", nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_permission", x => x.id);
                    table.ForeignKey(
                        name: "FK_permission_action_action_id",
                        column: x => x.action_id,
                        principalSchema: "auth",
                        principalTable: "action",
                        principalColumn: "id");
                    table.ForeignKey(
                        name: "FK_permission_resource_resource_id",
                        column: x => x.resource_id,
                        principalSchema: "auth",
                        principalTable: "resource",
                        principalColumn: "id");
                    table.ForeignKey(
                        name: "FK_permission_role_role_id",
                        column: x => x.role_id,
                        principalSchema: "auth",
                        principalTable: "role",
                        principalColumn: "id",
                        onDelete: ReferentialAction.Cascade);
                });

            migrationBuilder.CreateIndex(
                name: "IX_action_resource_id",
                schema: "auth",
                table: "action",
                column: "resource_id");

            migrationBuilder.CreateIndex(
                name: "IX_action_uuid",
                schema: "auth",
                table: "action",
                column: "uuid",
                unique: true);

            migrationBuilder.CreateIndex(
                name: "IX_endpoint_action_id",
                schema: "auth",
                table: "endpoint",
                column: "action_id");

            migrationBuilder.CreateIndex(
                name: "IX_endpoint_resource_id",
                schema: "auth",
                table: "endpoint",
                column: "resource_id");

            migrationBuilder.CreateIndex(
                name: "IX_endpoint_uuid",
                schema: "auth",
                table: "endpoint",
                column: "uuid",
                unique: true);

            migrationBuilder.CreateIndex(
                name: "IX_permission_action_id",
                schema: "auth",
                table: "permission",
                column: "action_id");

            migrationBuilder.CreateIndex(
                name: "IX_permission_resource_id",
                schema: "auth",
                table: "permission",
                column: "resource_id");

            migrationBuilder.CreateIndex(
                name: "IX_permission_role_id",
                schema: "auth",
                table: "permission",
                column: "role_id");

            migrationBuilder.CreateIndex(
                name: "IX_permission_uuid",
                schema: "auth",
                table: "permission",
                column: "uuid",
                unique: true);

            migrationBuilder.CreateIndex(
                name: "IX_resource_uuid",
                schema: "auth",
                table: "resource",
                column: "uuid",
                unique: true);

            migrationBuilder.CreateIndex(
                name: "IX_role_uuid",
                schema: "auth",
                table: "role",
                column: "uuid",
                unique: true);

            migrationBuilder.CreateIndex(
                name: "IX_user_uuid",
                schema: "auth",
                table: "user",
                column: "uuid",
                unique: true);

            migrationBuilder.CreateIndex(
                name: "IX_user_role_user_id",
                schema: "auth",
                table: "user_role",
                column: "user_id");
        }

        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropTable(
                name: "endpoint",
                schema: "auth");

            migrationBuilder.DropTable(
                name: "permission",
                schema: "auth");

            migrationBuilder.DropTable(
                name: "user_role",
                schema: "auth");

            migrationBuilder.DropTable(
                name: "action",
                schema: "auth");

            migrationBuilder.DropTable(
                name: "role",
                schema: "auth");

            migrationBuilder.DropTable(
                name: "user",
                schema: "auth");

            migrationBuilder.DropTable(
                name: "resource",
                schema: "auth");
        }
    }
}
