using System;
using System.Collections.Generic;
using System.Collections;
using System.Text;

namespace Elixir_Test
{
    class Program
    {
        static void Main()
        {
            //Get Entire List of Questions
            ET_Question_List QuestionList = new ET_Question_List();
            List<KeyValuePair<int, string>> QList = new List<KeyValuePair<int, string>>();
            QList = QuestionList.GetAllQuestions();

            // Get name
            Console.WriteLine("Hi, what is your name?");
            string UserName = Console.ReadLine();
            //See if User is already in DB and pull ID
            ET_User User = new ET_User(UserName);

            try
            {

                if (User.UserID > 0)
                {
                    Console.WriteLine("Do you want to answer a security question?");
                    string strans2 = Console.ReadLine();
                    if (strans2.ToUpper().StartsWith("Y"))
                    {
                        //Run Answer Proc
                        AnswerProc(User, QList);
                    }
                    else
                    {
                        //Clear out current responses and run Store Proc again
                        User.DeleteUserResponses();
                        StoreProc(User, QList);
                    }
                }
                else
                {
                    //Store Flow  - User Not found so prompt to see if they want to store questions

                    Console.WriteLine("Would you like to store security questions?");
                    string storechk = Console.ReadLine();
                    if (storechk.ToUpper().StartsWith("Y"))
                    {
                        //Add User to User Table
                        User.UserID = User.AddUser(UserName);

                        StoreProc(User, QList);
                    }

                }
            }
            catch (System.IO.IOException e)
            {
                Console.WriteLine("Error reading from {0}. Message = {1}", e.Message);
            }
            finally
            {

                //Clear out User and start over
                User = null;
                Console.Clear();

                Main();
            }
        }

        static void StoreProc(ET_User User, List<KeyValuePair<int, string>> QList)
        {

            int ans = 0;

            do              //Loop until you get 3 Answers
            {
                for (int i = 0; i < QList.Count; i++)              //Loop through each of the questions
                {
                    if (QList[i].Key.ToString() != null)
                    {
                        Console.WriteLine(QList[i].Value);
                        string strAns = Console.ReadLine();

                        //If they answer then log to DB
                        if (!string.IsNullOrEmpty(strAns))
                        {
                            ET_UserResponse ER = new ET_UserResponse();
                            ER = User.AddUserResponse(QList[i].Key, strAns);

                            //Remove question so they won't be prompted again
                            QList.RemoveAt(i);
                            ans += 1;
                            if (ans == 3) { break; }
                        }
                    }
                }

            } while (ans < 3);
        }

        static void AnswerProc(ET_User User, List<KeyValuePair<int, string>> QList)
        {
            //Answer Flow  - User found so need to answer questions
            //Get responses for name entered
            User.GetUserResponses(User.UserID);

            //Loop through and get responses answered
            string strAns = "";
            bool GoodAnswer = false;
            for (int i = 0; i < User.UserResponses.Count; i++)
            {
                Console.WriteLine(QList[User.UserResponses[i].QuestionID - 1].Value);
                strAns = Console.ReadLine();

                if (strAns == User.UserResponses[i].Response.ToString())
                {
                    GoodAnswer = true;
                    //kick out if they answer correctly
                    break;
                }
            }
            //Display message on success or failure
            string strmsg = "";
            if (GoodAnswer)
            {
                strmsg = "Congratulations, you answered the challenge question correctly";
            }
            else
            {
                strmsg = "You have run out of challenge questions";
            }
            Console.WriteLine(strmsg);
            strAns = Console.ReadLine();
        }

    }
}
