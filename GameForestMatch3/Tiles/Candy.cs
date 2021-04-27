namespace GameForestMatch3.Tiles
{
    public class Candy : Tile
    {
        public override TileType Type => TileType.Candy;
        public override int Prize => 100;
        public override string ContentName => "Candy";
    }
}