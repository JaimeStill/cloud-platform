﻿// <auto-generated />
using System;
using CloudPlatform.Data;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Infrastructure;
using Microsoft.EntityFrameworkCore.Metadata;
using Microsoft.EntityFrameworkCore.Storage.ValueConversion;

namespace CloudPlatform.Data.Migrations
{
    [DbContext(typeof(AppDbContext))]
    partial class AppDbContextModelSnapshot : ModelSnapshot
    {
        protected override void BuildModel(ModelBuilder modelBuilder)
        {
#pragma warning disable 612, 618
            modelBuilder
                .UseIdentityColumns()
                .HasAnnotation("Relational:MaxIdentifierLength", 128)
                .HasAnnotation("ProductVersion", "5.0.0");

            modelBuilder.Entity("CloudPlatform.Data.Entities.Folder", b =>
                {
                    b.Property<int>("Id")
                        .ValueGeneratedOnAdd()
                        .HasColumnType("int")
                        .UseIdentityColumn();

                    b.Property<string>("Name")
                        .HasColumnType("nvarchar(max)");

                    b.Property<int>("UserId")
                        .HasColumnType("int");

                    b.HasKey("Id");

                    b.HasIndex("UserId");

                    b.ToTable("Folder");
                });

            modelBuilder.Entity("CloudPlatform.Data.Entities.Note", b =>
                {
                    b.Property<int>("Id")
                        .ValueGeneratedOnAdd()
                        .HasColumnType("int")
                        .UseIdentityColumn();

                    b.Property<DateTime>("DateCreated")
                        .HasColumnType("datetime2");

                    b.Property<DateTime>("DateModified")
                        .HasColumnType("datetime2");

                    b.Property<int>("FolderId")
                        .HasColumnType("int");

                    b.Property<string>("Title")
                        .HasColumnType("nvarchar(max)");

                    b.Property<string>("Value")
                        .HasColumnType("nvarchar(max)");

                    b.HasKey("Id");

                    b.HasIndex("FolderId");

                    b.ToTable("Note");
                });

            modelBuilder.Entity("CloudPlatform.Data.Entities.SharedFolder", b =>
                {
                    b.Property<int>("Id")
                        .ValueGeneratedOnAdd()
                        .HasColumnType("int")
                        .UseIdentityColumn();

                    b.Property<int>("FolderId")
                        .HasColumnType("int");

                    b.Property<int?>("NoteId")
                        .HasColumnType("int");

                    b.Property<int>("UserId")
                        .HasColumnType("int");

                    b.HasKey("Id");

                    b.HasIndex("FolderId");

                    b.HasIndex("NoteId");

                    b.HasIndex("UserId");

                    b.ToTable("SharedFolder");
                });

            modelBuilder.Entity("CloudPlatform.Data.Entities.SharedNote", b =>
                {
                    b.Property<int>("Id")
                        .ValueGeneratedOnAdd()
                        .HasColumnType("int")
                        .UseIdentityColumn();

                    b.Property<int>("NoteId")
                        .HasColumnType("int");

                    b.Property<int>("UserId")
                        .HasColumnType("int");

                    b.HasKey("Id");

                    b.HasIndex("NoteId");

                    b.HasIndex("UserId");

                    b.ToTable("SharedNote");
                });

            modelBuilder.Entity("CloudPlatform.Data.Entities.User", b =>
                {
                    b.Property<int>("Id")
                        .ValueGeneratedOnAdd()
                        .HasColumnType("int")
                        .UseIdentityColumn();

                    b.Property<string>("Theme")
                        .HasColumnType("nvarchar(max)");

                    b.Property<string>("Username")
                        .HasColumnType("nvarchar(max)");

                    b.HasKey("Id");

                    b.ToTable("User");
                });

            modelBuilder.Entity("CloudPlatform.Data.Entities.Folder", b =>
                {
                    b.HasOne("CloudPlatform.Data.Entities.User", "User")
                        .WithMany("Folders")
                        .HasForeignKey("UserId")
                        .OnDelete(DeleteBehavior.Cascade)
                        .IsRequired();

                    b.Navigation("User");
                });

            modelBuilder.Entity("CloudPlatform.Data.Entities.Note", b =>
                {
                    b.HasOne("CloudPlatform.Data.Entities.Folder", "Folder")
                        .WithMany("Notes")
                        .HasForeignKey("FolderId")
                        .OnDelete(DeleteBehavior.Cascade)
                        .IsRequired();

                    b.Navigation("Folder");
                });

            modelBuilder.Entity("CloudPlatform.Data.Entities.SharedFolder", b =>
                {
                    b.HasOne("CloudPlatform.Data.Entities.Folder", "Folder")
                        .WithMany("SharedFolders")
                        .HasForeignKey("FolderId")
                        .OnDelete(DeleteBehavior.Cascade)
                        .IsRequired();

                    b.HasOne("CloudPlatform.Data.Entities.Note", null)
                        .WithMany("SharedFolders")
                        .HasForeignKey("NoteId");

                    b.HasOne("CloudPlatform.Data.Entities.User", "User")
                        .WithMany("SharedFolders")
                        .HasForeignKey("UserId")
                        .OnDelete(DeleteBehavior.Restrict)
                        .IsRequired();

                    b.Navigation("Folder");

                    b.Navigation("User");
                });

            modelBuilder.Entity("CloudPlatform.Data.Entities.SharedNote", b =>
                {
                    b.HasOne("CloudPlatform.Data.Entities.Note", "Note")
                        .WithMany("SharedNotes")
                        .HasForeignKey("NoteId")
                        .OnDelete(DeleteBehavior.Cascade)
                        .IsRequired();

                    b.HasOne("CloudPlatform.Data.Entities.User", "User")
                        .WithMany("SharedNotes")
                        .HasForeignKey("UserId")
                        .OnDelete(DeleteBehavior.Restrict)
                        .IsRequired();

                    b.Navigation("Note");

                    b.Navigation("User");
                });

            modelBuilder.Entity("CloudPlatform.Data.Entities.Folder", b =>
                {
                    b.Navigation("Notes");

                    b.Navigation("SharedFolders");
                });

            modelBuilder.Entity("CloudPlatform.Data.Entities.Note", b =>
                {
                    b.Navigation("SharedFolders");

                    b.Navigation("SharedNotes");
                });

            modelBuilder.Entity("CloudPlatform.Data.Entities.User", b =>
                {
                    b.Navigation("Folders");

                    b.Navigation("SharedFolders");

                    b.Navigation("SharedNotes");
                });
#pragma warning restore 612, 618
        }
    }
}
