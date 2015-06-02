using System;
using System.Collections.Generic;
using System.Linq;
using System.Net;
using System.Net.Http;
using System.Web.Http;
using Seaman.Core;

namespace Seaman.Web.Controllers
{
    public class SampleController : ApiController
    {
        #region Constructor
        public SampleController(Lazy<ISampleManager> sampleManagerLazy)
        {
            _sampleManagerLazy = sampleManagerLazy;
        }
        #endregion

        

        #region Privat
        private readonly Lazy<ISampleManager> _sampleManagerLazy;

        private ISampleManager DocumentManager
        {
            get
            {
                return _sampleManagerLazy.Value;
            }
        }
        #endregion
    }
}
