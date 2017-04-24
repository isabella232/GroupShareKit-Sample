using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Reactive.Linq;
using System.Security.Claims;
using System.Threading.Tasks;
using System.Web;
using System.Web.Mvc;
using Akavache;
using GroupShareKitSample.Models;
using GroupShareKitSample.Repository;
using Microsoft.AspNet.Identity;
using System.IO.Compression;



namespace GroupShareKitSample.Controllers
{

    [Authorize]
    public class HomeController : Controller
    {
        private static ProjectsRepository _projectsRepository;
        private static byte[] _projectsData;

        public async Task<ActionResult> Index()
        {

            _projectsRepository = new ProjectsRepository(HttpContext.User);
            var projects = await _projectsRepository.GetProjects();
            var templates = await _projectsRepository.GetTemplates();
            var organizations = await _projectsRepository.GetAllOrganizations();

            var projectsViewModel = new ProjectViewModel
            {
                Templates = templates,
                Projects = projects,
                Organizations = organizations
            };
            return View(projectsViewModel);
        }

        public ActionResult UploadFile(string template, string organization)
        {
            var files = new Dictionary<byte[], string>();
            foreach (string file in Request.Files)
            {
                var httpFileBase = Request.Files[file];
                if (httpFileBase != null)
                {
                    var fileName = httpFileBase.FileName;
                    using (var binaryReader = new BinaryReader(httpFileBase.InputStream))
                    {
                        var fileData = binaryReader.ReadBytes(httpFileBase.ContentLength);
                        files.Add(fileData, fileName);
                    }
                }
            }
            using (var compressedFileStream = new MemoryStream())
            {
                using (var zipArchive = new ZipArchive(compressedFileStream, ZipArchiveMode.Update, false))
                {
                    foreach (var file in files)
                    {
                        var zipEntry = zipArchive.CreateEntry(file.Value);
                        //Get the stream of the attachment

                        using (var originalFileStream = new MemoryStream(file.Key))
                        {
                            using (var zipEntryStream = zipEntry.Open())
                            {
                                originalFileStream.CopyTo(zipEntryStream);
                            }
                        }

                    }
                }
                var test = new FileContentResult(compressedFileStream.ToArray(), "application/zip")
                {
                    FileDownloadName = "Filename.zip"
                };
                _projectsData = test.FileContents;
            }


         



            return null;
        }

        [HttpPost]
        public async Task CreateProject(string projectName, string templateId, string organizationId,string dueDate)
        {
            DateTime date= new DateTime();
            try
            {
                date = Convert.ToDateTime(dueDate);
            }catch(Exception e) { }
            
            await _projectsRepository.CreateProject(projectName, templateId, organizationId, _projectsData,date);
        }
    }
}