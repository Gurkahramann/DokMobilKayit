
using DockMobile.ApiClient;
using DockMobile.ApiClient.Models.ApiModels;
using System.Collections.ObjectModel;
using System.Net.Http.Json;

namespace DockMobile.Maui
{
    public partial class MainPage : ContentPage
    {

        public ObservableCollection<string> PartiNumaralari { get; set; }
        private readonly ApiClientService _apiClientService;
        private DokPartiDetay _detail;
        private DokPartiAna _DockHareket;
        public MainPage(ApiClientService apiClientService)
        {
            InitializeComponent();
            PartiNumaralari = new ObservableCollection<string>();
            BindingContext = this;
            DockNumaraEntry.IsEnabled = true;
            DockNumaraEntry.Focus();
            _apiClientService = apiClientService;
        }

        private async void OnDockNumaraCompleted(object sender, EventArgs e)
        {
            try
            {
                var dockNumara = DockNumaraEntry.Text;

                // Dock numarası boş girildiğinde hata mesajı göster
                if (string.IsNullOrEmpty(dockNumara))
                {
                    await DisplayAlert("Hata", "Dock numarası boş bırakılamaz", "Tamam");
                    DockNumaraEntry.Focus(); // Dock numarası girişine odaklan
                    return;
                }
                // Dock numarası bulundu, parti numarası girişine geç
                DockNumaraEntry.IsEnabled = false; // Dock numarasını kilitle
                PartiNumaraEntry.IsEnabled = true; // Parti numarası girişini aç
                PartiNumaraEntry.Focus(); // Parti numarası girişine odaklan

            }
            catch (HttpRequestException ex)
            {
                Console.WriteLine($"Connection failure: {ex.Message}");
                await DisplayAlert("Error", $" {ex.Message} Unable to connect to the server. Please check your internet connection and try again.", "OK");
            }
            catch (Exception ex)
            {
                Console.WriteLine($"An unexpected error occurred: {ex.Message}");
                await DisplayAlert("Error", "An unexpected error occurred. Please try again.", "OK");
            }
        }
        private async void OnPartiNumaraCompleted(object sender, EventArgs e)
        {
            try
            {
                var partiNumara = PartiNumaraEntry.Text;
                // Dock numarası boş girildiğinde hata mesajı göster
                if (string.IsNullOrEmpty(partiNumara))
                {
                    await DisplayAlert("Hata", "Parti numarası boş bırakılamaz", "Tamam");
                    PartiNumaraEntry.Focus(); // Dock numarası girişine odaklan
                    return;
                }
                if (!string.IsNullOrEmpty(partiNumara))
                {
                    // Partinin daha önce eklenip eklenmediğini kontrol et
                    if (!PartiNumaralari.Contains(partiNumara))
                    {
                        PartiNumaralari.Add(partiNumara); // ObservableCollection güncellendi
                        PartiNumaraEntry.Text = string.Empty; // Parti numarası girişini temizle
                        PartiNumaraEntry.Focus(); // Parti numarası girişine odaklan
                    }
                    else
                    {
                        await DisplayAlert("Bilgi", "Parti numarası zaten mevcut", "Tamam");
                        PartiNumaraEntry.Text = string.Empty;
                        PartiNumaraEntry.Focus();
                        return;
                    }
                }
            }
            catch(HttpRequestException ex)
            {
                Console.WriteLine($"Connection failure: {ex.Message}");
                await DisplayAlert("Error", $" {ex.Message} Unable to connect to the server. Please check your internet connection and try again.", "OK");
            }
            catch (Exception ex)
            {
                Console.WriteLine($"An unexpected error occurred: {ex.Message}");
                await DisplayAlert("Error", "An unexpected error occurred. Please try again.", "OK");
            }
        }
        private async Task OnDeleteButtonClicked(object sender, EventArgs e)
        {
         
            try
            {
                var button = sender as Button;
                var partiNumara = button?.CommandParameter as string;
                if (partiNumara != null)
                {
                    PartiNumaralari.Remove(partiNumara); // Liste öğesini sil
                    PartyNumberListView.ItemsSource = null;
                    PartyNumberListView.ItemsSource = PartiNumaralari;
                }
            }
            catch(Exception ex)
            {
                Console.WriteLine($"An unexpected error occurred: {ex.Message}");
                await DisplayAlert("Error", "An unexpected error occurred. Please try again.", "OK");
            }
        }

        private async void OnKaydetClicked(object sender, EventArgs e)
        {
            try
            {
                // Yeni dock numarası oluştur
                await Navigation.PushModalAsync(new LoadingPage());
                var dockNumara = DockNumaraEntry.Text;

                // Dock numarası boş girildiğinde hata mesajı göster
                if (string.IsNullOrEmpty(dockNumara))
                {
                    await DisplayAlert("Hata", "Dock numarası boş bırakılamaz", "Tamam");
                    DockNumaraEntry.Focus(); // Dock numarası girişine odaklan
                    return;
                }
                if (PartiNumaralari.Count == 0)
                {
                    await DisplayAlert("Hata", "Parti Numarası Eklenmeden Kayıt Yapılamaz", "Tamam");
                    return;
                }
                // Yeni DockHareket nesnesi oluştur ve veritabanına ekle
                var newDockHareket = new DokPartiAna
                {
                    DokNo = dockNumara,
                    Tarih = DateTime.Now
                };

                // Dock bilgilerini veritabanına ekle ve kaydedilen dock bilgilerini al
                _DockHareket = await _apiClientService.SaveDock(newDockHareket);

                if (_DockHareket == null)
                {
                    await DisplayAlert("Hata", "Yeni dock oluşturulamadı veya dock bilgileri alınamadı", "Tamam");
                    DockNumaraEntry.Text = string.Empty;
                    DockNumaraEntry.Focus(); // Dock numarası girişine odaklan
                    return;
                }
                foreach (var partiNumara in PartiNumaralari)
                {
                    var newPartyDetail = new DokPartiDetay
                    {
                        PartiNo = partiNumara,
                        DokPartiAnaId = _DockHareket.Id,
                        Tarih = DateTime.Now
                    };

                    var savePartyResponse = await _apiClientService.SaveParty(newPartyDetail);
                    if (!savePartyResponse.IsSuccessStatusCode)
                    {
                        await DisplayAlert("Hata", $"Parti numarası {partiNumara} eklenemedi", "Tamam");
                    }
                }

                await DisplayAlert("Başarılı", "Veriler kaydedildi", "Tamam");
                ResetPage();
            }
            catch (HttpRequestException ex)
            {
                Console.WriteLine($"Connection failure: {ex.Message}");
                await DisplayAlert("Error", $" {ex.Message} Unable to connect to the server. Please check your internet connection and try again.", "OK");
            }
            catch (Exception ex)
            {
                Console.WriteLine($"An unexpected error occurred: {ex.Message}");
                await DisplayAlert("Error", "An unexpected error occurred. Please try again.", "OK");
            }
            finally
            {
                 await Navigation.PopModalAsync();
            }
        }

        private void OnSifirlaClicked(object sender,EventArgs e)
        {
            ResetPage();    
        }
        private void OnCikisClicked(object sender, EventArgs e)
        {
            Application.Current.Quit();
        }
        private void ResetPage()
        {
            DockNumaraEntry.Text = string.Empty;
            PartiNumaraEntry.Text = string.Empty;
            PartiNumaralari.Clear();

            DockNumaraEntry.IsEnabled = true;
            DockNumaraEntry.Focus();

            PartiNumaraEntry.IsEnabled = false;
            _DockHareket = new DokPartiAna();
        }
    }
}