using System.Runtime.ConstrainedExecution;
using Microsoft.EntityFrameworkCore.Migrations;
using Models;

#nullable disable

namespace MyWebsiteBackend.Migrations
{
    /// <inheritdoc />
    public partial class InitialCertificates : Migration
    {
        private IEnumerable<Certificate> Certificates = new List<Certificate>() {
            new Certificate {
                Name = "Build Apps Using React: Introducing React for Web Applications",
                Organization = "Skillsoft",
                IssueDate = new DateTime(2023, 11, 1, 0, 0, 0, 0, DateTimeKind.Unspecified),
                CredendialUrl = new Uri("https://skillsoft.digitalbadges-eu.skillsoft.com/31b7aaa2-14cc-4623-9b38-accf5bf0e476"),
                CredendialId = "6545857"
            },
            new Certificate {
                Name = "Test Driven Development: Implementing TDD", 
                Organization = "Skillsoft", 
                IssueDate = new DateTime(2023, 7, 1, 0, 0, 0, 0, DateTimeKind.Unspecified), 
                CredendialUrl = new Uri("https://skillsoft.digitalbadges-eu.skillsoft.com/f575f2c7-c7fd-4b1b-9247-28460fe19f12"),
                CredendialId = "5269447" 
            },
            new Certificate {
                Name = "Understanding Typescipt", 
                Organization = "Udemy", 
                IssueDate = new DateTime(2023, 10, 1, 0, 0, 0, 0, DateTimeKind.Unspecified), 
                CredendialUrl = new Uri("https://www.ude.my/UC-8d65f57b-b460-4e2c-b399-7229dcc27ccd"),
                CredendialId = "UC-8d65f57b-b460-4e2c-b399-7229dcc27ccd"
            }
        };

        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            // Make expiration date nullable
            migrationBuilder.AlterColumn<DateTime>(
                name: "ExpirationDate",
                table: "Certificates",
                nullable: true
                );

            // Insert data
            foreach (Certificate certificate in Certificates)
            {
                migrationBuilder.InsertData(
                    table: "Certificates",
                    columns: new[] { "Name", "Organization", "IssueDate", "CredendialUrl", "CredendialId", "ExpirationDate" },
                    values: new object[] { certificate.Name, certificate.Organization, certificate.IssueDate, certificate.CredendialUrl.AbsoluteUri, certificate.CredendialId, null });
            }
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            // Make expiration date non-nullable
            migrationBuilder.AlterColumn<DateTime>(
                name: "ExpirationDate",
                table: "Certificates",
                nullable: false
                );

            // Delete data
            foreach(Certificate certificate in Certificates) 
            {
                migrationBuilder.DeleteData(
                    table: "Certificates",
                    keyColumn: "Name",
                    keyValues: new object[] { certificate.Name, certificate.Organization, certificate.IssueDate, certificate.CredendialUrl.AbsoluteUri, certificate.CredendialId, null });
            }
        }
    }
}
