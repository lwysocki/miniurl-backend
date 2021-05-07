using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace MiniUrl.ApiGateway.Web.Settings
{
    public class GrpcUrls
    {
        public const string Section = "GrpcUrls";

        public string AssociationService { get; set; }
        public string UrlService { get; set; }
    }
}
