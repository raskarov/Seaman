using System;
using System.Collections.Generic;
using System.Linq;
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
                if(sample.Locations.Any(it => it.Id == attachedLocation.LocationId))
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

        public override List<CaneModel> GetCanes()
        {
            throw new NotImplementedException();
        }

        public override CaneModel SaveCane(CaneModel cane)
        {
            throw new NotImplementedException();
        }

        public override List<CanisterModel> GetCanisters()
        {
            throw new NotImplementedException();
        }

        public override CanisterModel SaveCanister(CanisterModel canister)
        {
            throw new NotImplementedException();
        }

        public override List<CollectionMethodModel> GetCollectionMethods()
        {
            throw new NotImplementedException();
        }

        public override CollectionMethodModel SaveCollectionMethod(CollectionMethodModel collectionMethod)
        {
            throw new NotImplementedException();
        }

        public override List<CommentModel> GetComments()
        {
            throw new NotImplementedException();
        }

        public override CommentModel SaveComment(CommentModel comment)
        {
            throw new NotImplementedException();
        }

        public override List<LocationModel> GetLocations()
        {
            throw new NotImplementedException();
        }

        public override LocationModel SaveLocation(LocationModel location)
        {
            throw new NotImplementedException();
        }

        public override List<PhysicianModel> GetPhysicians()
        {
            throw new NotImplementedException();
        }

        public override PhysicianModel SavePhysician(PhysicianModel physician)
        {
            throw new NotImplementedException();
        }

        public override List<TankModel> GetTanks()
        {
            throw new NotImplementedException();
        }

        public override TankModel SaveTank(TankModel tank)
        {
            throw new NotImplementedException();
        }
    }
}
