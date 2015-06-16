﻿using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Net;
using System.Net.Http;
using System.Reflection.Emit;
using System.Threading.Tasks;
using System.Web;
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

        [HttpGet]
        [Route("report/{id:int}")]
        public SampleReportModel GetReportSample(Int32 id)
        {
            return SampleManager.GetReportSample(id);
        }

        [HttpPost]
        [Route("report")]
        public List<LocationReportModel> GetReportSamples(ReportModel model)
        {
            return SampleManager.GetReport(model);
        }

        [HttpGet]
        [Route("random/{count:int}")]
        public List<LocationReportModel> GetRandomReport(Int32 count)
        {
            var samples = SampleManager.GetReport(new ReportModel()
            {
                Type = ReportType.Existing.ToString()
            });
            if (samples.Count > count)
            {
                var indexes = new List<Int32>();
                var random = new Random();
                while (indexes.Count < count)
                {
                    var index = random.Next(0, samples.Count);
                    if (indexes.Count == 0 || !indexes.Contains(index))
                    {
                        indexes.Add(index);
                    }
                }

                return indexes.Select(index => samples[index]).ToList();
            }
            return samples;
        }

        [HttpGet]
        [Route("report")]
        public List<SampleReportModel> GetAllReportSamples()
        {
            return SampleManager.GetReportSamples(new List<Int32>());
        }

        [HttpGet]
        [Route("locations/{id:int}")]
        public List<LocationBriefModel> GetLocations(Int32 id)
        {
            return SampleManager.GetLocations(id);
        }

        [HttpDelete]
        [Route("locations/{id:int}")]
        public IHttpActionResult RemoveLocation(Int32 id)
        {
            SampleManager.DeleteLocation(id);
            return Ok();
        }

        [HttpGet]
        [Route("extracted")]
        public List<SampleBriefModel> GetExtrcted()
        {
            return SampleManager.GetExtractedSamples();
        }

        [HttpPost]
        [Route("consent")]
        public async Task<IHttpActionResult> UploadConsentForm()
        {
            if (!Request.Content.IsMimeMultipartContent())
            {
                Request.CreateResponse(HttpStatusCode.UnsupportedMediaType);
                return null;
            }
            var provider = GetMultipartProvider();
            await Request.Content.ReadAsMultipartAsync(provider);
            var sampleIdString = provider.FormData["sampleId"].IfNotNull(it => it.ToLowerInvariant());
            if (String.IsNullOrEmpty(sampleIdString))
            {
                return BadRequest();
            }
            var sampleId = Int32.Parse(sampleIdString);
            var file = provider.FileData[0];
            if (string.IsNullOrEmpty(file.Headers.ContentDisposition.FileName))
            {
                Request.CreateResponse(HttpStatusCode.NotAcceptable, "This request is not properly formatted");
                return BadRequest();
            }
            string fileName = file.Headers.ContentDisposition.FileName;
            if (fileName.StartsWith("\"") && fileName.EndsWith("\""))
            {
                fileName = fileName.Trim('"');
            }
            if (fileName.Contains(@"/") || fileName.Contains(@"\"))
            {
                fileName = Path.GetFileName(fileName);
            }
            fileName = sampleIdString + "_" + fileName;
            var uploadFolder = "/uploads";
            var mappedUploadFolder = HttpContext.Current.Server.MapPath("~" + uploadFolder);
            if (!Directory.Exists(mappedUploadFolder))
            {
                Directory.CreateDirectory(mappedUploadFolder);
            }
            var destFileName = uploadFolder +"/" + fileName;
            var mappedDestFileName = HttpContext.Current.Server.MapPath("~" + destFileName);
            if (File.Exists(mappedDestFileName))
            {
                File.Delete(mappedDestFileName);
            }
            File.Move(file.LocalFileName, mappedDestFileName);
            SampleManager.AddConsentForm(destFileName, sampleId);
            return Ok(new {allowExtract = true});
        }

        #endregion
        #region Privat

        private readonly String _storageFolder = "~/App_Data";
        private String _tempFolder;
        private readonly Lazy<ISampleManager> _sampleManagerLazy;

        private ISampleManager SampleManager
        {
            get
            {
                return _sampleManagerLazy.Value;
            }
        }

        private MultipartFormDataStreamProvider GetMultipartProvider()
        {
            _tempFolder = HttpContext.Current.Server.MapPath(_storageFolder + "/t/");
            if (!Directory.Exists(_tempFolder))
            {
                Directory.CreateDirectory(_tempFolder);
            }
            return new MultipartFormDataStreamProvider(_tempFolder);
        }
        #endregion
    }
}
