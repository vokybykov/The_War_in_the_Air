using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace The_War_in_the_Air
{
	public class Point
	{
		//Класс описывающий общие параметры точек объектов
		public Point(double x, double y)
		{
			this.X = x;
			this.Y = y;
		}
		public double X;
		public double Y;

		public Point Clone()
		{
			return new Point(X, Y);
		}
	}
}
