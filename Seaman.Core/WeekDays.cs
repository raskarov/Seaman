using System;

namespace Seaman.Core
{
    [Flags]
    public enum WeekDays
    {
        Sunday = 1,
        Monday = 1 << 1,
        Tuesday = 1 << 2,
        Wednesday = 1 << 3,
        Thursday = 1 << 4,
        Friday = 1 << 5,
        Saturday = 1 << 6,
        All = 0x7F,
        Working = Monday | Tuesday | Wednesday | Thursday | Friday
    }
}