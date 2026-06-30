using System.Collections.ObjectModel;
using System.ComponentModel;
using System.Runtime.CompilerServices;
using System.Windows.Input;
using MasterFloorAPP.Models;
using MasterFloorAPP.Services;

namespace MasterFloorAPP.ViewModels
{
    public class PartnerSalesViewModel : INotifyPropertyChanged
    {
        private readonly ApiService _apiService;
        private int _partnerId;
        private string _partnerName = string.Empty;
        private ObservableCollection<SaleHistoryItem> _sales = new();
        private bool _isLoading;

        public ObservableCollection<SaleHistoryItem> Sales
        {
            get => _sales;
            set { _sales = value; OnPropertyChanged(); }
        }

        public string PartnerName
        {
            get => _partnerName;
            set { _partnerName = value; OnPropertyChanged(); }
        }

        public bool IsLoading
        {
            get => _isLoading;
            set { _isLoading = value; OnPropertyChanged(); }
        }

        public ICommand LoadSalesCommand { get; }

        public PartnerSalesViewModel(int partnerId, string partnerName)
        {
            _apiService = new ApiService();
            _partnerId = partnerId;
            PartnerName = partnerName;
            LoadSalesCommand = new Command(async () => await LoadSalesAsync());
            LoadSalesCommand.Execute(null);
        }

        private async Task LoadSalesAsync()
        {
            if (IsLoading) return;
            IsLoading = true;
            try
            {
                var sales = await _apiService.GetPartnerSalesAsync(_partnerId);
                if (sales != null)
                {
                    Sales.Clear();
                    foreach (var item in sales)
                        Sales.Add(item);
                }
                else
                {
                    await Application.Current.MainPage.DisplayAlert("Ошибка", "Не удалось загрузить историю продаж", "OK");
                }
            }
            catch (Exception ex)
            {
                await Application.Current.MainPage.DisplayAlert("Ошибка", ex.Message, "OK");
            }
            finally
            {
                IsLoading = false;
            }
        }

        public event PropertyChangedEventHandler? PropertyChanged;
        protected void OnPropertyChanged([CallerMemberName] string? propertyName = null) =>
            PropertyChanged?.Invoke(this, new PropertyChangedEventArgs(propertyName));
    }
}