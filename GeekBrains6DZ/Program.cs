using System;
using System.IO;
using System.Collections.Generic;
namespace DoubleBinary
{

    //Модифицировать программу нахождения минимума функции так, чтобы можно было
    //передавать функцию в виде делегата.
    //а) Сделать меню с различными функциями и представить пользователю выбор, для какой
    //функции и на каком отрезке находить минимум.
    //б) Использовать массив(или список) делегатов, в котором хранятся различные функции.
    //в) * Переделать функцию Load, чтобы она возвращала массив считанных значений.Пусть она
    //возвращает минимум через параметр.

    //Объявление делегата
    public delegate double Fun(double x);
    class Program
    {

        static double min = int.MaxValue;
        /// <summary>
        /// Математическая функция x * x - 50 * x + 10
        /// </summary>
        /// <param name="x">x</param>
        /// <returns>x * x - 50 * x + 10</returns>
        public static double Func1(double x)
        {
            return x * x - 50 * x + 10;
        }
        /// <summary>
        /// Математическая функция x * x - 50
        /// </summary>
        /// <param name="x"></param>
        /// <returns>x * x - 50</returns>
        public static double Func2(double x)
        {
            return x * x - 50;
        }

        /// <summary>
        /// Записывает в файл результаты выполнения функции с указанными началом, концом и шагом
        /// </summary>
        /// <param name="F">Математическая функция, что непосредственно вычисляется</param>
        /// <param name="fileName">Название создаваемого файла, куда всё и запишется</param>
        /// <param name="a">Начало функции</param>
        /// <param name="b">Конец функции</param>
        /// <param name="h">Шаг</param>
        public static void SaveFunc(Fun F, string fileName, double a, double b, double h)
        {
            FileStream fs = new FileStream(fileName, FileMode.Create, FileAccess.Write);
            BinaryWriter bw = new BinaryWriter(fs);
            double x = a;
            while (x <= b)
            {
                bw.Write(F(x));
                x += h;
            }
            bw.Close();
            fs.Close();
        }

        /// <summary>
        /// Считывает бинарный файл построчно, возвращает минимальное число оттуда
        /// </summary>
        /// <param name="fileName">путь к файлу откуда читать</param>
        /// <returns>double число, минимальное из содержимого файла (построчо)</returns>
        public static double[] Load(string fileName, ref double min)
        {
            FileStream fs = new FileStream(fileName, FileMode.Open, FileAccess.Read);
            BinaryReader bw = new BinaryReader(fs);
            double[] d = new double[fs.Length / sizeof(double)];
            for (int i = 0; i < fs.Length / sizeof(double); i++)
            {
                // Считываем значение и переходим к следующему
                d[i] = bw.ReadDouble();
                if (d[i] < min) min = d[i];
            }
            bw.Close();
            fs.Close();
            return d;
        }
        static void Main(string[] args)
        {
            List<Fun> func = new List<Fun>();
            func.Add(Func1);
            func.Add(Func2);
            int numberfunction = 999;
            while (numberfunction < 0 || numberfunction > func.Count - 1)
            {
                Console.WriteLine("Введите 1 или 2, чтобы соответственно использовать функцию x * x - 50 * x + 10 или  x * x - 50");
                int.TryParse(Console.ReadLine(), out numberfunction);
            }
            double a = -100, b = 100, h = 0.5;
            Console.WriteLine("Введите начальную границу, по умолчанию {0}", a);
            double.TryParse(Console.ReadLine(), out a);
            Console.WriteLine("Введите конечную границу, по умолчанию {0}", b);
            double.TryParse(Console.ReadLine(), out b);
            Console.WriteLine("Введите шаг, по умолчанию {0,2}", h);
            double.TryParse(Console.ReadLine(), out h);
            SaveFunc(func[numberfunction-1], "data.bin", a, b, h);
            Load("data.bin",ref min);
            Console.WriteLine("Минимальное значение в функции - {0}", min);
            Console.ReadKey();
        }

    }
}
