using System.Collections;
using System.Collections.Generic;

[System.Serializable]
public class PlayerInfo
{
    public string username;
    public string nickname;
    public int rank;
    public int totalMissionFought;
    public int totalMissionWon;
    public int totalDeaths;
    public int totalShotsFired;
    public int totalEnemiesKilled;
    public List<int> weapons;
    public List<int> stratagems;

    public PlayerInfo Clone()
    {
        PlayerInfo clone = new PlayerInfo();
        CopyTo(clone);
        return clone;
    }

    public void CopyTo(PlayerInfo data)
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