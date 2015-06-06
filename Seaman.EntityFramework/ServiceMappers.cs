using System;
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

            Mapper.CreateMap<Cane, CaneModel>();
            Mapper.CreateMap<CaneModel, Cane>();

            Mapper.CreateMap<Canister, CanisterModel>();
            Mapper.CreateMap<CanisterModel, Canister>();

            Mapper.CreateMap<CollectionMethod, CollectionMethodModel>();
            Mapper.CreateMap<CollectionMethodModel, CollectionMethod>();

            Mapper.CreateMap<Comment, CommentModel>();
            Mapper.CreateMap<CommentModel, Comment>();

            Mapper.CreateMap<Location, LocationModel>();
            Mapper.CreateMap<LocationModel, Location>();

            Mapper.CreateMap<Physician, PhysicianModel>();
            Mapper.CreateMap<PhysicianModel, Physician>();

            Mapper.CreateMap<Sample, SampleModel>();
            Mapper.CreateMap<SampleModel, Sample>();

            Mapper.CreateMap<Sample, SampleBriefModel>()
                .ForMember(it => it.Locations,
                    ctx => ctx.MapFrom(s => String.Join(", ", s.Locations.Select(l => l.UniqName))))
                .ForMember(it => it.CollectionMethod, ctx => ctx.MapFrom(s => s.CollectionMethod.Name))
                .ForMember(it => it.Comment, ctx => ctx.MapFrom(s => s.Comment.Name))
                .ForMember(it => it.Physician, ctx => ctx.MapFrom(s => s.Physician.Name))
                .ForMember(it => it.DepositorFullName,
                    ctx => ctx.ResolveUsing(s => s.DepositorLastName + " " + s.DepositorFirstName));
                

            Mapper.CreateMap<Tank, TankModel>();
            Mapper.CreateMap<TankModel, Tank>();

            Mapper.CreateMap<Position, PositionModel>();
            Mapper.CreateMap<PositionModel, Position>();
        }

        public static void Check()
        {
            return;
        }

    }
}