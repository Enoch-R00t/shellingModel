using ShellingModel.AbstractClasses;
using ShellingModel.Enums;

namespace ShellingModel.Objects
{
    [Serializable]
    internal class T : ShellingObject
    {
        public T(decimal discomfortability)
            : base(discomfortability)
        {
            Type = TypeEnum.T;

            Id = Guid.NewGuid();

            Array values = Enum.GetValues(typeof(TypeEnum));
            badTypes = values.Cast<TypeEnum>().ToList()
                                        .Where(v => v != Type)
                                        .Where(v => v != TypeEnum.Blank)
                                        .ToList();
        }
    }
}
