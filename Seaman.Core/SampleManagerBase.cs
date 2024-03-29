﻿using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Seaman.Core.Model;

namespace Seaman.Core
{
    public interface ISampleManager
    {
        void AddConsentForm(String fileName, Int32 id);
        SampleModel SaveSample(SaveSampleModel model, Int32? byUserId);
        Boolean CheckDepositor(SaveSampleModel model);
        SampleModel GetSample(Int32 id);
        SampleReportModel GetExtractedSample(Int32 id);
        SampleModel GetSample(String uniqLocatonName);
        SampleReportModel GetReportSample(Int32 id);
        List<SampleReportModel> GetReportSamples(ICollection<Int32> ids);
        List<SampleReportModel> GetExtractedSamples();
        List<SampleReportModel> GetSamples();
        List<SampleReportModel> ImportSamples(String fileUrl);
        PagedResult<SampleBriefModel> GetSamplesByTank(Int32 tankId);
        PagedResult<SampleBriefModel> GetSamplesByDoctor(Int32 doctorId);
        List<LocationReportModel> GetReport(ReportModel model);
        void DeleteSample(Int32 id);
        void DeleteSamples(List<Int32> ids);

        List<CollectionMethodModel> GetCollectionMethods();
        CollectionMethodModel SaveCollectionMethod(CollectionMethodModel collectionMethod);
        void DeleteCollectionMethod(Int32 id);

        LocationModel GetLocation(LocationModel model);
        List<LocationModel> GetLocations();
        List<LocationBriefModel> GetLocations(Int32 sampleId);
        LocationModel SaveLocation(LocationModel location);
        void DeleteLocation(Int32 id, Int32? reasonId);
        Boolean CheckCaneForEmpty(LocationModel location);

        List<PhysicianModel> GetPhysicians();
        PhysicianModel SavePhysician(PhysicianModel physician);
        void DeletePhysician(Int32 id);

        List<CryobankModel> GetCryobanks();
        CryobankModel SaveCryobank(CryobankModel physician);
        void DeleteCryobank(Int32 id);

        List<TankModel> GetTanks();
        TankModel SaveTank(TankModel tank);
        void DeleteTank(Int32 id);
       
        List<ExtractReasonModel> GetExtractReasons();
        ExtractReasonModel SaveExtractReason(ExtractReasonModel reason);
        void DeleteExtractReason(Int32 id);
    }
    public abstract class SampleManagerBase : ISampleManager
    {
        public abstract void AddConsentForm(string fileName, int id);
        public abstract SampleModel SaveSample(SaveSampleModel model, Int32? byUserId);
        public abstract Boolean CheckDepositor(SaveSampleModel model);
        public abstract SampleModel GetSample(int id);
        public abstract SampleReportModel GetExtractedSample(int id);
        public abstract SampleModel GetSample(string uniqLocatonName);
        public abstract SampleReportModel GetReportSample(int id);
        public abstract List<SampleReportModel> GetReportSamples(ICollection<int> ids);
        public abstract List<SampleReportModel> GetExtractedSamples();
        public abstract List<SampleReportModel> GetSamples();
        public abstract List<SampleReportModel> ImportSamples(String fileUrl);
        public abstract PagedResult<SampleBriefModel> GetSamplesByTank(int tankId);
        public abstract PagedResult<SampleBriefModel> GetSamplesByDoctor(int doctorId);
        public abstract List<LocationReportModel> GetReport(ReportModel model);
        public abstract void DeleteSample(int id);
        public abstract void DeleteSamples(List<int> ids);
        public abstract List<CollectionMethodModel> GetCollectionMethods();
        public abstract CollectionMethodModel SaveCollectionMethod(CollectionMethodModel collectionMethod);
        public abstract void DeleteCollectionMethod(int id);
        public abstract LocationModel GetLocation(LocationModel model);
        public abstract List<LocationModel> GetLocations();
        public abstract List<LocationBriefModel> GetLocations(int sampleId);
        public abstract LocationModel SaveLocation(LocationModel location);
        public abstract void DeleteLocation(int id, int? reasonId);
        public abstract bool CheckCaneForEmpty(LocationModel location);
        public abstract List<PhysicianModel> GetPhysicians();
        public abstract PhysicianModel SavePhysician(PhysicianModel physician);
        public abstract void DeletePhysician(int id);
        public abstract List<CryobankModel> GetCryobanks();
        public abstract CryobankModel SaveCryobank(CryobankModel physician);
        public abstract void DeleteCryobank(Int32 id);
        public abstract List<TankModel> GetTanks();
        public abstract TankModel SaveTank(TankModel tank);
        public abstract void DeleteTank(int id);
        public abstract List<ExtractReasonModel> GetExtractReasons();
        public abstract ExtractReasonModel SaveExtractReason(ExtractReasonModel reason);
        public abstract void DeleteExtractReason(int id);
    }

    public class SampleReportModel
    {
        public Int32 Id { get; set; }
        public String DepositorFullName { get; set; }
        public String DepositorFirstName { get; set; }
        public String DepositorLastName { get; set; }
        public String DepositorDob { get; set; }
        public String DepositorSsn { get; set; }
        public String PartnerFirstName { get; set; }
        public String PartnerLastName { get; set; }
        public String PartnerDob { get; set; }
        public String PartnerSsn { get; set; }
        public String CreatedDateString { get; set; }

        public String Autologous { get; set; }
        public String Refreeze { get; set; }
        public String TestingOnFile { get; set; }


        public Boolean CryobankPurchased { get; set; }
        public String CryobankName { get; set; }
        public String CryobankVialId { get; set; }


        public Boolean DirectedDonor { get; set; }
        public String DirectedDonorId { get; set; }
        public String DirectedDonorLastName { get; set; }
        public String DirectedDonorFirstName { get; set; }
        public String DirectedDonorDob { get; set; }

        public Boolean AnonymousDonor { get; set; }
        public String AnonymousDonorId { get; set; }

        public String Physician { get; set; }
        public String Comment { get; set; }

        public String ConsentFormUrl { get; set; }
        public List<LocationBriefModel> Locations
        {
            get { return _locations; }
            set { _locations = value; }
        }

        private List<LocationBriefModel> _locations = new List<LocationBriefModel>();
    }

    public class SaveSampleModel
    {
        public Int32 Id { get; set; }

        public String DepositorFirstName { get; set; }
        public String DepositorLastName { get; set; }
        public DateTime DepositorDob { get; set; }
        public String DepositorSsn { get; set; }
        public SsnType DepositorSsnType { get; set; }
        public String PartnerFirstName { get; set; }
        public String PartnerLastName { get; set; }
        public DateTime? PartnerDob { get; set; }
        public String PartnerSsn { get; set; }
        public SsnType PartnerSsnType { get; set; }

        public String DepositorAddress { get; set; }
        public String DepositorCity { get; set; }
        public String DepositorState { get; set; }
        public Int64? DepositorZip { get; set; }
        public String DepositorHomePhone { get; set; }
        public String DepositorCellPhone { get; set; }
        public String DepositorEmail { get; set; }

        public String PartnerAddress { get; set; }
        public String PartnerCity { get; set; }
        public String PartnerState { get; set; }
        public Int64? PartnerZip { get; set; }
        public String PartnerHomePhone { get; set; }
        public String PartnerCellPhone { get; set; }
        public String PartnerEmail { get; set; }

        public Boolean Autologous { get; set; }
        public Boolean Refreeze { get; set; }
        public Boolean TestingOnFile { get; set; }


        public Boolean CryobankPurchased { get; set; }
        public String CryobankName { get; set; }
        public String CryobankVialId { get; set; }


        public Boolean DirectedDonor { get; set; }
        public String DirectedDonorId { get; set; }
        public String DirectedDonorLastName { get; set; }
        public String DirectedDonorFirstName { get; set; }
        public DateTime? DirectedDonorDob { get; set; }

        public Boolean AnonymousDonor { get; set; }
        public String AnonymousDonorId { get; set; }


        public Int32? PhysicianId { get; set; }
        public String Comment { get; set; }

        public List<LocationModel> LocationsToAdd
        {
            get { return _locationsToAdd; }
            set { _locationsToAdd = value; }
        }

        public List<LocationModel> LocationsToRemove
        {
            get { return _locationsToRemove; }
            set { _locationsToRemove = value; }
        }

        private List<LocationModel> _locationsToAdd = new List<LocationModel>();
        private List<LocationModel> _locationsToRemove = new List<LocationModel>();
    }

    public class SampleBriefModel
    {
        public Int32 Id { get; set; }
        public String DepositorFullName { get; set; }
        public String DepositorDob { get; set; }
        public String Comment { get; set; }
        public String Physician { get; set; }
        public String ConsentFormUrl { get; set; }
        public List<LocationBriefModel> Locations
        {
            get { return _locations; }
            set { _locations = value; }
        }

        private List<LocationBriefModel> _locations = new List<LocationBriefModel>();
    }

    public class SampleModel : SampleBase
    {
        public List<LocationModel> Locations
        {
            get { return _locations; }
            set { _locations = value; }
        }

        private List<LocationModel> _locations = new List<LocationModel>();
    }

    public class SampleBase : IEntity
    {
        public SampleBase()
        {
            CreatedDate = DateTime.UtcNow;
            ModifiedDate = DateTime.UtcNow;
            Autologous = false;
            TestingOnFile = false;
        }
        public Int32 Id { get; set; }

        public String DepositorFirstName { get; set; }
        public String DepositorLastName { get; set; }
        public DateTime DepositorDob { get; set; }
        public String DepositorSsn { get; set; }
        public SsnType DepositorSsnType { get; set; }
        public String PartnerFirstName { get; set; }
        public String PartnerLastName { get; set; }
        public DateTime? PartnerDob { get; set; }
        public String PartnerSsn { get; set; }
        public SsnType PartnerSsnType { get; set; }
        public Boolean Autologous { get; set; }
        public Boolean Refreeze { get; set; }
        public Boolean TestingOnFile { get; set; }


        public Boolean CryobankPurchased { get; set; }
        public String CryobankName { get; set; }
        public String CryobankVialId { get; set; }


        public Boolean DirectedDonor { get; set; }
        public String DirectedDonorId { get; set; }
        public String DirectedDonorLastName { get; set; }
        public String DirectedDonorFirstName { get; set; }
        public DateTime? DirectedDonorDob { get; set; }

        public Boolean AnonymousDonor { get; set; }
        public String AnonymousDonorId { get; set; }

        public String ConsentFormUrl { get; set; }

        public DateTime CreatedDate { get; set; }
        public String CreatedDateString { get; set; }
        public Int32? CreatedByUserId { get; set; }
        public DateTime ModifiedDate { get; set; }
        public Int32? ModifiedByUserId { get; set; }
        public Int32? PhysicianId { get; set; }

        public String Comment { get; set; }

        public String DepositorAddress { get; set; }
        public String DepositorCity { get; set; }
        public String DepositorState { get; set; }
        public Int64? DepositorZip { get; set; }
        public String DepositorHomePhone { get; set; }
        public String DepositorCellPhone { get; set; }
        public String DepositorEmail { get; set; }

        public String PartnerAddress { get; set; }
        public String PartnerCity { get; set; }
        public String PartnerState { get; set; }
        public Int64? PartnerZip { get; set; }
        public String PartnerHomePhone { get; set; }
        public String PartnerCellPhone { get; set; }
        public String PartnerEmail { get; set; }
    }

    public class ReportModel
    {
        public DateTime? StartDate { get; set; }
        public DateTime? EndDate { get; set; }
        public DateTime? FrozenStartDate { get; set; }
        public DateTime? FrozenEndDate { get; set; }
        public Int32? PhysicianId { get; set; }
        public Int32? TankId { get; set; }
        public Int32? Canister { get; set; }
        public Int32? CollectionMethodId { get; set; }
        public String Type { get; set; }
    }

    public enum ReportType
    {
        Existing,
        Extracted,
        Missed, 
        All
    }

    public enum SsnType
    {
        SSN = 10 ,
        Dl = 20,
        Passport = 30,
        Other = 40
    }
}
