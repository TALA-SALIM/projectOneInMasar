using System;
using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace homework.Data.Migrations;

/// <inheritdoc />
public partial class _20261011151142_AddBaseEntityFields : Migration
{
    /// <inheritdoc />
    protected override void Up(MigrationBuilder migrationBuilder)
    {
        migrationBuilder.AlterColumn<float>(
            name: "Price",
            table: "Products",
            type: "real",
            nullable: false,
            oldClrType: typeof(decimal),
            oldType: "numeric");

        migrationBuilder.AddColumn<DateTime>(
            name: "CreatedAt",
            table: "Products",
            type: "timestamp with time zone",
            nullable: false,
            defaultValue: new DateTime(1, 1, 1, 0, 0, 0, 0, DateTimeKind.Unspecified));

        migrationBuilder.AddColumn<string>(
            name: "CreatedBy",
            table: "Products",
            type: "text",
            nullable: false,
            defaultValue: "");

        migrationBuilder.AddColumn<string>(
            name: "ImageUrl",
            table: "Products",
            type: "text",
            nullable: false,
            defaultValue: "");

        migrationBuilder.AddColumn<bool>(
            name: "IsDelete",
            table: "Products",
            type: "boolean",
            nullable: false,
            defaultValue: false);

        migrationBuilder.AddColumn<DateTime>(
            name: "UpdatedAt",
            table: "Products",
            type: "timestamp with time zone",
            nullable: true);

        migrationBuilder.AddColumn<string>(
            name: "UpdatedBy",
            table: "Products",
            type: "text",
            nullable: false,
            defaultValue: "");

        migrationBuilder.AddColumn<DateTime>(
            name: "CreatedAt",
            table: "Categories",
            type: "timestamp with time zone",
            nullable: false,
            defaultValue: new DateTime(1, 1, 1, 0, 0, 0, 0, DateTimeKind.Unspecified));

        migrationBuilder.AddColumn<string>(
            name: "CreatedBy",
            table: "Categories",
            type: "text",
            nullable: false,
            defaultValue: "");

        migrationBuilder.AddColumn<bool>(
            name: "IsDelete",
            table: "Categories",
            type: "boolean",
            nullable: false,
            defaultValue: false);

        migrationBuilder.AddColumn<DateTime>(
            name: "UpdatedAt",
            table: "Categories",
            type: "timestamp with time zone",
            nullable: true);

        migrationBuilder.AddColumn<string>(
            name: "UpdatedBy",
            table: "Categories",
            type: "text",
            nullable: false,
            defaultValue: "");
    }

    /// <inheritdoc />
    protected override void Down(MigrationBuilder migrationBuilder)
    {
        migrationBuilder.DropColumn(
            name: "CreatedAt",
            table: "Products");

        migrationBuilder.DropColumn(
            name: "CreatedBy",
            table: "Products");

        migrationBuilder.DropColumn(
            name: "ImageUrl",
            table: "Products");

        migrationBuilder.DropColumn(
            name: "IsDelete",
            table: "Products");

        migrationBuilder.DropColumn(
            name: "UpdatedAt",
            table: "Products");

        migrationBuilder.DropColumn(
            name: "UpdatedBy",
            table: "Products");

        migrationBuilder.DropColumn(
            name: "CreatedAt",
            table: "Categories");

        migrationBuilder.DropColumn(
            name: "CreatedBy",
            table: "Categories");

        migrationBuilder.DropColumn(
            name: "IsDelete",
            table: "Categories");

        migrationBuilder.DropColumn(
            name: "UpdatedAt",
            table: "Categories");

        migrationBuilder.DropColumn(
            name: "UpdatedBy",
            table: "Categories");

        migrationBuilder.AlterColumn<decimal>(
            name: "Price",
            table: "Products",
            type: "numeric",
            nullable: false,
            oldClrType: typeof(float),
            oldType: "real");
    }
}
