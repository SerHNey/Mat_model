﻿using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Data;
using System.Windows.Documents;
using System.Windows.Input;
using System.Windows.Media;
using System.Windows.Media.Imaging;
using System.Windows.Navigation;
using System.Windows.Shapes;

namespace Mat_model
{
    /// <summary>
    /// Логика взаимодействия для MainWindow.xaml
    /// </summary>
    public partial class MainWindow : Window
    {
        public MainWindow()
        {
            InitializeComponent();
        }
        private bool Check_input(string a)
        {
            try
            {
                int i = Convert.ToInt32(a);
                return true;
            }
            catch (System.FormatException)
            {
                return false;
            }

        }
        private void Update_vectors(int b)//получает числа введенные в size_r и size_c
        {
            //получаем кол-во колонок 
            for (int i = 0; i < b; i++)//создаем кол-во колонок и текстовых блоков равное чилу введенному в size_r
            {
                var text_box = new TextBox();//создаем экземпляр объекта TextBox
                var Col = new ColumnDefinition();//создаем экземпляр объекта ColumnDefinition

                text_box.Foreground = new SolidColorBrush(Colors.Black);//цвет текста
                text_box.Background = new SolidColorBrush(Colors.White);//цвет фона
                text_box.TextAlignment = TextAlignment.Center;//выравнивание по центру 
                text_box.Style = (Style)size_c.FindResource("TextBoxStyle1");//применяем стиль блока (там просто цвет выделения блока на белый поменял)
                text_box.Name = $"Power_{i}";//присваеваем блоку имя, чтобы потом по имени получать из него значение
                text_box.FontSize = 16;
                try
                {
                    this.RegisterName(text_box.Name, text_box);//пытаемся зарегистрировать имя
                }
                catch//если такое имя есть, удаляем его и еще раз регистрируем
                {
                    this.UnregisterName(text_box.Name);
                    this.RegisterName(text_box.Name, text_box);
                }
                //добавляем метод еоторый будет вызываться при изменениии значения в блоке 
                if (i != 0)
                {
                    text_box.BorderBrush = new SolidColorBrush(Colors.White);//цвет контура
                    text_box.BorderThickness = new Thickness(2, 0, 0, 0);//ширина контура (справо,верх,слево,низ)
                }
                else text_box.BorderThickness = new Thickness(0);
                //"засовываем" получившийся блок в сетку power_vector
                Grid.SetColumn(text_box, i);//выбираем колонку для элемента
            }
            //тут тоже самое только для другой 
            for (int i = 0; i < b; i++)//создаем кол-во колонок и текстовых блоков равное чилу введенному в size_с
            {
                var text_box = new TextBox();
                var Col = new ColumnDefinition();

                text_box.Foreground = new SolidColorBrush(Colors.White);
                text_box.Background = new SolidColorBrush(Colors.Black);
                text_box.TextAlignment = TextAlignment.Center;
                text_box.Style = (Style)size_c.FindResource("TextBoxStyle1");
                text_box.Name = $"Demand_{i}";
                text_box.FontSize = 16;
                try
                {
                    this.RegisterName(text_box.Name, text_box);
                }
                catch
                {
                    this.UnregisterName(text_box.Name);
                    this.RegisterName(text_box.Name, text_box);
                }

                if (i != 0)
                {
                    text_box.BorderBrush = new SolidColorBrush(Colors.White);
                    text_box.BorderThickness = new Thickness(2, 0, 0, 0);
                }
                else text_box.BorderThickness = new Thickness(0);
            }
        }
        private void Update_array(int b)
        {
            //получаем кол-во столбцов и строк в матрице чтобы удалить их
            int count_col = Array.ColumnDefinitions.Count;
            int count_row = Array.RowDefinitions.Count;
            for (int i = 0; i < count_col; i++) Array.ColumnDefinitions.RemoveAt(0);
            for (int i = 0; i < count_row; i++) Array.RowDefinitions.RemoveAt(0);
            //сохдаем и добавляем в сетку нужжное кол-во строк
            for (int i = 0; i <= b; i++)
            {
                var Row = new RowDefinition();
                Array.RowDefinitions.Add(Row);
            }
            //сохдаем и добавляем в сетку нужжное кол-во столбцов
            for (int i = 0; i <= b; i++)
            {
                var Col = new ColumnDefinition();
                Array.ColumnDefinitions.Add(Col);
            }
            //проходимся по каждомй столбцу в каждой строке
            for (int i = 0; i <= b; i++)
            {
                for (int e = 0; e <= b; e++)//тут все как в блоках в ввекторах, только контур другой и метод при изменениии текста
                {
                    var text_box = new TextBox();
                    text_box.Name = $"ArrayBox{i}_{e}";
                    try
                    {
                        this.RegisterName(text_box.Name, text_box);
                    }
                    catch
                    {
                        this.UnregisterName(text_box.Name);
                        this.RegisterName(text_box.Name, text_box);
                    }
                    text_box.TextChanged += ArrayBox_TextChanged;
                    text_box.Style = (Style)size_c.FindResource("TextBoxStyle1");
                    text_box.Foreground = new SolidColorBrush(Colors.White);
                    text_box.Background = new SolidColorBrush(Colors.Black);
                    text_box.HorizontalContentAlignment = HorizontalAlignment.Center;
                    text_box.VerticalContentAlignment = VerticalAlignment.Center;
                    text_box.BorderBrush = Brushes.White;
                    text_box.BorderThickness = new Thickness(0, 0, 2, 2);
                    text_box.FontSize = 22;
                    //матрица делается "на 1 больше" чем пишет пользователь, в этих "лишних" ячейках будут писаться значения из векторов и в них самому писать нельзя
                    if (i == 0) text_box.IsReadOnly = true;
                    else if (e == 0) text_box.IsReadOnly = true;
                    Array.Children.Add(text_box);
                    Grid.SetRow(text_box, i);
                    Grid.SetColumn(text_box, e);
                }
            }
        }
        private void ArrayBox_TextChanged(object sender, TextChangedEventArgs e)
        {
            //из-за того что кол-во ячеек в матрице не статично, при изменении текста в любой из них пробегаем по всем ячейкам
            int count_row = Array.RowDefinitions.Count;
            int count_col = Array.ColumnDefinitions.Count;
            int none_content_box = 0;//кол-во пустых ячеек
            for (int i = 0; i < count_row; i++)
            {
                for (int c = 0; c < count_col; c++)
                {
                    if (FindName($"ArrayBox{i}_{c}") is TextBox box)
                    {
                        if (!Check_input(box.Text))
                        {
                            box.Text = "";
                            none_content_box++;
                        }
                    }
                }
            }
        }
        

        private void size_c_TextChanged(object sender, TextChangedEventArgs e)
        {
            if (!Check_input(size_c.Text)) size_c.Text = ""; //Если метод, проверяющий буква вводится или нет, возвращает false строка становится пустой 
            if (size_c.Text != "")
            {
                Update_vectors(Convert.ToInt32(size_c.Text));//создаем в сетках для векторов нужное кол-во колонок и текстовых блоков
                Update_array(Convert.ToInt32(size_c.Text));//создаем матрицу
            }

        }
    }
}