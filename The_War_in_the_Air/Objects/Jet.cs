using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Tao.OpenGl;

namespace The_War_in_the_Air
{
	class Jet : Unit, CanBeShooted
	{
		public static double size = 7;
		static List<Point> textureMap;
		private int Live;
		private int MaxFrameCount;
		private GemJet GJet;

		private static class recount
		{
			public static double x(double a)
			{
				return (1 + a) / 2;
			}
			public static double y(double a)
			{
				return (1 - a) / 2;
			}
		}

		//Определение точек для наложения текстуры
		static Jet()
		{
			textureMap = new List<Point>(){
				new Point( recount.x(-1), recount.y(-1)),
				new Point( recount.x(-1), recount.y( 1)),
				new Point( recount.x( 1), recount.y( 1)),
				new Point( recount.x( 1), recount.y(-1)),
			};
		}

		//Определение точек объекта вражеского истребителя
		public Jet(double X, double Y, GemJet gJet) : base(X, Y, 0)
		{
			GJet = gJet;
			Live = 0;
			MaxFrameCount = Textures.Id_JetAnimation.Count;
			draw = new List<Point>() {
				new Point( -1 , -1 ),
				new Point( -1 ,  1 ),
				new Point(  1 ,  1 ),
				new Point(  1 , -1 ),
			};
			init(size);
		}

		//Инициализация размера объекта
		public void init(double size)
		{
			draw = Matrix.CreateZoom(size, draw);
		}

		//Отрисовка объекта вражеского истребителя с текстурами
		public override void Draw()
		{
			Gl.glColor4d(1, 1, 1, 1);

			Gl.glEnable(Gl.GL_TEXTURE_2D);

			int Z = 0;

			Gl.glBindTexture(Gl.GL_TEXTURE_2D, Textures.Id_JetAnimation[Live]);
			Gl.glBegin(Gl.GL_TRIANGLE_FAN);

			Gl.glTexCoord2f(0.5f, 0.5f);
			Gl.glVertex3d(position.X, position.Y, Z);
			int i = 0;
			Gl.glTexCoord2f((float)textureMap[i].X, (float)textureMap[i].Y);
			Gl.glVertex3d(position.X + draw[i].X, position.Y + draw[i].Y, Z);
			i = 1;
			Gl.glTexCoord2f((float)textureMap[i].X, (float)textureMap[i].Y);
			Gl.glVertex3d(position.X + draw[i].X, position.Y + draw[i].Y, Z);
			i = 2;
			Gl.glTexCoord2f((float)textureMap[i].X, (float)textureMap[i].Y);
			Gl.glVertex3d(position.X + draw[i].X, position.Y + draw[i].Y, Z);
			i = 3;
			Gl.glTexCoord2f((float)textureMap[i].X, (float)textureMap[i].Y);
			Gl.glVertex3d(position.X + draw[i].X, position.Y + draw[i].Y, Z);
			i = 0;
			Gl.glTexCoord2f((float)textureMap[i].X, (float)textureMap[i].Y);
			Gl.glVertex3d(position.X + draw[i].X, position.Y + draw[i].Y, Z);
			Gl.glEnd();
			Gl.glDisable(Gl.GL_TEXTURE_2D);
		}

		//Описание взаимодействия ракет с объектом вражеского истребителя
		bool CanBeShooted.ShotedBy(Rocket rocket)
		{
			bool ans = false;
			if (Matrix.Rasst(position, rocket.position) < size)
			{
				Live += 1;
				if (Live >= 3)
				{
					GJet.wipeJet();
				}
				ans = true;
				double angelM = Math.Atan2(rocket.position.Y - this.position.Y, rocket.position.X - this.position.X);
				double bAngle = rocket.angle;
				if (bAngle < 0)
				{
					bAngle += 2 * Math.PI;
				}
				bAngle %= 2 * Math.PI;//3
				bAngle = bAngle - Math.PI;
				double change = angelM - bAngle;
				rocket.angle = angelM + change;
				((CanMove)rocket).Move();
			}
			return ans;
		}

		void Destructible.Destruct() { }
	}
}
