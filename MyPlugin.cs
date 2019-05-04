using System;
using Microsoft.Xrm.Sdk;

public class MyPlugin : IPlugin
{
    public void Execute(IServiceProvider serviceProvider)
    {
        // Extract the tracing service for use in debugging sandboxed plug-ins.  
        // If you are not registering the plug-in in the sandbox, then you do  
        // not have to add any tracing service related code.  
        ITracingService tracingService =
            (ITracingService)serviceProvider.GetService(typeof(ITracingService));

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
            }

            catch (Exception ex)
            {
                tracingService.Trace("MyPlugin: {0}", ex.ToString());
                throw;
            }
        }
    }
}