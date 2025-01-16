using ShellingModel.AbstractClasses;
using ShellingModel.Enums;

namespace ShellingModel.Objects
{
    [Serializable]
    internal class Blank : ShellingObject
    {
       // private readonly Random rand;
        public Blank(decimal discomfortability)
            : base(discomfortability)
        {
            Type = TypeEnum.Blank;
            Id = Guid.NewGuid();
         //   rand = new Random();
        }

        internal override string DisplayValue => "_";

        //internal override bool HappyHere() => true; // rand.Next() > (Int32.MaxValue / 2);
    }
}
