using System;
using System.Collections.Generic;

namespace GameMazeCreator_01
{
	public class MazeGeneratorCommon {
		#region Create map by maze..
		/// <summary>
		/// Creates the map by maze.
		/// </summary>
		/// <returns>The map by maze.</returns>
		/// <param name="maze">Maze.</param>
		public static int[,] CreateMapByMaze (Maze maze) {

			int height = maze.maze.GetLength (0);
			int width = maze.maze.GetLength (1);
			Cell[,] grid = maze.maze.Clone () as Cell[,];

			int lenY = 1 + height * 3;
			int lenX = 1 + width * 3;
			int[,] map = new int[lenY, lenX];


			//the first row of wall
			for (int tempY = 0; tempY < lenY; tempY++)
				for (int tempX = 0; tempX < lenX; tempX++)
					map [tempY, tempX] = 1;


			for (int y= 0; y < height; y++) {
				//map [y, 0] = 1;
				for (int x = 0; x < width; x++) {

					if (grid [y, x].direction == 0) {
						continue;
					}

					map [1 + y * 3, 1 + x * 3] = 0;
					map [2 + y * 3, 1 + x * 3] = 0;
					map [1 + y * 3, 2 + x * 3] = 0;
					map [2 + y * 3, 2 + x * 3] = 0;

					if ((grid [y, x].direction & MazeCommon.N) != 0){
						map [0 + y * 3, 1 + x * 3] = 0;
						map [0 + y * 3, 2 + x * 3] = 0;
					}
					if ((grid [y, x].direction & MazeCommon.S) != 0) {
						map [3 + y * 3, 1 + x * 3] = 0;
						map [3 + y * 3, 2 + x * 3] = 0;
					}

					if ((grid [y, x].direction & MazeCommon.E) != 0) {
						map [1 + y * 3, 3 + x * 3] = 0;
						map [2 + y * 3, 3 + x * 3] = 0;
					}
					if ((grid [y, x].direction & MazeCommon.W) != 0) {
						map [1 + y * 3, 0 + x * 3] = 0;
						map [2 + y * 3, 0 + x * 3] = 0;
					}

				}
			}

			return map;
		}

		public static void PrintComplexTerrainMapWithLevelGrid(int[,] terrainMap, CellType[,] levelGrid) {
			int height = terrainMap.GetLength (0);
			int width = terrainMap.GetLength (1);
			CellType[,] map = new CellType[height, width];

			for (int i = 0; i < height; i++) {
				for (int j = 0; j < width; j++) {
					int tempY = i % 3;
					int tempX = j % 3;
					if (tempY == 0 || tempX == 0) {
						if (terrainMap [i, j] == 0)
							map [i, j] = CellType.PATH;
						else
							map [i, j] = CellType.BLOCK;
					} else {
						map [i, j] = levelGrid [(i - tempY) / 3 * 2 + tempY - 1, (j - tempX) / 3 * 2 + tempX - 1];
					}

				}
			}
			levelGrid = map;
			for (int i = 0; i < height; i++) {
				for (int j = 0; j < width; j++) {
					//Console.Write ((int)levelGrid [i, j]);
					if (i % 3 == 0 && j % 3 == 0) {
						Console.Write ("@");
						continue;
					}

					if (i % 3 == 0 || j % 3 == 0) {
						if ((levelGrid [i, j] == MazeGeneratorCommon.CellType.BLOCK)) {
							Console.Write ("#");
							continue;
						} else if ((levelGrid [i, j] == MazeGeneratorCommon.CellType.PATH)) {
							Console.Write ("_");
							continue;
						}
					}

					if ((levelGrid [i, j] == MazeGeneratorCommon.CellType.BLOCK))
						Console.Write ("#");
					else if ((levelGrid [i, j] == MazeGeneratorCommon.CellType.DOOR))
						Console.Write ("D");
					else if ((levelGrid [i, j] == MazeGeneratorCommon.CellType.PATH))
						Console.Write ("P");
					else if ((levelGrid [i, j] == MazeGeneratorCommon.CellType.SPACE))
						Console.Write (".");
					else
						Console.Write (" ");
				}
				Console.WriteLine ();
			}
		}

		/// <summary>
		/// Creates the map by terrain maze.
		/// </summary>
		/// <returns>The map by terrain maze.</returns>
		/// <param name="maze">Maze.</param>
		public static int[,] CreateMapByTerrainMaze(Maze maze) {
			Cell[,] grid = maze.terrainMaze.Clone () as Cell[,];
			int height = grid.GetLength (0);
			int width = grid.GetLength (1);

			if (height % 2 != 0 && width % 2 != 0) {
				Console.WriteLine ("CreateMapByScaleDoubleGrid Error!!! height: " + height + ", width: " + width);
				return new int[0, 0];
			}

			Console.WriteLine ("_CreateMapByTerrainMaze_");

			int lenY = 1 + (height / 2) * 3;
			int lenX = 1 + (width / 2) * 3;
			int[,] map = new int[lenY, lenX];


			//as first, set all to be wall
			for (int tempY = 0; tempY < lenY; tempY++)
				for (int tempX = 0; tempX < lenX; tempX++)
					map [tempY, tempX] = 1;


			for (int y = 0; y < height / 2; y++) {
				for (int x = 0; x < width / 2; x++) {

					for (int tempY = 0; tempY < 2; tempY++) {
						for (int tempX = 0; tempX < 2; tempX++) {

							if (grid [tempY + y * 2, tempX + x * 2].direction == 0) {
								map [1 + tempY + y * 3, 1 + tempX + x * 3] = -1;
							} else {
								map [1 + tempY + y * 3, 1 + tempX + x * 3] = 0;
							}

							if (tempY == 0) {
								if ((grid [tempY + y * 2, tempX + x * 2].direction & MazeCommon.N) != 0) {
									map [1 + tempY - 1 + y * 3, 1 + tempX + x * 3] = 0;
								}
							} else if (tempY == 1) {
								if ((grid [tempY + y * 2, tempX + x * 2].direction & MazeCommon.S) != 0) {
									map [1 + tempY + 1 + y * 3, 1 + tempX + x * 3] = 0;
								}
							}

							if (tempX == 0) {
								if ((grid [tempY + y * 2, tempX + x * 2].direction & MazeCommon.W) != 0) {
									map [1 + tempY + y * 3, 1 + tempX - 1 + x * 3] = 0; // tempX - 1 == -1
								}
							} else if (tempX == 1) {
								if ((grid [tempY + y * 2, tempX + x * 2].direction & MazeCommon.E) != 0) {
									map [1 + tempY + y * 3, 1 + tempX + 1 + x * 3] = 0; // tempX + 1 == 2
								}
							}
						}
					}
				}
			}

			return map;

		}
		#endregion

		/// <summary>
		/// Adjusts the maze border.
		/// </summary>
		/// <returns>The maze border.</returns>
		/// <param name="maze">Maze.</param>
		public static Maze AdjustMazeBorder(Maze maze) {

			Cell[,] grid = maze.maze.Clone () as Cell[,];
			int height = grid.GetLength (0);
			int width = grid.GetLength (1);
			Cell[,] gridClone = grid.Clone () as Cell[,];
			// adjust the grid here
			int y0 = 0;
			int y1 = 0;
			int x0 = 0;
			int x1 = 0;

			// y == 0
			if (!maze.neighborMazes.ContainsKey (MazeCommon.N)) {
				for (int j = 0; j < width; j++) {
					if ((gridClone [0, j].direction & MazeCommon.E) == 0) { // not connect to east direction
						int tempX = MazeCommon.GetRandom ().Next (x0, j + 1); // get x0 ~ j
						grid [0, tempX].direction |= MazeCommon.N;
						x0 = j + 1;
					} else {
						x0 = j;
					}
				}
			}

			// y == height - 1 
			if (!maze.neighborMazes.ContainsKey (MazeCommon.S)) {
				for (int j = 0; j < width; j++) {
					if ((gridClone [height - 1, j].direction & MazeCommon.E) == 0) { // not connect to east direction
						int tempX = MazeCommon.GetRandom ().Next (x1, j + 1); // get x0 ~ j
						grid [height - 1, tempX].direction |= MazeCommon.S;
						x1 = j + 1;
					} else {
						x1 = j;
					}
				}
			}

			// x == 0
			if (!maze.neighborMazes.ContainsKey (MazeCommon.W)) {
				for (int i = 0; i < height; i++) {
					if ((gridClone [i, 0].direction & MazeCommon.S) == 0) { // not connect to south direction
						int tempY = MazeCommon.GetRandom ().Next (y0, i + 1); // get y0 ~ i
						grid [tempY, 0].direction |= MazeCommon.W;
						y0 = i + 1;
					} else {
						y0 = i;
					}
				}
			}

			// x == width - 1
			if (!maze.neighborMazes.ContainsKey (MazeCommon.E)) {
				for (int i = 0; i < height; i++) {
					if ((gridClone [i, width - 1].direction & MazeCommon.S) == 0) { // not connect to south direction
						int tempY = MazeCommon.GetRandom ().Next (y1, i + 1); // get y0 ~ i
						grid [tempY, width - 1].direction |= MazeCommon.E;
						y1 = i + 1;
					} else {
						y1 = i;
					}
				}
			}

			maze.maze = grid;
			return maze;
		}

		/// <summary>
		/// Inits the maze by maze neighbors.
		/// </summary>
		/// <returns>The maze by neighbors.</returns>
		/// <param name="maze">Maze.</param>
		public static Maze InitMazeByNeighbors(Maze maze) {
				// N
			if (maze.neighborMazes.ContainsKey (MazeCommon.N)) {
				if (maze.neighborMazes [MazeCommon.N].maze.GetLength (0) == maze.maze.GetLength (0) &&
				    maze.neighborMazes [MazeCommon.N].maze.GetLength (1) == maze.maze.GetLength (1)) {
					int height = maze.maze.GetLength (0);
					int width = maze.maze.GetLength (1);
					Console.WriteLine ("has N");
					int y = height - 1;
					for (int x = 0; x < width; x++) {
						if ((maze.neighborMazes [MazeCommon.N].maze [y, x].direction & MazeCommon.S) != 0) {
							maze.maze [0, x].direction |= MazeCommon.N;
							maze.maze [0, x].level = maze.neighborMazes [MazeCommon.N].maze [y, x].level;
						}
					}
				}
			}
				// S
			if (maze.neighborMazes.ContainsKey (MazeCommon.S)) {
				if (maze.neighborMazes [MazeCommon.S].maze.GetLength (0) == maze.maze.GetLength (0) &&
				    maze.neighborMazes [MazeCommon.S].maze.GetLength (1) == maze.maze.GetLength (1)) {
					int height = maze.maze.GetLength (0);
					int width = maze.maze.GetLength (1);
					Console.WriteLine ("has S");
					int y = 0;
					for (int x = 0; x < width; x++) {
						if ((maze.neighborMazes [MazeCommon.S].maze [y, x].direction & MazeCommon.N) != 0) {
							maze.maze [height - 1, x].direction |= MazeCommon.S;
							maze.maze [height - 1, x].level = maze.neighborMazes [MazeCommon.S].maze [y, x].level;
						}
					}
				}
			}

				// W
			if (maze.neighborMazes.ContainsKey (MazeCommon.W)) {
				if (maze.neighborMazes [MazeCommon.W].maze.GetLength (0) == maze.maze.GetLength (0) &&
					maze.neighborMazes [MazeCommon.W].maze.GetLength (1) == maze.maze.GetLength (1)) {
					int height = maze.maze.GetLength (0);
					int width = maze.maze.GetLength (1);
					Console.WriteLine ("has W");
					int x = width - 1;
					for (int y = 0; y < height; y++) {
						if ((maze.neighborMazes [MazeCommon.W].maze [y, x].direction & MazeCommon.E) != 0) {
							maze.maze [y, 0].direction |= MazeCommon.W;
							maze.maze [y, 0].level = maze.neighborMazes [MazeCommon.W].maze [y, x].level;
						}
					}
				}
			}

			// E
			if (maze.neighborMazes.ContainsKey (MazeCommon.E)) {
				if (maze.neighborMazes [MazeCommon.E].maze.GetLength (0) == maze.maze.GetLength (0) &&
					maze.neighborMazes [MazeCommon.E].maze.GetLength (1) == maze.maze.GetLength (1)) {
					int height = maze.maze.GetLength (0);
					int width = maze.maze.GetLength (1);
					Console.WriteLine ("has E");
					int x = 0;
					for (int y = 0; y < height; y++) {
						if ((maze.neighborMazes [MazeCommon.E].maze [y, x].direction & MazeCommon.W) != 0) {
							maze.maze [y, width - 1].direction |= MazeCommon.E;
							maze.maze [y, width - 1].level = maze.neighborMazes [MazeCommon.E].maze [y, x].level;
						}
					}
				}
			}

			return maze;
		}

		/// <summary>
		/// Creates the terrain maze by the given normal maze. terrain maze scale is normal maze 2x2.
		/// </summary>
		/// <returns>The terrain maze.</returns>
		/// <param name="maze">Maze.</param>
		public static Maze CreateTerrainMaze(Maze maze) {
			int height = maze.maze.GetLength (0);
			int width = maze.maze.GetLength (1);
			Cell[,] grid = maze.maze.Clone () as Cell[,];
			//Cell[,] terrainGrid = new Cell[height * 2, width * 2];


			Cell[,] terrainGrid = new Cell[height * 2, width * 2];
			for (int y = 0; y < height; y++) {
				for (int x = 0; x < width; x++) {
					for (int tempY = 0; tempY < 2; tempY++) {
						for (int tempX = 0; tempX < 2; tempX++) {
							terrainGrid [tempY + y * 2, tempX + x * 2].level = grid [y, x].level; 
						}
					}
				}
			}

			for (int y = 0; y < height; y++) {
				for (int x = 0; x < width; x++) {
					#region check every cell in maze.maze, and scale it (as 2X2) to be terrain maze.
					// N
					if ((grid [y, x].direction & MazeCommon.N) != 0) {
						if (y > 0) {
							if ((terrainGrid [-1 + y * 2, 0 + x * 2].direction & MazeCommon.S) != 0)
								terrainGrid [0 + y * 2, 0 + x * 2].direction |= MazeCommon.N; // (0, 0)
							else if ((terrainGrid [-1 + y * 2, 1 + x * 2].direction & MazeCommon.S) != 0)
								terrainGrid [0 + y * 2, 1 + x * 2].direction |= MazeCommon.N; // (0, 1)
							else { // not reach by neighbor;
								int temp = MazeCommon.GetRandom ().Next (0, 2); //get 0 or 1
								terrainGrid [0 + y * 2, temp + x * 2].direction |= MazeCommon.N;
							} 
						} else if (maze.neighborMazes.ContainsKey (MazeCommon.N) && //else y == 0
							maze.neighborMazes [MazeCommon.N].terrainMaze.GetLength (0) == height * 2 &&
							maze.neighborMazes [MazeCommon.N].terrainMaze.GetLength (1) == width * 2) {
							if ((maze.neighborMazes [MazeCommon.N].terrainMaze [height * 2 - 1, 0 + x * 2].direction & MazeCommon.S) != 0) {
								terrainGrid [y * 2, 0 + x * 2].direction |= MazeCommon.N;
							} else if ((maze.neighborMazes [MazeCommon.N].terrainMaze [height * 2 - 1, 1 + x * 2].direction & MazeCommon.S) != 0) {
								terrainGrid [y * 2, 1 + x * 2].direction |= MazeCommon.N;
							}

						} else {
							int temp = MazeCommon.GetRandom ().Next (0, 2); //get 0 or 1
							terrainGrid [y * 2, temp + x * 2].direction |= MazeCommon.N;
						}
					}
					// S
					if ((grid [y, x].direction & MazeCommon.S) != 0) {
						if (y < height - 1) {
							if ((terrainGrid [2 + y * 2, 0 + x * 2].direction & MazeCommon.N) != 0)
								terrainGrid [1 + y * 2, 0 + x * 2].direction |= MazeCommon.S; // (1, 0)
							else if ((terrainGrid [2 + y * 2, 1 + x * 2].direction & MazeCommon.N) != 0)
								terrainGrid [1 + y * 2, 1 + x * 2].direction |= MazeCommon.S; // (1, 1)
							else { // not reach by neighbor;
								int temp = MazeCommon.GetRandom ().Next (0, 2); //get 0 or 1
								terrainGrid [1 + y * 2, temp + x * 2].direction |= MazeCommon.S;
							}
						} else if (maze.neighborMazes.ContainsKey (MazeCommon.S) &&
							maze.neighborMazes [MazeCommon.S].terrainMaze.GetLength (0) == height * 2 &&
							maze.neighborMazes [MazeCommon.S].terrainMaze.GetLength (1) == width * 2) {
							if ((maze.neighborMazes [MazeCommon.S].terrainMaze [0, 0 + x * 2].direction & MazeCommon.N) != 0) {
								terrainGrid [1 + y * 2, 0 + x * 2].direction |= MazeCommon.S;
							} else if ((maze.neighborMazes [MazeCommon.S].terrainMaze [0, 1 + x * 2].direction & MazeCommon.N) != 0) {
								terrainGrid [1 + y * 2, 1 + x * 2].direction |= MazeCommon.S;
							}

						} else {
							int temp = MazeCommon.GetRandom ().Next (0, 2); //get 0 or 1
							terrainGrid [1 + y * 2, temp + x * 2].direction |= MazeCommon.S;
						}
					}
					// W
					if ((grid [y, x].direction & MazeCommon.W) != 0) {
						if (x > 0) {
							if ((terrainGrid [0 + y * 2, -1 + x * 2].direction & MazeCommon.E) != 0)
								terrainGrid [0 + y * 2, 0 + x * 2].direction |= MazeCommon.W; // (0, 0)
							else if ((terrainGrid [1 + y * 2, -1 + x * 2].direction & MazeCommon.E) != 0)
								terrainGrid [1 + y * 2, 0 + x * 2].direction |= MazeCommon.W; // (1, 0)
							else { // not reach by neighbor;
								int temp = MazeCommon.GetRandom ().Next (0, 2); //get 0 or 1
								terrainGrid [temp + y * 2, 0 + x * 2].direction |= MazeCommon.W; // (temp, 0)
							}
						} else if (maze.neighborMazes.ContainsKey (MazeCommon.W) && // x == 0
							maze.neighborMazes [MazeCommon.W].terrainMaze.GetLength (0) == height * 2 &&
							maze.neighborMazes [MazeCommon.W].terrainMaze.GetLength (1) == width * 2) {
							if ((maze.neighborMazes [MazeCommon.W].terrainMaze [0 + y * 2, width * 2 - 1].direction & MazeCommon.E) != 0) {
								terrainGrid [0 + y * 2, x * 2].direction |= MazeCommon.W;
							} else if ((maze.neighborMazes [MazeCommon.W].terrainMaze [1 + y * 2, width * 2 - 1].direction & MazeCommon.E) != 0) {
								terrainGrid [1 + y * 2, x * 2].direction |= MazeCommon.W;
							}

						} else {
							int temp = MazeCommon.GetRandom ().Next (0, 2); //get 0 or 1
							terrainGrid [temp + y * 2, x * 2].direction |= MazeCommon.W;
						}
					}
					// E
					if ((grid [y, x].direction & MazeCommon.E) != 0) {
						if (x < width - 1) {
							if ((terrainGrid [0 + y * 2, 2 + x * 2].direction & MazeCommon.W) != 0)
								terrainGrid [0 + y * 2, 1 + x * 2].direction |= MazeCommon.E; // (0, 1)
							else if ((terrainGrid [1 + y * 2, 2 + x * 2].direction & MazeCommon.W) != 0)
								terrainGrid [1 + y * 2, 1 + x * 2].direction |= MazeCommon.E; // (1, 1)
							else { // not reach by neighbor;
								int temp = MazeCommon.GetRandom ().Next (0, 2); //get 0 or 1
								terrainGrid [temp + y * 2, 1 + x * 2].direction |= MazeCommon.E; // (temp, 1)
							} 
						} else if (maze.neighborMazes.ContainsKey (MazeCommon.E) && // x == width - 1
							maze.neighborMazes [MazeCommon.E].terrainMaze.GetLength (0) == height * 2 &&
							maze.neighborMazes [MazeCommon.E].terrainMaze.GetLength (1) == width * 2) {
							if ((maze.neighborMazes [MazeCommon.E].terrainMaze [0 + y * 2, 0].direction & MazeCommon.W) != 0) {
								terrainGrid [0 + y * 2, 1 + x * 2].direction |= MazeCommon.E;
							}
							else if ((maze.neighborMazes [MazeCommon.E].terrainMaze [1 + y * 2, 0].direction & MazeCommon.W) != 0) {
								terrainGrid [1 + y * 2, 1 + x * 2].direction |= MazeCommon.E;
							}

						} else {
							int temp = MazeCommon.GetRandom ().Next (0, 2); //get 0 or 1
							terrainGrid [temp + y * 2, 1 + x * 2].direction |= MazeCommon.E;
						}
					}
					#endregion

					#region make the terrain maze all pass
					// make the 2x2 area all pass;
					// (0, 0) & (1, 1) pass and (0, 1) & (1, 0) block
					if (terrainGrid [0 + y * 2, 0 + x * 2].direction != 0 && terrainGrid [1 + y * 2, 1 + x * 2].direction != 0
						&& terrainGrid [0 + y * 2, 1 + x * 2].direction == 0 && terrainGrid [1 + y * 2, 0 + x * 2].direction == 0) {
						int temp = MazeCommon.GetRandom ().Next (0, 2); //get 0 or 1
						if (temp == 0) {
							terrainGrid [0 + y * 2, 1 + x * 2].direction |= MazeCommon.W;
							terrainGrid [0 + y * 2, 1 + x * 2].direction |= MazeCommon.S;
						} else {
							terrainGrid [1 + y * 2, 0 + x * 2].direction |= MazeCommon.N;
							terrainGrid [1 + y * 2, 0 + x * 2].direction |= MazeCommon.E;
						}
					} // (0, 0) & (1, 1) block and (0, 1) & (1, 0) pass
					else if (terrainGrid [0 + y * 2, 0 + x * 2].direction == 0 && terrainGrid [1 + y * 2, 1 + x * 2].direction == 0
						&& terrainGrid [0 + y * 2, 1 + x * 2].direction != 0 && terrainGrid [1 + y * 2, 0 + x * 2].direction != 0) {
						int temp = MazeCommon.GetRandom ().Next (0, 2); //get 0 or 1
						if (temp == 0) {
							terrainGrid [0 + y * 2, 0 + x * 2].direction |= MazeCommon.E;
							terrainGrid [0 + y * 2, 0 + x * 2].direction |= MazeCommon.S;
						} else {
							terrainGrid [1 + y * 2, 1 + x * 2].direction |= MazeCommon.N;
							terrainGrid [1 + y * 2, 1 + x * 2].direction |= MazeCommon.W;
						}
					}

					// (0, 0) <--> (0, 1) E, W
					if (terrainGrid [0 + y * 2, 0 + x * 2].direction != 0 && terrainGrid [0 + y * 2, 1 + x * 2].direction != 0) {
						terrainGrid [0 + y * 2, 0 + x * 2].direction |= MazeCommon.E;
						terrainGrid [0 + y * 2, 1 + x * 2].direction |= MazeCommon.W;
					}
					// (0, 0) <--> (1, 0) 
					if (terrainGrid [0 + y * 2, 0 + x * 2].direction != 0 && terrainGrid [1 + y * 2, 0 + x * 2].direction != 0) {
						terrainGrid [0 + y * 2, 0 + x * 2].direction |= MazeCommon.S;
						terrainGrid [1 + y * 2, 0 + x * 2].direction |= MazeCommon.N;
					}
					// (1, 1) <--> (1, 0) W E
					if (terrainGrid [1 + y * 2, 1 + x * 2].direction != 0 && terrainGrid [1 + y * 2, 0 + x * 2].direction != 0) {
						terrainGrid [1 + y * 2, 1 + x * 2].direction |= MazeCommon.W;
						terrainGrid [1 + y * 2, 0 + x * 2].direction |= MazeCommon.E;
					}
					// (1, 1) <--> (0, 1) N S
					if (terrainGrid [1 + y * 2, 1 + x * 2].direction != 0 && terrainGrid [0 + y * 2, 1 + x * 2].direction != 0) {
						terrainGrid [1 + y * 2, 1 + x * 2].direction |= MazeCommon.N;
						terrainGrid [0 + y * 2, 1 + x * 2].direction |= MazeCommon.S;
					}
					#endregion

				}
			}

			maze.terrainMaze = terrainGrid;
			return maze;
		}

		public static Maze TerrainMazeInsertSpaces(Maze maze, int split) {
			Cell[,] grid = maze.terrainMaze.Clone () as Cell[,];

			int height = grid.GetLength (0);
			int width = grid.GetLength (1);
			if (height < split || width < split || split <= 4) // min split is 5
				return maze;

			int lenX = width / split;
			int lenY = height / split;
			Space[,] spaces = new Space[lenY, lenX];
			Console.WriteLine ("lenx * leny is " + (lenX * lenY));

			for (int i = 0; i < lenY; i++) {
				for (int j = 0; j < lenX; j++) {

					int tempWidth = MazeCommon.GetRandom ().Next (2, 4); // get 2 or 3
					int tempX = MazeCommon.GetRandom ().Next (1, split - 1); // get 1, 2, 3 ... split-1
					while (tempX + tempWidth > split - 1) {
						tempWidth = MazeCommon.GetRandom ().Next (2, 4);
						tempX = MazeCommon.GetRandom ().Next (1, split - 1);
					}

					int tempHeight = MazeCommon.GetRandom ().Next (2, 4); // get 2 or 3
					int tempY = MazeCommon.GetRandom ().Next (1, split - 1);
					while (tempY + tempHeight > split - 1) {
						tempHeight = MazeCommon.GetRandom ().Next (2, 4);
						tempY = MazeCommon.GetRandom ().Next (1, split - 1);
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

							grid [tempY + spaces [i, j].anchor.y + split * i, tempX + spaces [i, j].anchor.x + split * j].direction |= (MazeCommon.N + MazeCommon.S + MazeCommon.W + MazeCommon.E);

							if (tempY == 0 && 
								(grid [tempY + spaces [i, j].anchor.y + split * i - 1, tempX + spaces [i, j].anchor.x + split * j].direction & MazeCommon.S) == 0) {
								grid [tempY + spaces [i, j].anchor.y + split * i, tempX + spaces [i, j].anchor.x + split * j].direction -= MazeCommon.N;
							}
							else if (tempY == (spaces [i, j].height - 1) &&
								(grid [tempY + spaces [i, j].anchor.y + split * i + 1, tempX + spaces [i, j].anchor.x + split * j].direction & MazeCommon.N) == 0){
								grid [tempY + spaces [i, j].anchor.y + split * i, tempX + spaces [i, j].anchor.x + split * j].direction -= MazeCommon.S;
							}

							if (tempX == 0 &&
								(grid [tempY + spaces [i, j].anchor.y + split * i, tempX + spaces [i, j].anchor.x + split * j - 1].direction & MazeCommon.E) == 0) {
								grid [tempY + spaces [i, j].anchor.y + split * i, tempX + spaces [i, j].anchor.x + split * j].direction -= MazeCommon.W;
							}  else if  (tempX == (spaces [i, j].width - 1) &&
								(grid [tempY + spaces [i, j].anchor.y + split * i, tempX + spaces [i, j].anchor.x + split * j + 1].direction & MazeCommon.W) == 0) {
								grid [tempY + spaces [i, j].anchor.y + split * i, tempX + spaces [i, j].anchor.x + split * j].direction -= MazeCommon.E;
							}

						}
					}
					spaces [i, j].anchor.y = spaces [i, j].anchor.y + split * i;
					spaces [i, j].anchor.x = spaces [i, j].anchor.x + split * j;
				}
			}

			maze.terrainMaze = grid;
			maze.spaces = spaces;
			return maze;
		}

		public static Maze AdjustMazeLevel (Maze maze){
			int height = maze.terrainMaze.GetLength (0);
			int width = maze.terrainMaze.GetLength (1);
			Cell[,] terrainGrid = maze.terrainMaze.Clone () as Cell[,];

			Maze newMaze = AdjustMazeSpaces (maze);
			Space[,] spacesClone = newMaze.spaces.Clone () as Space[,];
			int[,] checkSpaceArea = new int[height, width]; // 1 is space area, -1 is path, 0 is road;
			int lenY = spacesClone.GetLength (0);
			int lenX = spacesClone.GetLength (1);
			for (int numY = 0; numY < lenY; numY++) {
				for (int numX = 0; numX < lenX; numX++) {
					Space tempSpace = spacesClone [numY, numX];
					for (int h = 0; h < tempSpace.height; h++) {
						for (int w = 0; h < tempSpace.width; w++) {
							checkSpaceArea [tempSpace.anchor.y + h, tempSpace.anchor.x + w] = 1;
						}
					}
					foreach (Point s in tempSpace.addOn) {
						checkSpaceArea [s.y, s.x] = 1;
					}
					foreach (Point s in tempSpace.paths) {
						checkSpaceArea [s.y, s.x] = -1;
					}
				}
			} // init the check space area array;

			List<CellWithPoint> levelList = new List<CellWithPoint> ();
			for (int h = 0; h < height - 1; h++) {
				if ((terrainGrid [h, 0].direction & MazeCommon.W) != 0) {
					levelList.Add (new CellWithPoint(new Point(0, h), terrainGrid [h, 0]));
				}
				if ((terrainGrid [h, width - 1].direction & MazeCommon.E) != 0) {
					levelList.Add (new CellWithPoint(new Point(width - 1, h), terrainGrid [h, width - 1]));
				}
			}
			for (int w = 0; w < width - 1; w++) {
				if ((terrainGrid [0, w].direction & MazeCommon.N) != 0 && !levelList.Contains(new CellWithPoint(new Point(w, 0), terrainGrid [0, w]))) {
					levelList.Add (new CellWithPoint(new Point(w, 0), terrainGrid [0, w]));
				}
				if ((terrainGrid [height - 1, w].direction & MazeCommon.S) != 0 && !levelList.Contains(new CellWithPoint(new Point(w, height - 1), terrainGrid [height -1, w]))) {
					levelList.Add (new CellWithPoint(new Point(w, height - 1), terrainGrid [height -1, w]));
				}
			}

			while (levelList.Count > 0) {
				int index = MazeCommon.GetRandom().Next(0, levelList.Count);
				CellWithPoint tempCell = levelList [index];
				for (int i = 0; i < 4; i++) {
					int dr = MazeCommon.DR [i];
					int ny = tempCell.point.y + MazeCommon.DY [dr]; int nx = tempCell.point.x + MazeCommon.DX [dr];
					if (ny >= 0 && ny < height && nx >= 0 && nx < width && terrainGrid[ny, nx].visited == false) {
						if ((tempCell.cell.direction & dr) != 0 &&
							(terrainGrid[ny, nx].direction & MazeCommon.OPPOSITE[dr]) != 0){
							//checkSpaceArea [ny, nx];
						}
					}
				}



				levelList.RemoveAt (index);
			}
			//List<Cell> 
			return newMaze;
		}

		struct CellWithPoint {
			public Point point;
			public Cell cell;

			public CellWithPoint (Point p, Cell c) {
				this.cell = c;
				this.point = p;
			}
		}

		public enum CellType {
			UNDEFINE = -1,
			SPACE = 1,
			DOOR = 2,
			PATH = 4,
			BLOCK = 8,
		}

		#region init the CellTypeGrid
		public static CellType[,] GetCellTypeGrid4Level(Maze maze) {
			Cell[,] terrainGrid = maze.terrainMaze.Clone () as Cell[,];
			int height = terrainGrid.GetLength (0);
			int width = terrainGrid.GetLength (1);
			CellType[,] result = new CellType[height, width];

			// init cell type grid
			result = InitCellTypeGrid(terrainGrid, result);

			// find all the spaces
			//check from space cell
			result = InitSpaceToCellTypeGrid (terrainGrid, result, maze.spaces);

			// set crossroad as door to cell type grid
			result = SetCrossroadToCellTypeGrid(terrainGrid, result);

			// set path in cell type grid
			result = SetPathToCellTypeGrid(terrainGrid, result);

			return result;
		}

		static CellType[,] InitCellTypeGrid (Cell[,] terrainGrid, CellType[,] cellTypeGird) {
			int height = terrainGrid.GetLength (0);
			int width = terrainGrid.GetLength (1);
			CellType[,] result = cellTypeGird.Clone () as CellType[,];

			// init cell type grid
			for (int h = 0; h < height; h++) {
				for (int w = 0; w < width; w++) {
					result [h, w] = CellType.UNDEFINE;
					if (terrainGrid [h, w].direction == 0)
						result [h, w] = CellType.BLOCK;
					else if ((h == 0 && (terrainGrid [h, w].direction & MazeCommon.N) != 0) ||
						(h == height - 1 && (terrainGrid [h, w].direction & MazeCommon.S) != 0) || 
						(w == 0 && (terrainGrid [h, w].direction & MazeCommon.W) != 0) ||
						(w == width - 1 && (terrainGrid [h, w].direction & MazeCommon.E) != 0)) 
					{
						/*
						 * int connectNum = 0;
						for (int k = 0; k < 4; k++) {
							if ((terrainGrid [h, w].direction & MazeCommon.DR [k]) != 0)
								connectNum++;
						}
						// not as door.
						if (connectNum != 2) {
							result [h, w] = CellType.SPACE;
						} else {
							result [h, w] = CellType.DOOR;
						}
						*/
						result [h, w] = CellType.DOOR;
					}
				}
			}
			return result;
		}

		static CellType[,] InitSpaceToCellTypeGrid (Cell[,] terrainGrid, CellType[,] cellTypeGird, Space[,] spacesClone) {
			int height = terrainGrid.GetLength (0);
			int width = terrainGrid.GetLength (1);
			CellType[,] result = cellTypeGird.Clone () as CellType[,];

			int lenY = spacesClone.GetLength (0);
			int lenX = spacesClone.GetLength (1);
			Console.WriteLine (lenY + "!!!" + lenX);
			for (int numY = 0; numY < lenY; numY++) {
				for (int numX = 0; numX < lenX; numX++) {
					Space tempSpace = spacesClone [numY, numX];
					Point spacePoint = spacesClone [numY, numX].anchor;
					for (int tempY = 0; tempY < tempSpace.height; tempY++) {
						for (int tempX = 0; tempX < tempSpace.width; tempX++) {
							result [spacePoint.y + tempY, spacePoint.x + tempX] = CellType.SPACE;
						}
					}
				}
			}

			//check from space cell
			for (int y = 0; y < height; y++) {
				for (int x = 0; x < width; x++) {
					if (result [y, x] != CellType.SPACE)
						continue;

					int connectToSpaceNum = 0;
					int connectNum = 0;
					for (int i = 0; i < 4; i++) {
						int dr = MazeCommon.DR [i];
						int ny = y + MazeCommon.DY [dr];
						int nx = x + MazeCommon.DX [dr];
						connectToSpaceNum = 0;
						connectNum = 0;
						if (ny >= 0 && ny < height && nx >= 0 && nx < width &&
							result [ny, nx] == CellType.UNDEFINE &&
							((terrainGrid [y, x].direction & dr) != 0) &&
							((terrainGrid [ny, nx].direction & MazeCommon.OPPOSITE [dr]) != 0)) {
							// check from terrainGrid [ny, nx]
							for (int k = 0; k < 4; k++) {
								int k_dr = MazeCommon.DR [k];
								int k_ny = ny + MazeCommon.DY [k_dr];
								int k_nx = nx + MazeCommon.DX [k_dr];
								if (k_ny >= 0 && k_ny < height && k_nx >= 0 && k_nx < width &&
									((terrainGrid [ny, nx].direction & k_dr) != 0) &&
									((terrainGrid [k_ny, k_nx].direction & MazeCommon.OPPOSITE [k_dr]) != 0)) {
									connectNum++;
									if (result [k_ny, k_nx] == CellType.SPACE)
										connectToSpaceNum++;
								}
							}// connectNum >= 1 and connectToSpaceNum >= 1
							if (connectToSpaceNum == 1 && connectNum == 2)
								result [ny, nx] = CellType.DOOR;
							else {
								result [ny, nx] = CellType.SPACE;
								x = 0;
								y = 0;
								break;
							}

						}
					}

				}
			}
			return result;
		}

		/// <summary>
		/// Sets the crossroad as Door to cell type grid.
		/// </summary>
		/// <returns>The crossroad to cell type grid.</returns>
		/// <param name="terrainGrid">Terrain grid.</param>
		/// <param name="cellTypeGird">Cell type gird.</param>
		static CellType[,] SetCrossroadToCellTypeGrid(Cell[,] terrainGrid, CellType[,] cellTypeGird){
			int height = terrainGrid.GetLength (0);
			int width = terrainGrid.GetLength (1);
			CellType[,] result = cellTypeGird.Clone () as CellType[,];

			// set the crossroad to be door
			for (int y = 0; y < height; y++) {
				for (int x = 0; x < width; x++) {
					if (result [y, x] != CellType.UNDEFINE)
						continue;
					int connectNum = 0;
					for (int k = 0; k < 4; k++) {
						if ((terrainGrid [y, x].direction & MazeCommon.DR [k]) != 0)
							connectNum++;
					}

					if (connectNum > 2)
						result [y, x] = CellType.DOOR;

				}
			}
			return result;
		}

		static CellType[,] SetPathToCellTypeGrid(Cell[,] terrainGrid, CellType[,] cellTypeGird){
			int height = terrainGrid.GetLength (0);
			int width = terrainGrid.GetLength (1);
			CellType[,] result = cellTypeGird.Clone () as CellType[,];

			for (int y = 0; y < height; y++) {
				for (int x = 0; x < width; x++) {
					if (result [y, x] != CellType.UNDEFINE)
						continue;

					int connectNum = 0;
					for (int k = 0; k < 4; k++) {
						if ((terrainGrid [y, x].direction & MazeCommon.DR [k]) != 0)
							connectNum++;
					}

					if (connectNum == 1) // end point can be set to be door
						result [y, x] = CellType.DOOR;
					else if (result [y, x] == CellType.UNDEFINE)
						result [y, x] = CellType.PATH;
				}
			}
			return result;
		}
		#endregion

		#region deal with the CellTypeGrid
		public static Dictionary<Point, Door> GetRoadByCellTypeGrid(Cell[,] terrainGrid, CellType[,] cellTypeGird, Space[,] spacesClone) {
			Dictionary<Point, Door> doorDict = new Dictionary<Point, Door> ();
			int height = cellTypeGird.GetLength (0);
			int width = cellTypeGird.GetLength (1);
			int[,] checkArray = new int[height, width];

			bool[,] canStairArray = CheckIfCanSetAStair (terrainGrid, cellTypeGird);

			#region Get direct link doors bt spaces
			int lenY = spacesClone.GetLength (0);
			int lenX = spacesClone.GetLength (1);
			for (int numY = 0; numY < lenY; numY++) {
				for (int numX = 0; numX < lenX; numX++) {
					List<Point> tempDoors = new List<Point> ();
					//Space tempSpace = spacesClone [numY, numX];
					Point spacePoint = spacesClone [numY, numX].anchor;
					List<Point> tempList = new List<Point> ();
					tempList.Add (spacePoint);

					while (tempList.Count > 0) {
						int index = tempList.Count;
						Point tempPoint = tempList [index - 1];
						for (int i = 0; i < 4; i++) {
							int dr = MazeCommon.DR [i];
							int ny = tempPoint.y + MazeCommon.DY [dr];
							int nx = tempPoint.x + MazeCommon.DX [dr];
							if (nx >= 0 && nx < width && ny >= 0 && ny < height && checkArray[ny, nx] == 0) {
								checkArray [ny, nx] = 1;
								if (cellTypeGird [ny, nx] == CellType.DOOR) {
									tempDoors.Add (new Point (nx, ny));
								} else if (cellTypeGird[ny, nx] == CellType.SPACE) {
									tempList.Add (new Point (nx, ny));
								}
							}
						}
						tempList.RemoveAt(index - 1);
					}


					for (int i = 0; i < tempDoors.Count; i++) {
						doorDict.Add (tempDoors [i], new Door (tempDoors [i]));
					}
					for (int i = 0; i < tempDoors.Count; i++) {
						//doorDict.Add (tempList [i], new Door(tempList [i]));
						//Point nextDoorcoor = tempList [i];
						for (int k = 0; k < tempDoors.Count; k++) {
							if (tempDoors[i].x == tempDoors[k].x && tempDoors[i].y == tempDoors[k].y)
								continue;
							DoorAdjvex da = new DoorAdjvex (tempDoors [k]);
							da.isSpace = true;
							doorDict [tempDoors [i]].adjvex.Add(da);
						}
					}
				}
			}
			#endregion

			for (int y = 0; y < height; y++) {
				for (int x = 0; x < width; x++) {
					if (cellTypeGird [y, x] == CellType.DOOR) {
						// get neighbor doors
						Point nowPoint = new Point (x, y);
						if (!doorDict.ContainsKey (nowPoint)) {
							doorDict.Add (nowPoint, new Door (nowPoint));
						}

						for (int i = 0; i < 4; i++) {
							int dr = MazeCommon.DR [i];
							int ny = y + MazeCommon.DY [dr];
							int nx = x + MazeCommon.DX [dr];
							if (nx >= 0 && nx < width && ny >= 0 && ny < height &&
								((terrainGrid[y, x].direction & dr) != 0) && 
								((terrainGrid[ny, nx].direction & MazeCommon.OPPOSITE[dr]) != 0)) {
								if (cellTypeGird [ny, nx] == CellType.DOOR) {
									DoorAdjvex da = new DoorAdjvex (nowPoint);
									doorDict [nowPoint].adjvex.Add (da);
								} else if (cellTypeGird [ny, nx] == CellType.PATH) {
									DoorAdjvex da = new DoorAdjvex ();
									da = CheckPath (new Point (nx, ny), nowPoint, terrainGrid, cellTypeGird, da);
									doorDict[nowPoint].adjvex.Add(da);
								}
							}
						}

						for (int i = 0; i < doorDict [nowPoint].adjvex.Count; i++) {
							int canStairCount = 0;
							//if (canStairArray [nowPoint.y, nowPoint.x])
								//canStairCount++;
							if (canStairArray [doorDict [nowPoint].adjvex[i].nextDoor.y, doorDict [nowPoint].adjvex[i].nextDoor.x])
								canStairCount++;
							if (!doorDict [nowPoint].adjvex [i].isSpace) {
								for (int j = 0; j < doorDict [nowPoint].adjvex [i].road.Count; j++) {
									if (canStairArray [doorDict [nowPoint].adjvex [i].road [j].y, doorDict [nowPoint].adjvex [i].road [j].x]) {
										canStairCount++;
									}
								}
							}
							var tempDoor = doorDict [nowPoint];
							var tempAdjvex = tempDoor.adjvex [i];
							tempAdjvex.canStairCount = canStairCount;
							tempDoor.adjvex [i] = tempAdjvex;
							doorDict [nowPoint] = tempDoor;
						}
					}
				}
			}

			return doorDict;
		}

		static DoorAdjvex CheckPath(Point point, Point lastPoint, Cell[,] terrainGrid, CellType[,] cellTypeGird, DoorAdjvex doorAdjvex ) {
			int y = point.y;int x = point.x;
			int height = cellTypeGird.GetLength (0);
			int width = cellTypeGird.GetLength (1);

			for (int i = 0; i < 4; i++) {
				int dr = MazeCommon.DR [i];
				int ny = y + MazeCommon.DY [dr];
				int nx = x + MazeCommon.DX [dr];
				if (ny == lastPoint.y && nx == lastPoint.x)
					continue;

				if (nx >= 0 && nx < width && ny >= 0 && ny < height &&
					((terrainGrid[y, x].direction & dr) != 0) &&
					((terrainGrid[ny, nx].direction & MazeCommon.OPPOSITE[dr]) != 0)) {
					if (cellTypeGird [ny, nx] != CellType.DOOR && cellTypeGird [ny, nx] != CellType.PATH)
						continue;

					if (cellTypeGird [ny, nx] == CellType.DOOR) {
						doorAdjvex.nextDoor = new Point (nx, ny);
						return doorAdjvex;
					} else if (cellTypeGird [ny, nx] == CellType.PATH) {
						doorAdjvex.road.Add (new Point (nx, ny));
						doorAdjvex = CheckPath (new Point(nx, ny), point, terrainGrid, cellTypeGird, doorAdjvex);
					} else {
						//throw((new System.Exception("error")).Data);
						Console.WriteLine("error");
					}
				}
			}
			return doorAdjvex;
		}

		struct PointWithType {
			public Point point;
			public CellType type;

			public PointWithType(Point point, CellType type) {
				this.point = point;
				this.type = type;
			}
		}

		static bool[,] CheckIfCanSetAStair(Cell[,] terrainGrid, CellType[,] cellTypeGird) {
			int height = cellTypeGird.GetLength (0);
			int width = cellTypeGird.GetLength (1);
			bool[,] result = new bool[height, width];

			for (int y = 0; y < height; y++) {
				for (int x = 0; x < width; x++) {
					result [y, x] = false;
					if (cellTypeGird [y, x] == CellType.DOOR || cellTypeGird [y, x] == CellType.PATH) {
						if ((terrainGrid [y, x].direction & MazeCommon.N) != 0 &&
						    (terrainGrid [y, x].direction & MazeCommon.S) != 0 &&
						    (terrainGrid [y, x].direction & MazeCommon.W) == 0 &&
						    (terrainGrid [y, x].direction & MazeCommon.E) == 0) {
							result [y, x] = true;
						} else if ((terrainGrid [y, x].direction & MazeCommon.N) == 0 &&
							(terrainGrid [y, x].direction & MazeCommon.S) == 0 &&
							(terrainGrid [y, x].direction & MazeCommon.W) != 0 &&
							(terrainGrid [y, x].direction & MazeCommon.E) != 0) {
							result [y, x] = true;
						}
					}
				}
			}
			return result;
		}

		#endregion

		#region CreateMazeWithLevel 
		public static Maze CreateMazeWithLevel(Maze maze) {
			Cell[,] terrainGrid = maze.terrainMaze.Clone () as Cell[,];
			int height = terrainGrid.GetLength (0);
			int width = terrainGrid.GetLength (1);
			CellType[,] cellTypeGird = GetCellTypeGrid4Level (maze);
			bool[,] canStairArray = CheckIfCanSetAStair (terrainGrid, cellTypeGird);
			Dictionary<Point, Door> doorDict = GetRoadByCellTypeGrid (terrainGrid, cellTypeGird, maze.spaces);
			// create level data and store in terrainGird.
			#region Create Doors Level first
			Dictionary<Point, PointWithLevel> pointLevelDict = new Dictionary<Point, PointWithLevel> ();
			for (int y = 0; y < height; y++) {
				for (int x = 0; x < width; x++) {
					if (((y == 0 && (terrainGrid [y, x].direction & MazeCommon.N) != 0) ||
					    (y == height - 1 && (terrainGrid [y, x].direction & MazeCommon.S) != 0) ||
					    (x == 0 && (terrainGrid [y, x].direction & MazeCommon.W) != 0) ||
					    (x == width - 1 && (terrainGrid [y, x].direction & MazeCommon.E) != 0)) &&
						cellTypeGird[y, x] == CellType.DOOR){
						Point nowPoint = new Point (x, y);
						pointLevelDict.Add (nowPoint, new PointWithLevel (nowPoint, terrainGrid [y, x].level, terrainGrid [y, x].level));
						PointWithLevel tempPointWithLevel = pointLevelDict [nowPoint];
						tempPointWithLevel.isCheck = true;
						pointLevelDict [nowPoint] = tempPointWithLevel;
					}
				}
			}

			List<PointWithLevel> doorList = new List<PointWithLevel> ();
			foreach (var kvp in pointLevelDict) {
				doorList.Add (kvp.Value);
			}

			while (doorList.Count > 0) {
				int count = doorList.Count;
				int index = MazeCommon.GetRandom ().Next (0, count);
				PointWithLevel nowPointWithLevel = doorList [index];
				for (int i = 0; i < doorDict [nowPointWithLevel.point].adjvex.Count; i++) {
					int canStairCount = doorDict [nowPointWithLevel.point].adjvex [i].canStairCount;
					int minLevel = nowPointWithLevel.minLevel - canStairCount;
					int maxLevel = nowPointWithLevel.maxLevel + canStairCount;
					if (!pointLevelDict.ContainsKey (doorDict [nowPointWithLevel.point].adjvex[i].nextDoor)) {
						PointWithLevel pwl = new PointWithLevel (doorDict [nowPointWithLevel.point].adjvex[i].nextDoor, minLevel, maxLevel);
						pwl.isCheck = true;
						pointLevelDict.Add (pwl.point, pwl);
						doorList.Add (pwl);
					}
					else {
						int tempMin = Math.Max (minLevel, pointLevelDict [nowPointWithLevel.point].minLevel);
						int tempMax = Math.Min (maxLevel, pointLevelDict [nowPointWithLevel.point].maxLevel);
						if (tempMin > tempMax)
							throw new Exception ("get wrong in level.. you need to recreate the maze again");
						PointWithLevel tempPointWithLevel = pointLevelDict [doorDict [nowPointWithLevel.point].adjvex[i].nextDoor];
						tempPointWithLevel.minLevel = tempMin;
						tempPointWithLevel.maxLevel = tempMax;
						pointLevelDict [nowPointWithLevel.point] = tempPointWithLevel;
					}
				}
				doorList.RemoveAt (index);
			}

			foreach (var kvp in pointLevelDict) {
				if (kvp.Value.minLevel == kvp.Value.maxLevel)
					terrainGrid [kvp.Value.point.y, kvp.Value.point.x].level = kvp.Value.maxLevel;
				else {
					#region It has something wrong here...
					//the terrainGrid [kvp.Value.point.y, kvp.Value.point.x].level should be has the distance of 1 to its neighbor;
					terrainGrid [kvp.Value.point.y, kvp.Value.point.x].level = MazeCommon.GetRandom ().Next (kvp.Value.minLevel, kvp.Value.maxLevel + 1);
					#endregion
				}
			}
			#endregion

			#region Connect the doors level by door's road
			List<Road> roadsList = new List<Road>();
			foreach (var kvp in doorDict) {
				for (int i = 0; i < kvp.Value.adjvex.Count; i++) {
					Point point1 = kvp.Value.coordinate;
					Point point2 = kvp.Value.adjvex[i].nextDoor;
					bool exist = CheckIfRoadExistInList(roadsList, point1, point2);
					if (!exist) {
						// deal with the road
						roadsList.Add(new Road(point1, point2, kvp.Value.adjvex[i].road));
						int minLevel;
						int maxLevel;
						if (terrainGrid[point1.y, point1.x].level > terrainGrid[point2.y, point2.x].level) {
							minLevel = terrainGrid[point2.y, point2.x].level;
							maxLevel = terrainGrid[point1.y, point1.x].level;
						} else {
							minLevel = terrainGrid[point1.y, point1.x].level;
							maxLevel = terrainGrid[point2.y, point2.x].level;
						}
						int stairCount = maxLevel - minLevel;
						if (kvp.Value.adjvex[i].canStairCount < stairCount)
							throw new Exception("error when assign road level, you need to recreate your maze");
						List<Point> canStairPointList = new List<Point>();
						for (int k = 0; k < kvp.Value.adjvex[i].road.Count; k++) {
							Point pathPoint = kvp.Value.adjvex[i].road[k];
							if (canStairArray[pathPoint.y, pathPoint.x]) {
								canStairPointList.Add (pathPoint);
							}
						}
						for (int k = 0; k < stairCount; k++) {
							int index = MazeCommon.GetRandom().Next(0, canStairPointList.Count);
							terrainGrid[canStairPointList[k].y, canStairPointList[k].x].level = minLevel + k + 1;
						}
					}
				}
			}
			#endregion

			maze.terrainMaze = terrainGrid;
			return maze;
		}

		static bool CheckIfRoadExistInList(List<Road> roadsList, Point Point1, Point Point2) {
			bool exist = false;
			for (int i = 0; i < roadsList.Count; i++) {
				if ((roadsList [i].startPoint.x == Point1.x && roadsList [i].startPoint.y == Point1.y &&
				    roadsList [i].endPoint.x == Point2.x && roadsList [i].endPoint.y == Point2.y) ||
				    (roadsList [i].startPoint.x == Point2.x && roadsList [i].startPoint.y == Point2.y &&
				    roadsList [i].endPoint.x == Point1.x && roadsList [i].endPoint.y == Point1.y)) {
					exist = true;
					return exist;
				}
			}
			return exist;
		}

		struct Road {
			public Point startPoint;
			public Point endPoint;
			public List<Point> road;

			public Road (Point start, Point end, List<Point> road) {
				this.startPoint = start;
				this.endPoint = end;
				this.road = road;
			}

		}

		struct PointWithLevel {
			public Point point;
			private int _maxLevel;
			public int maxLevel { get{ return _maxLevel;} set { if (value > 5)
						_maxLevel = 5; } }
			
			private int _minLevel;
			public int minLevel { get{ return _minLevel;} set { if (value < -5)
						_minLevel = -5;}}
			public bool isCheck;

			public PointWithLevel (Point point, int min, int max) {
				this.point = point;
				this._minLevel = min;
				this._maxLevel = max;
				this.isCheck = false;
			}


		}
		#endregion

		#region delete
		/// <summary>
		/// Gets the terrain grid for level.
		/// 1 is space, 2 is door, 4 is path, 8 is block.
		/// between 2 doors, you can get a road.
		/// ps: cross road is reguard as space.
		/// </summary>
		/// <returns>The terrain grid4 level.</returns>
		/// <param name="maze">Maze.</param>
		public static CellType[,] GetCellTypeGrid4Level_old(Maze maze) {
			Cell[,] terrainGrid = maze.terrainMaze.Clone () as Cell[,];
			int height = terrainGrid.GetLength (0);
			int width = terrainGrid.GetLength (1);
			CellType[,] result = new CellType[height, width];

			Space[,] spacesClone = maze.spaces.Clone () as Space[,];
			int lenY = spacesClone.GetLength (0);
			int lenX = spacesClone.GetLength (1);
			// for all the space, find the doors...
			for (int numY = 0; numY < lenY; numY++) {
				for (int numX = 0; numX < lenX; numX++) {
					Space tempSpace = spacesClone [numY, numX];
					Point spacePoint = spacesClone [numY, numX].anchor;
					for (int tempY = 0; tempY < tempSpace.height; tempY++) {
						for (int tempX = 0; tempX < tempSpace.width; tempX++) {
							result [spacePoint.y + tempY, spacePoint.x + tempX] = CellType.SPACE;
						}
					}

					result = FindDoorsFromSpace (terrainGrid, result, spacePoint);
				}
			}


			for (int h = 0; h < height; h++) {
				for (int w = 0; w < width; w++) {
					result [h, w] = CellType.UNDEFINE;
					if (terrainGrid [h, w].direction == 0)
						result [h, w] = CellType.BLOCK;
					else if ((h == 0 && (terrainGrid [h, w].direction & MazeCommon.N) != 0) ||
						(h == height - 1 && (terrainGrid [h, w].direction & MazeCommon.S) != 0) || 
						(w == 0 && (terrainGrid [h, w].direction & MazeCommon.W) != 0) ||
						(h == width - 1 && (terrainGrid [h, w].direction & MazeCommon.E) != 0)) 
					{
						int connectNum = 0;
						for (int k = 0; k < 4; k++) {
							if ((terrainGrid [h, w].direction & MazeCommon.DR [k]) != 0)
								connectNum++;
						}
						// not as door.
						if (connectNum != 2) {
							result [h, w] = CellType.SPACE;
							result = FindDoorsFromSpace (terrainGrid, result, new Point (w, h));
						} else {
							result [h, w] = CellType.DOOR;
							result = FindPathsFromDoor (terrainGrid, result, new Point (w, h));
						}
					}
				}
			}

			return result;
		}

		static CellType[,] FindDoorsFromSpace(Cell[,] terrainGrid, CellType[,] levelGrid, Point spacePoint) {
			int height = levelGrid.GetLength (0);
			int width = levelGrid.GetLength (1);
			if (terrainGrid.GetLength (0) != height || terrainGrid.GetLength (1) != width) {
				return levelGrid;
			}

			CellType[,] result = levelGrid.Clone () as CellType[,];
			List<Point> tempList = new List<Point> ();
			tempList.Add (spacePoint);

			while (tempList.Count > 0) {
				int index = tempList.Count;
				Point p = tempList [index - 1];
				for (int i = 0; i < 4; i++) {
					int dr = MazeCommon.DR [i];
					int ny = p.y + MazeCommon.DY [dr];
					int nx = p.x + MazeCommon.DX [dr];
					int connectNum = 0;
					if (ny >= 0 && ny < height && nx >= 0 && nx < width &&
						result [ny, nx] == CellType.UNDEFINE &&
						((terrainGrid [p.y, p.x].direction & dr) != 0) &&
						((terrainGrid [ny, nx].direction & MazeCommon.OPPOSITE [dr]) != 0) ) {
						for (int k = 0; k < 4; k++) {
							if ((terrainGrid [ny, nx].direction & MazeCommon.DR [k]) != 0)
								connectNum++;
						}
						// not as door.
						if (connectNum > 0 && connectNum != 2) {
							result [ny, nx] = CellType.SPACE;
							tempList.Add (new Point (nx, ny));
						} else {
							result [ny, nx] = CellType.DOOR;
							result = FindPathsFromDoor (terrainGrid, result, new Point (nx, ny));
						}
					}
				}
				tempList.RemoveAt (index - 1);
			}

			return result;
		}

		static CellType[,] FindPathsFromDoor(Cell[,] terrainGrid, CellType[,] levelGrid, Point doorPoint) {
			int height = levelGrid.GetLength (0);
			int width = levelGrid.GetLength (1);
			if (terrainGrid.GetLength (0) != height || terrainGrid.GetLength (1) != width) {
				return levelGrid;
			}

			CellType[,] result = levelGrid.Clone () as CellType[,];
			List<Point> tempList = new List<Point> ();
			tempList.Add (doorPoint);

			while (tempList.Count > 0) {
				int index = tempList.Count;
				Point tempPoint = tempList [index - 1];
				for (int i = 0; i < 4; i++) {
					int dr = MazeCommon.DR [i];
					int ny = tempPoint.y + MazeCommon.DY [dr];
					int nx = tempPoint.x + MazeCommon.DX [dr];
					int connectNum = 0;
					if (ny >= 0 && ny < height && nx >= 0 && nx < width &&
						result [ny, nx] == CellType.UNDEFINE &&
						((terrainGrid [tempPoint.y, tempPoint.x].direction & dr) != 0) &&
						((terrainGrid [ny, nx].direction & MazeCommon.OPPOSITE [dr]) != 0) ) {
						for (int k = 0; k < 4; k++) {
							if ((terrainGrid [ny, nx].direction & MazeCommon.DR [k]) != 0)
								connectNum++;
						}
						// not as door.
						if (connectNum == 2) {
							result [ny, nx] = CellType.PATH;
							tempList.Add (new Point (nx, ny));
						} else if (connectNum == 1) {
							result [ny, nx] = CellType.DOOR;
						}
						else if (connectNum > 2) {
							result [ny, nx] = CellType.SPACE;
							result [tempPoint.y, tempPoint.x] = CellType.DOOR;
							result = FindDoorsFromSpace (terrainGrid, result, new Point (nx, ny));
						}
					}
				}
				tempList.RemoveAt (index - 1);
			}

			return result;
		}


		static Maze AdjustMazeSpaces(Maze maze) {
			int height = maze.terrainMaze.GetLength (0);
			int width = maze.terrainMaze.GetLength (1);
			Cell[,] terrainGrid = maze.terrainMaze.Clone () as Cell[,];
			Space[,] spacesClone = maze.spaces.Clone () as Space[,];
			// scaling the space blockers of maze.spaces; save the add on point in the addOn list;
			int lenY = spacesClone.GetLength (0);
			int lenX = spacesClone.GetLength (1);
			for (int numY = 0; numY < lenY; numY++) {
				for (int numX = 0; numX < lenX; numX++) {
					// check the space area here, and save the add on points in list
					// infect from space area, only the 2 directions cell should not be add to the list
					Space tempSpace = spacesClone [numY, numX];
					List<Point> checkedList = new List<Point>();
					List<Point> tempList = new List<Point>();
					for (int h = 0; h < tempSpace.height; h++) {
						for (int w = 0; h < tempSpace.width; w++) {
							tempList.Add (new Point (tempSpace.anchor.y + h, tempSpace.anchor.x + w));
							checkedList.Add (new Point (tempSpace.anchor.y + h, tempSpace.anchor.x + w));
						}
					}

					while (tempList.Count > 0) {
						int index = tempList.Count;
						Point p = tempList [index - 1];
						for (int i = 0; i < 4; i++) {
							int dr = MazeCommon.DR [i];
							int ny = p.y + MazeCommon.DY [dr]; int nx = p.x + MazeCommon.DX [dr];
							if (ny >= 0 && ny < height && nx >= 0 && nx < width &&
								(terrainGrid [ny, nx].direction & MazeCommon.OPPOSITE [dr]) != 0) { // at first ,the point (ny, nx) should be valid

								bool isPath = false;
								if ((terrainGrid [ny, nx].direction & MazeCommon.N) != 0 && (terrainGrid [ny, nx].direction & MazeCommon.S) != 0 &&
									(terrainGrid [ny, nx].direction & MazeCommon.W) == 0 && (terrainGrid [ny, nx].direction & MazeCommon.E) == 0) {
									isPath = true;
								} else if ((terrainGrid [ny, nx].direction & MazeCommon.N) == 0 && (terrainGrid [ny, nx].direction & MazeCommon.S) == 0 &&
									(terrainGrid [ny, nx].direction & MazeCommon.W) != 0 && (terrainGrid [ny, nx].direction & MazeCommon.E) != 0) {
									isPath = true;
								}
								Point tempPoint = new Point (nx, ny);
								if (!isPath) {

									tempList.Add (tempPoint);
									tempSpace.addOn.Add (tempPoint);
								} else {
									tempSpace.paths.Add (tempPoint);
								}
							}

						}
						tempList.RemoveAt (index - 1);
					}
					spacesClone [numY, numX] = tempSpace;
				}
			}
			maze.spaces = spacesClone;
			return maze;
		}

		#endregion
	}



	public struct Door {
		public Point coordinate;
		public List<DoorAdjvex> adjvex;

		public Door(Point p) {
			this.coordinate = p;
			this.adjvex = new List<DoorAdjvex>();
		}
	}

	public struct DoorAdjvex {
		public Point nextDoor;
		public bool isSpace;
		public List<Point> road;
		public int canStairCount;
		public DoorAdjvex () : this(new Point()){}

		public DoorAdjvex (Point p) {
			this.nextDoor = p;
			this.isSpace = false;
			this.canStairCount = 0;
			this.road = new List<Point>();
		}
	}
}
