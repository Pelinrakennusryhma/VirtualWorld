using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class QuestDatabase : MonoBehaviour
{
    public List<Quest> quests = new List<Quest>();
    private void Awake()
    {
        BuildDatabase();
    }
    public Quest GetQuest(int id)
    {
        return quests.Find(quest => quest.id == id);
    }
    void BuildDatabase()
    {
        //new Quest(id, name, category, {Askeleen kuvaus, Kuinka monta jotakin tarvtisee}
        quests = new List<Quest>
        {
            new Quest(0, "Main Quest 1", "Main Quest",
            new Dictionary<string, int>
            {
                {"Click the button 3 times", 3},
                {"Click the button 4 times", 4},
                {"Click the button", 0},
                {"Click the button 6 times", 6},
                {"Quest Complete!", 0},
            }),
            new Quest(1, "Main Quest 2", "Main Quest",
            new Dictionary<string, int>
            {
                {"Main Quest 2 First Step", 1},
                {"Main Quest 2 Second Step", 1},
            }),
            new Quest(2, "Main Quest 3", "Main Quest",
            new Dictionary<string, int>
            {
                {"Main Quest 3 First Step", 1},
                {"Main Quest 3 Second Step", 1},
            }),
            new Quest(3, "Side Quest 1", "Side Quest",
            new Dictionary<string, int>
            {
                {"Side Quest 1 First Step", 1},
                {"Side Quest 1 Second Step", 1},
            }),
            new Quest(4, "Side Quest 2", "Side Quest",
            new Dictionary<string, int>
            {
                {"Side Quest 2 First Step", 1},
                {"Side Quest 2 Second Step", 1},
            }),
        };
    }
}
