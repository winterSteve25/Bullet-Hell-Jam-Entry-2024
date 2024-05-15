using System;
using System.Collections.Generic;
using System.Linq;
using UnityEngine;
using Random = UnityEngine.Random;

namespace Procedural
{
    public class Layout
    {
        public readonly List<RectInt> Rectangles = new List<RectInt>();
        public readonly List<RectInt> SacredRectangles = new List<RectInt>();
        public readonly List<(PremadeRoom, RectInt)> PremadeRooms = new List<RectInt>();
        public readonly RectInt Whole;

        private int _spawn;
        private int _numVertSplit;
        private List<RectInt> _built;

        public int Spawn => Spawn;

        public Layout(int width, int height)
        {
            Whole = new RectInt(0, 0, width, height);
            Rectangles.Add(Whole);
        }

        public void SliceRandomRect(int minLength)
        {
            if (Rectangles.Count <= 0)
            {
                return;
            }

            int index = Random.Range(0, Rectangles.Count);
            SliceRect(index, minLength);
        }

        private int SliceRect(int index, int minLength)
        {
            RectInt rect = Rectangles[index];
            Rectangles.RemoveAt(index);
            RectInt newRect;

            if (rect.width > minLength * 2 && Random.Range(0, 1f) < ProbabilityToVert())
            {
                _numVertSplit++;
                int currentWidth = rect.width;
                int newWidth = Random.Range(minLength, rect.width - minLength);
                newRect = new RectInt(rect.xMin + (currentWidth - newWidth), rect.yMin, newWidth, rect.height);
                rect.width = currentWidth - newWidth;
            }
            else if (rect.height > minLength * 2)
            {
                _numVertSplit--;
                int currentHeight = rect.height;
                int newHeight = Random.Range(minLength, rect.height - minLength);
                newRect = new RectInt(rect.xMin, rect.yMin + (currentHeight - newHeight), rect.width, newHeight);
                rect.height = currentHeight - newHeight;
            }
            else
            {
                SacredRectangles.Add(rect);
                return 0;
            }

            int i = 0;

            if (CanBeSliced(newRect, minLength))
            {
                Rectangles.Add(newRect);
                i++;
            }
            else
            {
                SacredRectangles.Add(newRect);
            }

            if (CanBeSliced(rect, minLength))
            {
                Rectangles.Add(rect);
                i++;
            }
            else
            {
                SacredRectangles.Add(rect);
            }

            return i;
        }

        private float ProbabilityToVert()
        {
            return Mathf.Pow(2, -(_numVertSplit + 1));
        }

        private bool CanBeSliced(RectInt rect, int minLength)
        {
            return rect.width >= minLength * 2 || rect.height >= minLength * 2;
        }

        public List<RectInt> Build(int minGapMargin, int gapMargin, int minRoomLength, int maxRoomLength)
        {
            if (_built != null) return _built;

            List<RectInt> rooms = new List<RectInt>();

            for (int i = 0; i < Rectangles.Count; i++)
            {
                RectInt rect = Rectangles[i];
                if (rect.width < maxRoomLength && rect.height < maxRoomLength)
                {
                    continue;
                }

                int im = SliceRect(i, minRoomLength);

                if (im > 0)
                {
                    i--;
                }
            }

            foreach (var rect in SacredRectangles.Concat(Rectangles))
            {
                RectInt r = rect;

                r.width -= Random.Range(minGapMargin, Mathf.Min(gapMargin, r.width));
                r.height -= Random.Range(minGapMargin, Mathf.Min(gapMargin, r.height));

                rooms.Add(r);
            }

            _spawn = Random.Range(0, rooms.Count);
            _built = rooms;

            return rooms;
        }

        public Direction RectIsClosestToCorner(int i)
        {
            if (_built == null)
            {
                throw new ArgumentException("Layout has not been built");
            }

            RectInt r = _built[i];
            bool x = IsCloserToFirst(r.x, Whole.x, Whole.width);
            bool y = IsCloserToFirst(r.y, Whole.y, Whole.height);

            return (x, y) switch
            {
                (true, true) => Direction.Left,
                (false, true) => Direction.Down,
                (true, false) => Direction.Up,
                (false, false) => Direction.Right
            };
        }

        private bool IsCloserToFirst(int number, int option1, int option2)
        {
            int distanceToOption1 = Mathf.Abs(number - option1);
            int distanceToOption2 = Mathf.Abs(number - option2);

            if (distanceToOption1 < distanceToOption2)
            {
                return true;
            }
            else
            {
                return false;
            }
        }
    }
}
