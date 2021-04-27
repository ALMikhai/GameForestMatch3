namespace GameForestMatch3.Tiles
{
    public class Cake : Tile
    {
        public override TileType Type => TileType.Cake;
        public override int Prize => 200;
        public override string ContentName => "Cake";
    }
}