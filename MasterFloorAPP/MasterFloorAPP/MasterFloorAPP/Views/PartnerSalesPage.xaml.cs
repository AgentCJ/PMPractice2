using MasterFloorAPP.ViewModels;

namespace MasterFloorAPP;

[QueryProperty(nameof(PartnerId), "partnerId")]
[QueryProperty(nameof(PartnerName), "partnerName")]
public partial class PartnerSalesPage : ContentPage
{
    private string? _partnerId;
    private string? _partnerName;

    public string? PartnerId
    {
        get => _partnerId;
        set
        {
            _partnerId = value;
            if (!string.IsNullOrEmpty(value) && int.TryParse(value, out int id) && !string.IsNullOrEmpty(PartnerName))
                BindingContext = new PartnerSalesViewModel(id, PartnerName);
        }
    }

    public string? PartnerName
    {
        get => _partnerName;
        set
        {
            _partnerName = value;
            if (!string.IsNullOrEmpty(value) && !string.IsNullOrEmpty(PartnerId) && int.TryParse(PartnerId, out int id))
                BindingContext = new PartnerSalesViewModel(id, value);
        }
    }

    public PartnerSalesPage()
    {
        InitializeComponent();
    }
}