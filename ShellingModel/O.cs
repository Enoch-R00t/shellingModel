using System.Runtime.CompilerServices;

namespace ShellingModel
{
    internal class O : ShellingObject
    {
        public O(decimal discomfortability, short xLoc, short yLoc, ref ShellingGrid shellingGrid)
            : base(discomfortability, xLoc, yLoc, ref shellingGrid)
        {
            this.Type = TypeEnum.O;

            Array values = Enum.GetValues(typeof(TypeEnum));
            badTypes = values.Cast<TypeEnum>().ToList()
                                        .Where(v => v != Type)
                                        .Where(v => v != TypeEnum.Blank)
                                        .ToList();
        }
    }
}
