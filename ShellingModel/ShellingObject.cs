using ShellingModel;

namespace ShellingModel
{
    internal abstract class ShellingObject
    {
        internal TypeEnum Type;
        internal decimal Discomfortability;
        internal short xLoc;
        internal short yLoc;
        internal ShellingGrid shellingGrid;
        internal List<TypeEnum> badTypes;

        protected ShellingObject(decimal discomfortability, short xLoc, short yLoc, ref ShellingGrid shellingGrid)
        {
            Discomfortability = discomfortability;
            this.xLoc = xLoc;
            this.yLoc = yLoc;
            this.shellingGrid = shellingGrid;
        }

        internal virtual string DisplayValue => this.Type.ToString();

        internal bool TryMove()
        {
            short x = xLoc;
            short y = yLoc;

            // start where we are and move to the right and down
            while (y < shellingGrid.GridHeight)
            {
                while (x < shellingGrid.GridWidth)
                {
                    if (shellingGrid.Grid[y, x].Type == TypeEnum.Blank)
                    {
                        if (Search(x, y))
                        {
                            // Console.WriteLine($"Good here: {y},{x}");
                            // Console.WriteLine($"Moving {yLoc},{xLoc} to {y},{x}");

                            Swap(shellingGrid.Grid[yLoc, xLoc], yLoc, xLoc, y, x);
                            return true;
                        }
                    }

                    x++;
                }

                x = 0;
                y++;
            }
            // we have reached the bottom. Lets start at the top and search back to where we are now.
            x = 0;
            y = 0;
            while (y < shellingGrid.GridHeight)
            {
                while (x < shellingGrid.GridWidth)
                {
                    if (shellingGrid.Grid[y, x].Type == TypeEnum.Blank)
                    {
                        if (Search(x, y))
                        {
                            //   Console.WriteLine($"Good here: {y},{x}");
                            //   Console.WriteLine($"Moving {yLoc},{xLoc} to {y},{x}");

                            Swap(shellingGrid.Grid[yLoc, xLoc], yLoc, xLoc, y, x);

                            return true;
                        }
                    }

                    x++;
                }

                x = 0;
                y++;
            }

            return false;
        }

        internal bool Search(short x, short y)
        {
            short badCount = 0;

            // Console.WriteLine($"Checking blank location at: {y},{x}");

            // check above left
            //Console.WriteLine($"Checking above left: {y - 1},{x - 1}");
            if (y > 0 && x > 0)
            {
                //Console.WriteLine($"{y - 1},{x - 1},{shellingGrid.Grid[y - 1, x - 1].DisplayValue}");

                if (badTypes.Contains(shellingGrid.Grid[y - 1, x - 1].Type))
                {
                    // Console.WriteLine("no good");
                    badCount++;
                }
            }
            else
            {
                // Console.WriteLine("Not Found");
            }

            // check above
            // Console.WriteLine($"Checking above: {y - 1},{x}");
            if (y > 0)
            {
                //Console.WriteLine($"{y - 1},{x},{shellingGrid.Grid[y - 1, x].DisplayValue}");

                if (badTypes.Contains(shellingGrid.Grid[y - 1, x].Type))
                {
                    // Console.WriteLine("no good");
                    badCount++;
                }
            }
            else
            {
                // Console.WriteLine("Not Found");
            }

            // check above right
            //Console.WriteLine($"Checking above right: {y - 1},{x + 1}");
            if (y > 0 && y < shellingGrid.GridHeight && x < shellingGrid.GridWidth - 1)
            {
                //Console.WriteLine($"{y - 1},{x + 1},{shellingGrid.Grid[y - 1, x + 1].DisplayValue}");

                if (badTypes.Contains(shellingGrid.Grid[y - 1, x + 1].Type))
                {
                    // Console.WriteLine("no good");
                    badCount++;
                }
            }
            else
            {
                // Console.WriteLine("Not Found");
            }

            // check right
            // Console.WriteLine($"Checking right: {y},{x + 1}");
            if (x < shellingGrid.GridWidth - 1)
            {
                // Console.WriteLine($"{y},{x + 1},{shellingGrid.Grid[y, x + 1].DisplayValue}");

                if (badTypes.Contains(shellingGrid.Grid[y, x + 1].Type))
                {
                    // Console.WriteLine("no good");
                    badCount++;
                }
            }
            else
            {
                // Console.WriteLine("Not Found");
            }

            // check bottom right
            // Console.WriteLine($"Check bottom right: {y + 1},{x + 1}");
            if (y < shellingGrid.GridHeight - 1 && x < shellingGrid.GridHeight - 1)
            {
                //Console.WriteLine($"{y + 1},{x + 1},{shellingGrid.Grid[y + 1, x + 1].DisplayValue}");

                if (badTypes.Contains(shellingGrid.Grid[y + 1, x + 1].Type))
                {
                    // Console.WriteLine("no good");
                    badCount++;
                }
            }
            else
            {
                // Console.WriteLine("Not Found");
            }

            // check bottom
            // Console.WriteLine($"Check bottom: {y + 1},{x}");
            if (y < shellingGrid.GridHeight - 1)
            {
                //Console.WriteLine($"{y + 1},{x},{shellingGrid.Grid[y + 1, x].DisplayValue}");

                if (badTypes.Contains(shellingGrid.Grid[y + 1, x].Type))
                {
                    // Console.WriteLine("no good");
                    badCount++;
                }
            }
            else
            {
                //  Console.WriteLine("Not Found");
            }

            // check bottom left
            // Console.WriteLine($"Check bottom left: {y + 1},{x - 1}");
            if (y >= 0 && y < shellingGrid.GridHeight - 1 && x >= 1)
            {
                // Console.WriteLine($"{y + 1},{x - 1},{shellingGrid.Grid[y + 1, x - 1].DisplayValue}");

                if (badTypes.Contains(shellingGrid.Grid[y + 1, x - 1].Type))
                {
                    // Console.WriteLine("no good");
                    badCount++;
                }
            }
            else
            {
                // Console.WriteLine("Not Found");
            }



            //check left
            //  Console.WriteLine($"Checking Left: {y},{x - 1}");
            if (x < shellingGrid.GridWidth && x > 0)
            {

                // Console.WriteLine($"{y},{x - 1},{shellingGrid.Grid[y, x - 1].DisplayValue}");

                if (badTypes.Contains(shellingGrid.Grid[y, x - 1].Type))
                {
                    // Console.WriteLine("no good");
                    badCount++;
                }
            }
            else
            {
                // Console.WriteLine("Not Found");
            }


            // Console.WriteLine(badCount <= Discomfortability);
            return badCount <= Discomfortability;
        }

        internal virtual bool HappyHere()
        {
            short badCount = 0;

            short x = this.xLoc;
            short y = this.yLoc;

            //Console.WriteLine($"Checking blank location at: {y},{x}");

            // check above left
            //Console.WriteLine($"Checking above left: {y - 1},{x - 1}");
            if (y > 0 && x > 0)
            {
                //Console.WriteLine($"{y - 1},{x - 1},{shellingGrid.Grid[y - 1, x - 1].DisplayValue}");

                if (shellingGrid.Grid[y - 1, x - 1].Type != this.Type && shellingGrid.Grid[y - 1, x - 1].Type != TypeEnum.Blank)
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

                if (shellingGrid.Grid[y - 1, x].Type != this.Type && shellingGrid.Grid[y - 1, x].Type != TypeEnum.Blank)
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
            if (y > 0 && y < shellingGrid.GridHeight && x < shellingGrid.GridWidth - 1)
            {
                //Console.WriteLine($"{y - 1},{x + 1},{shellingGrid.Grid[y - 1, x + 1].DisplayValue}");

                if (shellingGrid.Grid[y - 1, x + 1].Type != this.Type && shellingGrid.Grid[y - 1, x + 1].Type != TypeEnum.Blank)
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
            if (x < shellingGrid.GridWidth - 1)
            {
                //Console.WriteLine($"{y},{x + 1},{shellingGrid.Grid[y, x + 1].DisplayValue}");

                if (shellingGrid.Grid[y, x + 1].Type != this.Type && shellingGrid.Grid[y, x + 1].Type != TypeEnum.Blank)
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
            if (y < shellingGrid.GridHeight - 1 && x < shellingGrid.GridHeight - 1)
            {
                // Console.WriteLine($"{y + 1},{x + 1},{shellingGrid.Grid[y + 1, x + 1].DisplayValue}");

                if (shellingGrid.Grid[y + 1, x + 1].Type != this.Type && shellingGrid.Grid[y + 1, x + 1].Type != TypeEnum.Blank)
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
            if (y < shellingGrid.GridHeight - 1)
            {
                //Console.WriteLine($"{y + 1},{x},{shellingGrid.Grid[y + 1, x].DisplayValue}");

                if (shellingGrid.Grid[y + 1, x].Type != this.Type && shellingGrid.Grid[y + 1, x].Type != TypeEnum.Blank)
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
            if (y >= 0 && y < shellingGrid.GridHeight - 1 && x >= 1)
            {
                //Console.WriteLine($"{y + 1},{x - 1},{shellingGrid.Grid[y + 1, x - 1].DisplayValue}");

                if (shellingGrid.Grid[y + 1, x - 1].Type != this.Type && shellingGrid.Grid[y + 1, x - 1].Type != TypeEnum.Blank)
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
            if (x < shellingGrid.GridWidth && x > 0)
            {

                //Console.WriteLine($"{y},{x - 1},{shellingGrid.Grid[y, x - 1].DisplayValue}");

                if (shellingGrid.Grid[y, x - 1].Type != this.Type && shellingGrid.Grid[y, x - 1].Type != TypeEnum.Blank)
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

        internal virtual void Swap(ShellingObject obj, short fromY, short fromX, short toY, short toX)
        {
            var newBlank = new Blank(3, fromX, fromY, ref shellingGrid);

            shellingGrid.Grid[toY, toX] = obj;
            obj.yLoc = toY;
            obj.xLoc = toX;

            shellingGrid.Grid[fromY, fromX] = newBlank;
        }
    }
}
