using Microsoft.EntityFrameworkCore.Metadata;
using Microsoft.EntityFrameworkCore.Migrations;
using System;
using System.Collections.Generic;

namespace SS.Web.Security.Data.Migrations.SS
{
    public partial class externalprovidersschema : Migration
    {
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.EnsureSchema(
                name: "ss");

            migrationBuilder.CreateTable(
                name: "ExternalIdentityProvider",
                schema: "ss",
                columns: table => new
                {
                    Id = table.Column<int>(nullable: false)
                        .Annotation("SqlServer:ValueGenerationStrategy", SqlServerValueGenerationStrategy.IdentityColumn),
                    AlternateIdClaimType = table.Column<string>(maxLength: 512, nullable: true),
                    AlternateIdSourceKey = table.Column<string>(maxLength: 450, nullable: true),
                    AuthenticationProtocol = table.Column<int>(nullable: false),
                    AuthenticationScheme = table.Column<string>(maxLength: 50, nullable: false),
                    DisplayName = table.Column<string>(maxLength: 200, nullable: false),
                    AllowClaimsFromUserInfoEndpoint = table.Column<bool>(nullable: true),
                    Authority = table.Column<string>(maxLength: 200, nullable: true),
                    ClientId = table.Column<string>(maxLength: 200, nullable: true),
                    ClientSecretName = table.Column<string>(maxLength: 100, nullable: true),
                    RequireHttpsMetadata = table.Column<bool>(nullable: true),
                    SaveTokens = table.Column<bool>(nullable: true),
                    SignInScheme = table.Column<string>(maxLength: 50, nullable: true),
                    JsonConfiguration = table.Column<string>(nullable: true)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_ExternalIdentityProvider", x => x.Id);
                    table.UniqueConstraint("AK_ExternalIdentityProvider_AuthenticationScheme", x => x.AuthenticationScheme);
                });

            migrationBuilder.CreateIndex(
                name: "IX_ExternalIdentityProvider_AuthenticationScheme",
                schema: "ss",
                table: "ExternalIdentityProvider",
                column: "AuthenticationScheme");
        }

        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropTable(
                name: "ExternalIdentityProvider",
                schema: "ss");
        }
    }
}
