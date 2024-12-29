using System;
using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

#pragma warning disable CA1814 // Prefer jagged arrays over multidimensional

namespace InsurancePolicy.Migrations
{
    /// <inheritdoc />
    public partial class insurance : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.CreateTable(
                name: "InsurancePlans",
                columns: table => new
                {
                    PlanId = table.Column<Guid>(type: "uniqueidentifier", nullable: false, defaultValueSql: "NEWSEQUENTIALID()"),
                    PlanName = table.Column<string>(type: "nvarchar(100)", maxLength: 100, nullable: false),
                    Status = table.Column<bool>(type: "bit", nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_InsurancePlans", x => x.PlanId);
                });

            migrationBuilder.CreateTable(
                name: "Roles",
                columns: table => new
                {
                    Id = table.Column<Guid>(type: "uniqueidentifier", nullable: false, defaultValueSql: "NEWSEQUENTIALID()"),
                    Name = table.Column<string>(type: "nvarchar(50)", maxLength: 50, nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_Roles", x => x.Id);
                });

            migrationBuilder.CreateTable(
                name: "States",
                columns: table => new
                {
                    StateId = table.Column<Guid>(type: "uniqueidentifier", nullable: false),
                    StateName = table.Column<string>(type: "nvarchar(100)", maxLength: 100, nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_States", x => x.StateId);
                });

            migrationBuilder.CreateTable(
                name: "TaxSettings",
                columns: table => new
                {
                    TaxId = table.Column<Guid>(type: "uniqueidentifier", nullable: false, defaultValueSql: "NEWSEQUENTIALID()"),
                    TaxPercentage = table.Column<double>(type: "float", nullable: false),
                    UpdatedAt = table.Column<DateTime>(type: "datetime2", nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_TaxSettings", x => x.TaxId);
                });

            migrationBuilder.CreateTable(
                name: "InsuranceSchemes",
                columns: table => new
                {
                    SchemeId = table.Column<Guid>(type: "uniqueidentifier", nullable: false, defaultValueSql: "NEWSEQUENTIALID()"),
                    SchemeName = table.Column<string>(type: "nvarchar(100)", maxLength: 100, nullable: false),
                    SchemeImage = table.Column<string>(type: "nvarchar(max)", nullable: false),
                    Description = table.Column<string>(type: "nvarchar(500)", maxLength: 500, nullable: false),
                    MinAmount = table.Column<double>(type: "float", nullable: false),
                    MaxAmount = table.Column<double>(type: "float", nullable: false),
                    MinInvestTime = table.Column<int>(type: "int", nullable: false),
                    MaxInvestTime = table.Column<int>(type: "int", nullable: false),
                    MinAge = table.Column<int>(type: "int", nullable: false),
                    MaxAge = table.Column<int>(type: "int", nullable: false),
                    ProfitRatio = table.Column<double>(type: "float", nullable: false),
                    RegistrationCommRatio = table.Column<double>(type: "float", nullable: false),
                    InstallmentCommRatio = table.Column<double>(type: "float", nullable: false),
                    Status = table.Column<bool>(type: "bit", nullable: false),
                    PlanId = table.Column<Guid>(type: "uniqueidentifier", nullable: false),
                    ClaimDeductionPercentage = table.Column<double>(type: "float", nullable: false),
                    PenaltyDeductionPercentage = table.Column<double>(type: "float", nullable: false),
                    RequiredDocuments = table.Column<string>(type: "nvarchar(max)", nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_InsuranceSchemes", x => x.SchemeId);
                    table.ForeignKey(
                        name: "FK_InsuranceSchemes_InsurancePlans_PlanId",
                        column: x => x.PlanId,
                        principalTable: "InsurancePlans",
                        principalColumn: "PlanId",
                        onDelete: ReferentialAction.Cascade);
                });

            migrationBuilder.CreateTable(
                name: "Users",
                columns: table => new
                {
                    UserId = table.Column<Guid>(type: "uniqueidentifier", nullable: false, defaultValueSql: "NEWSEQUENTIALID()"),
                    UserName = table.Column<string>(type: "nvarchar(50)", maxLength: 50, nullable: false),
                    Password = table.Column<string>(type: "nvarchar(350)", maxLength: 350, nullable: false),
                    RoleId = table.Column<Guid>(type: "uniqueidentifier", nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_Users", x => x.UserId);
                    table.ForeignKey(
                        name: "FK_Users_Roles_RoleId",
                        column: x => x.RoleId,
                        principalTable: "Roles",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Cascade);
                });

            migrationBuilder.CreateTable(
                name: "Cities",
                columns: table => new
                {
                    CityId = table.Column<Guid>(type: "uniqueidentifier", nullable: false, defaultValueSql: "NEWSEQUENTIALID()"),
                    CityName = table.Column<string>(type: "nvarchar(100)", maxLength: 100, nullable: false),
                    StateId = table.Column<Guid>(type: "uniqueidentifier", nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_Cities", x => x.CityId);
                    table.ForeignKey(
                        name: "FK_Cities_States_StateId",
                        column: x => x.StateId,
                        principalTable: "States",
                        principalColumn: "StateId",
                        onDelete: ReferentialAction.Cascade);
                });

            migrationBuilder.CreateTable(
                name: "Admins",
                columns: table => new
                {
                    AdminId = table.Column<Guid>(type: "uniqueidentifier", nullable: false, defaultValueSql: "NEWSEQUENTIALID()"),
                    AdminFirstName = table.Column<string>(type: "nvarchar(50)", maxLength: 50, nullable: false),
                    AdminLastName = table.Column<string>(type: "nvarchar(50)", maxLength: 50, nullable: false),
                    AdminEmail = table.Column<string>(type: "nvarchar(100)", maxLength: 100, nullable: false),
                    AdminPhone = table.Column<string>(type: "nvarchar(max)", nullable: false),
                    Status = table.Column<bool>(type: "bit", nullable: false),
                    UserId = table.Column<Guid>(type: "uniqueidentifier", nullable: true)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_Admins", x => x.AdminId);
                    table.ForeignKey(
                        name: "FK_Admins_Users_UserId",
                        column: x => x.UserId,
                        principalTable: "Users",
                        principalColumn: "UserId");
                });

            migrationBuilder.CreateTable(
                name: "Employees",
                columns: table => new
                {
                    EmployeeId = table.Column<Guid>(type: "uniqueidentifier", nullable: false, defaultValueSql: "NEWSEQUENTIALID()"),
                    EmployeeFirstName = table.Column<string>(type: "nvarchar(50)", maxLength: 50, nullable: false),
                    EmployeeLastName = table.Column<string>(type: "nvarchar(50)", maxLength: 50, nullable: false),
                    Phone = table.Column<string>(type: "nvarchar(max)", nullable: false),
                    Email = table.Column<string>(type: "nvarchar(100)", maxLength: 100, nullable: false),
                    Salary = table.Column<double>(type: "float", nullable: false),
                    Status = table.Column<bool>(type: "bit", nullable: false),
                    UserId = table.Column<Guid>(type: "uniqueidentifier", nullable: true)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_Employees", x => x.EmployeeId);
                    table.ForeignKey(
                        name: "FK_Employees_Users_UserId",
                        column: x => x.UserId,
                        principalTable: "Users",
                        principalColumn: "UserId");
                });

            migrationBuilder.CreateTable(
                name: "Address",
                columns: table => new
                {
                    AddressId = table.Column<Guid>(type: "uniqueidentifier", nullable: false),
                    HouseNo = table.Column<string>(type: "nvarchar(max)", nullable: false),
                    Apartment = table.Column<string>(type: "nvarchar(max)", nullable: false),
                    Pincode = table.Column<int>(type: "int", nullable: false),
                    CityId = table.Column<Guid>(type: "uniqueidentifier", nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_Address", x => x.AddressId);
                    table.ForeignKey(
                        name: "FK_Address_Cities_CityId",
                        column: x => x.CityId,
                        principalTable: "Cities",
                        principalColumn: "CityId",
                        onDelete: ReferentialAction.Cascade);
                });

            migrationBuilder.CreateTable(
                name: "Agents",
                columns: table => new
                {
                    AgentId = table.Column<Guid>(type: "uniqueidentifier", nullable: false, defaultValueSql: "NEWSEQUENTIALID()"),
                    AgentFirstName = table.Column<string>(type: "nvarchar(50)", maxLength: 50, nullable: false),
                    AgentLastName = table.Column<string>(type: "nvarchar(50)", maxLength: 50, nullable: false),
                    Qualification = table.Column<string>(type: "nvarchar(100)", maxLength: 100, nullable: true),
                    Email = table.Column<string>(type: "nvarchar(max)", nullable: false),
                    Phone = table.Column<string>(type: "nvarchar(max)", nullable: false),
                    CommissionEarned = table.Column<double>(type: "float", nullable: false),
                    Status = table.Column<bool>(type: "bit", nullable: false),
                    UserId = table.Column<Guid>(type: "uniqueidentifier", nullable: true),
                    AddressId = table.Column<Guid>(type: "uniqueidentifier", nullable: true)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_Agents", x => x.AgentId);
                    table.ForeignKey(
                        name: "FK_Agents_Address_AddressId",
                        column: x => x.AddressId,
                        principalTable: "Address",
                        principalColumn: "AddressId");
                    table.ForeignKey(
                        name: "FK_Agents_Users_UserId",
                        column: x => x.UserId,
                        principalTable: "Users",
                        principalColumn: "UserId");
                });

            migrationBuilder.CreateTable(
                name: "AgentEarnings",
                columns: table => new
                {
                    Id = table.Column<Guid>(type: "uniqueidentifier", nullable: false, defaultValueSql: "NEWSEQUENTIALID()"),
                    AgentId = table.Column<Guid>(type: "uniqueidentifier", nullable: false),
                    Amount = table.Column<double>(type: "float", nullable: false),
                    WithdrawalDate = table.Column<DateTime>(type: "datetime2", nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_AgentEarnings", x => x.Id);
                    table.ForeignKey(
                        name: "FK_AgentEarnings_Agents_AgentId",
                        column: x => x.AgentId,
                        principalTable: "Agents",
                        principalColumn: "AgentId",
                        onDelete: ReferentialAction.Cascade);
                });

            migrationBuilder.CreateTable(
                name: "Customers",
                columns: table => new
                {
                    CustomerId = table.Column<Guid>(type: "uniqueidentifier", nullable: false, defaultValueSql: "NEWSEQUENTIALID()"),
                    CustomerFirstName = table.Column<string>(type: "nvarchar(50)", maxLength: 50, nullable: false),
                    CustomerLastName = table.Column<string>(type: "nvarchar(50)", maxLength: 50, nullable: false),
                    Email = table.Column<string>(type: "nvarchar(100)", maxLength: 100, nullable: false),
                    Phone = table.Column<string>(type: "nvarchar(max)", nullable: false),
                    DateOfBirth = table.Column<DateTime>(type: "datetime2", nullable: false),
                    AddressId = table.Column<Guid>(type: "uniqueidentifier", nullable: false),
                    Status = table.Column<bool>(type: "bit", nullable: false),
                    UserId = table.Column<Guid>(type: "uniqueidentifier", nullable: true),
                    AgentId = table.Column<Guid>(type: "uniqueidentifier", nullable: true)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_Customers", x => x.CustomerId);
                    table.ForeignKey(
                        name: "FK_Customers_Address_AddressId",
                        column: x => x.AddressId,
                        principalTable: "Address",
                        principalColumn: "AddressId",
                        onDelete: ReferentialAction.Cascade);
                    table.ForeignKey(
                        name: "FK_Customers_Agents_AgentId",
                        column: x => x.AgentId,
                        principalTable: "Agents",
                        principalColumn: "AgentId");
                    table.ForeignKey(
                        name: "FK_Customers_Users_UserId",
                        column: x => x.UserId,
                        principalTable: "Users",
                        principalColumn: "UserId");
                });

            migrationBuilder.CreateTable(
                name: "CustomersQueries",
                columns: table => new
                {
                    QueryId = table.Column<Guid>(type: "uniqueidentifier", nullable: false, defaultValueSql: "NEWSEQUENTIALID()"),
                    Title = table.Column<string>(type: "nvarchar(255)", maxLength: 255, nullable: false),
                    Message = table.Column<string>(type: "nvarchar(max)", nullable: false),
                    Response = table.Column<string>(type: "nvarchar(max)", nullable: true),
                    QueryType = table.Column<string>(type: "nvarchar(max)", nullable: false),
                    IsResolved = table.Column<bool>(type: "bit", nullable: false),
                    ResolvedByEmployeeId = table.Column<Guid>(type: "uniqueidentifier", nullable: true),
                    CustomerId = table.Column<Guid>(type: "uniqueidentifier", nullable: false),
                    PolicyId = table.Column<Guid>(type: "uniqueidentifier", nullable: true),
                    CreatedAt = table.Column<DateTime>(type: "datetime2", nullable: false),
                    ResolvedAt = table.Column<DateTime>(type: "datetime2", nullable: true)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_CustomersQueries", x => x.QueryId);
                    table.ForeignKey(
                        name: "FK_CustomersQueries_Customers_CustomerId",
                        column: x => x.CustomerId,
                        principalTable: "Customers",
                        principalColumn: "CustomerId",
                        onDelete: ReferentialAction.Cascade);
                    table.ForeignKey(
                        name: "FK_CustomersQueries_Employees_ResolvedByEmployeeId",
                        column: x => x.ResolvedByEmployeeId,
                        principalTable: "Employees",
                        principalColumn: "EmployeeId");
                });

            migrationBuilder.CreateTable(
                name: "InsurancePolicies",
                columns: table => new
                {
                    PolicyId = table.Column<Guid>(type: "uniqueidentifier", nullable: false, defaultValueSql: "NEWSEQUENTIALID()"),
                    InsuranceSchemeId = table.Column<Guid>(type: "uniqueidentifier", nullable: false),
                    CustomerId = table.Column<Guid>(type: "uniqueidentifier", nullable: false),
                    IssueDate = table.Column<DateTime>(type: "datetime2", nullable: false),
                    MaturityDate = table.Column<DateTime>(type: "datetime2", nullable: false),
                    PremiumType = table.Column<int>(type: "int", nullable: false),
                    SumAssured = table.Column<double>(type: "float", nullable: false),
                    PolicyTerm = table.Column<long>(type: "bigint", nullable: false),
                    PremiumAmount = table.Column<double>(type: "float", nullable: false),
                    InstallmentAmount = table.Column<double>(type: "float", nullable: true),
                    TotalPaidAmount = table.Column<double>(type: "float", nullable: true),
                    Status = table.Column<int>(type: "int", nullable: false),
                    AgentId = table.Column<Guid>(type: "uniqueidentifier", nullable: true),
                    TaxId = table.Column<Guid>(type: "uniqueidentifier", nullable: true),
                    TaxSettingsTaxId = table.Column<Guid>(type: "uniqueidentifier", nullable: false),
                    CancellationDate = table.Column<DateTime>(type: "datetime2", nullable: true)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_InsurancePolicies", x => x.PolicyId);
                    table.ForeignKey(
                        name: "FK_InsurancePolicies_Agents_AgentId",
                        column: x => x.AgentId,
                        principalTable: "Agents",
                        principalColumn: "AgentId");
                    table.ForeignKey(
                        name: "FK_InsurancePolicies_Customers_CustomerId",
                        column: x => x.CustomerId,
                        principalTable: "Customers",
                        principalColumn: "CustomerId",
                        onDelete: ReferentialAction.Cascade);
                    table.ForeignKey(
                        name: "FK_InsurancePolicies_InsuranceSchemes_InsuranceSchemeId",
                        column: x => x.InsuranceSchemeId,
                        principalTable: "InsuranceSchemes",
                        principalColumn: "SchemeId",
                        onDelete: ReferentialAction.Cascade);
                    table.ForeignKey(
                        name: "FK_InsurancePolicies_TaxSettings_TaxSettingsTaxId",
                        column: x => x.TaxSettingsTaxId,
                        principalTable: "TaxSettings",
                        principalColumn: "TaxId",
                        onDelete: ReferentialAction.Cascade);
                });

            migrationBuilder.CreateTable(
                name: "WithdrawalRequests",
                columns: table => new
                {
                    WithdrawalRequestId = table.Column<Guid>(type: "uniqueidentifier", nullable: false, defaultValueSql: "NEWSEQUENTIALID()"),
                    AgentId = table.Column<Guid>(type: "uniqueidentifier", nullable: true),
                    CustomerId = table.Column<Guid>(type: "uniqueidentifier", nullable: true),
                    RequestType = table.Column<int>(type: "int", nullable: false),
                    Amount = table.Column<double>(type: "float", nullable: false),
                    RequestDate = table.Column<DateTime>(type: "datetime2", nullable: false),
                    Status = table.Column<int>(type: "int", nullable: false),
                    RejectedReason = table.Column<string>(type: "nvarchar(max)", nullable: true),
                    ApprovedAt = table.Column<DateTime>(type: "datetime2", nullable: true)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_WithdrawalRequests", x => x.WithdrawalRequestId);
                    table.ForeignKey(
                        name: "FK_WithdrawalRequests_Agents_AgentId",
                        column: x => x.AgentId,
                        principalTable: "Agents",
                        principalColumn: "AgentId");
                    table.ForeignKey(
                        name: "FK_WithdrawalRequests_Customers_CustomerId",
                        column: x => x.CustomerId,
                        principalTable: "Customers",
                        principalColumn: "CustomerId");
                });

            migrationBuilder.CreateTable(
                name: "Claims",
                columns: table => new
                {
                    ClaimId = table.Column<Guid>(type: "uniqueidentifier", nullable: false, defaultValueSql: "NEWSEQUENTIALID()"),
                    PolicyId = table.Column<Guid>(type: "uniqueidentifier", nullable: false),
                    CustomerId = table.Column<Guid>(type: "uniqueidentifier", nullable: false),
                    ClaimAmount = table.Column<double>(type: "float", nullable: false),
                    BankAccountNumber = table.Column<string>(type: "nvarchar(20)", maxLength: 20, nullable: false),
                    BankIFSCCode = table.Column<string>(type: "nvarchar(15)", maxLength: 15, nullable: false),
                    Status = table.Column<int>(type: "int", nullable: false),
                    ClaimDate = table.Column<DateTime>(type: "datetime2", nullable: false),
                    ClaimReason = table.Column<string>(type: "nvarchar(max)", nullable: true),
                    RejectionReason = table.Column<string>(type: "nvarchar(max)", nullable: true),
                    ApprovalDate = table.Column<DateTime>(type: "datetime2", nullable: false),
                    RejectionDate = table.Column<DateTime>(type: "datetime2", nullable: false),
                    Claim = table.Column<Guid>(type: "uniqueidentifier", nullable: true)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_Claims", x => x.ClaimId);
                    table.ForeignKey(
                        name: "FK_Claims_Agents_Claim",
                        column: x => x.Claim,
                        principalTable: "Agents",
                        principalColumn: "AgentId");
                    table.ForeignKey(
                        name: "FK_Claims_Customers_CustomerId",
                        column: x => x.CustomerId,
                        principalTable: "Customers",
                        principalColumn: "CustomerId",
                        onDelete: ReferentialAction.Cascade);
                    table.ForeignKey(
                        name: "FK_Claims_InsurancePolicies_PolicyId",
                        column: x => x.PolicyId,
                        principalTable: "InsurancePolicies",
                        principalColumn: "PolicyId",
                        onDelete: ReferentialAction.Restrict);
                });

            migrationBuilder.CreateTable(
                name: "Commissions",
                columns: table => new
                {
                    CommissionId = table.Column<Guid>(type: "uniqueidentifier", nullable: false, defaultValueSql: "NEWSEQUENTIALID()"),
                    CommissionType = table.Column<int>(type: "int", nullable: false),
                    IssueDate = table.Column<DateTime>(type: "datetime2", nullable: false),
                    Amount = table.Column<double>(type: "float", nullable: false),
                    AgentId = table.Column<Guid>(type: "uniqueidentifier", nullable: false),
                    PolicyNo = table.Column<Guid>(type: "uniqueidentifier", nullable: true)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_Commissions", x => x.CommissionId);
                    table.ForeignKey(
                        name: "FK_Commissions_Agents_AgentId",
                        column: x => x.AgentId,
                        principalTable: "Agents",
                        principalColumn: "AgentId",
                        onDelete: ReferentialAction.Cascade);
                    table.ForeignKey(
                        name: "FK_Commissions_InsurancePolicies_PolicyNo",
                        column: x => x.PolicyNo,
                        principalTable: "InsurancePolicies",
                        principalColumn: "PolicyId");
                });

            migrationBuilder.CreateTable(
                name: "Documents",
                columns: table => new
                {
                    DocumentId = table.Column<string>(type: "nvarchar(450)", nullable: false),
                    DocumentName = table.Column<int>(type: "int", maxLength: 250, nullable: false),
                    DocumentPath = table.Column<string>(type: "nvarchar(max)", nullable: false),
                    Status = table.Column<int>(type: "int", nullable: false),
                    CustomerId = table.Column<Guid>(type: "uniqueidentifier", nullable: false),
                    VerifiedById = table.Column<Guid>(type: "uniqueidentifier", nullable: true),
                    RejectedReason = table.Column<string>(type: "nvarchar(max)", nullable: true),
                    PolicyId = table.Column<Guid>(type: "uniqueidentifier", nullable: true)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_Documents", x => x.DocumentId);
                    table.ForeignKey(
                        name: "FK_Documents_Customers_CustomerId",
                        column: x => x.CustomerId,
                        principalTable: "Customers",
                        principalColumn: "CustomerId",
                        onDelete: ReferentialAction.Cascade);
                    table.ForeignKey(
                        name: "FK_Documents_Employees_VerifiedById",
                        column: x => x.VerifiedById,
                        principalTable: "Employees",
                        principalColumn: "EmployeeId");
                    table.ForeignKey(
                        name: "FK_Documents_InsurancePolicies_PolicyId",
                        column: x => x.PolicyId,
                        principalTable: "InsurancePolicies",
                        principalColumn: "PolicyId");
                });

            migrationBuilder.CreateTable(
                name: "Installments",
                columns: table => new
                {
                    InstallmentId = table.Column<Guid>(type: "uniqueidentifier", nullable: false),
                    PolicyId = table.Column<Guid>(type: "uniqueidentifier", nullable: true),
                    DueDate = table.Column<DateTime>(type: "datetime2", nullable: false),
                    PaymentDate = table.Column<DateTime>(type: "datetime2", nullable: true),
                    AmountDue = table.Column<double>(type: "float", nullable: false),
                    AmountPaid = table.Column<double>(type: "float", nullable: true),
                    Status = table.Column<int>(type: "int", nullable: false),
                    PaymentReference = table.Column<string>(type: "nvarchar(max)", nullable: true)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_Installments", x => x.InstallmentId);
                    table.ForeignKey(
                        name: "FK_Installments_InsurancePolicies_PolicyId",
                        column: x => x.PolicyId,
                        principalTable: "InsurancePolicies",
                        principalColumn: "PolicyId");
                });

            migrationBuilder.CreateTable(
                name: "Nomines",
                columns: table => new
                {
                    NomineeId = table.Column<Guid>(type: "uniqueidentifier", nullable: false, defaultValueSql: "NEWSEQUENTIALID()"),
                    NomineeName = table.Column<string>(type: "nvarchar(100)", maxLength: 100, nullable: false),
                    Relationship = table.Column<int>(type: "int", nullable: false),
                    PolicyNo = table.Column<Guid>(type: "uniqueidentifier", nullable: true)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_Nomines", x => x.NomineeId);
                    table.ForeignKey(
                        name: "FK_Nomines_InsurancePolicies_PolicyNo",
                        column: x => x.PolicyNo,
                        principalTable: "InsurancePolicies",
                        principalColumn: "PolicyId");
                });

            migrationBuilder.CreateTable(
                name: "Payments",
                columns: table => new
                {
                    PaymentId = table.Column<Guid>(type: "uniqueidentifier", nullable: false, defaultValueSql: "NEWSEQUENTIALID()"),
                    paymentType = table.Column<int>(type: "int", nullable: false),
                    AmountPaid = table.Column<double>(type: "float", nullable: false),
                    Tax = table.Column<double>(type: "float", nullable: false),
                    TotalPayment = table.Column<double>(type: "float", nullable: false),
                    PaymentDate = table.Column<DateTime>(type: "datetime2", nullable: false),
                    PolicyId = table.Column<Guid>(type: "uniqueidentifier", nullable: true)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_Payments", x => x.PaymentId);
                    table.ForeignKey(
                        name: "FK_Payments_InsurancePolicies_PolicyId",
                        column: x => x.PolicyId,
                        principalTable: "InsurancePolicies",
                        principalColumn: "PolicyId");
                });

            migrationBuilder.InsertData(
                table: "Roles",
                columns: new[] { "Id", "Name" },
                values: new object[,]
                {
                    { new Guid("365c919c-4fee-41f1-b22d-1a0b559b5e11"), "Agent" },
                    { new Guid("bc350535-0f7f-44ec-9d87-923b3d2ce029"), "Customer" },
                    { new Guid("beb947ff-01c9-4db1-b940-88a2ee008182"), "Employee" },
                    { new Guid("c19e9b7e-a54d-42a6-94cf-17221d307003"), "Admin" }
                });

            migrationBuilder.InsertData(
                table: "States",
                columns: new[] { "StateId", "StateName" },
                values: new object[,]
                {
                    { new Guid("17422ca8-4def-4094-b7de-cd591f897afb"), "Arunachal Pradesh" },
                    { new Guid("1ee821b4-c6e0-4ff9-b911-b7d6bccf8f1f"), "Karnataka" },
                    { new Guid("1f8aad90-6212-4b75-960c-ac25c16216ff"), "Uttarakhand" },
                    { new Guid("2a4f33e3-943a-4928-8625-eed7a108c012"), "Meghalaya" },
                    { new Guid("2aa06e2d-0732-44f7-9066-bef9c57afc1e"), "Sikkim" },
                    { new Guid("3b436e80-e1b9-463c-8db5-4ccdffa7ff75"), "Tamil Nadu" },
                    { new Guid("455e0a11-7002-4ef5-9387-36fb33da9896"), "Telangana" },
                    { new Guid("47d7c7c0-5f33-4b15-84db-7967c1f9ac80"), "Goa" },
                    { new Guid("48dc0f80-6386-4450-821f-c42bf3ee4514"), "Rajasthan" },
                    { new Guid("4d10c40c-5482-4bd5-96e4-f59d3bcdc471"), "Maharashtra" },
                    { new Guid("55735c68-5b63-43e2-8710-ff60eef52e64"), "Andhra Pradesh" },
                    { new Guid("5aeb9fc5-7a24-4747-b6df-e1317c4ffcee"), "Nagaland" },
                    { new Guid("5c44e3a1-4f1b-441e-825b-ac56814bea11"), "Uttar Pradesh" },
                    { new Guid("5e05934c-23d1-4867-890a-c081a216ad3c"), "Kerala" },
                    { new Guid("5e9015eb-7116-4fb5-b156-a02d81831197"), "Odisha" },
                    { new Guid("763cf5d0-2230-43fc-8929-66957dd6f6d4"), "Haryana" },
                    { new Guid("805930cc-ccef-496b-81fe-9e54382082c0"), "Bihar" },
                    { new Guid("8249a4ff-9c8e-4ca9-97f0-77e53684d1c2"), "Gujarat" },
                    { new Guid("890ad1ec-d1d3-48bb-8999-3280a33c0367"), "Mizoram" },
                    { new Guid("8c75c389-77ec-4da0-91b8-6366771ca45c"), "Assam" },
                    { new Guid("a10cd463-0861-4a55-9a42-01b1b7b95e13"), "West Bengal" },
                    { new Guid("a84b877f-4445-4c02-9249-a9c46deeae1c"), "Himachal Pradesh" },
                    { new Guid("ace59a1b-c61f-4899-83da-55df6dccd442"), "Punjab" },
                    { new Guid("b9d66452-15d5-413f-8011-72f0214d7009"), "Tripura" },
                    { new Guid("d4eebe08-d389-455e-941f-3c4b9428ffd0"), "Madhya Pradesh" },
                    { new Guid("f43ef4d2-5eec-4e0e-9c31-aaede3b806be"), "Manipur" },
                    { new Guid("f47e7f15-747d-4134-b483-1a0408e02889"), "Jharkhand" },
                    { new Guid("fb79ada4-cf92-48cf-8472-a3379513982a"), "Chhattisgarh" }
                });

            migrationBuilder.InsertData(
                table: "Cities",
                columns: new[] { "CityId", "CityName", "StateId" },
                values: new object[,]
                {
                    { new Guid("007111fb-091f-41c3-af72-3d24f5643c5f"), "Allahabad", new Guid("5c44e3a1-4f1b-441e-825b-ac56814bea11") },
                    { new Guid("00b5b348-af4a-4d15-a347-196c69d69dce"), "Ujjain", new Guid("d4eebe08-d389-455e-941f-3c4b9428ffd0") },
                    { new Guid("04b48196-5a3f-4240-a2af-32434c6c7509"), "Rajkot", new Guid("8249a4ff-9c8e-4ca9-97f0-77e53684d1c2") },
                    { new Guid("05c0c479-5e49-4a76-b483-c8f846d8670e"), "Bongaigaon", new Guid("8c75c389-77ec-4da0-91b8-6366771ca45c") },
                    { new Guid("05d765a5-c7cb-4d55-9c42-2defd834fbdd"), "Mahasamund", new Guid("fb79ada4-cf92-48cf-8472-a3379513982a") },
                    { new Guid("07e3f892-5de4-4ff9-9ce5-0133d9ece809"), "Chhindwara", new Guid("d4eebe08-d389-455e-941f-3c4b9428ffd0") },
                    { new Guid("081bcc71-80fb-4a69-8be4-abf66385466c"), "Dharamshala", new Guid("a84b877f-4445-4c02-9249-a9c46deeae1c") },
                    { new Guid("08aa1e58-b80c-4b07-a9fc-d0a895a2cb3b"), "Jorhat", new Guid("8c75c389-77ec-4da0-91b8-6366771ca45c") },
                    { new Guid("0cd44f1a-5a4e-47c9-afa2-36e0a206f648"), "Tinsukia", new Guid("8c75c389-77ec-4da0-91b8-6366771ca45c") },
                    { new Guid("0e150b9e-f05f-42b3-9cc4-8c5c4bde60b2"), "Ambala", new Guid("763cf5d0-2230-43fc-8929-66957dd6f6d4") },
                    { new Guid("0fbabdd9-46bd-4303-8f2f-26af13aebd6d"), "Haldia", new Guid("a10cd463-0861-4a55-9a42-01b1b7b95e13") },
                    { new Guid("0fcbe8a2-3cc9-4242-8966-ac82518c484f"), "Pasighat", new Guid("17422ca8-4def-4094-b7de-cd591f897afb") },
                    { new Guid("0fce0ad5-e6af-46fd-9744-e7a8707bd4c4"), "Darjeeling", new Guid("a10cd463-0861-4a55-9a42-01b1b7b95e13") },
                    { new Guid("110bd6ba-8d4f-47b2-86f6-437e31ea3555"), "Solapur", new Guid("4d10c40c-5482-4bd5-96e4-f59d3bcdc471") },
                    { new Guid("114c467b-179f-4fb1-8b55-8f33899ba939"), "Meerut", new Guid("5c44e3a1-4f1b-441e-825b-ac56814bea11") },
                    { new Guid("1191110b-d05a-4d10-bcca-b8b7ea527c72"), "Sagar", new Guid("d4eebe08-d389-455e-941f-3c4b9428ffd0") },
                    { new Guid("1301c6d2-8e11-47e6-969e-b1aef040f9f4"), "Anand", new Guid("8249a4ff-9c8e-4ca9-97f0-77e53684d1c2") },
                    { new Guid("13071ed2-4406-4d34-bb94-b992465d2b6a"), "Jamnagar", new Guid("8249a4ff-9c8e-4ca9-97f0-77e53684d1c2") },
                    { new Guid("134f0baf-af08-4b46-9080-5b928d77eb61"), "Purnia", new Guid("805930cc-ccef-496b-81fe-9e54382082c0") },
                    { new Guid("14b0a021-58c0-474c-a84f-70563034e4b8"), "Rajnandgaon", new Guid("fb79ada4-cf92-48cf-8472-a3379513982a") },
                    { new Guid("14cfa303-9d19-48f8-9ba2-7e8cea6233c8"), "Solan", new Guid("a84b877f-4445-4c02-9249-a9c46deeae1c") },
                    { new Guid("1528b01c-ceb8-4695-b58a-4c043655438e"), "Ziro", new Guid("17422ca8-4def-4094-b7de-cd591f897afb") },
                    { new Guid("15d9d4cb-121d-4b10-8af2-151f24ed2ce8"), "Ambikapur", new Guid("fb79ada4-cf92-48cf-8472-a3379513982a") },
                    { new Guid("16452267-b63d-453c-b665-4809664b7931"), "Thoothukudi", new Guid("3b436e80-e1b9-463c-8db5-4ccdffa7ff75") },
                    { new Guid("16c5db34-c4db-4c95-a1c3-67f5c41f401e"), "Rajahmundry", new Guid("55735c68-5b63-43e2-8710-ff60eef52e64") },
                    { new Guid("17befca7-0b72-440f-a40c-10d9a43182d7"), "Nashik", new Guid("4d10c40c-5482-4bd5-96e4-f59d3bcdc471") },
                    { new Guid("18eb95eb-d04b-49f5-9a3e-3e646fe6f039"), "Sasaram", new Guid("805930cc-ccef-496b-81fe-9e54382082c0") },
                    { new Guid("1a844524-c51c-42d1-b982-0a6f05f47faa"), "Porvorim", new Guid("47d7c7c0-5f33-4b15-84db-7967c1f9ac80") },
                    { new Guid("1bb3dc55-3197-4dd0-bb18-83661c597fdb"), "Hamirpur", new Guid("a84b877f-4445-4c02-9249-a9c46deeae1c") },
                    { new Guid("1c32c7d9-12e4-40e6-838d-b1a73c1dae57"), "Begusarai", new Guid("805930cc-ccef-496b-81fe-9e54382082c0") },
                    { new Guid("1ca225ea-2c44-4b6c-b749-7a0e9cfd5265"), "Pune", new Guid("4d10c40c-5482-4bd5-96e4-f59d3bcdc471") },
                    { new Guid("1d213024-97ba-43f1-af39-22983056a41f"), "Ahmedabad", new Guid("8249a4ff-9c8e-4ca9-97f0-77e53684d1c2") },
                    { new Guid("1fd9e27f-86d6-4100-845a-8b170012c612"), "Mehsana", new Guid("8249a4ff-9c8e-4ca9-97f0-77e53684d1c2") },
                    { new Guid("2329f89e-e161-4906-9dd5-4ac4b21d1b66"), "Siliguri", new Guid("a10cd463-0861-4a55-9a42-01b1b7b95e13") },
                    { new Guid("277bca0a-e0b0-4b99-b647-0f8e44e3e484"), "Bhopal", new Guid("d4eebe08-d389-455e-941f-3c4b9428ffd0") },
                    { new Guid("29dbbf76-3582-46ca-a80d-2c3d32616b12"), "Vadodara", new Guid("8249a4ff-9c8e-4ca9-97f0-77e53684d1c2") },
                    { new Guid("2b8812ac-3811-4802-9d5c-4f6e02cb0602"), "Vijayawada", new Guid("55735c68-5b63-43e2-8710-ff60eef52e64") },
                    { new Guid("2d34619b-9c39-44d0-86c4-79c48d032381"), "Bellary", new Guid("1ee821b4-c6e0-4ff9-b911-b7d6bccf8f1f") },
                    { new Guid("2ef15c16-424d-45e7-bfc2-0447e76cedbf"), "Khammam", new Guid("455e0a11-7002-4ef5-9387-36fb33da9896") },
                    { new Guid("31367d45-b4ad-48d7-9d57-150039073cf4"), "Mumbai", new Guid("4d10c40c-5482-4bd5-96e4-f59d3bcdc471") },
                    { new Guid("339c20cc-9170-488f-acbe-10e6a580fd41"), "Kakinada", new Guid("55735c68-5b63-43e2-8710-ff60eef52e64") },
                    { new Guid("34815e60-2788-427b-8002-e32b08a31a48"), "Raigarh", new Guid("fb79ada4-cf92-48cf-8472-a3379513982a") },
                    { new Guid("36161c06-5cce-4be7-a1eb-adb8db26d3bd"), "Itanagar", new Guid("17422ca8-4def-4094-b7de-cd591f897afb") },
                    { new Guid("380f5cf4-7ea4-45d9-9fde-188b5d7b8f0d"), "Sikar", new Guid("48dc0f80-6386-4450-821f-c42bf3ee4514") },
                    { new Guid("3ccf48c5-4236-41bb-9aae-075eb5233ef5"), "Muzaffarpur", new Guid("805930cc-ccef-496b-81fe-9e54382082c0") },
                    { new Guid("3d5ff21a-6c2f-4a31-a2fc-c42976f44ab7"), "Godda", new Guid("f47e7f15-747d-4134-b483-1a0408e02889") },
                    { new Guid("3da30652-3a94-44a3-b376-257932c658d5"), "Ratlam", new Guid("d4eebe08-d389-455e-941f-3c4b9428ffd0") },
                    { new Guid("3df57fc8-0531-4d97-92d9-70c7ff386235"), "Faridabad", new Guid("763cf5d0-2230-43fc-8929-66957dd6f6d4") },
                    { new Guid("3f2b605a-b21c-4dad-83a6-33596f621519"), "Belgaum", new Guid("1ee821b4-c6e0-4ff9-b911-b7d6bccf8f1f") },
                    { new Guid("410bc4fc-5184-4b77-950d-62869f1292b7"), "Kozhikode", new Guid("5e05934c-23d1-4867-890a-c081a216ad3c") },
                    { new Guid("412ba8bb-8413-4081-a7db-548e06fbc25a"), "Mahbubnagar", new Guid("455e0a11-7002-4ef5-9387-36fb33da9896") },
                    { new Guid("4363e20a-1303-401b-a991-5355132e436c"), "Ghaziabad", new Guid("5c44e3a1-4f1b-441e-825b-ac56814bea11") },
                    { new Guid("454fc9b0-66ca-483e-9735-9177a82ed948"), "Alappuzha", new Guid("5e05934c-23d1-4867-890a-c081a216ad3c") },
                    { new Guid("45b79571-2f06-4390-b6ce-86ca0823e95a"), "Vellore", new Guid("3b436e80-e1b9-463c-8db5-4ccdffa7ff75") },
                    { new Guid("4668ffc7-5a57-474c-a377-9a0d6d32a7e7"), "Darbhanga", new Guid("805930cc-ccef-496b-81fe-9e54382082c0") },
                    { new Guid("4950ca89-0afd-4178-b0fc-bca693da967c"), "Jamshedpur", new Guid("f47e7f15-747d-4134-b483-1a0408e02889") },
                    { new Guid("49cbfd24-f33d-45fb-8830-10f324168571"), "Hajipur", new Guid("805930cc-ccef-496b-81fe-9e54382082c0") },
                    { new Guid("4bae821e-0bf0-485f-a0c7-f6042f55b247"), "Deoghar", new Guid("f47e7f15-747d-4134-b483-1a0408e02889") },
                    { new Guid("4c9757b1-59a7-4038-96b5-a741bf450e94"), "Canacona", new Guid("47d7c7c0-5f33-4b15-84db-7967c1f9ac80") },
                    { new Guid("4cbf1600-be69-4601-8e66-d078f2b4c91e"), "Ramgarh", new Guid("f47e7f15-747d-4134-b483-1a0408e02889") },
                    { new Guid("4df27889-b663-4bb7-bb0c-d7178fc79140"), "Bhiwani", new Guid("763cf5d0-2230-43fc-8929-66957dd6f6d4") },
                    { new Guid("50df9aa6-bd60-4a18-ab15-dd17e0938bc3"), "Tirunelveli", new Guid("3b436e80-e1b9-463c-8db5-4ccdffa7ff75") },
                    { new Guid("51d2702e-5ab4-46e3-b598-79d27ea8eda4"), "Thane", new Guid("4d10c40c-5482-4bd5-96e4-f59d3bcdc471") },
                    { new Guid("5218294b-60a9-4bfa-ad8d-74aeabf3ecdd"), "Amravati", new Guid("4d10c40c-5482-4bd5-96e4-f59d3bcdc471") },
                    { new Guid("55f2456d-d48b-4898-9645-78fc13fe3bcf"), "Dibrugarh", new Guid("8c75c389-77ec-4da0-91b8-6366771ca45c") },
                    { new Guid("58ac86a9-6f2e-4d16-8617-7e2b58057af8"), "Karimnagar", new Guid("455e0a11-7002-4ef5-9387-36fb33da9896") },
                    { new Guid("591c63ce-7286-4f9e-be57-76555877064c"), "Kollam", new Guid("5e05934c-23d1-4867-890a-c081a216ad3c") },
                    { new Guid("59d09965-cd80-49db-b24a-7ac6bf99692f"), "Giridih", new Guid("f47e7f15-747d-4134-b483-1a0408e02889") },
                    { new Guid("5a24603f-3c3e-4ce9-9292-4bfc40226f20"), "Gurgaon", new Guid("763cf5d0-2230-43fc-8929-66957dd6f6d4") },
                    { new Guid("5de75c07-a45f-42ad-9d8c-d7502248a8f0"), "Tezpur", new Guid("8c75c389-77ec-4da0-91b8-6366771ca45c") },
                    { new Guid("5f05c489-595a-4e42-98a5-c3a14ac81bd7"), "Ara", new Guid("805930cc-ccef-496b-81fe-9e54382082c0") },
                    { new Guid("60781e7f-3787-4c93-a662-1c7dc15a08fc"), "Bardhaman", new Guid("a10cd463-0861-4a55-9a42-01b1b7b95e13") },
                    { new Guid("617d9b05-dead-4eb0-a490-ef91bae9c3c3"), "Mangalore", new Guid("1ee821b4-c6e0-4ff9-b911-b7d6bccf8f1f") },
                    { new Guid("6325f562-25cd-4f53-938c-02c27759f885"), "Aligarh", new Guid("5c44e3a1-4f1b-441e-825b-ac56814bea11") },
                    { new Guid("63d660e2-365e-41db-bed6-395f73f05730"), "Lucknow", new Guid("5c44e3a1-4f1b-441e-825b-ac56814bea11") },
                    { new Guid("6492e394-7911-4cb5-bde9-335b0f65e811"), "Thrissur", new Guid("5e05934c-23d1-4867-890a-c081a216ad3c") },
                    { new Guid("6737d404-0f9c-40a4-9457-6ee374e99ab8"), "Anini", new Guid("17422ca8-4def-4094-b7de-cd591f897afb") },
                    { new Guid("677c8488-f3e6-4ac3-86fa-154dcc0064e7"), "Bicholim", new Guid("47d7c7c0-5f33-4b15-84db-7967c1f9ac80") },
                    { new Guid("6794d870-7ce2-4060-941e-d6b20773308e"), "Chennai", new Guid("3b436e80-e1b9-463c-8db5-4ccdffa7ff75") },
                    { new Guid("67c811a4-a55a-4fea-b02d-a88b2ef870eb"), "Salem", new Guid("3b436e80-e1b9-463c-8db5-4ccdffa7ff75") },
                    { new Guid("67f55fb2-e1e4-42d2-93ec-6100e621d802"), "Jalpaiguri", new Guid("a10cd463-0861-4a55-9a42-01b1b7b95e13") },
                    { new Guid("68472c33-ac67-4cbe-9aab-e6af3e132c73"), "Bijapur", new Guid("1ee821b4-c6e0-4ff9-b911-b7d6bccf8f1f") },
                    { new Guid("68944119-8a9b-45f2-b047-393c82aef64d"), "Mandi", new Guid("a84b877f-4445-4c02-9249-a9c46deeae1c") },
                    { new Guid("6929f2f4-31f0-492b-8e68-71062a1643f0"), "Kannur", new Guid("5e05934c-23d1-4867-890a-c081a216ad3c") },
                    { new Guid("6c38f038-fc67-41b0-88fa-7578c2cde196"), "Sirsa", new Guid("763cf5d0-2230-43fc-8929-66957dd6f6d4") },
                    { new Guid("6cec7177-a0bc-4b56-90e7-8a0974364209"), "Palakkad", new Guid("5e05934c-23d1-4867-890a-c081a216ad3c") },
                    { new Guid("6f7e2feb-b52d-4e0d-9007-a94c0d7b6acd"), "Howrah", new Guid("a10cd463-0861-4a55-9a42-01b1b7b95e13") },
                    { new Guid("6f84eebe-d11c-432b-abdb-bf785b7f05b0"), "Jabalpur", new Guid("d4eebe08-d389-455e-941f-3c4b9428ffd0") },
                    { new Guid("701a6716-659a-491b-85c6-cf0e909a304b"), "Kadapa", new Guid("55735c68-5b63-43e2-8710-ff60eef52e64") },
                    { new Guid("72d3f5d8-3047-4a7b-82e0-1aae38834710"), "Patna", new Guid("805930cc-ccef-496b-81fe-9e54382082c0") },
                    { new Guid("737501d1-23e6-4c0a-a5d3-22de67169f13"), "Tiruchirappalli", new Guid("3b436e80-e1b9-463c-8db5-4ccdffa7ff75") },
                    { new Guid("74d5168d-5881-4ed1-a45c-ad7879e23d03"), "Bareilly", new Guid("5c44e3a1-4f1b-441e-825b-ac56814bea11") },
                    { new Guid("763c9071-9a68-47ad-8c36-f390a630a7cd"), "Shimoga", new Guid("1ee821b4-c6e0-4ff9-b911-b7d6bccf8f1f") },
                    { new Guid("79c1d3db-59ce-4e91-b50b-58dca502e654"), "Tezu", new Guid("17422ca8-4def-4094-b7de-cd591f897afb") },
                    { new Guid("7a107982-3a3d-4a94-b530-a39924be99bd"), "Bharatpur", new Guid("48dc0f80-6386-4450-821f-c42bf3ee4514") },
                    { new Guid("7ab9c3c4-ae25-452c-a929-345cc7bc66e6"), "Yamunanagar", new Guid("763cf5d0-2230-43fc-8929-66957dd6f6d4") },
                    { new Guid("7c31945f-9b51-45dd-9161-301991752028"), "Bilaspur", new Guid("a84b877f-4445-4c02-9249-a9c46deeae1c") },
                    { new Guid("7d8565ca-deca-47fc-b857-7ccf63d89fb2"), "Dindigul", new Guid("3b436e80-e1b9-463c-8db5-4ccdffa7ff75") },
                    { new Guid("817bcc34-5cea-412e-8367-c08e48a38880"), "Hazaribagh", new Guid("f47e7f15-747d-4134-b483-1a0408e02889") },
                    { new Guid("8414df58-f850-4362-a757-170e19e0a56f"), "Gwalior", new Guid("d4eebe08-d389-455e-941f-3c4b9428ffd0") },
                    { new Guid("89666e85-c850-4de9-a77a-92ed91f68507"), "Durg", new Guid("fb79ada4-cf92-48cf-8472-a3379513982a") },
                    { new Guid("8bb8ea66-38f6-4816-b225-af62276d0483"), "Gaya", new Guid("805930cc-ccef-496b-81fe-9e54382082c0") },
                    { new Guid("8d1647c4-cefc-41a2-9c67-88f7fda3c8ff"), "Bhagalpur", new Guid("805930cc-ccef-496b-81fe-9e54382082c0") },
                    { new Guid("8e0b9548-c55a-4281-aee4-947d8f276b10"), "Hisar", new Guid("763cf5d0-2230-43fc-8929-66957dd6f6d4") },
                    { new Guid("8f02ce3e-d743-4da7-a788-68a96241cc5b"), "Bikaner", new Guid("48dc0f80-6386-4450-821f-c42bf3ee4514") },
                    { new Guid("919a8f88-0ec7-47d7-990b-8c8756fd2a13"), "Mancherial", new Guid("455e0a11-7002-4ef5-9387-36fb33da9896") },
                    { new Guid("9237b5cf-4f68-4a06-b393-5f03aa9be5a3"), "Kochi", new Guid("5e05934c-23d1-4867-890a-c081a216ad3c") },
                    { new Guid("96f7a8eb-f020-418b-ad08-ce9fcf1012f8"), "Davanagere", new Guid("1ee821b4-c6e0-4ff9-b911-b7d6bccf8f1f") },
                    { new Guid("9723da62-eb3d-4931-9427-29004a490edd"), "Tawang", new Guid("17422ca8-4def-4094-b7de-cd591f897afb") },
                    { new Guid("98431de5-08c1-4fe9-ac06-2d8b41b3b18c"), "Anantapur", new Guid("55735c68-5b63-43e2-8710-ff60eef52e64") },
                    { new Guid("98a51817-5e5b-4354-85aa-4f825635ab02"), "Jagdalpur", new Guid("fb79ada4-cf92-48cf-8472-a3379513982a") },
                    { new Guid("9c32bb0a-174a-47ae-8101-1fe381f6c816"), "Nahan", new Guid("a84b877f-4445-4c02-9249-a9c46deeae1c") },
                    { new Guid("9cb7a3d4-2e7d-4b2c-9155-d067285722aa"), "Rohtak", new Guid("763cf5d0-2230-43fc-8929-66957dd6f6d4") },
                    { new Guid("9d898733-891f-40ed-ab03-44b177656c92"), "Tirupati", new Guid("55735c68-5b63-43e2-8710-ff60eef52e64") },
                    { new Guid("9fc0b28d-8e40-4e7c-b0b4-7087d179f5d6"), "Karnal", new Guid("763cf5d0-2230-43fc-8929-66957dd6f6d4") },
                    { new Guid("9fccb20c-44d5-4faf-a9c1-4d08963ec7b8"), "Dhanbad", new Guid("f47e7f15-747d-4134-b483-1a0408e02889") },
                    { new Guid("9fcd1bc2-941d-4c8c-8a47-09891a14ce48"), "Mysore", new Guid("1ee821b4-c6e0-4ff9-b911-b7d6bccf8f1f") },
                    { new Guid("9ffd90d5-883c-4c1c-824f-b6e49a2f7cf4"), "Nellore", new Guid("55735c68-5b63-43e2-8710-ff60eef52e64") },
                    { new Guid("a04737d4-8ba5-42d1-8663-c253372ec5c0"), "Bhavnagar", new Guid("8249a4ff-9c8e-4ca9-97f0-77e53684d1c2") },
                    { new Guid("a0799e60-c8f0-4137-b3d9-cdedf0f03fb7"), "Nizamabad", new Guid("455e0a11-7002-4ef5-9387-36fb33da9896") },
                    { new Guid("a0f4d3c4-abdd-412b-a854-2df812dca676"), "Vasco da Gama", new Guid("47d7c7c0-5f33-4b15-84db-7967c1f9ac80") },
                    { new Guid("a2567612-d3ac-4842-8ae9-82120ec66034"), "Bilaspur", new Guid("fb79ada4-cf92-48cf-8472-a3379513982a") },
                    { new Guid("a38ffc6f-efea-4037-bc5e-3240034a609e"), "Surat", new Guid("8249a4ff-9c8e-4ca9-97f0-77e53684d1c2") },
                    { new Guid("a415c42b-9509-4eeb-90be-232895cd9eec"), "Along", new Guid("17422ca8-4def-4094-b7de-cd591f897afb") },
                    { new Guid("a5325172-78cf-45c6-9cbd-f498e2b1b367"), "Madurai", new Guid("3b436e80-e1b9-463c-8db5-4ccdffa7ff75") },
                    { new Guid("a56b7408-8abf-4a06-9d37-dc159cc13247"), "Shimla", new Guid("a84b877f-4445-4c02-9249-a9c46deeae1c") },
                    { new Guid("a69a3f13-3ec1-4413-9102-f5966f9fed56"), "Sangli", new Guid("4d10c40c-5482-4bd5-96e4-f59d3bcdc471") },
                    { new Guid("a6c00223-c71e-497a-a8db-14bf43932e7b"), "Hubli", new Guid("1ee821b4-c6e0-4ff9-b911-b7d6bccf8f1f") },
                    { new Guid("a9241167-84cf-4e68-be05-0e45b319c7a1"), "Udupi", new Guid("1ee821b4-c6e0-4ff9-b911-b7d6bccf8f1f") },
                    { new Guid("aa91926b-946c-4f53-a9fb-b48c707f659e"), "Guwahati", new Guid("8c75c389-77ec-4da0-91b8-6366771ca45c") },
                    { new Guid("ad9c92e5-708b-4e44-83bc-4b6688c680ae"), "Kolkata", new Guid("a10cd463-0861-4a55-9a42-01b1b7b95e13") },
                    { new Guid("ae5c8847-2941-4411-9907-49edbc06bda5"), "Silchar", new Guid("8c75c389-77ec-4da0-91b8-6366771ca45c") },
                    { new Guid("aef74684-4e72-4131-95d2-54b2571ca03e"), "Rewa", new Guid("d4eebe08-d389-455e-941f-3c4b9428ffd0") },
                    { new Guid("b2443b9a-b028-4249-99e6-caf75226eca2"), "Durgapur", new Guid("a10cd463-0861-4a55-9a42-01b1b7b95e13") },
                    { new Guid("b527edf1-2e8e-40ef-aff6-032bb858cb64"), "Manali", new Guid("a84b877f-4445-4c02-9249-a9c46deeae1c") },
                    { new Guid("b5e1d7a0-10c8-4f7e-bab9-efbc52772866"), "Goalpara", new Guid("8c75c389-77ec-4da0-91b8-6366771ca45c") },
                    { new Guid("b5fd92a3-79b5-41b5-a911-5cfaebbc7157"), "Jaisalmer", new Guid("48dc0f80-6386-4450-821f-c42bf3ee4514") },
                    { new Guid("b66c949f-9593-4f53-aada-72d375c25cb2"), "Barpeta", new Guid("8c75c389-77ec-4da0-91b8-6366771ca45c") },
                    { new Guid("b8dd7195-05f9-4433-852a-9a5a99ee913f"), "Alwar", new Guid("48dc0f80-6386-4450-821f-c42bf3ee4514") },
                    { new Guid("b923bc04-f401-45eb-bc20-6b119ada3a39"), "Pakur", new Guid("f47e7f15-747d-4134-b483-1a0408e02889") },
                    { new Guid("b9926766-6107-4550-8808-e765bf978109"), "Erode", new Guid("3b436e80-e1b9-463c-8db5-4ccdffa7ff75") },
                    { new Guid("ba07f271-a8fd-48f1-8fdf-6b5483f9a111"), "Gandhinagar", new Guid("8249a4ff-9c8e-4ca9-97f0-77e53684d1c2") },
                    { new Guid("bdd16177-ea89-4948-a4c9-6c8ca845823b"), "Thiruvananthapuram", new Guid("5e05934c-23d1-4867-890a-c081a216ad3c") },
                    { new Guid("bf30e4eb-9646-43da-9a5d-e1464d15cf45"), "Panipat", new Guid("763cf5d0-2230-43fc-8929-66957dd6f6d4") },
                    { new Guid("bf5681ba-c4c9-46fd-ba2c-fb2f8b22d850"), "Jaipur", new Guid("48dc0f80-6386-4450-821f-c42bf3ee4514") },
                    { new Guid("c15bbf14-67d3-4993-a621-1b1b500d3893"), "Siddipet", new Guid("455e0a11-7002-4ef5-9387-36fb33da9896") },
                    { new Guid("c400b37b-5589-482e-b039-e55e6619e2bc"), "Chamba", new Guid("a84b877f-4445-4c02-9249-a9c46deeae1c") },
                    { new Guid("c4740880-4431-4a46-8995-1361066d1518"), "Guntur", new Guid("55735c68-5b63-43e2-8710-ff60eef52e64") },
                    { new Guid("c7ab82b2-8f69-4b1f-8d6c-1231057b7248"), "Ramagundam", new Guid("455e0a11-7002-4ef5-9387-36fb33da9896") },
                    { new Guid("cb0c6571-b617-4001-9e08-a7a809048fb7"), "Mapusa", new Guid("47d7c7c0-5f33-4b15-84db-7967c1f9ac80") },
                    { new Guid("cb393a34-d093-425f-b7ba-855d3132f7b5"), "Kolhapur", new Guid("4d10c40c-5482-4bd5-96e4-f59d3bcdc471") },
                    { new Guid("ccb6db4b-eefc-4846-8b62-264488e8aafb"), "Nagpur", new Guid("4d10c40c-5482-4bd5-96e4-f59d3bcdc471") },
                    { new Guid("cde43d38-25a1-4a11-a94e-f55702539b69"), "Raipur", new Guid("fb79ada4-cf92-48cf-8472-a3379513982a") },
                    { new Guid("ceac7b3f-624e-4f6e-9f38-a16fb5fcb30e"), "Asansol", new Guid("a10cd463-0861-4a55-9a42-01b1b7b95e13") },
                    { new Guid("cf2f17de-b4e6-43a4-ad02-6cd47c9efb7a"), "Roing", new Guid("17422ca8-4def-4094-b7de-cd591f897afb") },
                    { new Guid("d0ed2d47-8ff3-4ee0-bd44-f0ce44f8831e"), "Jodhpur", new Guid("48dc0f80-6386-4450-821f-c42bf3ee4514") },
                    { new Guid("d1579be1-18fb-4f20-9698-b8bc0d8d03b2"), "Indore", new Guid("d4eebe08-d389-455e-941f-3c4b9428ffd0") },
                    { new Guid("d35650d8-3596-432b-b9cc-accdc05fca34"), "Dona Paula", new Guid("47d7c7c0-5f33-4b15-84db-7967c1f9ac80") },
                    { new Guid("d46b2926-2f98-4323-a905-0cfa449b7596"), "Margao", new Guid("47d7c7c0-5f33-4b15-84db-7967c1f9ac80") },
                    { new Guid("d4766815-f22e-4281-99cc-b1a258de6f5b"), "Kullu", new Guid("a84b877f-4445-4c02-9249-a9c46deeae1c") },
                    { new Guid("d47d5604-0f9d-435c-b16f-cd4771481f7f"), "Udaipur", new Guid("48dc0f80-6386-4450-821f-c42bf3ee4514") },
                    { new Guid("d4a9ee27-7c68-46f4-849a-74d3a2702706"), "Navsari", new Guid("8249a4ff-9c8e-4ca9-97f0-77e53684d1c2") },
                    { new Guid("d666e70c-673a-4e02-9849-931c9b45b2c1"), "Satna", new Guid("d4eebe08-d389-455e-941f-3c4b9428ffd0") },
                    { new Guid("d9cc703c-0b1d-4636-aa1d-5623f55b206b"), "Chittoor", new Guid("55735c68-5b63-43e2-8710-ff60eef52e64") },
                    { new Guid("dac3d4c8-b45c-4786-bf80-b922b58bbed7"), "Bokaro", new Guid("f47e7f15-747d-4134-b483-1a0408e02889") },
                    { new Guid("dad931e1-95ec-44c2-a30f-6a447e218db2"), "Panaji", new Guid("47d7c7c0-5f33-4b15-84db-7967c1f9ac80") },
                    { new Guid("db849f28-01db-4ab9-9104-fb70e61cc8f1"), "Bangalore", new Guid("1ee821b4-c6e0-4ff9-b911-b7d6bccf8f1f") },
                    { new Guid("dc7d5aad-56de-46e0-ada9-2fc53e6400d1"), "Bomdila", new Guid("17422ca8-4def-4094-b7de-cd591f897afb") },
                    { new Guid("dd9b6ca8-0613-46ed-a7d6-0b7798a1dd27"), "Moradabad", new Guid("5c44e3a1-4f1b-441e-825b-ac56814bea11") },
                    { new Guid("de48ca4a-1cc8-49fc-9772-21d13e5e0893"), "Visakhapatnam", new Guid("55735c68-5b63-43e2-8710-ff60eef52e64") },
                    { new Guid("df36e66b-cfb9-487c-9fa5-c024a0a54378"), "Kota", new Guid("48dc0f80-6386-4450-821f-c42bf3ee4514") },
                    { new Guid("e0b7a904-8dab-4641-ab1d-d449c8ed78e6"), "Hyderabad", new Guid("455e0a11-7002-4ef5-9387-36fb33da9896") },
                    { new Guid("e1bce2f0-e556-4550-8d07-bdf3a0c0f765"), "Changlang", new Guid("17422ca8-4def-4094-b7de-cd591f897afb") },
                    { new Guid("e81ce01c-45a9-4089-a90d-973055044384"), "Varanasi", new Guid("5c44e3a1-4f1b-441e-825b-ac56814bea11") },
                    { new Guid("e95112bc-b0a1-4fd4-b30e-8bf39f03a54a"), "Malappuram", new Guid("5e05934c-23d1-4867-890a-c081a216ad3c") },
                    { new Guid("ea72e8c0-4531-454f-bb34-5a706e80be7f"), "Ajmer", new Guid("48dc0f80-6386-4450-821f-c42bf3ee4514") },
                    { new Guid("ea9d2244-b78a-43fb-87d9-c4309164e800"), "Adilabad", new Guid("455e0a11-7002-4ef5-9387-36fb33da9896") },
                    { new Guid("ebcd22e0-b15c-4a82-98c3-044857b61897"), "Ponda", new Guid("47d7c7c0-5f33-4b15-84db-7967c1f9ac80") },
                    { new Guid("ec119acb-9ad1-4533-8276-7d06d1d586ae"), "Kanpur", new Guid("5c44e3a1-4f1b-441e-825b-ac56814bea11") },
                    { new Guid("ec33ac24-0db7-408c-9de4-84887f4075aa"), "Ranchi", new Guid("f47e7f15-747d-4134-b483-1a0408e02889") },
                    { new Guid("ee64e304-1694-40f8-ab96-b4d49d624303"), "Calangute", new Guid("47d7c7c0-5f33-4b15-84db-7967c1f9ac80") },
                    { new Guid("ef17feb0-4aa9-453a-97a7-8dc456dea7ca"), "Nagaon", new Guid("8c75c389-77ec-4da0-91b8-6366771ca45c") },
                    { new Guid("f3043d8f-63f4-4418-82c4-ba5a8f14cd8d"), "Idukki", new Guid("5e05934c-23d1-4867-890a-c081a216ad3c") },
                    { new Guid("f51a544c-009f-4c03-80d9-6c11261e32dc"), "Warangal", new Guid("455e0a11-7002-4ef5-9387-36fb33da9896") },
                    { new Guid("f61610ae-970d-49af-a911-241c87b1dde4"), "Aurangabad", new Guid("4d10c40c-5482-4bd5-96e4-f59d3bcdc471") },
                    { new Guid("f9a87c3c-1844-4b41-bcc2-7234c2ec9bbe"), "Malda", new Guid("a10cd463-0861-4a55-9a42-01b1b7b95e13") },
                    { new Guid("fb0f7838-9f04-4442-8cd9-658cd2ad88c1"), "Coimbatore", new Guid("3b436e80-e1b9-463c-8db5-4ccdffa7ff75") },
                    { new Guid("fbfcf37b-e6c0-42fd-aa3c-5f1c5390697d"), "Agra", new Guid("5c44e3a1-4f1b-441e-825b-ac56814bea11") },
                    { new Guid("feaa7468-8014-4f18-a61c-0aa303ce1cc5"), "Bhilai", new Guid("fb79ada4-cf92-48cf-8472-a3379513982a") },
                    { new Guid("fec7a9ab-5ccc-4893-9f8b-6d56dc231347"), "Korba", new Guid("fb79ada4-cf92-48cf-8472-a3379513982a") }
                });

            migrationBuilder.CreateIndex(
                name: "IX_Address_CityId",
                table: "Address",
                column: "CityId");

            migrationBuilder.CreateIndex(
                name: "IX_Admins_UserId",
                table: "Admins",
                column: "UserId",
                unique: true,
                filter: "[UserId] IS NOT NULL");

            migrationBuilder.CreateIndex(
                name: "IX_AgentEarnings_AgentId",
                table: "AgentEarnings",
                column: "AgentId");

            migrationBuilder.CreateIndex(
                name: "IX_Agents_AddressId",
                table: "Agents",
                column: "AddressId");

            migrationBuilder.CreateIndex(
                name: "IX_Agents_UserId",
                table: "Agents",
                column: "UserId",
                unique: true,
                filter: "[UserId] IS NOT NULL");

            migrationBuilder.CreateIndex(
                name: "IX_Cities_StateId",
                table: "Cities",
                column: "StateId");

            migrationBuilder.CreateIndex(
                name: "IX_Claims_Claim",
                table: "Claims",
                column: "Claim");

            migrationBuilder.CreateIndex(
                name: "IX_Claims_CustomerId",
                table: "Claims",
                column: "CustomerId");

            migrationBuilder.CreateIndex(
                name: "IX_Claims_PolicyId",
                table: "Claims",
                column: "PolicyId",
                unique: true);

            migrationBuilder.CreateIndex(
                name: "IX_Commissions_AgentId",
                table: "Commissions",
                column: "AgentId");

            migrationBuilder.CreateIndex(
                name: "IX_Commissions_PolicyNo",
                table: "Commissions",
                column: "PolicyNo");

            migrationBuilder.CreateIndex(
                name: "IX_Customers_AddressId",
                table: "Customers",
                column: "AddressId");

            migrationBuilder.CreateIndex(
                name: "IX_Customers_AgentId",
                table: "Customers",
                column: "AgentId");

            migrationBuilder.CreateIndex(
                name: "IX_Customers_UserId",
                table: "Customers",
                column: "UserId",
                unique: true,
                filter: "[UserId] IS NOT NULL");

            migrationBuilder.CreateIndex(
                name: "IX_CustomersQueries_CustomerId",
                table: "CustomersQueries",
                column: "CustomerId");

            migrationBuilder.CreateIndex(
                name: "IX_CustomersQueries_ResolvedByEmployeeId",
                table: "CustomersQueries",
                column: "ResolvedByEmployeeId");

            migrationBuilder.CreateIndex(
                name: "IX_Documents_CustomerId",
                table: "Documents",
                column: "CustomerId");

            migrationBuilder.CreateIndex(
                name: "IX_Documents_PolicyId",
                table: "Documents",
                column: "PolicyId");

            migrationBuilder.CreateIndex(
                name: "IX_Documents_VerifiedById",
                table: "Documents",
                column: "VerifiedById");

            migrationBuilder.CreateIndex(
                name: "IX_Employees_UserId",
                table: "Employees",
                column: "UserId",
                unique: true,
                filter: "[UserId] IS NOT NULL");

            migrationBuilder.CreateIndex(
                name: "IX_Installments_PolicyId",
                table: "Installments",
                column: "PolicyId");

            migrationBuilder.CreateIndex(
                name: "IX_InsurancePolicies_AgentId",
                table: "InsurancePolicies",
                column: "AgentId");

            migrationBuilder.CreateIndex(
                name: "IX_InsurancePolicies_CustomerId",
                table: "InsurancePolicies",
                column: "CustomerId");

            migrationBuilder.CreateIndex(
                name: "IX_InsurancePolicies_InsuranceSchemeId",
                table: "InsurancePolicies",
                column: "InsuranceSchemeId");

            migrationBuilder.CreateIndex(
                name: "IX_InsurancePolicies_TaxSettingsTaxId",
                table: "InsurancePolicies",
                column: "TaxSettingsTaxId");

            migrationBuilder.CreateIndex(
                name: "IX_InsuranceSchemes_PlanId",
                table: "InsuranceSchemes",
                column: "PlanId");

            migrationBuilder.CreateIndex(
                name: "IX_Nomines_PolicyNo",
                table: "Nomines",
                column: "PolicyNo");

            migrationBuilder.CreateIndex(
                name: "IX_Payments_PolicyId",
                table: "Payments",
                column: "PolicyId");

            migrationBuilder.CreateIndex(
                name: "IX_Users_RoleId",
                table: "Users",
                column: "RoleId");

            migrationBuilder.CreateIndex(
                name: "IX_WithdrawalRequests_AgentId",
                table: "WithdrawalRequests",
                column: "AgentId");

            migrationBuilder.CreateIndex(
                name: "IX_WithdrawalRequests_CustomerId",
                table: "WithdrawalRequests",
                column: "CustomerId");
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropTable(
                name: "Admins");

            migrationBuilder.DropTable(
                name: "AgentEarnings");

            migrationBuilder.DropTable(
                name: "Claims");

            migrationBuilder.DropTable(
                name: "Commissions");

            migrationBuilder.DropTable(
                name: "CustomersQueries");

            migrationBuilder.DropTable(
                name: "Documents");

            migrationBuilder.DropTable(
                name: "Installments");

            migrationBuilder.DropTable(
                name: "Nomines");

            migrationBuilder.DropTable(
                name: "Payments");

            migrationBuilder.DropTable(
                name: "WithdrawalRequests");

            migrationBuilder.DropTable(
                name: "Employees");

            migrationBuilder.DropTable(
                name: "InsurancePolicies");

            migrationBuilder.DropTable(
                name: "Customers");

            migrationBuilder.DropTable(
                name: "InsuranceSchemes");

            migrationBuilder.DropTable(
                name: "TaxSettings");

            migrationBuilder.DropTable(
                name: "Agents");

            migrationBuilder.DropTable(
                name: "InsurancePlans");

            migrationBuilder.DropTable(
                name: "Address");

            migrationBuilder.DropTable(
                name: "Users");

            migrationBuilder.DropTable(
                name: "Cities");

            migrationBuilder.DropTable(
                name: "Roles");

            migrationBuilder.DropTable(
                name: "States");
        }
    }
}
