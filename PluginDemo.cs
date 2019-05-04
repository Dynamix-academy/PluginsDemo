using System;
using Microsoft.Xrm.Sdk;

namespace PluginsDemo
{
    public class PluginDemo : IPlugin
    {
        public void Execute(IServiceProvider serviceProvider)
        {
            ITracingService tracingService = (ITracingService)
                        serviceProvider.GetService(typeof(ITracingService));

            IPluginExecutionContext context = (IPluginExecutionContext)
                serviceProvider.GetService(typeof(IPluginExecutionContext));


            if (context.InputParameters.Contains("Target") &&
                context.InputParameters["Target"] is Entity)
            {
                Entity entity = (Entity)context.InputParameters["Target"];

                if (entity.LogicalName != "account")
                    return;

                IOrganizationServiceFactory serviceFactory =
                    (IOrganizationServiceFactory)serviceProvider.GetService(typeof(IOrganizationServiceFactory));

                IOrganizationService service = serviceFactory.CreateOrganizationService(context.UserId);

                try
                {
                    Entity newContact = new Entity("contact");
                    newContact.Attributes.Add("firstname", "ABC");
                    newContact.Attributes.Add("lastname", "XYZ");
                    newContact.Attributes.Add("parentcustomerid", entity.ToEntityReference());
                    service.Create(newContact);
                    //throw new InvalidPluginExecutionException("Exception");
                }

                catch (Exception ex)
                {

                    tracingService.Trace("An Errror Occured " + ex.Message);

                }
            }
        }
    }
}
