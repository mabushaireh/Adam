using i2fam.DAL.Core;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace i2fam.DAL.Models
{
    public class LookupItem
    {
        public int Id { get; set; }
        public LocaleCategory CategoryId { get; set; }
        public Language Lang { get; set; }
        public int LocaleId { get; set; }
        public string LocaleString { get; set; }
    }
}
