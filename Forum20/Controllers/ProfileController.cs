using Forum20.Data.Models;
using Forum20.Data;
using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Mvc;
using Forum20.Models.ApplicationUser;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Http;
using Microsoft.Extensions.Configuration;
using System.Net.Http.Headers;
using System.Linq;
using Microsoft.AspNetCore.Authorization;

namespace Forum20.Controllers
{
    [Authorize]
    public class ProfileController : Controller
    {
        private readonly UserManager<ApplicationUser> _userManager;
        private readonly IApplicationUser _userService;
        private readonly IUpload _uploadService;
        private readonly IConfiguration _configuration;

        public ProfileController(UserManager<ApplicationUser> userManager, IApplicationUser userService, IUpload uploadService,IConfiguration configuration)
        {
            _userManager = userManager;
            _userService = userService;
            _uploadService = uploadService;
            _configuration = configuration;
        }
        public IActionResult Detail(string id)
        {
            var user = _userService.GetById(id);
            var userRoles = _userManager.GetRolesAsync(user).Result;
            var model = new ProfileModel
            {
                UserId = user.Id,
                Username = user.UserName,
                UserRating = user.Rating.ToString(),
                Email = user.Email,
                ProfileImageUrl = user.ProfileImageUrl,
                MemberSince = user.MemberSince,
               // IsActive = user.IsActive,
                IsAdmin = userRoles.Contains("Admin")
            };
            return View(model);
        }

        [HttpPost]
        public async Task<IActionResult> UploadProfileImage(IFormFile file)
        {
            var userId = _userManager.GetUserId(User);
            //Connect to an Azure Storage Account
            //Get Blob Container
            var connectionString = _configuration.GetConnectionString("AzureStorageAccount");
            //Parse the Content Disposition response header
            //Grab the filename
            var container = _uploadService.GetBlobContainer(connectionString,"profile-images");

            var ContentDisposition = ContentDispositionHeaderValue.Parse(file.ContentDisposition);

            var filename = ContentDisposition.FileName.Trim('"');

            var blockBlob = container.GetBlockBlobReference(filename);
            //Get a reference block blob
            //Get the block blod,upload the file

            await blockBlob.UploadFromStreamAsync(file.OpenReadStream());
            await _userService.SetProfileImage(userId, blockBlob.Uri);

            return RedirectToAction("Detail", "Profile", new { id = userId });
            //set the profile image url
            //Redirect to detail page 
           // return View();
        }

        [Authorize(Roles = "Admin")]
        public IActionResult Index()
        {
            var profiles = _userService.GetAll()
                .OrderByDescending(user => user.Rating)
                .Select(u => new ProfileModel
                {
                    UserId=u.Id,
                    Email = u.Email,
                    Username=u.UserName,
                    ProfileImageUrl = u.ProfileImageUrl,
                    UserRating = u.Rating.ToString(),
                    MemberSince = u.MemberSince
                   // IsActive = u.IsActive
                });

            var model = new ProfileListModel
            {
                Profiles = profiles
            };

            return View(model);
        }
        public IActionResult Deactivate(string userId)
        {
            var user = _userService.GetById(userId);
            _userService.Deactivate(user);
            return RedirectToAction("Index", "Profile");
        }
    }
}