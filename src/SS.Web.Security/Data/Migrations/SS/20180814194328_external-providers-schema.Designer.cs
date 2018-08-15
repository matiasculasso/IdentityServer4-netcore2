﻿// <auto-generated />
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Infrastructure;
using Microsoft.EntityFrameworkCore.Metadata;
using Microsoft.EntityFrameworkCore.Migrations;
using Microsoft.EntityFrameworkCore.Storage;
using Microsoft.EntityFrameworkCore.Storage.Internal;
using Microsoft.EntityFrameworkCore.ValueGeneration;
using SS.Web.Security.Data;
using SS.Web.Security.Entities;
using System;

namespace SS.Web.Security.Data.Migrations.SS
{
    [DbContext(typeof(SSDbContext))]
    [Migration("20180814194328_external-providers-schema")]
    partial class externalprovidersschema
    {
        protected override void BuildTargetModel(ModelBuilder modelBuilder)
        {
#pragma warning disable 612, 618
            modelBuilder
                .HasAnnotation("ProductVersion", "2.0.1-rtm-125")
                .HasAnnotation("SqlServer:ValueGenerationStrategy", SqlServerValueGenerationStrategy.IdentityColumn);

            modelBuilder.Entity("SS.Web.Security.Entities.ExternalIdentityProvider", b =>
                {
                    b.Property<int>("Id")
                        .ValueGeneratedOnAdd();

                    b.Property<string>("AlternateIdClaimType")
                        .HasMaxLength(512);

                    b.Property<string>("AlternateIdSourceKey")
                        .HasMaxLength(450);

                    b.Property<int>("AuthenticationProtocol");

                    b.Property<string>("AuthenticationScheme")
                        .IsRequired()
                        .HasMaxLength(50);

                    b.Property<string>("DisplayName")
                        .IsRequired()
                        .HasMaxLength(200);

                    b.HasKey("Id");

                    b.HasAlternateKey("AuthenticationScheme");

                    b.HasIndex("AuthenticationScheme");

                    b.ToTable("ExternalIdentityProvider","ss");

                    b.HasDiscriminator<int>("AuthenticationProtocol");
                });

            modelBuilder.Entity("SS.Web.Security.Entities.LDAPIdentityProvider", b =>
                {
                    b.HasBaseType("SS.Web.Security.Entities.ExternalIdentityProvider");


                    b.ToTable("LDAPIdentityProvider");

                    b.HasDiscriminator().HasValue(3);
                });

            modelBuilder.Entity("SS.Web.Security.Entities.OpenIdConnectIdentityProvider", b =>
                {
                    b.HasBaseType("SS.Web.Security.Entities.ExternalIdentityProvider");

                    b.Property<bool?>("AllowClaimsFromUserInfoEndpoint")
                        .IsRequired();

                    b.Property<string>("Authority")
                        .IsRequired()
                        .HasMaxLength(200);

                    b.Property<string>("ClientId")
                        .IsRequired()
                        .HasMaxLength(200);

                    b.Property<string>("ClientSecretName")
                        .HasMaxLength(100);

                    b.Property<bool>("RequireHttpsMetadata");

                    b.Property<bool>("SaveTokens");

                    b.Property<string>("SignInScheme")
                        .IsRequired()
                        .HasMaxLength(50);

                    b.ToTable("OpenIdConnectIdentityProvider");

                    b.HasDiscriminator().HasValue(1);
                });

            modelBuilder.Entity("SS.Web.Security.Entities.SAMLIdentityProvider", b =>
                {
                    b.HasBaseType("SS.Web.Security.Entities.ExternalIdentityProvider");

                    b.Property<string>("JsonConfiguration")
                        .IsRequired();

                    b.ToTable("SAMLIdentityProvider");

                    b.HasDiscriminator().HasValue(2);
                });
#pragma warning restore 612, 618
        }
    }
}