using System;
using Microsoft.Xrm.Sdk;

namespace PluginsDemo
{
    public class PostOperationAccount : IPlugin
    {
        public void Execute(IServiceProvider serviceProvider)
        {
            // Obtain the execution context from the service provider.  
            IPluginExecutionContext context = (IPluginExecutionContext)
                serviceProvider.GetService(typeof(IPluginExecutionContext));


            if (context.MessageName != "Create")
                return;

            // The InputParameters collection contains all the data passed in the message request.  
            if (context.InputParameters.Contains("Target") &&
                context.InputParameters["Target"] is Entity)
            {
                // Obtain the target entity from the input parameters.  
                Entity entity = (Entity)context.InputParameters["Target"];

                // Verify that the target entity represents an entity type you are expecting.   
                // For example, an account. If not, the plug-in was not registered correctly.  
                if (entity.LogicalName != "account")
                    return;


                // Obtain the organization service reference which you will need for  
                // web service calls.  
                IOrganizationServiceFactory serviceFactory =
                    (IOrganizationServiceFactory)serviceProvider.GetService(typeof(IOrganizationServiceFactory));

                IOrganizationService service = serviceFactory.CreateOrganizationService(context.UserId);

                try
                {
                    // Plug-in business logic goes here.  
                    if (context.Stage == 40)
                    {
                       // string getValue = context.SharedVariables["CALSTRING"].ToString();

                        Entity updatedAccount = new Entity("account");
                        updatedAccount.Id = entity.Id;
                        updatedAccount.Attributes["esciption"] = "abc";
                        service.Update(updatedAccount);

                    }

                }

                catch (Exception ex)
                {

                    throw new InvalidPluginExecutionException();

                }
            }
        }
    }
}
