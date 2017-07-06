using System;
using System.Collections.Generic;
using System.Collections;

namespace GameMazeCreator_01
{
	public class MazeCommon
	{
		// arry sort by E W N S
		//
		public static int N = 1, S = 2, E = 4, W = 8;
		public static int lockN = 16, lockS = 32, lockE = 64, lockW = 128;
		public static int[] DR = new int[] {N, S, E, W };
		public static int[] lockDR = new int[] {lockN, lockS, lockE, lockW};
		public static Dictionary<int, int> DX = new Dictionary<int, int> {{E, 1}, {W, -1}, {N, 0}, {S, 0}};
		public static Dictionary<int, int> DY = new Dictionary<int, int> {{E, 0}, {W, 0}, {N, -1}, {S, 1}};
		public static Dictionary<int, int> OPPOSITE = new Dictionary<int, int> {{E, W}, {W, E}, {N, S}, {S, N}};

		/// <summary>
		/// create the map by grid.
		/// 根据原始grid，生成不带block的简单map 
		/// </summary>
		/// <param name="grid">Grid.</param>
		public static int[,] CreateMapByGrid(int[,] grid)
		{
			System.Console.WriteLine ('_');

			int lenY = 1 + (grid.GetLength (0)) * 3;
			int lenX = 1 + (grid.GetLength (1)) * 3;
			int[,] map = new int[lenY, lenX ];


			//the first row of wall
			for (int tempY = 0; tempY < lenY; tempY++)
				for (int tempX = 0; tempX < lenX; tempX++)
					map [tempY, tempX] = 1;


			for (int y= 0; y < grid.GetLength(0); y++) {
				//map [y, 0] = 1;
				for (int x = 0; x < grid.GetLength (1); x++) {

					if (grid [y, x] == 0) {
						continue;
					}

					map [1 + y * 3, 1 + x * 3] = 0;
					map [2 + y * 3, 1 + x * 3] = 0;
					map [1 + y * 3, 2 + x * 3] = 0;
					map [2 + y * 3, 2 + x * 3] = 0;

					if ((grid [y, x] & N) != 0) {
						map [0 + y * 3, 1 + x * 3] = 0;
						map [0 + y * 3, 2 + x * 3] = 0;
					}
					if ((grid [y, x] & S) != 0) {
						map [3 + y * 3, 1 + x * 3] = 0;
						map [3 + y * 3, 2 + x * 3] = 0;
					}

					if ((grid [y, x] & E) != 0) {
						map [1 + y * 3, 3 + x * 3] = 0;
						map [2 + y * 3, 3 + x * 3] = 0;
					}
					if ((grid [y, x] & W) != 0) {
						map [1 + y * 3, 0 + x * 3] = 0;
						map [2 + y * 3, 0 + x * 3] = 0;
					}

				}
			}

			return map;

		}

		public static int[,] CreateMapByScaleDoubleGrid(int[,] grid)
		{
			if (grid.GetLength (0) % 2 != 0 && grid.GetLength (1) % 2 != 0) {
				Console.WriteLine ("CreateMapByScaleDoubleGrid Error!!! height: " + grid.GetLength (0) + ", width: " + grid.GetLength (1));
				return new int[0, 0];
			}

			System.Console.WriteLine ('_');

			int lenY = 1 + (grid.GetLength (0) / 2) * 3;
			int lenX = 1 + (grid.GetLength (1) / 2) * 3;
			int[,] map = new int[lenY, lenX];


			//the first row of wall
			for (int tempY = 0; tempY < lenY; tempY++)
				for (int tempX = 0; tempX < lenX; tempX++)
					map [tempY, tempX] = 1;


			for (int y= 0; y < grid.GetLength(0) / 2; y++) {
				//map [y, 0] = 1;
				for (int x = 0; x < grid.GetLength (1) / 2; x++) {

					for (int tempY = 0; tempY < 2; tempY++) {
						for (int tempX = 0; tempX < 2; tempX++) {
							
							if (grid [tempY + y * 2, tempX + x * 2] == 0) {
								map [1 + tempY + y * 3, 1 + tempX + x * 3] = -1;
							} else {
								map [1 + tempY + y * 3, 1 + tempX + x * 3] = 0;
							}

							if (tempY == 0) {
								if ((grid [tempY + y * 2, tempX + x * 2] & N) != 0) {
									map [1 + tempY - 1 + y * 3, 1 + tempX + x * 3] = 0;
								}
							} else if (tempY == 1) {
								if ((grid [tempY + y * 2, tempX + x * 2] & S) != 0) {
									map [1 + tempY + 1 + y * 3, 1 + tempX + x * 3] = 0;
								}
							}

							if (tempX == 0) {
								if ((grid [tempY + y * 2, tempX + x * 2] & W) != 0) {
									map [1 + tempY + y * 3, 1 + tempX - 1 + x * 3] = 0;
								}
							} else if (tempX == 1) {
								if ((grid [tempY + y * 2, tempX + x * 2] & E) != 0) {
									map [1 + tempY + y * 3, 1 + tempX + 1 + x * 3] = 0;
								}
							}
						}
					}
				}
			}

			return map;

		}

		/// <summary>
		/// Creates the map by grid v2.
		/// 根据原始grid，生成带block的最终map
		/// </summary>
		/// <returns>The map by grid v2.</returns>
		/// <param name="grid">Grid.</param>
		public static int[,] CreateMapByGridV2(int[,] grid)
		{
			System.Console.WriteLine ("version 0.2");

			/*for (int y= 0; y < grid.GetLength(0); y++) {
				for(int x= 0; x < grid.GetLength(0); x++)
					System.Console.Write(grid[y, x]);
				System.Console.WriteLine ();
			}*/
			int lenY = 1 + (grid.GetLength (0)) * 3;
			int lenX = 1 + (grid.GetLength (1)) * 3;
			int[,] map = new int[lenY, lenX ];


			//the first row of wall
			for (int tempY = 0; tempY < lenY; tempY++)
				for (int tempX = 0; tempX < lenX; tempX++)
					map [tempY, tempX] = 1;


			for (int y= 0; y < grid.GetLength(0); y++) {
				//map [y, 0] = 1;
				for (int x = 0; x < grid.GetLength (0); x++) {

					if ((grid [y, x] & N) != 0) {
						if (map [0 + y * 3, 1 + x * 3] != 0 && map [0 + y * 3, 2 + x * 3] != 0) { // not reach by neighbor;
							int temp = MazeCommon.GetRandom().Next(1, 3); //get 1 or 2
							map [0 + y * 3, temp + x * 3] = 0;
						}
					}
					if ((grid [y, x] & S) != 0) {
						if (map [3 + y * 3, 1 + x * 3] != 0 && map [3 + y * 3, 2 + x * 3] != 0) { // not reach by neighbor;
							int temp = MazeCommon.GetRandom().Next(1, 3); //get 1 or 2
							map [3 + y * 3, temp + x * 3] = 0;
						}
					}
					if ((grid [y, x] & W) != 0) {
						if (map [1 + y * 3, 0 + x * 3] != 0 && map [2 + y * 3, 0 + x * 3] != 0) { // not reach by neighbor;
							int temp = MazeCommon.GetRandom().Next(1, 3); //get 1 or 2
							map [temp + y * 3, 0 + x * 3] = 0;
						}
					}
					if ((grid [y, x] & E) != 0) {
						if (map [1 + y * 3, 3 + x * 3] != 0 && map [2 + y * 3, 3 + x * 3] != 0) { // not reach by neighbor;
							int temp = MazeCommon.GetRandom().Next(1, 3); //get 1 or 2
							map [temp + y * 3, 3 + x * 3] = 0;
						}
					}
						

					if (map [0 + y * 3, 1 + x * 3] == 0 || map [1 + y * 3, 0 + x * 3] == 0)
						map [1 + y * 3, 1 + x * 3] = 0; // (0, 1), (1, 0) ==> (1, 1)
					else {
						if (MazeCommon.GetRandom().Next(0, 2) == 0)
							map [1 + y * 3, 1 + x * 3] = -1;
						else 
							map [1 + y * 3, 1 + x * 3] = 0;
					}
					
					if (map [0 + y * 3, 2 + x * 3] == 0 || map [1 + y * 3, 3 + x * 3] == 0)
						map [1 + y * 3, 2 + x * 3] = 0; // (0, 2), (1, 3) ==> (1, 2)
					else {
						if (MazeCommon.GetRandom ().Next (0, 2) == 0)
							map [1 + y * 3, 2 + x * 3] = -1;
						else
							map [1 + y * 3, 2 + x * 3] = 0;
					}

					if (map [3 + y * 3, 1 + x * 3] == 0 || map [2 + y * 3, 0 + x * 3] == 0)
						map [2 + y * 3, 1 + x * 3] = 0; // (3, 1), (2, 0) ==> (2, 1)
					else {
						if (MazeCommon.GetRandom ().Next (0, 2) == 0)
							map [2 + y * 3, 1 + x * 3] = -1;
						else
							map [2 + y * 3, 1 + x * 3] = 0;
					}

					if (map [3 + y * 3, 2 + x * 3] == 0 || map [2 + y * 3, 3 + x * 3] == 0)
						map [2 + y * 3, 2 + x * 3] = 0; // (3, 2), (2, 3) ==> (2, 2)
					else {
						if (MazeCommon.GetRandom ().Next (0, 2) == 0)
							map [2 + y * 3, 2 + x * 3] = -1;
						else
							map [2 + y * 3, 2 + x * 3] = 0;
					}

					if (map [1 + y * 3, 1 + x * 3] == 0 && map [2 + y * 3, 2 + x * 3] == 0 
					    && map [1 + y * 3, 2 + x * 3] != 0 && map [2 + y * 3, 1 + x * 3] != 0) {
						if (MazeCommon.GetRandom ().Next (0, 2) == 0)
							map [1 + y * 3, 2 + x * 3] = 0;
						else
							map [2 + y * 3, 1 + x * 3] = 0;
					} else if (map [1 + y * 3, 1 + x * 3] != 0 && map [2 + y * 3, 2 + x * 3] != 0 
						&& map [1 + y * 3, 2 + x * 3] == 0 && map [2 + y * 3, 1 + x * 3] == 0) {
						if (MazeCommon.GetRandom ().Next (0, 2) == 0)
							map [1 + y * 3, 1 + x * 3] = 0;
						else
							map [2 + y * 3, 2 + x * 3] = 0;
					}
						
				}
			}

			return map;

		}

		public static int[,] GridScaleDouble (int[,] grid) {
			int height = grid.GetLength (0);
			int width = grid.GetLength (1);
			int[,] result = new int[height * 2, width * 2 ];
			for (int y = 0; y < height * 2; y++) {
				for (int x = 0; x < width * 2; x++) {
					result [y, x] |= (N + S + E + W);
				}
			}

			for (int y = 0; y < height; y++) {
				for (int x = 0; x < width; x++) {
					if ((grid [y, x] & N) == 0) {
						result [y * 2, x * 2] -= N;
						result [y * 2, x * 2 + 1] -= N;
					}

					if ((grid [y, x] & S) == 0) {
						result [y * 2 + 1, x * 2] -= S;
						result [y * 2 + 1, x * 2 + 1] -= S;
					}

					if ((grid [y, x] & W) == 0) {
						result [y * 2, x * 2] -= W;
						result [y * 2 + 1, x * 2] -= W;
					}

					if ((grid [y, x] & E) == 0) {
						result [y * 2, x * 2 + 1] -= E;
						result [y * 2 + 1, x * 2 + 1] -= E;
					}
				}
			}

			return result;
		}

		public static int[,] GridScaleDoubleWithBlock(int[,] grid) {
			int height = grid.GetLength (0);
			int width = grid.GetLength (1);
			int[,] result = new int[height * 2, width * 2 ];

			for (int y = 0; y < height; y++) {
				for (int x = 0; x < width; x++) {
					
					if ((grid [y, x] & N) != 0) {
						if (y > 0) {
							if ((result [-1 + y * 2, 0 + x * 2] & S) != 0)
								result [0 + y * 2, 0 + x * 2] |= N; // (0, 0)
						else if ((result [-1 + y * 2, 1 + x * 2] & S) != 0)
								result [0 + y * 2, 1 + x * 2] |= N; // (0, 1)
						else { // not reach by neighbor;
								int temp = MazeCommon.GetRandom ().Next (0, 2); //get 0 or 1
								result [0 + y * 2, temp + x * 2] |= N;
							}
						} else {
							int temp = MazeCommon.GetRandom ().Next (0, 2); //get 0 or 1
							result [0 + y * 2, temp + x * 2] |= N;
						}
					}

					if ((grid [y, x] & S) != 0 ) {
						if (y < height - 1) {
							if ((result [2 + y * 2, 0 + x * 2] & N) != 0)
								result [1 + y * 2, 0 + x * 2] |= S; // (1, 0)
						else if ((result [2 + y * 2, 1 + x * 2] & N) != 0)
								result [1 + y * 2, 1 + x * 2] |= S; // (1, 1)
						else { // not reach by neighbor;
								int temp = MazeCommon.GetRandom ().Next (0, 2); //get 0 or 1
								result [1 + y * 2, temp + x * 2] |= S;
							}
						} else {
							int temp = MazeCommon.GetRandom ().Next (0, 2); //get 0 or 1
							result [1 + y * 2, temp + x * 2] |= S;
						}
					}

					if ((grid [y, x] & W) != 0) {
						if (x > 0) {
							if ((result [0 + y * 2, -1 + x * 2] & E) != 0)
								result [0 + y * 2, 0 + x * 2] |= W; // (0, 0)
						else if ((result [1 + y * 2, -1 + x * 2] & E) != 0)
								result [1 + y * 2, 0 + x * 2] |= W; // (1, 0)
						else { // not reach by neighbor;
								int temp = MazeCommon.GetRandom ().Next (0, 2); //get 0 or 1
								result [temp + y * 2, 0 + x * 2] |= W;
							}
						} else {
							int temp = MazeCommon.GetRandom ().Next (0, 2); //get 0 or 1
							result [temp + y * 2, 0 + x * 2] |= W;
						}
					}

					if ((grid [y, x] & E) != 0) {
						if (x < width - 1) {
							if ((result [0 + y * 2, 2 + x * 2] & W) != 0)
								result [0 + y * 2, 1 + x * 2] |= E; // (0, 1)
						else if ((result [1 + y * 2, 2 + x * 2] & W) != 0)
								result [1 + y * 2, 1 + x * 2] |= E; // (1, 1)
						else { // not reach by neighbor;
								int temp = MazeCommon.GetRandom ().Next (0, 2); //get 0 or 1
								result [temp + y * 2, 1 + x * 2] |= E;
							}
						} else {
							int temp = MazeCommon.GetRandom ().Next (0, 2); //get 0 or 1
							result [temp + y * 2, 1 + x * 2] |= E;
						}
					}

					if (result [0 + y * 2, 0 + x * 2] != 0 && result [1 + y * 2, 1 + x * 2] != 0
					    && result [0 + y * 2, 1 + x * 2] == 0 && result [1 + y * 2, 0 + x * 2] == 0) {
						int temp = MazeCommon.GetRandom ().Next (0, 2); //get 0 or 1
						if (temp == 0) {
							result [0 + y * 2, 1 + x * 2] |= W;
							result [0 + y * 2, 1 + x * 2] |= S;
						} else {
							result [1 + y * 2, 0 + x * 2] |= E;
							result [1 + y * 2, 0 + x * 2] |= N;
						}
					} else if (result [0 + y * 2, 0 + x * 2] == 0 && result [1 + y * 2, 1 + x * 2] == 0
					           && result [0 + y * 2, 1 + x * 2] != 0 && result [1 + y * 2, 0 + x * 2] != 0) {
						int temp = MazeCommon.GetRandom ().Next (0, 2); //get 0 or 1
						if (temp == 0) {
							result [0 + y * 2, 0 + x * 2] |= E;
							result [0 + y * 2, 0 + x * 2] |= S;
						} else {
							result [1 + y * 2, 1 + x * 2] |= W;
							result [1 + y * 2, 1 + x * 2] |= N;
						}
					}

					// (0, 0) <--> (0, 1) E, W
					if (result [0 + y * 2, 0 + x * 2] != 0 && result [0 + y * 2, 1 + x * 2] != 0) {
						result [0 + y * 2, 0 + x * 2] |= E;
						result [0 + y * 2, 1 + x * 2] |= W;
					}
					// (0, 0) <--> (1, 0) 
					if (result [0 + y * 2, 0 + x * 2] != 0 && result [1 + y * 2, 0 + x * 2] != 0) {
						result [0 + y * 2, 0 + x * 2] |= S;
						result [1 + y * 2, 0 + x * 2] |= N;
					}
					// (1, 1) <--> (1, 0) W E
					if (result [1 + y * 2, 1 + x * 2] != 0 && result [1 + y * 2, 0 + x * 2] != 0) {
						result [1 + y * 2, 1 + x * 2] |= W;
						result [1 + y * 2, 0 + x * 2] |= E;
					}
					// (1, 1) <--> (0, 1) N S
					if (result [1 + y * 2, 1 + x * 2] != 0 && result [0 + y * 2, 1 + x * 2] != 0) {
						result [1 + y * 2, 1 + x * 2] |= N;
						result [0 + y * 2, 1 + x * 2] |= S;
					}

				}
			}
			//return 
			return result;
		}



		#region Print Funcs
		public static void PrintArray(int[,] map) {
			for (int i = 0; i < map.GetLength (0); i++) {
				for (int j = 0; j < map.GetLength (1); j++) {
					Console.Write (map [i, j]);
				}
				Console.WriteLine ();
			}
		}

		public static void Print2DArrayWithWall(int[,] map) {
			for (int i = 0; i < map.GetLength (0); i++) {
				for (int j = 0; j < map.GetLength (1); j++) {
					if (j % 3 == 0 && i % 3 == 0) {
						Console.Write ("@");
						//continue;
					} else if (map [i, j] == 0)
						Console.Write (".");
					//Console.Write (map [i, j]);
					else if (map [i, j] < 0)
						Console.Write ("*");
					else
						Console.Write ("#");
					//Console.Write (map [i, j]);
				}
				Console.WriteLine ();
			}

		}

		public static void Print2DArray(int[,] map) {
			int space = 0;
			int block = 0;
			for (int i = 0; i < map.GetLength (0); i++) {
				if (i % 3 == 0)
					continue;
				for (int j = 0; j < map.GetLength (1); j++) {
					if (j % 3 == 0)
						continue;
					if (map [i, j] == 0) {
						Console.Write (".");
						space++;
					}
						//Console.Write (map [i, j]);
					else if (map [i, j] < 0) {
						Console.Write ("*");
						block++;
					} else {
						Console.Write ("#");
						//Console.Write (map [i, j]);
					}
				}
				Console.WriteLine ();
			}
			Console.WriteLine (". count is " + space + ", * count is " + block);
			Console.WriteLine (". proportion is " + ((float)space / (space + block)) + ", * proportion is " + ((float)block / (space + block)) );
		}
		#endregion

		/// <summary>
		/// insert spaceBlocker every (split * split)
		/// split should > 4
		/// </summary>
		/// <returns>The insert space.</returns>
		/// <param name="maze">grid. gridx2 with blocks</param>
		/// <param name="split">Split.</param>
		public static int[,] MazeInsertSpace(int[,] gird, int split){
			int height = gird.GetLength (0);
			int width = gird.GetLength (1);
			if (height < split || width < split || split <= 4) // min split is 5
				return gird;

			int lenX = width / split;
			int lenY = height / split;
			Space[,] spaces = new Space[lenY, lenX];
			Console.WriteLine ("lenx * leny is " + (lenX * lenY));

			for (int i = 0; i < lenY; i++) {
				for (int j = 0; j < lenX; j++) {
					
					int tempWidth = GetRandom ().Next (2, 4); // get 2 or 3
					int tempX = GetRandom ().Next (1, split - 1); // get 1, 2, 3 ... split-1
					while (tempX + tempWidth > split - 1) {
						tempWidth = GetRandom ().Next (2, 4);
						tempX = GetRandom ().Next (1, split - 1);
					}

					int tempHeight = GetRandom ().Next (2, 4); // get 2 or 3
					int tempY = GetRandom ().Next (1, split - 1);
					while (tempY + tempHeight > split - 1) {
						tempHeight = GetRandom ().Next (2, 4);
						tempY = GetRandom ().Next (1, split - 1);
					}
					// so the space blocker is define by (tempY, tempX), tempHeight and tempWidth;
					spaces[i, j] = new Space(tempX, tempY, tempWidth, tempHeight);
					Console.WriteLine("i : " + i + ", j : " + j + ", x : " + tempX + ", y : " + tempY + ", width : " + tempWidth + ", height : " + tempHeight);
				}
			}

			for (int i = 0; i < lenY; i++) {
				for (int j = 0; j < lenX; j++) {
					//change the cell attributes
					for (int tempY = 0; tempY < spaces [i, j].height; tempY++) {
						for (int tempX = 0; tempX < spaces [i, j].width; tempX++) {
							
							gird [tempY + spaces [i, j].anchor.y + split * i, tempX + spaces [i, j].anchor.x + split * j] |= (N + S + W + E);

							if (tempY == 0 && 
								(gird [tempY + spaces [i, j].anchor.y + split * i - 1, tempX + spaces [i, j].anchor.x + split * j] & S) == 0) {
								gird [tempY + spaces [i, j].anchor.y + split * i, tempX + spaces [i, j].anchor.x + split * j] -= N;
							} else if (tempY == (spaces [i, j].height - 1) &&
								(gird [tempY + spaces [i, j].anchor.y + split * i + 1, tempX + spaces [i, j].anchor.x + split * j] & N) == 0){
								gird [tempY + spaces [i, j].anchor.y + split * i, tempX + spaces [i, j].anchor.x + split * j] -= S;
							}

							if (tempX == 0 &&
								(gird [tempY + spaces [i, j].anchor.y + split * i, tempX + spaces [i, j].anchor.x + split * j - 1] & E) == 0) {
								gird [tempY + spaces [i, j].anchor.y + split * i, tempX + spaces [i, j].anchor.x + split * j] -= W;
							} else if  (tempX == (spaces [i, j].width - 1) &&
								(gird [tempY + spaces [i, j].anchor.y + split * i, tempX + spaces [i, j].anchor.x + split * j + 1] & W) == 0) {
								gird [tempY + spaces [i, j].anchor.y + split * i, tempX + spaces [i, j].anchor.x + split * j] -= E;
							}
						}
					}
				}
			}

			return gird;
		}

		public static int[,] AdjustMazeBorder(int[,] grid) {
			int height = grid.GetLength (0);
			int width = grid.GetLength (1);
			int[,] gridClone = grid.Clone () as int[,];
			// adjust the grid here
			int y0 = 0;
			int y1 = 0;
			int x0 = 0;
			int x1 = 0;

			// y == 0
			for (int j = 0; j < width; j++) {
				if ((gridClone [0, j] & MazeCommon.E) == 0) { // not connect to east direction
					int tempX = MazeCommon.GetRandom ().Next (x0, j + 1); // get x0 ~ j
					grid [0, tempX] |= MazeCommon.N;
					x0 = j + 1;
				} else {
					x0 = j;
				}
			}

			// y == height - 1 
			for (int j = 0; j < width; j++) {
				if ((gridClone [height - 1, j] & MazeCommon.E) == 0) { // not connect to east direction
					int tempX = MazeCommon.GetRandom ().Next (x1, j + 1); // get x0 ~ j
					grid [height -1, tempX] |= MazeCommon.S;
					x1 = j + 1;
				} else {
					x1 = j;
				}
			}

			// x == 0
			for (int i = 0; i < height; i++) {
				if ((gridClone [i, 0] & MazeCommon.S) == 0) { // not connect to south direction
					int tempY = MazeCommon.GetRandom ().Next (y0, i + 1); // get y0 ~ i
					grid [tempY, 0] |= MazeCommon.W;
					y0 = i + 1;
				} else {
					y0 = i;
				}
			}

			// x == width - 1
			for (int i = 0; i < height; i++) {
				if ((gridClone [i, width - 1] & MazeCommon.S) == 0) { // not connect to south direction
					int tempY = MazeCommon.GetRandom ().Next (y1, i + 1); // get y0 ~ i
					grid [tempY, width - 1] |= MazeCommon.E;
					y1 = i + 1;
				} else {
					y1 = i;
				}
			}
				
			return grid;
		}

		#region Get Random
		static Hashtable hashtable = new Hashtable ();
		static bool isGo = false;
		//再写一个函数保证一定时间内生成的所有seed都不同
		public static Random GetRandom(){
			isGo = !isGo;
			if (isGo) {
				int iSeed = GetRandomSeed();
				Random ro = new Random(iSeed);
				return ro;
			}
			long tick = DateTime.Now.Ticks;
			Random ran = new Random((int)(tick & 0xffffffffL) | (int) (tick >> 32)); 
			return ran;
		}

		static int GetRandomSeed() {
			while (true) {
				if (hashtable.Count >= (int.MaxValue / 10))
					hashtable = new Hashtable ();

				int seed = (new Random()).Next ();
				if (!hashtable.Contains (seed)) {
					hashtable.Add (seed, seed);
					return seed;
				}
			}
		}
		#endregion


	}

	public struct Space {
		//public int x;
		//public int y;
		public Point anchor;
		public int height;
		public int width;
		public int level;
		public List<Point> addOn; //save the add on points of space area. the coodinate is the actual point's coodinate of (y, x);
		public List<Point> paths;
		public Space(int x, int y, int width, int height){
			this.anchor = new Point (x, y);
			this.width = width;
			this.height = height;
			this.level = 0;
			this.addOn = new List<Point> ();
			this.paths = new List<Point> ();
		}

		public Space(int x, int y, int width, int height, int level) : this (x, y, width, height) {
			this.level = level;
		}
	}
}

