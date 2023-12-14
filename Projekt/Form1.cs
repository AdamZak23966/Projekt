using System;
using System.Collections.Generic;
using System.Windows.Forms;
using System.Diagnostics;
using System.Windows.Forms.DataVisualization.Charting;

namespace Projekt
{
    public partial class Form1 : Form
    {
        private SortAlgorithm sorter;
        private List<int> unsortedData;
        private Dictionary<string, long> algorithmTimes;

        private System.Windows.Forms.ListBox listBox1;

        public Form1()
        {
            InitializeComponent();
            sorter = new SortAlgorithm();
            unsortedData = new List<int>();
            algorithmTimes = new Dictionary<string, long>();

            this.listBox1 = new System.Windows.Forms.ListBox();
            this.listBox1.FormattingEnabled = true;
            this.listBox1.Location = new System.Drawing.Point(538, 350);
            this.listBox1.Name = "listBox1";
            this.listBox1.Size = new System.Drawing.Size(300, 95);
            this.listBox1.TabIndex = 13;

            this.Controls.Add(this.listBox1);
        }

        private void radioButton1_CheckedChanged(object sender, EventArgs e)
        {
            GenerateData();
        }

        private void radioButton2_CheckedChanged(object sender, EventArgs e)
        {
            GenerateData();
        }

        private void radioButton3_CheckedChanged(object sender, EventArgs e)
        {
            GenerateData();
        }

        private void comboBox1_SelectedIndexChanged(object sender, EventArgs e)
        {
            UpdateChart();
        }

        private void textBox1_TextChanged(object sender, EventArgs e)
        {
            if (!string.IsNullOrWhiteSpace(textBox1.Text))
            {
                GenerateData();
            }
            else
            {
                chart1.Series.Clear();
                unsortedData.Clear();
            }
        }

        private void chart1_Click(object sender, EventArgs e)
        {
        }

        private void chart2_Click(object sender, EventArgs e)
        {
        }

        private void button1_Click(object sender, EventArgs e)
        {
            if (!int.TryParse(textBox1.Text, out int dataSize) || dataSize <= 0)
            {
                MessageBox.Show("Wprowadź poprawną ilość elementów do posortowania.", "Błąd", MessageBoxButtons.OK, MessageBoxIcon.Error);
                return;
            }

            SortAndDisplay();
        }

        private void button2_Click(object sender, EventArgs e)
        {
            // Wyczyść wybór rodzaju ciągu do sortowania
            radioButton1.Checked = false;
            radioButton2.Checked = false;
            radioButton3.Checked = false;

            // Wyczyść wybór rodzaju algorytmu do sortowania
            comboBox1.SelectedIndex = -1;

            // Wyczyść pole ilości elementów do sortowania
            textBox1.Clear();

            // Wyczyść obie wykresy
            chart1.Series.Clear();
            chart2.Series.Clear();

            // Wyczyść listę czasów algorytmów
            algorithmTimes.Clear();

            // Wyczyść listbox z czasami
            listBox1.Items.Clear();

            // Aktualizuj etykietę z czasem sortowania
            label5.Text = "label5";
        }

        private void GenerateData()
        {
            int dataSize;
            if (int.TryParse(textBox1.Text, out dataSize))
            {
                if (radioButton1.Checked)
                    unsortedData = GenerateAscendingData(dataSize);
                else if (radioButton2.Checked)
                    unsortedData = GenerateDescendingData(dataSize);
                else
                    unsortedData = GenerateRandomData(dataSize);
            }

            UpdateChart();
        }

        private List<int> GenerateAscendingData(int size)
        {
            List<int> data = new List<int>();
            for (int i = 1; i <= size; i++)
            {
                data.Add(i);
            }
            return data;
        }

        private List<int> GenerateDescendingData(int size)
        {
            List<int> data = new List<int>();
            for (int i = size; i > 0; i--)
            {
                data.Add(i);
            }
            return data;
        }

        private List<int> GenerateRandomData(int size)
        {
            List<int> data = new List<int>();
            Random random = new Random();
            for (int i = 0; i < size; i++)
            {
                data.Add(random.Next(1, 100));
            }
            return data;
        }

        private void UpdateChart()
        {
            chart1.Series.Clear();
            chart1.Series.Add("Unsorted");
            chart1.Series["Unsorted"].Points.DataBindY(unsortedData);
        }

        private void SortAndDisplay()
        {
            string selectedAlgorithm = comboBox1.SelectedItem?.ToString();

            if (string.IsNullOrEmpty(selectedAlgorithm))
            {
                MessageBox.Show("Wybierz algorytm sortowania.", "Błąd", MessageBoxButtons.OK, MessageBoxIcon.Error);
                return;
            }

            List<int> sortedData = new List<int>(unsortedData);

            Stopwatch stopwatch = new Stopwatch();
            stopwatch.Start();

            switch (selectedAlgorithm)
            {
                case "Sortowanie bąbelkowe":
                    sorter.BubbleSort(sortedData);
                    break;

                case "Sortowanie przez wybór":
                    sorter.SelectionSort(sortedData);
                    break;

                case "Sortowanie przez wstawianie":
                    sorter.InsertionSort(sortedData);
                    break;

                case "Sortowanie szybkie":
                    sorter.QuickSort(sortedData);
                    break;

                case "Sortowanie przez scalanie":
                    sortedData = sorter.MergeSort(sortedData);
                    break;

                default:
                    break;
            }

            stopwatch.Stop();

            algorithmTimes[selectedAlgorithm] = stopwatch.ElapsedTicks;

            if (chart1.Series.IndexOf("Sorted") != -1)
            {
                chart1.Series.Remove(chart1.Series["Sorted"]);
            }

            Series sortedSeries = new Series("Sorted");
            sortedSeries.Points.DataBindY(sortedData);
            chart1.Series.Add(sortedSeries);

            label5.Text = $"Czas sortowania ({selectedAlgorithm}): {stopwatch.ElapsedTicks} ticks";

            UpdateAlgorithmTimeChart();
        }

        private void UpdateAlgorithmTimeChart()
        {
            chart2.Series.Clear();
            listBox1.Items.Clear();

            foreach (var kvp in algorithmTimes)
            {
                listBox1.Items.Add($"Czas sortowania ({kvp.Key}): {kvp.Value} ticks");

                chart2.Series.Add(kvp.Key);
                chart2.Series[kvp.Key].Points.AddY(kvp.Value);
            }
        }
    }
}