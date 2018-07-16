using System;
using System.Collections;
using System.Collections.Generic;
//using System.Linq;
using System.IO;
//using System.Threading.Tasks;

namespace MatrixVectorLib
{
    public class Vector
    {
        double[] arr;
        /// <summary>
        /// Кол-во элементов в векторе = его размерность
        /// длина вектора как массива
        /// </summary>
        public int Count { get => arr.Length; }
        /// <summary>
        /// Длинна вектора в Евклидовом пр-ве
        /// </summary>
        public double Length
        {
            get
            {
                double len = 0;
                for (int i = 0; i < Count; i++)
                    len += arr[i] * arr[i];
                return Math.Sqrt(len);
            }
        }
        public Vector(int size = 10)
        {
            arr = new double[size];
        }
        public Vector(double[] ar)
        {
            arr = new double[ar.Length];
            for (int i = 0; i < Count; i++)
                arr[i] = ar[i];
        }
        /// <summary>
        /// Конструктор копирования
        /// </summary>
        /// <param name="a">копируемый веткор</param>
        public Vector(Vector a)
        {
            arr = new double[a.Count];
            for (int i = 0; i < Count; i++)
                arr[i] = a[i];
        }
        /// <summary>
        /// Создание вектора со случайными числами заданного рамера
        /// </summary>
        /// <param name="size">размер вектора</param>
        /// <returns></returns>
        public static Vector CreateRandomVector(int size)
        {
            Vector res = new Vector(size);
            Random r = new Random();
            for (int i = 0; i < size; i++)
                res[i] = r.Next(-10, 10) + r.NextDouble();
            return res;
        }
        /// <summary>
        /// Преобразование вектора в строку для вывода
        /// </summary>
        /// <returns></returns>
        public override string ToString()
        {
            string res = "{";
            for (int i = 0; i < Count; i++)
                res += String.Format("{0, 6:f}", arr[i]) + (i == Count - 1 ? " }" : ", ");
            return res;
        }
        /// <summary>
        /// Загрузка вектора из файла. Не испоьзуется
        /// </summary>
        /// <param name="sr">Поток файла</param>
        /// <returns></returns>
        public static Vector LoadVector(StreamReader sr)
        {
            int.TryParse(sr.ReadLine(), out int n);
            double[] vec = new double[n];
            for (int i = 0; i < n; i++)
                double.TryParse(sr.ReadLine(), out vec[i]);
            return new Vector(vec);
        }
        /// <summary>
        /// Нормализация вектора - деление на его длину
        /// </summary>
        public void Normalize()
        {
            double len = Length;
            for (int i = 0; i < Count; i++)
                arr[i] /= len;
        }
        /// <summary>
        /// Сортировка вектор в порядке убывания
        /// </summary>
        /// <returns>отсортированный вектор</returns>
        public Vector Sort()
        {
            Array.Sort(arr);
            Array.Reverse(arr);
            return this;
        }

        /// <summary>
        /// Печать вектора
        /// </summary>
        /// <param name="writer">с пом. чгео выводим</param>
        public void Print(TextWriter writer)
        {
            writer.WriteLine(this);
        }
        /// <summary>
        /// Автоматически созданный метод для сравнения
        /// </summary>
        /// <param name="obj"></param>
        /// <returns></returns>
        public override bool Equals(object obj)
        {
            var vector = obj as Vector;
            return vector != null &&
                   EqualityComparer<double[]>.Default.Equals(arr, vector.arr) &&
                   Count == vector.Count;
        }
        /// <summary>
        /// Автоматически созданный метод для быстрого сравннеия объектов
        /// </summary>
        /// <returns></returns>
        public override int GetHashCode()
        {
            var hashCode = 889275500;
            hashCode = hashCode * -1521134295 + EqualityComparer<double[]>.Default.GetHashCode(arr);
            hashCode = hashCode * -1521134295 + Count.GetHashCode();
            return hashCode;
        }


        public double Sum()
        {
            var res = 0.0;
            for (int i = 0; i < Count; i++)
                res += this[i];
            return res;
        }
        public double Middle()
        {
            return Sum() / Count;
        }

        public double Dispersion()
        {
            double mid = Middle();
            double sum = 0.0;
            for (int i = 0; i < Count; i++)
                sum += Math.Pow(arr[i] - mid, 2);
            return sum / Count;
        }

        #region Operators
        /// <summary>
        /// Индексатор - доступ к i-тому элементу вектора(координате)
        /// </summary>
        /// <param name="i">индекс элемента</param>
        /// <returns></returns>
        public double this[int i]
        {
            get => arr[i];
            set => arr[i] = value;
        }
        /// <summary>
        /// Приведение вектора в массиву даблов
        /// </summary>
        /// <param name="vec"></param>
        public static explicit operator double[] (Vector vec)
        {
            double[] arr = new double[vec.Count];
            for (int i = 0; i < vec.Count; i++)
                arr[i] = vec[i];
            return arr;
        }
        /// <summary>
        /// Приведение массива даблов к вектору
        /// </summary>
        /// <param name="arr"></param>
        public static implicit operator Vector(double[] arr)
        {
            return new Vector(arr);
        }
        /// <summary>
        /// Сложение векторов
        /// </summary>
        /// <param name="a"></param>
        /// <param name="b"></param>
        /// <returns></returns>
        public static Vector operator +(Vector a, Vector b)
        {
            Vector c = new Vector(a.Count);
            for (int i = 0; i < a.Count; i++)
                c[i] = a[i] + b[i];
            return c;
        }

        public static bool operator ==(Vector a, Vector b)
        {
            if (a.Count != b.Count)
                return false;
            for (int i = 0; i < a.Count; i++)
                if (Math.Abs(a[i] - b[i]) > 1E-6)
                    return false;
            return true;
        }
        public static bool operator !=(Vector a, Vector b)
        {
            if (a.Count != b.Count)
                return true;
            for (int i = 0; i < a.Count; i++)
                if (Math.Abs(a[i] - b[i]) > 1E-6)
                    return true;
            return false;
        }
        /// <summary>
        /// Вычитание векторов
        /// </summary>
        /// <param name="a"></param>
        /// <param name="b"></param>
        /// <returns></returns>
        public static Vector operator -(Vector a, Vector b)
        {
            Vector c = new Vector(a.Count);
            for (int i = 0; i < a.Count; i++)
                c[i] = a[i] - b[i];
            return c;
        }
        /// <summary>
        /// Умножение вектора на число
        /// </summary>
        /// <param name="vec"></param>
        /// <param name="val"></param>
        /// <returns></returns>
        public static Vector operator *(Vector vec, double val)
        {
            var res = new Vector(vec.Count);
            for (int i = 0; i < vec.Count; i++)
                res[i] = vec[i] * val;
            return res;
        }
        /// <summary>
        /// Тоже умножеине вектора на число
        /// </summary>
        /// <param name="val"></param>
        /// <param name="vec"></param>
        /// <returns></returns>
        public static Vector operator *(double val, Vector vec)
        {
            var res = new Vector(vec.Count);
            for (int i = 0; i < vec.Count; i++)
                res[i] = vec[i] * val;
            return res;
        }
        /// <summary>
        /// Скалярное пр-е векторов
        /// </summary>
        /// <param name="vec1"></param>
        /// <param name="vec2"></param>
        /// <returns></returns>
        public static double operator *(Vector vec1, Vector vec2)
        {
            double sum = 0;
            for (int i = 0; i < vec1.Count; i++)
                sum += vec1[i] * vec2[i];
            return sum;
        }
        /// <summary>
        /// Умножеине вектора на матрицу
        /// </summary>
        /// <param name="vec"></param>
        /// <param name="mat"></param>
        /// <returns></returns>
        public static Vector operator *(Vector vec, Matrix mat)
        {
            if (mat.M != vec.Count)
                throw new ArgumentException();
            Vector res = new Vector(mat.N);
            for (int i = 0; i < res.Count; i++)
            {
                double sum = 0;
                for (int j = 0; j < vec.Count; j++)
                    sum += vec[j] * mat[j, i];
                res[i] = sum;
            }
            return res;
        }
        /// <summary>
        /// Умножение матрицы на веткор
        /// </summary>
        /// <param name="mat"></param>
        /// <param name="vec"></param>
        /// <returns></returns>
        public static Vector operator *(Matrix mat, Vector vec)
        {
            if (mat.N != vec.Count)
                throw new ArgumentException();
            Vector res = new Vector(mat.M);
            for (int i = 0; i < res.Count; i++)
            {
                double sum = 0;
                for (int j = 0; j < vec.Count; j++)
                    sum += vec[j] * mat[i, j];
                res[i] = sum;
            }
            return res;
        }
        #endregion


    }
}
