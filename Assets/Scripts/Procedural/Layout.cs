using System.Collections.Generic;
using System.Linq;
using UnityEngine;

namespace Procedural
{
    public class Layout
    {
        public readonly List<RectInt> Rectangles = new List<RectInt>();
        public readonly List<RectInt> SacredRectangles = new List<RectInt>();

        private int numVertSplit;

        public Layout(int width, int height)
        {
            Rectangles.Add(new RectInt(0, 0, width, height));
        }

        public void SliceRandomRect(int minLength)
        {
            if (Rectangles.Count <= 0)
            {
                return;
            }

            int index = Random.Range(0, Rectangles.Count);
            RectInt rect = Rectangles[index];
            Rectangles.RemoveAt(index);
            RectInt newRect;

            if (Random.Range(0, 1f) < ProbabilityToVert())
            {
                numVertSplit++;
                int currentWidth = rect.width;
                int newWidth = Random.Range(minLength, rect.width - minLength);
                newRect = new RectInt(rect.xMin + (currentWidth - newWidth), rect.yMin, newWidth, rect.height);
                rect.width = currentWidth - newWidth;
            }
            else
            {
                numVertSplit--;
                int currentHeight = rect.height;
                int newHeight = Random.Range(minLength, rect.height - minLength);
                newRect = new RectInt(rect.xMin, rect.yMin + (currentHeight - newHeight), rect.width, newHeight);
                rect.height = currentHeight - newHeight;
            }

            if (CanBeSliced(newRect, minLength))
            {
                Rectangles.Add(newRect);
            }
            else
            {
                SacredRectangles.Add(newRect);
            }

            if (CanBeSliced(rect, minLength))
            {
                Rectangles.Add(rect);
            }
            else
            {
                SacredRectangles.Add(rect);
            }
        }

        private float ProbabilityToVert()
        {
            return Mathf.Pow(2, -(numVertSplit + 1));
        }

        private bool CanBeSliced(RectInt rect, int minLength)
        {
            return rect.width >= minLength * 2 && rect.height >= minLength * 2;
        }

        public List<RectInt> Complete()
        {
            List<RectInt> rooms = new List<RectInt>();
            RectInt? first = null;

            foreach (var rect in SacredRectangles.Concat(Rectangles))
            {
                RectInt r = rect;

                if (first == null)
                {
                    first = r;
                    rooms.Add(r);
                    continue;
                }

                rooms.Add(r);
            }

            return rooms;
        }
    }
}
