using System;
using System.Collections.Generic;
using System.Linq;
using System.Net;
using System.Net.Http;
using System.Web.Http;
using WebAPI_SQLVol2.Models;

namespace WebAPI_SQLVol2.Controllers
{
    public class AccountController : ApiController
    {
        LGH_UserAccountsEntities db = new LGH_UserAccountsEntities();
        Encryptor_strings encryptor_strings = new Encryptor_strings();
        private const string key_api_login = "ur api key here";

        [ActionName("CreateFull")]
        [Route("api/CreateFull")]
        [HttpPost]
        public HttpResponseMessage CreateFull(UserAccount _newUser)
        {

            try
            {
                db.UserAccounts.Add(_newUser);
                db.SaveChanges();
                return Request.CreateResponse(HttpStatusCode.Accepted, "Successfully Create");
            }
            catch (Exception ex)
            {
                return Request.CreateResponse(HttpStatusCode.Accepted, "Exception: " + ex.ToString());
                throw;
            }
        
        }

 
        [ActionName("CreateTest")]
        [Route("api/CreateTest")]
        [AcceptVerbs("GET", "POST")]
        public HttpResponseMessage CreateTest()
        {
            UserAccount newAccount = new UserAccount();
            newAccount.UserID = Guid.NewGuid();
            newAccount.Name = "Testname" + DateTime.Now;
            newAccount.Password = "password";
            newAccount.Email = "email";
            newAccount.KarmaPoints = "karmaPoints";
            newAccount.FacebookLink = "facebookLink";
            newAccount.Age = "age";
            newAccount.City = "city";
            newAccount.Gender = "gender";

            db.UserAccounts.Add(newAccount);

            db.SaveChanges(); 
            return Request.CreateResponse(HttpStatusCode.Accepted, "Successfully TestCreate");
        }

      // http://localhost:51307/api/Login?username=navn&password=Kode
        [ActionName("Login")]
        [Route("api/Login")]
        [HttpGet]
        public HttpResponseMessage Login(string username, string password)
        {
            username = encryptor_strings.DecryptText(username);
            password = encryptor_strings.DecryptText(password);

            var user = db.UserAccounts.Where(x => x.Name == username && x.Password == password).FirstOrDefault();
            if (user == null)
            {
                return Request.CreateResponse(HttpStatusCode.Unauthorized, "Please Enter valid UserName and Password");
            }
            else
            {
                return Request.CreateResponse(HttpStatusCode.Accepted, "Successfull Login");
            }
        }

        [ActionName("GetAll")]
        [Route("api/GetAll")]
        [HttpGet]
        public List<string> GetAllUserNames()
        {
            List<string> temp = new List<string>();

            foreach (var user in db.UserAccounts)
            {
                temp.Add(user.Name);
            }

            return temp;
        }
    }
}
