using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace The_War_in_the_Air
{
	//Геттер и сеттер для точек объектов
	interface GetSetPoints
	{
		int Points { get; set; }
	}

	//Интерфейс, добавляющий объекту возможность стрелять ракетами
	interface RocketHolder
	{
		void AddRocket(List<Rocket> rockets);
	}

	//Интерфейс, добавляющий объекту возможность двигаться
	interface CanMove
	{
		bool Move();
	}

	//Интерфейс, определяющий может ли объект быть разрушенным
	interface Destructible
	{
		void Destruct();
	}

	//Интерфейс, определяющий чем может быть разрушен объект 	
	interface CanBeShooted : Destructible
	{
		bool ShotedBy(Rocket rocket);
	}

	//Интерфейс для обновления состояния истребителя
	interface GemJet
	{
		void wipeJet();
	}
}
