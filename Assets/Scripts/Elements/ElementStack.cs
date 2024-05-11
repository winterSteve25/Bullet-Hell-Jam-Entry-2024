using System;

namespace Elements
{
    [Serializable]
    public sealed class ElementStack
    {
        public Element element;
        public float amount;

        public void Empty()
        {
            element = null;
            amount = 0;
        }

        public bool IsEmpty()
        {
            return element == null || amount == 0;
        }

        public void DecreasePercentage(float percentage)
        {
            amount -= percentage * amount;
            
            if (amount <= 0)
            {
                Empty();
            }
        }

        public override string ToString()
        {
            if (IsEmpty())
            {
                return "Empty";
            }
            
            return $"{element.DisplayName}: {amount}";
        }

        public void Set(ElementStack other)
        {
            element = other.element;
            amount = other.amount;
        }
    }
}