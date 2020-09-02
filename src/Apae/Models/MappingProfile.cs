using System;
using Apae.Data;
using Apae.Extensions;
using Apae.Models.Beneficiaries;
using Apae.Models.Family;
using Apae.Models.Users;
using AutoMapper;

namespace Apae.Models
{
    public class MappingProfile : Profile
    {
        public MappingProfile()
        {
            // Admin
            CreateMap<User, UserListItem>();
            CreateMap<User, UpdateUser>();


            // Beneficiaries
            CreateMap<Beneficiary, BeneficiaryListItem>();
            CreateMap<FamilyMember, BeneficiaryListItemFamilyMember>();

            CreateMap<CreateBeneficiary, Beneficiary>()
                .ForMember(dst => dst.Dob, opts => opts.MapFrom(src => src.Dob.FromBrazilToUtc()));
            CreateMap<UpdateBeneficiary, Beneficiary>()
                .ForMember(dst => dst.Dob, opts => opts.MapFrom(src => src.Dob.FromBrazilToUtc()));

            CreateMap<Beneficiary, UpdateBeneficiary>()
                .ForMember(dst => dst.Dob, opts => opts.MapFrom(src => src.Dob.FromUtcToBrazil()));
            CreateMap<Beneficiary, BeneficiaryDetails>()
                .ForMember(dst => dst.Dob, opts => opts.MapFrom(src => src.Dob.FromUtcToBrazil()));

            CreateMap<Benefit, BeneficiaryBenefit>()
                .ForMember(t => t.RegisteredBy, opts => opts.MapFrom(s => $"{s.CreatedBy.FirstName} {s.CreatedBy.LastName}"));

            CreateMap<FamilyMember, BeneficaryFamilyMember>()
                .ForMember(dst => dst.Dob, opts => opts.MapFrom(src => src.Dob.FromUtcToBrazil()));

            // Family Members
            CreateMap<CreateFamilyMember, FamilyMember>()
                .ForMember(dst => dst.Dob, opts => opts.MapFrom(src => src.Dob.FromBrazilToUtc()));

            CreateMap<FamilyMember, UpdateFamilyMember>()
                .ForMember(dst => dst.Dob, opts => opts.MapFrom(src => src.Dob.FromUtcToBrazil()));
            CreateMap<UpdateFamilyMember, FamilyMember>()
                .ForMember(dst => dst.BeneficiaryId, opts => opts.Ignore())
                .ForMember(dst => dst.Dob, opts => opts.MapFrom(src => src.Dob.FromBrazilToUtc()));
        }
    }
}
