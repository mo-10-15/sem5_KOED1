using System;
using System.Collections.Generic;
using System.IO;


namespace MatrixVectorLib
{
    /// <summary>
    /// Класс матрицы из дробных чисел
    /// </summary>
    public class Matrix
    {
        /// <summary>
        /// Массив чисел
        /// </summary>
        double[,] arr;
        /// <summary>
        /// N - кол-во столбцов матрицы
        /// </summary>
        public int N { get => arr.GetLength(1); }
        /// <summary>
        /// Кол-во строк матрицы
        /// </summary>
        public int M { get => arr.GetLength(0); }

        public Matrix(int m = 5, int n = 5)
        {
            arr = new double[m, n];
        }
        public Matrix(double[,] ar)
        {
            arr = new double[ar.GetLength(0), ar.GetLength(1)];
            for (int i = 0; i < M; i++)
                for (int j = 0; j < N; j++)
                    arr[i, j] = ar[i, j];
        }
        /// <summary>
        /// Индексатор - доступ к [i, j] элементу матрицы
        /// </summary>
        /// <param name="i">индекс строки</param>
        /// <param name="j">индекс столбца</param>
        /// <returns></returns>
        public double this[int i, int j]
        {
            get => arr[i, j];
            set => arr[i, j] = value;
        }
        /// <summary>
        /// Метод получения i-той строки. Не используется
        /// </summary>
        /// <param name="i">индекс строки</param>
        /// <returns></returns>
        public Vector GetLine(int i)
        {
            Vector vec = new Vector(N);
            for (int j = 0; j < N; j++)
                vec[j] = arr[i, j];
            return vec;
        }
        /// <summary>
        /// Метод получения j-того столбца. Не используется
        /// </summary>
        /// <param name="j">индекс столбца</param>
        /// <returns></returns>
        public Vector GetColumn(int j)
        {
            Vector vec = new Vector(M);
            for (int i = 0; i < M; i++)
                vec[i] = arr[i, j];
            return vec;
        }

        public override string ToString()
        {
            string std = "{\n";
            for (int i = 0; i < M; i++)
            {
                std += "{ ";
                for (int j = 0; j < N; j++)
                {
                    std += String.Format("{0, 6:f} ", arr[i, j]).Replace(',', '.') ;

                    std += (j != N - 1) ? "," : "}";
                }

                std += (i==M-1?"\n}":",\n");
            }
           // std += "}";
            return std;
        }
        /// <summary>
        /// Вычисление определителя матрицы. Для генерация положительно-определенных матриц
        /// </summary>
        /// <returns></returns>
        public double Det
        {
            get
            {
                Matrix copy = new Matrix(this);
                for (int i = 0; i < M - 1; i++)//прямой ход метода Гаусса
                {
                    for (int j = i + 1; j < M; j++)
                    {
                        double mnoj = copy[j, i] / copy[i, i];
                        for (int k = 0; k < M; k++)
                            copy[j, k] -= mnoj * copy[i, k];
                    }
                }

                for (int i = M - 1; i > 0; i--)//обратный ход
                {
                    for (int j = i - 1; j >= 0; j--)
                    {
                        double mnoj = copy[j, i] / copy[i, i];
                        for (int k = 0; k < M; k++)
                            copy[j, k] -= mnoj * copy[i, k];
                    }
                }
                double det = 1;
                for (int i = 0; i < M; i++)
                    det *= copy[i, i];
                return det;
            }
        }
        /// <summary>
        /// Создание случайной матрицы заданного размера
        /// </summary>
        /// <param name="size">Размер матрицы</param>
        /// <returns>Сгенерированная матрица</returns>
        public static Matrix CreateRandomMatrix(int size)
        {
            Matrix m = new Matrix(size, size);
            Random r = new Random();
            for (int i = 0; i < size; i++)
                for (int j = 0; j < size; j++)
                    m[i, j] = r.Next(-5, 5) + r.NextDouble();
            return  m;

        }

        /// <summary>
        /// Создание случайной симметричной положительно-определенной матрицы заданного размера
        /// </summary>
        /// <param name="size">Размер матрицы</param>
        /// <returns>Сгенерированная матрица</returns>
        public static Matrix CreateRandomSimmetricalMatrix(int size)
        {
            Matrix m = CreateRandomMatrix(size);
            return m.Transpose() * m;

        }

        /// <summary>
        /// Проверка матрицы на симметричность
        /// </summary>
        /// <returns></returns>
        public bool IsSimmetrical()
        {
            if (M != N)
                throw new Exception("Не квадратная матрица не может быть симметричной");
            for (int i = 0; i < N; i++)
                for (int j = i + 1; j < N; j++)
                    if (arr[i, j] != arr[j, i])
                        return false;
            return true;
        }
        /// <summary>
        /// Транспонирование матрицы
        /// </summary>
        /// <returns>Транспонированная матрица</returns>
        public Matrix Transpose()
        {
            Matrix res = new Matrix(N, M);
            for (int i = 0; i < M; i++)
                for (int j = 0; j < N; j++)
                    res[j, i] = arr[i, j];
            return res;
        }

        /// <summary>
        /// Печать матрицы с помощью потока
        /// </summary>
        /// <param name="tw">поток вывода</param>
        public void PrintMatrix(TextWriter tw)
        {
            tw.WriteAsync("{");
            for(int i=0; i<M; i++)
                tw.WriteLine(GetLine(i));
            tw.WriteAsync("}");
        }

        /// <summary>
        /// Загрузка матрицы из файла
        /// </summary>
        /// <param name="sr"></param>
        /// <returns></returns>
        public static Matrix LoadMatrix(TextReader sr)
        {
            double[,] mat;
            string[] input = sr.ReadLine().Split(' ');
            if (!int.TryParse(input[0], out int m))
                throw new ArgumentException();
            if (!int.TryParse(input[1], out int n))
                throw new ArgumentException();
            mat = new double[m, n];
            for (int i = 0; i < m; i++)
            {
                input = sr.ReadLine().Split(' ');
                for (int j = 0; j < n; j++)
                    if (!double.TryParse(input[j], out mat[i, j]))
                        throw new ArgumentException();

            }
            var matrix = new Matrix(mat);
            return matrix;
        }
        /// <summary>
        /// Единичная матрица нужного размера
        /// </summary>
        /// <param name="size">размер матрицы</param>
        /// <returns></returns>
        public static Matrix E(int size)
        {
            Matrix e = new Matrix(size, size);
            for (int i = 0; i < size; i++)
                e[i, i] = 1;
            return e;
        }
        /// <summary>
        /// Автоматически созданный метод для сравннеия матриц
        /// </summary>
        /// <param name="obj"></param>
        /// <returns></returns>
        public override bool Equals(object obj)
        {
            var matrix = obj as Matrix;
            return matrix != null &&
                   EqualityComparer<double[,]>.Default.Equals(arr, matrix.arr) &&
                   N == matrix.N &&
                   M == matrix.M;
        }
        /// <summary>
        /// Автоматически созданный етод для быстрого сравнения
        /// </summary>
        /// <returns></returns>
        public override int GetHashCode()
        {
            var hashCode = 171297321;
            hashCode = hashCode * -1521134295 + EqualityComparer<double[,]>.Default.GetHashCode(arr);
            hashCode = hashCode * -1521134295 + N.GetHashCode();
            hashCode = hashCode * -1521134295 + M.GetHashCode();
            return hashCode;
        }
        /// <summary>
        /// Конструктор копирования
        /// </summary>
        /// <param name="b"></param>
        public Matrix(Matrix b)
        {
            arr = new double[b.M, b.N];
            for (int i = 0; i < b.M; i++)
                for (int j = 0; j < b.N; j++)
                    arr[i, j] = b[i, j];
        }
        /// <summary>
        /// Создание стандартизованной матрицы
        /// </summary>
        /// <param name="Z">Исходная матрица</param>
        /// <returns></returns>
        public static Matrix CreateNorm(Matrix Z)
        {
            Matrix Res = new Matrix(Z.M, Z.N);
            for(int j=0; j<Z.N; j++)
            {
                var mid = Z.GetColumn(j).Middle();
                var otkl = Math.Sqrt( Z.GetColumn(j).Dispersion());
                for (int i = 0; i < Z.M; i++)
                    Res[i, j] = (Z[i, j] - mid) / otkl;
            }

            return Res;
        }

        /// <summary>
        /// Создание ковариационной матрицы
        /// </summary>
        /// <param name="Z">Исходная матрица</param>
        /// <returns></returns>
        public static Matrix CreateCovMatrix(Matrix Z)
        {
            var Cov = new Matrix(Z.N, Z.N);
            var mids = new Vector(Z.N);
            for (int i = 0; i < Z.N; i++)
                mids[i] = Z.GetColumn(i).Middle();

            for(int i=0; i<Z.N; i++)
                for(int j=0; j<Z.N; j++)
                {
                    var sum = 0.0;
                    for (int k = 0; k < Z.M; k++)
                        sum += (Z[k, i] - mids[i]) * (Z[k, j] - mids[j]);
                    Cov[i, j] = sum / (Z.M /*- 1*/);
                }
            return Cov;
        }
        /// <summary>
        /// Создание корреляционной матрицы
        /// </summary>
        /// <param name="x"></param>
        /// <returns></returns>
        public static Matrix CreateCorr(Matrix x)
        {
            Matrix r = new Matrix(x.N, x.N);
            for (int i = 0; i < x.N; i++)
                for (int j = 0; j < x.N; j++)
                {
                    var sum = 0.0;
                    for (int k = 0; k < x.M; k++)
                        sum += x[k, i] * x[k, j];
                    r[i, j] = sum / (x.M );
                }
            return r;
        }



        #region Operators
        public static Matrix operator *(Matrix a, Matrix b)
        {
            if (a.N != b.M)
                throw new ArgumentException("Умножение матриц невозможно. Количество столбцов первой не равно кол-ву строк второй");
            Matrix res = new Matrix(a.M, b.N);
            for (int i = 0; i < a.M; i++)
                for (int j = 0; j < b.N; j++)
                {
                    double sum = 0;
                    for (int k = 0; k < a.N; k++)
                        sum += a[i, k] * b[k, j];
                    res[i, j] = sum;
                }
            return res;
        }

        public static Matrix operator +(Matrix a, Matrix b)
        {
            if (a.N != b.N || a.M != b.M)
                throw new ArgumentException("сложеине матриц разного размера");
            Matrix c = new Matrix(a.M, a.N);
            for (int i = 0; i < a.M; i++)
                for (int j = 0; j < a.N; j++)
                    c[i, j] = a[i, j] + b[i, j];
            return c;
        }

        public static bool operator ==(Matrix a, Matrix b)
        {
            if (a.N != b.N || a.M != b.M)
                return false;
            for (int i = 0; i < a.M; i++)
                for (int j = 0; j < a.N; j++)
                    if (Math.Abs(a[i, j] - b[i, j]) > 1E-10)
                        return false;
            return true;
        }
        public static bool operator !=(Matrix a, Matrix b)
        {
            if (a.N != b.N || a.M != b.M)
                return true;
            for (int i = 0; i < a.M; i++)
                for (int j = 0; j < a.N; j++)
                    if (Math.Abs(a[i, j] - b[i, j]) > 1E-10)
                        return true;
            return false;
        }

        public static Matrix operator -(Matrix a, Matrix b)
        {
            if (a.N != b.N || a.M != b.M)
                throw new ArgumentException("сложеине матриц разного размера");
            Matrix c = new Matrix(a.M, a.N);
            for (int i = 0; i < a.M; i++)
                for (int j = 0; j < a.N; j++)
                    c[i, j] = a[i, j] - b[i, j];
            return c;
        }

        public static Matrix operator *(Matrix a, double b)
        {

            Matrix c = new Matrix(a.M, a.N);
            for (int i = 0; i < a.M; i++)
                for (int j = 0; j < a.N; j++)
                    c[i, j] = a[i, j] * b;
            return c;
        }
        public static Matrix operator *(double b, Matrix a)
        {

            Matrix c = new Matrix(a.M, a.N);
            for (int i = 0; i < a.M; i++)
                for (int j = 0; j < a.N; j++)
                    c[i, j] = a[i, j] * b;
            return c;
        }


        #endregion
    }
}
