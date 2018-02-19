using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;

namespace ImModelingLab1
{
    public partial class Form1 : Form
    {
        const int outputCount=100;
        const double accuracy = 0.0000000001;

        Random random;

        double R => random.NextDouble();

        public Form1()
        {
            InitializeComponent();
            random = new Random();
        }

        private void button1_Click(object sender, EventArgs e)
        {
            double[] values = CalcuateValues(Int32.Parse(CountTextBox.Text), Double.Parse(LambdaTextBox.Text));
            Tuple<double, uint[]> tulpe = CalculateRow(values, outputCount);
            DrawChart(tulpe.Item1,tulpe.Item2);
        }

        double[] CalcuateValues(int count, double lambda)
        {
            double[] poolArray = new double[count];

            for(int i=0; i<count; i++)
            {
                poolArray[i] = (1-Math.Sqrt(1-R))/(lambda/2);
            }

            return poolArray;
        }

        Tuple<double, uint[]> CalculateRow(double[] values, int outputCount)
        {
            double maxValue = GetMaxValue(values);
            uint[] row = new uint[outputCount];
            double step = maxValue / outputCount;

            for(int i = 0; i < outputCount; i++) { row[i] = 0; }

            foreach(double a in values)
            {
                double pool = a;
                uint pos = 0;

                while (pool > step)
                {
                    pool -= step;

                    if(pool>accuracy)
                        pos++;
                }

                row[pos]++;
            }
            return Tuple.Create<double, uint[]>(step, row);
        }

        double GetMaxValue(double[] values)
        {
            double maxValue = double.MinValue;
            foreach(double a in values)
            {
                if (a > maxValue) maxValue = a;
            }
            return maxValue;
        }

        void DrawChart(double step, uint[] values)
        {
            chart1.Series[0].Points.Clear();
            double thisStep = 0;
            foreach (uint a in values)
            {
                chart1.Series[0].Points.AddXY(thisStep, a);
                thisStep += step;
            }
        }

        //ЗАЩИТА-----------------------------------------------------------------------
        double PI(double lyambda, int i, double[] x) //расчёт P[i]
        {
            return ((lyambda * x[i] - lyambda * x[i - 1]) - ((lyambda * lyambda * x[i] * x[i] / 4) - (lyambda * lyambda * x[i - 1] * x[i - 1] / 4))); //тут в определённый момент становится нечего вычитать(возвращается 0)
        }

        double GrandRandom(double random1, double lyambda, double[] x, int n)
        {
            Random r2;
            r2 = new Random();
            double random2;
            double res = 0;

            for (int i = 1; i < n; i++)
            {
                if (random1 <= PI(lyambda, i, x))
                {
                    random2 = r2.NextDouble();
                    res = x[i - 1] + random2 * (x[i] - x[i - 1]);
                    Console.WriteLine("Результат: " + res); return res;
                }
                else
                {
                    random1 = random1 - PI(lyambda, i, x);
                    continue;
                }
            }

            random2 = r2.NextDouble();
            res = x[n - 2] + random2 * (x[n - 1] - x[n - 2]);
            Console.WriteLine("Результат: " + res);

            return res;
        }

        double HI(int n, double lyambda, double[] x)
        {
            int k = Convert.ToInt32(Console.ReadLine()); //количество интервалов
            double pi;
            double interval; // "эталонное" количество чисел в интервале
            double xkvadrat = 0;
            int mi = 1;

            for (int i = 1; i < k; i++)
            {
                pi = PI(lyambda, i, x);
                interval = pi * n;
                xkvadrat = xkvadrat + Math.Pow((mi - interval), 2) / interval;
            }

            return xkvadrat;
        }

        double[] GetValuseMain(int n, double lyambda)
        {
            double xMax = 1;
            int j = 0;
            Random r1;
            double random1;

            double[] resmas = new double[n];
            double[] x = new double[n];

            //заполнение массива
            x[0] = 0;
            for (int m = 1; m < n; m++) { x[m] = x[m - 1] + xMax / n; }
            x[n - 1] = 1;

            r1 = new Random();
            

            for (int i=1; i < n; i++)
            {
                random1 = r1.NextDouble();
                resmas[i] = GrandRandom(random1, lyambda, x, n);
            }

            resmas[0] = 0;

            Console.ReadLine();

            int a = 0;
            int b = n - 1;

            while (a != b || b > a)
            {
                double t;
                t = resmas[a];
                resmas[a] = resmas[b];
                resmas[b] = t;
                a++; b--;
            }

            int k = 0;
            while (k < resmas.Length) { Console.WriteLine("Массив: :" + resmas[k]); k++; Console.WriteLine("\n"); }

            Console.ReadLine();

            return resmas;
        }
    
    private void button2_Click(object sender, EventArgs e)
        {
            double[] values = GetValuseMain(Int32.Parse(CountTextBox.Text),Double.Parse(LambdaTextBox.Text));
            //DrawChart(1 / Int32.Parse(CountTextBox.Text), values);
        }
    }
}
