using UnityEngine;

namespace Elements
{
    public static class Reaction
    {
        public static void React(ElementObject a, ElementObject b)
        {
            if (b.ReactionPriority > a.ReactionPriority) return;
            bool can = true;
            
            if (a.element.IsEmpty())
            {
                a.element.Set(b.element);
                b.element.Empty();
                can = false;
            }

            React(a.element, b, can);
            Debug.Log("A: " + a);
            Debug.Log("B: " + b);
        }

        public static void React(ElementStack a, ElementObject b, bool f = true)
        {
            if (f && b.element.IsEmpty())
            {
                b.element.Set(a);
                a.Empty();
            }
        }
    }
}