﻿// <auto-generated />
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Infrastructure;
using Microsoft.EntityFrameworkCore.Migrations;
using Microsoft.EntityFrameworkCore.Storage.ValueConversion;
using Npgsql.EntityFrameworkCore.PostgreSQL.Metadata;
using ShimahaiDatabase;

#nullable disable

namespace ShimaHai.Migrations
{
    [DbContext(typeof(DatabaseContext))]
    [Migration("20230915173007_0")]
    partial class _0
    {
        /// <inheritdoc />
        protected override void BuildTargetModel(ModelBuilder modelBuilder)
        {
#pragma warning disable 612, 618
            modelBuilder
                .HasAnnotation("ProductVersion", "7.0.11")
                .HasAnnotation("Relational:MaxIdentifierLength", 63);

            NpgsqlModelBuilderExtensions.UseIdentityByDefaultColumns(modelBuilder);

            modelBuilder.Entity("ShimahaiDatabase.Models.Friend", b =>
                {
                    b.Property<int>("Id")
                        .ValueGeneratedOnAdd()
                        .HasColumnType("integer")
                        .HasColumnName("id");

                    NpgsqlPropertyBuilderExtensions.UseIdentityByDefaultColumn(b.Property<int>("Id"));

                    b.Property<int>("FriendDataId")
                        .HasColumnType("integer")
                        .HasColumnName("friend_data_id");

                    b.Property<string>("Greeting")
                        .IsRequired()
                        .HasColumnType("text")
                        .HasColumnName("greeting");

                    b.Property<string>("Introduction")
                        .IsRequired()
                        .HasColumnType("text")
                        .HasColumnName("introduction");

                    b.Property<string>("Name")
                        .IsRequired()
                        .HasColumnType("text")
                        .HasColumnName("name");

                    b.Property<string>("NameCn")
                        .IsRequired()
                        .HasColumnType("text")
                        .HasColumnName("name_cn");

                    b.Property<string>("NameEn")
                        .IsRequired()
                        .HasColumnType("text")
                        .HasColumnName("name_en");

                    b.Property<string>("NameFlag")
                        .IsRequired()
                        .HasColumnType("text")
                        .HasColumnName("name_flag");

                    b.Property<string>("NameSci")
                        .IsRequired()
                        .HasColumnType("text")
                        .HasColumnName("name_sci");

                    b.Property<string>("Nickname")
                        .IsRequired()
                        .HasColumnType("text")
                        .HasColumnName("nickname");

                    b.HasKey("Id")
                        .HasName("pk_friends");

                    b.HasIndex("FriendDataId")
                        .HasDatabaseName("ix_friends_friend_data_id");

                    b.ToTable("friends", (string)null);
                });

            modelBuilder.Entity("ShimahaiDatabase.Models.FriendData", b =>
                {
                    b.Property<int>("Id")
                        .ValueGeneratedOnAdd()
                        .HasColumnType("integer")
                        .HasColumnName("id");

                    NpgsqlPropertyBuilderExtensions.UseIdentityByDefaultColumn(b.Property<int>("Id"));

                    b.Property<int>("Atk")
                        .HasColumnType("integer")
                        .HasColumnName("atk");

                    b.Property<int>("Attribute")
                        .HasColumnType("integer")
                        .HasColumnName("attribute");

                    b.Property<int>("Def")
                        .HasColumnType("integer")
                        .HasColumnName("def");

                    b.Property<double>("Evd")
                        .HasColumnType("double precision")
                        .HasColumnName("evd");

                    b.Property<int>("Hp")
                        .HasColumnType("integer")
                        .HasColumnName("hp");

                    b.Property<int>("Rarity")
                        .HasColumnType("integer")
                        .HasColumnName("rarity");

                    b.Property<int>("Status")
                        .HasColumnType("integer")
                        .HasColumnName("status");

                    b.HasKey("Id")
                        .HasName("pk_friend_data");

                    b.ToTable("friend_data", (string)null);
                });

            modelBuilder.Entity("ShimahaiDatabase.Models.Friend", b =>
                {
                    b.HasOne("ShimahaiDatabase.Models.FriendData", "FriendData")
                        .WithMany()
                        .HasForeignKey("FriendDataId")
                        .OnDelete(DeleteBehavior.Cascade)
                        .IsRequired()
                        .HasConstraintName("fk_friends_friend_data_friend_data_id");

                    b.Navigation("FriendData");
                });
#pragma warning restore 612, 618
        }
    }
}