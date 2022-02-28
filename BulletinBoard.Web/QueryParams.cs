using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace BulletinBoard.Web
{
    public class QueryParams
    {
        public QueryParams()
        {
            ShowPhotos = false;
            ShowDescription = false;
        }

        public bool ShowPhotos { get; set; }
        public bool ShowDescription { get; set; }
    }
}
