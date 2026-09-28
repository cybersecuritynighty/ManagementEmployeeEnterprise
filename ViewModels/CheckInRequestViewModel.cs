using System.ComponentModel.DataAnnotations;

namespace MEE.ViewModels
{
    public class CheckInRequestViewModel
    {
        [Required]
        public decimal LocationLat { get; set; }
        
        [Required]
        public decimal LocationLon { get; set; }
        
        [Required]
        public string CapturedDeviceId { get; set; }
        
        [Required]
        public string CheckInMethod { get; set; } // 'QR' or 'GPS'
    }
}
