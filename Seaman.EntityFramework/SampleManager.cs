﻿using System;
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
            sample.CommentId = model.CommentId;

            sample.AnonymousDonor = model.AnonymousDonor;
            sample.AnonymousDonorId = model.AnonymousDonorId;

            sample.CryobankPurchased = model.CryobankPurchased;
            sample.CryobankName = model.CryobankName;
            sample.CryobankVialId = model.CryobankVialId;

            sample.DirectedDonor = model.DirectedDonor;
            sample.DirectedDonorFirstName = model.DirectedDonorFirstName;
            sample.DirectedDonorLastName = model.DirectedDonorLastName;
            sample.DirectedDonorDob = model.DirectedDonorDob;
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

            foreach (var locationToAdd in model.LocationsToAdd)
            {
                Location location;
                if (locationToAdd.Id == 0)
                {
                    location = _context.CreateAndAdd<Location>();
                    _context.SaveChanges();
                }
                else
                {
                    location = _context.Locations.Get(locationToAdd.Id, "location not found");
                }
                if (location.Extracted)
                {
                    location = _context.CreateAndAdd<Location>();
                    _context.SaveChanges();
                }
                if (sample.Locations.Any(l => l.Id == location.Id))
                    continue;
                sample.Locations.Add(location);
                location.Tank = locationToAdd.Tank;
                location.Canister = locationToAdd.Canister;
                location.Cane = locationToAdd.Cane;
                location.Position = locationToAdd.Position;
                location.DateStored = locationToAdd.DateStored;
                location.CollectionMethodId = locationToAdd.CollectionMethodId;
                location.Available = false;
                location.UniqName = location.Tank + location.Canister + location.Cane + location.Position;

            }
            foreach (var locationToRemove in model.LocationsToRemove)
            {
                var location = _context.Locations.Get(locationToRemove.Id, "location not found");
                location.Extracted = true;
                location.Available = true;
            }

            _context.SaveChanges();
            return Mapper.Map<SampleModel>(sample);
        }

        public override SampleModel GetSample(int id)
        {
            return Mapper.Map<SampleModel>(_context.Samples.Get(id, "Sample not found"));
        }

        public override SampleModel GetSample(String uniqLocatonName)
        {
            return
                Mapper.Map<SampleModel>(
                    _context.Samples.FirstOrDefault(x => x.Locations.Any(l => l.UniqName == uniqLocatonName)));
        }

        public override SampleReportModel GetReportSample(int id)
        {
            return Mapper.Map<SampleReportModel>(_context.Samples.Get(id, "Sample not found"));
        }

        public override List<SampleReportModel> GetReportSamples(ICollection<int> ids)
        {
            return Mapper.Map<List<SampleReportModel>>(ids.Any() ? _context.Samples.Where(s => ids.Any(id => id == s.Id)) : _context.Samples);
        }

        public override PagedResult<SampleBriefModel> GetSamples(PagedQuery query)
        {
            var samples = _context.Samples.AsQueryable();
            samples = samples.OrderBy(it => it.Id);
            var total = query.SkipTakeCount(ref samples);

            return new PagedResult<SampleBriefModel>
            {
                Data = Mapper.Map<List<SampleBriefModel>>(samples),
                Query = query,
                Total = total
            };
        }

        public override PagedResult<SampleBriefModel> GetSamplesByTank(int tankId)
        {
            throw new NotImplementedException();
        }

        public override PagedResult<SampleBriefModel> GetSamplesByDoctor(int doctorId)
        {
            throw new NotImplementedException();
        }

        public override List<LocationReportModel> GetReport(ReportModel model)
        {
            IQueryable<Location> locations = _context.Locations;
            if (model.Type == ReportType.Extracted.ToString())
            {
                locations = locations.Where(l => l.Extracted);
            }
            else if (model.Type == ReportType.Existing.ToString())
            {
                locations = locations.Where(l => !l.Extracted);
            }

            var startDate = model.StartDate ?? DateTime.MinValue;
            var endDate = model.EndDate ?? DateTime.MaxValue;
            if (startDate > endDate)
            {
                startDate = endDate;
            }

            locations = locations.Where(l => l.DateStored >= startDate && l.DateStored <= endDate);
            if (model.TankId.HasValue)
            {
                var tank = _context.Tanks.Get(model.TankId.Value, "Tank not found");
                locations = locations.Where(l => l.Tank == tank.Name);
            }

            if (model.PhysicianId.HasValue)
            {
                locations = locations.Where(l => l.Sample.PhysicianId == model.PhysicianId.Value);
            }
            return Mapper.Map<List<LocationReportModel>>(locations.OrderBy(l => l.Extracted));
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

        public override List<CaneModel> GetCanes(int canisterId)
        {
            return Mapper.Map<List<CaneModel>>(_context.Canes.Where(x => x.Canister.Id == canisterId));
        }

        public override CaneModel SaveCane(CaneModel cane, int canisterId)
        {
            var canister = _context.Canisters.Get(canisterId, "Canister not found");
            if (canister == null) return null;
            var exist = cane.Id == 0 ? _context.CreateAndAdd<Cane>() : _context.Canes.Get(cane.Id, "Cane not found");
            exist.Name = cane.Name;
            exist.Color = cane.Color;
            canister.Canes.Add(exist);
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

        public override List<CanisterModel> GetCanisters(int tankdId)
        {
            return Mapper.Map<List<CanisterModel>>(_context.Canisters.Where(x => x.Tank.Id == tankdId));
        }

        public override CanisterModel SaveCanister(CanisterModel canister, int tankId)
        {
            var tank = _context.Tanks.Get(tankId, "Tank not found");
            if (tank == null) return null;
            var exist = canister.Id == 0 ? _context.CreateAndAdd<Canister>() : _context.Canisters.Get(canister.Id, "Canister not found");
            exist.Name = canister.Name;
            //tank.Canisters.Add(exist);
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
            foreach (var sample in collectionMethod.Samples)
            {
                sample.CollectionMethod = null;
            }
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
            foreach (var sample in comment.Samples)
            {
                sample.Comment = null;
            }
            _context.Comments.Remove(comment);
            _context.SaveChanges();
        }

        public override LocationModel GetLocation(string uniqName)
        {
            var location = _context.Locations.FirstOrDefault(x => x.UniqName == uniqName && !x.Extracted);
            return Mapper.Map<LocationModel>(location);
        }

        public override List<LocationModel> GetLocations()
        {
            return Mapper.Map<List<LocationModel>>(_context.Locations);
        }

        public override List<LocationBriefModel> GetLocations(int sampleId)
        {
            return Mapper.Map<List<LocationBriefModel>>(_context.Locations.Where(l => l.Sample.Id == sampleId));
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
            location.Extracted = true;
            location.Available = true;
            location.CollectionMethodId = null;
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
            foreach (var sample in physician.Samples)
            {
                sample.Physician = null;
            }
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
            exist.CanesCount = tank.CanesCount;
            exist.CanistersCount = tank.CanistersCount;
            exist.PositionsCount = tank.PositionsCount;
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

        public override List<PositionModel> GetPositions(int caneId)
        {
            return Mapper.Map<List<PositionModel>>(_context.Positions.Where(x => x.Cane.Id == caneId));
        }

        public override PositionModel SavePosition(PositionModel position, int caneId)
        {
            var cane = _context.Canes.Get(caneId, "Cane not found");
            if (cane == null) return null;
            var exist = position.Id == 0 ? _context.CreateAndAdd<Position>() : _context.Positions.Get(position.Id, "Position not found");
            exist.Name = position.Name;
            cane.Positions.Add(exist);
            _context.SaveChanges();
            return Mapper.Map<PositionModel>(exist);
        }

        public override void DeletePosition(int id)
        {
            var position = _context.Positions.Get(id, "Tank not found");
            _context.Positions.Remove(position);
            _context.SaveChanges();
        }

        public override List<ExtractReasonModel> GetExtractReasons()
        {
            return Mapper.Map<List<ExtractReasonModel>>(_context.ExtractReasons);
        }

        public override ExtractReasonModel SaveExtractReason(ExtractReasonModel reason)
        {
            var exist = reason.Id == 0 ? _context.CreateAndAdd<ExtractReason>() : _context.ExtractReasons.Get(reason.Id, "Location not found");
            exist.Name = reason.Name;
            _context.SaveChanges();
            return Mapper.Map<ExtractReasonModel>(exist);
        }

        public override void DeleteExtractReason(int id)
        {
            var reason = _context.ExtractReasons.Get(id, "Tank not found");
            _context.ExtractReasons.Remove(reason);
            _context.SaveChanges();
        }
    }
}
