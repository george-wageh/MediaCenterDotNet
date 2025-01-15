using Microsoft.AspNetCore.Http;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace MediaCenterCore.ViewModels
{
    public class UploadMediaVaVM
    {
        public IFormFile MediaVa { get; set; }
        public string Token { get; set; }
        public int MediaId { get; set; }
        public string Url { get; set; } = Shared.DomainsNames.HostStaticDomain + "/" + "UploadMediaVa";
    }
}
