using System;
using System.Collections.Generic;
using System.Text;
using System.Threading.Tasks;
using System.Collections;
using System.Data;

namespace Elixir_Test
{
    class Elixir_Test_BLL
    {
    }

    //We would normally split these up in their own files but just adding all here for simplicity

    class ET_User
    {
        public int UserID { get; set; }
        public string UserName { get; set; }
        //public ET_UserResponse[] Responses { get; set; }

        public List<ET_UserResponse> UserResponses = new List<ET_UserResponse>();

        public ET_User(string UserName) 
        {
            this.UserName = UserName;
            this.UserID = GetUserID();
        }

        //Get User
        public int GetUserID()
        {
            return Elixir_Test_DAL.GetUser(this.UserName);
            
        }

        //Add User
        public int AddUser (string Username)
        {
            int AddUser = Elixir_Test_DAL.AddUser(Username);
            this.UserID = AddUser;
            return AddUser;

        }
        
        public void GetUserResponses(int UserID)
        {
            //normally don't pull dataset in, use object
            DataSet ds = Elixir_Test_DAL.GetUserResponses(UserID);
            foreach (DataRow dtRow in ds.Tables[0].Rows)
            {
                AddResponseToUser(new ET_UserResponse(Int32.Parse(dtRow[0].ToString()), Int32.Parse(dtRow[1].ToString()), Int32.Parse(dtRow[2].ToString()), dtRow[3].ToString()));
            }
        }

        public void AddResponseToUser(ET_UserResponse UserResponse)
        {
            this.UserResponses.Add(UserResponse);
        }


        //Add User Response
        public ET_UserResponse AddUserResponse(int QuestionID, string Response)
        {
            int rtn = Elixir_Test_DAL.AddUserResponse(this.UserID, QuestionID, Response);
            ET_UserResponse er = new ET_UserResponse(rtn, this.UserID, QuestionID, Response);

            if (rtn > 0)
            {
                AddResponseToUser(er);
            }
            return er;
        }
        public void DeleteUserResponses()
        {
            int rtn = Elixir_Test_DAL.DeleteUserResponses(this.UserID);
        }

    }

    public class ET_Question_List
    {
        
        //Get Question array
        public List<KeyValuePair<int, string>> GetAllQuestions()
        {
            return Elixir_Test_DAL.GetQuestions();

        }

    }

    class ET_UserResponse
    {
        public int UserResponseID { get; set; }
        public int UserID { get; set; }
        public int QuestionID { get; set; }
        public string Response { get; set; }

        public ET_UserResponse()
        {
        }
        public ET_UserResponse(int UserResponseID,int UserID, int QuestionID, string Response)
        {
            this.UserResponseID = UserResponseID;
            this.UserID = UserID;
            this.QuestionID = QuestionID;
            this.Response = Response;
        }
    }
}
