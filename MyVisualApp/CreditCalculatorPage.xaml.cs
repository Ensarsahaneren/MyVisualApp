using System;
using System.Globalization;
using Microsoft.Maui.Controls;

namespace MyVisualApp.Pages
{
    public partial class CreditCalculatorPage : ContentPage
    {
        bool _suppressTermEvents;

        public CreditCalculatorPage()
        {
            InitializeComponent();

            // Picker'a kredi türlerini ekle
            LoanTypePicker.ItemsSource = new[]
            {
                "İhtiyaç Kredisi",
                "Taşıt Kredisi",
                "Konut Kredisi",
                "Ticari Kredisi"
            };

            // Başlangıçta vade = 1 ay
            TermSlider.Value = 1;
            TermEntry.Text = "1";
        }

        // Slider değiştiğinde Entry'yi güncelle
        void TermSlider_ValueChanged(object sender, ValueChangedEventArgs e)
        {
            if (_suppressTermEvents) return;

            _suppressTermEvents = true;
            TermEntry.Text = ((int)e.NewValue).ToString();
            _suppressTermEvents = false;
        }

        // Entry değiştiğinde Slider'ı güncelle
        void TermEntry_TextChanged(object sender, TextChangedEventArgs e)
        {
            if (_suppressTermEvents) return;

            if (int.TryParse(e.NewTextValue, out int v))
            {
                // 1–120 arasında tut
                v = Math.Max(1, Math.Min(120, v));

                _suppressTermEvents = true;
                TermSlider.Value = v;
                _suppressTermEvents = false;
            }
        }

        // Hesapla tuşuna basıldığında çağrılır
        void OnCalculateClicked(object sender, EventArgs e)
        {
            // 1) Geçerli seçim ve giriş
            if (LoanTypePicker.SelectedItem == null)
            {
                DisplayAlert("Uyarı", "Lütfen bir kredi türü seçin.", "Tamam");
                return;
            }
            if (!double.TryParse(AmountEntry.Text, NumberStyles.Any, CultureInfo.InvariantCulture, out double amount) || amount <= 0)
            {
                DisplayAlert("Uyarı", "Lütfen geçerli bir kredi tutarı girin.", "Tamam");
                return;
            }
            if (!double.TryParse(RateEntry.Text, NumberStyles.Any, CultureInfo.InvariantCulture, out double annualRate) || annualRate <= 0)
            {
                DisplayAlert("Uyarı", "Lütfen geçerli bir faiz oranı girin.", "Tamam");
                return;
            }
            int months = (int)TermSlider.Value;

            // 2) KKDF/BSMV oranları
            double kkdf = 0, bsmv = 0;
            switch (LoanTypePicker.SelectedItem as string)
            {
                case "İhtiyaç Kredisi": kkdf = 0.15; bsmv = 0.10; break;
                case "Taşıt Kredisi": kkdf = 0.15; bsmv = 0.05; break;
                case "Konut Kredisi": kkdf = 0.00; bsmv = 0.00; break;
                case "Ticari Kredisi": kkdf = 0.00; bsmv = 0.05; break;
            }

            // 3) Aylık brüt faiz oranı
            double monthlyRate = ((annualRate + (annualRate * bsmv) + (annualRate * kkdf)) / 100.0);

            // 4) Eşit taksit formülü
            double factor = Math.Pow(1 + monthlyRate, months);
            double payment = factor * monthlyRate / (factor - 1) * amount;
            double totalPay = payment * months;
            double totalIntr = totalPay - amount;

            // 5) Sonuçları güncelle
            MonthlyPaymentLabel.Text = $"Aylık Taksit: {payment:F2} ₺";
            TotalPaymentLabel.Text = $"Toplam Ödeme: {totalPay:F2} ₺";
            TotalInterestLabel.Text = $"Toplam Faiz: {totalIntr:F2} ₺";
        }
    }
}
