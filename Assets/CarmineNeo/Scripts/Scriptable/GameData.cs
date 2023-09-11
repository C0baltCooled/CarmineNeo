using UnityEngine;

namespace Harpnet.Carmine {
    public class GameData : MonoBehaviour {
        
    }

    public struct GameOptions {
        bool twoDMenu;
        bool clickTab;
        bool menuFadeIn;
        bool showScoreboardAtDeath;
        bool blood;
        bool ragdollDeaths;
        bool keepAfterRespawn;
        bool hideDeadPlayers;
        int ragdollVelocity;
        // Language
    }
}