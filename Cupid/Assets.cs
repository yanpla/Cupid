using MiraAPI.Utilities.Assets;

namespace Cupid;

public static class Assets
{
    public static LoadableResourceAsset CupidRoleBanner { get; } = 
        new("Cupid.Resources.CupidBanner.png");
    public static LoadableResourceAsset CupidShootButton { get; } = 
        new("Cupid.Resources.CupidShootButton.png");
}