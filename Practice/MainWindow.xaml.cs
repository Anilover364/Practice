using System.Data;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Media;

namespace Practice
{
    public partial class MainWindow : Window
    {
        public MainWindow()
        {
            InitializeComponent();
        }

        // Поля для хранения данных заданий
        int[] array; // Массив для задания 3
        int[,] matrix; // Матрица для задания 4
        int arrayQuantityColumns = 5; // Количество столбцов в массиве
        int matrixQuantityColumns = 5; // Количество столбцов в матрице
        int matrixQuantityRows = 5; // Количество строк в матрице

        // Константы для ограничений ввода
        const int MINVALUE = 3; // Минимальное значение для настроек
        const int MAXVALUE = 10; // Максимальное значение для настроек
        const int STEP = 1; // Шаг изменения значений

        /// <summary>
        /// Чек для отслеживания программных изменений текста и предотвращения рекурсивных вызовов TextChanged
        /// </summary>
        private bool isProgrammaticTextChange = false;

        // Текстовые описания заданий
        string firstTaskText = "Ввести двузначное число. Определить: одинаковы ли его цифры.";
        string secondTaskText = "Ввести три целых числа. Возвести в квадрат те из них, значения которых неотрицательны.";
        string thirdTaskText = "Выяснить, имеются ли в данном массиве 2 идущих подряд положительных элемента. Подсчитать количество таких пар.";
        string foughtTaskText = "Дана матрица размера M * N и целое число K (1 < K < N). Найти сумму и произведение элементов K-го столбца данной матрицы.";

        /// <summary>
        /// Заполнение массива случайными числами
        /// </summary>
        private void FillArray()
        {
            Random r = new Random();
            DataTable dt = new DataTable();

            // Создание столбцов таблицы
            for (int i = 0; i < arrayQuantityColumns; i++)
            {
                dt.Columns.Add($"Кол {i + 1}");
            }

            array = new int[arrayQuantityColumns];
            DataRow row = dt.NewRow();

            // Заполнение массива
            for (int i = 0; i < array.Length; i++)
            {
                array[i] = r.Next(-5, 6);
                row[i] = array[i];
            }

            dt.Rows.Add(row);
            dgArray.ItemsSource = dt.DefaultView;
        }

        /// <summary>
        /// Заполнение матрицы случайными числами
        /// </summary>
        private void FillMatrix()
        {
            Random r = new Random();
            DataTable dt = new DataTable();
            matrix = new int[matrixQuantityRows, matrixQuantityColumns];

            // Создание столбцов таблицы
            for (int col = 0; col < matrixQuantityColumns; col++)
            {
                dt.Columns.Add($"Колонка {col + 1}", typeof(int));
            }

            // Заполнение матрицы
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

        /// <summary>
        /// Задание 1: Проверка двузначного числа
        /// Проверяет, одинаковы ли цифры в двузначном числе
        /// </summary>
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
                txtFirstTaskOutput.Text = $"Цифры числа {value} одинаковы";
            else
                txtFirstTaskOutput.Text = $"Цифры числа {value} не одинаковы";
        }

        /// <summary>
        /// Задание 2: Возведение чисел в квадрат
        /// Берет три числа из текстовых полей
        /// Возводит в квадрат неотрицательные числа
        /// </summary>
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

                if (originalValue != value)
                {
                    // Флаг предотвращает обработку TextChanged при программном изменении
                    isProgrammaticTextChange = true;
                    textBox.Text = value.ToString();
                    isProgrammaticTextChange = false;
                    anyChanged = true;
                }
            }

            if (anyChanged)
                MessageBox.Show("Неотрицательные числа, которые не могли бы выйти за максимальный предел были возведены в квадрат");
            else
                MessageBox.Show("Не было неотрицательных чисел или чисел, которые не могли бы выйти за максимальный предел");
        }

        /// <summary>
        /// Задание 3: Поиск положительных пар в массиве
        /// Подсчитывает количество пар соседних положительных элементов
        /// </summary>
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
                txtThirdTaskOutput.Text = $"В массиве {quantityPairs} положительных пар";
            else
                txtThirdTaskOutput.Text = "В массиве нет положительных пар";
        }

        /// <summary>
        /// Задание 4: Работа с матрицей
        /// Выбирает случайный столбец K (1 < K < N)
        /// Вычисляет сумму и произведение элементов столбца
        /// </summary>
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
            txtFourthTaskOutput.Text = $"Сумма {k}-й колонки = {sum}\nПроизведение {k}-й колонки = {multiply}";
        }

        /// <summary>
        /// Обработка прокрутки колесика мыши для изменения значения настроек с учетом ограничений
        /// </summary>
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

        /// <summary>
        /// Скрытие водяного знака при фокусировке
        /// </summary>
        private void HideWatermark(object sender, RoutedEventArgs e)
        {
            var send = sender as TextBox;
            if (send.Text == send.Tag.ToString())
            {
                isProgrammaticTextChange = true;
                send.Text = string.Empty;
                send.Foreground = Brushes.Black;
                isProgrammaticTextChange = false;
            }
        }

        /// <summary>
        /// Показание водяного знака при потере фокусирвке
        /// </summary>
        private void ShowWatermark(object sender, RoutedEventArgs e)
        {
            var send = sender as TextBox;
            if (string.IsNullOrWhiteSpace(send.Text) || send.Text == send.Tag.ToString())
            {
                isProgrammaticTextChange = true;
                send.Text = send.Tag.ToString();
                send.Foreground = Brushes.Gray;
                isProgrammaticTextChange = false;
            }
            send.Text = send.Text.Trim();
        }

        /// <summary>
        /// Обработчик кликов по кнопкам
        /// </summary>
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

        /// <summary>
        /// Обработчик прокрутки для текстовых полей настроек
        /// </summary>
        private void TextBox_MouseWheel(object sender, System.Windows.Input.MouseWheelEventArgs e)
        {
            var send = sender as TextBox;
            switch (send.Name)
            {
                case "txtArrayQuantityColumns":
                    ScrollValue(e, ref arrayQuantityColumns, MINVALUE, MAXVALUE, STEP);
                    isProgrammaticTextChange = true;
                    send.Text = send.Tag.ToString() + arrayQuantityColumns;
                    isProgrammaticTextChange = false;
                    break;
                case "txtMatrixQuantityColumns":
                    ScrollValue(e, ref matrixQuantityColumns, MINVALUE, MAXVALUE, STEP);
                    isProgrammaticTextChange = true;
                    send.Text = send.Tag.ToString() + matrixQuantityColumns;
                    isProgrammaticTextChange = false;
                    break;
                case "txtMatrixQuantityRows":
                    ScrollValue(e, ref matrixQuantityRows, MINVALUE, MAXVALUE, STEP);
                    isProgrammaticTextChange = true;
                    send.Text = send.Tag.ToString() + matrixQuantityRows;
                    isProgrammaticTextChange = false;
                    break;
            }
        }

        /// <summary>
        /// Обработчик кликов по пунктам меню
        /// </summary>
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

        /// <summary>
        /// Обработчик изменения текста
        /// </summary>
        private void TextBox_TextChanged(object sender, TextChangedEventArgs e)
        {
            // Пропускаем обработку если изменение программное
            if (isProgrammaticTextChange) return;

            var send = sender as TextBox;
            if (send.Tag == null) return;
            int maxLength = 5;

            // Проверяем, не является ли текущий текст водяным знаком
            if (send.Text == send.Tag?.ToString())
                return;

            // Основная логика ограничения длины
            if (send.Text.Length > maxLength)
            {
                // Устанавливаем чек чтобы следующее изменение не обрабатывалось
                isProgrammaticTextChange = true;

                try
                {
                    // Сохраняем позицию курсора для правильного восстановления
                    int cursorPos = send.SelectionStart;

                    // Обрезаем текст до допустимой длины
                    send.Text = send.Text.Substring(0, maxLength);

                    // Восстанавливаем позицию курсора
                    send.SelectionStart = Math.Min(cursorPos, maxLength);
                    MessageBox.Show("Максимальное количество символов, которых можно ввести = 5");
                }
                finally
                {
                    // Сбрасываем чек, даже при возникновении исключения
                    isProgrammaticTextChange = false;
                }
            }
        }
    }

    /// <summary>
    /// Содержит методы для всех 4 заданий
    /// </summary>
    public class Operations
    {
        /// <summary>
        /// Сравнение цифр двузначного числа
        /// </summary>
        public static bool Compare(int value)
        {
            // Проверка что число действительно двузначное
            if (value < 10 || value > 99)
            {
                return false;
            }
            else
            {
                // Извлечение цифр числа
                int firstDigit = value / 10; // Десятки
                int secondDigit = value % 10; // Единицы
                return firstDigit == secondDigit;
            }
        }

        /// <summary>
        /// Возведение числа в квадрат
        /// </summary>
        public static void InSquare(ref int value)
        {
            if (value >= 0)
            {
                try
                {
                    // Проверка на переполнение
                    checked { value = value * value; }
                }
                catch (OverflowException)
                {
                    // При переполнении значение не изменяется
                }
            }
        }

        /// <summary>
        /// Подсчет пар положительных соседних элементов в массиве
        /// </summary>
        public static int PositivePairs(int[] mas)
        {
            // Защита от некорректных данных
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

        /// <summary>
        /// Вычисление суммы и произведения элементов столбца матрицы
        /// </summary>
        public static void SumAndMultiply(out int sum, out int multiply, int[,] matr, int k)
        {
            sum = 0;
            multiply = 1;
            if (k < 0 || k >= matr.GetLength(1))
            {
                multiply = 0;
                return;
            }
            for (int row = 0; row < matr.GetLength(0); row++)
            {
                int value = matr[row, k];
                sum += value;
                multiply *= value;
            }
        }
    }
}