using System;
using System.Text;

namespace MatrixLibrary
{
    public class Matrix
    {
        private readonly double[,] _data;

        public int Rows => _data.GetLength(0);
        public int Cols => _data.GetLength(1);

        // Индексатор
        public double this[int r, int c]
        {
            get
            {
                ValidateIndices(r, c);
                return _data[r, c];
            }
            set
            {
                ValidateIndices(r, c);
                _data[r, c] = value;
            }
        }

        // Конструктор по умолчанию (1х1)
        public Matrix() : this(1, 1) { }

        // Конструктор по размерам
        public Matrix(int rows, int cols)
        {
            if (rows <= 0 || cols <= 0)
                throw new ArgumentException("Matrix dimensions must be strictly greater than zero.");

            _data = new double[rows, cols];
        }

        // Конструктор из двумерного массива
        public Matrix(double[,] data)
        {
            if (data == null)
                throw new ArgumentNullException(nameof(data), "Array cannot be null.");

            int rows = data.GetLength(0);
            int cols = data.GetLength(1);

            if (rows == 0 || cols == 0)
                throw new ArgumentException("Array cannot be empty.");

            _data = new double[rows, cols];
            for (int i = 0; i < rows; i++)
                for (int j = 0; j < cols; j++)
                    _data[i, j] = data[i, j];
        }

        // Конструктор копирования
        public Matrix(Matrix other)
        {
            if (other == null)
                throw new ArgumentNullException(nameof(other), "Object to copy cannot be null.");

            _data = new double[other.Rows, other.Cols];
            for (int i = 0; i < other.Rows; i++)
                for (int j = 0; j < other.Cols; j++)
                    _data[i, j] = other._data[i, j];
        }

        // Сложение матриц
        public static Matrix operator +(Matrix a, Matrix b)
        {
            CheckNull(a, b);
            if (a.Rows != b.Rows || a.Cols != b.Cols)
                throw new ArgumentException("Matrices must have identical dimensions for addition.");

            Matrix result = new Matrix(a.Rows, a.Cols);
            for (int i = 0; i < a.Rows; i++)
                for (int j = 0; j < a.Cols; j++)
                    result[i, j] = a[i, j] + b[i, j];

            return result;
        }

        // Умножение матриц
        public static Matrix operator *(Matrix a, Matrix b)
        {
            CheckNull(a, b);
            if (a.Cols != b.Rows)
                throw new ArgumentException("Number of columns of the first matrix must match the number of rows of the second matrix.");

            Matrix result = new Matrix(a.Rows, b.Cols);
            for (int i = 0; i < a.Rows; i++)
            {
                for (int j = 0; j < b.Cols; j++)
                {
                    double sum = 0;
                    for (int k = 0; k < a.Cols; k++)
                        sum += a[i, k] * b[k, j];
                    result[i, j] = sum;
                }
            }
            return result;
        }

        // Умножение матрицы на число
        public static Matrix operator *(Matrix a, double scalar)
        {
            if (a is null) throw new ArgumentNullException(nameof(a));
            Matrix result = new Matrix(a.Rows, a.Cols);
            for (int i = 0; i < a.Rows; i++)
                for (int j = 0; j < a.Cols; j++)
                    result[i, j] = a[i, j] * scalar;

            return result;
        }

        public static Matrix operator *(double scalar, Matrix a) => a * scalar;

        // Транспонирование (оператор !)
        public static Matrix operator !(Matrix a)
        {
            if (a is null) throw new ArgumentNullException(nameof(a));
            Matrix result = new Matrix(a.Cols, a.Rows);
            for (int i = 0; i < a.Rows; i++)
                for (int j = 0; j < a.Cols; j++)
                    result[j, i] = a[i, j];

            return result;
        }

        // Операторы true / false
        public static bool operator true(Matrix a)
        {
            if (a is null) return false;
            for (int i = 0; i < a.Rows; i++)
                for (int j = 0; j < a.Cols; j++)
                    if (Math.Abs(a[i, j]) > 1e-9)
                        return true;
            return false;
        }

        public static bool operator false(Matrix a)
        {
            if (a is null) return true;
            for (int i = 0; i < a.Rows; i++)
                for (int j = 0; j < a.Cols; j++)
                    if (Math.Abs(a[i, j]) > 1e-9)
                        return false;
            return true;
        }

        // Неявное приведение: double[,] -> Matrix
        public static implicit operator Matrix(double[,] array) => new Matrix(array);

        // Явное приведение: Matrix -> double (норма матрицы)
        public static explicit operator double(Matrix a)
        {
            if (a is null) throw new ArgumentNullException(nameof(a));
            double sum = 0;
            for (int i = 0; i < a.Rows; i++)
                for (int j = 0; j < a.Cols; j++)
                    sum += a[i, j] * a[i, j];
            return Math.Sqrt(sum);
        }

        // Операторы сравнения на равенство
        public static bool operator ==(Matrix a, Matrix b)
        {
            if (ReferenceEquals(a, b)) return true;
            if (a is null || b is null) return false;
            if (a.Rows != b.Rows || a.Cols != b.Cols) return false;

            for (int i = 0; i < a.Rows; i++)
                for (int j = 0; j < a.Cols; j++)
                    if (Math.Abs(a[i, j] - b[i, j]) > 1e-6)
                        return false;

            return true;
        }

        public static bool operator !=(Matrix a, Matrix b) => !(a == b);

        // Операторы порядка (на основе нормы матрицы)
        public static bool operator <(Matrix a, Matrix b) => (double)a < (double)b;
        public static bool operator >(Matrix a, Matrix b) => (double)a > (double)b;
        public static bool operator <=(Matrix a, Matrix b) => (double)a <= (double)b;
        public static bool operator >=(Matrix a, Matrix b) => (double)a >= (double)b;

        public override bool Equals(object obj) => obj is Matrix m && this == m;

        public override int GetHashCode()
        {
            unchecked
            {
                int hash = 17;
                hash = hash * 23 + Rows.GetHashCode();
                hash = hash * 23 + Cols.GetHashCode();
                for (int i = 0; i < Math.Min(Rows, 3); i++)
                {
                    for (int j = 0; j < Math.Min(Cols, 3); j++)
                    {
                        hash = hash * 23 + _data[i, j].GetHashCode();
                    }
                }
                return hash;
            }
        }

        public override string ToString()
        {
            StringBuilder sb = new StringBuilder();
            for (int i = 0; i < Rows; i++)
            {
                sb.Append("| ");
                for (int j = 0; j < Cols; j++)
                {
                    sb.Append($"{_data[i, j],7:F2} ");
                }
                sb.AppendLine("|");
            }
            return sb.ToString();
        }

        // Функция ввода из консоли
        public static Matrix Input()
        {
            Console.Write("Enter number of rows: ");
            int rows = int.Parse(Console.ReadLine() ?? "1");
            Console.Write("Enter number of columns: ");
            int cols = int.Parse(Console.ReadLine() ?? "1");

            Matrix m = new Matrix(rows, cols);
            Console.WriteLine("Enter elements row by row separated by spaces:");
            for (int i = 0; i < rows; i++)
            {
                string line = Console.ReadLine() ?? "";
                string[] parts = line.Split(new char[] { ' ' }, StringSplitOptions.RemoveEmptyEntries);
                for (int j = 0; j < cols; j++)
                {
                    m[i, j] = j < parts.Length ? double.Parse(parts[j]) : 0.0;
                }
            }
            return m;
        }

        private void ValidateIndices(int r, int c)
        {
            if (r < 0 || r >= Rows || c < 0 || c >= Cols)
                throw new IndexOutOfRangeException($"Index [{r}, {c}] is out of bounds for matrix {Rows}x{Cols}.");
        }

        private static void CheckNull(Matrix a, Matrix b)
        {
            if (a is null) throw new ArgumentNullException(nameof(a));
            if (b is null) throw new ArgumentNullException(nameof(b));
        }
    }
}