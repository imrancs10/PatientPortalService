﻿// <auto-generated />
using System;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Infrastructure;
using Microsoft.EntityFrameworkCore.Metadata;
using Microsoft.EntityFrameworkCore.Migrations;
using Microsoft.EntityFrameworkCore.Storage.ValueConversion;
using SWN.MobileService.Api.Data;

namespace SWN.MobileService.Api.Migrations
{
    [DbContext(typeof(MobileServiceContext))]
    [Migration("20180725121841_Initial")]
    partial class Initial
    {
        protected override void BuildTargetModel(ModelBuilder modelBuilder)
        {
#pragma warning disable 612, 618
            modelBuilder
                .HasAnnotation("ProductVersion", "2.1.1-rtm-30846")
                .HasAnnotation("Relational:MaxIdentifierLength", 128)
                .HasAnnotation("SqlServer:ValueGenerationStrategy", SqlServerValueGenerationStrategy.IdentityColumn);

            modelBuilder.Entity("SWN.MobileService.Api.Models.MessageDetail", b =>
                {
                    b.Property<int>("Id")
                        .ValueGeneratedOnAdd()
                        .HasAnnotation("SqlServer:ValueGenerationStrategy", SqlServerValueGenerationStrategy.IdentityColumn);

                    b.Property<long>("ContactPointId");

                    b.Property<DateTime>("ExpirationDate");

                    b.Property<string>("MessageText")
                        .IsRequired();

                    b.Property<long>("MessageTransactionId");

                    b.Property<int>("MessageType");

                    b.Property<long>("RecipientId");

                    b.HasKey("Id");

                    b.ToTable("MessageDetails");
                });

            modelBuilder.Entity("SWN.MobileService.Api.Models.MobileRecipient", b =>
                {
                    b.Property<long>("Id")
                        .ValueGeneratedOnAdd()
                        .HasAnnotation("SqlServer:ValueGenerationStrategy", SqlServerValueGenerationStrategy.IdentityColumn);

                    b.Property<int?>("MessageDetailId");

                    b.Property<long>("MobileUserId");

                    b.HasKey("Id");

                    b.HasIndex("MessageDetailId");

                    b.ToTable("MobileRecipients");
                });

            modelBuilder.Entity("SWN.MobileService.Api.Models.MobileRecipient", b =>
                {
                    b.HasOne("SWN.MobileService.Api.Models.MessageDetail")
                        .WithMany("MobileRecipients")
                        .HasForeignKey("MessageDetailId");
                });
#pragma warning restore 612, 618
        }
    }
}
