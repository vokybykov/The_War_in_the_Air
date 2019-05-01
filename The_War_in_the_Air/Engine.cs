using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Tao.OpenGl;

namespace The_War_in_the_Air
{
	class Engine : RocketHolder
	{
		//Объявляем экземпляры нобходимых объектов 
		private User user;
		private List<Rocket> rockets;
		private List<Gem> gems;
		private BG bg;

		public Engine()
		{
			//Загружаем текстуры
			Textures.LoadTextures();

			//Создаем экземпляры необходимых объектов 
			user = new User(0, 0, 1, 4, this);
			bg = new BG(0, 0, 0);
			rockets = new List<Rocket>();
			gems = new List<Gem>();
			if (StaticData.test)
			{
				gems.Add(new Gem(StaticData.max + 10, StaticData.max + 10, 0, Math.PI * 5 / 4, TrackType.Test));
			}
			else
			{
				gems.Add(new Gem(StaticData.max + 10, StaticData.max + 10, 0, Math.PI * 5 / 4, TrackType.First));
				gems.Add(new Gem(StaticData.max + 10, StaticData.max + 10, 0, Math.PI * 7 / 4, TrackType.Second));
			}
		}

		public void MouseMove(int x, int y)
		{
			//4.83 коэффицент для пересчёта пикселей в тайтлы = размер экрана px/размер экрана tl
			user.coursor.X = (x - 300) / 4.83;
			user.coursor.Y = -(y - 300) / 4.83;
		}

		//Отрисовка объектов сцены
		public void Draw()
		{
			Gl.glDisable(Gl.GL_DEPTH_TEST);
			bg.Draw();
			user.Draw();
			for (int i = 0; i < rockets.Count; i++)
			{
				rockets[i].Draw();
			}
			for (int i = 0; i < gems.Count; i++)
			{
				gems[i].Draw();
			}
		}

		//Определение функции возможности игрока стрелять
		public void StartUserShot()
		{
			user.setShot(true);
		}
		public void StopUserShot()
		{
			user.setShot(false);
		}

		//Определение функции движения игрока
		public void MoveUser()
		{
			((CanMove)user).Move();
		}
		public int GetUserPoints()
		{
			return ((GetSetPoints)user).Points;
		}

		public void ChangeUserMoveParam(int v, bool isMoving)
		{
			user.setMoveWay(v, isMoving);
		}


		public void MoveRockets()
		{
			lock (rockets)
			{
				for (int i = 0; i < rockets.Count; i++)
				{
					if (((CanMove)rockets[i]).Move())
					{
						rockets.RemoveAt(i);
						i--;
					}
				}
			}
		}

		public void CheckCollisions()
		{
			lock (rockets)
			{
				for (int i = 0; i < rockets.Count; i++)
				{
					for (int i1 = 0; i1 < gems.Count; i1++)
					{
						if (((CanBeShooted)gems[i1]).ShotedBy(rockets[i]))
						{
							((CanBeShooted)gems[i1]).Destruct();
							gems.RemoveAt(i1);
							i1--;
							rockets.RemoveAt(i);
							i--;
							break;
						}
					}
				}
			}
			if (gems.Count == 0)
			{
				if (StaticData.test)
				{
					gems.Add(new Gem(StaticData.max + 10, StaticData.max + 10, 0, Math.PI * 5 / 4, TrackType.Test));
				}
				else
				{
					gems.Add(new Gem(StaticData.max + 10, StaticData.max + 10, 0, Math.PI * 5 / 4, TrackType.First));
					gems.Add(new Gem(StaticData.max + 10, StaticData.max + 10, 0, Math.PI * 7 / 4, TrackType.Second));
				}
			}
		}

		public void MoveGems()
		{
			for (int i = 0; i < gems.Count; i++)
			{
				if (((CanMove)gems[i]).Move())
				{
					((CanBeShooted)gems[i]).Destruct();
					gems.RemoveAt(i);
					i--;
					continue;
				}
			}
		}

		public void AddRocket(List<Rocket> rockets)
		{
			lock (this.rockets)
			{
				for (int i = 0; i < rockets.Count; i++)
				{
					this.rockets.Add(rockets[i]);
				}
			}
		}
	}
}
