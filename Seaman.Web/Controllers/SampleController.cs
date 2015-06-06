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
    [RoutePrefix("api/sample")]
    public class SampleController : ApiController
    {
        #region Constructor
        public SampleController(Lazy<ISampleManager> sampleManagerLazy)
        {
            _sampleManagerLazy = sampleManagerLazy;
        }
        #endregion
        #region public

        [HttpPost]
        [Route("available")]
        public IHttpActionResult CheckLocation(LocationModel model)
        {
            var uniqName = model.Tank + model.Canister.ToString() + model.Cane + model.Position.ToString();
            var location = SampleManager.GetLocation(uniqName);
            return Ok(location);
        }

        [HttpPost]
        [Route("")]
        public SampleModel SaveSample(SaveSampleModel model)
        {
            var userId = this.GetUserId();
            return SampleManager.SaveSample(model, userId);
        }

        [HttpGet]
        [Route("{id:int}")]
        public SampleModel GetSample(Int32 id)
        {
            return SampleManager.GetSample(id);
        }

        [HttpGet]
        [Route("")]
        public PagedResult<SampleBriefModel> GetSamples()
        {
            var query = new PagedQuery(Request.RequestUri.ParseQueryString());
            return SampleManager.GetSamples(query);
        }

        [HttpDelete]
        [Route("{id:int}")]
        public IHttpActionResult DeleteSample(Int32 id)
        {
            SampleManager.DeleteSample(id);
            return Ok();
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
