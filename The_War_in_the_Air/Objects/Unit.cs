using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace The_War_in_the_Air
{
	//Абстрактный класс, описывающий все объекты
	abstract class Unit
	{
		public Point position = new Point(0, 0);
		public List<Point> draw = new List<Point>();
		public double speed;

		public Unit(double X, double Y, double speed)
		{
			this.position = new Point(X, Y);
			this.speed = speed;
		}

		public abstract void Draw();
	}
}
