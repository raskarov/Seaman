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
        SampleModel SaveSample(SaveSampleModel model, Int32? byUserId);
        SampleModel GetSample(Int32 id);
        SampleModel GetSample(String uniqLocatonName);
        PagedResult<SampleModel> GetSamples();
        PagedResult<SampleModel> GetSamplesByTank(Int32 tankId);
        PagedResult<SampleModel> GetSamplesByDoctor(Int32 doctorId);
        void DeleteSample(Int32 id);
        void DeleteSamples(List<Int32> ids);

        List<CaneModel> GetCanes();
        CaneModel SaveCane(CaneModel cane);
        void DeleteCane(Int32 id);

        List<CanisterModel> GetCanisters();
        CanisterModel SaveCanister(CanisterModel canister);
        void DeleteCanister(Int32 id);

        List<CollectionMethodModel> GetCollectionMethods();
        CollectionMethodModel SaveCollectionMethod(CollectionMethodModel collectionMethod);
        void DeleteCollectionMethod(Int32 id);

        List<CommentModel> GetComments();
        CommentModel SaveComment(CommentModel comment);
        void DeleteComment(Int32 id);
        

        List<LocationModel> GetLocations();
        LocationModel SaveLocation(LocationModel location);
        void DeleteLocation(Int32 id);

        List<PhysicianModel> GetPhysicians();
        PhysicianModel SavePhysician(PhysicianModel physician);
        void DeletePhysician(Int32 id);

        List<TankModel> GetTanks();
        TankModel SaveTank(TankModel tank);
        void DeleteTank(Int32 id);

        List<PositionModel> GetPositions();
        PositionModel SavePosition(PositionModel tank);
        void DeletePosition(Int32 id);
    }
    public abstract class SampleManagerBase : ISampleManager
    {
        public abstract SampleModel SaveSample(SaveSampleModel model, Int32? byUserId);
        public abstract SampleModel GetSample(int id);
        public abstract SampleModel GetSample(string uniqLocatonName);
        public abstract PagedResult<SampleModel> GetSamples();
        public abstract PagedResult<SampleModel> GetSamplesByTank(int tankId);
        public abstract PagedResult<SampleModel> GetSamplesByDoctor(int doctorId);
        public abstract void DeleteSample(int id);
        public abstract void DeleteSamples(List<int> ids);
        public abstract List<CaneModel> GetCanes();
        public abstract CaneModel SaveCane(CaneModel cane);
        public abstract void DeleteCane(int id);
        public abstract List<CanisterModel> GetCanisters();
        public abstract CanisterModel SaveCanister(CanisterModel canister);
        public abstract void DeleteCanister(int id);
        public abstract List<CollectionMethodModel> GetCollectionMethods();
        public abstract CollectionMethodModel SaveCollectionMethod(CollectionMethodModel collectionMethod);
        public abstract void DeleteCollectionMethod(int id);
        public abstract List<CommentModel> GetComments();
        public abstract CommentModel SaveComment(CommentModel comment);
        public abstract void DeleteComment(int id);
        public abstract List<LocationModel> GetLocations();
        public abstract LocationModel SaveLocation(LocationModel location);
        public abstract void DeleteLocation(int id);
        public abstract List<PhysicianModel> GetPhysicians();
        public abstract PhysicianModel SavePhysician(PhysicianModel physician);
        public abstract void DeletePhysician(int id);
        public abstract List<TankModel> GetTanks();
        public abstract TankModel SaveTank(TankModel tank);
        public abstract void DeleteTank(int id);
        public abstract List<PositionModel> GetPositions();
        public abstract PositionModel SavePosition(PositionModel tank);
        public abstract void DeletePosition(int id);
    }

    public class SaveSampleModel
    {
        public Int32 Id { get; set; }

        public String DepositorFirstName { get; set; }
        public String DepositorLastName { get; set; }
        public String DepositorDob { get; set; }
        public String DepositorSsn { get; set; }
        public String PartnerFirstName { get; set; }
        public String PartnerLastName { get; set; }
        public String PartnerDob { get; set; }
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

        public Boolean AnonymousDonor { get; set; }
        public String AnonymousDonorId { get; set; }

        public Int32? PhysicianId { get; set; }
        public Int32? CollectionMethodId { get; set; }
        public Int32? CommentId { get; set; }

        public List<AttachedLocationModel> Locations
        {
            get { return _locations; }
            set { _locations = value; }
        }

        private List<AttachedLocationModel> _locations = new List<AttachedLocationModel>();
    }

    public class AttachedLocationModel
    {
        public Int32 TankId { get; set; }
        public Int32 CanisterId { get; set; }
        public Int32 CaneId { get; set; }
        public Int32 LocationId { get; set; }
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
        public String DepositorDob { get; set; }
        public String DepositorSsn { get; set; }
        public String PartnerFirstName { get; set; }
        public String PartnerLastName { get; set; }
        public String PartnerDob { get; set; }
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

        public Boolean AnonymousDonor { get; set; }
        public String AnonymousDonorId { get; set; }

        public DateTime CreatedDate { get; set; }
        public Int32? CreatedByUserId { get; set; }
        public DateTime ModifiedDate { get; set; }
        public Int32? ModifiedByUserId { get; set; }
        public Int32? PhysicianId { get; set; }
        public Int32? CollectionMethodId { get; set; }
        public Int32? CommentId { get; set; }
    }
}
