using System;
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
        SampleModel GetSample(Int32 id);
        SampleModel GetSample(String uniqLocatonName);
        SampleReportModel GetReportSample(Int32 id);
        List<SampleReportModel> GetReportSamples(ICollection<Int32> ids);
        List<SampleBriefModel> GetExtractedSamples();
        PagedResult<SampleBriefModel> GetSamples(PagedQuery query);
        PagedResult<SampleBriefModel> GetSamplesByTank(Int32 tankId);
        PagedResult<SampleBriefModel> GetSamplesByDoctor(Int32 doctorId);
        List<LocationReportModel> GetReport(ReportModel model);
        void DeleteSample(Int32 id);
        void DeleteSamples(List<Int32> ids);

        List<CaneModel> GetCanes();
        List<CaneModel> GetCanes(Int32 canisterId);
        CaneModel SaveCane(CaneModel cane, Int32 canisterId);
        void DeleteCane(Int32 id);

        List<CanisterModel> GetCanisters();
        List<CanisterModel> GetCanisters(Int32 tankdId);
        CanisterModel SaveCanister(CanisterModel canister, Int32 tankId);
        void DeleteCanister(Int32 id);

        List<CollectionMethodModel> GetCollectionMethods();
        CollectionMethodModel SaveCollectionMethod(CollectionMethodModel collectionMethod);
        void DeleteCollectionMethod(Int32 id);

        List<CommentModel> GetComments();
        CommentModel SaveComment(CommentModel comment);
        void DeleteComment(Int32 id);

        LocationModel GetLocation(String uniqName);
        List<LocationModel> GetLocations();
        List<LocationBriefModel> GetLocations(Int32 sampleId);
        LocationModel SaveLocation(LocationModel location);
        void DeleteLocation(Int32 id);

        List<PhysicianModel> GetPhysicians();
        PhysicianModel SavePhysician(PhysicianModel physician);
        void DeletePhysician(Int32 id);

        List<TankModel> GetTanks();
        TankModel SaveTank(TankModel tank);
        void DeleteTank(Int32 id);

        List<PositionModel> GetPositions();
        List<PositionModel> GetPositions(Int32 caneId);
        PositionModel SavePosition(PositionModel position, Int32 caneId);
        void DeletePosition(Int32 id);

        List<ExtractReasonModel> GetExtractReasons();
        ExtractReasonModel SaveExtractReason(ExtractReasonModel reason);
        void DeleteExtractReason(Int32 id);
    }
    public abstract class SampleManagerBase : ISampleManager
    {
        public abstract void AddConsentForm(string fileName, int id);
        public abstract SampleModel SaveSample(SaveSampleModel model, Int32? byUserId);
        public abstract SampleModel GetSample(int id);
        public abstract SampleModel GetSample(string uniqLocatonName);
        public abstract SampleReportModel GetReportSample(int id);
        public abstract List<SampleReportModel> GetReportSamples(ICollection<int> ids);
        public abstract List<SampleBriefModel> GetExtractedSamples();
        public abstract PagedResult<SampleBriefModel> GetSamples(PagedQuery query);
        public abstract PagedResult<SampleBriefModel> GetSamplesByTank(int tankId);
        public abstract PagedResult<SampleBriefModel> GetSamplesByDoctor(int doctorId);
        public abstract List<LocationReportModel> GetReport(ReportModel model);
        public abstract void DeleteSample(int id);
        public abstract void DeleteSamples(List<int> ids);
        public abstract List<CaneModel> GetCanes();
        public abstract List<CaneModel> GetCanes(int canisterId);
        public abstract CaneModel SaveCane(CaneModel cane, int canisterId);
        public abstract void DeleteCane(int id);
        public abstract List<CanisterModel> GetCanisters();
        public abstract List<CanisterModel> GetCanisters(int tankdId);
        public abstract CanisterModel SaveCanister(CanisterModel canister, int tankId);
        public abstract void DeleteCanister(int id);
        public abstract List<CollectionMethodModel> GetCollectionMethods();
        public abstract CollectionMethodModel SaveCollectionMethod(CollectionMethodModel collectionMethod);
        public abstract void DeleteCollectionMethod(int id);
        public abstract List<CommentModel> GetComments();
        public abstract CommentModel SaveComment(CommentModel comment);
        public abstract void DeleteComment(int id);
        public abstract LocationModel GetLocation(string uniqName);
        public abstract List<LocationModel> GetLocations();
        public abstract List<LocationBriefModel> GetLocations(int sampleId);
        public abstract LocationModel SaveLocation(LocationModel location);
        public abstract void DeleteLocation(int id);
        public abstract List<PhysicianModel> GetPhysicians();
        public abstract PhysicianModel SavePhysician(PhysicianModel physician);
        public abstract void DeletePhysician(int id);
        public abstract List<TankModel> GetTanks();
        public abstract TankModel SaveTank(TankModel tank);
        public abstract void DeleteTank(int id);
        public abstract List<PositionModel> GetPositions();
        public abstract List<PositionModel> GetPositions(int caneId);
        public abstract PositionModel SavePosition(PositionModel position, int caneId);
        public abstract void DeletePosition(int id);
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
        public String PartnerFirstName { get; set; }
        public String PartnerLastName { get; set; }
        public DateTime? PartnerDob { get; set; }
        public String PartnerSsn { get; set; }

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
        public Int32? CommentId { get; set; }

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
        public String PartnerFirstName { get; set; }
        public String PartnerLastName { get; set; }
        public DateTime? PartnerDob { get; set; }
        public String PartnerSsn { get; set; }
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
        public Int32? CreatedByUserId { get; set; }
        public DateTime ModifiedDate { get; set; }
        public Int32? ModifiedByUserId { get; set; }
        public Int32? PhysicianId { get; set; }

        public Int32? CommentId { get; set; }
    }

    public class ReportModel
    {
        public DateTime? StartDate { get; set; }
        public DateTime? EndDate { get; set; }
        public Int32? PhysicianId { get; set; }
        public Int32? TankId { get; set; }
        public String Type { get; set; }
    }

    public enum ReportType
    {
        Existing,
        Extracted,
        All
    }
}
