using Microsoft.EntityFrameworkCore.Migrations;
using System;
using Bogus;
using Bogus.Extensions.Brazil;

#nullable disable

namespace lcpc.Migrations
{
    public partial class SeedMigration : Migration
    {
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            // Instanciando Faker para gerar dados aleatórios
            var faker = new Faker("pt_BR");

            // Seeding States
            var states = new[]
            {
                new { UF = "SP", Name = "São Paulo" },
                new { UF = "RJ", Name = "Rio de Janeiro" },
                new { UF = "MG", Name = "Minas Gerais" },
                new { UF = "RS", Name = "Rio Grande do Sul" },
                new { UF = "BA", Name = "Bahia" }
            };

            foreach (var state in states)
            {
                migrationBuilder.InsertData(
                    table: "State",
                    columns: new[] { "UF", "Name" },
                    values: new object[] { state.UF, state.Name });
            }

            // Seeding Cities
            var cityIds = new List<Guid>();
            for (int i = 0; i < 5; i++)
            {
                var cityId = Guid.NewGuid();
                cityIds.Add(cityId);

                migrationBuilder.InsertData(
                    table: "City",
                    columns: new[] { "Id", "Name", "StateUF", "CreatedAt" },
                    values: new object[] { cityId, faker.Address.City(), states[i % states.Length].UF, DateTime.UtcNow });
            }

            // Seeding Users (Admin and test users)
            var users = new List<Guid>();

            // Admin User
            var adminId = Guid.NewGuid();
            users.Add(adminId);
            migrationBuilder.InsertData(
                table: "User",
                columns: new[] { "Id", "Username", "Password", "Email", "CreatedAt" },
                values: new object[] { adminId, "admin", "admin123", "admin@exemplo.com", DateTime.UtcNow });

            // Test User
            var testId = Guid.NewGuid();
            users.Add(testId);
            migrationBuilder.InsertData(
                table: "User",
                columns: new[] { "Id", "Username", "Password", "Email", "CreatedAt" },
                values: new object[] { testId, "teste", "teste", "teste@exemplo.com", DateTime.UtcNow });

            // Seeding more random users
            for (int i = 0; i < 3; i++)
            {
                var userId = Guid.NewGuid();
                users.Add(userId);

                var username = faker.Internet.UserName();
                if (username.Length > 20) username = username.Substring(0, 20);

                migrationBuilder.InsertData(
                    table: "User",
                    columns: new[] { "Id", "Username", "Password", "Email", "CreatedAt" },
                    values: new object[] { userId, username, faker.Internet.Password(), faker.Internet.Email(), DateTime.UtcNow });
            }

            // Seeding Clients
            var clients = new List<Guid>();
            for (int i = 0; i < 15; i++)
            {
                var clientId = Guid.NewGuid();
                clients.Add(clientId);

                migrationBuilder.InsertData(
                    table: "Client",
                    columns: new[] { "Id", "Name", "Streetplace", "Neighborhood", "Number", "Complement", "Phone", "Email", "CNPJ", "FkCityId", "CreatedAt" },
                    values: new object[] {
                        clientId,
                        faker.Company.CompanyName(),
                        faker.Address.StreetName(),
                        faker.Address.SecondaryAddress(),
                        faker.Address.BuildingNumber(),
                        faker.Address.FullAddress(),
                        faker.Phone.PhoneNumber().Replace("(", "").Replace(")", "").Replace("-", "").Replace(" ", ""),
                        faker.Internet.Email(),
                        faker.Company.Cnpj().Replace(".", "").Replace("-", "").Replace("/", ""),
                        cityIds[i % cityIds.Count],
                        DateTime.UtcNow
                    });
            }

            // Seeding Products
            var productIds = new List<Guid>();
            for (int i = 0; i < 5; i++)
            {
                var productId = Guid.NewGuid();
                productIds.Add(productId);

                migrationBuilder.InsertData(
                    table: "Product",
                    columns: new[] { "Id", "Name", "ProductType", "Description", "Value", "Thickness", "Width", "Length", "CreatedAt" },
                    values: new object[] {
                        productId,
                        faker.Commerce.ProductName(),
                        0, // Supondo que ProductType seja um enum com valor 0
                        faker.Commerce.ProductAdjective(),
                        faker.Random.Decimal(100, 500),
                        faker.Random.Decimal(5, 20),
                        1.22,
                        2.44,
                        DateTime.UtcNow
                    });
            }

            // Seeding Orders
            var orderIds = new List<Guid>();
            for (int i = 0; i < 15; i++)
            {
                var orderId = Guid.NewGuid();
                orderIds.Add(orderId);

                migrationBuilder.InsertData(
                    table: "Order",
                    columns: new[] { "Id", "Description", "TotalValue", "CreationDate", "ExpectedDeliveryDate", "NInstallments", "FkUserId", "FkClientId", "CreatedAt", "Status" },
                    values: new object[] {
                        orderId,
                        $"Pedido {i+1} - {faker.Lorem.Sentence()}",
                        faker.Random.Decimal(500, 10000),
                        DateTime.UtcNow,
                        DateTime.UtcNow.AddDays(faker.Random.Int(5, 30)),
                        faker.Random.Int(1, 12),
                        users[i % users.Count],
                        clients[i % clients.Count],
                        DateTime.UtcNow,
                        1 // Supondo que o Status seja um enum com valor 1 (InProgress, por exemplo)
                    });
            }

            // Seeding Installments
            for (int i = 0; i < 15; i++)
            {
                migrationBuilder.InsertData(
                    table: "Installment",
                    columns: new[] { "Id", "ExpirationDate", "Value", "Situation", "FkOrderId", "CreatedAt" },
                    values: new object[] {
                        Guid.NewGuid(),
                        DateTime.UtcNow.AddDays(faker.Random.Int(30, 90)),
                        faker.Random.Decimal(100, 500),
                        true,
                        orderIds[i % orderIds.Count], // Chave estrangeira de Order
                        DateTime.UtcNow
                    });
            }

            // Seeding ItemOrders
            for (int i = 0; i < 15; i++)
            {
                migrationBuilder.InsertData(
                    table: "ItemOrder",
                    columns: new[] { "FkProductId", "FkOrderId", "Quantity", "ItemValue", "CreatedAt" },
                    values: new object[] {
                        productIds[i % productIds.Count],
                        orderIds[i % orderIds.Count], // Chave estrangeira de Order
                        faker.Random.Int(1, 10),
                        faker.Random.Decimal(50, 300),
                        DateTime.UtcNow
                    });
            }

            // Seeding Payments
            for (int i = 0; i < 15; i++)
            {
                migrationBuilder.InsertData(
                    table: "Payment",
                    columns: new[] { "Id", "DataPayment", "Value", "PaymentType", "ReceivementType", "FkInstallmentId", "FkUserId", "CreatedAt" },
                    values: new object[] {
                        Guid.NewGuid(),
                        DateTime.UtcNow,
                        faker.Random.Decimal(100, 500),
                        0, // Supondo que PaymentType seja um enum com valor 0
                        0, // Supondo que ReceivementType seja um enum com valor 0
                        orderIds[i % orderIds.Count], // Chave estrangeira de Installment
                        users[i % users.Count],
                        DateTime.UtcNow
                    });
            }
        }

        protected override void Down(MigrationBuilder migrationBuilder)
        {
            // Para reverter as inserções
            migrationBuilder.Sql("DELETE FROM Payment");
            migrationBuilder.Sql("DELETE FROM ItemOrder");
            migrationBuilder.Sql("DELETE FROM Installment");
            migrationBuilder.Sql("DELETE FROM [Order]");
            migrationBuilder.Sql("DELETE FROM Product");
            migrationBuilder.Sql("DELETE FROM Client");
            migrationBuilder.Sql("DELETE FROM [User]");
            migrationBuilder.Sql("DELETE FROM City");
            migrationBuilder.Sql("DELETE FROM State");
        }
    }
}
