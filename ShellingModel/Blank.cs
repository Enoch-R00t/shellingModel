namespace ShellingModel
{
    internal class Blank : ShellingObject
    {
        public Blank(decimal discomfortability, short xLoc, short yLoc, ref ShellingGrid shellingGrid)
            : base(discomfortability, xLoc, yLoc, ref shellingGrid)
        {
            this.Type = TypeEnum.Blank;
        }

        internal override string DisplayValue => " ";

        internal override bool HappyHere() => true;
    }
}
