using System;

namespace ClearMinimap;

internal enum MarkerKind { Place, Home, Deal, Teammate, Objective, NPC, Dealer }

internal readonly record struct MarkerVisibility(
    bool places, bool properties, bool deals, bool teammates, bool objectives, bool npcs, bool dealers)
{
    public bool allows(MarkerKind kind) => kind switch
    {
        MarkerKind.Place => places,
        MarkerKind.Home => properties,
        MarkerKind.Deal => deals,
        MarkerKind.Teammate => teammates,
        MarkerKind.Objective => objectives,
        MarkerKind.NPC => npcs,
        MarkerKind.Dealer => dealers,
        _ => throw new ArgumentOutOfRangeException(nameof(kind))
    };
}
