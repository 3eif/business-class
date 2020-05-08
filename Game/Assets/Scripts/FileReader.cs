using System.Collections;
using System;
using System.IO;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

using AssemblyCSharp.Assets.Scripts;

public class FileReader : MonoBehaviour {

    private static List<QA> qaList = new List<QA>();
    private static System.Random random = new System.Random();

    public static QA getRandomQuestion() {

        // load the questions if there are none left (this happens the first time and every time all questions are exhausted)

        if (qaList.Count == 0) {
            readFile(Application.dataPath + "/Resources/QuestionsAnswers.csv");
        }


        // Retrieve and remove a question from the list
        int rand = random.Next(qaList.Count);
        QA ret = qaList[rand];
        qaList.RemoveAt(rand);
        Debug.Log(qaList.Count);
        return ret;
    }


    //reads the CSV file, in which the questions are stored
    public static void readFile(string filename) {
        StreamReader reader = new StreamReader(filename);

        //creates a string out of a line, which has all the info for a question
        string line;
        while ((line = reader.ReadLine()) != null) {

            // split the line by files
            int[] indexes = new int[7];
            indexes[0] = -1;
            indexes[6] = line.Length;
            int k = 1;
            int numQuotes = 0;
            char[] chars = line.ToCharArray();
            for (int i = 0; i < chars.Length; i++) {
                if (chars[i] == '"') {
                    numQuotes++;
                } else if (chars[i] == ',') {
                    if (numQuotes % 2 == 0) {
                        indexes[k] = i;
                        k++;
                    }
                }
            }

            // creates separate tokens from the split indexes
            string[] qa = new string[6];
            for (int i = 0; i < 6; i++) {
                //
                qa[i] = line.Substring(indexes[i] + 1, indexes[i + 1] - (indexes[i] + 1));
            }
           



            // Removes quotations from the question if it starts with "
            string question = qa[0];
            if (question.StartsWith("\"")) {
                question = question.Substring(1, question.Length - 2);
            }

            // Generates answers from the split tokens
            string[] answers = new string[4];

            for (int i = 0; i < 4; i++) {
                answers[i] = qa[i + 1];
                if (answers[i].StartsWith("\"")) {
                    answers[i] = answers[i].Substring(1, answers[i].Length - 2);
                }
            }

            // Gets correct answer from 
            int correctAns = int.Parse(qa[5]);


            // Creates question object from data
            QA qaObj = new QA(question, answers, correctAns);
            qaList.Add(qaObj);

        }
    }

    void Start() {

    }
}