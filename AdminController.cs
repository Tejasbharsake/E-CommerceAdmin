using GSTECommerceLibrary.Admin;
using RahiShop.Services;
using System;
using System.Collections.Generic;
using System.Data;
using System.Data.SqlClient;
using System.Diagnostics;
using System.IO;
using System.Linq;
using System.Net;
using System.Threading.Tasks;
using System.Web;
using System.Web.Mvc;
using System.Web.UI.WebControls;
using static GSTECommerceLibrary.Admin.Admin;
//using GSTECommerceLibrary.ViewModels;

namespace GSTECommerce.Controllers
{
    public class AdminController : Controller
    {
        BALAdmin Obj = new BALAdmin();
        private readonly EmailService _emailService;
         private readonly string apiKey = "MHlrYUFTTjZFQnMxTjdxQWpRY1RDNzhXd1kxOFB0SWdLWGUxSGF3RQ==";
    private readonly string apiUrl = "https://api.countrystatecity.in/v1/countries/IN/states/MH/cities";


        public AdminController()
        {
            _emailService = new EmailService();
        }

        public ActionResult LoginTB() => View(new Admin());

        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<ActionResult> LoginTB(Admin model)
        {
            if (!ModelState.IsValid)
            {
                ViewBag.Error = "Please enter valid credentials.";
                return View(model);
            }

            SqlDataReader dr = await Obj.LoginTB(model.EmailId, model.UserPassword);

            if (dr.HasRows)
            {
                while (dr.Read())
                {
                    Session["ClientCode"] = dr["ClientCode"].ToString();
                    Session["UserName"] = dr["UserName"].ToString();
                }
                dr.Close();
                return RedirectToAction("Dashboard");
            }
            else
            {
                ViewBag.Error = "Invalid login credentials!";
                return View(model);
            }
        }

        public ActionResult ForgotPasswordTB()
        {
            return View(new ForgotPasswordViewModel());
        }


        // GET: Admin/Registration
        [HttpGet]
        public ActionResult RegisterAdminTB()
        {
            return View();
        }

        // POST: Admin/Registration
        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<ActionResult> RegisterAdminTB(Admin model, HttpPostedFileBase UploadPhotoFile, HttpPostedFileBase UploadAadharFile, HttpPostedFileBase UploadPanCardFile, HttpPostedFileBase UploadBusinessLicenceFile)
        {
            // Model valid ahe ka te check kara (e.g., Required fields bharle ahet ka)
            if (ModelState.IsValid)
            {
                try
                {
                    // === FILE UPLOAD HANDLING ===
                    // Pratyek file la Base64 string madhe convert karun model madhe save kara.

                    // 1. User Photo
                    if (UploadPhotoFile != null && UploadPhotoFile.ContentLength > 0)
                    {
                        using (var binaryReader = new BinaryReader(UploadPhotoFile.InputStream))
                        {
                            byte[] fileData = binaryReader.ReadBytes(UploadPhotoFile.ContentLength);
                            model.UserPhoto = Convert.ToBase64String(fileData);
                        }
                    }

                    // 2. Aadhaar Card
                    if (UploadAadharFile != null && UploadAadharFile.ContentLength > 0)
                    {
                        using (var binaryReader = new BinaryReader(UploadAadharFile.InputStream))
                        {
                            byte[] fileData = binaryReader.ReadBytes(UploadAadharFile.ContentLength);
                            model.Aadharcard = Convert.ToBase64String(fileData);
                        }
                    }

                    // 3. PAN Card
                    if (UploadPanCardFile != null && UploadPanCardFile.ContentLength > 0)
                    {
                        using (var binaryReader = new BinaryReader(UploadPanCardFile.InputStream))
                        {
                            byte[] fileData = binaryReader.ReadBytes(UploadPanCardFile.ContentLength);
                            model.PanCard = Convert.ToBase64String(fileData);
                        }
                    }

                    // 4. Business Licence
                    if (UploadBusinessLicenceFile != null && UploadBusinessLicenceFile.ContentLength > 0)
                    {
                        using (var binaryReader = new BinaryReader(UploadBusinessLicenceFile.InputStream))
                        {
                            byte[] fileData = binaryReader.ReadBytes(UploadBusinessLicenceFile.ContentLength);
                            model.BusinessLicence = Convert.ToBase64String(fileData);
                        }
                    }

                    model.UserRegistrationDate = DateTime.Now;

                    BALAdmin bal = new BALAdmin(); // BAL object banava
                    int result = await bal.RegistrationTB(model); // BAL method la call kara

                    if (result == 1) // Yashasvi (Success)
                    {
                        TempData["SuccessMessage"] = "Registration successful! You can now login.";
                        return RedirectToAction("Login");
                    }
                    else if (result == -1) // Duplicate Email
                    {
                        TempData["ErrorMessage"] = "This Email ID is already registered. Please use a different email.";
                    }
                    else
                    {
                        TempData["ErrorMessage"] = "Something went wrong during registration. Please try again.";
                    }
                }
                catch (Exception ex)
                {
                    // Stored Procedure madhil 'THROW' ithe error pakdel.
                    // Ha error log karun theva mhanje nehmka kay problem ahe te kalel.
                    // System.Diagnostics.Debug.WriteLine(ex.ToString());
                    TempData["ErrorMessage"] = "A critical error occurred: " + ex.Message;
                }
            }
            else
            {
                TempData["ErrorMessage"] = "Please fill all the required fields correctly.";
            }

            // Jar kahi chuk asel tar form parat dakhva
            return View(model);
        }

        public ActionResult Login()
        {
            return View();
        }


        //[HttpPost]
        //[ValidateAntiForgeryToken]
        //public async Task<ActionResult> SendResetLink(ForgotPasswordViewModel model)
        //{
        //    if (!ModelState.IsValid)
        //    {
        //        ViewBag.Message = "Please enter a valid email address.";
        //        return View("ForgotPasswordTB", model);
        //    }

        //    string userEmail = model.EmailId;
        //    string otpCode = GenerateRandomOtp(6);
        //    DateTime otpExpiry = DateTime.Now.AddMinutes(3);

        //    Session["OTP"] = otpCode;
        //    Session["OTP_Expiry"] = otpExpiry;
        //    Session["EmailForOTPVerification"] = userEmail;

        //    try
        //    {
        //        string subject = "RahiShop: Your Password Reset OTP";
        //        string emailBody = $"<p>Hi, your OTP is: <strong>{otpCode}</strong>. Valid for 3 minutes.</p>";

        //        await _emailService.SendEmailAsync(userEmail, subject, emailBody);

        //        TempData["OTPSentSuccessMessage"] = "An OTP has been sent to your email.";
        //        return RedirectToAction("VerifyOtp");
        //    }
        //    catch (Exception ex)
        //    {
        //        ViewBag.Message = "Failed to send OTP. Try again.";
        //        Debug.WriteLine("SendResetLink Error: " + ex.Message);
        //        return View("ForgotPasswordTB", model);
        //    }
        //}

        // ✅ NEW METHOD: GET: Admin/ResendOtp
        //public async Task<ActionResult> ResendOtp(string email)
        //{
        //    if (string.IsNullOrEmpty(email))
        //    {
        //        TempData["OTPSentSuccessMessage"] = "❌ Email not found.";
        //        return RedirectToAction("ForgotPasswordTB");
        //    }

        //    string otpCode = GenerateRandomOtp(6);
        //    DateTime otpExpiry = DateTime.Now.AddMinutes(3);

        //    Session["OTP"] = otpCode;
        //    Session["OTP_Expiry"] = otpExpiry;
        //    Session["EmailForOTPVerification"] = email;

        //    try
        //    {
        //        string subject = "RahiShop: Resent OTP";
        //        string body = $"<p>Your new OTP is: <strong>{otpCode}</strong>. Valid for 3 minutes.</p>";

        //        await _emailService.SendEmailAsync(email, subject, body);
        //        TempData["OTPSentSuccessMessage"] = "🔁 OTP has been resent.";
        //        return RedirectToAction("VerifyOtp");
        //    }
        //    catch (Exception ex)
        //    {
        //        Debug.WriteLine("ResendOtp Error: " + ex.Message);
        //        TempData["OTPSentSuccessMessage"] = "❌ Failed to resend OTP.";
        //        return RedirectToAction("VerifyOtp");
        //    }
        //}

        public ActionResult VerifyOtp()
        {
            string email = Session["EmailForOTPVerification"] as string;
            if (string.IsNullOrEmpty(email))
            {
                ViewBag.Message = "Please request an OTP first.";
                return RedirectToAction("ForgotPasswordTB");
            }

            var model = new OtpVerificationViewModel { Email = email };
            if (TempData["OTPSentSuccessMessage"] != null)
            {
                ViewBag.Message = TempData["OTPSentSuccessMessage"].ToString();
            }

            return View(model);
        }

        [HttpPost]
        [ValidateAntiForgeryToken]
        public ActionResult VerifyOtpTB(OtpVerificationViewModel model)
        {
            if (!ModelState.IsValid)
            {
                ViewBag.Message = "Invalid OTP format or email.";
                return View(model);
            }

            string sessionOtp = Session["OTP"]?.ToString();
            DateTime? expiry = Session["OTP_Expiry"] as DateTime?;
            string sessionEmail = Session["EmailForOTPVerification"]?.ToString();

            if (sessionOtp == null || expiry == null || sessionEmail == null)
            {
                ViewBag.Message = "❌ OTP session expired or not found.";
                return View(model);
            }

            if (DateTime.Now > expiry.Value)
            {
                Session.Clear();
                ViewBag.Message = "⏰ OTP Expired!";
                return View(model);
            }

            if (model.Email != sessionEmail)
            {
                ViewBag.Message = "❌ Email mismatch.";
                return View(model);
            }

            if (model.OtpCode == sessionOtp)
            {
                Session.Clear();
                TempData["EmailForPasswordReset"] = model.Email;
                return RedirectToAction("ChangePassword");
            }
            else
            {
                ViewBag.Message = "❌ Invalid OTP!";
                return View(model);
            }
        }
        [HttpPost]
        public ActionResult ChangePassword()
        {
            string email = TempData["EmailForPasswordReset"] as string;
            if (string.IsNullOrEmpty(email))
            {
                ViewBag.Message = "Please verify OTP first.";
                return RedirectToAction("ForgotPasswordTB");
            }

            ViewBag.Email = email;
            return View();
        }

        [HttpPost]
        [ValidateAntiForgeryToken]
        public ActionResult ChangePassword(string email, string newPassword, string confirmPassword)
        {
            if (string.IsNullOrEmpty(newPassword) || newPassword != confirmPassword)
            {
                ViewBag.Message = "Passwords do not match.";
                ViewBag.Email = email;
                return View();
            }

            // Add BALAdmin update password logic here
            ViewBag.Message = "Password changed successfully.";
            return RedirectToAction("LoginTB");
        }

        private string GenerateRandomOtp(int length)
        {
            const string chars = "0123456789";
            var random = new Random();
            return new string(Enumerable.Repeat(chars, length).Select(s => s[random.Next(s.Length)]).ToArray());
        }

        /// <summary>
        ///  from this is the otp 
        /// </summary>
        /// <returns></returns>
        /// 


        [HttpPost]
        [ValidateAntiForgeryToken]
        public ActionResult SendResetLink(Admin model)
        {
            if (string.IsNullOrEmpty(model.EmailId))
            {
                ViewBag.Message = "Please enter your email address.";
                return View("LoginTB", model);
            }


            // Generate OTP
            Random rnd = new Random();
            string otp = rnd.Next(100000, 999999).ToString();


            // Send email
            var emailService = new EmailService();
            string subject = "Your OTP for Password Reset";
            string body = $"Hello Admin,<br/><br/>Your OTP is: <b>{otp}</b><br/><br/>Thank you.";

            bool emailSent = emailService.SendEmail(model.EmailId, subject, body);

            if (emailSent)
                ViewBag.Message = "OTP sent successfully to your email.";
            else
                ViewBag.Message = "Failed to send email. Please try again.";

            return View("LoginTB", model);
        }




        /// <summary>
        /// Displays a list of all feedback entries submitted by client.
        /// Fetches data asynchronously from the database and maps it to a list of Admin model.
        /// </summary>
        /// <returns>Returns the FeedbackTB view with a list of feedbacks.</returns>

        // Feedback list
        public async Task<ActionResult> FeedbackTB()
        {
            List<Admin> feedbackList = new List<Admin>();
            DataSet ds = await Obj.GetAdminFeedBackDetails();

            if (ds != null && ds.Tables.Count > 0 && ds.Tables[0].Rows.Count > 0)
            {
                foreach (DataRow row in ds.Tables[0].Rows)
                {
                    feedbackList.Add(new Admin
                    {
                        OrderCode = row["OrderCode"].ToString(),
                        ProductName = row["ProductName"].ToString(),
                        Rating = Convert.ToInt32(row["Rating"]),
                        Feedback = row["Feedback"].ToString(),
                        FeedbackDate = Convert.ToDateTime(row["FeedbackDate"])
                    });
                }
            }

            return View("FeedbackTB", feedbackList); // View path: Views/Admin/FeedbackTB.cshtml
        }



        /// <summary>
        /// Displays detailed feedback information for a specific order.
        /// Takes an OrderCode as a parameter, fetches the corresponding feedback from the database,
        /// and returns it in a partial view.
        /// </summary>
        /// <param >  Client name = 'orderCode'  Unique identifier for the order   </param>
        /// <returns>Returns a partial view (_FeedbackDetailsTB) with detailed feedback info.</returns>
        // Feedback details
        public async Task<ActionResult> FeedbackDetailsTB(string orderCode)
        {
            if (string.IsNullOrEmpty(orderCode))
                return Content("Invalid Order Code");

            DataSet ds = await Obj.GetFeedbackDetailsByOrderCode(orderCode);
            Admin model = new Admin();

            if (ds != null && ds.Tables.Count > 0 && ds.Tables[0].Rows.Count > 0)
            {
                var row = ds.Tables[0].Rows[0];
                model.OrderCode = row["OrderCode"].ToString();
                model.ProductName = row["ProductName"].ToString();
                model.Rating = Convert.ToInt32(row["Rating"]);
                model.Feedback = row["Feedback"].ToString();
                model.FeedbackDate = Convert.ToDateTime(row["FeedbackDate"]);
                model.CustomerName = row["CustomerName"].ToString();
            }

            return PartialView("~/Views/Shared/_FeedbackDetailsTB.cshtml", model); // 👈 Correct path
        }






        // In AdminController.cs
        //[HttpGet]
        //public async Task<ActionResult> _AdminProfileTB()
        //{
        //    // Logged-in user cha client code session madhun ghe
        //    string clientCode = Session["ClientCode"]?.ToString();
        //    if (string.IsNullOrEmpty(clientCode))
        //        clientCode = "CC001"; // Default client code, change as needed

        //    // DB madhun data ghe
        //    DataSet ds = await Obj.ViewProfile(clientCode);
        //    Admin model = new Admin();

        //    if (ds != null && ds.Tables.Count > 0 && ds.Tables[0].Rows.Count > 0)
        //    {
        //        var row = ds.Tables[0].Rows[0];
        //        model.UserName = row["AdminName"].ToString();
        //        model.BusinessName = row["BusinessName"].ToString();
        //        model.EmailId = row["EmailId"].ToString();
        //        model.Gender = row["Gender"].ToString();
        //        model.Address = row["Address"].ToString();
        //        model.MobileNo = row["MobileNo"].ToString();
        //        model.AlternateMobileNo = row["AlternateMobileNo"].ToString();
        //        model.GSTIN = row["GSTIN"].ToString();
        //        model.AadhaarCardNo = row["AadhaarCardNo"].ToString();
        //        model.PanCardNo = row["PanCardNo"].ToString();
        //        model.BankAccountNo = row["BankAccountNo"].ToString();
        //        model.IFSCCode = row["IFSCCode"].ToString();
        //        model.CityId = Convert.ToInt32(row["CityId"]);
        //    }

        //    return PartialView(model);
        //}

        //[HttpGet]
        //public async Task<ActionResult> AdminProfileTB()
        //{
        //    Admin model = new Admin();
        //    // इथे DB मधून डेटा आण
        //    // model.Name = ... etc
        //    return View(model);
        //}
        //[HttpGet]
        public async Task<ActionResult> ViewProfileTB()
        {
            string clientCode = Session["ClientCode"]?.ToString();
            if (string.IsNullOrEmpty(clientCode))
                clientCode = "CC001"; // fallback default

            DataSet ds = await Obj.ViewProfile(clientCode); // your DB call
            Admin model = new Admin();

            if (ds != null && ds.Tables.Count > 0 && ds.Tables[0].Rows.Count > 0)
            {
                var row = ds.Tables[0].Rows[0];

                model.UserName = row["AdminName"].ToString();
                model.BusinessName = row["BusinessName"].ToString();
                model.EmailId = row["EmailId"].ToString();
                model.Gender = row["Gender"].ToString();
                model.Address = row["Address"].ToString();
                model.MobileNo = row["MobileNo"].ToString();
                model.AlternateMobileNo = row["AlternateMobileNo"].ToString();
                model.GSTIN = row["GSTIN"].ToString();
                model.AadhaarCardNo = row["AadhaarCardNo"].ToString();
                model.PanCardNo = row["PanCardNo"].ToString();
                model.BankAccountNo = row["BankAccountNo"].ToString();
                model.IFSCCode = row["IFSCCode"].ToString();

                // Check null before converting to int
                if (row["CityId"] != DBNull.Value)
                    model.CityId = Convert.ToInt32(row["CityId"]);
            }

            return  View();
        }



        [HttpGet]
        public ActionResult _EditProfile()
        {
            Admin model = new Admin();
            return PartialView("_EditProfile", model);
        }

        [HttpGet]
        public ActionResult _ChangePassword()
        {
            return PartialView("_ChangePassword");
        }
    }





}






