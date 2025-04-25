using System;
using System.Globalization;
using Microsoft.Maui.Controls;

namespace MyVisualApp.Pages
{
    public partial class BMICalculatorPage : ContentPage
    {
        // Karþýlýklý güncelleme döngüsünü önlemek için bayrak
        bool _suppressEvents;

        public BMICalculatorPage()
        {
            InitializeComponent();

            // Baþlangýç deðerleri
            WeightSlider.Value = 70.0;
            HeightSlider.Value = 170.0;

            // Entry’leri eþitle ve ilk VKÝ’yi hesapla
            WeightEntry.Text = WeightSlider.Value.ToString("F1", CultureInfo.InvariantCulture);
            HeightEntry.Text = HeightSlider.Value.ToString("F1", CultureInfo.InvariantCulture);
            UpdateBmi();
        }

        // Slider deðeri deðiþtiðinde Entry’yi güncelle
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

        // Entry metni deðiþtiðinde Slider’ý güncelle
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

        // VKÝ’yi hesaplayýp ekranda gösterir
        void UpdateBmi()
        {
            double kg = WeightSlider.Value;
            double m = HeightSlider.Value / 100.0;
            double bmi = kg / (m * m);

            // Kategori belirleme
            string category;
            if (bmi < 16) category = "Ýleri Düzeyde Zayýf";
            else if (bmi < 17) category = "Orta Düzeyde Zayýf";
            else if (bmi < 18.5) category = "Hafif Düzeyde Zayýf";
            else if (bmi < 25) category = "Normal Kilolu";
            else if (bmi < 30) category = "Hafif Þiþman";
            else if (bmi < 35) category = "1. Derece Obez";
            else if (bmi < 40) category = "2. Derece Obez";
            else               category = "3. Derece Obez / Morbid Obez";

            BmiLabel.Text = $"VKÝ: {bmi:F2}   {category}";
        }
    }
}
