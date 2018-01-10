using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Web;
using System.Web.Mvc;

namespace KYHBPA_TeamA.Models
{
    public class CreateBlogPostViewModel
    {
        public virtual int Id { get; set; }

        public virtual string Title { get; set; }

        [DisplayName("Short Description")]
        public virtual string ShortDescription { get; set; }

        public virtual string Description { get; set; }

        public virtual bool Published { get; set; }

        public virtual DateTime PostedOn { get; set; }

        public virtual DateTime? Modified { get; set; }

        public virtual Category Category { get; set; }

        public virtual IList<Tag> Tags { get; set; }

        public virtual List<Comment> Comments { get; set; }



        public List<CategoryListViewModel> Categories
        {
            get
            {
                return new List<CategoryListViewModel>()
                {
                    new CategoryListViewModel()
                    {
                        Id = 1,
                        Name = "Blog"
                    },
                    new CategoryListViewModel()
                    {
                        Id = 2,
                        Name = "News"
                    }
                };
            }
        }

        [Display(Name = "Category")]
        [Required]
        public int SelectedCategoryId { get; set; }

        public IEnumerable<SelectListItem> CategoryList
        {
            get { return new SelectList(Categories, "Id", "Name"); }

        }

        public HttpPostedFileBase File { get; set; } = null;
        public byte[] PhotoContent { get; set; }
    }

    public class DisplayPostsViewModel
    {
        public int Id { get; set; }

        public string Title { get; set; }

        public string ShortDescription { get; set; }

        public string Description { get; set; }
  
        public bool Published { get; set; }

        public DateTime PostedOn { get; set; }

        public Category Category { get; set; }

        public List<Comment> Comments { get; set; }

        public byte[] PhotoContent { get; set; }

    }
}