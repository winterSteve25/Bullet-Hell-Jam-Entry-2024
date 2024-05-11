using UnityEngine;

namespace Elements
{
    public static class Reaction
    {
        public static void React(ElementObject a, ElementObject b)
        {
            if (b.ReactionPriority > a.ReactionPriority) return;
            
            if (a.element.IsEmpty())
            {
                a.element.Set(b.element);
                b.element.Empty();
            } else if (b.element.IsEmpty())
            {
                b.element.Set(a.element);
                a.element.Empty();
            }

            ReactInternal(b.element, a);
            Debug.Log("A: " + a);
            Debug.Log("B: " + b);
        }

        private static void ReactInternal(ElementStack a, ElementObject b)
        {
            if (b.element.element == a.element)
            {
                b.element.amount += a.amount;
                a.Empty();
            }
        }

        public static void React(ElementStack a, ElementObject b)
        {
            ReactInternal(a, b);
        }
    }
}