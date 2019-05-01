using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Timers;
using Tao.OpenGl;

namespace The_War_in_the_Air
{
	class Gem : Unit, CanMove, GemJet, CanBeShooted
	{
		static Gem()
		{
			//Определение точек для наложения текстуры
			textureMap = new List<Point>(){
				new Point( recount.x(0.6), recount.y( 0)),
				new Point( recount.x(0  ), recount.y( 0.3)),
				new Point( recount.x(0  ), recount.y( 1)),
				new Point( recount.x(Math.Cos(Math.PI*3/4)), recount.y(Math.Sin(Math.PI*3/4))),
				new Point( recount.x(-1 ), recount.y(0)),
				new Point( recount.x(Math.Cos(Math.PI*5/4)), recount.y(Math.Sin(Math.PI*5/4))),
				new Point( recount.x(0  ), recount.y( -1)),
				new Point( recount.x(0  ), recount.y( -0.3)),
			};
		}
		static double size = 4;
		static List<Point> textureMap;

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

		private double Frame;
		private int MaxFrameCount;

		//Создание нового объекта вражеского истребителя для алмаза
		private Jet Jet;

		public Gem(double X, double Y, double speed, double angle, TrackType type) : base(X, Y, speed)
		{
			//Определение позиции для целей по оси Y 
			position.Y = 20;

			Jet = new Jet(position.X, position.Y, this);
			Frame = 0;
			MaxFrameCount = Textures.Id_GemAnimation.Count;

			//Рисование объекта
			this.draw = new List<Point>() {
				new Point( 0.6 , 0  ),
				new Point( 0   , 0.3),
				new Point( 0   , 1  ),
				new Point( Math.Cos(Math.PI*3/4),Math.Sin(Math.PI*3/4)),
				new Point(-1   , 0  ),
				new Point( Math.Cos(Math.PI*5/4),Math.Sin(Math.PI*5/4)),
				new Point( 0   ,-1  ),
				new Point( 0   ,-0.3),
			};
			this.track = new Track(type);
			this.angle = angle;
			init(size);
		}

		private Track track;
		double angle = 0;

		//Инициализация размера объекта
		public void init(double size)
		{
			draw = Matrix.CreateZoom(size, draw);
		}

		public Point GetNextPoint()
		{
			angle += 0.05;
			if (angle >= 2 * Math.PI)
			{
				angle = 0;
			}
			Point answer = track.GetNext();
			return answer;
		}

		//Отрисовка объекта цели с текстурами
		private void drawGem()
		{
			List<Point> toDraw = Matrix.CreateRotate(angle, draw);

			//Определение цвета точек
			Gl.glColor3d(1, 1, 1);

			//Привязка текстур
			Gl.glBindTexture(Gl.GL_TEXTURE_2D, Textures.Id_GemAnimation[(int)Frame]);
			Frame += 0.2;
			if (Frame > MaxFrameCount)
			{
				Frame = 0;
			}

			Gl.glEnable(Gl.GL_TEXTURE_2D);

			Gl.glBlendFunc(Gl.GL_SRC_ALPHA, Gl.GL_ONE_MINUS_SRC_ALPHA);
			int Z = 0;

			Gl.glBegin(Gl.GL_TRIANGLE_FAN);

			Gl.glTexCoord2f(0.5f, 0.5f);
			Gl.glVertex3d(position.X, position.Y, Z);
			int i = 0;
			Gl.glTexCoord2f((float)textureMap[i].X, (float)textureMap[i].Y);
			Gl.glVertex3d(position.X + toDraw[i].X, position.Y + toDraw[i].Y, Z);
			i = 1;
			Gl.glTexCoord2f((float)textureMap[i].X, (float)textureMap[i].Y);
			Gl.glVertex3d(position.X + toDraw[i].X, position.Y + toDraw[i].Y, Z);
			i = 2;
			Gl.glTexCoord2f((float)textureMap[i].X, (float)textureMap[i].Y);
			Gl.glVertex3d(position.X + toDraw[i].X, position.Y + toDraw[i].Y, Z);
			i = 3;
			Gl.glTexCoord2f((float)textureMap[i].X, (float)textureMap[i].Y);
			Gl.glVertex3d(position.X + toDraw[i].X, position.Y + toDraw[i].Y, Z);
			i = 4;
			Gl.glTexCoord2f((float)textureMap[i].X, (float)textureMap[i].Y);
			Gl.glVertex3d(position.X + toDraw[i].X, position.Y + toDraw[i].Y, Z);
			i = 5;
			Gl.glTexCoord2f((float)textureMap[i].X, (float)textureMap[i].Y);
			Gl.glVertex3d(position.X + toDraw[i].X, position.Y + toDraw[i].Y, Z);
			i = 6;
			Gl.glTexCoord2f((float)textureMap[i].X, (float)textureMap[i].Y);
			Gl.glVertex3d(position.X + toDraw[i].X, position.Y + toDraw[i].Y, Z);
			i = 7;
			Gl.glTexCoord2f((float)textureMap[i].X, (float)textureMap[i].Y);
			Gl.glVertex3d(position.X + toDraw[i].X, position.Y + toDraw[i].Y, Z);
			i = 0;
			Gl.glTexCoord2f((float)textureMap[i].X, (float)textureMap[i].Y);
			Gl.glVertex3d(position.X + toDraw[i].X, position.Y + toDraw[i].Y, Z);
			Gl.glEnd();
			Gl.glDisable(Gl.GL_TEXTURE_2D);
		}

		//Отрисовка алмаза вместе с объектом истребителя
		public override void Draw()
		{
			if (Jet != null)
			{
				Jet.Draw();
			}
			drawGem();
		}

		//Описание движения алмаза вместе с истребителем
		bool CanMove.Move()
		{
			Point next = GetNextPoint();
			if (next == null)
			{
				return true;
			}
			if (Jet != null)
			{
				Jet.position.X = position.X;
			}
			if (next == null)
			{
				return true;
			}
			position.X = next.X;
			return false;
		}

		void GemJet.wipeJet()
		{
			Jet = null;
		}

		//Описание взаимодействия ракет с объектом алмаза
		bool CanBeShooted.ShotedBy(Rocket rocket)
		{
			bool ans = false;
			List<Point> foo = Matrix.CreateRotate(angle, draw);
			List<Point> toCheck = new List<Point>();

			foreach (Point item in foo)
			{
				toCheck.Add(new Point(item.X + position.X, item.Y + position.Y));
			}
			ans = Matrix.IsPointInside(toCheck, rocket.position);
			if (!ans && Jet != null)
			{
				((CanBeShooted)Jet).ShotedBy(rocket);
			}
			if (ans)
			{
				rocket.AddPointsToOwner(1);
			}
			return ans;
		}

		void Destructible.Destruct()
		{
			((GemJet)this).wipeJet();
		}
	}
}
