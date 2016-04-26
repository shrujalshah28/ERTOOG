using System.Web.Mvc;

namespace ErtoOfGujarat.Areas.License
{
    public class LicenseAreaRegistration : AreaRegistration 
    {
        public override string AreaName 
        {
            get 
            {
                return "License";
            }
        }

        public override void RegisterArea(AreaRegistrationContext context) 
        {
            context.MapRoute(
                "License_default",
                "License/{controller}/{action}/{id}",
                new { action = "Index", id = UrlParameter.Optional }
            );
        }
    }
}