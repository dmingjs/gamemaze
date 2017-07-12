using System;

namespace GameMazeCreator_01
{
	class MainClass
	{
		public static void Main (string[] args)
		{
			Console.WriteLine ("Hello World!");

			BasicMazeCreator mazeCreator = new BasicMazeCreator (9, 9, "");

			Maze mazeN = mazeCreator.GrowingTree_Maze();
			Maze mazeS = mazeCreator.GrowingTree_Maze();
			Maze mazeW = mazeCreator.GrowingTree_Maze();
			Maze mazeE = mazeCreator.GrowingTree_Maze();
			Maze maze = new Maze (mazeCreator.GetWidth(), mazeCreator.GetHeight());

			mazeN = MazeGeneratorCommon.AdjustMazeBorder (mazeN);
			mazeN = MazeGeneratorCommon.CreateTerrainMaze (mazeN);
			maze.neighborMazes.Add (MazeCommon.N, mazeN);

			mazeS = MazeGeneratorCommon.AdjustMazeBorder (mazeS);
			mazeS = MazeGeneratorCommon.CreateTerrainMaze (mazeS);
			maze.neighborMazes.Add (MazeCommon.S, mazeS);

			mazeW = MazeGeneratorCommon.AdjustMazeBorder (mazeW);
			mazeW = MazeGeneratorCommon.CreateTerrainMaze (mazeW);
			maze.neighborMazes.Add (MazeCommon.W, mazeW);

			mazeE = MazeGeneratorCommon.AdjustMazeBorder (mazeE);
			mazeE = MazeGeneratorCommon.CreateTerrainMaze (mazeE);
			maze.neighborMazes.Add (MazeCommon.E, mazeE);

			MazeGeneratorCommon.InitMazeByNeighbors (maze);
			maze = mazeCreator.GrowingTree_Maze (maze);
			//Maze mazeAdjust = MazeGeneratorCommon.AdjustMazeBorder (maze);
			//int[,] map = MazeGeneratorCommon.CreateMapByMaze (MazeGeneratorCommon.AdjustMazeLevel(maze));
			int[,] map = MazeGeneratorCommon.CreateMapByMaze (maze);

			int[,] mapN = MazeGeneratorCommon.CreateMapByMaze (mazeN);
			int[,] mapS = MazeGeneratorCommon.CreateMapByMaze (mazeS);
			Console.WriteLine ("mapN");
			MazeCommon.Print2DArrayWithWall (mapN);
			Console.WriteLine ("map");
			MazeCommon.Print2DArrayWithWall (map);
			Console.WriteLine ("mapS");
			MazeCommon.Print2DArrayWithWall (mapS);
			Console.WriteLine ("\n~~~~~~~~~~~~~~~~~~~line~~~~~~~~~~~~~\n");
			maze = MazeGeneratorCommon.CreateTerrainMaze (maze);
			int[,] terrainMap = MazeGeneratorCommon.CreateMapByTerrainMaze (maze);
			MazeCommon.Print2DArrayWithWall (terrainMap);
			Console.WriteLine ("\n~~~~~~~~~~~~~~~~~~~line~~~~~~~~~~~~~\n");

			maze = MazeGeneratorCommon.TerrainMazeInsertSpaces (maze, 6);
			terrainMap = MazeGeneratorCommon.CreateMapByTerrainMaze (maze);
			MazeCommon.Print2DArrayWithWall (terrainMap);
			Console.WriteLine ("\n~~~~~~~~~~~~~~~~~~~line~~~~~~~~~~~~~\n");
			MazeGeneratorCommon.CellType[,] levelGrid = MazeGeneratorCommon.GetCellTypeGrid4Level (maze);
			MazeGeneratorCommon.PrintComplexTerrainMapWithLevelGrid (terrainMap, levelGrid);
			Console.WriteLine ("\n~~~~~~~~~~~~~~~~~~~line~~~~~~~~~~~~~\n");
			for (int i = 0; i < levelGrid.GetLength (0); i++) {
				for (int j = 0; j < levelGrid.GetLength (1); j++) {
					//Console.Write ((int)levelGrid [i, j]);
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
			Console.WriteLine ("\n~~~~~~~~~~~~~~~~~~~line~~~~~~~~~~~~~\n");

			System.Collections.Generic.Dictionary<Point, Door> da = MazeGeneratorCommon.GetRoadByCellTypeGrid (maze.terrainMaze, levelGrid, maze.spaces);
			//Console.WriteLine (da.Count);
			foreach (var d in da) {
				if (d.Value.adjvex.Count == 0) {
					Console.WriteLine ("wrong in {0}", d.Value.coordinate.ToString ());
				} else {
					Console.WriteLine ("point ({0}, {1}) adjvex count is {2}", d.Key.x, d.Key.y, d.Value.adjvex.Count);
				}
			}

			maze = MazeGeneratorCommon.CreateMazeWithLevel (maze);
			Console.Read ();
		}
	}
}
