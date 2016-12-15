using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.UI;
using System.Web.UI.WebControls;
//using iFrameProjectExample.it.sella.ecomms2s; //PRO
using iFrameProjectExample.it.sella.testecomm; //TEST
using System.Xml;


namespace iFrameProjectExample
{
    public partial class WebForm2 : System.Web.UI.Page
    {
        protected void Page_Load(object sender, EventArgs e)
        {
            string EncString;
            string Shop_Login;
            string XMLOut;

            Shop_Login = Request.Params["a"];
            EncString = Request.Params["b"];
            if (EncString != null && Shop_Login != null)
            {
                WSCryptDecrypt objDecrypt = new WSCryptDecrypt();
                XMLOut = objDecrypt.Decrypt(Shop_Login, EncString).OuterXml;
                XmlDocument XMLReturn = new XmlDocument();
                XMLReturn.LoadXml(XMLOut.ToLower());
                Response.Write("Shop:" + Shop_Login + "<br />");
                XmlNode ThisNode = XMLReturn.SelectSingleNode("/gestpaycryptdecrypt/errorcode");
                string ErrorCode = ThisNode.InnerText;
                ThisNode = XMLReturn.SelectSingleNode("/gestpaycryptdecrypt/errordescription");
                string ErrorDesc = ThisNode.InnerText;
                ThisNode = XMLReturn.SelectSingleNode("/gestpaycryptdecrypt/authorizationcode");
                if (ErrorCode != "9999")
                {
                    string AuthCode = ThisNode.InnerText;
                    ThisNode = XMLReturn.SelectSingleNode("/gestpaycryptdecrypt/banktransactionid");
                    string BankTrxID = ThisNode.InnerText;
                    ThisNode = XMLReturn.SelectSingleNode("/gestpaycryptdecrypt/amount");
                    string Amount = ThisNode.InnerText;
                    ThisNode = XMLReturn.SelectSingleNode("/gestpaycryptdecrypt/shoptransactionid");
                    string ShopTrxID = ThisNode.InnerText;
                    ThisNode = XMLReturn.SelectSingleNode("/gestpaycryptdecrypt/transactionresult");
                    string TrxResut = ThisNode.InnerText;
                    ThisNode = XMLReturn.SelectSingleNode("/gestpaycryptdecrypt/buyer/buyername");
                    string BuyerName = ThisNode.InnerText;
                    ThisNode = XMLReturn.SelectSingleNode("/gestpaycryptdecrypt/buyer/buyeremail");
                    string BuyerEmail = ThisNode.InnerText;
                    Response.Write("ErrorCode:" + ErrorCode + "<br />ErrorDesc:" + ErrorDesc + "<br />TrxResult:" + TrxResut + "<br />BankTrxID:" + BankTrxID + "<br />AuthCode:" + AuthCode + "<br />Amount:" + Amount + "<br/>BuyerName:" + BuyerName + "<br />BuyerEmail:" + BuyerEmail + "<br />");
                }
                else
                {
                    Response.Write("ErrorCode:" + ErrorCode + "<br />ErrorDesc:" + ErrorDesc + "<br />");

                }
                // Response.Write(Server.HtmlEncode(XMLOut));

            }

        }
    }
}