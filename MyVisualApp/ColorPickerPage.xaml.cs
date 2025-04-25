using System;
using Microsoft.Maui.Controls;       // ContentPage, Clipboard
using Microsoft.Maui.Graphics;       // Color

namespace MyVisualApp.Pages
{
    public partial class ColorPickerPage : ContentPage
    {
        public ColorPickerPage()
        {
            InitializeComponent();
            // Baþlangýç rengi
            UpdateColor();
        }

        void OnColorChanged(object sender, ValueChangedEventArgs e)
        {
            UpdateColor();
        }

        void UpdateColor()
        {
            // Slider deðerlerini al
            int r = (int)RedSlider.Value;
            int g = (int)GreenSlider.Value;
            int b = (int)BlueSlider.Value;

            // Etiketleri güncelle
            RedLabel.Text = r.ToString();
            GreenLabel.Text = g.ToString();
            BlueLabel.Text = b.ToString();

            // Hex kodu oluþtur
            string hex = $"#{r:X2}{g:X2}{b:X2}";
            HexLabel.Text = hex;

            // Arka planý deðiþtir
            BackgroundColor = Color.FromRgb(r, g, b);
        }

        async void OnCopyClicked(object sender, EventArgs e)
        {
            string hex = HexLabel.Text;
            await Clipboard.Default.SetTextAsync(hex);
            await DisplayAlert("Kopyalandý", hex, "OK");
        }

        void OnRandomClicked(object sender, EventArgs e)
        {
            var rnd = new Random();
            RedSlider.Value = rnd.Next(256);
            GreenSlider.Value = rnd.Next(256);
            BlueSlider.Value = rnd.Next(256);
        }
    }
}
