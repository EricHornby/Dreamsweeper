using UnityEngine;
using System.Collections;
using Newtonsoft.Json;
using System.IO;
using System.Collections.Generic;

public class JSONMapReader : MonoBehaviour {

	static int TILE_MAX = 625;
	public int mapX;
	public int mapY;

	// Use this for initialization
	void Start () {
		CreateMap(LoadJson("0-1"));
		//CreateMap(LoadJson("SampleMap2"));
	}
	
	// Update is called once per frame
	void Update () {
	
	}
	
	public MapData LoadJson(string mapName)
	{
		MapData map;
		using (StreamReader r = new StreamReader(Path.Combine(Directory.GetCurrentDirectory(),"Maps/" + mapName + ".json")))
		{
			string json = r.ReadToEnd();
			map = JsonConvert.DeserializeObject<MapData>(json);
		}
		
		return map;
	}
	
	public void CreateMap(MapData m)
	{

		for (int i = m.layers.Count-1; i >= 0; i--)
		{
			MapLayer layer = m.layers[i];

				Debug.Log("Loading layer " + layer.name);
				for (int y = 0; y < layer.height; y++)
				{
					for (int x = 0; x < layer.width; x++)
					{
						if (layer.data[x+y*layer.width] != 0)
						{
							try
							{
								GameObject tile = GetTile(layer.data[x+y*layer.width]);
								tile.transform.position = new Vector3(x*20f,y*-20f,0f);
								tile.transform.parent = transform;
							}
							catch
							{
							
							}
							
						}
						
					}
				}
			
			mapX = layer.width;
			mapY = layer.height;
		}

        for (int y = 0; y < mapY; y++)
        {
            int x = -1;
            GameObject tile = GetTile(TILE_MAX + 2);
            tile.transform.position = new Vector3(x * 20f, y * -20f, 0f);
            tile.transform.parent = transform;

            x = mapX;
            tile = GetTile(TILE_MAX + 2);
            tile.transform.position = new Vector3(x * 20f, y * -20f, 0f);
            tile.transform.parent = transform;
        }
	}
	
	public GameObject GetTile(int tileID)
	{
		string folder = "";
		if (tileID < TILE_MAX)
		{
			folder = "BG";
		}
		else if (tileID > TILE_MAX && tileID < TILE_MAX*2)
		{
			folder = "G";
		}
		else if (tileID > TILE_MAX*2 && tileID < TILE_MAX*3)
		{
			folder = "M";
		}
		else if (tileID > TILE_MAX*3)
		{
			folder = "F";
		}


		//Temporary Disable Monster Spawns
		tileID = tileID % TILE_MAX;
		//Debug.Log("Instaniate " + folder + "-" + tileID);
		
		//Debug.Log("TILE ID: " + tileID);
		
		GameObject tile = Instantiate(Resources.Load("Prefabs/Tiles/" + folder + "/" + folder + "-" + tileID) as GameObject) as GameObject;
		return tile;
	}
	
	public class MapData
	{
		public int height;
		public List<MapLayer> layers;
	}
	
	public class MapLayer
	{
		public int[] data;
		public int height;
		public int width;
		public string name;
		public float opacity;
		public string type;
	}
}
