using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace Squadfree.Models
{
    public class Testimonial
    {
        public int Id { get; set; }
        public string Avatar { get; set; }
        public string Name { get; set; }
        public string Role { get; set; }
        public string Comment { get; set; }
    }
}
