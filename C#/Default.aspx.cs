using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.UI;
using System.Web.UI.WebControls;

namespace API_Example
{
    public partial class _Default : System.Web.UI.Page
    {
        protected void Page_Load(object sender, EventArgs e)
        {
            // Set credential values
            string sourcename = "YourSourceName";
            string password = "YourPassword";
            int[] siteIDs = {-99};


            ///////////////////////
            // Standard API call //
            ///////////////////////

            // Create Service
            ClassService.ClassService classService = new ClassService.ClassService();

            // Create request
            ClassService.GetClassesRequest classRequest = new ClassService.GetClassesRequest();

            // Create and fill credentials
            classRequest.SourceCredentials = new ClassService.SourceCredentials();
            classRequest.SourceCredentials.SourceName = sourcename;
            classRequest.SourceCredentials.Password = password;
            classRequest.SourceCredentials.SiteIDs = siteIDs;

            // Run call with request and fill result  
            ClassService.GetClassesResult classResult = classService.GetClasses(classRequest);
            
            // Display result in label
            foreach(ClassService.Class thisClass in classResult.Classes)
            {
                Label.Text += "<br/>______________________________________<div><p>" + 
                    thisClass.ClassDescription.Name + "</p>" +
                    "<p>" + thisClass.ID + "</p>" +
                    "<p>" + thisClass.EndDateTime + " - " + thisClass.EndDateTime + "</p>" +
                    "<p>" + thisClass.ClassDescription.Description + "</p></div>";
            }


            ////////////////////////////
            // SSL protected API call //
            ////////////////////////////

            // Create Service
            SaleService.SaleService saleService = new SaleService.SaleService();

            // Create request
            SaleService.CheckoutShoppingCartRequest saleRequest = new SaleService.CheckoutShoppingCartRequest();

            // Create and fill credentials
            saleRequest.SourceCredentials = new SaleService.SourceCredentials();
            saleRequest.SourceCredentials.SourceName = sourcename;
            saleRequest.SourceCredentials.Password = password;
            saleRequest.SourceCredentials.SiteIDs = siteIDs;

            // Add ClientID
            saleRequest.ClientID = "100000000";

            // Create and add cart items
            SaleService.CartItem[] items = {new SaleService.CartItem()};
            items[0].ID = 93;
            saleRequest.CartItems = items;

            // Create and add credit card info
            SaleService.CreditCardInfo[] payments = {new SaleService.CreditCardInfo()};
            payments[0].CreditCardNumber = "4111111111111111";
            payments[0].Amount = 2.00M;
            payments[0].BillingAddress = "123 Something";
            payments[0].BillingCity = "SLO";
            payments[0].BillingState = "CA";
            payments[0].BillingPostalCode = "93405";
            payments[0].BillingName = "MindBody";
            payments[0].ExpMonth = "7";
            payments[0].ExpYear = "2016";

            saleRequest.Payments = payments;

            // Manually set endpoint to https
            saleService.Url = "https://api.mindbodyonline.com/0_5/SaleService.asmx";

            // Run call with request and fill result 
            SaleService.CheckoutShoppingCartResult saleResult = saleService.CheckoutShoppingCart(saleRequest);

            // Display result in label
            Label.Text += "<br/>**************************************<div><p>" + saleResult.Message + "</p>";
            
        }
    }
}
