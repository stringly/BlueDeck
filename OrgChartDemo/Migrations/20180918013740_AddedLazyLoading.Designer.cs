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
    [DbContext(typeof(ApplicationDbContext))]
    [Migration("20180918013740_AddedLazyLoading")]
    partial class AddedLazyLoading
    {
#pragma warning disable 1591
        protected override void BuildTargetModel(ModelBuilder modelBuilder)
        {
#pragma warning disable 612, 618
            modelBuilder
                .HasAnnotation("ProductVersion", "2.1.1-rtm-30846")
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

                    b.Property<int?>("PositionId");

                    b.Property<int?>("RankId");

                    b.HasKey("MemberId");

                    b.HasIndex("PositionId");

                    b.HasIndex("RankId");

                    b.ToTable("Members");
                });

            modelBuilder.Entity("OrgChartDemo.Models.Position", b =>
                {
                    b.Property<int>("PositionId")
                        .ValueGeneratedOnAdd()
                        .HasAnnotation("SqlServer:ValueGenerationStrategy", SqlServerValueGenerationStrategy.IdentityColumn);

                    b.Property<bool>("IsManager");

                    b.Property<bool>("IsUnique");

                    b.Property<string>("JobTitle");

                    b.Property<string>("Name");

                    b.Property<int?>("ParentComponentComponentId");

                    b.HasKey("PositionId");

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
                        .HasForeignKey("PositionId");

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
#pragma warning restore 1591
        }
    }
}
