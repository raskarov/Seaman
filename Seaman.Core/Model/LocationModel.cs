using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Seaman.Core.Model
{
    public class LocationModel : LocationBase
    {
        public Int32 SampleId { get; set; }
    }

    public class LocationBase : NamedBase
    {
        public Boolean Available { get; set; }
        public Boolean Extracted { get; set; }
        public String Tank { get; set; }
        public Int32 Canister { get; set; }
        public String CaneLetter { get; set; }
        public String CaneColor { get; set; }
        public String Position { get; set; }
        public String[] PosForShow { get; set; }

        public String UniqName { get; set; }
        public String SpecimenNumber { get; set; }
        public DateTime? DateStored { get; set; }
        public DateTime? DateExtracted { get; set; }
        public Int32? CollectionMethodId { get; set; }
        public Int32? ExtractReasonId { get; set; }
    }

    public class LocationBriefModel
    {
        public Int32 Id { get; set; }
        public String UniqName { get; set; }
        public String DateStored { get; set; }
        public String DateExtracted { get; set; }
        public Int32? CollectionMethodId { get; set; }
        public String CollectionMethod { get; set; }
        public String ReasonForExtraction { get; set; }
    }

    public class LocationReportModel
    {
        public Int32 Id { get; set; }
        public String UniqName { get; set; }
        public String DateStored { get; set; }
        public String CollectionMethod { get; set; }
        public String DepositorFullName { get; set; }
        public String DepositorDob { get; set; }
        public String Physician { get; set; }

        public String Extracted { get; set; }
        public String DateExtracted { get; set; }
    }

    public class ExtractLocationsModel
    {
        public List<Int32> LocationIds { get; set; }
        public Int32 SampleId { get; set; }
        public String ConsentFormName { get; set; }
        public Int32? ReasonId { get; set; }
    }
}
