using System;
using System.Collections.Generic;

namespace PatientPortalService.Api.Data
{
    public partial class Department
    {
        public Department()
        {
            Doctor = new HashSet<Doctor>();
            PatientInfo = new HashSet<PatientInfo>();
        }

        public int DepartmentId { get; set; }
        public string DepartmentName { get; set; }

        public ICollection<Doctor> Doctor { get; set; }
        public ICollection<PatientInfo> PatientInfo { get; set; }
    }
}
