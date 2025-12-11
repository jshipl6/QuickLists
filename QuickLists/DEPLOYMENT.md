# QuickLists Azure App Service deployment guide  
Week sixteen simulated deployment

> Note  
> I have exhausted my Azure student credits, so I did not press the final Publish button.  
> This document walks through the exact steps I would use so that the project can be deployed later without changes.

## Prerequisites

* An Azure account that has permission to create resources  
* Visual Studio twenty twenty two with the ASP NET and web workload installed  
* The QuickLists project builds and runs locally in Development  
* A production ready connection string available for either SQLite or SQL Server  
  Secrets should be stored outside source control, for example in user secrets or in the Azure portal

## One Create a resource group

1. Sign in to the Azure portal  
2. Open **Resource groups** and choose **Create**  
3. Use a name such as `QuickListsRG`  
4. Choose the region closest to your users  
5. Leave the rest of the defaults and create the group

## Two Create an App Service

1. In the portal search for **App Services** and choose **Create**  
2. Select the subscription and the resource group created above  
3. Enter an app name such as `quicklistsprod`  
4. Publish type should be **Code**  
5. Runtime stack should be **NET eight LTS**  
6. Choose your region again  
7. For pricing select a free or basic tier that supports NET  
8. Review and create the App Service

## Three Configure app settings and connection strings

1. After the App Service is created open it in the portal  
2. Go to **Configuration**  
3. Under **Application settings** add a setting named `ASPNETCORE_ENVIRONMENT` with value `Production`  
4. Under **Connection strings** add a new entry  
   * Name `QuickLists`  
   * Type `SQLServer` or `Custom` depending on the database  
   * Value is your production connection string  
   * Mark it as a connection string so that ASP NET can read it  
5. Save the configuration and allow the app to restart

## Four Prepare the project for Production

1. Add `appsettings.Production.json` to the project if it does not already exist  
2. Put only non secret configuration here, for example  
   * Logging levels  
   * Feature flags  
3. Confirm that the real production connection string is **not** in source control  
   It should come from the connection string in the Azure portal

## Five Publish from Visual Studio

1. Open the QuickLists solution in Visual Studio  
2. Right click the **QuickLists** project and choose **Publish**  
3. In the target selection choose **Azure** then **Azure App Service**  
4. Choose **Select Existing** and sign in with the same Azure account if needed  
5. Pick the subscription, resource group, and App Service created earlier  
6. Visual Studio will create a publish profile  
7. Click **Publish**  
   Visual Studio will build the project, package it, and send it to the App Service  

Because this is a simulated deployment, I would stop here and not actually send the build if no credits are left.  


## Six Verify the site and health endpoint

When a real deployment is run the steps would be

1. Open the App Service in the portal and copy the default URL, something like  

   `https://quicklistsprod.azurewebsites.net`  

2. In a browser visit the root URL and confirm that the QuickLists home page loads  
3. Visit the health endpoint  

   `https://quicklistsprod.azurewebsites.net/healthz`  

4. Confirm that the response contains a clear status such as  

   ```json
   { "status": "Healthy" }