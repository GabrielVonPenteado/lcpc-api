using System;
using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace lcpc.Migrations
{
    /// <inheritdoc />
    public partial class AddInitCreate : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.CreateTable(
                name: "Product",
                columns: table => new
                {
                    Id = table.Column<Guid>(type: "uuid", nullable: false),
                    Name = table.Column<string>(type: "character varying(100)", maxLength: 100, nullable: false),
                    ProductType = table.Column<int>(type: "integer", nullable: false),
                    Description = table.Column<string>(type: "character varying(200)", maxLength: 200, nullable: false),
                    Value = table.Column<decimal>(type: "numeric", nullable: false),
                    Thickness = table.Column<decimal>(type: "numeric", nullable: false),
                    Width = table.Column<decimal>(type: "numeric", nullable: false),
                    Length = table.Column<decimal>(type: "numeric", nullable: false),
                    CreatedAt = table.Column<DateTime>(type: "timestamp with time zone", nullable: false),
                    UpdatedAt = table.Column<DateTime>(type: "timestamp with time zone", nullable: true),
                    DeletedAt = table.Column<DateTime>(type: "timestamp with time zone", nullable: true)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_Product", x => x.Id);
                });

            migrationBuilder.CreateTable(
                name: "State",
                columns: table => new
                {
                    UF = table.Column<string>(type: "character varying(2)", maxLength: 2, nullable: false),
                    Name = table.Column<string>(type: "character varying(20)", maxLength: 20, nullable: false),
                    DeletedAt = table.Column<DateTime>(type: "timestamp with time zone", nullable: true)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_State", x => x.UF);
                });

            migrationBuilder.CreateTable(
                name: "User",
                columns: table => new
                {
                    Id = table.Column<Guid>(type: "uuid", nullable: false),
                    Username = table.Column<string>(type: "character varying(20)", maxLength: 20, nullable: false),
                    Password = table.Column<string>(type: "character varying(200)", maxLength: 200, nullable: false),
                    Email = table.Column<string>(type: "character varying(100)", maxLength: 100, nullable: false),
                    CreatedAt = table.Column<DateTime>(type: "timestamp with time zone", nullable: false),
                    UpdatedAt = table.Column<DateTime>(type: "timestamp with time zone", nullable: true),
                    DeletedAt = table.Column<DateTime>(type: "timestamp with time zone", nullable: true)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_User", x => x.Id);
                });

            migrationBuilder.CreateTable(
                name: "City",
                columns: table => new
                {
                    Id = table.Column<Guid>(type: "uuid", nullable: false),
                    Name = table.Column<string>(type: "character varying(50)", maxLength: 50, nullable: false),
                    StateUF = table.Column<string>(type: "character varying(2)", maxLength: 2, nullable: false),
                    CreatedAt = table.Column<DateTime>(type: "timestamp with time zone", nullable: false),
                    UpdatedAt = table.Column<DateTime>(type: "timestamp with time zone", nullable: true),
                    DeletedAt = table.Column<DateTime>(type: "timestamp with time zone", nullable: true)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_City", x => x.Id);
                    table.ForeignKey(
                        name: "FK_City_State_StateUF",
                        column: x => x.StateUF,
                        principalTable: "State",
                        principalColumn: "UF",
                        onDelete: ReferentialAction.Cascade);
                });

            migrationBuilder.CreateTable(
                name: "Client",
                columns: table => new
                {
                    Id = table.Column<Guid>(type: "uuid", nullable: false),
                    Name = table.Column<string>(type: "character varying(50)", maxLength: 50, nullable: false),
                    Streetplace = table.Column<string>(type: "character varying(100)", maxLength: 100, nullable: false),
                    Neighborhood = table.Column<string>(type: "character varying(100)", maxLength: 100, nullable: false),
                    Number = table.Column<string>(type: "character varying(6)", maxLength: 6, nullable: false),
                    Complement = table.Column<string>(type: "character varying(255)", maxLength: 255, nullable: false),
                    Phone = table.Column<string>(type: "character varying(15)", maxLength: 15, nullable: false),
                    Email = table.Column<string>(type: "character varying(100)", maxLength: 100, nullable: false),
                    CNPJ = table.Column<string>(type: "character varying(14)", maxLength: 14, nullable: false),
                    FkCityId = table.Column<Guid>(type: "uuid", nullable: false),
                    CreatedAt = table.Column<DateTime>(type: "timestamp with time zone", nullable: false),
                    UpdatedAt = table.Column<DateTime>(type: "timestamp with time zone", nullable: true),
                    DeletedAt = table.Column<DateTime>(type: "timestamp with time zone", nullable: true)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_Client", x => x.Id);
                    table.ForeignKey(
                        name: "FK_Client_City_FkCityId",
                        column: x => x.FkCityId,
                        principalTable: "City",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Cascade);
                });

            migrationBuilder.CreateTable(
                name: "Order",
                columns: table => new
                {
                    Id = table.Column<Guid>(type: "uuid", nullable: false),
                    Description = table.Column<string>(type: "character varying(512)", maxLength: 512, nullable: false),
                    TotalValue = table.Column<decimal>(type: "numeric", nullable: false),
                    ShippingDate = table.Column<DateTime>(type: "timestamp with time zone", nullable: true),
                    CreationDate = table.Column<DateTime>(type: "timestamp with time zone", nullable: false),
                    DeliveryDate = table.Column<DateTime>(type: "timestamp with time zone", nullable: true),
                    ExpectedDeliveryDate = table.Column<DateTime>(type: "timestamp with time zone", nullable: false),
                    State = table.Column<string>(type: "character varying(50)", maxLength: 50, nullable: false),
                    NInstallments = table.Column<int>(type: "integer", nullable: false),
                    FkUserId = table.Column<Guid>(type: "uuid", nullable: false),
                    FkClientId = table.Column<Guid>(type: "uuid", nullable: false),
                    CreatedAt = table.Column<DateTime>(type: "timestamp with time zone", nullable: false),
                    UpdatedAt = table.Column<DateTime>(type: "timestamp with time zone", nullable: true),
                    DeletedAt = table.Column<DateTime>(type: "timestamp with time zone", nullable: true)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_Order", x => x.Id);
                    table.ForeignKey(
                        name: "FK_Order_Client_FkClientId",
                        column: x => x.FkClientId,
                        principalTable: "Client",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Cascade);
                    table.ForeignKey(
                        name: "FK_Order_User_FkUserId",
                        column: x => x.FkUserId,
                        principalTable: "User",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Cascade);
                });

            migrationBuilder.CreateTable(
                name: "Installment",
                columns: table => new
                {
                    Id = table.Column<Guid>(type: "uuid", nullable: false),
                    ExpirationDate = table.Column<DateTime>(type: "timestamp with time zone", nullable: false),
                    Value = table.Column<decimal>(type: "numeric", nullable: false),
                    Situation = table.Column<bool>(type: "boolean", nullable: false),
                    FkOrderId = table.Column<Guid>(type: "uuid", nullable: false),
                    CreatedAt = table.Column<DateTime>(type: "timestamp with time zone", nullable: false),
                    UpdatedAt = table.Column<DateTime>(type: "timestamp with time zone", nullable: true),
                    DeletedAt = table.Column<DateTime>(type: "timestamp with time zone", nullable: true)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_Installment", x => x.Id);
                    table.ForeignKey(
                        name: "FK_Installment_Order_FkOrderId",
                        column: x => x.FkOrderId,
                        principalTable: "Order",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Cascade);
                });

            migrationBuilder.CreateTable(
                name: "ItemOrder",
                columns: table => new
                {
                    Id = table.Column<Guid>(type: "uuid", nullable: false),
                    FkProductId = table.Column<Guid>(type: "uuid", nullable: false),
                    FkOrderId = table.Column<Guid>(type: "uuid", nullable: false),
                    Quantity = table.Column<int>(type: "integer", nullable: false),
                    ItemValue = table.Column<decimal>(type: "numeric", nullable: false),
                    CreatedAt = table.Column<DateTime>(type: "timestamp with time zone", nullable: false),
                    UpdatedAt = table.Column<DateTime>(type: "timestamp with time zone", nullable: true),
                    DeletedAt = table.Column<DateTime>(type: "timestamp with time zone", nullable: true)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_ItemOrder", x => x.Id);
                    table.ForeignKey(
                        name: "FK_ItemOrder_Order_FkOrderId",
                        column: x => x.FkOrderId,
                        principalTable: "Order",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Cascade);
                    table.ForeignKey(
                        name: "FK_ItemOrder_Product_FkProductId",
                        column: x => x.FkProductId,
                        principalTable: "Product",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Cascade);
                });

            migrationBuilder.CreateTable(
                name: "Payment",
                columns: table => new
                {
                    Id = table.Column<Guid>(type: "uuid", nullable: false),
                    DataPayment = table.Column<DateTime>(type: "timestamp with time zone", nullable: true),
                    Value = table.Column<decimal>(type: "numeric", nullable: false),
                    PaymentType = table.Column<int>(type: "integer", nullable: false),
                    ReceivementType = table.Column<int>(type: "integer", nullable: false),
                    FkInstallmentId = table.Column<Guid>(type: "uuid", nullable: false),
                    FkUserId = table.Column<Guid>(type: "uuid", nullable: false),
                    CreatedAt = table.Column<DateTime>(type: "timestamp with time zone", nullable: false),
                    UpdatedAt = table.Column<DateTime>(type: "timestamp with time zone", nullable: true),
                    DeletedAt = table.Column<DateTime>(type: "timestamp with time zone", nullable: true)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_Payment", x => x.Id);
                    table.ForeignKey(
                        name: "FK_Payment_Installment_FkInstallmentId",
                        column: x => x.FkInstallmentId,
                        principalTable: "Installment",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Cascade);
                    table.ForeignKey(
                        name: "FK_Payment_User_FkUserId",
                        column: x => x.FkUserId,
                        principalTable: "User",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Cascade);
                });

            migrationBuilder.CreateIndex(
                name: "IX_City_StateUF",
                table: "City",
                column: "StateUF");

            migrationBuilder.CreateIndex(
                name: "IX_Client_FkCityId",
                table: "Client",
                column: "FkCityId");

            migrationBuilder.CreateIndex(
                name: "IX_Installment_FkOrderId",
                table: "Installment",
                column: "FkOrderId");

            migrationBuilder.CreateIndex(
                name: "IX_ItemOrder_FkOrderId",
                table: "ItemOrder",
                column: "FkOrderId");

            migrationBuilder.CreateIndex(
                name: "IX_ItemOrder_FkProductId",
                table: "ItemOrder",
                column: "FkProductId");

            migrationBuilder.CreateIndex(
                name: "IX_Order_FkClientId",
                table: "Order",
                column: "FkClientId");

            migrationBuilder.CreateIndex(
                name: "IX_Order_FkUserId",
                table: "Order",
                column: "FkUserId");

            migrationBuilder.CreateIndex(
                name: "IX_Payment_FkInstallmentId",
                table: "Payment",
                column: "FkInstallmentId");

            migrationBuilder.CreateIndex(
                name: "IX_Payment_FkUserId",
                table: "Payment",
                column: "FkUserId");
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropTable(
                name: "ItemOrder");

            migrationBuilder.DropTable(
                name: "Payment");

            migrationBuilder.DropTable(
                name: "Product");

            migrationBuilder.DropTable(
                name: "Installment");

            migrationBuilder.DropTable(
                name: "Order");

            migrationBuilder.DropTable(
                name: "Client");

            migrationBuilder.DropTable(
                name: "User");

            migrationBuilder.DropTable(
                name: "City");

            migrationBuilder.DropTable(
                name: "State");
        }
    }
}
