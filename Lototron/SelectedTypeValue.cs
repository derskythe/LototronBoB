using Lototron.Containers;

namespace Lototron
{
    public class SelectedTypeValue
    {
        public SelectedType Type { get; set; } public string Value { get; set; }

        public SelectedTypeValue(SelectedType type, string value)
        {
            Type = type;
            Value = value;
        }
    }
}
