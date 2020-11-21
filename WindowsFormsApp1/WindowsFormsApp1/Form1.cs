using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.IO;
using System.Linq;
using System.Runtime.InteropServices;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;
using System.Windows.Forms.DataVisualization.Charting;

namespace WindowsFormsApp1
{
    public partial class Form1 : Form
    {
        public Form1()
        {
            InitializeComponent();
            
            if (!System.IO.File.Exists(fileLibrary)) //проверяем существует ли библиотека
            {
                MessageBox.Show("Библиотека " + fileLibrary + " не найдена!");
                button3.Enabled = false;
                

            }


        }


        const string ap = "andrey parfenov";



        double Opredelitel(double[,] a)
        {
            double otv = a[0, 0] * a[1, 1] * a[2, 2] 
                + a[2,0] * a[0,1] * a[1,2] 
                + a[1,0] * a[2,1] * a[0,2]
                - a[2,0] * a[1,1] * a[0,2]
                - a[1,0] * a[0,1] * a[2,2]
                - a[0,0] * a[1,2] * a[2,1];

            return otv;
        }

       

        private void button3_Click(object sender, EventArgs e)
        {
            Axis ax = new Axis();
            ax.Title = "X";
            chart1.ChartAreas[0].AxisX = ax;
            chart2.ChartAreas[0].AxisX = ax;
            Axis ay = new Axis();
            ay.Title = "Y";
            chart1.ChartAreas[0].AxisY = ay;
            chart2.ChartAreas[0].AxisY = ay;

            chart1.Series["Series1"].LegendText = "График TheFunc";
            chart2.Series["Series1"].LegendText = "График a,b,c";

           

            GetDate(); //находим коэффициенты a,b,c и строим график

            FirstGr();//график строим обращаясь к функции TheFunc библиотеки

        }


        void FirstGr()//график строим обращаясь к функции TheFunc библиотеки
        {

            chart1.Series[0].ChartType = SeriesChartType.Line;
            

            double x = 0;
            do
            {


                double a = TheFunc(ap, x);
                
                chart1.Series[0].Points.AddXY(x, a);
                x = x + 0.5;


            } while (x <= 10.0 + 0.00001);


        }



        const string fileLibrary = @"c:\0\Lib2-1.dll";
        [DllImport(fileLibrary, CharSet = CharSet.Ansi, CallingConvention = CallingConvention.Cdecl)]


        public static extern double TheFunc(String caption, double type);

        void GetDate()//находим коэффициенты a,b,c и строим график
        {
            //берём  три произвольных значений х

            double x1 = 0.0;
            double x2 = 1.0;
            double x3 = 2.0;

            // получаем для них значения Y
            double y1 = TheFunc(ap , x1);
            double y2 = TheFunc(ap , x2);
            double y3 = TheFunc(ap , x3);

            // находим a,b,c, Методом Крамера
            double[,] op =
                {   { x1* x1,   x1, 1},
                    { x2* x2,   x2, 1},
                    { x3* x3,   x3, 1}
            };

            double[,] op1 =
                {   { y1, x1, 1},
                    { y2, x2, 1},
                    { y3, x3, 1}
            };

            double[,] op2 =
                {   { x1 *x1,   y1, 1},
                    { x2 *x2,   y2, 1},
                    { x3 *x3,   y3, 1}
            };

            double[,] op3 =
                {   { x1* x1,   x1, y1},
                    { x2* x2,   x2, y2},
                    { x3* x3,   x3, y3}
            };

            textBox1.Text = "Находим коэффициенты:";

            double a = Opredelitel(op1) / Opredelitel(op);
            textBox1.Text = textBox1.Text + Environment.NewLine + "a=" + a.ToString();

            double b = Opredelitel(op2) / Opredelitel(op);
            textBox1.Text = textBox1.Text + Environment.NewLine + "b=" + b.ToString();

            double c = Opredelitel(op3) / Opredelitel(op);
            textBox1.Text = textBox1.Text + Environment.NewLine + "c=" + c.ToString();

            //выводим формулу в легенду графика
            chart2.Series["Series1"].LegendText = chart2.Series["Series1"].LegendText + Environment.NewLine
                + a.ToString() + "x^2" + b.ToString() + "x"+c.ToString()+"=0";

            //стрим график

            chart2.Series[0].ChartType = SeriesChartType.Line;

            double x = 0;
            do
            {

                double y = a * x * x + b * x + c;

                chart2.Series[0].Points.AddXY(x, y);
                x = x + 0.5;


            } while (x <= 10.0 + 0.00001);


        }

        private void button1_Click_1(object sender, EventArgs e)
        {
            Application.Exit(); //Выход
        }
    }
}
