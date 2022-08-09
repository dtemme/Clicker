using System;

namespace Clicker.Shared
{
    public class CounterInfo
    {
        public Guid Id { get; set; } = Guid.NewGuid();
        public string Name { get; set; }
        public int Value { get; set; }
    }
}
