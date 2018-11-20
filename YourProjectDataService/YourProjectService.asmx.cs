using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Services;
using YourProjectDataService.Model;

namespace YourProjectDataService
{
    /// <summary>
    /// Summary description for YourProjectService
    /// </summary>
    [WebService(Namespace = "http://tempuri.org/")]
    [WebServiceBinding(ConformsTo = WsiProfiles.BasicProfile1_1)]
    [System.ComponentModel.ToolboxItem(false)]
    // To allow this Web Service to be called from script, using ASP.NET AJAX, uncomment the following line. 
    [System.Web.Script.Services.ScriptService]
    public class YourProjectService : System.Web.Services.WebService
    {

        #region Patron
        [WebMethod]
        public List<Patron> GetAllPatrons()
        {
            var data = new Patron().GetAll<Patron>(Patron.QuerySelectAll);
            return data;
        }

        [WebMethod]
        public Patron GetSinglePatron(int id)
        {
            var data = new Patron().GetSingle<Patron>(id, Patron.QuerySelectOne);
            return data;
        }

        [WebMethod]
        public Patron UpdatePatron(Patron patron)
        {
            return patron.Update(patron, Patron.QueryUpdate);
        }

        [WebMethod]
        public Patron CreatePatron(Patron patron)
        {
            return patron.Create(patron, Patron.QueryInsertInto);
        }

        [WebMethod]
        public bool DeletePatron(int id)
        {
            var patron = new Patron().GetSingle<Patron>(id, Patron.QuerySelectOne);
            if (patron == null)
            {
                return false;
            }
            var result = patron.Delete<Patron>(id, Patron.QueryDelete);
            return result;
        }
        #endregion

        #region Tool
        [WebMethod]
        public List<Tool> GetAllTools()
        {
            var data = new Tool().GetAll<Tool>(Tool.QuerySelectAll);
            return data;
        }

        // Web method used for selecting a list of tools with a condition.
        [WebMethod]
        public List<Tool> GetAllToolsWithCondition(string condition)
        {
            var query = Tool.QuerySelectAll + condition;
            var data = new Tool().GetAll<Tool>(query);
            return data;
        }


        [WebMethod]
        public Tool GetSingleTool(int id)
        {
            var data = new Tool().GetSingle<Tool>(id, Tool.QuerySelectOne);
            return data;
        }

        [WebMethod]
        public Tool UpdateTool(Tool tool)
        {
            return tool.Update(tool, Tool.QueryUpdate);
        }

        [WebMethod]
        public Tool CreateTool(Tool tool)
        {
            return tool.Create(tool, Tool.QueryInsertInto);
        }

        [WebMethod]
        public bool DeleteTool(int id)
        {
            var tool = new Tool().GetSingle<Tool>(id, Tool.QuerySelectOne);
            if (tool == null)
            {
                return false;
            }
            var result = tool.Delete<Tool>(id, Tool.QueryDelete);
            return result;
        }


        #endregion

        #region Brand
        [WebMethod]
        public List<Brand> GetAllBrands()
        {
            var data = new Brand().GetAll<Brand>(Brand.QuerySelectAll);
            return data;
        }

        [WebMethod]
        public Brand GetSingleBrand(int id)
        {
            var data = new Brand().GetSingle<Brand>(id, Brand.QuerySelectOne);
            return data;
        }

        [WebMethod]
        public Brand UpdateBrand(Brand brand)
        {
            return brand.Update(brand, Brand.QueryUpdate);
        }

        [WebMethod]
        public Brand CreateBrand(Brand brand)
        {
            return brand.Create(brand, Brand.QueryInsertInto);
        }

        [WebMethod]
        public bool DeleteBrand(int id)
        {
            var brand = new Brand().GetSingle<Brand>(id, Brand.QuerySelectOne);
            if (brand == null)
            {
                return false;
            }
            var result = brand.Delete<Brand>(id, Brand.QueryDelete);
            return result;
        }

        #endregion

        #region ToolLoanInvoices
        [WebMethod]
        public List<PatronToolLoanInvoice> GetAllInvoices()
        {
            var data = new PatronToolLoanInvoice().GetAll<PatronToolLoanInvoice>(PatronToolLoanInvoice.QuerySelectAll);
            return data;
        }

        [WebMethod]
        public List<PatronToolLoanInvoice> GetAllInvoicesWithCondition(string condition)
        {
            var data = new PatronToolLoanInvoice().GetAll<PatronToolLoanInvoice>(PatronToolLoanInvoice.QuerySelectAll+condition);
            return data;
        }

        [WebMethod]
        public PatronToolLoanInvoice GetSinglePatronToolLoanInvoice(int id)
        {
            var data = new PatronToolLoanInvoice().GetSingle<PatronToolLoanInvoice>(id, PatronToolLoanInvoice.QuerySelectOne);
            return data;
        }

        [WebMethod]
        public PatronToolLoanInvoice UpdatePatronToolLoanInvoice(PatronToolLoanInvoice invoice)
        {
            return invoice.Update(invoice, PatronToolLoanInvoice.QueryUpdate);
        }

        [WebMethod]
        public PatronToolLoanInvoice CreatePatronToolLoanInvoice(PatronToolLoanInvoice invoice)
        {
            return invoice.Create(invoice, PatronToolLoanInvoice.QueryInsertInto);
        }

        [WebMethod]
        public bool DeletePatronToolLoanInvoice(int id)
        {
            var invoice = new PatronToolLoanInvoice().GetSingle<PatronToolLoanInvoice>(id, PatronToolLoanInvoice.QuerySelectOne);
            if (invoice == null)
            {
                return false;
            }
            var result = invoice.Delete<PatronToolLoanInvoice>(id, PatronToolLoanInvoice.QueryDelete);
            return result;
        }


        #endregion

        #region Staff

        [WebMethod]
        public bool ValidCredentials(Staff staff)
        {
            return Model.Database.ValidateUserCredentials(staff.UserName, staff.UserPassword);

        }

        #endregion


    }
}
