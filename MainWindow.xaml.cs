using Newtonsoft.Json;
using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Data;

namespace budget
{
    public partial class MainWindow : Window
    {
        private List<Notes> notes = new List<Notes>();
        public List<string> type = new List<string>();
        private double totalAmount;

        public MainWindow()
        {
            InitializeComponent();
            notes = MyConverter.MyDeserialize<List<Notes>>() ?? new List<Notes>();
            typeBox.ItemsSource = type;
            LoadDataFromJson();
            UpdateNotesList();
            
        }


        private void LoadDataGrid()
        {
            listbox1.ItemsSource = notes;
            listbox1.Columns.Clear();

           
        }

        private void Button_Click(object sender, RoutedEventArgs e)
        {
            var selectedDate = datepicker.SelectedDate;

            if (selectedDate == null)
            {
                MessageBox.Show("Выберите дату");
                return;
            }

            var noteDate = selectedDate.Value;
            var note = new Notes
            {
                day = noteDate.ToString("dd.MM.yyyy"),
                note = textbox1.Text,
                type = typeBox.Text,
                amount_money = textbox2.Text,
                positive_or_negative = Convert.ToDouble(textbox2.Text) >= 0
            };

            notes.Add(note);
            MyConverter.MySerealize(notes);
            UpdateNotesList();

        }

        private void Button_Click_1(object sender, RoutedEventArgs e)
        {
            if (listbox1.SelectedItem != null)
            {
                var selectedNote = (Notes)listbox1.SelectedItem;

                // Удалить выбранный элемент из списка
                notes.Remove(selectedNote);

                // Сериализовать список и сохранить его в файл JSON
                string json = JsonConvert.SerializeObject(notes);
                File.WriteAllText("C:\\Users\\Vladi\\OneDrive\\Рабочий стол\\budget\\my.json", json);

                // Обновить список и DataGrid
                UpdateNotesList();
                LoadDataGrid();
            }
        }




        private void UpdateNotesList()
        {
            var selectedDate = datepicker.SelectedDate;
            if (selectedDate != null)
            {
                var selectedDateString = selectedDate.Value.ToString("dd.MM.yyyy");
                var selectedNotes = notes.Where(n => n.day == selectedDateString).ToList();
                listbox1.ItemsSource = selectedNotes;
            }
            else
            {
                listbox1.ItemsSource = null;
            }

            typeBox.ItemsSource = notes.Where(n => n.type != null).Select(n => n.type).Distinct().ToList();


            // Вычислить суммарный итог за все время
            totalAmount = notes.Sum(n => Convert.ToDouble(n.amount_money));
            totalLabel.Content = $"Total: {totalAmount:F2}";
        }



        private void Button_Click_3(object sender, RoutedEventArgs e)
        {
            if (listbox1.SelectedItem != null)
            {
                var selectedNote = (Notes)listbox1.SelectedItem;
                selectedNote.note = textbox1.Text;
                selectedNote.type = typeBox.Text;

                int pos_nev = Convert.ToInt32(textbox2.Text);
                if (pos_nev > 0)
                {
                    selectedNote.positive_or_negative = true;
                }
                else
                {
                    selectedNote.positive_or_negative = false;
                }

                MyConverter.MySerealize(notes);

                UpdateNotesList();
            }
        }

        private void datepicker_SelectedDateChanged_1(object sender, SelectionChangedEventArgs e)
        {
            UpdateNotesList();
        }
        private void LoadDataFromJson()
        {
            if (File.Exists("C:\\Users\\Vladi\\OneDrive\\Рабочий стол\\budget\\my.json"))
            {
                string json = File.ReadAllText("C:\\Users\\Vladi\\OneDrive\\Рабочий стол\\budget\\my.json");
                if (!string.IsNullOrEmpty(json))
                {
                    notes = JsonConvert.DeserializeObject<List<Notes>>(json);
                }
            }

            LoadDataGrid();
        }


        private void Button_Click_4(object sender, RoutedEventArgs e)
        {
            CreateType createType = new CreateType();
            createType.ShowDialog();

            string newType = createType.TypeName;

            if (!string.IsNullOrEmpty(newType))
            {
                type.Add(newType);
                typeBox.ItemsSource = null;
                typeBox.ItemsSource = type;
            }
        }
    }
}
