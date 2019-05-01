using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.IO;
using StbSharp;
using Tao.OpenGl;

namespace The_War_in_the_Air
{
	static class Textures
	{
		//Создание списков текстур объектов
		public static List<int> Id_UserAnimation;
		public static List<int> Id_GemAnimation;
		public static List<int> Id_JetAnimation;
		public static List<int> Id_BGAnimation;

		//Список анимации пользователя
		static List<string> userAnimationToLoad = new List<string>
		{
			"stels_left.png",
			"stels.png",
			"stels_right.png",
			"stels.png",
		};

		//Список анимации алмаза
		static List<string> gemAnimationToLoad = new List<string>
		{
			"gem.png",
			"gem.png",
		};

		//Список анимации фона
		static List<string> BGStatesToLoad = new List<string>
		{
			"sky_1.png",
			"sky_2.png",

		};

		//Список анимации вражеского истребителя
		static List<string> JetStatesToLoad = new List<string>
		{
			"jet.png",
			"jet.png",
			"jet.png",
		};

		public static void LoadTextures()
		{
			Id_UserAnimation = LoadTexture(userAnimationToLoad);
			Id_GemAnimation = LoadTexture(gemAnimationToLoad);
			Id_JetAnimation = LoadTexture(JetStatesToLoad);
			Id_BGAnimation = LoadTexture(BGStatesToLoad);
		}

		private static byte[] loadImage(string path, out int w, out int h)
		{
			Image img;
			using (Stream stream = File.Open(path, FileMode.Open))
			{
				img = new ImageReader().Read(stream, StbImage.STBI_rgb_alpha);
				w = img.Width;
				h = img.Height;
			}
			return img.Data;
		}

		private static byte[] loadImage(string path, int w, int h, int x, int y)
		{
			Image img;
			using (Stream stream = File.Open(path, FileMode.Open))
			{
				img = new ImageReader().Read(stream, StbImage.STBI_rgb_alpha);
			}
			byte[] answer = new byte[w * h * 4];
			int k = 0;
			for (int i = 0; i < w; i++)
			{
				for (int j = 0; j < h; j++)
				{
					answer[k] = img.Data[y * img.Width * 4 + x * 4 + i * 4 + j * 4];
					answer[k + 1] = img.Data[y * img.Width * 4 + x * 4 + i * 4 + j * 4];
					answer[k + 2] = img.Data[y * img.Width * 4 + x * 4 + i * 4 + j * 4];
					answer[k + 3] = img.Data[y * img.Width * 4 + x * 4 + i * 4 + j * 4];
					k += 4;
				}
			}
			return img.Data;
		}
		private static int LoadTexture(byte[] pixels, int w, int h)
		{
			int answer;
			Gl.glGenTextures(1, out answer);
			Gl.glBindTexture(Gl.GL_TEXTURE_2D, answer);
			Gl.glTexImage2D(Gl.GL_TEXTURE_2D, 0, Gl.GL_RGBA, w, h, 0, Gl.GL_RGBA, Gl.GL_UNSIGNED_BYTE, pixels);
			Gl.glTexParameteri(Gl.GL_TEXTURE_2D, Gl.GL_TEXTURE_MIN_FILTER, Gl.GL_NEAREST);
			Gl.glBindTexture(Gl.GL_TEXTURE_2D, answer);
			return answer;
		}
		private static int LoadTexture(string path)
		{
			int w;
			int h;
			byte[] pixels = loadImage(path, out w, out h);
			return LoadTexture(pixels, w, h);
		}
		private static List<int> LoadTexture(List<string> fileNames)
		{
			List<int> answer = new List<int>();

			for (int i = 0; i < fileNames.Count; i++)
			{
				answer.Add(LoadTexture(fileNames[i]));
			}
			return answer;
		}
	}
}
