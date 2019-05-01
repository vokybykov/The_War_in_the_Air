using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace The_War_in_the_Air
{
	class Matrix
	{
		public static List<Point> multiply(List<Point> ListToEdit, double[,] matrix)
		{
			List<Point> obj = new List<Point>();
			for (int i = 0; i < ListToEdit.Count; i++)
			{
				obj.Add(new Point(ListToEdit[i].X, ListToEdit[i].Y));
			}

			double res_1, res_2;
			for (int i = 0; i < obj.Count; i++)
			{
				res_1 = (obj[i].X * matrix[0, 0] + obj[i].Y * matrix[0, 1] + matrix[2, 0]);
				res_2 = (obj[i].X * matrix[1, 0] + obj[i].Y * matrix[1, 1] + matrix[2, 1]);
				obj[i].X = res_1;
				obj[i].Y = res_2;
			}
			return obj;
		}
		public static Point multiply(Point pointToEdit, double[,] matrix)
		{
			Point obj = new Point(pointToEdit.X, pointToEdit.Y);
			double res_1, res_2;
			res_1 = (obj.X * matrix[0, 0] + obj.Y * matrix[0, 1] + matrix[2, 0]);
			res_2 = (obj.X * matrix[1, 0] + obj.Y * matrix[1, 1] + matrix[2, 1]);
			obj.X = res_1;
			obj.Y = res_2;
			return obj;
		}

		public static List<Point> CreateZoom(double coef, List<Point> toResize)
		{
			// функция масштабирования
			double[,] Zoom2D = new double[3, 3];
			Zoom2D[0, 0] = coef;
			Zoom2D[1, 0] = 0;
			Zoom2D[2, 0] = 0;

			Zoom2D[0, 1] = 0;
			Zoom2D[1, 1] = coef;
			Zoom2D[2, 1] = 0;

			Zoom2D[0, 2] = 0;
			Zoom2D[1, 2] = 0;
			Zoom2D[2, 2] = 1;


			return multiply(toResize, Zoom2D);
		}
		public static Point CreateZoom(double coef, Point toResize)
		{
			double[,] Zoom2D = new double[3, 3];
			Zoom2D[0, 0] = coef;
			Zoom2D[1, 0] = 0;
			Zoom2D[2, 0] = 0;

			Zoom2D[0, 1] = 0;
			Zoom2D[1, 1] = coef;
			Zoom2D[2, 1] = 0;

			Zoom2D[0, 2] = 0;
			Zoom2D[1, 2] = 0;
			Zoom2D[2, 2] = 1;
			return multiply(toResize, Zoom2D);
		}

		public static List<Point> CreateMove(double angle, double speed, List<Point> toMove)
		{
			// функция переноса 
			double[,] Tran2D = new double[3, 3];
			Tran2D[0, 0] = 1;
			Tran2D[1, 0] = 0;
			Tran2D[2, 0] = 0;

			Tran2D[0, 1] = 0;
			Tran2D[1, 1] = 1;
			Tran2D[2, 1] = 0;

			Tran2D[0, 2] = 0;
			Tran2D[1, 2] = 0;
			Tran2D[2, 2] = 1;
			// устанавливаем коэфицент переноса для необходимой (выбранной и переданной в качестве параметра) оси
			double x = Math.Cos(angle) * speed;
			double y = Math.Sin(angle) * speed;
			Tran2D[2, 0] = x;
			Tran2D[2, 1] = y;
			return multiply(toMove, Tran2D);
		}
		public static Point CreateMove(double angle, double speed, Point toMove)
		{
			// функция переноса 
			double[,] Tran2D = new double[3, 3];
			Tran2D[0, 0] = 1;
			Tran2D[1, 0] = 0;
			Tran2D[2, 0] = 0;

			Tran2D[0, 1] = 0;
			Tran2D[1, 1] = 1;
			Tran2D[2, 1] = 0;

			Tran2D[0, 2] = 0;
			Tran2D[1, 2] = 0;
			Tran2D[2, 2] = 1;
			// устанавливаем коэфицент переноса для необходимой (выбранной и переданной в качестве параметра) оси
			double x = Math.Cos(angle) * speed;
			double y = Math.Sin(angle) * speed;
			Tran2D[2, 0] = x;
			Tran2D[2, 1] = y;
			return multiply(toMove, Tran2D);
		}
		public static List<Point> CreateRotate(double angle, List<Point> toRotate)
		{
			// реализация поворота 
			double[,] Rotate2D = new double[3, 3];

			Rotate2D[0, 0] = (float)Math.Cos(angle);
			Rotate2D[1, 0] = (float)Math.Sin(angle);
			Rotate2D[2, 0] = 0;

			Rotate2D[0, 1] = (float)-Math.Sin(angle);
			Rotate2D[1, 1] = (float)Math.Cos(angle);
			Rotate2D[2, 1] = 0;

			Rotate2D[0, 2] = 0;
			Rotate2D[1, 2] = 0;
			Rotate2D[2, 2] = 1;
			return multiply(toRotate, Rotate2D);
		}

		public static bool IsPointInside(List<Point> polygon, Point point)
		{
			if (polygon.Count <= 1)
			{
				return false;
			}
			int intersections_num = 0;
			int prev = polygon.Count - 1;
			bool prev_under = polygon[prev].Y < point.Y;

			for (int i = 0; i < polygon.Count; ++i)
			{
				bool cur_under = polygon[i].Y < point.Y;

				Point a = new Point(polygon[prev].X - point.X, polygon[prev].Y - point.Y);
				Point b = new Point(polygon[i].X - point.X, polygon[i].Y - point.Y);

				double t = (a.X * (b.Y - a.Y) - a.Y * (b.X - a.X));
				if (cur_under && !prev_under)
				{
					if (t > 0)
					{
						intersections_num++;
					}
				}
				if (!cur_under && prev_under)
				{
					if (t < 0)
					{
						intersections_num++;
					}
				}
				prev = i;
				prev_under = cur_under;
			}
			return (intersections_num & 1) != 0;
		}

		public static double Rasst(Point P1, Point P2)
		{
			return Math.Sqrt(Math.Pow(P2.X - P1.X, 2) + Math.Pow(P2.Y - P1.Y, 2));
		}
	}
}
