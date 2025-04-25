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
            // Ba�lang�� rengi
            UpdateColor();
        }

        void OnColorChanged(object sender, ValueChangedEventArgs e)
        {
            UpdateColor();
        }

        void UpdateColor()
        {
            // Slider de�erlerini al
            int r = (int)RedSlider.Value;
            int g = (int)GreenSlider.Value;
            int b = (int)BlueSlider.Value;

            // Etiketleri g�ncelle
            RedLabel.Text = r.ToString();
            GreenLabel.Text = g.ToString();
            BlueLabel.Text = b.ToString();

            // Hex kodu olu�tur
            string hex = $"#{r:X2}{g:X2}{b:X2}";
            HexLabel.Text = hex;

            // Arka plan� de�i�tir
            BackgroundColor = Color.FromRgb(r, g, b);
        }

        async void OnCopyClicked(object sender, EventArgs e)
        {
            string hex = HexLabel.Text;
            await Clipboard.Default.SetTextAsync(hex);
            await DisplayAlert("Kopyaland�", hex, "OK");
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
