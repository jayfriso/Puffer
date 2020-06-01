using System.Collections.Generic;

namespace JFrisoGames.Engine
{
	public class SeededRandom
	{
		public int Seed { get; }

		private System.Random _random;

		public static int GetRandomSeed() => (new System.Random()).Next();

		public SeededRandom(int? seed = null)
		{
			Seed = seed ?? GetRandomSeed();
			_random = new System.Random(Seed);
		}

		public int RandInt() => _random.Next();
		public int RandInt(int maxValue) => _random.Next(maxValue);
		public int RandInt(int minValue, int maxValue) => _random.Next(minValue, maxValue);

		public float RandFloat() => (float)_random.NextDouble();
		public float RandFloat(float minValue, float maxValue) => (float)_random.NextDouble() * (maxValue - minValue) + minValue;

		public double RandDouble() => _random.NextDouble();
		public double RandDouble(double minValue, double maxValue) => _random.NextDouble() * (maxValue - minValue) + minValue;

		public bool Bool() => _random.NextDouble() >= 0.5;
		public bool Bool(double probability) => _random.NextDouble() >= probability;

		public T ChooseRandom<T>(T[] choices) => choices[RandInt(0, choices.Length)];
		public T ChooseRandom<T>(IList<T> choices) => choices[RandInt(0, choices.Count)];

		public override string ToString() => $"Random ({Seed})";
	}
}
