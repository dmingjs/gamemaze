using System;
using System.Collections.Generic;

namespace GameMazeCreator_01
{
	public class MazeLevelCommon
	{
		public MazeLevelCommon ()
		{
		}

		public enum CellType {
			UNDEFINE = -1,
			SPACE = 1,
			DOOR = 2,
			PATH = 4,
			BLOCK = 8,
		}

		/// <summary>
		/// Gets the cell type grid for level.
		/// CellType[,] GetCellTypeGrid4Level(Maze maze)
		/// </summary>
		/// <returns>The cell type grid4 level.</returns>
		/// <param name="maze">Maze.</param>
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


			for (int numY = 0; numY < lenY; numY++) {
				for (int numX = 0; numX < lenX; numX++) {
					List<Point> checkList = new List<Point> ();
					List<Point> spaceList = new List<Point> ();

					Space tempSpace = spacesClone [numY, numX];
					Point spacePoint = spacesClone [numY, numX].anchor;
					for (int tempY = 0; tempY < tempSpace.height; tempY++) {
						for (int tempX = 0; tempX < tempSpace.width; tempX++) {
							//result [spacePoint.y + tempY, spacePoint.x + tempX] = CellType.SPACE;
							checkList.Add (new Point (spacePoint.x + tempX, spacePoint.y + tempY));
							spaceList.Add (new Point (spacePoint.x + tempX, spacePoint.y + tempY));
						}
					}


					// meybe it should not use the ergodic method, 
					// if it can use the dfs method??? -- dming
					while (checkList.Count > 0) {
						int index = MazeCommon.GetRandom ().Next (0, checkList.Count);
						Point nowPoint = checkList [index];
						int connectToSpaceNum = 0;
						int connectNum = 0;
						for (int i = 0; i < 4; i++) {
							int dr = MazeCommon.DR [i];
							int ny = nowPoint.y + MazeCommon.DY [dr];
							int nx = nowPoint.x + MazeCommon.DX [dr];
							connectToSpaceNum = 0;
							connectNum = 0;
							#region if undefine
							if (ny >= 0 && ny < height && nx >= 0 && nx < width &&
								result [ny, nx] == CellType.UNDEFINE) {
								// to check if it should be a door ??
								// it should check the point(door_point) next to the space area 
								// and the next point to the door_point, the next point should be seperate to the space area

								// check from terrainGrid [ny, nx]
								if (((terrainGrid [nowPoint.y, nowPoint.x].direction & dr) != 0) &&
									((terrainGrid [ny, nx].direction & MazeCommon.OPPOSITE [dr]) != 0)) {
									for (int k = 0; k < 4; k++) {
										int k_dr = MazeCommon.DR [k];
										if (dr == MazeCommon.OPPOSITE [k_dr]) {
											continue;
										}

										int k_ny = ny + MazeCommon.DY [k_dr];
										int k_nx = nx + MazeCommon.DX [k_dr];
										if (k_ny >= 0 && k_ny < height && k_nx >= 0 && k_nx < width &&
											((terrainGrid [ny, nx].direction & k_dr) != 0) &&
											((terrainGrid [k_ny, k_nx].direction & MazeCommon.OPPOSITE [k_dr]) != 0)) {
											connectNum++;
											for (int n = 0; n < 4; n++) {
												int n_dr = MazeCommon.DR [n];
												if (n_dr == MazeCommon.OPPOSITE [k_dr])
													continue;
												int n_ny = k_ny + MazeCommon.DY [n_dr];
												int n_nx = k_nx + MazeCommon.DX [n_dr];
												if (n_ny >= 0 && n_ny < height && n_nx >= 0 && n_nx < width &&
													((terrainGrid [k_ny, k_nx].direction & n_dr) != 0) &&
													((terrainGrid [n_ny, n_nx].direction & MazeCommon.OPPOSITE [n_dr]) != 0)) {
													if (ListHasPoint(spaceList, new Point(n_nx, n_ny)))
														connectToSpaceNum++;
												}
											}
										}
									}

									if (connectToSpaceNum == 0) {
										result [ny, nx] = CellType.DOOR;
									} else {
										//Console.WriteLine(connectToSpaceNum);
										result [ny, nx] = CellType.SPACE;
										checkList.Add (new Point (nx, ny));
										spaceList.Add (new Point (nx, ny));
									}
								}

							}
							#endregion
						}
						checkList.RemoveAt (index);
					}
				}
			}


			//check from space cell
			return result;
		}

		static bool ListHasPoint(List<Point> List, Point point) {
			foreach (Point p in List) {
				if (p.x == point.x && p.y == point.y) {
					return true;
				}
			}
			return false;
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
		public static Dictionary<Point, Door> GetRoadMap_old(Cell[,] terrainGrid, CellType[,] cellTypeGird, Space[,] spacesClone) {
			Dictionary<Point, Door> roadMap = new Dictionary<Point, Door> ();
			int height = cellTypeGird.GetLength (0);
			int width = cellTypeGird.GetLength (1);
			int[,] checkArray = new int[height, width];
			bool[,] canStairArray = CheckIfCanSetAStair (terrainGrid, cellTypeGird);

			#region Get direct link doors by spaces
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
							if (nx >= 0 && nx < width && ny >= 0 && ny < height && checkArray[ny, nx] == 0 && 
								(terrainGrid[tempPoint.y, tempPoint.x].direction & dr) != 0) {
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
						roadMap.Add (tempDoors [i], new Door (tempDoors [i]));
					}
					for (int i = 0; i < tempDoors.Count; i++) {
						//doorDict.Add (tempList [i], new Door(tempList [i]));
						//Point nextDoorcoor = tempList [i];
						int spaceDR = 0;
						for (int d = 0; d < 4; d++) {
							int dr = MazeCommon.DR[d];
							int ny = tempDoors[i].y + MazeCommon.DY[dr];
							int nx = tempDoors[i].x + MazeCommon.DX[dr];
							if (ny >= 0 && ny < height && nx >= 0 && nx < width &&
								(terrainGrid[tempDoors[i].y, tempDoors[i].x].direction & dr) != 0 &&
								(terrainGrid[ny, nx].direction & MazeCommon.OPPOSITE[dr]) != 0) {
								if (cellTypeGird[ny, nx] == CellType.SPACE) {
									spaceDR = dr;
									break;
								}
							}
						}

						if (spaceDR != 0) {
							roadMap [tempDoors[i]].neighbors.Add(spaceDR, new List<DoorAdjvex>());
							for (int k = 0; k < tempDoors.Count; k++) {
								if (tempDoors[i].x == tempDoors[k].x && tempDoors[i].y == tempDoors[k].y)
									continue;
								DoorAdjvex da = new DoorAdjvex (tempDoors [k]);
								da.isSpace = true;

								List<DoorAdjvex> tempAdjvex = roadMap[tempDoors[i]].neighbors[spaceDR];
								tempAdjvex.Add(da);
								roadMap[tempDoors[i]].neighbors[spaceDR] = tempAdjvex;
								roadMap[tempDoors[i]].directionLevelRange[spaceDR] = new LevelRange(-5, 5);
								//roadMap [tempDoors [i]].adjvex.Add(da);
							}
						} else {
							throw new Exception("it has no direction to connect space area");
						}

					}
				}
			}
			#endregion

			#region deal with road 

			for (int y = 0; y < height; y++) {
				for (int x = 0; x < width; x++) {
					if (cellTypeGird [y, x] == CellType.DOOR) {
						// get neighbor doors
						Point nowPoint = new Point (x, y);
						if (!roadMap.ContainsKey (nowPoint)) {
							roadMap.Add (nowPoint, new Door (nowPoint));
						}

						for (int i = 0; i < 4; i++) {
							int dr = MazeCommon.DR [i];
							int ny = y + MazeCommon.DY [dr];
							int nx = x + MazeCommon.DX [dr];
							LevelRange lr = new LevelRange(-5, 5);
							if (nx >= 0 && nx < width && ny >= 0 && ny < height &&
								((terrainGrid[y, x].direction & dr) != 0) && 
								((terrainGrid[ny, nx].direction & MazeCommon.OPPOSITE[dr]) != 0)) {
								if (cellTypeGird [ny, nx] == CellType.DOOR) {
									DoorAdjvex da = new DoorAdjvex (nowPoint);
									//roadMap [nowPoint].adjvex.Add (da);
									roadMap [nowPoint].directionLevelRange.Add(dr, lr);
									List<DoorAdjvex> value = new List<DoorAdjvex>();
									value.Add(da);
									roadMap [nowPoint].neighbors.Add(dr, value);
								} else if (cellTypeGird [ny, nx] == CellType.PATH) {
									roadMap [nowPoint].directionLevelRange.Add(dr, lr);
									DoorAdjvex da = new DoorAdjvex ();
									da.road.Enqueue (new Point (nx, ny));
									da = CheckPath (new Point (nx, ny), nowPoint, terrainGrid, cellTypeGird, da);
									//roadMap[nowPoint].adjvex.Add(da);
									List<DoorAdjvex> value = new List<DoorAdjvex>();
									value.Add(da);
									roadMap [nowPoint].neighbors.Add(dr, value);
								}
							}
						}

						// get canStairCount
						for (int i = 0; i < 4; i++) {
							int dr = MazeCommon.DR[i];

							if (!roadMap[nowPoint].neighbors.ContainsKey(dr)) continue;

							for (int k = 0; k < roadMap[nowPoint].neighbors[dr].Count; k++) {
								int canStairCount = 0;

								if (!roadMap[nowPoint].neighbors[dr][k].isSpace) {
									Point[] roadArray = roadMap[nowPoint].neighbors[dr][k].road.ToArray ();
									for (int j = 0; j < roadArray.Length; j++) {
										if (canStairArray [roadArray[j].y, roadArray[j].x]) {
											canStairCount++;
										}
									}
								}
								var tempDoor = roadMap [nowPoint];
								tempDoor.canStair = canStairArray[tempDoor.coordinate.y, tempDoor.coordinate.x];
								var tempAdjvex = tempDoor.neighbors[dr][k];
								tempAdjvex.canStairCount = canStairCount;
								tempDoor.neighbors[dr][k] = tempAdjvex;
								roadMap [nowPoint] = tempDoor;
							}
						}
					}
				}
			}
			#endregion

			return roadMap;
		}


		public static LevelCell[,] GetLevelCellGrid(Cell[,] terrainGrid, CellType[,] cellTypeGird) {
			Dictionary<Point, Door> roadMap = new Dictionary<Point, Door> ();
			int height = cellTypeGird.GetLength (0);
			int width = cellTypeGird.GetLength (1);
			int[,] checkArray = new int[height, width];
			bool[,] canStairArray = CheckIfCanSetAStair (terrainGrid, cellTypeGird);

			// init levelCellGrid as the result
			LevelCell[,] levelCellGrid = new LevelCell[height, width];
			for (int y = 0; y < height; y++) {
				for (int x = 0; x < width; x ++) {
					levelCellGrid [y, x] = new LevelCell ();
					levelCellGrid [y, x].point = new Point (x, y);
					levelCellGrid [y, x].canStair = canStairArray [y, x];
					levelCellGrid [y, x].type = cellTypeGird [y, x];

					levelCellGrid [y, x].direction = terrainGrid [y, x].direction;
					levelCellGrid [y, x].level = terrainGrid [y, x].level;

					for (int i = 0; i < 4; i++) {
						int dr = MazeCommon.DR [i];
						if ((levelCellGrid [y, x].direction & dr) != 0) {
							levelCellGrid [y, x].levelRanges.Add (dr, new LevelRange (-5, 5));
						}
					}
				}
			}

			// init about the door, changes are store in the neighbors
			for (int y = 0; y < height; y++) {
				for (int x = 0; x < width; x++) {
					if (levelCellGrid [y, x].type != CellType.DOOR)
						continue;

					for (int i = 0; i < 4; i++) {
						int dr = MazeCommon.DR [i];
						int ny = y + MazeCommon.DY [dr];
						int nx = x + MazeCommon.DX [dr];

						// not connected , so ignored it..   ! at beginning
						if (!(nx >= 0 && nx < width && ny >= 0 && ny < height && checkArray [ny, nx] == 0 &&
							(levelCellGrid [y, x].direction & dr) != 0 &&
							(levelCellGrid [ny, nx].direction & MazeCommon.OPPOSITE[dr]) != 0))
							continue;

						// 从该space point 开始，寻找所有space area直连的door
						if (levelCellGrid [ny, nx].type == CellType.SPACE) {
							// find all the doors connect to the space area
							List<Point> spaceList = new List<Point> ();
							List<Point> checkedList = new List<Point> ();
							spaceList.Add (new Point (nx, ny));
							checkedList.Add (new Point (nx, ny));

							while (spaceList.Count > 0) {
								int index = 0;
								Point nowPoint = spaceList [index];
								//checkedList.Add (nowPoint);
								spaceList.RemoveAt (index);

								for (int t_k = 0; t_k < 4; t_k++) {
									int t_dr = MazeCommon.DR [t_k];
									int t_ny = nowPoint.y + MazeCommon.DY [t_dr];
									int t_nx = nowPoint.x + MazeCommon.DX [t_dr];

									if (t_ny == y && t_nx == x)
										continue;

									// if connected to (ny, nx)
									if (t_nx >= 0 && t_nx < width && t_ny >= 0 && t_ny < height &&
										(levelCellGrid [nowPoint.y, nowPoint.x].direction & t_dr) != 0 &&
									    (levelCellGrid [t_ny, t_nx].direction & MazeCommon.OPPOSITE [t_dr]) != 0) {
										Point checkingPoint = new Point (t_nx, t_ny);

										if (levelCellGrid [t_ny, t_nx].type == CellType.SPACE && !ListHasPoint (checkedList, checkingPoint)) {
											spaceList.Add (checkingPoint);
											checkedList.Add (checkingPoint);
										} else if (levelCellGrid [t_ny, t_nx].type == CellType.DOOR) { // get the neighbor door
											if (!levelCellGrid [y, x].neighbors.ContainsKey (dr))
												levelCellGrid [y, x].neighbors.Add (dr, new List<DoorAdjvex> ());

											List<DoorAdjvex> tempList = levelCellGrid [y, x].neighbors [dr];
											DoorAdjvex da = new DoorAdjvex (checkingPoint);
											da.isSpace = true;
											tempList.Add (da);
											Console.WriteLine ("door ({0}, {1}) neighbor door is ({2}, {3}), near ({4}, {5})", x, y, t_nx, t_ny, nowPoint.x, nowPoint.y);
											levelCellGrid [y, x].neighbors [dr] = tempList;
										}
									}
								}

							}
						}
						else if (levelCellGrid [ny, nx].type == CellType.DOOR) {
							Point nowPoint = new Point (nx, ny);
							DoorAdjvex da = new DoorAdjvex (nowPoint);
							List<DoorAdjvex> value = new List<DoorAdjvex> ();
							value.Add (da);
							levelCellGrid [y, x].neighbors.Add (dr, value);
						} 
						else if (levelCellGrid [ny, nx].type == CellType.PATH) {
							Point nowPoint = new Point (x, y);
							DoorAdjvex da = new DoorAdjvex ();
							da.road.Enqueue (new Point (nx, ny));
							da = CheckPath (new Point (nx, ny), nowPoint, terrainGrid, cellTypeGird, da);

							// culculate the canStairCount
							int canStairCount = 0;
							Point[] roadArray = da.road.ToArray ();
							for (int j = 0; j < roadArray.Length; j++) {
								if (canStairArray [roadArray [j].y, roadArray [j].x]) {
									canStairCount++;
								}
							}
							da.canStairCount = canStairCount;

							List<DoorAdjvex> value = new List<DoorAdjvex> ();
							value.Add (da);
							levelCellGrid [y, x].neighbors.Add (dr, value);
						} 
						else
							throw new Exception ("not space not door not path, so error");
					}
				}
			}

			return levelCellGrid;
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
						doorAdjvex.road.Enqueue (new Point (nx, ny));
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
			//bool[,] canStairArray = CheckIfCanSetAStair (terrainGrid, cellTypeGird);
			LevelCell[,] levelCellGrid = GetLevelCellGrid (terrainGrid, cellTypeGird);

			List<Point> doorList = new List<Point> ();
			List<Point> checkedList = new List<Point> ();

			//先初始化迷宫的四面入口，加入到doorlist 里
			for (int y = 0; y < height; y++) {
				for (int x = 0; x < width; x++) {
					if (levelCellGrid [y, x].type != CellType.DOOR)
						continue;

					Point nowPoint = new Point (x, y);
					if (y == 0 && (levelCellGrid [y, x].direction & MazeCommon.N) != 0) {
						LevelRange lr = new LevelRange (levelCellGrid [y, x].level, levelCellGrid [y, x].level);
						lr.checkTimes = 2; // when checkTimes is > 1, then not change
						levelCellGrid [y, x].levelRanges[MazeCommon.N] = lr;
						if (!ListHasPoint (doorList, nowPoint))
							doorList.Add (nowPoint);
					}
					if (y == height - 1 && (levelCellGrid [y, x].direction & MazeCommon.S) != 0) {
						LevelRange lr = new LevelRange (levelCellGrid [y, x].level, levelCellGrid [y, x].level);
						lr.checkTimes = 2;
						levelCellGrid [y, x].levelRanges[MazeCommon.S] = lr;
						if (!ListHasPoint (doorList, nowPoint))
							doorList.Add (nowPoint);
					}
					if (x == 0 && (levelCellGrid [y, x].direction & MazeCommon.W) != 0) {
						LevelRange lr = new LevelRange (levelCellGrid [y, x].level, levelCellGrid [y, x].level);
						lr.checkTimes = 2;
						levelCellGrid [y, x].levelRanges[MazeCommon.W] = lr;
						if (!ListHasPoint (doorList, nowPoint))
							doorList.Add (nowPoint);
					}
					if (y == width - 1 && (levelCellGrid [y, x].direction & MazeCommon.E) != 0) {
						LevelRange lr = new LevelRange (levelCellGrid [y, x].level, levelCellGrid [y, x].level);
						lr.checkTimes = 2;
						levelCellGrid [y, x].levelRanges[MazeCommon.E] = lr;
						if (!ListHasPoint (doorList, nowPoint))
							doorList.Add (nowPoint);
					}
				}



			}
			// 对所有的cell tyle == door 进行取值范围均衡
			while (doorList.Count > 0) {
				int index = MazeCommon.GetRandom ().Next (0, doorList.Count);
				Point nowPoint = doorList [index];
				checkedList.Add (nowPoint);
				doorList.RemoveAt (index);

				LevelCell lc = levelCellGrid [nowPoint.y, nowPoint.x];
				if (lc.type != CellType.DOOR)
					throw new Exception ("type should be door");

				// check the level range inside the door first
				lc = InsideLevelRangeChange(lc);
				levelCellGrid [nowPoint.y, nowPoint.x] = lc;

				// check outside
				for (int i = 0; i < 4; i++) {
					int dr = MazeCommon.DR [i];
					if (lc.neighbors.ContainsKey (dr)) {
						for (int n = 0; n < lc.neighbors[dr].Count; n++) {
							DoorAdjvex da = lc.neighbors [dr] [n];
							LevelCell neighborLC = levelCellGrid [da.nextDoor.y, da.nextDoor.x];
							int neighborDR = GetDoorNeighborDR (neighborLC, nowPoint);
							if (neighborDR == 0)
								throw new Exception ("neighbor dr should not be equal to 0");

							int tempMin = lc.levelRanges [dr].minLevel - da.canStairCount;
							int tempMax = lc.levelRanges [dr].maxLevel + da.canStairCount;
							tempMin = Math.Max (tempMin, neighborLC.levelRanges [neighborDR].minLevel);
							tempMax = Math.Min (tempMax, neighborLC.levelRanges [neighborDR].maxLevel);
							if (tempMin > tempMax)
								throw new Exception ("tempMin > tempMax");
							LevelRange neighborLrange = new LevelRange (tempMin, tempMax);
							neighborLC.levelRanges [neighborDR] = neighborLrange;
							neighborLC = InsideLevelRangeChange (neighborLC);
							levelCellGrid [da.nextDoor.y, da.nextDoor.x] = neighborLC;

							if (!ListHasPoint (checkedList, neighborLC.point))
								doorList.Add (neighborLC.point);
						}
					}
				}
				

				//
			}

			// 试图去取值

			return maze;
		}

		static LevelCell InsideLevelRangeChange (LevelCell lc) {
			for (int i = 0; i < 4; i++) {
				int dr = MazeCommon.DR [i];
				if (lc.levelRanges.ContainsKey (dr)) {
					int tempMin = 5;
					int tempMax = -5;
					if (lc.canStair) {
						tempMin = lc.levelRanges [dr].minLevel - 1;
						tempMax = lc.levelRanges [dr].maxLevel + 1;
					} else {
						tempMin = lc.levelRanges [dr].minLevel;
						tempMax = lc.levelRanges [dr].maxLevel;
					}
					for (int t_i = 0; t_i < 4; t_i++) {
						if (i == t_i)
							continue;

						int t_dr = MazeCommon.DR [t_i];
						if (lc.levelRanges.ContainsKey (t_dr)) {
							int min = Math.Max (tempMin, lc.levelRanges [t_dr].minLevel);
							int max = Math.Min (tempMax, lc.levelRanges [t_dr].maxLevel);
							if (min > max)
								throw new Exception ("min > max");
							LevelRange lrange = new LevelRange (min, max);
							lc.levelRanges [t_dr] = lrange;
						}
					}
				}
			}

			return lc;
		}

		static int GetDoorNeighborDR(LevelCell lc, Point neighborPoint) {
			if (lc.type != CellType.DOOR)
				return 0;

			for (int i = 0; i < 4; i++) {
				int dr = MazeCommon.DR [i];
				if (lc.neighbors.ContainsKey (dr)) {
					for (int n = 0; n < lc.neighbors [dr].Count; n++) {
						if (lc.neighbors[dr][n].nextDoor.x == neighborPoint.x &&
							lc.neighbors[dr][n].nextDoor.y == neighborPoint.y)
							return dr;
					}
				}
			}

			return 0;
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
						if ((levelGrid [i, j] == CellType.BLOCK)) {
							Console.Write ("#");
							continue;
						} else if ((levelGrid [i, j] == CellType.PATH)) {
							Console.Write ("_");
							continue;
						}
					}

					if ((levelGrid [i, j] == CellType.BLOCK))
						Console.Write ("#");
					else if ((levelGrid [i, j] == CellType.DOOR))
						Console.Write ("D");
					else if ((levelGrid [i, j] == CellType.PATH))
						Console.Write ("P");
					else if ((levelGrid [i, j] == CellType.SPACE))
						Console.Write (".");
					else
						Console.Write (" ");
				}
				Console.WriteLine ();
			}
		}

	}
}

#endregion