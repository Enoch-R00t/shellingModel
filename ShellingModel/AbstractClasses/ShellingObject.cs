using ShellingModel.Enums;
using ShellingModel.Objects;

using System.Xml.Linq;

namespace ShellingModel.AbstractClasses
{
    [Serializable]
    internal abstract class ShellingObject : ICloneable
    {
        //internal short xLoc;
        //internal short yLoc;
        //internal ShellingGrid shellingGrid;
        internal List<TypeEnum> badTypes;
        internal TypeEnum Type;
        internal decimal Discomfortability;
        internal Guid _id;

        protected ShellingObject(decimal discomfortability)
        {
            Discomfortability = discomfortability;
            // this.xLoc = xLoc;
            // this.yLoc = yLoc;
            // this.shellingGrid = shellingGrid;
            //_id = new Guid();
        }

        internal virtual Guid Id 
        {
            get { return _id; }
            set { _id = value; }
        }

        internal virtual string DisplayValue => Type.ToString();

        public virtual byte[] ToBinaryString()
        {
            var result = new byte[6];
            return result;
        }

        public override bool Equals(object obj)
        {
            var item = obj as ShellingObject;
            return Equals(item);
        }

        protected bool Equals(ShellingObject other)
        {
            return string.Equals(Type, other.Type) &&
                Id == other.Id;
        }

        public override int GetHashCode()
        {
            return _id.GetHashCode();
        }

        internal virtual bool HappyHere(int y, int x, int GridHeight, int GridWidth, ref ShellingObject[,] grid)
        {
            short badCount = 0;


//            Console.WriteLine($"Checking blank location at: {y},{x}");

            //check above left
            //Console.WriteLine($"Checking above left: {y - 1},{x - 1}");
            if (y > 0 && x > 0)
            {
                //Console.WriteLine($"{y - 1},{x - 1},{shellingGrid.Grid[y - 1, x - 1].DisplayValue}");

                if (grid[y - 1, x - 1].Type != Type && grid[y - 1, x - 1].Type != TypeEnum.Blank)
                {
                    //Console.WriteLine("no good");
                    badCount++;
                }
            }
            else
            {
                //Console.WriteLine("Not Found");
            }

            // check above
            //Console.WriteLine($"Checking above: {y - 1},{x}");
            if (y > 0)
            {
                //Console.WriteLine($"{y - 1},{x},{shellingGrid.Grid[y - 1, x].DisplayValue}");

                if (grid[y - 1, x].Type != Type && grid[y - 1, x].Type != TypeEnum.Blank)
                {
                    //Console.WriteLine("no good");
                    badCount++;
                }
            }
            else
            {
                // Console.WriteLine("Not Found");
            }

            // check above right
            //Console.WriteLine($"Checking above right: {y - 1},{x + 1}");
            if (y > 0 && y < GridHeight && x < GridWidth - 1)
            {
                //Console.WriteLine($"{y - 1},{x + 1},{shellingGrid.Grid[y - 1, x + 1].DisplayValue}");

                if (grid[y - 1, x + 1].Type != Type && grid[y - 1, x + 1].Type != TypeEnum.Blank)
                {
                    //Console.WriteLine("no good");
                    badCount++;
                }
            }
            else
            {
                //Console.WriteLine("Not Found");
            }

            // check right
            // Console.WriteLine($"Checking right: {y},{x + 1}");
            if (x < GridWidth - 1)
            {
                //Console.WriteLine($"{y},{x + 1},{shellingGrid.Grid[y, x + 1].DisplayValue}");

                if (grid[y, x + 1].Type != Type && grid[y, x + 1].Type != TypeEnum.Blank)
                {
                    //Console.WriteLine("no good");
                    badCount++;
                }
            }
            else
            {
                // Console.WriteLine("Not Found");
            }

            // check bottom right
            // Console.WriteLine($"Check bottom right: {y + 1},{x + 1}");
            if (y < GridHeight - 1 && x < GridHeight - 1)
            {
                // Console.WriteLine($"{y + 1},{x + 1},{shellingGrid.Grid[y + 1, x + 1].DisplayValue}");

                if (grid[y + 1, x + 1].Type != Type && grid[y + 1, x + 1].Type != TypeEnum.Blank)
                {
                    // Console.WriteLine("no good");
                    badCount++;
                }
            }
            else
            {
                //   Console.WriteLine("Not Found");
            }

            // check bottom
            //Console.WriteLine($"Check bottom: {y + 1},{x}");
            if (y < GridHeight - 1)
            {
                //Console.WriteLine($"{y + 1},{x},{shellingGrid.Grid[y + 1, x].DisplayValue}");

                if (grid[y + 1, x].Type != Type && grid[y + 1, x].Type != TypeEnum.Blank)
                {
                    //Console.WriteLine("no good");
                    badCount++;
                }
            }
            else
            {
                //Console.WriteLine("Not Found");
            }

            // check bottom left
            //Console.WriteLine($"Check bottom left: {y + 1},{x - 1}");
            if (y >= 0 && y < GridHeight - 1 && x >= 1)
            {
                //Console.WriteLine($"{y + 1},{x - 1},{shellingGrid.Grid[y + 1, x - 1].DisplayValue}");

                if (grid[y + 1, x - 1].Type != Type && grid[y + 1, x - 1].Type != TypeEnum.Blank)
                {
                    //Console.WriteLine("no good");
                    badCount++;
                }
            }
            else
            {
                // Console.WriteLine("Not Found");
            }


            //check left
            // Console.WriteLine($"Checking Left: {y},{x - 1}");
            if (x < GridWidth && x > 0)
            {

                //Console.WriteLine($"{y},{x - 1},{shellingGrid.Grid[y, x - 1].DisplayValue}");

                if (grid[y, x - 1].Type != Type && grid[y, x - 1].Type != TypeEnum.Blank)
                {
                    //Console.WriteLine("no good");
                    badCount++;
                }
            }
            else
            {
                //Console.WriteLine("Not Found");
            }

            //Console.WriteLine(badCount <= Discomfortability);
            return badCount <= Discomfortability;
        }

        public object Clone()
        {
            ShellingObject clone;

            switch (Type)
            {
                case TypeEnum.T:
                    clone = new T(this.Discomfortability)
                    {
                        badTypes = this.badTypes,
                        _id = this._id
                    };
                    return clone;
                case TypeEnum.O:
                    clone = new O(this.Discomfortability)
                    {
                        badTypes = this.badTypes,
                        _id = this._id
                    };
                    return clone;
                case TypeEnum.X:
                    clone = new X(this.Discomfortability)
                    {
                        badTypes = this.badTypes,
                        _id = this._id
                    };
                    return clone;
                case TypeEnum.Y:
                    clone = new Y(this.Discomfortability)
                    {
                        badTypes = this.badTypes,
                        _id = this._id
                    };
                    return clone;

                default:
                    clone = new Blank(this.Discomfortability)
                    {
                        badTypes = this.badTypes,
                        _id = this._id
                    };
                    return clone;
            }
        }
    }
}
