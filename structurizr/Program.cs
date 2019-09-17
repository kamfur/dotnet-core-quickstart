using Structurizr;
using Structurizr.Api;
using Structurizr.Documentation;

namespace structurizr
{
    
    /// <summary>
    /// This is a simple example of how to get started with Structurizr for .NET.
    /// </summary>
    class Program
    {

        private const long WorkspaceId = 46661;
        private const string ApiKey = "4e6ff25c-a4fa-4e4f-9cb2-3633bac26323";
        private const string ApiSecret = "caa32198-39a5-4965-8988-84581ada4622";

        
        static void AddContext(Model model1, ViewSet viewSet)
        {
           
        }
        static void Main(string[] args)
        {
            

            Workspace workspace = new Workspace("Park Soft", "Software for parking system");
            Model model = workspace.Model;
            ViewSet views = workspace.Views;
            
            AddContext(model, views);
            model.Enterprise = new Enterprise("System parkingowy");
            Person customer = model.AddPerson(
                Location.External,
                "Klient",
                "Klient parkingu, " +
                "posiada konto w systemie");
            
            Person parkingOperator = model.AddPerson(
                Location.External,
                "Operator",
                "Operator parkingu, " +
                "posiada konto w systemie");

            SoftwareSystem clientPlatformSystem = model.AddSoftwareSystem(
                Location.Internal,
                "Platforma kliencka",
                "Pozwala na zarządzanie swoimi rezerwacjami");
            customer.Uses(clientPlatformSystem, "Używa");

            SoftwareSystem mainframeParkingSystem = model.AddSoftwareSystem(
                Location.Internal,
                "System parkingowy",
                "Core'owy system parkingowy.");
            
            clientPlatformSystem.Uses(
                mainframeParkingSystem,
                "Pobiera informacje/Wysyła informacje");
            parkingOperator.Uses(mainframeParkingSystem, "obsługuje");
            
            SoftwareSystem emailSystem = model.AddSoftwareSystem(Location.Internal,
            "E-mail System", "System e-mailowy Microsoft Exchange");
            clientPlatformSystem.Uses(emailSystem, "Wysyła e-maile używając");
            emailSystem.Delivers(customer, "Wysyła e-maile do");
            clientPlatformSystem.Uses(
                mainframeParkingSystem,
                "Pobiera informacje/Wysyła informacje");

            
            
            SystemContextView systemContextView = views.CreateSystemContextView(
                clientPlatformSystem,
                "SystemContext",
                "Diagram kontekstowy systemu parkingowego");
            systemContextView.AddNearestNeighbours(clientPlatformSystem);
            systemContextView.AddAnimation(clientPlatformSystem);
            systemContextView.AddAnimation(customer);
            //systemContextView.AddAnimation(parkingOperator);
            systemContextView.AddAnimation(mainframeParkingSystem);
            
            //wcześniej zdefiniowany kontekst
            Container mobileApp = mainframeParkingSystem.AddContainer(
                "Aplikacja mobilna",
                "Dostarcza ograniczoną funkcjonalność bankowości online dla klienta",
            "Xamarin");
            Container apiApplication = mainframeParkingSystem.AddContainer(
                "API",
                "Dostarcza funkcjonalność bankowości online poprzez JSON/HTTP",
            "Java i Spring MVC");
            Container database = mainframeParkingSystem.AddContainer(
                "Baza danych", "Informacje o klientach, hashowane hasła, logi",
            "Relacyjna baza danych");
            parkingOperator.Uses(mobileApp, "Używa", "");
            apiApplication.Uses(database,
                "Czyta/Zapisuje",
                "JDBC");
            apiApplication.Uses(mainframeParkingSystem,
                "Używa",
                "XML/HTTPS");
            apiApplication.Uses(emailSystem, "Wysyła maile", "SMTP");
            
            
            ContainerView containerView = views.CreateContainerView(
                clientPlatformSystem,
                "Containers",
                "Diagram kontenerów systemu Platformy Bankowowości Online");
            containerView.Add(customer);
            containerView.AddAllContainers();
            containerView.Add(mainframeParkingSystem);
            containerView.Add(emailSystem);
            containerView.AddAnimation(
                customer,
                mainframeParkingSystem,
                emailSystem);
//            containerView.AddAnimation(mobileApp);
//            containerView.AddAnimation(apiApplication);


            UploadWorkspaceToStructurizr(workspace);
        }

   

        private static void UploadWorkspaceToStructurizr(Workspace workspace) {
            StructurizrClient structurizrClient = new StructurizrClient(ApiKey, ApiSecret);
            structurizrClient.PutWorkspace(WorkspaceId, workspace);
        }

    }

}