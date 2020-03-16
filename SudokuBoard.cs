using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace SudokuVariants
{
	class SudokuBoard
	{
		public Int32[,] Board { get; set; }
		public Int32 Number1 { get; }
		public Int32 Number2 { get; }
		public Int32 WidthLen { get; }
		static public Random Rnd { get; } = new Random((Int32)((DateTimeOffset)DateTime.UtcNow).ToUnixTimeSeconds());

		/// <summary>
		/// a board with numbers distributed over [column,row]
		/// </summary>
		/// <param name="number1">together with number 2 determines the width and length of the board, and if they differ determines the non-square pattern of the board</param>
		/// <param name="number2"></param>
		public SudokuBoard(Int32 number1, Int32 number2)
		{
			//ensure number2 isn't bigger than number1
			if (number1 < number2)
			{
				number1 *= number2;
				number2 = number1 / number2;
				number1 /= number2;
			}

			Number1 = number1;
			Number2 = number2;

			WidthLen = number1 * number2;

			Board = new Int32[WidthLen, WidthLen];

			for (Int32 x = 0; x < WidthLen; x++)
			{
				for (Int32 y = 0; y < WidthLen; y++)
				{
					Board[x, y] = (x + y * number1 + y / number2) % WidthLen + 1;
				}
			}
		}

		/// <summary>
		/// Shuffle the board around without invalidating it. If the regions are 3 wide by 2 tall the first 3 columns can be swapped with each other, and the next 3, and so on. Rows are swapped similarly. Finally, regions in a column are swapped with regions in other columns.
		/// </summary>
		public void Shuffle()
		{
			Int32[] temp1 = new Int32[Number1];
			for (int i = 0; i < Number1; i++)
			{
				temp1[i] = i;
			}

			//randomize the order in which the rows and columns will be shuffled within their regional sets of rows and columns.
			Int32[] rowNumbering = new Int32[WidthLen];
			Int32[] columnNumbering = new Int32[WidthLen];
			foreach (var v in new List<Int32[]> {rowNumbering, columnNumbering})
			{
				for (Int32 i = 0; i < Number2; i++)
				{
					temp1 = temp1.OrderBy(c => Rnd.Next()).ToArray();
					for (Int32 j = 0; j < Number1; j++)
					{
						v[i * Number1 + j] = temp1[j] + i * Number2;
					}
				}
			}

			//randomize the order in which the sets of columns/rows will be shuffled
			Int32[] regionRowNumbering = new Int32[Number1];
			for (int i = 0; i < Number1; i++)
			{
				regionRowNumbering[i] = i;
			}

			Int32[] regionColumnNumbering = new Int32[Number2];
			for (int i = 0; i < Number2; i++)
			{
				regionColumnNumbering[i] = i;
			}

			//randomize the order in which the sets of columns/rows will be shuffled
			regionColumnNumbering = regionColumnNumbering.OrderBy(_ => Rnd.Next()).ToArray();
			regionRowNumbering = regionRowNumbering.OrderBy(_ => Rnd.Next()).ToArray();

			//finally create and assign the new shuffled array
			Int32[,] newArr = new Int32[WidthLen, WidthLen];
			for (int x = 0; x < WidthLen; x++)
			{
				for (int y = 0; y < WidthLen; y++)
				{
					newArr[x, y] = Board[
						columnNumbering[regionColumnNumbering[x/Number1]*Number1 + x%Number1], 
						rowNumbering[regionRowNumbering[y/Number2]*Number2 + y%Number2]
					];
				}
			}

			Board = newArr;
		}
	}
}