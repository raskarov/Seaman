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
        [Route("reason")]
        public List<ExtractReasonModel> GetReasons()
        {
            return SampleManager.GetExtractReasons();
        }

        [HttpPost]
        [Route("reason")]
        public ExtractReasonModel PostReason(ExtractReasonModel model)
        {
            if (!ModelState.IsValid)
                throw new SeamanInvalidModelException();
            return SampleManager.SaveExtractReason(model);
        }

        [HttpDelete]
        [Route("reason/{id:int}")]
        public void DeleteReason(Int32 id)
        {
            SampleManager.DeleteExtractReason(id);
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
