using Cupid.Roles;
using MiraAPI.Hud;
using MiraAPI.Utilities;
using MiraAPI.Utilities.Assets;
using UnityEngine;

namespace Cupid.Buttons;

public class CupidShootButton : CustomActionButton<PlayerControl>
{
    public override string Name => "Shoot Arrow";
    public override float Cooldown => 10f;
    public override float EffectDuration => 0f;
    public override int MaxUses => 2;
    public override LoadableAsset<Sprite> Sprite => Assets.CupidShootButton;

    public override bool Enabled(RoleBehaviour? role)
    {
        return role is CupidRole && CupidRole.SelectedPlayers.Count < 2;
    }

    protected override void OnClick()
    {
        if (Target == null || CupidRole.SelectedPlayers.Contains(Target))
            return;

        CupidRole.SelectedPlayers.Add(Target);
        Target.cosmetics.SetOutline(true, new Il2CppSystem.Nullable<Color>(Color.magenta));
    }

    public override PlayerControl? GetTarget()
    {
        return PlayerControl.LocalPlayer.GetClosestPlayer(true, Distance);
    }

    public override void SetOutline(bool active)
    {
        Target?.cosmetics.SetOutline(active, new Il2CppSystem.Nullable<Color>(Color.magenta));
    }

    public override bool IsTargetValid(PlayerControl? target)
    {
        return target != null && target != PlayerControl.LocalPlayer && !CupidRole.SelectedPlayers.Contains(target);
    }
    
}