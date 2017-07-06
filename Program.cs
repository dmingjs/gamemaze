using System;

namespace GameMazeCreator_01
{
	class MainClass
	{
		public static void Main (string[] args)
		{
			Console.WriteLine ("Hello World!");

			BasicMazeCreator maze = new BasicMazeCreator (9, 9, "");

			int[,] grid = maze.GrowingTree_Maze();
			int[,] gridAdjust = MazeCommon.AdjustMazeBorder (grid);
			int[,] gridx2Adjust = MazeCommon.GridScaleDoubleWithBlock (gridAdjust);
			int[,] map = MazeCommon.CreateMapByGrid (gridx2Adjust);
			int[,] mapx2 = MazeCommon.CreateMapByScaleDoubleGrid (gridx2Adjust);
			int[,] gridx2AdjustSpace = MazeCommon.MazeInsertSpace (gridx2Adjust, 6);
			int[,] mapAdjustV2 = MazeCommon.CreateMapByScaleDoubleGrid (gridx2AdjustSpace);

			MazeCommon.Print2DArrayWithWall (mapAdjustV2);
			Console.WriteLine ("\n~~~~~~~~~~~~~~~~~~~line~~~~~~~~~~~~~\n");
			Console.WriteLine ("\n~~~~~~~~~~~~~~~~~~~line~~~~~~~~~~~~~\n");
			Console.Read ();
		}
	}
}
