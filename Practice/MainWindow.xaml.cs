using PracticeLibrary;
using System.Data;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Media;

namespace Practice
{
    /// <summary>
    /// Interaction logic for MainWindow.xaml
    /// </summary>
    public partial class MainWindow : Window
    {
        public MainWindow()
        {
            InitializeComponent();

        }
        //private bool TryParseValue() 
        //{

        //}
        int[] array;
        int[,] matrix;
        int arrayQuantityColumns = 5;
        int matrixQuantityColumns = 5;
        int matrixQuantityRows = 5;
        const int MINVALUE = 3;
        const int MAXVALUE = 10;
        const int STEP = 1;
        string firstTaskText = "Ввести двузначное число. Определить: одинаковы ли его цифры.";
        string secondTaskText = "Ввести три целых числа. Возвести в квадрат те из них, значения которых неотрицательны.";
        string thirdTaskText = "Выяснить, имеются ли в данном массиве 2 идущих подряд положительных элемента. Подсчитать количество таких пар.";
        string foughtTaskText = "Дана матрица размера M * N и целое число K (1 < K < N). Найти сумму и произведение элементов K-го столбца данной матрицы.";
        private void FillArray()
        {
            Random r = new Random();
            DataTable dt = new DataTable();
            // Сначала создаем все столбцы
            for (int i = 0; i < arrayQuantityColumns; i++)
            {
                dt.Columns.Add($"Кол {i + 1}");
            }

            array = new int[arrayQuantityColumns];
            DataRow row = dt.NewRow();

            for (int i = 0; i < array.Length; i++)
            {
                array[i] = r.Next(-5, 6);
                row[i] = array[i];
            }

            dt.Rows.Add(row);
            dgArray.ItemsSource = dt.DefaultView;
        }
        private void FillMatrix()
        {
            Random r = new Random();
            DataTable dt = new DataTable();

            // Создаем матрицу
            matrix = new int[matrixQuantityRows, matrixQuantityColumns];

            // 1. Создаем столбцы в DataTable (каждый столбец DataTable = столбец матрицы)
            for (int col = 0; col < matrixQuantityColumns; col++)
            {
                dt.Columns.Add($"Колонка {col + 1}", typeof(int));
            }

            // 2. Заполняем строки DataTable (каждая строка DataTable = строка матрицы)
            for (int row = 0; row < matrixQuantityRows; row++)
            {
                DataRow dataRow = dt.NewRow();

                for (int col = 0; col < matrixQuantityColumns; col++)
                {
                    matrix[row, col] = r.Next(-5, 6);
                    dataRow[col] = matrix[row, col];
                }

                dt.Rows.Add(dataRow);
            }

            dgMatrix.ItemsSource = dt.DefaultView;
        }
        private void FirstTask()
        {
            if (!int.TryParse(txtTwoDigitValue.Text, out int value))
            {
                MessageBox.Show("Введите корректное целое число");
                return;
            }
            if (value < 10 || value > 99)
            {
                MessageBox.Show("Введите двузначное число (10-99)");
                return;
            }
            if (Operations.Compare(value))
                MessageBox.Show($"Цифры числа {value.ToString()} одинаковы");
            else
                MessageBox.Show($"Цифры числа {value.ToString()} не одинаковы");
        }
        private void SecondTask()
        {
            var textBoxes = new List<TextBox>() { txtAValue, txtBValue, txtCValue };
            bool anyChanged = false;

            foreach (TextBox textBox in textBoxes)
            {
                if (!int.TryParse(textBox.Text, out int value))
                {
                    MessageBox.Show($"Введите корректное число в поле {textBox.Tag}");
                    return;
                }

                int originalValue = value;
                Operations.InSquare(ref value);

                if (originalValue != value) // Проверяем, изменилось ли значение
                {
                    textBox.Text = value.ToString();
                    anyChanged = true;
                }
            }

            if (anyChanged)
                MessageBox.Show("Неотрицательные числа были возведены в квадрат");
            else
                MessageBox.Show("Не было неотрицательных чисел для возведения в квадрат");
        }
        private void ThirdTask()
        {
            if (array == null)
            {
                MessageBox.Show("Заполните массив");
                return;
            }
            int quantityPairs = Operations.PositivePairs(array);
            if (quantityPairs > 0)
                MessageBox.Show($"В массиве {quantityPairs} положительных пар");
            else
                MessageBox.Show($"В массиве нет положительных пар");
        }
        private void FourthTask()
        {
            if (matrix == null)
            {
                MessageBox.Show("Заполните матрицу");
                return;
            }
            Random r = new Random();
            int k = r.Next(2, matrixQuantityColumns);
            txtRandomKColumn.Text = txtRandomKColumn.Tag.ToString() + k;
            txtRandomKColumn.Foreground = Brushes.Black;
            int sum, multiply;
            Operations.SumAndMultiply(out sum, out multiply, matrix, k - 1);
            MessageBox.Show($"Сумма {k}-й колонки = {sum}\r\nПроизведение {k}-й колонки = {multiply}");
        }
        private void ScrollValue(System.Windows.Input.MouseWheelEventArgs e, ref int value, int min, int max, int step)
        {
            if (e.Delta > 0)
            {
                if (value < max)
                    value += step;
                else
                {
                    MessageBox.Show($"Значение не может быть выше {max}");
                }

                e.Handled = true;
            }
            else if (e.Delta < 0)
            {
                if (value > min)
                    value -= step;
                else
                {
                    MessageBox.Show($"Значение не может быть ниже {min}");
                }
                e.Handled = true;
            }
        }
        private void HideWatermark(object sender, RoutedEventArgs e)
        {
            var send = sender as TextBox;
            if (send.Text == send.Tag.ToString())
            {
                send.Text = string.Empty;
                send.Foreground = Brushes.Black;
            }
        }

        private void ShowWatermark(object sender, RoutedEventArgs e)
        {
            var send = sender as TextBox;
            if (string.IsNullOrWhiteSpace(send.Text) || send.Text == send.Tag.ToString())
            {
                send.Text = send.Tag.ToString();
                send.Foreground = Brushes.Gray;
            }
            send.Text = send.Text.Trim();
        }

        private void Button_Click(object sender, RoutedEventArgs e)
        {
            var send = sender as Button;
            switch (send.Name)
            {
                case "btnCheckTwoDigitValue":
                    FirstTask();
                    break;
                case "btnInSquareValues":
                    SecondTask();
                    break;
                case "btnFillArray":
                    FillArray();
                    break;
                case "btnFindPairs":
                    ThirdTask();
                    break;
                case "btnFillMatrix":
                    FillMatrix();
                    break;
                case "btnFindSumAndMultiply":
                    FourthTask();
                    break;
            }
        }

        private void TextBox_MouseWheel(object sender, System.Windows.Input.MouseWheelEventArgs e)
        {
            var send = sender as TextBox;
            switch (send.Name)
            {
                case "txtArrayQuantityColumns":
                    ScrollValue(e, ref arrayQuantityColumns, MINVALUE, MAXVALUE, STEP);
                    send.Text = send.Tag.ToString() + arrayQuantityColumns;
                    break;
                case "txtMatrixQuantityColumns":
                  
                    ScrollValue(e, ref matrixQuantityColumns, MINVALUE, MAXVALUE, STEP);
                    send.Text = send.Tag.ToString() + matrixQuantityColumns;
                    break;
                case "txtMatrixQuantityRows":
                   
                    ScrollValue(e, ref matrixQuantityRows, MINVALUE, MAXVALUE, STEP);
                    send.Text = send.Tag.ToString() + matrixQuantityRows;
                    break;
            }
        }

        private void MenuItem_Click(object sender, RoutedEventArgs e)
        {
            var send = sender as MenuItem;
            switch (send.Name) 
            {
                case "btmFirstTask":
                    MessageBox.Show(firstTaskText);
                    break;
                case "btmSecondTask":
                    MessageBox.Show(secondTaskText);
                    break;
                case "btmthirdTask":
                    MessageBox.Show(thirdTaskText);
                    break;
                case "btmFourthTask":
                    MessageBox.Show(foughtTaskText);
                    break;
            }
        }
    }
}