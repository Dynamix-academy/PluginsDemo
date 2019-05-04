using System;
using Microsoft.Xrm.Sdk;
using Microsoft.Xrm.Sdk.Client;
using Microsoft.Xrm.Sdk.Query;

namespace PluginsDemo
{
    public class PreOperationAccount : IPlugin
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


                Entity elevatedUser = GetUser((OrganizationServiceProxy)service, "admin@dynamixacademytrial.onmicrosoft.com");

                IOrganizationService elevatedService = serviceFactory.CreateOrganizationService(elevatedUser.Id);

                try
                {
                    // Plug-in business logic goes here.  
                    if (context.Stage == 20)
                    {

                     var calString =   entity.Attributes["name"];
                        calString = calString + " - " + DateTime.Now.ToShortDateString() + " - " + "Pre";

                        context.SharedVariables.Add("CALSTRING", calString);
                    }

                }

                catch (Exception ex)
                {


                }
            }
        }

        public static Entity GetUser(OrganizationServiceProxy orgService, string domainName)
        {
            QueryExpression queryUser = new QueryExpression
            {
                EntityName = "systemuser",
                ColumnSet = new ColumnSet("systemuserid", "businessunitid", "domainname"),
                Criteria = new FilterExpression
                {
                    Conditions =
                    {
                        new ConditionExpression
                        {
                            AttributeName = "domainname",
                            Operator = ConditionOperator.Equal,
                            Values = {domainName}
                        }
                    }
                }
            };

            EntityCollection results = orgService.RetrieveMultiple(queryUser);
            return results.Entities[0];
        }
    }

}
