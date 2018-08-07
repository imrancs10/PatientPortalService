using System;
using System.Collections.Generic;

namespace PatientPortalService.Api.Data
{
    public partial class MeridiemMaster
    {
        public MeridiemMaster()
        {
            DoctorScheduleTimeFromMeridiem = new HashSet<DoctorSchedule>();
            DoctorScheduleTimeToMeridiem = new HashSet<DoctorSchedule>();
        }

        public int MeridiemId { get; set; }
        public string MeridiemValue { get; set; }

        public ICollection<DoctorSchedule> DoctorScheduleTimeFromMeridiem { get; set; }
        public ICollection<DoctorSchedule> DoctorScheduleTimeToMeridiem { get; set; }
    }
}
