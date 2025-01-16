
using ShellingModel.AbstractClasses;
using ShellingModel.Enums;

namespace ShellingModel.Objects
{
    [Serializable]
    internal class Y : ShellingObject
    {
        public Y(decimal discomfortability)
            : base(discomfortability)
        {
            this.Type = TypeEnum.Y;

            Id = Guid.NewGuid();

            Array values = Enum.GetValues(typeof(TypeEnum));
            badTypes = values.Cast<TypeEnum>().ToList()
                                        .Where(v => v != Type)
                                        .Where(v => v != TypeEnum.Blank)
                                        .ToList();
        }
    }
}
