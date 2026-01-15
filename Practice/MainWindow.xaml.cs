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

        int[] array;
        int[,] matrix;
        int arrayQuantityColumns = 5;
        int matrixQuantityColumns = 5;
        int matrixQuantityRows = 5;
        const int MINVALUE = 3;
        const int MAXVALUE = 10;
        const int STEP = 1;
        private bool isProgrammaticTextChange = false; // Флаг для программных изменений
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
            txtFirstTaskOutput.Visibility = Visibility.Visible;
            if (Operations.Compare(value))
                txtFirstTaskOutput.Text=($"Цифры числа {value.ToString()} одинаковы");
            else
                txtFirstTaskOutput.Text = ($"Цифры числа {value.ToString()} не одинаковы");
        }

        private void SecondTask()
        {
            var textBoxes = new List<TextBox>() { txtAValue, txtBValue, txtCValue };
            bool anyChanged = false;

            foreach (TextBox textBox in textBoxes)
            {
                if (!int.TryParse(textBox.Text, out int value))
                {
                    MessageBox.Show($"Введите корректное число в поле '{textBox.Tag}'");
                    return;
                }

                int originalValue = value;
                Operations.InSquare(ref value);

                if (originalValue != value) // Проверяем, изменилось ли значение
                {
                    // Устанавливаем флаг перед программным изменением
                    isProgrammaticTextChange = true;
                    textBox.Text = value.ToString();
                    isProgrammaticTextChange = false; // Сбрасываем флаг
                    anyChanged = true;
                }
            }

            if (anyChanged)
                MessageBox.Show("Неотрицательные числа, которые не могли бы выйти за максимальный предел были возведены в квадрат");
            else
                MessageBox.Show("Не было неотрицательных чисел или чисел, которые не могли бы выйти за максимальный предел");
        }

        private void ThirdTask()
        {
            if (array == null)
            {
                MessageBox.Show("Заполните массив");
                return;
            }
            int quantityPairs = Operations.PositivePairs(array);
            txtThirdTaskOutput.Visibility = Visibility.Visible;
            if (quantityPairs > 0)
                txtThirdTaskOutput.Text=($"В массиве {quantityPairs} положительных пар");
            else
                txtThirdTaskOutput.Text=($"В массиве нет положительных пар");
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
            txtFourthTaskOutput.Visibility = Visibility.Visible;
            txtFourthTaskOutput.Text=($"Сумма {k}-й колонки = {sum}\r\nПроизведение {k}-й колонки = {multiply}");
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
                // Устанавливаем флаг перед программным изменением
                isProgrammaticTextChange = true;
                send.Text = string.Empty;
                send.Foreground = Brushes.Black;
                isProgrammaticTextChange = false; // Сбрасываем флаг
            }
        }

        private void ShowWatermark(object sender, RoutedEventArgs e)
        {
            var send = sender as TextBox;
            if (string.IsNullOrWhiteSpace(send.Text) || send.Text == send.Tag.ToString())
            {
                // Устанавливаем флаг перед программным изменением
                isProgrammaticTextChange = true;
                send.Text = send.Tag.ToString();
                send.Foreground = Brushes.Gray;
                isProgrammaticTextChange = false; // Сбрасываем флаг
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
                    // Устанавливаем флаг перед программным изменением
                    isProgrammaticTextChange = true;
                    send.Text = send.Tag.ToString() + arrayQuantityColumns;
                    isProgrammaticTextChange = false; // Сбрасываем флаг
                    break;
                case "txtMatrixQuantityColumns":
                    ScrollValue(e, ref matrixQuantityColumns, MINVALUE, MAXVALUE, STEP);
                    // Устанавливаем флаг перед программным изменением
                    isProgrammaticTextChange = true;
                    send.Text = send.Tag.ToString() + matrixQuantityColumns;
                    isProgrammaticTextChange = false; // Сбрасываем флаг
                    break;
                case "txtMatrixQuantityRows":
                    ScrollValue(e, ref matrixQuantityRows, MINVALUE, MAXVALUE, STEP);
                    // Устанавливаем флаг перед программным изменением
                    isProgrammaticTextChange = true;
                    send.Text = send.Tag.ToString() + matrixQuantityRows;
                    isProgrammaticTextChange = false; // Сбрасываем флаг
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

        private void TextBox_TextChanged(object sender, TextChangedEventArgs e)
        {
            // Если это программное изменение - пропускаем обработку
            if (isProgrammaticTextChange) return;

            var send = sender as TextBox;
            if (send.Tag == null) return;
            int maxLength = 5;

            // Если это водяной знак - пропускаем обработку
            if (send.Text == send.Tag?.ToString())
                return;

            // Если текст превышает максимальную длину
            if (send.Text.Length > maxLength)
            {
                // Устанавливаем флаг перед программным изменением
                isProgrammaticTextChange = true;

                try
                {
                    // Сохраняем позицию курсора
                    int cursorPos = send.SelectionStart;

                    // Обрезаем текст
                    send.Text = send.Text.Substring(0, maxLength);

                    // Восстанавливаем позицию курсора
                    send.SelectionStart = Math.Min(cursorPos, maxLength);

                    MessageBox.Show("Максимальное количество символов, которых можно ввести = 5");
                }
                finally
                {
                    // Всегда сбрасываем флаг, даже если произошла ошибка
                    isProgrammaticTextChange = false;
                }
            }
        }
    }

    public class Operations
    {
        public static bool Compare(int value)
        {
            bool returned;
            if (value < 10 || value > 99)
            {
                returned = false;
            }
            else
            {
                int firstDigit = value / 10;
                int secondDigit = value % 10;
                if (firstDigit == secondDigit)
                    returned = true;
                else
                    returned = false;
            }
            return returned;
        }

        public static void InSquare(ref int value)
        {
            if (value >= 0)
            {
                try
                {
                    checked { value = value * value; }
                }
                catch (OverflowException)
                { }
            }
        }

        public static int PositivePairs(int[] mas)
        {
            if (mas == null || mas.Length < 2)
                return 0;

            int quantityPairs = 0;
            for (int i = 0; i < mas.Length - 1; i++)
            {
                if (mas[i] > 0 && mas[i + 1] > 0)
                {
                    quantityPairs++;
                }
            }
            return quantityPairs;
        }

        public static void SumAndMultiply(out int sum, out int multiply, int[,] matr, int columnIndex)
        {
            sum = 0;
            multiply = 1;

            // Проверка на корректность columnIndex
            if (columnIndex < 0 || columnIndex >= matr.GetLength(1))
            {
                multiply = 0;
                return;
            }

            // Обходим все строки в заданном столбце
            for (int row = 0; row < matr.GetLength(0); row++)
            {
                int value = matr[row, columnIndex];
                sum += value;
                multiply *= value;
            }
        }
    }
}