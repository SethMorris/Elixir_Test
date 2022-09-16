using System;
using System.Collections.Generic;
using System.Data;
using System.Data.SqlClient;
using System.Data.SqlTypes;
using System.Text;
using System.Threading.Tasks;
using System.Collections;

namespace Elixir_Test
{
    static class Elixir_Test_DAL
    {
        //Will need to change this to where it is loaded to
        static string constring = "Data Source=(LocalDB)\\MSSQLLocalDB;AttachDbFilename=C:\\Users\\sethm\\OneDrive\\Documents\\Seth\\Work\\Elixir_Test\\Elixir_DB.mdf;Integrated Security=True";

        //CRUD Operations

        //Get User
        public static int GetUser(string UserName)
        {
            using (SqlConnection con = new SqlConnection(constring))
            {
                string strSQL = "Select ID from [User] where UserName='" + UserName + "'";

                using (SqlCommand cmd = new SqlCommand(strSQL, con))
                {
                    
                    con.Open();
                    var newID = cmd.ExecuteScalar();
                    newID = newID ?? 0;


                    if (con.State == System.Data.ConnectionState.Open) con.Close();
                    return Int32.Parse(newID.ToString());
                    
                }
            }

        }

        //Get Questions
        public static List<KeyValuePair<int, string>> GetQuestions()
        {
            string strSQL = "Select Id,QuestionValue from Question";
            DataSet ds = GetDataSet(strSQL, constring);
            return DS_To_List(ds);
        }


        //Get User Responses
        public static DataSet GetUserResponses(int UserID)
        {
            string strSQL = "Select ua.Id, UserID, QuestionID, Response, q.QuestionValue from UserAnswer as ua inner join Question as q on ua.QuestionID = q.id where UserID = " + UserID.ToString();
            DataSet ds = GetDataSet(strSQL, constring);
            return ds;
        }
        //Add User
        public static int AddUser(string UserName)
        {
            using (SqlConnection con = new SqlConnection(constring))
            {
                string strSQL = "INSERT INTO [User](UserName) VALUES(@UserName); SELECT CAST(scope_identity() AS int)";

                using (SqlCommand cmd = new SqlCommand(strSQL, con))
                {
                    cmd.Parameters.AddWithValue("@UserName", UserName);

                    con.Open();
                    var newID = cmd.ExecuteScalar();
                    newID = newID ?? 0;

                    if (con.State == System.Data.ConnectionState.Open) con.Close();
                    return Int32.Parse(newID.ToString());
                }
            }

        }

        //Add User Responses
        public static int AddUserResponse(int UserID, int QuestionID, string UserResponse)
        {
            using (SqlConnection con = new SqlConnection(constring))
            {
                string strSQL = "INSERT INTO UserAnswer(UserID, QuestionID, Response) VALUES(@UserID, @QuestionID, @UserResponse); SELECT CAST(scope_identity() AS int)";

                using (SqlCommand cmd = new SqlCommand(strSQL, con))
                {
                    cmd.Parameters.AddWithValue("@UserID", UserID);
                    cmd.Parameters.AddWithValue("@QuestionID", QuestionID);
                    cmd.Parameters.AddWithValue("@UserResponse", UserResponse);

                    con.Open();
                    var newID = cmd.ExecuteScalar();

                    if (con.State == System.Data.ConnectionState.Open) con.Close();
                    return Int32.Parse(newID.ToString());
                }
            }
        }

        //Delete User Responses
        public static int DeleteUserResponses(int UserID)
        {
            using (SqlConnection con = new SqlConnection(constring))
            {
                string strSQL = "Delete From UserAnswer where UserID=@UserID";

                using (SqlCommand cmd = new SqlCommand(strSQL, con))
                {
                    cmd.Parameters.AddWithValue("@UserID", UserID);
                    
                    con.Open();
                    int newID = cmd.ExecuteNonQuery();

                    if (con.State == System.Data.ConnectionState.Open) con.Close();
                    return newID;
                }
            }
        }

        //Generic function to pull dataset.
        //This should be how all SQL procs are done
        public static DataSet GetDataSet(string sqlCommand, string connectionString)
            {
                DataSet ds = new DataSet();
                using (SqlCommand cmd = new SqlCommand(
                    sqlCommand, new SqlConnection(connectionString)))
                {
                    cmd.Connection.Open();
                    DataTable table = new DataTable();
                    table.Load(cmd.ExecuteReader());
                    ds.Tables.Add(table);
                }
                return ds;
            }

        //General function to format Key Value Pair arrays
        private static List<KeyValuePair<int, string>> DS_To_List(DataSet ds)
        {
            var list = new List<KeyValuePair<int, string>>();
            foreach (DataRow dtRow in ds.Tables[0].Rows)
            {
                list.Add(new KeyValuePair<int, string>(Int32.Parse(dtRow[0].ToString()), dtRow[1].ToString()));
            }
            return list;
        }
    }
}

