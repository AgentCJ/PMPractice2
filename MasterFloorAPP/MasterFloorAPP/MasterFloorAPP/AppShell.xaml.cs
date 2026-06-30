namespace MasterFloorAPP
{
    public partial class AppShell : Shell
    {
        public AppShell()
        {
            InitializeComponent();
            Routing.RegisterRoute(nameof(AddEditPartnerPage), typeof(AddEditPartnerPage));
            Routing.RegisterRoute(nameof(PartnerSalesPage), typeof(PartnerSalesPage));
        }
    }
}
