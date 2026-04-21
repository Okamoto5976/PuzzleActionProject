using UnityEngine;


public enum TeamNames {
    Player,
    Enemy,
}


interface ITeam
{
    public TeamNames Team { get; }

}
