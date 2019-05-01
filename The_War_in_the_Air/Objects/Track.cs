using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace The_War_in_the_Air
{
	//Класс для просчета траекторий объектов
	class Track
	{
		static List<List<Point>> allTracks;
		private List<List<Point>> trackList;
		private List<int> trackBegin;
		private int trackPosition = -1;
		bool repeadLast;

		static int IdJet_Track;
		static int IdRightTop_To_Jet_Track;
		static int IdLeftTop_To_Jet_Track;
		static int IdLeftDown_To_Jet_Track;
		static int IdRightDown_To_Jet_Track;

		static int IdTest_Track;

		private static int AddTrack(List<List<Point>> Tracks, List<Point> track)
		{
			Tracks.Add(track);
			return Tracks.Count - 1;
		}

		private static class TrackGenerator
		{
			public static List<Point> JetTrack(int r, double x0, double y0, bool way)
			{
				List<Point> track = new List<Point>();
				for (int i = 0; i < 360; i++)
				{
					int iway = 1;
					if (!way)
					{
						iway = -1;
					}
					double angle = iway * i * 3.14 / 180;
					double x = x0 + r * Math.Cos(angle);
					double y = y0 + r * Math.Sin(angle);
					track.Add(new Point(x, y));
				}
				return track;
			}
			public static List<Point> LineTrack(double x0, double y0, double x1, double y1, int points)
			{
				List<Point> track = new List<Point>();
				double dx = (x1 - x0) / points;
				double dy = (y1 - y0) / points;
				for (int i = 0; i < points; i++)
				{
					double x = x0 + dx * i;
					double y = y0 + dy * i;
					track.Add(new Point(x, y));
				}
				return track;
			}
			public static List<Point> PointTrack(double x, double y)
			{
				List<Point> track = new List<Point>();
				track.Add(new Point(x, y));
				return track;
			}
		}


		static Track()
		{
			//Инициализация всех видов траекторий
			allTracks = new List<List<Point>>();
			{
				//Траектория движения по окуржности
				IdJet_Track = AddTrack(allTracks, TrackGenerator.JetTrack(30, 0, 0, true));
			}
			{
				//Траектория выхода на окружность из правого верхнего угла
				int r = 30;
				double x0 = 62, y0 = 62, x1 = r * Math.Sin(45 * Math.PI / 180), y1 = r * Math.Sin(45 * Math.PI / 180);
				IdRightTop_To_Jet_Track = AddTrack(allTracks, TrackGenerator.LineTrack(x0, y0, x1, y1, 120));
			}
			{
				//Траектория выхода на окружность из левого верхнего угла
				int r = 30;
				double x0 = -62, y0 = 62, x1 = -1 * r * Math.Sin(45 * Math.PI / 180), y1 = r * Math.Sin(45 * Math.PI / 180);
				IdLeftTop_To_Jet_Track = AddTrack(allTracks, TrackGenerator.LineTrack(x0, y0, x1, y1, 120));
			}
			{
				//Траектория выхода на окружность из левого нижнего угла
				int r = 30;
				double x0 = -62, y0 = -62, x1 = -1 * r * Math.Sin(45 * Math.PI / 180), y1 = -1 * r * Math.Sin(45 * Math.PI / 180);
				IdLeftDown_To_Jet_Track = AddTrack(allTracks, TrackGenerator.LineTrack(x0, y0, x1, y1, 120));
			}
			{
				//Траектория выхода на окружность из правого нижнего угла
				int r = 30;
				double x0 = 62, y0 = -62, x1 = r * Math.Sin(45 * Math.PI / 180), y1 = -1 * r * Math.Sin(45 * Math.PI / 180);
				IdRightDown_To_Jet_Track = AddTrack(allTracks, TrackGenerator.LineTrack(x0, y0, x1, y1, 120));
			}
			{
				IdTest_Track = AddTrack(allTracks, TrackGenerator.PointTrack(10, 10));
			}
		}

		//Функция генерации траектории для объектов
		private void GenerateTrack(TrackType type, out bool repeadLast)
		{
			repeadLast = false;
			switch (type)
			{
				case TrackType.First:
					trackBegin.Add(0);
					trackList.Add(allTracks[Track.IdRightTop_To_Jet_Track]);
					trackBegin.Add(45);
					trackList.Add(allTracks[Track.IdJet_Track]);
					repeadLast = true;
					break;
				case TrackType.Second:
					trackBegin.Add(0);
					trackList.Add(allTracks[Track.IdLeftTop_To_Jet_Track]);
					trackBegin.Add(135);
					trackList.Add(allTracks[Track.IdJet_Track]);
					repeadLast = true;
					break;
				case TrackType.Third:
					trackBegin.Add(0);
					trackList.Add(allTracks[Track.IdLeftDown_To_Jet_Track]);
					trackBegin.Add(225);
					trackList.Add(allTracks[Track.IdJet_Track]);
					repeadLast = true;
					break;
				case TrackType.Fourh:
					trackBegin.Add(0);
					trackList.Add(allTracks[Track.IdRightDown_To_Jet_Track]);
					trackBegin.Add(315);
					trackList.Add(allTracks[Track.IdJet_Track]);
					repeadLast = true;
					break;
				case TrackType.Test:
					trackBegin.Add(0);
					trackList.Add(allTracks[Track.IdTest_Track]);
					repeadLast = true;
					break;
				default:
					break;
			}
		}
		public Track(TrackType type)
		{
			trackPosition = -1;
			trackList = new List<List<Point>>();
			trackBegin = new List<int>();
			GenerateTrack(type, out repeadLast);
		}

		//Функция получения следующей точки траектории
		public Point GetNext()
		{
			if (trackPosition == -1)
			{
				trackPosition = trackBegin[0];
				trackBegin.RemoveAt(0);
			}
			trackPosition += 1;
			if (trackPosition == trackList[0].Count && trackList.Count != 1)
			{
				trackPosition = trackBegin[0];
				trackBegin.RemoveAt(0);
				trackList.RemoveAt(0);
			}
			if (trackList.Count == 1 && trackPosition == trackList[0].Count && !repeadLast)
			{
				return null;
			}
			trackPosition %= trackList[0].Count;
			return trackList[0][trackPosition];
		}
	}
}
