﻿// Company: The Puzzlers
// Copyright (c) 2018 All Rights Reserved
// Author: Anthony Nguyen
// Date: 04/13/2018
/* Summary: 
 *
*/
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using UnityEngine.SceneManagement;
using System.Linq;
using System;
using Random = UnityEngine.Random;

public class WordPasscode : Puzzle
{

    // Sets the parent fields
    
    void Awake()
    {
        puzzleName = "WordPasscode";
        difficulty = NextSceneManager.instance.setPuzzledifficulty;
        placeholder = NextSceneManager.instance.placeholder;
        Debug.Log("Difficulty for puzzle " + puzzleName + " is: " + this.difficulty);
    }

    //list of questions
    public Questions[] questions;

    //list of unanswered questions
    private List<Questions> unansweredQuestions;

    private Questions currentQuestion;

    //variable to store random integer for question randomization
    private int randQuestionIndex;

    //counter variables
    //try making these static so they dont reset after scene is restarted
    //take the variables out from Question.cs
    private int totalQuestionsAnswered;
    private int numErrors;

    [SerializeField]
    private Text questionText;

    [SerializeField]
    private Text optionA;

    [SerializeField]
    private Text optionB;

    [SerializeField]
    private Text optionC;

    [SerializeField]
    private Text optionD;



    [SerializeField]
    private GameObject optionButtonA;

    [SerializeField]
    private GameObject optionButtonB;

    [SerializeField]
    private GameObject optionButtonC;

    [SerializeField]
    private GameObject optionButtonD;


    [SerializeField]
    private Text incorrectAnswers;

    [SerializeField]
    private Text correctAnswers;

    //sound files for correct/incorrect answer
    public AudioSource correct;
    public AudioSource incorrect;

    private void Start()
    {
        
        totalQuestionsAnswered = 1;
        //reset error counter
        numErrors = 0;

        //load all unanswered questions into unanswered question list
        if (unansweredQuestions == null || unansweredQuestions.Count == 0)
        {
            unansweredQuestions = questions.ToList<Questions>();
        }

        //get a random question -- comment out for difficulties?
        SetCurrentQuestion();
    }

    void SetCurrentQuestion()
    {
        if (difficulty == 1)
        {
            randQuestionIndex = Random.Range(0, 20);
        }
          
        if (difficulty == 2)
        {
            randQuestionIndex = Random.Range(21, 39);
        }

        if (difficulty == 3)
        {
            randQuestionIndex = Random.Range(40, 59);
        }
 
        //proper code. 
        //int randQuestionIndex = Random.Range(0, unansweredQuestions.Count);

        //set current question to randomly selected question
        currentQuestion = unansweredQuestions[randQuestionIndex];

        //display question in panel
        questionText.text = currentQuestion.question;

        //display multiple choice options
        optionA.text = currentQuestion.a;
        optionB.text = currentQuestion.b;
        optionC.text = currentQuestion.c;
        optionD.text = currentQuestion.d;

        //display number of incorrect answers
        //incorrectAnswers.text = numErrors.ToString();
    }

    //transition to next question
    void LoadNextQuestion()
    {
        correctAnswers.text = (totalQuestionsAnswered - 1).ToString();

        //4 questions to complete a difficulty
        if (totalQuestionsAnswered < 5)
        {
            //remove element -- not index
            unansweredQuestions.Remove(currentQuestion);
            SetCurrentQuestion();
        }
        else
        {
            if (numErrors == 0)
            {
                questionText.text = "Home Run!";
            }
            else {
                questionText.text = "Completed!";
            }

            optionButtonA.SetActive(false);
            optionButtonB.SetActive(false);
            optionButtonC.SetActive(false);
            optionButtonD.SetActive(false);
            //call puzzle is complete function
            Invoke("PuzzleComplete", 1.5f);
        }  
    }

    //code for users selected answer
    //option A
    public void UserSelectA()
    {
        if (currentQuestion.a == currentQuestion.answer)
        {
            correct.Play();
            Invoke("LoadNextQuestion", 0.5f);
            totalQuestionsAnswered += 1;
        } else
        {
            incorrect.Play();
            numErrors += 1;

            UpdateErrors();

            if (numErrors == 3)
            {
                manyErrors();
            }
        }
    }

    //option B
    public void UserSelectB() {

        if (currentQuestion.b == currentQuestion.answer)
        {
            correct.Play(); 
            Invoke("LoadNextQuestion", 0.5f);
            totalQuestionsAnswered += 1;
        }
        else
        {
            incorrect.Play();
            numErrors += 1;

            UpdateErrors();

            if (numErrors == 3)
            {
                manyErrors();
            }
        }
    }

    //option C
    public void UserSelectC() {

        if (currentQuestion.c == currentQuestion.answer)
        {
            correct.Play();         
            Invoke("LoadNextQuestion", 0.5f);
            totalQuestionsAnswered += 1;
        }
        else
        {
            //display number of incorrect answers
            incorrectAnswers.text = numErrors.ToString();

            incorrect.Play();
            numErrors += 1;

            UpdateErrors();

            if (numErrors == 3)
            {
                manyErrors();
            }
        }   
        
    }

    //option D
    public void UserSelectD() {

        if (currentQuestion.d == currentQuestion.answer)
        {
            correct.Play();
            Invoke("LoadNextQuestion", 0.5f);
            totalQuestionsAnswered += 1;
        }
        else
        {
            incorrect.Play();
            numErrors += 1;

            UpdateErrors();

            if (numErrors == 3)
            {
                manyErrors();
            }
        }
     }

    public void manyErrors()
    {
        questionText.text = "3 strikes -- you're out!";
        optionButtonA.SetActive(false);
        optionButtonB.SetActive(false);
        optionButtonC.SetActive(false);
        optionButtonD.SetActive(false);
        incorrectAnswers.text = "";

        Invoke("PuzzleExit", 1.5f);
    }

    public void UpdateErrors()
    {
        //display number of incorrect answers
        incorrectAnswers.text = numErrors.ToString();
    }

    }
