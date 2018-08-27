using System;
namespace Naxam.Controls.Mapbox.Forms
{
    public enum OfflinePackState
    {
        // It is unknown whether the pack is inactive, active, or complete.
        // This is the initial state of a pack.
        // An invalid pack always has a state of Invalid
        Unknown,

        // The pack is incomplete and is not currently downloading.
        Inactive,

        // The pack is incomplete and is currently downloading.
        Active,

        // The pack has downloaded to completion.
        Completed,

        // The pack has been removed
        Invalid
    }
}
