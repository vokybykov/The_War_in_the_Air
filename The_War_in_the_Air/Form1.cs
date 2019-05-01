using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;

using Tao.OpenGl;
using Tao.FreeGlut;
using Tao.Platform.Windows;

namespace The_War_in_the_Air
{
	public partial class Form1 : Form
	{
		Engine engine;
		public Form1()
		{
			InitializeComponent();
			AnT.InitializeContexts();
			engine = new Engine();
		}

		bool keyW = false;
		bool keyS = false;
		bool keyA = false;
		bool keyD = false;
		bool keySpace = true;

		private void помощьToolStripMenuItem_Click(object sender, EventArgs e)
		{

			Form2 form2 = new Form2();
			form2.Show();

		}

		private void Form1_Load(object sender, EventArgs e)
		{
			Glut.glutInit();
			// инициализация режима экрана
			Glut.glutInitDisplayMode(Glut.GLUT_RGB | Glut.GLUT_DOUBLE | Glut.GLUT_DEPTH);

			Gl.glViewport(0, 0, AnT.Width, AnT.Height);
			// активация проекционной матрицы

			Gl.glMatrixMode(Gl.GL_PROJECTION);
			// очистка матрицы
			Gl.glLoadIdentity();
			Glu.gluPerspective(45, 1, 0.1, 600);

			Gl.glMatrixMode(Gl.GL_MODELVIEW);
			Gl.glLoadIdentity();
			Gl.glEnable(Gl.GL_DEPTH_TEST);
			Gl.glEnable(Gl.GL_BLEND);
			Gl.glBlendFunc(Gl.GL_SRC_ALPHA, Gl.GL_ONE_MINUS_SRC_ALPHA);

			Timer.Start();

			System.Media.SoundPlayer player = new System.Media.SoundPlayer();
			player.SoundLocation = "theme.wav";
			player.Play();

		}

		private int choseWay()
		{
			int result = -1;
			if ((keyW && !keyS) && ((!keyD && !keyA) || (keyD && keyA)))
			{
				return 0;
			}
			if (keyW && !keyS && keyD && !keyA)
			{
				return 1;
			}
			if (((!keyW && !keyS) || (keyW && keyS)) && (keyD && !keyA))
			{
				return 2;
			}
			if (!keyW && keyS && keyD && !keyA)
			{
				return 3;
			}
			if ((!keyW && keyS) && ((!keyD && !keyA) || (keyD && keyA)))
			{
				return 4;
			}
			if (!keyW && keyS && !keyD && keyA)
			{
				return 5;
			}
			if (((!keyW && !keyS) || (keyW && keyS)) && (!keyD && keyA))
			{
				return 6;
			}
			if (keyW && !keyS && !keyD && keyA)
			{
				return 7;
			}
			return result;
		}

		private void Timer_Tick(object sender, EventArgs e)
		{
			int moveWay = choseWay();
			if (moveWay > -1)
			{
				engine.ChangeUserMoveParam(moveWay, true);
			}
			else
			{
				engine.ChangeUserMoveParam(moveWay, false);
			}
			engine.MoveRockets();
			engine.MoveGems();
			engine.MoveUser();
			engine.CheckCollisions();

			// очистка буфера цвета и буфера глубины 
			Gl.glClear(Gl.GL_COLOR_BUFFER_BIT | Gl.GL_DEPTH_BUFFER_BIT);
			Gl.glClearColor(255, 255, 255, 1);

			// очищение текущей матрицы 
			Gl.glLoadIdentity();

			// помещаем состояние матрицы в стек матриц 
			Gl.glPushMatrix();

			// перемещаем камеру для лучшего обзора объекта 
			Gl.glTranslated(0, 0, -150);
			engine.Draw();

			// возвращаем состояние матрицы 
			Gl.glPopMatrix();

			// отрисовываем геометрию 
			Gl.glFlush();
			AnT.Invalidate();
		}

		private void AnT_KeyDown(object sender, KeyEventArgs e)
		{
			e.Handled = false;
			switch (e.KeyCode)
			{
				case Keys.W:
					{
						keyW = true;
						break;
					}
				case Keys.D:
					{
						keyD = true;
						break;
					}
				case Keys.S:
					{
						keyS = true;
						break;
					}
				case Keys.A:
					{
						keyA = true;
						break;
					}
				case Keys.Space:
					{
						//keySpace = true;
						engine.StartUserShot();
						break;
					}
			}
		}

		private void AnT_KeyUp(object sender, KeyEventArgs e)
		{
			e.Handled = false;
			switch (e.KeyCode)
			{
				case Keys.W:
					{
						keyW = false;
						break;
					}
				case Keys.D:
					{
						keyD = false;
						break;
					}
				case Keys.S:
					{
						keyS = false;
						break;
					}
				case Keys.A:
					{
						keyA = false;
						break;
					}
				case Keys.Space:
					{
						keySpace = false;
						engine.StopUserShot();
						break;
					}
			}
		}

		private void AnT_MouseMove(object sender, MouseEventArgs e)
		{
			engine.MouseMove(e.X, e.Y);
		}
	}
}
