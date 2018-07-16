using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using MatrixVectorLib;
using Microsoft.Office.Interop.Excel;
using System.IO;

namespace KorrAn
{
    class Program
    {

        static int StartIndex { get; set; } = 11;//номер начала чтения из таблицы
        static int LastIndex {get; set;} = 20;//номер конца чтения из экселя
        static List<string> names = new List<string>();
        static Matrix LoadFromExcel(Worksheet ObjWorkSheet)
        {

            var lastCell = ObjWorkSheet.Cells.SpecialCells(XlCellType.xlCellTypeLastCell);//1 ячейку
            string[,] list = new string[lastCell.Column, lastCell.Row]; // массив значений с листа равен по размеру листу

            List<double> nums = new List<double>();
            for (int i = 3; i <= lastCell.Row; i++) //по всем колонкам
            {
                bool haveFreeSpace = false;
                for (int j = StartIndex + 1; j <= LastIndex + 1; j++) // по всем строкам
                    if ((ObjWorkSheet.Cells[i, j]).Text.ToString() == "" || (ObjWorkSheet.Cells[i, j]).Text.ToString() == "...")
                    {
                        haveFreeSpace = true;
                        break;
                    }
                if (haveFreeSpace)
                    continue;

                for (int j = StartIndex + 1; j <= LastIndex + 1; j++) // по всем строкам
                    nums.Add(double.Parse(ObjWorkSheet.Cells[i, j].Text.ToString()));

            }
            for (int j = StartIndex + 1; j <= LastIndex + 1; j++) // по всем строкам
                names.Add(ObjWorkSheet.Cells[1, j].Text.ToString());
            Matrix mat = new Matrix(nums.Count / 10, 10);
            for (int i = 0; i < nums.Count; i++)
                mat[i / 10, i % 10] = nums[i];



            return mat;
        }

        static void Main(string[] args)
        {

            Application ObjWorkExcel = new Application(); //открыть эксель
            Console.WriteLine("Начато");
            Workbook ObjWorkBook = ObjWorkExcel.Workbooks.Open(Filename: @"C:\Users\Миша\source\repos\KorrAn\KorrAn\исходные_данные_для_лаб.xls", ReadOnly: true); //открыть файл
            var ObjWorkSheet = (Worksheet)ObjWorkBook.Sheets[1];
            Console.WriteLine("Загрузка матрицы из Excel");
            Matrix a = LoadFromExcel(ObjWorkSheet);
            Console.WriteLine("Матрица готова");
            Console.WriteLine(a);
            ObjWorkBook.Close();

            StreamWriter[] sw =
            {
                new StreamWriter(@"C:\Users\Миша\source\repos\KorrAn\KorrAn\OutputZ.txt"),
                new StreamWriter(@"C:\Users\Миша\source\repos\KorrAn\KorrAn\OutputX.txt"),
                new StreamWriter(@"C:\Users\Миша\source\repos\KorrAn\KorrAn\OutputR.txt"),
                new StreamWriter(@"C:\Users\Миша\source\repos\KorrAn\KorrAn\OutputE.txt")

            };
            var x = Matrix.CreateNorm(a);
            Matrix[] matrixs =
            {
                a,
                x,
                Matrix.CreateCovMatrix(a),
                Matrix.CreateCorr(x)
            };
            for (int i = 0; i < 4; i++)
                matrixs[i].PrintMatrix(sw[i]);
            for (int i = 0; i < 4; i++)
                sw[i].Close();
            Console.WriteLine("Матрицы получены");
            Console.WriteLine("Проверка свойств стандартизованной матрицы");
            for (int i = 0; i < x.N; i++)
                Console.WriteLine("среднее по столбике=" + x.GetColumn(i).Middle());
            for (int i = 0; i < x.N; i++)
                Console.WriteLine("СКО=" + x.GetColumn(i).Dispersion());

            TextWriter res = new StreamWriter(@"C:\Users\Миша\source\repos\KorrAn\KorrAn\Result.txt");

            
            double t_tabl = 1.9929971;// для alph = 0.05;
            var R = matrixs[3];
            Console.WriteLine("Проверка корреляции");

            for (int i = 0; i < R.M; i++)
                for (int j = i + 1; j < R.N; j++)
                {
                    var t = R[i, j] * Math.Sqrt(a.M - 2) / Math.Sqrt(1 - R[i, j] * R[i, j]);
                    if (Math.Abs(t) >= t_tabl)
                    {
                        var resu = "Корреляция между " + names[i] + " и " + names[j];
                        Console.WriteLine(resu);
                        res.WriteLine(resu);
                    }
                }
            res.Close();

            Console.ReadKey();
        }
    }
}
