using System;
using System.Collections.Generic;

namespace PatientPortalService.Api.Data
{
    public partial class HospitalDetail
    {
        public int Id { get; set; }
        public string HospitalName { get; set; }
        public byte[] HospitalLogo { get; set; }
        public bool? IsActive { get; set; }
    }
}
