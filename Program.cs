using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace SudokuVariants
{
	class Program
	{
		public static void Main(string[] args)
		{
			while (true)
			{
				Cycle();
			}
		}

		private static void Cycle()
		{
			Console.WriteLine("Type in two numbers");

			//create new board
			SudokuBoard sb = new SudokuBoard(
				Console.ReadKey().KeyChar - 48,
				Console.ReadKey().KeyChar - 48
			);
			Console.WriteLine();

			//shuffle it
			sb.Shuffle();

			//write board to console
			for (int i = 0; i < sb.WidthLen; i++)
			{
				for (int j = 0; j < sb.WidthLen; j++)
				{
					if (sb.Board[j, i] < 10)
						Console.Write(sb.Board[j, i].ToString() + "  ");
					else
						Console.Write(sb.Board[j, i] + " ");
			}
				Console.WriteLine();
			}


			Console.ReadKey();
	  }
	}
}
