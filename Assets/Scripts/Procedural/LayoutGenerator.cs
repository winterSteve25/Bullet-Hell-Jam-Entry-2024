using System;
using System.Collections.Generic;
using UnityEngine;

namespace Procedural
{
    public class LayoutGenerator : MonoBehaviour
    {
        [SerializeField] private int minRoomLength = 16;
        [SerializeField] private int minGap = 8;
        [SerializeField] private int gap = 12;
        [SerializeField] private int width = 200;
        [SerializeField] private int height = 200;
        [SerializeField] private int sliceCount = 50;
        [SerializeField] private float removeChance = 0.18f;

        public Layout Generate(List<PremadeRoom> premadeRooms)
        {
            Layout layout = new Layout(width, height);

            for (int i = 0; i < sliceCount; i++)
            {
                layout.SliceRandomRect(minRoomLength + minGap);
            }

            retry:
            try
            {
                layout.Build(minGap, gap, removeChance, premadeRooms);
            }
            catch (Exception)
            {
                // dont flame me its actually optimal
                goto retry;
            }

            return layout;
        }
    }
}
