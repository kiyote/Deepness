
namespace Model.Map
{
    public class MapTerrain
    {
        private readonly int _id;
        private readonly string _name;
        private readonly bool _walls;

        public MapTerrain(int id, string name, bool walls)
        {
            _id = id;
            _name = name;
            _walls = walls;
        }

        public int Id
        {
            get
            {
                return _id;
            }
        }

        public string Name
        {
            get
            {
                return _name;
            }
        }

        public bool Walls
        {
            get
            {
                return _walls;
            }
        }
    }
}

