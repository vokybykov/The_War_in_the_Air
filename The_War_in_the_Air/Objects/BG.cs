using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Tao.OpenGl;

namespace The_War_in_the_Air
{
	class BG : Unit
	{
		public static double size = 1.1;
		static List<Point> textureMap;
		private int Live;
		private int MaxFrameCount;

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
		static BG()
		{
			textureMap = new List<Point>(){
				new Point( recount.x(-1), recount.y(-1)),
				new Point( recount.x(-1), recount.y( 1)),
				new Point( recount.x( 1), recount.y( 1)),
				new Point( recount.x( 1), recount.y(-1)),
			};
		}

		//Отрисовка объекта
		public BG(double X, double Y, double speed) : base(X, Y, speed)
		{
			speed = 1;
			Live = 1;
			MaxFrameCount = Textures.Id_BGAnimation.Count;
			draw = new List<Point>() {
				new Point( -90 , -60 ),
				new Point( -90 ,  60 ),
				new Point(  90 ,  60 ),
				new Point(  90 , -60 ),
			};
			init(size);
		}

		//Инициализация размера объекта
		public void init(double size)
		{
			draw = Matrix.CreateZoom(size, draw);
		}

		//Отрисовка объекта фона с текстурами
		public override void Draw()
		{
			Gl.glEnable(Gl.GL_TEXTURE_2D);
			int Z = 0;

			Gl.glBindTexture(Gl.GL_TEXTURE_2D, Textures.Id_BGAnimation[Live]);
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
	}
}
