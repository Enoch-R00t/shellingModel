using ShellingModel.AbstractClasses;
using ShellingModel.Enums;

namespace ShellingModel.Objects
{
    internal class Blank : ShellingObject
    {
        private readonly Random rand;
        public Blank(decimal discomfortability, short xLoc, short yLoc, ref ShellingGrid shellingGrid)
            : base(discomfortability, xLoc, yLoc, ref shellingGrid)
        {
            Type = TypeEnum.Blank;
            rand = new Random();
        }

        internal override string DisplayValue => "_";

        internal override bool HappyHere() => true; // rand.Next() > (Int32.MaxValue / 2);
    }
}
