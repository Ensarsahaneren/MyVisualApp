using System;
using System.Globalization;
using Microsoft.Maui.Controls;

namespace MyVisualApp.Pages
{
    public partial class BMICalculatorPage : ContentPage
    {
        // Kar��l�kl� g�ncelleme d�ng�s�n� �nlemek i�in bayrak
        bool _suppressEvents;

        public BMICalculatorPage()
        {
            InitializeComponent();

            // Ba�lang�� de�erleri
            WeightSlider.Value = 70.0;
            HeightSlider.Value = 170.0;

            // Entry�leri e�itle ve ilk VKݒyi hesapla
            WeightEntry.Text = WeightSlider.Value.ToString("F1", CultureInfo.InvariantCulture);
            HeightEntry.Text = HeightSlider.Value.ToString("F1", CultureInfo.InvariantCulture);
            UpdateBmi();
        }

        // Slider de�eri de�i�ti�inde Entry�yi g�ncelle
        void WeightSlider_ValueChanged(object sender, ValueChangedEventArgs e)
        {
            if (_suppressEvents) return;
            _suppressEvents = true;

            WeightEntry.Text = e.NewValue.ToString("F1", CultureInfo.InvariantCulture);
            UpdateBmi();

            _suppressEvents = false;
        }

        void HeightSlider_ValueChanged(object sender, ValueChangedEventArgs e)
        {
            if (_suppressEvents) return;
            _suppressEvents = true;

            HeightEntry.Text = e.NewValue.ToString("F1", CultureInfo.InvariantCulture);
            UpdateBmi();

            _suppressEvents = false;
        }

        // Entry metni de�i�ti�inde Slider�� g�ncelle
        void WeightEntry_TextChanged(object sender, TextChangedEventArgs e)
        {
            if (_suppressEvents) return;

            if (double.TryParse(e.NewTextValue, NumberStyles.AllowDecimalPoint, CultureInfo.InvariantCulture, out double kg))
            {
                kg = Math.Clamp(kg, WeightSlider.Minimum, WeightSlider.Maximum);
                _suppressEvents = true;

                WeightSlider.Value = kg;
                UpdateBmi();

                _suppressEvents = false;
            }
        }

        void HeightEntry_TextChanged(object sender, TextChangedEventArgs e)
        {
            if (_suppressEvents) return;

            if (double.TryParse(e.NewTextValue, NumberStyles.AllowDecimalPoint, CultureInfo.InvariantCulture, out double cm))
            {
                cm = Math.Clamp(cm, HeightSlider.Minimum, HeightSlider.Maximum);
                _suppressEvents = true;

                HeightSlider.Value = cm;
                UpdateBmi();

                _suppressEvents = false;
            }
        }

        // VKݒyi hesaplay�p ekranda g�sterir
        void UpdateBmi()
        {
            double kg = WeightSlider.Value;
            double m = HeightSlider.Value / 100.0;
            double bmi = kg / (m * m);

            // Kategori belirleme
            string category;
            if (bmi < 16) category = "�leri D�zeyde Zay�f";
            else if (bmi < 17) category = "Orta D�zeyde Zay�f";
            else if (bmi < 18.5) category = "Hafif D�zeyde Zay�f";
            else if (bmi < 25) category = "Normal Kilolu";
            else if (bmi < 30) category = "Hafif �i�man";
            else if (bmi < 35) category = "1. Derece Obez";
            else if (bmi < 40) category = "2. Derece Obez";
            else               category = "3. Derece Obez / Morbid Obez";

            BmiLabel.Text = $"VK�: {bmi:F2}   {category}";
        }
    }
}
