using System.Runtime.CompilerServices;
using ShellingModel.AbstractClasses;
using ShellingModel.Enums;

namespace ShellingModel.Objects
{
    [Serializable]
    internal class O : ShellingObject
    {
        public O(decimal discomfortability)
            : base(discomfortability)
        {
            Type = TypeEnum.O;

            Id = Guid.NewGuid();

            Array values = Enum.GetValues(typeof(TypeEnum));
            badTypes = values.Cast<TypeEnum>().ToList()
                                        .Where(v => v != Type)
                                        .Where(v => v != TypeEnum.Blank)
                                        .ToList();
        }
    }
}
