using GroupShareKitSample.Helper;
using GroupShareKitSample.Models;
using GroupShareKitSample.Repository;
using Newtonsoft.Json;
using System;
using System.Collections.Generic;
using System.IO;
using System.IO.Compression;
using System.Threading.Tasks;
using System.Web.Mvc;

namespace GroupShareKitSample.Controllers
{
    [GSAuthorizeAttribute]
    public class HomeController : Controller
    {
        private static ProjectsRepository _projectsRepository;
        private static string _zipPath = Path.Combine(Path.GetTempPath(), "GSFiles.zip");

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
            // remove the zip folder if exists (used in case user does not finalize the project creation, and arhive is already created)
            HelperMethods.DeleteFolder(_zipPath);

            // get files from the server request
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

            // add each file to a local zip (used when creating project)
            using (ZipArchive archive = new ZipArchive(System.IO.File.Create(_zipPath), ZipArchiveMode.Create))
            {
                foreach (var file in files)
                { 
                    ZipArchiveEntry entry = archive.CreateEntry(file.Value);
                    using (BinaryWriter writer = new BinaryWriter(entry.Open()))
                    {
                        writer.Write(file.Key);
                    }
                }
            }
            return null;
        }

        [HttpPost]
        public async Task CreateProject(string projectName, string templateId, string organizationId, string dueDate)
        {
            DateTime date = DateTime.Now.AddDays(7);
            try
            {
                date = Convert.ToDateTime(dueDate);
                await _projectsRepository.CreateProject(projectName, templateId, organizationId, date, _zipPath);
            }
            catch (Exception ex)
            {
                throw new Exception(ex.Message);
            }
        }      

        [HttpGet]
        public async Task<JsonResult> AnalyseProject(string projectId)
        {
            var analyseResult = await _projectsRepository.AnalyseProject(projectId);

            var json = JsonConvert.SerializeObject(analyseResult);
            return Json(json, JsonRequestBehavior.AllowGet);
        }
    }
}