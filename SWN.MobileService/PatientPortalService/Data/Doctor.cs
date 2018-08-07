using System;
using System.Collections.Generic;

namespace PatientPortalService.Api.Data
{
    public partial class Doctor
    {
        public Doctor()
        {
            AppointmentInfo = new HashSet<AppointmentInfo>();
            DoctorSchedule = new HashSet<DoctorSchedule>();
        }

        public int DoctorId { get; set; }
        public string DoctorName { get; set; }
        public int DepartmentId { get; set; }

        public Department Department { get; set; }
        public ICollection<AppointmentInfo> AppointmentInfo { get; set; }
        public ICollection<DoctorSchedule> DoctorSchedule { get; set; }
    }
}
