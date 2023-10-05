using Crito.Models;
using Crito.Services;
using Microsoft.AspNetCore.Mvc;
using Umbraco.Cms.Core.Cache;
using Umbraco.Cms.Core.Logging;
using Umbraco.Cms.Core.Routing;
using Umbraco.Cms.Core.Services;
using Umbraco.Cms.Core.Web;
using Umbraco.Cms.Infrastructure.Persistence;
using Umbraco.Cms.Web.Website.Controllers;

namespace Crito.Controllers
{
    public class ContactController : SurfaceController
    {
        public ContactController(IUmbracoContextAccessor umbracoContextAccessor, IUmbracoDatabaseFactory databaseFactory, ServiceContext services, AppCaches appCaches, IProfilingLogger profilingLogger, IPublishedUrlProvider publishedUrlProvider) : base(umbracoContextAccessor, databaseFactory, services, appCaches, profilingLogger, publishedUrlProvider)
        {
        }

        [HttpPost]
        public IActionResult Index(ContactForm contactForm)
        {
            if (!ModelState.IsValid)
            
                return CurrentUmbracoPage();

            using var mail = new MailService("no-reply@crito.com", "smpt.crito.com", 587, "support@crito.com", "BytMig123!");
            //To Sender
            mail.SendAsync(contactForm.Email, "Your contact request was received", "Hi your message was received to us and we will contact you as soon as possible").ConfigureAwait(false);

            //To Receiver
            mail.SendAsync("support@crito.com", $"{contactForm.Name} sent a contact request", contactForm.Message).ConfigureAwait(false);

            return LocalRedirect(contactForm.RedirectUrl ?? "/");
        }
    }
}
