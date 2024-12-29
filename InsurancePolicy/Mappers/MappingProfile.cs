using AutoMapper;
using InsurancePolicy.DTOs;
using InsurancePolicy.enums;
using InsurancePolicy.Models;

namespace InsurancePolicy.Mappers
{
    public class MappingProfile : Profile
    {
        public MappingProfile()
        {
            CreateMap<AdminRequestDto, Admin>()
    .ForMember(dest => dest.User, opt => opt.MapFrom((src, dest) =>
    {
        if (dest.User == null)
        {
            // Create a new User if none exists (e.g., during Add operation)
            return new User
            {
                UserName = src.UserName,
                Password = src.Password, // Leave encryption to service layer
                RoleId = Guid.Empty // Assign RoleId in service
            };
        }

        // Update existing User (e.g., during Update operation)
        dest.User.UserName = src.UserName;
        if (!string.IsNullOrEmpty(src.Password))
        {
            dest.User.Password = src.Password; // Leave encryption to service layer
        }
        return dest.User;
    }))
    .ForMember(dest => dest.AdminId, opt => opt.Condition(src => src.AdminId.HasValue)); // Map AdminId only if provided

            // Mapping from Admin to AdminResponseDto
            CreateMap<Admin, AdminResponseDto>()
                .ForMember(dest => dest.UserName, opt => opt.MapFrom(src => src.User.UserName)); // Map UserName

            // Mapping from AgentRequestDto to Agent
            CreateMap<AgentRequestDto, Agent>()
        .ForMember(dest => dest.User, opt => opt.MapFrom((src, dest) =>
        {
            if (dest.User == null)
            {
                // Create a new User for Add operations
                return new User
                {
                    UserName = src.UserName,
                    Password = src.Password, // Encryption handled in the service layer
                    RoleId = Guid.Empty // Assign RoleId in the service layer
                };
            }

            // Update existing User for Update operations
            dest.User.UserName = src.UserName;
            if (!string.IsNullOrEmpty(src.Password))
            {
                dest.User.Password = src.Password; // Encryption handled in the service layer
            }
            return dest.User;
        }))
        .ForMember(dest => dest.AgentId, opt => opt.Condition(src => src.AgentId.HasValue)); // Map AgentId only if provided
            CreateMap<Agent, AgentRequestDto>();
           
   
            CreateMap<Agent, AgentResponseDto>()
    .ForMember(dest => dest.UserName, opt => opt.MapFrom(src => src.User != null ? src.User.UserName : null))
    .ForMember(dest => dest.TotalCustomers, opt => opt.MapFrom(src => src.Customers != null ? src.Customers.Count : 0))
    .ForMember(dest => dest.TotalPolicies, opt => opt.MapFrom(src =>
        src.Customers != null ? src.Customers.Sum(c => c.Policies != null ? c.Policies.Count : 0) : 0));

            CreateMap<CustomerRequestDto, Customer>()
        .ForMember(dest => dest.User, opt => opt.MapFrom((src, dest) =>
        {
            if (dest.User == null)
            {
                // Create a new User for Add operations
                return new User
                {
                    UserName = src.UserName,
                    Password = src.Password, // Encrypt in the service layer
                    RoleId = Guid.Empty // Assign RoleId in the service layer
                };
            }

            // Update existing User for Update operations
            dest.User.UserName = src.UserName;
            if (!string.IsNullOrEmpty(src.Password))
            {
                dest.User.Password = src.Password; // Encrypt in the service layer
            }

            return dest.User;
        }))
        .ForMember(dest => dest.Address, opt => opt.MapFrom((src, dest) =>
        {
            if (dest.Address == null)
            {
                return new Address
                {
                    HouseNo = src.HouseNo, // Map individual address fields
                    Apartment = src.Apartment,
                    Pincode = src.Pincode,
                    City = new City
                    {
                        CityId = Guid.NewGuid(), // Generate new City ID for Add operations
                        CityName = src.City,
                        State = new State
                        {
                            StateId = Guid.NewGuid(), // Generate new State ID for Add operations
                            StateName = src.State
                        }
                    }
                };
            }

            // Update existing address details
            dest.Address.HouseNo = src.HouseNo;
            dest.Address.Apartment = src.Apartment;
            dest.Address.Pincode = src.Pincode;

            if (dest.Address.City != null)
            {
                dest.Address.City.CityName = src.City;
                if (dest.Address.City.State != null)
                {
                    dest.Address.City.State.StateName = src.State;
                }
            }

            return dest.Address;
        }))
        .ForMember(dest => dest.AgentId, opt => opt.Condition(src => src.AgentId.HasValue)) // Allow null AgentId
        .ForMember(dest => dest.CustomerId, opt => opt.Condition(src => src.CustomerId.HasValue)); // Map CustomerId only if provided

            CreateMap<Customer, CustomerResponseDto>()
                .ForMember(dest => dest.UserName, opt => opt.MapFrom(src => src.User.UserName)) // Map UserName
                .ForMember(dest => dest.TotalPolicies, opt => opt.MapFrom(src => src.Policies != null ? src.Policies.Count : 0)) // Map TotalPolicies
                .ForMember(dest => dest.AgentName, opt => opt.MapFrom(src => src.Agent != null
                    ? $"{src.Agent.AgentFirstName} {src.Agent.AgentLastName}" // Combine Agent's first and last name
                    : null)) // If no Agent, set null
                .ForMember(dest => dest.HouseNo, opt => opt.MapFrom(src => src.Address != null ? src.Address.HouseNo : null))
                .ForMember(dest => dest.Apartment, opt => opt.MapFrom(src => src.Address != null ? src.Address.Apartment : null))
                .ForMember(dest => dest.Pincode, opt => opt.MapFrom(src => src.Address != null ? src.Address.Pincode : 0))
                .ForMember(dest => dest.City, opt => opt.MapFrom(src => src.Address != null && src.Address.City != null ? src.Address.City.CityName : null))
                .ForMember(dest => dest.State, opt => opt.MapFrom(src => src.Address != null && src.Address.City != null && src.Address.City.State != null
                    ? src.Address.City.State.StateName
                    : null));

            CreateMap<EmployeeRequestDto, Employee>()
        .ForMember(dest => dest.User, opt => opt.MapFrom((src, dest) =>
        {
            if (dest.User == null)
            {
                // Create a new User for Add operations
                return new User
                {
                    UserName = src.UserName,
                    Password = src.Password, // Encrypt in the service layer
                    RoleId = Guid.Empty // Assign RoleId in the service layer
                };
            }

            // For updates, update the existing User
            dest.User.UserName = src.UserName;
            if (!string.IsNullOrEmpty(src.Password))
            {
                dest.User.Password = src.Password; // Encrypt in the service layer
            }

            return dest.User;
        }))
        .ForMember(dest => dest.EmployeeId, opt => opt.Condition(src => src.EmployeeId.HasValue)); // Map EmployeeId only if provided
            CreateMap<Employee, EmployeeResponseDto>()
                .ForMember(dest => dest.UserName, opt => opt.MapFrom(src => src.User.UserName));


            CreateMap<InsurancePlanRequestDto, InsurancePlan>()
    .ForMember(dest => dest.PlanId, opt => opt.Condition(src => src.PlanId.HasValue)) // Map PlanId only if provided
    .ForMember(dest => dest.Status, opt => opt.MapFrom(src => src.Status)) // Map Status directly
    .ForMember(dest => dest.Schemes, opt => opt.Ignore()); // Ignore Schemes during mapping from request DTO

            CreateMap<InsurancePlan, InsurancePlanResponseDto>()
                .ForMember(dest => dest.SchemeNames, opt => opt.MapFrom(src => src.Schemes != null
                    ? src.Schemes.Select(s => s.SchemeName).ToList()
                    : null)) // Map scheme names
                .ForMember(dest => dest.TotalSchemes, opt => opt.MapFrom(src => src.Schemes != null
                    ? src.Schemes.Count
                    : 0)); // Calculate the total number of schemes



            CreateMap<InsuranceSchemeRequestDto, InsuranceScheme>()
    .ForMember(dest => dest.SchemeId, opt => opt.Condition(src => src.SchemeId.HasValue));

            CreateMap<InsuranceScheme, InsuranceSchemeResponseDto>()
                .ForMember(dest => dest.PlanName, opt => opt.MapFrom(src => src.InsurancePlan != null ? src.InsurancePlan.PlanName : null))
                .ForMember(dest => dest.PoliciesCount, opt => opt.MapFrom(src => src.Policies != null ? src.Policies.Count : 0));

            CreateMap<TaxSettingsRequestDto, TaxSettings>()
                .ForMember(dest => dest.TaxId, opt => opt.Condition(src => src.TaxId != Guid.Empty)); // Map TaxId only if it exists

            CreateMap<TaxSettings, TaxSettingsResponseDto>();
            CreateMap<NomineeRequestDto, Nominee>()
           .ForMember(dest => dest.NomineeId, opt => opt.Condition(src => src.NomineeId.HasValue))
           .ForMember(dest => dest.Relationship, opt => opt.MapFrom(src => src.Relationship.ToString()));

            CreateMap<Nominee, NomineeResponseDto>()
                .ForMember(dest => dest.Relationship, opt => opt.MapFrom(src => src.Relationship.ToString()));

            CreateMap<InstallmentRequestDto, Installment>()
            .ForMember(dest => dest.InstallmentId, opt => opt.Condition(src => src.InstallmentId.HasValue));

            CreateMap<Installment, InstallmentResponseDto>()
                .ForMember(dest => dest.PolicyName, opt => opt.MapFrom(src => src.InsurancePolicy != null ? src.InsurancePolicy.InsuranceScheme.SchemeName : "N/A"));
            CreateMap<PaymentRequestDto, Payment>();
            CreateMap<Payment, PaymentResponseDto>()
                .ForMember(dest => dest.PolicyName, opt => opt.MapFrom(src => src.Policy.InsuranceScheme.SchemeName));

            // Commission mappings
            CreateMap<CommissionRequestDto, Commission>();

            CreateMap<Commission, CommissionResponseDto>()
                .ForMember(dest => dest.CommissionType, opt => opt.MapFrom(src => src.CommissionType.ToString())) // Map enum to string
                .ForMember(dest => dest.AgentName, opt => opt.MapFrom(src => src.Agent != null ? src.Agent.AgentFirstName : null))
                .ForMember(dest => dest.PolicyName, opt => opt.MapFrom(src => src.PolicyAccount != null ? src.PolicyAccount.InsuranceScheme.SchemeName : null));

            // Claim mappings
            CreateMap<ClaimRequestDto, Claim>();
            CreateMap<Claim, ClaimResponseDto>()
                .ForMember(dest => dest.Status, opt => opt.MapFrom(src => src.Status.ToString()))
                .ForMember(dest => dest.PolicyName, opt => opt.MapFrom(src => src.Policy != null ? src.Policy.InsuranceScheme.SchemeName : null))
                .ForMember(dest => dest.CustomerName, opt => opt.MapFrom(src => src.Customer != null ? src.Customer.CustomerFirstName : null));

            //// PolicyRequestDto to Policy
            //// Map InsuranceSettingsRequestDto to InsuranceSettings
            //CreateMap<InsuranceSettingsRequestDto, InsuranceSettings>()
            //    .ForMember(dest => dest.Id, opt => opt.Ignore()); // Ignore Id since it is generated by the database

            //// Map InsuranceSettings to InsuranceSettingsResponseDto
            //CreateMap<InsuranceSettings, InsuranceSettingsResponseDto>();

            CreateMap<InstallmentRequestDto, Installment>()
  .ForMember(dest => dest.InstallmentId, opt => opt.Condition(src => src.InstallmentId.HasValue));

            CreateMap<Installment, InstallmentResponseDto>()
                .ForMember(dest => dest.PolicyName, opt => opt.MapFrom(src => src.InsurancePolicy != null ? src.InsurancePolicy.InsuranceScheme.SchemeName : "N/A"));
            CreateMap<PaymentRequestDto, Payment>().ForMember(dest=>dest.PolicyId, opt => opt.MapFrom(src => src.PolicyId));
            CreateMap<Payment, PaymentResponseDto>()
                .ForMember(dest => dest.PolicyName, opt => opt.MapFrom(src => src.Policy.InsuranceScheme.SchemeName));

            CreateMap<CommissionRequestDto, Commission>()
                .ForMember(dest => dest.CommissionId, opt => opt.Condition(src => src.CommissionId.HasValue))
                .ForMember(dest => dest.PolicyNo, opt => opt.Condition(src => src.PolicyNo.HasValue));


            CreateMap<Commission, CommissionResponseDto>()
                .ForMember(dest => dest.CommissionType, opt => opt.MapFrom(src => src.CommissionType.ToString())) // Map enum to string
                .ForMember(dest => dest.AgentName, opt => opt.MapFrom(src => src.Agent != null ? src.Agent.AgentFirstName : null))
                .ForMember(dest => dest.PolicyName, opt => opt.MapFrom(src => src.PolicyAccount != null ? src.PolicyAccount.InsuranceScheme.SchemeName : null));

            // Claim mappings
            CreateMap<ClaimRequestDto, Claim>();
            CreateMap<Claim, ClaimResponseDto>()
                .ForMember(dest => dest.Status, opt => opt.MapFrom(src => src.Status.ToString()))
                .ForMember(dest => dest.PolicyName, opt => opt.MapFrom(src => src.Policy != null ? src.Policy.InsuranceScheme.SchemeName : null))
                .ForMember(dest => dest.CustomerName, opt => opt.MapFrom(src => src.Customer != null ? src.Customer.CustomerFirstName : null));

            // Map PolicyRequestDto to Policy
            // Map PolicyRequestDto to Policy
            CreateMap<PolicyRequestDto, Policy>()
                .ForMember(dest => dest.PolicyId, opt => opt.Ignore()) // Set by the database
                .ForMember(dest => dest.Installments, opt => opt.Ignore()) // Handled separately
                .ForMember(dest => dest.Nominees, opt => opt.Ignore()) // Handled separately
                .ForMember(dest => dest.Payments, opt => opt.Ignore()) // Handled separately
                .ForMember(dest => dest.TaxSettings, opt => opt.Ignore()) // Handled in service
                /*.ForMember(dest => dest.InsuranceSettings, opt => opt.Ignore())*/; // Handled in service

            // Map InstallmentRequestDto to Installment
            CreateMap<InstallmentRequestDto, Installment>()
                .ForMember(dest => dest.InstallmentId, opt => opt.Condition(src => src.InstallmentId.HasValue)) // Map InstallmentId only if provided
                .ForMember(dest => dest.PolicyId, opt => opt.MapFrom(src => src.PolicyId)); // Map PolicyNo from PolicyId



            // Map Policy to PolicyResponseDto
            CreateMap<Policy, PolicyResponseDto>()
                .ForMember(dest => dest.InsuranceSchemeName, opt => opt.MapFrom(src => src.InsuranceScheme != null ? src.InsuranceScheme.SchemeName : null)) // Handle null InsuranceScheme
                .ForMember(dest => dest.CustomerName, opt => opt.MapFrom(src => src.Customer != null ? src.Customer.CustomerFirstName : null)) // Handle null Customer
                .ForMember(dest => dest.PolicyStatus, opt => opt.MapFrom(src => src.Status.ToString())) // Convert enum to string
                .ForMember(dest => dest.AgentName, opt => opt.MapFrom(src => src.Agent != null ? src.Agent.AgentFirstName : null)) // Handle null Agent
                .ForMember(dest => dest.Nominees, opt => opt.MapFrom(src => src.Nominees)) // Map Nominees
                .ForMember(dest => dest.Installments, opt => opt.MapFrom(src => src.Installments)) // Map Installments
                .ForMember(dest => dest.Payments, opt => opt.MapFrom(src => src.Payments)); // Map Payments

            // Map NomineeRequestDto to Nominee
            CreateMap<NomineeRequestDto, Nominee>()
                .ForMember(dest => dest.PolicyNo, opt => opt.Ignore()) // PolicyNo will be set in service
                .ForMember(dest => dest.NomineeId, opt => opt.Condition(src => src.NomineeId.HasValue)) // Map NomineeId only if provided
                .ForMember(dest => dest.Relationship, opt => opt.MapFrom(src => src.Relationship)); // Map enum directly

            // Map Nominee to NomineeResponseDto
            CreateMap<Nominee, NomineeResponseDto>()
                .ForMember(dest => dest.Relationship, opt => opt.MapFrom(src => src.Relationship.ToString())); // Convert enum to string

            // Map InstallmentRequestDto to Installment
            CreateMap<InstallmentRequestDto, Installment>()
                .ForMember(dest => dest.InstallmentId, opt => opt.Condition(src => src.InstallmentId.HasValue)) // Map InstallmentId only if provided
                .ForMember(dest => dest.PolicyId, opt => opt.Ignore()); // PolicyNo will be set in service

            // Map Installment to InstallmentResponseDto
            CreateMap<Installment, InstallmentResponseDto>()
                .ForMember(dest => dest.PolicyName, opt => opt.MapFrom(src => src.InsurancePolicy != null ? src.InsurancePolicy.InsuranceScheme.SchemeName : null)); // Handle null InsurancePolicy

            CreateMap<Payment, PaymentResponseDto>()
                .ForMember(dest => dest.PolicyName, opt => opt.MapFrom(src => src.Policy != null ? src.Policy.InsuranceScheme.SchemeName : null)); // Handle null Policy

            //Map WithdrawalRequestDto to WithdrawalRequest
        CreateMap<WithdrawalRequestDto, WithdrawalRequest>()
            .ForMember(dest => dest.WithdrawalRequestId, opt => opt.Condition(src => src.WithdrawalRequestId.HasValue)) // Map only if provided
            .ForMember(dest => dest.Agent, opt => opt.Ignore()) // Handled in service layer
            .ForMember(dest => dest.Customer, opt => opt.Ignore()) // Handled in service layer
            .ForMember(dest => dest.TotalCommission, opt => opt.Ignore()); // Calculate dynamically

            // Map WithdrawalRequest to WithdrawalRequestResponseDto
            CreateMap<WithdrawalRequest, WithdrawalRequestResponseDto>()
    .ForMember(dest => dest.AgentName,
        opt => opt.MapFrom(src => src.Agent != null ? $"{src.Agent.AgentFirstName} {src.Agent.AgentLastName}" : null))
    .ForMember(dest => dest.CustomerName,
        opt => opt.MapFrom(src => src.Customer != null ? $"{src.Customer.CustomerFirstName} {src.Customer.CustomerLastName}" : null))
    .ForMember(dest => dest.TotalCustomers,
        opt => opt.MapFrom(src => src.Agent != null ?
            (src.Agent.Customers != null ? src.Agent.Customers.Count : 0) : 0))
    .ForMember(dest => dest.TotalPolicies,
        opt => opt.MapFrom(src => src.Agent != null ?
            (src.Agent.Customers != null ?
                src.Agent.Customers.Sum(c => c.Policies != null ? c.Policies.Count : 0)
                : 0)
            : 0))
    .ForMember(dest => dest.TotalCommission,
        opt => opt.MapFrom(src => src.Agent != null ? src.Agent.CommissionEarned : 0));

            // Map CustomerQueryRequestDto to CustomerQuery and vice versa
            CreateMap<CustomerQueryRequestDto, CustomerQuery>()
            .ForMember(dest => dest.QueryId, opt => opt.MapFrom(src => src.QueryId ?? Guid.NewGuid()))
            .ForMember(dest => dest.QueryType, opt => opt.MapFrom(src => src.Type))
            .ForMember(dest => dest.PolicyId, opt => opt.MapFrom(src => src.PolicyId))
            .ForMember(dest => dest.CreatedAt, opt => opt.MapFrom(src => DateTime.Now));

            CreateMap<CustomerQuery, CustomerQueryResponseDto>()
     .ForMember(dest => dest.ResolvedBy, opt => opt.MapFrom(src => src.ResolvedBy != null ? $"{src.ResolvedBy.EmployeeFirstName} {src.ResolvedBy.EmployeeLastName}" : null))
     .ForMember(dest => dest.CustomerName, opt => opt.MapFrom(src => src.Customer != null ? $"{src.Customer.CustomerFirstName} {src.Customer.CustomerLastName}" : null));

            CreateMap<Document, DocumentResponseDto>()
            .ForMember(dest => dest.CustomerName, opt => opt.MapFrom(src => src.Customer != null ? src.Customer.CustomerFirstName + " " + src.Customer.CustomerLastName : null))
            .ForMember(dest => dest.VerifiedByName, opt => opt.MapFrom(src => src.VerifiedBy != null ? src.VerifiedBy.EmployeeFirstName + " " + src.VerifiedBy.EmployeeLastName : null));
            CreateMap<DocumentRequestDto, Document>()
    .ForMember(dest => dest.DocumentId, opt => opt.MapFrom(src => Guid.NewGuid().ToString()));


        }
    }
}
