using ShellingModel.AbstractClasses;
using ShellingModel.Enums;

namespace ShellingModel.Objects
{
    [Serializable]
    internal class X : ShellingObject
    {
        public X(decimal discomfortability)
            : base(discomfortability)
        {
            Type = TypeEnum.X;

            Id = Guid.NewGuid();

            Array values = Enum.GetValues(typeof(TypeEnum));
            badTypes = values.Cast<TypeEnum>().ToList()
                                        .Where(v => v != Type)
                                        .Where(v => v != TypeEnum.Blank)
                                        .ToList();
        }
    }
}
