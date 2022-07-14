using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;

public class ObjectiveManager : MonoBehaviour
{
    public List<Objective> Objectives;
    public bool LevelComplete = false;

    private void Awake()
    {
        foreach (var objective in Objectives)
            objective.CheckObjectives += CheckObjectives;
    }

    public void CheckObjectives()
    {
        LevelComplete = AreAllObjectivesCollected();

        if (LevelComplete)
            SaveAndLoadProgress.SaveGame();
    }

    public bool AreAllObjectivesCollected()
    {
        foreach (var objective in Objectives) 
            if (!objective.IsCollected)
                return false;

        return true;
    }
}
