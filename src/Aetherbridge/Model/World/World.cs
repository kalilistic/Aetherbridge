namespace ACT_FFXIV_Aetherbridge
{
    public class World : IWorld
    {
        public World()
        {
        }

        public World(int id, string name)
        {
            Id = id;
            Name = name;
        }

        public int Id { get; set; }
        public string Name { get; set; }

        public override string ToString()
        {
            return Name + "(" + Id + ")";
        }
    }
}