﻿using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.UI;
using System.Web.UI.WebControls;
using System.Data;
using System.Data.SqlClient;
using System.Configuration;
using Microsoft.Ajax.Utilities;
using IWinBO;
using IWinBLL;


namespace IWin.UI
{
    public partial class ad_addstore : System.Web.UI.Page
    {
        int Result = 0;
        protected void Page_Load(object sender, EventArgs e)
        {

        }

        protected void btn_saveSeller_Click(object sender, EventArgs e)
        {
            //Session["name"]=
            try
            {
                IWinBO.Seller se = new IWinBO.Seller();
                se.SellerName = txtSName.Text;
                se.SellerImage = txtSImage.Text;
                se.SellerProvince = Convert.ToInt32(txtSPro.Text);
                se.SellerAddr = txtSAddr.Text;
                se.SellerPin = Convert.ToInt32(txtSPin.Text);
                se.SellerContact = txtSContact.Text;
                se.OP = 1;  //represent an insert operation 
                IWinBLL.Seller obj = new IWinBLL.Seller();
                Result = obj.insertSeller(se);
                if (Result == 2)
                {
                    lblmessage.Text = "<span class='glyphicon glyphicon-ok'></span>"+"Seller added successfully";
                    lblmessage.Visible = true;
                }

            }
            catch
            {

            }
        }
    }
}