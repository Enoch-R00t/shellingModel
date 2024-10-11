namespace ShellingModel
{
    internal class X : ShellingObject
    {
        public X(decimal discomfortability, short xLoc, short yLoc, ref ShellingGrid shellingGrid) 
            : base(discomfortability, xLoc, yLoc, ref shellingGrid)
        {
            this.Type = TypeEnum.X;

            Array values = Enum.GetValues(typeof(TypeEnum));
            badTypes = values.Cast<TypeEnum>().ToList()
                                        .Where(v => v != Type)
                                        .Where(v => v != TypeEnum.Blank)
                                        .ToList();
        }
    }
}
