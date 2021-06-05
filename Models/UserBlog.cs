using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;

namespace mvc1.Models
{
    public class UserBlog
    {
        [Required]
        public long Id { get; set; }
        [Required]
        public string User { get; set; }

        //public ICollection<Post> Posts{get; set;}
    }
}