using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Tao.OpenGl;
using System.Drawing;

namespace The_War_in_the_Air
{
	class Rocket : Unit, CanMove
	{
		GetSetPoints rocketOwner;
		static double rocketCoef = 1;

		//Определение точек объекта ракеты
		public static new List<Point> draw = Matrix.CreateZoom(rocketCoef, new List<Point>() {
			new Point( 0    ,  2.5),
			new Point( 0.25    ,  2),
			new Point( 0.55 , 0.55),
			new Point( 0.7  ,    0),
			new Point( 0.55 ,-0.55),
			new Point( -0.55, -0.55),
			new Point( -0.7 ,    0),
			new Point( -0.55, 0.55)
		});

		public double angle = 0;
		public Color color;

		public Rocket(double X, double Y, double speed, double angle, Color color, GetSetPoints owner = null) : base(X, Y, speed)
		{
			rocketOwner = owner;
			this.angle = angle;
			this.color = color;
		}

		//Отрисовка объекта по точкам
		public override void Draw()
		{
			Gl.glColor4d(1, 0.2, 0.2, 1);
			int Z = 0;

			Gl.glBegin(Gl.GL_TRIANGLE_FAN);

			Gl.glTexCoord2f(0.5f, 0.5f);
			Gl.glVertex3d(position.X, position.Y, Z);
			int i = 0;
			Gl.glVertex3d(position.X + draw[i].X, position.Y + draw[i].Y, Z);
			i = 1;
			Gl.glVertex3d(position.X + draw[i].X, position.Y + draw[i].Y, Z);
			i = 2;
			Gl.glVertex3d(position.X + draw[i].X, position.Y + draw[i].Y, Z);
			i = 3;
			Gl.glVertex3d(position.X + draw[i].X, position.Y + draw[i].Y, Z);
			i = 4;
			Gl.glVertex3d(position.X + draw[i].X, position.Y + draw[i].Y, Z);
			i = 5;
			Gl.glVertex3d(position.X + draw[i].X, position.Y + draw[i].Y, Z);
			i = 6;
			Gl.glVertex3d(position.X + draw[i].X, position.Y + draw[i].Y, Z);
			i = 7;
			Gl.glVertex3d(position.X + draw[i].X, position.Y + draw[i].Y, Z);
			i = 0;
			Gl.glVertex3d(position.X + draw[i].X, position.Y + draw[i].Y, Z);
			Gl.glEnd();
			Gl.glDisable(Gl.GL_TEXTURE_2D);
		}

		//Описание движения ракеты
		bool CanMove.Move()
		{
			if ((position.X > StaticData.max)
				|| (position.X < StaticData.min)
				|| (position.Y > StaticData.max)
				|| (position.Y < StaticData.min))
			{
				return true;
			}
			position = Matrix.CreateMove(angle, speed, position);
			return false;
		}

		//Функция добавления ракет объекту, который может стрелять
		public void AddPointsToOwner(int points)
		{
			if (rocketOwner != null)
			{
				rocketOwner.Points += 1;
			}
		}
	}
}
