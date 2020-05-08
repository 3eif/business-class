using System;
using System.Collections;
using System.Collections.Generic;
namespace AssemblyCSharp.Assets.Scripts
{
    // Stores questions and answers
    public class QA
    {
        public string question;
        public string[] answers;
        public int correctAnswer;

        public QA(string q, string[] a, int ca)
        {
            question = q;
            answers = a;
            correctAnswer = ca;
        }

        public override string ToString() {
            string str = question;
            for (int i = 0; i < answers.Length; i++) {
                str += "\n" + answers[i] + ((correctAnswer == i) ? " ***" : "");
            }
            return str;
        }
    }
}
