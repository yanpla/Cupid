using Cupid.Buttons;
using Cupid.Roles;
using MiraAPI.Events;
using MiraAPI.Events.Vanilla.Gameplay;
using MiraAPI.Events.Vanilla.Player;
using MiraAPI.Hud;
using UnityEngine;

namespace Cupid.Events;

public static class CupidEvents
{
  [RegisterEvent]
  public static void HandleRoundStartEvent(RoundStartEvent @event)
  {
    if (@event.TriggeredByIntro) return;
    if (PlayerControl.LocalPlayer.Data.Role is not CupidRole) return;
    
    CupidRole.onRoundStart();
  }

  [RegisterEvent]
  public static void HandlePlayerDeathEvent(PlayerDeathEvent e)
  {
    if (PlayerControl.LocalPlayer.Data.Role is not CupidRole) return;

    var player = e.Player;

    // If Cupid dies: clear selection and outlines
    if (player == PlayerControl.LocalPlayer)
    {
      foreach (var p in CupidRole.SelectedPlayers)
        p.cosmetics.SetOutline(false, new Il2CppSystem.Nullable<Color>(Color.magenta));

      CupidRole.SelectedPlayers.Clear();
      return;
    }

    // If a selected target dies: unselect and give the button back a charge
    if (!CupidRole.SelectedPlayers.Contains(player)) return;

    player.cosmetics.SetOutline(false, new Il2CppSystem.Nullable<Color>(Color.magenta));
    CupidRole.SelectedPlayers.Remove(player);

    var btn = CustomButtonSingleton<CupidShootButton>.Instance;
    btn.IncreaseUses();
    btn.DecreaseTimer(5f);
  }
}