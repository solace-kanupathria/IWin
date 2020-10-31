﻿using System;
using System.Data;
using System.Collections.Generic;
using System.Data.SqlClient;
using System.Configuration;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Runtime.InteropServices;
using System.Runtime.Remoting.Messaging;

namespace IWinDAL
{
    public class dbUtil
    {

        SqlConnection con = new SqlConnection(ConfigurationManager.ConnectionStrings["iwinConn"].ToString());
        int result;
        string val;
        public DataSet GetDataSetForSP(SqlCommand command)
        {
            DataSet ds = new DataSet();
            SqlDataAdapter da;
            try
            {
                con.Open();
                command.CommandType = CommandType.StoredProcedure;
                command.Connection = con;
                da = new SqlDataAdapter(command);
                da.Fill(ds, "Table");
                con.Close();
                return ds;
            }
            catch (Exception ex)
            {
                MsgBox("Unable to connect to Database");
                //ErrHandler.WriteError(ex.ToString());
                return ds;
            }
        }
        public void ExecuteSPint(string StoredProcedureName, out DataTable dtemp, [Optional] int val)
        {
            string constr = ConfigurationManager.ConnectionStrings["iwinConn"].ConnectionString;
            using (SqlConnection con = new SqlConnection(constr))
            {
                using (SqlCommand command = new SqlCommand())
                {
                    con.Open();
                    command.CommandType = CommandType.StoredProcedure;
                    SqlDataAdapter adp = new SqlDataAdapter();
                    DataTable dt = new DataTable();
                    command.CommandText = StoredProcedureName;
                    command.CommandTimeout = 300;
                    try
                    {
                        command.Parameters.AddWithValue("@thing", val);
                    }
                    catch (Exception ex)
                    {
                        //ErrHandler.WriteError(ex.ToString());
                        throw ex;
                    }
                    command.Connection = con;
                    adp.SelectCommand = command;
                    adp.Fill(dt);
                    con.Close();
                    dtemp = dt;
                }
            }
        }
        public DataSet GetDataSetForQuery(SqlCommand command)
        {
            DataSet ds = new DataSet();
            SqlDataAdapter da;
            try
            {
                con.Open();
                command.Connection = con;
                da = new SqlDataAdapter(command);
                da.Fill(ds, "Table");
                con.Close();
                return ds;
            }
            catch (Exception ex)
            {
                MsgBox("Unable to connect to Database");
                //ErrHandler.WriteError(ex.ToString());
                return ds;
            }
        }
        public int ExecuteQuery(String query)
        {
            DataSet ds = new DataSet();
            try
            {
                result = -1;
                if (query.Length > 0)
                {
                    SqlCommand command = new SqlCommand(query);
                    command.CommandType = CommandType.Text;
                    con.Open();
                    command.Connection = con;
                    result = command.ExecuteNonQuery();
                    con.Close();
                }
                return result;
            }
            catch (Exception ex)
            {
                MsgBox("Unable to connect to Database");
                //ErrHandler.WriteError(ex.ToString());
                return result;
            }
        }
        public void ExecuteSP(string StoredProcedureName, out DataSet dtemp, [Optional] string[,] aryParameters)
        {
            string constr = ConfigurationManager.ConnectionStrings["iwinConn"].ConnectionString;
            using (con = new SqlConnection(constr))
            {
                using (SqlCommand command = new SqlCommand())
                {
                    con.Open();
                    command.CommandType = CommandType.StoredProcedure;
                    SqlDataAdapter adp = new SqlDataAdapter();
                    DataSet dt = new DataSet();
                    command.CommandText = StoredProcedureName;
                    command.CommandTimeout = 300;
                    try
                    {
                        for (int i = 0; i < aryParameters.GetLength(0); i++)
                        {

                            command.Parameters.AddWithValue(aryParameters[i, 0], aryParameters[i, 1]);
                        }
                    }
                    catch (Exception ex)
                    {
                        //ErrHandler.WriteError(ex.ToString());
                        throw ex;
                    }
                    command.Connection = con;
                    adp.SelectCommand = command;
                    adp.Fill(dt);
                    con.Close();
                    dtemp = dt;
                    //string hh;
                    int a = 0;
                    //string an = dtemp.Tables[0].Rows[0].ToString();
                    foreach (DataRow pRow in dtemp.Tables[0].Rows)
                    {
                        a++;
                    }
                }
            }
        }
        public void ExecuteSP(string StoredProcedureName, [Optional] out int res, [Optional] string[,] aryParameters)
        {
            string constr = ConfigurationManager.ConnectionStrings["iwinConn"].ConnectionString;
            using (SqlConnection con = new SqlConnection(constr))
            {
                using (SqlCommand command = new SqlCommand())
                {
                    con.Open();
                    command.CommandType = CommandType.StoredProcedure;
                    int result = 0;
                    command.CommandText = StoredProcedureName;
                    command.CommandTimeout = 300;
                    try
                    {
                        for (int i = 0; i < aryParameters.GetLength(0); i++)
                        {

                            command.Parameters.Add(new SqlParameter(aryParameters[i, 0], aryParameters[i, 1]));
                        }
                    }
                    catch (Exception ex)
                    {
                        //ErrHandler.WriteError(ex.ToString());
                        throw ex;
                    }
                    command.Connection = con;
                    SqlDataReader reader = command.ExecuteReader();
                    reader.Read();
                    result = Convert.ToInt32(reader["status"]);
                    con.Close();
                    res = result;
                }
            }
        }
        public void ExecuteSP(string StoredProcedureName, out List<string> ds, [Optional] string[,] aryParameters)
        {
            string constr = ConfigurationManager.ConnectionStrings["iwinConn"].ConnectionString;
            using (con = new SqlConnection(constr))
            {
                using (SqlCommand command = new SqlCommand())
                {
                    con.Open();
                    command.CommandType = CommandType.StoredProcedure;
                    List<string> hh = new List<string>();
                    //SqlDataAdapter adp = new SqlDataAdapter();
                    DataSet ds2 = new DataSet();
                    command.CommandText = StoredProcedureName;
                    command.CommandTimeout = 300;
                    try
                    {
                        for (int i = 0; i < aryParameters.GetLength(0); i++)
                        {
                            command.Parameters.Add(new SqlParameter(aryParameters[i, 0], aryParameters[i, 1]));
                        }
                    }
                    catch (Exception ex)
                    {
                        //ErrHandler.WriteError(ex.ToString());
                        throw ex;
                    }
                    command.Connection = con;
                    SqlDataReader reader = command.ExecuteReader();
                    if (reader.HasRows)
                    {
                        while (reader.Read())
                        {
                            hh.Add(reader.GetString(0));
                        }
                    }
                    reader.Close();
                    ds = hh;
                    con.Close();
                }
            }
        }
        public void MsgBox(String sMessage)
        {
            string msg = "<script language=\"javascript\">";
            msg += "alert('" + sMessage + "');";
            msg += "</script>";
            System.Web.HttpContext.Current.Response.Write(msg);
        }

    }
}