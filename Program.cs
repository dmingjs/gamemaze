using System;

namespace GameMazeCreator_01
{
	class MainClass
	{
		public static void Main (string[] args)
		{
			Console.WriteLine ("Hello World!");

			BasicMazeCreator maze = new BasicMazeCreator (9, 6, "");
			//int[,] map = MazeCommon.CreateMapByGrid (maze.InitGrid_WithBlocks());
			int[,] grid = maze.GrowingTree_Maze();
			int[,] gridx2 = MazeCommon.GridScaleDouble (grid);
			int[,] map = MazeCommon.CreateMapByGrid(grid);
			int[,] map2 = MazeCommon.CreateMapByScaleDoubleGrid (gridx2);
			//MazeCommon.PrintArray (grid);
			Console.WriteLine ("\n~~~~~~~~~~~~~~~~~~~line~~~~~~~~~~~~~\n");
			MazeCommon.Print2DArray (map2);
			Console.WriteLine ("\n~~~~~~~~~~~~~~~~~~~line~~~~~~~~~~~~~\n");
			MazeCommon.Print2DArray (map);
			Console.WriteLine ("\n~~~~~~~~~~~~~~~~~~~line~~~~~~~~~~~~~\n");
			MazeCommon.Print2DArrayWithWall (map);
			Console.WriteLine ("\n~~~~~~~~~~~~~~~~~~~line~~~~~~~~~~~~~\n");
			MazeCommon.Print2DArrayWithWall (map2);
			Console.WriteLine ("\n~~~~~~~~~~~~~~~~~~~line~~~~~~~~~~~~~\n");
			int[,] spacegrid = MazeCommon.MazeInsertSpace (gridx2, 6);
			int[,] spacemap = MazeCommon.CreateMapByScaleDoubleGrid (spacegrid);
			MazeCommon.Print2DArrayWithWall (spacemap);
			Console.WriteLine ("\n~~~~~~~~~~~~~~~~~~~line~~~~~~~~~~~~~\n");
			MazeCommon.Print2DArray (spacemap);
			Console.WriteLine ("\n~~~~~~~~~~~~~~~~~~~line~~~~~~~~~~~~~\n");
			Console.Read ();
		}
	}
}
