using InsurancePolicy.Models;
using Microsoft.EntityFrameworkCore;

namespace InsurancePolicy.Data
{
    public class AppDbContext : DbContext
    {
        public DbSet<Role> Roles { get; set; }
        public DbSet<User> Users { get; set; }
        public DbSet<Admin> Admins { get; set; }
        public DbSet<Agent> Agents { get; set; }
        public DbSet<AgentEarnings> AgentEarnings { get; set; }
        public DbSet<City> Cities { get; set; }
        public DbSet<Claim> Claims { get; set; }
        public DbSet<Commission> Commissions { get; set; }
        public DbSet<Customer> Customers { get; set; }
        public DbSet<CustomerQuery> CustomersQueries { get; set; }
        public DbSet<Document> Documents { get; set; }
        public DbSet<Employee> Employees { get; set; }
        public DbSet<Installment> Installments { get; set; }
        public DbSet<InsurancePlan> InsurancePlans { get; set; }
        public DbSet<InsuranceScheme> InsuranceSchemes { get; set; }
        //public DbSet<InsuranceSettings> InsuranceSettings { get; set; }
        public DbSet<Nominee> Nomines { get; set; }
        public DbSet<Payment> Payments { get; set; }
        public DbSet<Policy> InsurancePolicies { get; set; }
        public DbSet<State> States { get; set; }
        public DbSet<TaxSettings> TaxSettings { get; set; }
        public DbSet<WithdrawalRequest> WithdrawalRequests { get; set; }
        public AppDbContext(DbContextOptions<AppDbContext> options) : base(options)
        { }
        protected override void OnModelCreating(ModelBuilder modelBuilder)
        {
            modelBuilder.Entity<Role>(entity =>
            {
                entity.HasKey(e => e.Id);
                entity.Property(e => e.Id).HasDefaultValueSql("NEWSEQUENTIALID()");
            });
            modelBuilder.Entity<User>(entity =>
            {
                entity.HasKey(e => e.UserId);
                entity.Property(e => e.UserId).HasDefaultValueSql("NEWSEQUENTIALID()");
            });

            modelBuilder.Entity<Admin>(entity =>
            {
                entity.HasKey(e => e.AdminId);
                entity.Property(e => e.AdminId).HasDefaultValueSql("NEWSEQUENTIALID()");
            });

            modelBuilder.Entity<Agent>(entity =>
            {
                entity.HasKey(e => e.AgentId);
                entity.Property(e => e.AgentId).HasDefaultValueSql("NEWSEQUENTIALID()");
            });

            modelBuilder.Entity<AgentEarnings>(entity =>
            {
                entity.HasKey(e => e.Id);
                entity.Property(e => e.Id).HasDefaultValueSql("NEWSEQUENTIALID()");
            });
            modelBuilder.Entity<City>(entity =>
            {
                entity.HasKey(e => e.CityId);
                entity.Property(e => e.CityId).HasDefaultValueSql("NEWSEQUENTIALID()");
            });
            modelBuilder.Entity<Commission>(entity =>
            {
                entity.HasKey(e => e.CommissionId);
                entity.Property(e => e.CommissionId).HasDefaultValueSql("NEWSEQUENTIALID()");
            });

            modelBuilder.Entity<Customer>(entity =>
            {
                entity.HasKey(e => e.CustomerId);
                entity.Property(e => e.CustomerId).HasDefaultValueSql("NEWSEQUENTIALID()");
            });
            modelBuilder.Entity<CustomerQuery>(entity =>
            {
                entity.HasKey(e => e.QueryId);
                entity.Property(e => e.QueryId).HasDefaultValueSql("NEWSEQUENTIALID()");
            });
            modelBuilder.Entity<Employee>(entity =>
            {
                entity.HasKey(e => e.EmployeeId);
                entity.Property(e => e.EmployeeId).HasDefaultValueSql("NEWSEQUENTIALID()");
            });
            modelBuilder.Entity<Claim>(entity =>
            {
                entity.HasKey(e => e.ClaimId);
                entity.Property(e => e.ClaimId).HasDefaultValueSql("NEWSEQUENTIALID()");
            });
            modelBuilder.Entity<InsurancePlan>(entity =>
            {
                entity.HasKey(e => e.PlanId);
                entity.Property(e => e.PlanId).HasDefaultValueSql("NEWSEQUENTIALID()");
            });
            modelBuilder.Entity<InsuranceScheme>(entity =>
            {
                entity.HasKey(e => e.SchemeId);
                entity.Property(e => e.SchemeId).HasDefaultValueSql("NEWSEQUENTIALID()");
            });
            //modelBuilder.Entity<InsuranceSettings>(entity =>
            //{
            //    entity.HasKey(e => e.Id);
            //    entity.Property(e => e.Id).HasDefaultValueSql("NEWSEQUENTIALID()");
            //});
            modelBuilder.Entity<Nominee>(entity =>
            {
                entity.HasKey(e => e.NomineeId);
                entity.Property(e => e.NomineeId).HasDefaultValueSql("NEWSEQUENTIALID()");
            });
            modelBuilder.Entity<Payment>(entity =>
            {
                entity.HasKey(e => e.PaymentId);
                entity.Property(e => e.PaymentId).HasDefaultValueSql("NEWSEQUENTIALID()");
            });
            modelBuilder.Entity<Policy>(entity =>
            {
                entity.HasKey(e => e.PolicyId);
                entity.Property(e => e.PolicyId).HasDefaultValueSql("NEWSEQUENTIALID()");
            });
            modelBuilder.Entity<TaxSettings>(entity =>
            {
                entity.HasKey(e => e.TaxId);
                entity.Property(e => e.TaxId).HasDefaultValueSql("NEWSEQUENTIALID()");
            });
            modelBuilder.Entity<WithdrawalRequest>(entity =>
            {
                entity.HasKey(e => e.WithdrawalRequestId);
                entity.Property(e => e.WithdrawalRequestId).HasDefaultValueSql("NEWSEQUENTIALID()");
            });



            var adminRoleId = Guid.NewGuid();
            var agentRoleId = Guid.NewGuid();
            var employeeId = Guid.NewGuid();
            var customerId = Guid.NewGuid();
            modelBuilder.Entity<Role>().HasData(
                new Role { Id = adminRoleId, Name = "Admin" },
                new Role { Id = agentRoleId, Name = "Agent" },
                new Role { Id = employeeId, Name = "Employee" },
                new Role { Id = customerId, Name = "Customer" }
                );

            var states = new List<State>
        {
        new State { StateId = Guid.NewGuid(), StateName = "Andhra Pradesh" },
        new State { StateId = Guid.NewGuid(), StateName = "Arunachal Pradesh" },
        new State { StateId = Guid.NewGuid(), StateName = "Assam" },
        new State { StateId = Guid.NewGuid(), StateName = "Bihar" },
        new State { StateId = Guid.NewGuid(), StateName = "Chhattisgarh" },
        new State { StateId = Guid.NewGuid(), StateName = "Goa" },
        new State { StateId = Guid.NewGuid(), StateName = "Gujarat" },
        new State { StateId = Guid.NewGuid(), StateName = "Haryana" },
        new State { StateId = Guid.NewGuid(), StateName = "Himachal Pradesh" },
        new State { StateId = Guid.NewGuid(), StateName = "Jharkhand" },
        new State { StateId = Guid.NewGuid(), StateName = "Karnataka" },
        new State { StateId = Guid.NewGuid(), StateName = "Kerala" },
        new State { StateId = Guid.NewGuid(), StateName = "Madhya Pradesh" },
        new State { StateId = Guid.NewGuid(), StateName = "Maharashtra" },
        new State { StateId = Guid.NewGuid(), StateName = "Manipur" },
        new State { StateId = Guid.NewGuid(), StateName = "Meghalaya" },
        new State { StateId = Guid.NewGuid(), StateName = "Mizoram" },
        new State { StateId = Guid.NewGuid(), StateName = "Nagaland" },
        new State { StateId = Guid.NewGuid(), StateName = "Odisha" },
        new State { StateId = Guid.NewGuid(), StateName = "Punjab" },
        new State { StateId = Guid.NewGuid(), StateName = "Rajasthan" },
        new State { StateId = Guid.NewGuid(), StateName = "Sikkim" },
        new State { StateId = Guid.NewGuid(), StateName = "Tamil Nadu" },
        new State { StateId = Guid.NewGuid(), StateName = "Telangana" },
        new State { StateId = Guid.NewGuid(), StateName = "Tripura" },
        new State { StateId = Guid.NewGuid(), StateName = "Uttar Pradesh" },
        new State { StateId = Guid.NewGuid(), StateName = "Uttarakhand" },
        new State { StateId = Guid.NewGuid(), StateName = "West Bengal" }
        };

            modelBuilder.Entity<State>().HasData(states);
            var stateCityMap = new List<(string StateName, string[] Cities)>
    {
        ("Andhra Pradesh", new[] { "Visakhapatnam", "Vijayawada", "Guntur", "Nellore", "Tirupati", "Rajahmundry", "Kakinada", "Anantapur", "Kadapa", "Chittoor" }),
        ("Arunachal Pradesh", new[] { "Itanagar", "Tawang", "Ziro", "Pasighat", "Bomdila", "Along", "Roing", "Tezu", "Anini", "Changlang" }),
        ("Assam", new[] { "Guwahati", "Silchar", "Dibrugarh", "Jorhat", "Tezpur", "Tinsukia", "Nagaon", "Barpeta", "Goalpara", "Bongaigaon" }),
        ("Bihar", new[] { "Patna", "Gaya", "Bhagalpur", "Muzaffarpur", "Darbhanga", "Purnia", "Hajipur", "Ara", "Sasaram", "Begusarai" }),
        ("Chhattisgarh", new[] { "Raipur", "Bilaspur", "Korba", "Durg", "Bhilai", "Jagdalpur", "Rajnandgaon", "Raigarh", "Ambikapur", "Mahasamund" }),
        ("Goa", new[] { "Panaji", "Margao", "Vasco da Gama", "Mapusa", "Ponda", "Calangute", "Bicholim", "Porvorim", "Canacona", "Dona Paula" }),
        ("Gujarat", new[] { "Ahmedabad", "Surat", "Vadodara", "Rajkot", "Bhavnagar", "Jamnagar", "Gandhinagar", "Anand", "Navsari", "Mehsana" }),
        ("Haryana", new[] { "Gurgaon", "Faridabad", "Panipat", "Ambala", "Hisar", "Rohtak", "Karnal", "Yamunanagar", "Sirsa", "Bhiwani" }),
        ("Himachal Pradesh", new[] { "Shimla", "Manali", "Dharamshala", "Kullu", "Solan", "Mandi", "Chamba", "Bilaspur", "Hamirpur", "Nahan" }),
        ("Jharkhand", new[] { "Ranchi", "Jamshedpur", "Dhanbad", "Bokaro", "Hazaribagh", "Deoghar", "Giridih", "Ramgarh", "Godda", "Pakur" }),
        ("Karnataka", new[] { "Bangalore", "Mysore", "Mangalore", "Hubli", "Belgaum", "Davanagere", "Bellary", "Bijapur", "Shimoga", "Udupi" }),
        ("Kerala", new[] { "Thiruvananthapuram", "Kochi", "Kozhikode", "Thrissur", "Kollam", "Alappuzha", "Kannur", "Palakkad", "Malappuram", "Idukki" }),
        ("Madhya Pradesh", new[] { "Bhopal", "Indore", "Gwalior", "Jabalpur", "Ujjain", "Sagar", "Rewa", "Ratlam", "Satna", "Chhindwara" }),
        ("Maharashtra", new[] { "Mumbai", "Pune", "Nagpur", "Nashik", "Thane", "Aurangabad", "Solapur", "Kolhapur", "Amravati", "Sangli" }),
        ("Rajasthan", new[] { "Jaipur", "Jodhpur", "Udaipur", "Kota", "Ajmer", "Bikaner", "Alwar", "Bharatpur", "Jaisalmer", "Sikar" }),
        ("Tamil Nadu", new[] { "Chennai", "Coimbatore", "Madurai", "Tiruchirappalli", "Salem", "Tirunelveli", "Erode", "Vellore", "Thoothukudi", "Dindigul" }),
        ("Telangana", new[] { "Hyderabad", "Warangal", "Nizamabad", "Karimnagar", "Khammam", "Mahbubnagar", "Ramagundam", "Adilabad", "Siddipet", "Mancherial" }),
        ("Uttar Pradesh", new[] { "Lucknow", "Kanpur", "Varanasi", "Agra", "Meerut", "Allahabad", "Ghaziabad", "Bareilly", "Aligarh", "Moradabad" }),
        ("West Bengal", new[] { "Kolkata", "Howrah", "Darjeeling", "Siliguri", "Durgapur", "Asansol", "Malda", "Bardhaman", "Jalpaiguri", "Haldia" })
    };

            foreach (var stateCity in stateCityMap)
            {
                var stateId = states.First(s => s.StateName == stateCity.StateName).StateId;
                foreach (var city in stateCity.Cities)
                {
                    modelBuilder.Entity<City>().HasData(new City
                    {
                        CityId = Guid.NewGuid(),
                        CityName = city,
                        StateId = stateId
                    });
                }
            }
        }
    }

}
