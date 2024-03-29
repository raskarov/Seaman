using System;
using System.Globalization;
using System.Linq;
using AutoMapper;
using Seaman.Core;
using Seaman.Core.Model;
using Seaman.EntityFramework.Entity;

namespace Seaman.EntityFramework
{
    public static class ServiceMappers
    {
        static ServiceMappers()
        {
            Mapper.CreateMap<UserModel, User>();
            Mapper.CreateMap<User, UserModel>();
            Mapper.CreateMap<User, UserCheckRecord>();
            Mapper.CreateMap<Role, RoleModel>();
            Mapper.CreateMap<RoleModel, Role>();
            
            Mapper.CreateMap<CollectionMethod, CollectionMethodModel>();
            Mapper.CreateMap<CollectionMethodModel, CollectionMethod>();
       
            Mapper.CreateMap<Location, LocationModel>()
                .ForMember(l => l.SampleId, ctx => ctx.MapFrom(s => s.Sample.Id));
            Mapper.CreateMap<LocationModel, Location>();

            Mapper.CreateMap<Location, LocationBriefModel>()
                .ForMember(it => it.CollectionMethod, ctx => ctx.MapFrom(l => l.CollectionMethod.Name))
                .ForMember(it => it.DateStored,
                    ctx =>
                        ctx.MapFrom(
                            l =>
                                l.DateStored.HasValue
                                    ? l.DateStored.Value.ToString("MM/dd/yyyy", CultureInfo.InvariantCulture)
                                    : String.Empty))
                .ForMember(it => it.DateFrozen,
                    ctx =>
                        ctx.MapFrom(
                            l =>
                                l.DateFrozen.HasValue
                                    ? l.DateFrozen.Value.ToString("MM/dd/yyyy", CultureInfo.InvariantCulture)
                                    : String.Empty))
                .ForMember(it => it.DateExtracted,
                    ctx =>
                        ctx.MapFrom(
                            l =>
                                l.DateExtracted.HasValue
                                    ? l.DateExtracted.Value.ToString("MM/dd/yyyy", CultureInfo.InvariantCulture)
                                    : String.Empty))
                .ForMember(it => it.ReasonForExtraction, ctx => ctx.MapFrom(l => l.ExtractReason.Name));
                

            Mapper.CreateMap<Location, LocationReportModel>()
                .ForMember(it => it.CollectionMethod, ctx => ctx.MapFrom(l => l.CollectionMethod.Name))
                .ForMember(it => it.DateStored,
                    ctx =>
                        ctx.MapFrom(
                            l =>
                                l.DateStored.HasValue
                                    ? l.DateStored.Value.ToString("MM/dd/yyyy", CultureInfo.InvariantCulture)
                                    : String.Empty))
                .ForMember(it => it.DateFrozen,
                    ctx =>
                        ctx.MapFrom(
                            l =>
                                l.DateFrozen.HasValue
                                    ? l.DateFrozen.Value.ToString("MM/dd/yyyy", CultureInfo.InvariantCulture)
                                    : String.Empty))
                .ForMember(it => it.DateExtracted, ctx => ctx.MapFrom(l => l.DateExtracted.HasValue ? l.DateExtracted.Value.ToString("MM/dd/yyyy", CultureInfo.InvariantCulture) : String.Empty))
                .ForMember(it => it.DepositorFullName,
                    ctx => ctx.MapFrom(l => l.Sample.DepositorFirstName + " " + l.Sample.DepositorLastName))
                .ForMember(it => it.DepositorDob,
                    ctx => ctx.MapFrom(l => l.Sample.DepositorDob.ToString("MM/dd/yyyy", CultureInfo.InvariantCulture)))
                .ForMember(it => it.Physician, ctx => ctx.MapFrom(l => l.Sample.Physician.Name))
                .ForMember(it => it.Extracted, ctx => ctx.MapFrom(l => l.Extracted ? "Yes" : "No"));


            Mapper.CreateMap<Physician, PhysicianModel>();
            Mapper.CreateMap<PhysicianModel, Physician>();

            Mapper.CreateMap<Sample, SampleModel>()
                .ForMember(it => it.Locations, ctx => ctx.MapFrom(s => s.Locations.Where(l => !l.Extracted)));
            Mapper.CreateMap<SampleModel, Sample>();


            Mapper.CreateMap<Sample, SampleBriefModel>()
                .ForMember(it => it.Physician, ctx => ctx.MapFrom(s => s.Physician.Name))
                .ForMember(it => it.DepositorDob, ctx => ctx.MapFrom(s => s.DepositorDob.ToString("MM/dd/yyyy", CultureInfo.InvariantCulture)))
                .ForMember(it => it.DepositorFullName,
                    ctx => ctx.ResolveUsing(s => s.DepositorLastName + " " + s.DepositorFirstName))
                .ForMember(it => it.Locations, ctx => ctx.MapFrom(s => s.Locations.Where(l => l.Extracted)));


            Mapper.CreateMap<Sample, SampleReportModel>()
                .ForMember(it => it.DepositorDob,
                    ctx => ctx.MapFrom(s => s.DepositorDob.ToString("MM/dd/yyyy", CultureInfo.InvariantCulture)))
                .ForMember(it => it.PartnerDob,
                    ctx =>
                        ctx.MapFrom(
                            s =>
                                s.PartnerDob.HasValue
                                    ? s.PartnerDob.Value.ToString("MM/dd/yyyy", CultureInfo.InvariantCulture)
                                    : String.Empty))
                .ForMember(it => it.DirectedDonorDob,
                    ctx =>
                        ctx.MapFrom(
                            s =>
                                s.DirectedDonorDob.HasValue
                                    ? s.DirectedDonorDob.Value.ToString("MM/dd/yyyy", CultureInfo.InvariantCulture)
                                    : String.Empty))
                .ForMember(it => it.Physician, ctx => ctx.MapFrom(s => s.Physician.Name))
                .ForMember(it => it.Autologous, ctx => ctx.MapFrom(s => s.Autologous ? "Yes" : "No"))
                .ForMember(it => it.TestingOnFile, ctx => ctx.MapFrom(s => s.TestingOnFile ? "Yes" : "No"))
                .ForMember(it => it.Refreeze, ctx => ctx.MapFrom(s => s.Refreeze ? "Yes" : "No"))
                .ForMember(it => it.DepositorFullName, ctx => ctx.ResolveUsing(s => s.DepositorLastName + " " + s.DepositorFirstName));

            Mapper.CreateMap<Tank, TankModel>();
            Mapper.CreateMap<TankModel, Tank>();

            Mapper.CreateMap<Cryobank, CryobankModel>();
            Mapper.CreateMap<CryobankModel, Cryobank>();

            Mapper.CreateMap<ExtractReasonModel, ExtractReason>();
            Mapper.CreateMap<ExtractReason, ExtractReasonModel>();
        }

        public static void Check()
        {
            return;
        }
    }
}