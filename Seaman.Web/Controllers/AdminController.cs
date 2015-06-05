using System;
using System.Collections.Generic;
using System.Linq;
using System.Net;
using System.Net.Http;
using System.Web.Http;
using Seaman.Core;
using Seaman.Core.Model;
using Seaman.Web.Code;

namespace Seaman.Web.Controllers
{
    [RoutePrefix("api/admin")]
    [Authorize]
    public class AdminController : ApiController
    {
        #region Constructor
        public AdminController(Lazy<ISampleManager> sampleManagerLazy)
        {
            _sampleManagerLazy = sampleManagerLazy;
        }
        #endregion

        #region Lists

        [HttpGet]
        [Route("collectionMethod")]
        public List<CollectionMethodModel> GetCollectionMethods()
        {
            return SampleManager.GetCollectionMethods();
        }

        [HttpPost]
        [Route("collectionMethod")]
        public CollectionMethodModel PostCollectionMethod(CollectionMethodModel model)
        {
            if (!ModelState.IsValid)
                throw new SeamanInvalidModelException();
            return SampleManager.SaveCollectionMethod(model);
        }

        [HttpDelete]
        [Route("collectionMethod/{id:int}")]
        public void DeleteCollectionMethod(Int32 id)
        {
            SampleManager.DeleteCollectionMethod(id);
        }

        [HttpGet]
        [Route("comment")]
        public List<CommentModel> GetComments()
        {
            return SampleManager.GetComments();
        }

        [HttpPost]
        [Route("comment")]
        public CommentModel PostComment(CommentModel model)
        {
            if (!ModelState.IsValid)
                throw new SeamanInvalidModelException();
            return SampleManager.SaveComment(model);
        }

        [HttpDelete]
        [Route("comment/{id:int}")]
        public void DeleteComment(Int32 id)
        {
            SampleManager.DeleteComment(id);
        }

        [HttpGet]
        [Route("physician")]
        public List<PhysicianModel> GetPhysician()
        {
            return SampleManager.GetPhysicians();
        }

        [HttpPost]
        [Route("physician")]
        public PhysicianModel PostPhysician(PhysicianModel model)
        {
            if (!ModelState.IsValid)
                throw new SeamanInvalidModelException();
            return SampleManager.SavePhysician(model);
        }

        [HttpDelete]
        [Route("physician/{id:int}")]
        public void DeletePhysician(Int32 id)
        {
            SampleManager.DeletePhysician(id);
        }

        [HttpGet]
        [Route("tank")]
        public List<TankModel> GetTanks()
        {
            return SampleManager.GetTanks();
        }

        [HttpPost]
        [Route("tank")]
        public TankModel PostTank(TankModel model)
        {
            if (!ModelState.IsValid)
                throw new SeamanInvalidModelException();
            return SampleManager.SaveTank(model);
        }

        [HttpDelete]
        [Route("tank/{id:int}")]
        public void DeleteTank(Int32 id)
        {
            SampleManager.DeleteTank(id);
        }

        [HttpGet]
        [Route("canister")]
        public List<CanisterModel> GetCanisters()
        {
            return SampleManager.GetCanisters();
        }

        [HttpPost]
        [Route("canister")]
        public CanisterModel PostCanister(CanisterModel model)
        {
            if (!ModelState.IsValid)
                throw new SeamanInvalidModelException();
            return SampleManager.SaveCanister(model);
        }

        [HttpDelete]
        [Route("canister/{id:int}")]
        public void DeleteCanister(Int32 id)
        {
            SampleManager.DeleteCanister(id);
        }

        [HttpGet]
        [Route("cane")]
        public List<CaneModel> GetCanes()
        {
            return SampleManager.GetCanes();
        }

        [HttpPost]
        [Route("cane")]
        public CaneModel PostCane(CaneModel model)
        {
            if (!ModelState.IsValid)
                throw new SeamanInvalidModelException();
            return SampleManager.SaveCane(model);
        }

        [HttpDelete]
        [Route("cane/{id:int}")]
        public void DeleteCane(Int32 id)
        {
            SampleManager.DeleteCane(id);
        }

        [HttpGet]
        [Route("position")]
        public List<PositionModel> GetPositions()
        {
            return SampleManager.GetPositions();
        }

        [HttpPost]
        [Route("position")]
        public PositionModel PostPosition(PositionModel model)
        {
            if (!ModelState.IsValid)
                throw new SeamanInvalidModelException();
            return SampleManager.SavePosition(model);
        }

        [HttpDelete]
        [Route("position/{id:int}")]
        public void DeletePosition(Int32 id)
        {
            SampleManager.DeletePosition(id);
        }


        #endregion




        #region Privat
        private readonly Lazy<ISampleManager> _sampleManagerLazy;

        private ISampleManager SampleManager
        {
            get
            {
                return _sampleManagerLazy.Value;
            }
        }
        #endregion
    }
}
