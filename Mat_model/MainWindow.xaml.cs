using System;
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
        private void Edinicmat()
        {
            int count_col = Array.ColumnDefinitions.Count;
            for (int i = 0; i < count_col; i++)
            {
                for (int j = 0; j < count_col; j++)
                {
                    if (FindName($"ArrayObrBox{i}_{j}") is TextBox boxx)
                    {
                        if (i == j) { boxx.Text ="1"; }
                        else { boxx.Text = "0"; }
                    }
                }
            }
        }
        private void ObtatPostr()
        {
            int count_col = Array.ColumnDefinitions.Count;
            double[,] A = new double[count_col, count_col]; // основная матрица
            double[,] AObrat = new double[count_col, count_col]; // Обратная матрица
            double[,] ACopy = new double[count_col, count_col]; // Копия основной матрицы
            for (int i = 0; i < count_col; i++) // Запись данных в массив 
            {
                for (int c = 0; c < count_col; c++)
                {
                    if (FindName($"ArrayBox{i}_{c}") is TextBox box)
                    {
                        A[i, c] = Convert.ToDouble(box.Text);

                    }
                    if (FindName($"ArrayObrBox{i}_{c}") is TextBox boxx)
                    {
                        AObrat[i, c] = Convert.ToDouble(boxx.Text);

                    }
                }
            }
            //задаем обратную матрицу как единичную  
            for (int i = 0; i < count_col; i++)
            {
                for (int j = 0; j < count_col; j++)
                {
                    if (i == j) { AObrat[i, j] = 1; }
                    else { AObrat[i, j] = 0; }
                    ACopy[i, j] = A[i, j];    //создаем копию матрицы См
                    Console.WriteLine("ACopy[i, j] =" + ACopy[i, j]);
                }
            }
            //прямой ход
            for (int k = 0; k < count_col; ++k)
            {
                double div = ACopy[k, k];
                for (int m = 0; m < count_col; ++m)
                {// делим строку на выбранный элемент === 1  ф  ф
                    ACopy[k, m] /= div;
                    AObrat[k, m] /= div;
                }
                for (int i = k + 1; i < count_col; ++i) //идем по столбц ниже полученой единицы
                {
                    double multi = ACopy[i, k]; //элемент, который хотим занулить
                    for (int j = 0; j < count_col; ++j)// элемент по счету в строке i
                    {
                        ACopy[i, j] -= multi * ACopy[k, j];
                        AObrat[i, j] -= multi * AObrat[k, j];
                    }
                }
            }
            // обратный
            for (int kk = count_col - 1; kk > 0; kk--)
            {
                for (int i = kk - 1; i + 1 > 0; i--)
                {
                        double multi2 = ACopy[i, kk];
                        for (int j = 0; j < count_col; j++)
                        {
                            ACopy[i, j] -= multi2 * ACopy[kk, j];
                            AObrat[i, j] -= multi2 * AObrat[kk, j];
                        }
                }
            }

            // Вывод обратной
            for (int i = 0; i < count_col; i++)
            {
                for (int c = 0; c < count_col; c++)
                {
                    if (FindName($"ArrayBox{i}_{c}") is TextBox box)
                    {
                        if (i == c) { box.Text = "1"; }
                        else { box.Text = "0"; }
                        ACopy[i, c] = A[i, c];
                    }
                    if (FindName($"ArrayObrBox{i}_{c}") is TextBox boxx)
                    {
                        boxx.Text = Convert.ToString(Math.Round(AObrat[i, c],2));
                    }
                }
            }
        }
        private void size_c_TextChanged(object sender, TextChangedEventArgs e)
        {
            if (!Check_input(size_c.Text)) size_c.Text = ""; //Если метод, проверяющий буква вводится или нет, возвращает false строка становится пустой 
            if (size_c.Text != "")
            {
                Update_array(Convert.ToInt32(size_c.Text));
                Update_array1(Convert.ToInt32(size_c.Text));//создаем матрицу
                Edinicmat();
            }

        }
        private void Update_array1(int b)
        {
            //получаем кол-во столбцов и строк в матрице чтобы удалить их
            int count_col = ArrayObr.ColumnDefinitions.Count;
            int count_row = ArrayObr.RowDefinitions.Count;


            for (int i = 0; i < count_col; i++) ArrayObr.ColumnDefinitions.RemoveAt(0);
            for (int i = 0; i < count_row; i++) ArrayObr.RowDefinitions.RemoveAt(0);

            //сохдаем и добавляем в сетку нужжное кол-во строк
            for (int i = 0; i < b; i++)
            {
                var Row = new RowDefinition();
                ArrayObr.RowDefinitions.Add(Row);
            }
            //сохдаем и добавляем в сетку нужжное кол-во столбцов
            for (int i = 0; i < b; i++)
            {
                var Col = new ColumnDefinition();
                ArrayObr.ColumnDefinitions.Add(Col);
            }
            //проходимся по каждомй столбцу в каждой строке
            for (int i = 0; i < b; i++)
            {
                for (int e = 0; e < b; e++)//тут все как в блоках в ввекторах, только контур другой и метод при изменениии текста
                {
                    var text_box = new TextBox();
                    text_box.Name = $"ArrayObrBox{i}_{e}";
                    try
                    {
                        this.RegisterName(text_box.Name, text_box);
                    }
                    catch
                    {
                        this.UnregisterName(text_box.Name);
                        this.RegisterName(text_box.Name, text_box);
                    }
 //                   text_box.TextChanged += ArrayBox_TextChanged;
                    text_box.Style = (Style)size_c.FindResource("TextBoxStyle1");
                    text_box.Foreground = new SolidColorBrush(Colors.White);
                    text_box.Background = new SolidColorBrush(Colors.Black);
                    text_box.HorizontalContentAlignment = HorizontalAlignment.Center;
                    text_box.VerticalContentAlignment = VerticalAlignment.Center;
                    text_box.BorderBrush = Brushes.White;
                    text_box.BorderThickness = new Thickness(1, 1, 1, 1);
                    text_box.FontSize = 22;
                    //матрица делается "на 1 больше" чем пишет пользователь, в этих "лишних" ячейках будут писаться значения из векторов и в них самому писать нельзя
                    text_box.IsReadOnly = true;
                    ArrayObr.Children.Add(text_box);
                    Grid.SetRow(text_box, i);
                    Grid.SetColumn(text_box, e);
                }
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
            for (int i = 0; i < b; i++)
            {
                var Row = new RowDefinition();
                Array.RowDefinitions.Add(Row);
            }
            //сохдаем и добавляем в сетку нужжное кол-во столбцов
            for (int i = 0; i < b; i++)
            {
                var Col = new ColumnDefinition();
                Array.ColumnDefinitions.Add(Col);
            }
            //проходимся по каждомй столбцу в каждой строке
            for (int i = 0; i < b; i++)
            {
                for (int e = 0; e < b; e++)//тут все как в блоках в ввекторах, только контур другой и метод при изменениии текста
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
                    text_box.Style = (Style)size_c.FindResource("TextBoxStyle1");
                    text_box.Foreground = new SolidColorBrush(Colors.White);
                    text_box.Background = new SolidColorBrush(Colors.Black);
                    text_box.HorizontalContentAlignment = HorizontalAlignment.Center;
                    text_box.VerticalContentAlignment = VerticalAlignment.Center;
                    text_box.BorderBrush = Brushes.White;
                    text_box.BorderThickness = new Thickness(1, 1, 1, 1);
                    text_box.FontSize = 22;
                    Array.Children.Add(text_box);
                    Grid.SetRow(text_box, i);
                    Grid.SetColumn(text_box, e);
                }
            }
        }
        private void ATVET_Click(object sender, RoutedEventArgs e)
        {
            ObtatPostr();
        }
    }
}
