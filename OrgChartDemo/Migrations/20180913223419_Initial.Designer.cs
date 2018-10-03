﻿// <auto-generated />
using System;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Infrastructure;
using Microsoft.EntityFrameworkCore.Metadata;
using Microsoft.EntityFrameworkCore.Migrations;
using Microsoft.EntityFrameworkCore.Storage.ValueConversion;
using OrgChartDemo.Models;

namespace OrgChartDemo.Migrations
{
#pragma warning disable 1591
    [DbContext(typeof(ApplicationDbContext))]
    [Migration("20180913223419_Initial")]
    partial class Initial
    {
        protected override void BuildTargetModel(ModelBuilder modelBuilder)
        {
#pragma warning disable 612, 618
            modelBuilder
                .HasAnnotation("ProductVersion", "2.1.2-rtm-30932")
                .HasAnnotation("Relational:MaxIdentifierLength", 128)
                .HasAnnotation("SqlServer:ValueGenerationStrategy", SqlServerValueGenerationStrategy.IdentityColumn);

            modelBuilder.Entity("OrgChartDemo.Models.Component", b =>
                {
                    b.Property<int>("ComponentId")
                        .ValueGeneratedOnAdd()
                        .HasAnnotation("SqlServer:ValueGenerationStrategy", SqlServerValueGenerationStrategy.IdentityColumn);

                    b.Property<string>("Acronym");

                    b.Property<string>("Name");

                    b.Property<int?>("ParentComponentComponentId");

                    b.HasKey("ComponentId");

                    b.HasIndex("ParentComponentComponentId");

                    b.ToTable("Components");
                });

            modelBuilder.Entity("OrgChartDemo.Models.Member", b =>
                {
                    b.Property<int>("MemberId")
                        .ValueGeneratedOnAdd()
                        .HasAnnotation("SqlServer:ValueGenerationStrategy", SqlServerValueGenerationStrategy.IdentityColumn);

                    b.Property<string>("Email");

                    b.Property<string>("FirstName");

                    b.Property<string>("IdNumber");

                    b.Property<string>("LastName");

                    b.Property<string>("MiddleName");

                    b.Property<int?>("PositionPostionId");

                    b.Property<int?>("RankId");

                    b.HasKey("MemberId");

                    b.HasIndex("PositionPostionId");

                    b.HasIndex("RankId");

                    b.ToTable("Members");
                });

            modelBuilder.Entity("OrgChartDemo.Models.Position", b =>
                {
                    b.Property<int>("PostionId")
                        .ValueGeneratedOnAdd()
                        .HasAnnotation("SqlServer:ValueGenerationStrategy", SqlServerValueGenerationStrategy.IdentityColumn);

                    b.Property<bool>("IsUnique");

                    b.Property<string>("JobTitle");

                    b.Property<string>("Name");

                    b.Property<int?>("ParentComponentComponentId");

                    b.HasKey("PostionId");

                    b.HasIndex("ParentComponentComponentId");

                    b.ToTable("Positions");
                });

            modelBuilder.Entity("OrgChartDemo.Models.Types.MemberRank", b =>
                {
                    b.Property<int>("RankId")
                        .ValueGeneratedOnAdd()
                        .HasAnnotation("SqlServer:ValueGenerationStrategy", SqlServerValueGenerationStrategy.IdentityColumn);

                    b.Property<string>("PayGrade");

                    b.Property<string>("RankFullName");

                    b.Property<string>("RankShort");

                    b.HasKey("RankId");

                    b.ToTable("MemberRank");
                });

            modelBuilder.Entity("OrgChartDemo.Models.Component", b =>
                {
                    b.HasOne("OrgChartDemo.Models.Component", "ParentComponent")
                        .WithMany()
                        .HasForeignKey("ParentComponentComponentId");
                });

            modelBuilder.Entity("OrgChartDemo.Models.Member", b =>
                {
                    b.HasOne("OrgChartDemo.Models.Position")
                        .WithMany("Members")
                        .HasForeignKey("PositionPostionId");

                    b.HasOne("OrgChartDemo.Models.Types.MemberRank", "Rank")
                        .WithMany()
                        .HasForeignKey("RankId");
                });

            modelBuilder.Entity("OrgChartDemo.Models.Position", b =>
                {
                    b.HasOne("OrgChartDemo.Models.Component", "ParentComponent")
                        .WithMany("Positions")
                        .HasForeignKey("ParentComponentComponentId");
                });
#pragma warning restore 612, 618
        }
    }
#pragma warning restore 1591
}
