using UnityEngine;

namespace Assets.script
{
    public class Map : MonoBehaviour
    {
        public Vector3Int GridSize;
        public GameObject Wall;
        public GameObject player;
        public GameObject EmenyTank;
       
    
    
        // Start is called before the first frame update
        void Start()
        {
            AddWall();
            transform.position = new Vector3Int(GridSize.x / 2, 0, GridSize.z / 2);
      //      Instantiate(player, new Vector3Int(GridSize.x / 2, 0, GridSize.z / 2), Quaternion.identity);
      //    Instantiate(EmenyTank, new Vector3Int(GridSize.x / 2, 1, GridSize.z / 2), Quaternion.identity);
            transform.localScale = new Vector3(GridSize.x/10 , 1 , GridSize.z/10);
        }

        // Update is called once per frame
        void Update()
        {


        }

        public void AddWall() //Set a frame for the game
        {
            for (int x = 0; x < GridSize.x; x++)
            {
               Instantiate(Wall, new Vector3(x, 0, 0), Quaternion.identity);
               Instantiate(Wall, new Vector3(x, 0, GridSize.x - 1), Quaternion.identity);
            }
            for (int y = 0; y < GridSize.z; y++)
            {
                Instantiate(Wall, new Vector3(0, 0, y), Quaternion.identity);
                Instantiate(Wall, new Vector3(GridSize.z -1, 0, y), Quaternion.identity);
            }
        }






    }
}
