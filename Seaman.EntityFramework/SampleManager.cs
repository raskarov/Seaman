using System;
using System.Collections.Generic;
using System.Data.Entity;
using System.Data.Entity.Core.Mapping;
using System.Data.Entity.Migrations;
using System.IO;
using System.Linq;
using System.Security.Cryptography.X509Certificates;
using System.Text;
using System.Threading.Tasks;
using AutoMapper;
using ClosedXML.Excel;
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

        public override void AddConsentForm(string fileName, int id)
        {
            var sample = _context.Samples.Get(id, "Sample not found");
            if (sample != null)
            {
                sample.ConsentFormUrl = fileName;
            }
            _context.SaveChanges();
        }

        public override Boolean CheckDepositor(SaveSampleModel model)
        {
            if (model.Id == 0)
            {
                var samples = _context.Samples
                .Where(x => x.DepositorFirstName == model.DepositorFirstName
                    && x.DepositorLastName == model.DepositorLastName);

                return samples.Count() != 0 ? true : false;  
            }
            else
            {
                return false;  
            }
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
            sample.Comment = model.Comment;

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
            sample.DepositorSsnType = model.DepositorSsnType;

            sample.PartnerFirstName = model.PartnerFirstName;
            sample.PartnerLastName = model.PartnerLastName;
            sample.PartnerDob = model.PartnerDob;
            sample.PartnerSsn = model.PartnerSsn;
            sample.PartnerSsnType = model.PartnerSsnType;

            sample.Autologous = model.Autologous;
            sample.TestingOnFile = model.TestingOnFile;
            sample.Refreeze = model.Refreeze;

            sample.DepositorAddress = model.DepositorAddress;
            sample.DepositorCity = model.DepositorCity;
            sample.DepositorState = model.DepositorState;
            sample.DepositorZip = model.DepositorZip;
            sample.DepositorHomePhone = model.DepositorHomePhone;
            sample.DepositorCellPhone = model.DepositorCellPhone;
            sample.DepositorEmail = model.DepositorEmail;

            sample.PartnerAddress = model.PartnerAddress;
            sample.PartnerCity = model.PartnerCity;
            sample.PartnerState = model.PartnerState;
            sample.PartnerZip = model.PartnerZip;
            sample.PartnerHomePhone = model.PartnerHomePhone;
            sample.PartnerCellPhone = model.PartnerCellPhone;
            sample.PartnerEmail = model.PartnerEmail;

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
                //if (sample.Locations.Any(l => l.Id == location.Id))
                //    continue;
                sample.Locations.Add(location);
                location.Tank = locationToAdd.Tank;
                location.Canister = locationToAdd.Canister;
                location.CaneLetter = locationToAdd.CaneLetter;
                location.CaneColor = locationToAdd.CaneColor;
                String position = "";
                foreach (var pos in locationToAdd.PosForShow)
                {
                    position += position.Length == 0 ? pos : "," + pos;
                }
                location.Position = position;
                location.DateStored = locationToAdd.DateStored;
                location.DateFrozen = locationToAdd.DateFrozen;
                location.CollectionMethodId = locationToAdd.CollectionMethodId;
                location.SpecimenNumber = locationToAdd.SpecimenNumber;
                location.Available = false;
                location.UniqName = GetUniqNameByLocation(locationToAdd);
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

        public override SampleReportModel GetExtractedSample(int id)
        {            
             var extractedSample =
                  _context.Samples.FirstOrDefault(x => x.Locations.Any(l => l.Extracted) && x.Id == id);
            if(extractedSample!=null)
                extractedSample.Locations = extractedSample.Locations.Where(l => l.Extracted).OrderByDescending(l => l.DateExtracted).ToList();
            return Mapper.Map<SampleReportModel>(extractedSample);
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
            var sample = _context.Samples.Get(id, "Sample not found");
            sample.Locations = sample.Locations.Where(l => !l.Extracted).ToList();

            return Mapper.Map<SampleReportModel>(sample);
        }

        public override List<SampleReportModel> GetReportSamples(ICollection<int> ids)
        {
            return Mapper.Map<List<SampleReportModel>>(ids.Any() ? _context.Samples.Where(s => ids.Any(id => id == s.Id)) : _context.Samples);
        }

        public override List<SampleReportModel> GetExtractedSamples()
        {
            var extractedSamples = _context.Samples.Where(s => s.Locations.Any(l => l.Extracted)).ToList();
            foreach (var sample in extractedSamples)
            {
                sample.Locations = sample.Locations.Where(l => l.Extracted).OrderByDescending(l => l.DateExtracted).ToList();
            }

            return Mapper.Map<List<SampleReportModel>>(extractedSamples.OrderByDescending(s => s.Id));
        }

        public override List<SampleReportModel> GetSamples()
        {
            var samples = _context.Samples.OrderByDescending(x => x.CreatedDate).ToList();
            foreach (var sample in samples)
            {
                sample.CreatedDateString = sample.CreatedDate.Month.ToString("00") + 
                    "/" + sample.CreatedDate.Day.ToString("00") + "/" + sample.CreatedDate.Year;
                sample.Locations = sample.Locations.Where(l => !l.Extracted).ToList();
            }
            
            return Mapper.Map<List<SampleReportModel>>(samples);
        }

        public override List<SampleReportModel> ImportSamples(String fileUrl)
        {
            if (String.IsNullOrEmpty(fileUrl) || !File.Exists(fileUrl)) return null;
            var wb = new XLWorkbook(fileUrl);
            var ws = wb.Worksheets.FirstOrDefault();

            if (ws != null)
            {
                var firstRowUsed = ws.FirstRowUsed();
                var firstRowNumber = firstRowUsed.RowNumber();

                var firstAdress = ws.Row(firstRowNumber).FirstCell().Address;
                var lastAdress = ws.LastCellUsed().Address;

                var wordsRange = ws.Range(firstAdress, lastAdress).RangeUsed();

                var wordsTable = wordsRange.AsTable();

                var result = wordsTable.DataRange.Rows()
                    .Select(wordRow => wordRow);
            }
            return null;
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
            else if (model.Type == ReportType.Missed.ToString())
            {
                locations = locations.Where(l => String.IsNullOrEmpty(l.Sample.DepositorSsn)
                           || String.IsNullOrEmpty(l.Sample.PartnerFirstName)
                           || String.IsNullOrEmpty(l.Sample.PartnerLastName)
                           || String.IsNullOrEmpty(l.Sample.PartnerSsn)
                           || l.Sample.PartnerDob.HasValue
                           || !l.Sample.CryobankPurchased
                           || !l.Sample.DirectedDonor
                           || !l.Sample.AnonymousDonor);
            }

            var startDate = model.StartDate ?? DateTime.MinValue;
            var endDate = model.EndDate ?? DateTime.MaxValue;
    
            if (startDate > endDate)
            {
                startDate = endDate;
            }

            var frozenStartDate = model.FrozenStartDate ?? DateTime.MinValue;
            var frozenEndDate = model.FrozenEndDate ?? DateTime.MaxValue;
            locations = locations.Where(l => l.DateStored >= startDate && l.DateStored <= endDate 
                && l.DateFrozen >= frozenStartDate && l.DateFrozen <= frozenEndDate);
            if (model.TankId.HasValue)
            {
                var tank = _context.Tanks.Get(model.TankId.Value, "Tank not found");
                locations = locations.Where(l => l.Tank == tank.Name);
                if (model.Canister.HasValue)
                {
                    locations = locations.Where(l => l.Canister == model.Canister.Value);
                }
            }

            

            if (model.CollectionMethodId.HasValue)
            {
                locations = locations.Where(l => l.CollectionMethodId == model.CollectionMethodId.Value);
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

        public override LocationModel GetLocation(LocationModel model)
        {
            Boolean isFindLocation = false;
            var findLocation = _context.Locations.FirstOrDefault(x => !x.Extracted);
            var locations = _context.Locations.Where(x => x.Tank == model.Tank
                                                          && x.Canister == model.Canister
                                                          && x.CaneColor == model.CaneColor
                                                          && x.CaneLetter == model.CaneLetter
                                                          && x.Id != model.Id
                                                          && !x.Extracted);
            var newPositions = model.PosForShow.ToList();
            foreach (var location in locations)
            {
                var positions = location.Position.Split(',');
                foreach (var position in positions)
                {
                    foreach (var newPosition in newPositions)
                    {
                        if (position == newPosition)
                        {
                            isFindLocation = true;
                        }
                    }
                }
            }
            if (isFindLocation)
            {
                return Mapper.Map<LocationModel>(findLocation);              
            }               
            else
            {
                return Mapper.Map<LocationModel>(null);
            }          
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

        public override void DeleteLocation(Int32 id, Int32? reasonId)
        {
            var location = _context.Locations.Get(id, "Location not found");
            location.Extracted = true;
            location.DateExtracted = DateTime.Now;
            location.Available = true;
            location.ExtractReasonId = reasonId;
            _context.SaveChanges();
        }

        public override bool CheckCaneForEmpty(LocationModel location)
        {
            return
                !_context.Locations.Any(
                    l =>
                        l.Tank == location.Tank && l.Canister == location.Canister &&
                        l.CaneLetter == location.CaneLetter && l.CaneColor == location.CaneColor && !l.Extracted);
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

        public override List<CryobankModel> GetCryobanks()
        {
            return Mapper.Map<List<CryobankModel>>(_context.Cryobanks);
        }

        public override CryobankModel SaveCryobank(CryobankModel cryobank)
        {
            var exist = cryobank.Id == 0 ? _context.CreateAndAdd<Cryobank>() : _context.Cryobanks.Get(cryobank.Id, "Location not found");
            exist.Name = cryobank.Name;
            exist.VialId = cryobank.VialId;
            _context.SaveChanges();
            return Mapper.Map<CryobankModel>(exist);
        }

        public override void DeleteCryobank(int VialId)
        {
            var cryobanks = _context.Cryobanks.Get(VialId, "Physician not found");
            _context.Cryobanks.Remove(cryobanks);
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
            exist.TankDescription = tank.TankDescription;
            _context.SaveChanges();
            return Mapper.Map<TankModel>(exist);
        }

        public override void DeleteTank(int id)
        {
            var tank = _context.Tanks.Get(id, "Tank not found");
            _context.Tanks.Remove(tank);
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

        private String GetUniqNameByLocation(LocationModel location)
        {
            String position = "";
            foreach (var pos in location.PosForShow)
            {
                position += position.Length == 0 ? pos : "," + pos;
            }
            return String.Format("{0}-{1}-{2}-{3}-{4}", location.Tank, location.Canister, location.CaneLetter, position, location.CaneColor);
        }
    }
}
