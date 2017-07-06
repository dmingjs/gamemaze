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

					if ((grid [y, x].direction & MazeCommon.N) != 0) {
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
							} else if (tempY == (spaces [i, j].height - 1) &&
								(grid [tempY + spaces [i, j].anchor.y + split * i + 1, tempX + spaces [i, j].anchor.x + split * j].direction & MazeCommon.N) == 0){
								grid [tempY + spaces [i, j].anchor.y + split * i, tempX + spaces [i, j].anchor.x + split * j].direction -= MazeCommon.S;
							}

							if (tempX == 0 &&
								(grid [tempY + spaces [i, j].anchor.y + split * i, tempX + spaces [i, j].anchor.x + split * j - 1].direction & MazeCommon.E) == 0) {
								grid [tempY + spaces [i, j].anchor.y + split * i, tempX + spaces [i, j].anchor.x + split * j].direction -= MazeCommon.W;
							} else if  (tempX == (spaces [i, j].width - 1) &&
								(grid [tempY + spaces [i, j].anchor.y + split * i, tempX + spaces [i, j].anchor.x + split * j + 1].direction & MazeCommon.W) == 0) {
								grid [tempY + spaces [i, j].anchor.y + split * i, tempX + spaces [i, j].anchor.x + split * j].direction -= MazeCommon.E;
							}
						}
					}
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

			//List<Cell> 
			return newMaze;
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
	}
}
