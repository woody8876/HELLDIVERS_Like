using System.Collections;
using System.Collections.Generic;

namespace Bryan.Character
{
    public class PlayerData
    {
        private string username;
        private string nickname;
        private int rank;
        private int totalMissionFought;
        private int totalMissionWon;
        private int totalDeaths;
        private int totalShotsFired;
        private int totalEnemiesKilled;
        private List<string> weapons;
        private List<string> stratagems;

        public PlayerData Clone()
        {
            PlayerData clone = new PlayerData();
            CopyTo(clone);
            return clone;
        }

        public void CopyTo(PlayerData data)
        {
            data.username = this.username;
            data.nickname = this.nickname;
            data.rank = this.rank;
            data.totalMissionFought = this.totalMissionFought;
            data.totalMissionWon = this.totalMissionWon;
            data.totalDeaths = this.totalDeaths;
            data.totalShotsFired = this.totalShotsFired;
            data.totalEnemiesKilled = this.totalEnemiesKilled;
            data.weapons = this.weapons;
            data.stratagems = this.stratagems;
        }
    }
}