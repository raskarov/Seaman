using System;
using System.Collections.Generic;
using System.Data.Entity.Migrations;
using System.Linq;
using System.Security.Cryptography.X509Certificates;
using System.Text;
using System.Threading.Tasks;
using AutoMapper;
using Seaman.Core;
using Seaman.Core.Model;
using Seaman.EntityFramework.Entity;

namespace Seaman.EntityFramework
{
    public class SampleManager : SampleManagerBase
    {
        private readonly SeamanDbContext _context;

        public SampleManager(SeamanDbContext context)
        {
            _context = context;
        }

        public override SampleModel SaveSample(SaveSampleModel model, Int32? byUserId)
        {
            Sample sample;
            if (model.Id == 0)
            {
                sample = _context.CreateAndAdd<Sample>();
                sample.CreatedByUserId = byUserId;
            }
            else
            {
                sample = _context.Samples.Get(model.Id, "sample not found");
            }

            sample.ModifiedByUserId = byUserId;
            sample.PhysicianId = model.PhysicianId;
            sample.CollectionMethodId = model.CollectionMethodId;
            sample.CommentId = model.CommentId;

            sample.AnonymousDonor = model.AnonymousDonor;
            sample.AnonymousDonorId = model.AnonymousDonorId;

            sample.CryobankPurchased = model.CryobankPurchased;
            sample.CryobankName = model.CryobankName;
            sample.CryobankVialId = model.CryobankVialId;

            sample.DirectedDonor = model.DirectedDonor;
            sample.DirectedDonorFirstName = model.DirectedDonorFirstName;
            sample.DirectedDonorLastName = model.DirectedDonorLastName;
            sample.DirectedDonorId = model.DirectedDonorId;

            sample.DepositorFirstName = model.DepositorFirstName;
            sample.DepositorLastName = model.DepositorLastName;
            sample.DepositorDob = model.DepositorDob;
            sample.DepositorSsn = model.DepositorSsn;

            sample.PartnerFirstName = model.PartnerFirstName;
            sample.PartnerLastName = model.PartnerLastName;
            sample.PartnerDob = model.PartnerDob;
            sample.PartnerSsn = model.PartnerSsn;

            sample.Autologous = model.Autologous;
            sample.TestingOnFile = model.TestingOnFile;
            sample.Refreeze = model.Refreeze;

            foreach (var attachedLocation in model.Locations)
            {
                if (sample.Locations.Any(it => it.Id == attachedLocation.LocationId))
                    continue;
                var location = _context.Locations.Get(attachedLocation.LocationId, "location not found");
                var tank = _context.Tanks.Get(attachedLocation.TankId, "tank not found");
                var canister = _context.Canisters.Get(attachedLocation.CanisterId, "canister not found");
                var cane = _context.Canes.Get(attachedLocation.CaneId, "cane not found");

                location.UniqName = tank.Name + canister.Name + cane.Name + cane.Color + location.Name;
                sample.Locations.Add(location);
            }
            _context.SaveChanges();
            return Mapper.Map<SampleModel>(sample);
        }

        public override SampleModel GetSample(int id)
        {
            throw new NotImplementedException();
        }

        public override SampleModel GetSample(String uniqLocatonName)
        {
            throw new NotImplementedException();
        }

        public override PagedResult<SampleModel> GetSamples()
        {
            throw new NotImplementedException();
        }

        public override PagedResult<SampleModel> GetSamplesByTank(int tankId)
        {
            throw new NotImplementedException();
        }

        public override PagedResult<SampleModel> GetSamplesByDoctor(int doctorId)
        {
            throw new NotImplementedException();
        }

        public override void DeleteSample(int id)
        {
            var sample = _context.Samples.FindLocalOrRemote(x => x.Id == id);
            foreach (var location in sample.Locations)
            {
                location.Available = true;
                _context.Locations.AddOrUpdate(location);
            }
            _context.Samples.Remove(sample);
            _context.SaveChanges();
        }

        public override void DeleteSamples(List<int> ids)
        {
            foreach (var id in ids)
            {
                var sample = _context.Samples.FindLocalOrRemote(x => x.Id == id);
                foreach (var location in sample.Locations)
                {
                    location.Available = true;
                    _context.Locations.AddOrUpdate(location);
                }
                _context.Samples.Remove(sample);
            }
            _context.SaveChanges();
        }

        public override List<CaneModel> GetCanes()
        {
            return Mapper.Map<List<CaneModel>>(_context.Canes);
        }

        public override CaneModel SaveCane(CaneModel cane)
        {
            var exist = cane.Id == 0 ? _context.CreateAndAdd<Cane>() : _context.Canes.Get(cane.Id, "Cane not found");
            exist.Name = cane.Name;
            exist.Color = cane.Color;
            _context.SaveChanges();
            return Mapper.Map<CaneModel>(exist);
        }

        public override void DeleteCane(int id)
        {
            var cane = _context.Canes.Get(id, "Cane not found");
            _context.Canes.Remove(cane);
            _context.SaveChanges();
        }

        public override List<CanisterModel> GetCanisters()
        {
            return Mapper.Map<List<CanisterModel>>(_context.Canisters);
        }

        public override CanisterModel SaveCanister(CanisterModel canister)
        {
            var exist = canister.Id == 0 ? _context.CreateAndAdd<Canister>() : _context.Canisters.Get(canister.Id, "Canister not found");
            exist.Name = canister.Name;
            _context.SaveChanges();
            return Mapper.Map<CanisterModel>(exist);
        }

        public override void DeleteCanister(int id)
        {
            var canister = _context.Canisters.Get(id, "Cane not found");
            _context.Canisters.Remove(canister);
            _context.SaveChanges();
        }

        public override List<CollectionMethodModel> GetCollectionMethods()
        {
            return Mapper.Map<List<CollectionMethodModel>>(_context.CollectionMethods);
        }

        public override CollectionMethodModel SaveCollectionMethod(CollectionMethodModel collectionMethod)
        {
            var exist = collectionMethod.Id == 0 ? _context.CreateAndAdd<CollectionMethod>() : _context.CollectionMethods.Get(collectionMethod.Id, "Collection method not found");
            exist.Name = collectionMethod.Name;
            _context.SaveChanges();
            return Mapper.Map<CollectionMethodModel>(exist);
        }

        public override void DeleteCollectionMethod(int id)
        {
            var collectionMethod = _context.CollectionMethods.Get(id, "Collection method not found");
            _context.CollectionMethods.Remove(collectionMethod);
            _context.SaveChanges();
        }

        public override List<CommentModel> GetComments()
        {
            return Mapper.Map<List<CommentModel>>(_context.Comments);
        }

        public override CommentModel SaveComment(CommentModel comment)
        {
            var exist = comment.Id == 0 ? _context.CreateAndAdd<Comment>() : _context.Comments.Get(comment.Id, "Comment not found");
            exist.Name = comment.Name;
            _context.SaveChanges();
            return Mapper.Map<CommentModel>(exist);
        }

        public override void DeleteComment(int id)
        {
            var comment = _context.Comments.Get(id, "Comment not found");
            _context.Comments.Remove(comment);
            _context.SaveChanges();
        }

        public override List<LocationModel> GetLocations()
        {
            return Mapper.Map<List<LocationModel>>(_context.Locations);
        }

        public override LocationModel SaveLocation(LocationModel location)
        {
            var exist = location.Id == 0 ? _context.CreateAndAdd<Location>() : _context.Locations.Get(location.Id, "Location not found");
            exist.Name = location.Name;
            _context.SaveChanges();
            return Mapper.Map<LocationModel>(exist);
        }

        public override void DeleteLocation(int id)
        {
            var location = _context.Locations.Get(id, "Location not found");
            _context.Locations.Remove(location);
            _context.SaveChanges();
        }

        public override List<PhysicianModel> GetPhysicians()
        {
            return Mapper.Map<List<PhysicianModel>>(_context.Physicians);
        }

        public override PhysicianModel SavePhysician(PhysicianModel physician)
        {
            var exist = physician.Id == 0 ? _context.CreateAndAdd<Physician>() : _context.Physicians.Get(physician.Id, "Location not found");
            exist.Name = physician.Name;
            _context.SaveChanges();
            return Mapper.Map<PhysicianModel>(exist);
        }

        public override void DeletePhysician(int id)
        {
            var physician = _context.Physicians.Get(id, "Physician not found");
            _context.Physicians.Remove(physician);
            _context.SaveChanges();
        }

        public override List<TankModel> GetTanks()
        {
            return Mapper.Map<List<TankModel>>(_context.Tanks);
        }

        public override TankModel SaveTank(TankModel tank)
        {
            var exist = tank.Id == 0 ? _context.CreateAndAdd<Tank>() : _context.Tanks.Get(tank.Id, "Location not found");
            exist.Name = tank.Name;
            _context.SaveChanges();
            return Mapper.Map<TankModel>(exist);
        }

        public override void DeleteTank(int id)
        {
            var tank = _context.Tanks.Get(id, "Tank not found");
            _context.Tanks.Remove(tank);
            _context.SaveChanges();
        }

        public override List<PositionModel> GetPositions()
        {
            return Mapper.Map<List<PositionModel>>(_context.Positions);
        }

        public override PositionModel SavePosition(PositionModel position)
        {
            var exist = position.Id == 0 ? _context.CreateAndAdd<Position>() : _context.Positions.Get(position.Id, "Position not found");
            exist.Name = position.Name;
            _context.SaveChanges();
            return Mapper.Map<PositionModel>(exist);
        }

        public override void DeletePosition(int id)
        {
            var position = _context.Positions.Get(id, "Tank not found");
            _context.Positions.Remove(position);
            _context.SaveChanges();
        }
    }
}
