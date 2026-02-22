using UnityEngine;
using TMPro;
using System.Collections.Generic;
using System.Linq;

public class RowQuestionManager : MonoBehaviour
{
    public PlatformLogic[] platforms;
    public TextMeshPro[] answerLabels;
    public string[] answerTexts;
    public string correctAnswer;

    // 👇 static = shared antar baris
    private static int lastCorrectIndex = -1;

    void Start()
    {
        SetupAnswers();
    }

    void SetupAnswers()
    {
        // 1️⃣ tentukan index jawaban benar untuk baris ini (0/1/2)
        int newCorrectIndex;

        if (lastCorrectIndex == -1)   // baris pertama, random bebas
        {
            newCorrectIndex = Random.Range(0, platforms.Length);
        }
        else
        {
            List<int> allowed = new List<int>();

            if (lastCorrectIndex == 0) allowed.AddRange(new int[] { 0, 1 });
            if (lastCorrectIndex == 1) allowed.AddRange(new int[] { 1, 0});
            

            newCorrectIndex = allowed[Random.Range(0, allowed.Count)];
        }

        lastCorrectIndex = newCorrectIndex;

        // 2️⃣ acak urutan jawaban tanpa mengubah posisi platform benar
        List<string> remaining = answerTexts.Where(a => a != correctAnswer).ToList();
        remaining = remaining.OrderBy(x => Random.value).ToList();

        for (int i = 0; i < platforms.Length; i++)
        {
            if (i == newCorrectIndex)
            {
                answerLabels[i].text = correctAnswer;
                platforms[i].isCorrect = true;
            }
            else
            {
                string wrong = remaining[0];
                remaining.RemoveAt(0);

                answerLabels[i].text = wrong;
                platforms[i].isCorrect = false;
            }
        }
    }
}
