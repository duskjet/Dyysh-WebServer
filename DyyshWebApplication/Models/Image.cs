using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace DyyshWebApplication.Models
{
    public class Image
    {
        public int Id { get; set; }
        public string ThumbnailPath { get; set; }
        public string Path { get; set; }
        public int FileSize { get; set; }
        public virtual ApplicationUser Owner { get; set; }
    }
}