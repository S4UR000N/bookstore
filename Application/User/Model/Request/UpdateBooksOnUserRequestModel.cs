using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Application.User.Model.Request
{
    public class UpdateBooksOnUserRequestModel
    {
        public long UserId { get; set; }
        public long[] AddBooks { get; set; }
        public long[] RemoveBooks { get; set;}
    }
}
