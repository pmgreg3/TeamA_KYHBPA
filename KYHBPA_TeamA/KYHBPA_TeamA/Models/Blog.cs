using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Linq;
using System.Web;

namespace KYHBPA_TeamA.Models
{
    public class Post
    {
        public virtual int Id { get; set; }

        public virtual string Title { get; set; }

        [DisplayName("Short Description")]
        public virtual string ShortDescription { get; set; }

        public virtual string Description { get; set; }

        public virtual string Meta { get; set; }

        public virtual string UrlSlug { get; set; }

        public virtual bool Published { get; set; }

        public virtual DateTime PostedOn { get; set; }

        public virtual DateTime? Modified { get; set; }

        public virtual Category Category { get; set; }

        public virtual IList<Tag> Tags { get; set; }
        public virtual List<Comment> Comments { get; set; }
        public byte[] PhotoContent { get; set; }
        public string MimeType { get; set; }
        public byte[] ThumbnailPhotoContent { get; set; }

        [DisplayName("Front Page Feature")]
        public bool FrontPageFeature { get; set; }
    }
}