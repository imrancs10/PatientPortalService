using System;
using System.Collections.Generic;

namespace PatientPortalService.Api.Data
{
    public partial class PatientInfo
    {
        public PatientInfo()
        {
            AppointmentInfo = new HashSet<AppointmentInfo>();
            PatientTransaction = new HashSet<PatientTransaction>();
        }

        public int PatientId { get; set; }
        public string RegistrationNumber { get; set; }
        public string MobileNumber { get; set; }
        public string Password { get; set; }
        public string Email { get; set; }
        public string FirstName { get; set; }
        public string MiddleName { get; set; }
        public string LastName { get; set; }
        public DateTime? Dob { get; set; }
        public string Gender { get; set; }
        public string Address { get; set; }
        public string City { get; set; }
        public string Country { get; set; }
        public int? PinCode { get; set; }
        public string Religion { get; set; }
        public int? DepartmentId { get; set; }
        public string Otp { get; set; }
        public string State { get; set; }
        public byte[] Photo { get; set; }

        public Department Department { get; set; }
        public ICollection<AppointmentInfo> AppointmentInfo { get; set; }
        public ICollection<PatientTransaction> PatientTransaction { get; set; }
    }
}
