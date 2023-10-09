using Solitons.Collections.Specialized;

namespace UsageExamples.Collections.Specialized;

[Example]
public sealed class ExampleModelingSoftwareProject
{
    public void Example()
    {
        var project = new ProjectActivityCollection();

        // Initial Project Setup [1]
        var projectKickOff = project.Add("Project Kick-off", 5);

        // Infrastructure as Code (IaC) on Azure [4]
        var setupAzureResourceGroup = project.Add("Setup Azure Resource Group", 15, projectKickOff);
        var setupVNet = project.Add("Setup VNet", 20, setupAzureResourceGroup);
        var setupStorage = project.Add("Setup Storage", 25, setupAzureResourceGroup);
        var setupKubernetesCluster = project.Add("Setup Kubernetes Cluster", 30, setupVNet);

        // Docker Stack [4]
        var setupBaseImages = project.Add("Setup Base Docker Images", 15, projectKickOff);
        var dockerizeWebApp = project.Add("Dockerize Web Application", 40, setupBaseImages);
        var dockerizeAPI = project.Add("Dockerize REST API", 50, setupBaseImages);
        var dockerizeSpark = project.Add("Dockerize Spark Workflows", 60, setupBaseImages);

        // Deploy to Kubernetes [4]
        var deployWebApp = project.Add("Deploy Web App", 30, dockerizeWebApp, setupKubernetesCluster);
        var deployAPI = project.Add("Deploy REST API", 35, dockerizeAPI, setupKubernetesCluster);
        var deploySpark = project.Add("Deploy Spark Workflows", 40, dockerizeSpark, setupKubernetesCluster);
        var deployMonitoring = project.Add("Deploy Monitoring Tools", 25, setupKubernetesCluster);

        // Data Modeling and Database Setup [4]
        var defineDataModels = project.Add("Define Data Models", 35, projectKickOff);
        var setupDatabase = project.Add("Setup Database", 25, defineDataModels, setupAzureResourceGroup);
        var dataMigration = project.Add("Data Migration", 30, setupDatabase);
        var setupCaching = project.Add("Setup Caching Layer", 20, setupAzureResourceGroup);

        // Web UX [3]
        var uxDesign = project.Add("UX Design", 60, projectKickOff);
        var uxImplementation = project.Add("UX Implementation", 80, uxDesign);
        var uxIntegration = project.Add("UX Backend Integration", 40, uxImplementation, deployWebApp, deployAPI);

        // API Development [4]
        var apiDesign = project.Add("API Design", 30, projectKickOff);
        var apiImplementation = project.Add("API Implementation", 60, apiDesign);
        var apiDocumentation = project.Add("API Documentation", 15, apiImplementation);
        var apiSecurity = project.Add("API Security Implementation", 20, apiImplementation);

        // IoT Development [4]
        var iotPrototype = project.Add("IoT Prototyping", 40, projectKickOff);
        var iotDataModel = project.Add("IoT Data Modeling", 25, iotPrototype);
        var iotDataIngestion = project.Add("IoT Data Ingestion Setup", 30, iotDataModel, deploySpark);
        var iotSecurity = project.Add("IoT Security Measures", 20, iotDataIngestion);

        // Spark Workflows [3]
        var sparkJobDesign = project.Add("Spark Job Design", 35, defineDataModels);
        var sparkJobImplementation = project.Add("Spark Job Implementation", 50, sparkJobDesign);
        var sparkOptimization = project.Add("Spark Job Optimization", 30, sparkJobImplementation);

        // Testing and Quality Assurance (QA) [4]
        var unitTesting = project.Add("Unit Testing", 30, apiImplementation, uxImplementation);
        var integrationTesting = project.Add("Integration Testing", 35, deployWebApp, deployAPI, deploySpark);
        var performanceTesting = project.Add("Performance Testing", 25, deployMonitoring);
        var securityTesting = project.Add("Security Testing", 30, apiSecurity, iotSecurity);

        // Final Activities [2]
        var clientTraining = project.Add("Client Training", 20, uxIntegration, apiDocumentation);
        var systemTest = project.Add("System Test", 50, unitTesting, integrationTesting, performanceTesting, securityTesting);

        // Calculate Critical Path
        var criticalPath = project.GetCriticalPath();
        Console.WriteLine("Critical Path Activities:");
        foreach (var activity in criticalPath)
        {
            Console.WriteLine($"{activity.ActivityId} - {activity.EffortInDays} days - Start: {activity.StartDate} - End: {activity.EndDate}");
        }
    }
}