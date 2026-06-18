using MasterFloorAPP.ViewModels;

namespace MasterFloorAPP
{
    [QueryProperty(nameof(PartnerId), "partnerId")]
    public partial class AddEditPartnerPage : ContentPage
    {
        private string? _partnerId;
        public string? PartnerId
        {
            get => _partnerId;
            set
            {
                _partnerId = value;
                int? id = null;
                if (!string.IsNullOrEmpty(value) && int.TryParse(value, out int parsed))
                    id = parsed;
                BindingContext = new AddEditPartnerViewModel(id);
            }
        }

        public AddEditPartnerPage()
        {
            InitializeComponent();
        }
    }
}