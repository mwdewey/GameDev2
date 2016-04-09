using UnityEngine;
using System.Collections;
using System.Collections.Generic;

public class Room {
	public int x = 0;
	public int y = 0;
	public int w = 0;
	public int h = 0;
	public Room connectedTo = null;
}

public class SpawnList {
	public int x = 0;
	public int y = 0;
	public bool byWall;
	public bool byCorridor;
	public bool asDoor;
	public bool spawnedObject;
}

[System.Serializable] //Serializing = "Show this in the editor"
public class SpawnOption {
	public int minSpawnCount = 0;
	public int maxSpawnCount = 0;
	public bool spawnByWall;
	public GameObject gameObject;
}

public class MapTile {
	public int type = 0;
	public Room room = null;
}

public class dungeonGenerator : MonoBehaviour {
	public GameObject startPrefab;
	public GameObject exitPrefab;
	public List<SpawnList> spawnedObjectLocations = new List<SpawnList>();
	public GameObject floorPrefab;
	public GameObject wallPrefab;
	public GameObject doorPrefab;

	public GameObject cornerPrefab;
	public bool cornerRotation = false;
	public bool generateOnLoad;

	public int minimumRoomCount = 0;
	public int maximumRoomCount = 0;
	public int minRoomSize = 0;
	public int maxRoomSize = 0;
	public float tileScaling = 0.0f;
	public List<SpawnOption> spawnOptions = new List<SpawnOption>();
	int roomMargin = 3;

	class Dungeon {
		public static int map_size;
		public static MapTile[,] map;

		public static List<Room> rooms = new List<Room>();
		public static Room goalRoom;
		public static Room startRoom;

		public int min_size = 0;
		public int max_size = 0;

		public int maximumRoomCount = 0;
		public int minimumRoomCount = 0;
		public int roomMargin = 0;
		public int roomMarginTemp = 0;

		//tile types for ease
		public static List<int> roomsandfloors = new List<int> { 1, 3 };
		public static List<int> corners = new List<int> {4,5,6,7};

		public void Generate() {
			int room_count = Random.Range(this.minimumRoomCount, this.maximumRoomCount);
			int min_size = this.min_size;
			int max_size = this.max_size;

			map_size = (maximumRoomCount + roomMargin) * max_size ;
			map = new MapTile[map_size, map_size];

			for (var x = 0; x < map_size; x++) {
				for (var y = 0; y < map_size; y++) {
					map[x,y] = new MapTile();
					map[x,y].type = 0;
				}
			}
			rooms = new List<Room> ();

			this.roomMarginTemp = this.roomMargin;
			for (var i = 0; i < room_count; i++) {

				Room room = new Room(); 
				if(rooms.Count == 0) {
					room.x = Random.Range(10,20);
					room.y = Random.Range(10,20);
					room.w = Random.Range(min_size, max_size);
					room.h = Random.Range(min_size, max_size);
				}
				else{
					Room lastRoom = rooms[rooms.Count -1];

					if (lastRoom.x > 1 + max_size) {
						room.x = Random.Range(1  , lastRoom.x + lastRoom.w  + this.roomMargin);
					}
					else {
						room.x = Random.Range(lastRoom.x  , lastRoom.x  + lastRoom.w + this.roomMargin);
					}

					if (lastRoom.y > 1 + max_size) {
						room.y = Random.Range(1 , lastRoom.y  + lastRoom.h + this.roomMargin);						
					}
					else {
						room.y = Random.Range(lastRoom.y , lastRoom.y  + lastRoom.h + this.roomMargin);
					}

					room.w = Random.Range(min_size, max_size);
					room.h = Random.Range(min_size, max_size);
				}

				bool doesCollide =this.DoesCollide(room,0);

				if (doesCollide) {
					i--;
					this.roomMargin += 1;
				}
				else {
					this.roomMargin = roomMarginTemp;
					room.w--;
					room.h--;

					rooms.Add(room);
				}
			}

			//corridor making
			for (int i = 0; i < rooms.Count; i++) {
				Room roomA = rooms[i];
				Room roomB = null;
				//Room roomB = FindClosestRoom(roomA);
				if(i > 0) {
					roomB = rooms[i - 1];
				}

				if (roomB != null) {
					var pointA = new Room();
					pointA.x = Random.Range(roomA.x, roomA.x + roomA.w);
					pointA.y = Random.Range(roomA.y, roomA.y + roomA.h);

					var pointB = new Room();
					pointB.x = Random.Range(roomB.x, roomB.x + roomB.w);
					pointB.y = Random.Range(roomB.y, roomB.y + roomB.h);

					roomA.connectedTo = roomB;

					while ((pointB.x != pointA.x) || (pointB.y != pointA.y)) {
						if (pointB.x != pointA.x) {
							if (pointB.x > pointA.x) { 
								pointB.x--; 
							}
							else { 
								pointB.x++; 
							}
						} 
						else if (pointB.y != pointA.y) {
							if (pointB.y > pointA.y) { 
								pointB.y--; 
							}
							else {
								pointB.y++; 
							}
						}

						map[pointB.x,pointB.y].type = 3;
					}
				}
			}

			//room making
			for (int i = 0; i < rooms.Count; i++) {
				Room room = rooms[i];
				for (int x = room.x; x < room.x + room.w; x++) {
					for (int y = room.y; y < room.y + room.h; y++) {
						map[x,y].type = 1;
						map[x,y].room = room;

					}
				}
			}

			//walls 
			for (int x = 0; x < map_size -1  ; x++) {
				for (int y = 0; y < map_size -1 ; y++) {
					if (map[x,y].type == 0) {
						if(map[x + 1,y].type == 1 || map[x + 1,y].type == 3) { 
							map[x,y].type = 2;
						}
						if (x > 0 && (map[x - 1,y].type == 1 || map[x - 1,y].type == 3 )) {
							map[x,y].type = 2;
						}

						if (map[x,y + 1].type == 1 || map[x,y + 1].type == 3) { 
							map[x,y].type = 2;
						}

						if (y > 0 && (map[x,y - 1].type == 1 || map[x,y - 1].type == 3) ) {
							map[x,y].type = 2;
						}
					}
				}
			}

			//corners
			for (int x = 0; x < map_size -1  ; x++) {
				for (int y = 0; y < map_size -1 ; y++) {
					if (map[x,y + 1].type == 2 && map[x + 1,y].type == 2 && roomsandfloors.Contains(map[x + 1,y +1].type) ) { 
						map[x,y].type = 4;
					}
					if(y > 0){
						if (map[x + 1,y].type == 2 && map[x,y - 1].type == 2 && roomsandfloors.Contains(map[x + 1,y - 1].type) ) { 
							map[x,y].type = 5;
						}
					}
					if(x > 0){
						if (map[x - 1,y].type == 2 && map[x,y + 1].type == 2 && roomsandfloors.Contains(map[x - 1,y + 1].type) ) { 
							map[x,y].type = 7;
						}
					}
					if(x > 0 && y >0){
						if (map[x - 1,y].type == 2 && map[x,y - 1].type == 2 && roomsandfloors.Contains(map[x - 1,y - 1].type) ) { 
							map[x,y].type = 6;
						}
					} 
				}
			}

			//find far away room
			goalRoom = rooms[rooms.Count -1 ];
			if (goalRoom != null) {
				goalRoom.x = goalRoom.x + (goalRoom.w / 2);
				goalRoom.y = goalRoom.y + (goalRoom.h / 2);
			}
			//starting point
			startRoom = rooms[0];
			startRoom.x = startRoom.x + (startRoom.w / 2);
			startRoom.y = startRoom.y + (startRoom.h / 2);
		}

		private bool DoesCollide (Room room, int ignore) {
			int random_blankliness =  Random.Range(3,7);
			for (int i = 0; i < rooms.Count; i++) {
				//if (i == ignore) continue;
				var check = rooms[i];
				if (!((room.x + room.w + random_blankliness < check.x) || (room.x > check.x + check.w + random_blankliness) || (room.y + room.h + random_blankliness < check.y) || (room.y > check.y + check.h + random_blankliness))) { 
					return true;
				}
			}
			return false;
		}

		private void SquashRooms() {
			for (int i = 0; i < 10; i++) {
				for (int j = 0; j < rooms.Count; j++) {
					Room room = rooms[j];
					while (true) {
						Room old_position = new Room();

						old_position.x = room.x;
						old_position.y = room.y;

						if (room.x > 1) { 
							room.x--; 
						}
						if (room.y > 1) { 
							room.y--; 
						}
						if ((room.x == 1) && (room.y == 1)) {
							break;
						}
						if (this.DoesCollide(room, j)) {
							room.x = old_position.x;
							room.y = old_position.y;
							break;
						}
					}
				}
			}
		}

		private float lineDistance( Room point1, Room point2 ) {
			var xs = 0;
			var ys = 0;

			xs = point2.x - point1.x;
			xs = xs * xs;

			ys = point2.y - point1.y;
			ys = ys * ys;

			return Mathf.Sqrt( xs + ys );
		}
	}

	public void ClearOldDungeon(bool immediate = false) {
		int childs = transform.childCount;
		for (var i = childs - 1; i >= 0; i--) {
			if(immediate) {
				DestroyImmediate(transform.GetChild(i).gameObject);
			}
			else {
				Destroy(transform.GetChild(i).gameObject);
			}
		}
	}

	public void Generate() {
		Dungeon dungeon = new Dungeon ();

		dungeon.min_size = minRoomSize;
		dungeon.max_size = maxRoomSize;
		dungeon.minimumRoomCount = minimumRoomCount;
		dungeon.maximumRoomCount = maximumRoomCount;
		dungeon.roomMargin = roomMargin;

		dungeon.Generate ();

		//Dungeon.map = floodFill(Dungeon.map,1,1);

		for (var y = 0; y < Dungeon.map_size; y++) {
			for (var x = 0; x < Dungeon.map_size; x++) {
				int tile = Dungeon.map [x, y].type;
				GameObject created_tile;
				Vector3 tile_location;
				tile_location = new Vector3 (x * tileScaling, y * tileScaling, 0);

				created_tile = null;
				if (tile == 1) {
					created_tile = GameObject.Instantiate (floorPrefab, tile_location, Quaternion.identity) as GameObject;
				}

				if (tile == 2) {
					created_tile = GameObject.Instantiate (wallPrefab, tile_location, Quaternion.identity) as GameObject;
				}

				if (tile == 3) {
					created_tile = GameObject.Instantiate (floorPrefab, tile_location, Quaternion.identity) as GameObject;
				}

				if (Dungeon.corners.Contains(tile)) {
					if(cornerPrefab){
						created_tile = GameObject.Instantiate (cornerPrefab, tile_location, Quaternion.identity) as GameObject;
						if(cornerRotation) {
							created_tile.transform.Rotate(Vector3.forward  * (-90 * (tile -4)));
						}
					}
					else {
						created_tile = GameObject.Instantiate (wallPrefab, tile_location, Quaternion.identity) as GameObject;
					}
				}

				if (created_tile) {
					created_tile.transform.parent = transform;
				}
			}
		}

		GameObject end_point;
		GameObject start_point;
		end_point = GameObject.Instantiate (exitPrefab, new Vector3 (Dungeon.goalRoom.x * tileScaling, Dungeon.goalRoom.y * tileScaling, 0), Quaternion.identity) as GameObject;
		start_point = GameObject.Instantiate (startPrefab, new Vector3 (Dungeon.startRoom.x * tileScaling, Dungeon.startRoom.y * tileScaling, 0), Quaternion.identity) as GameObject;

		end_point.transform.parent = transform;
		start_point.transform.parent = transform;

		//Spawn Objects;
		List<SpawnList> spawnedObjectLocations = new List<SpawnList> ();

		//OTHERS
		for (int x = 0; x < Dungeon.map_size; x++) {
			for (int y = 0; y < Dungeon.map_size; y++) {
				//&& (Dungeon.startRoom.x < x || Dungeon.startRoom.x + Dungeon.startRoom.w > x ) && (Dungeon.startRoom.y < y || Dungeon.startRoom.y + Dungeon.startRoom.h > y )

				if (Dungeon.map [x, y].type == 1 && ((Dungeon.startRoom != Dungeon.map [x, y].room && Dungeon.goalRoom != Dungeon.map [x, y].room) || maximumRoomCount <= 3)) {
					var location = new SpawnList ();
					location.x = x;
					location.y = y;
					if (Dungeon.map [x + 1, y].type == 2 || Dungeon.map [x - 1, y].type == 2 || Dungeon.map [x, y + 1].type == 2 || Dungeon.map [x, y - 1].type == 2) {
						location.byWall = true;
					}
					if (Dungeon.map [x + 1, y].type == 3 || Dungeon.map [x - 1, y].type == 3 || Dungeon.map [x, y + 1].type == 3 || Dungeon.map [x, y - 1].type == 3) {
						location.byCorridor = true;
					}
					if (Dungeon.map [x + 1, y + 1].type == 3 || Dungeon.map [x - 1, y - 1].type == 3 || Dungeon.map [x - 1, y + 1].type == 3 || Dungeon.map [x + 1, y - 1].type == 3) {
						location.byCorridor = true;

					}
					spawnedObjectLocations.Add (location);
				} 
				else if (Dungeon.map[x, y].type == 3) {
					var location = new SpawnList ();
					location.x = x;
					location.y = y;	
					if ( (Dungeon.map [x + 1, y].type == 1 || Dungeon.map [x - 1, y].type == 1  ||  Dungeon.map [x, y + 1].type == 1 || Dungeon.map [x, y - 1].type == 1)
						&& ( (Dungeon.map [x + 1, y].type == 2 && Dungeon.map [x - 1, y].type == 2) || (Dungeon.map [x, y + 1].type == 2 && Dungeon.map [x, y - 1].type == 2) ) ) {
						location.byCorridor = true;
						location.asDoor = true;
						spawnedObjectLocations.Add (location);
					}
				}
			}
		}

		for (int i = 0; i < spawnedObjectLocations.Count; i++) {
			SpawnList temp = spawnedObjectLocations [i];
			int randomIndex = Random.Range (i, spawnedObjectLocations.Count);
			spawnedObjectLocations [i] = spawnedObjectLocations [randomIndex];
			spawnedObjectLocations [randomIndex] = temp;
		}

		int objectCountToSpawn = 0;

		//DOORS
		if (doorPrefab) {
			for (int i = 0; i < spawnedObjectLocations.Count; i++) {
				if (spawnedObjectLocations[i].asDoor){
					GameObject newObject;
					SpawnList spawnLocation = spawnedObjectLocations[i];
					newObject = GameObject.Instantiate(doorPrefab,new Vector3(spawnLocation.x * tileScaling ,spawnLocation.y * tileScaling,0),Quaternion.identity) as GameObject;

					newObject.transform.parent = transform;
					spawnedObjectLocations[i].spawnedObject = newObject; 
				}
			}
		}

		//OTHERS
		foreach (SpawnOption objectToSpawn in spawnOptions){
			objectCountToSpawn = Random.Range(objectToSpawn.minSpawnCount,objectToSpawn.maxSpawnCount);
			while (objectCountToSpawn > 0){
				for (int i = 0; i < spawnedObjectLocations.Count; i++){
					bool createHere= false;

					if (!spawnedObjectLocations[i].spawnedObject && !spawnedObjectLocations[i].byCorridor){
						if ( objectToSpawn.spawnByWall && spawnedObjectLocations[i].byWall ) {
							createHere = true;
						}
						else {
							createHere = true;
						}
					}

					if (createHere){
						SpawnList spawnLocation = spawnedObjectLocations[i];
						GameObject newObject;
						newObject = GameObject.Instantiate(objectToSpawn.gameObject,new Vector3(spawnLocation.x * tileScaling ,spawnLocation.y * tileScaling,0),Quaternion.identity) as GameObject;

						newObject.transform.parent = transform;
						spawnedObjectLocations[i].spawnedObject = newObject; 
						objectCountToSpawn--;
						break;
					}
				}
			}
		}
	}

	void Start () {
		if (generateOnLoad) {
			ClearOldDungeon();
			Generate();
		}
	}
}