namespace ShellingModel
{
    internal class T : ShellingObject
    {
        public T(decimal discomfortability, short xLoc, short yLoc, ref ShellingGrid shellingGrid) 
            : base(discomfortability, xLoc, yLoc, ref shellingGrid)
        {
            this.Type = TypeEnum.T;

            Array values = Enum.GetValues(typeof(TypeEnum));
            badTypes = values.Cast<TypeEnum>().ToList()
                                        .Where(v => v != Type)
                                        .Where(v => v != TypeEnum.Blank)
                                        .ToList();
        }
    }
}
