using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.UI;
using System.Web.UI.HtmlControls;
using System.Web.UI.WebControls;
//using iFrameProjectExample.it.sella.ecomms2s; //PRO
using iFrameProjectExample.it.sella.testecomm; //TEST

using System.Xml;

namespace iFrameProjectExample
{

    public partial class WebForm1 : System.Web.UI.Page
    {
        public string Shop_Login = ""; //ADD YOUR SHOP_LOGIN CODE HERE
        public string EncryptedString;
        public string ErrorCode;
        public string ErrorDescription;
        public string XMLOUT;
        public string PARes;
        public string TransKey;
        public string ErrorClass = "Off";
        Page page = HttpContext.Current.Handler as Page;

        protected void Page_Load(object sender, EventArgs e)
        {
            //Handle postback after 3D authentication
            PARes = Request.Params["PARes"];
            if (PARes != null)
            {
                //This is a 2nd call after 3D authentication
                //Read EncryptedString and Trankey from the cookie
                HttpCookie myEncStringCookie;
                HttpCookie myTransKeyCookie;
                myEncStringCookie = Request.Cookies["encString"];
                if (myEncStringCookie != null) EncryptedString = myEncStringCookie.Value;
                myTransKeyCookie = Request.Cookies["TransKey"];
                if (myEncStringCookie != null) TransKey = myTransKeyCookie.Value;

            }
            else
            {
                //Encrypt the needed parameters 
                string Amount = "0.05";
                string ShopTransactionId = "iframeCryptTest";
                string Currency = "242"; //UicCode
                PaymentTypeDetail PaymentTDetail = new PaymentTypeDetail();
                ShippingDetails ShipDetails = new ShippingDetails();
                RedBillingInfo RedBilling = new RedBillingInfo();
                RedCustomerData RedCustomerData = new RedCustomerData();
                RedCustomerInfo RedCustomerInfo = new RedCustomerInfo();
                RedItems RedItem = new RedItems();
                RedShippingInfo RedShipping = new RedShippingInfo();
                ConselCustomerInfo ConselCustomer = new ConselCustomerInfo();
                string[] PaymentTypes = { "" };
                string[] RedCustomInfo = { "" };

                WSCryptDecrypt objCrypt = new WSCryptDecrypt();
                XMLOUT = objCrypt.Encrypt(Shop_Login, Currency, Amount, ShopTransactionId, "", "", "", "", "", "", "", "", "", "", ShipDetails, PaymentTypes, PaymentTDetail, "", RedCustomerInfo, RedShipping, RedBilling, RedCustomerData, RedCustomInfo, RedItem, "", ConselCustomer, "").OuterXml;
                XmlDocument XmlReturn = new XmlDocument();
                XmlReturn.LoadXml(XMLOUT);
                XmlNode ThisNode = XmlReturn.SelectSingleNode("/GestPayCryptDecrypt/ErrorCode");
                ErrorCode = ThisNode.InnerText;
                if (ErrorCode == "0")
                {
                    XmlNode ThisNode2 = XmlReturn.SelectSingleNode("//GestPayCryptDecrypt/CryptDecryptString");
                    EncryptedString = ThisNode2.InnerText;

                }
                else
                {
                    //Put error handle code HERE
                    ThisNode = XmlReturn.SelectSingleNode("/GestPayCryptDecrypt/ErrorDescription");
                    ErrorDescription = ThisNode.InnerText;
                    ErrorClass = "on";
                }
            }
        }
    }
}