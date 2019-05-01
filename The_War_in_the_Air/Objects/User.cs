using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Timers;
using Tao.OpenGl;

namespace The_War_in_the_Air
{
	class User : Unit, CanMove, GetSetPoints
	{
		List<Point> textureMap = new List<Point>();
		static List<Point> shotableMap;
		RocketHolder rocketHolder;
		RocketGeneratorType shotType = RocketGeneratorType.Row;
		double userR;
		private double Frame;
		private int MaxFrameCount;

		//Инициализация размера объекта
		public void init(double size, ref List<Point> shotableMap)
		{
			draw = Matrix.CreateZoom(size, draw);
			userR = size;
			shotableMap = Matrix.CreateZoom(size, shotableMap);
		}

		private bool isMoving = false;
		private double angle = 0;

		public Point coursor;
		Timer shotTimer;

		//Определение точек для наложения текстуры и карты испускания ракет
		private void generateMaps(out List<Point> textureMap, out List<Point> shotableMap)
		{
			textureMap = new List<Point>(){
				new Point( 0, 1),
				new Point( 0, 0),
				new Point( 1, 0),
				new Point( 1, 1),
			};
			shotableMap = new List<Point>() {
				new Point(-1, 1  ),
				new Point( 1, 0.5),
				new Point( 1,-0.5),
				new Point(-1,-1  ),
			};
		}

		//Определение точек объекта пользователя и параметров таймера испускания ракет
		public User(double X, double Y, double speed, double size, RocketHolder rocketHolder) : base(X, Y, speed)
		{
			generateMaps(out textureMap, out shotableMap);

			Frame = 0;
			MaxFrameCount = Textures.Id_UserAnimation.Count;
			this.rocketHolder = rocketHolder;

			coursor = new Point(0, 0);

			draw.Add(new Point(-3, 3));
			draw.Add(new Point(3, 3));
			draw.Add(new Point(3, -3));
			draw.Add(new Point(-3, -3));

			shotTimer = new Timer();
			shotTimer.Interval = 300;
			shotTimer.AutoReset = true;
			shotTimer.Enabled = false;
			shotTimer.Elapsed += new ElapsedEventHandler(GenerateShot);
			init(size, ref shotableMap);
		}

		//Включение таймера стрельбы
		public void setShot(bool enabled)
		{
			shotTimer.Enabled = enabled;
		}

		//Определение угла обзора объекта
		public double LookAngle()
		{
			return Math.Atan2(coursor.Y - position.Y, coursor.X - position.X);
		}

		//Определение позиции ракет
		private Point RocketPosition(Point center, double r, double a)
		{
			return new Point(center.X + r * Math.Cos(a), center.Y + r * Math.Sin(a));
		}

		//Функция генерации ракет
		private void GenerateShot(object source, ElapsedEventArgs e)
		{
			List<Rocket> rockets = new List<Rocket>();
			switch (shotType)
			{
				case (RocketGeneratorType.Row):
					{
						double angle = LookAngle();
						if (angle < 0)
							angle += 2 * Math.PI;
						Point point = RocketPosition(this.position, userR, angle);
						Rocket rocket = new Rocket(point.X, point.Y, 1.5, angle, System.Drawing.Color.Blue, this);
						rockets.Add(rocket);
						break;
					}
				case (RocketGeneratorType.FastRow):
					{
						double angle = LookAngle();
						if (angle < 0)
							angle += 2 * Math.PI;
						Point point = RocketPosition(this.position, userR, angle);
						Rocket rocket = new Rocket(point.X, point.Y, 2.5, angle, System.Drawing.Color.Blue, this);
						rockets.Add(rocket);
						break;
					}
			}
			rocketHolder.AddRocket(rockets);
		}

		//Отрисовка объекта пользователя с текстурами
		public override void Draw()
		{
			Gl.glColor4d(1, 1, 1, 1);
			position.Y = -45;
			double Z = 0;
			double a = LookAngle();
			List<Point> toDraw = Matrix.CreateRotate(a, draw);

			Gl.glBindTexture(Gl.GL_TEXTURE_2D, Textures.Id_UserAnimation[(int)Frame]);
			Frame += 0.2;
			if (Frame > MaxFrameCount)
			{
				Frame = 0;
			}

			Gl.glEnable(Gl.GL_TEXTURE_2D);

			Gl.glBlendFunc(Gl.GL_SRC_ALPHA, Gl.GL_ONE_MINUS_SRC_ALPHA);

			Gl.glBegin(Gl.GL_QUADS);
			for (int i = 0; i < toDraw.Count; i++)
			{
				Gl.glTexCoord2f((float)textureMap[i].X, (float)textureMap[i].Y);
				Gl.glVertex3d(position.X + toDraw[i].X, position.Y + toDraw[i].Y, Z);
			}
			Gl.glEnd();
			Gl.glDisable(Gl.GL_TEXTURE_2D);
		}

		//Функция определения поворота объекта для двмжения в нужную сторону 
		public void setMoveWay(int v, bool isMoving)
		{
			this.isMoving = isMoving;
			switch (v)
			{
				case 0:
					angle = Math.PI / 2;
					break;
				case 1:
					angle = Math.PI / 4;
					break;
				case 2:
					angle = 0;
					break;
				case 3:
					angle = -Math.PI / 4;
					break;
				case 4:
					angle = -Math.PI / 2;
					break;
				case 5:
					angle = -3 * Math.PI / 4;
					break;
				case 6:
					angle = Math.PI;
					break;
				case 7:
					angle = 3 * Math.PI / 4;
					break;
			}

		}

		//Функция движения пользователя
		bool CanMove.Move()
		{
			if (!isMoving)
				return false;
			Point p = Matrix.CreateMove(angle, speed, position);
			if (p.X > StaticData.max - StaticData.userPading)
				p.X = StaticData.max - StaticData.userPading;
			if (p.X < StaticData.min + StaticData.userPading)
				p.X = StaticData.min + StaticData.userPading;
			if (p.Y > StaticData.max - StaticData.userPading)
				p.Y = StaticData.max - StaticData.userPading;
			if (p.Y < StaticData.min + StaticData.userPading)
				p.Y = StaticData.min + StaticData.userPading;
			position = p;
			return false;
		}


		private int points = 0;
		int GetSetPoints.Points
		{
			get
			{
				return points;
			}
			set
			{
				if (value != points)
				{
					points = value;
				}
			}
		}
	}
}
