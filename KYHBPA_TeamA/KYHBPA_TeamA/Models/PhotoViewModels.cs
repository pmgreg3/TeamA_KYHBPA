using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Linq;
using System.Web;

namespace KYHBPA_TeamA.Models
{
    public class AddPhotoViewModel
    {
        public string Title { get; set; }
        public string Description { get; set; }
        public string Link { get; set; }

        [DisplayName("Landing Page Carousel")]
        public bool InLandingPageCarousel { get; set; }

        [DisplayName("Partner Org Carousel")]
        public bool InPartnerOrgCarousel { get; set; }

        [DisplayName("Photo Gallery")]
        public bool InPhotoGallery { get; set; }
        public HttpPostedFileBase Image { get; set; } = null;
        public string Credit { get; set; }
    }

    public class DisplayPhotosViewModel
    {
        public int Id { get; set; }
        public byte[] Data { get; set; }
        public string Description { get; set; }
        public string ShorterDescription { get; set; }
        public string Title { get; set; }
        public DateTime Date { get; set; }
        public bool InLandingPageCarousel { get; set; }
        public bool InPartnerOrgCarousel { get; set; }
        public bool InPhotoGallery { get; set; }
        public string Credit { get; set; }
        public string Link { get; set; }
    }

    public class EditPhotoViewModel : AddPhotoViewModel
    {
        public int Id { get; set; }
        public DateTime Date { get; set; }
    }

    public class DisplayPartnerOrgViewModel: DisplayPhotosViewModel
    {
    }

    public class PhotoGalleryViewModel
    {
        public List<DisplayPhotosViewModel> Photos { get; set; }
        public List<Event> Events { get; set; }
    }

    public class PartnerOrgSlide
    {
        public List<DisplayPartnerOrgViewModel> PartnerPhotos { get; set; } = new List<DisplayPartnerOrgViewModel>();
    }

    public class PartnerOrgSlides
    {
        public List<PartnerOrgSlide> Slides { get; set; }
    }
}