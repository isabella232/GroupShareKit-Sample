using GroupShareKitSample.Models;
using Sdl.Community.GroupShareKit.Clients;
using System;
using System.Collections.Generic;
using System.IO;
using System.Security.Principal;
using System.Threading.Tasks;

namespace GroupShareKitSample.Repository
{
    public class ProjectsRepository
    {
        private readonly IPrincipal user;

        public ProjectsRepository(IPrincipal user)
        {
            this.user = user;
        }

        public async Task<List<Project>> GetProjects()
        {
            var gsClient = Helper.HelperMethods.GetCurrentGsClient(user);
            var gsProjects = await gsClient.Project.GetAllProjects();
            var projects = new List<Project>();

            foreach (var gsProject in gsProjects.Items)
            {
                var project = new Project
                {
                    Name = gsProject.Name,
                    CompletedAt = gsProject.CompletedAt,
                    CreatedBy = gsProject.CreatedBy,
                    DueDate = gsProject.DueDate,
                    OrganizationId = gsProject.OrganizationId,
                    ProjectId = gsProject.ProjectId,
                    SourceLanguage = gsProject.SourceLanguage,
                    TargetLanguage = gsProject.TargetLanguage,
                    CustomerName = gsProject.CustomerName,
                    OrganizationName = gsProject.OrganizationName,

                };
                projects.Add(project);
            }
            return projects;
        }

        public async Task<string> CreateProject(string projectName, string templateId, string organizationId, DateTime dueDate, string zipPath)
        {
            try
            {
                // get user credentials
                var gsClient = Helper.HelperMethods.GetCurrentGsClient(user);

                // read zip files as bytes
                var rawData = File.ReadAllBytes(zipPath);

                // create project using necessary information
                var projectId = await gsClient.Project.CreateProject(new CreateProjectRequest(
                    projectName,
                    organizationId,
                    null,
                    dueDate,
                    templateId,
                    rawData));

                // delete the zip folder from the local machine
                Helper.HelperMethods.DeleteFolder(zipPath);

                return projectId;
            }
            catch (Exception ex)
            {
                // if the project creation process is crashing, delete the exiting zip folder from the local machine
                Helper.HelperMethods.DeleteFolder(zipPath);
                throw new Exception(ex.Message);
            }
        }

        public async Task<List<Template>> GetTemplates()
        {
            var gsClient = Helper.HelperMethods.GetCurrentGsClient(user);
            var gsTemplates = await gsClient.Project.GetAllTemplates();
            var templates = new List<Template>();
            foreach (var gsTemplate in gsTemplates)
            {
                var template = new Template
                {
                    Id = gsTemplate.Id,
                    Name = gsTemplate.Name
                };
                templates.Add(template);
            }
            return templates;
        }

        public async Task<List<Organization>> GetAllOrganizations()
        {
            var gsClient = Helper.HelperMethods.GetCurrentGsClient(user);
            var gsOrganizations = await gsClient.Organization.GetAll(new OrganizationRequest(false));
            var organizations = new List<Organization>();
            foreach (var gsOrganization in gsOrganizations)
            {
                var organization = new Organization
                {
                    Id = gsOrganization.UniqueId.ToString(),
                    Name = gsOrganization.Name
                };
                organizations.Add(organization);
            }
            return organizations;
        }

        public  async Task<List<Analyse>> AnalyseProject(string projectId)
        {
            var gsClient = Helper.HelperMethods.GetCurrentGsClient(user);
            var gsAnalyse = await gsClient.Project.GetAnalysisReportsAsHtml(projectId, null);
            var analyseResults = new List<Analyse>();
            foreach(var analyse in gsAnalyse)
            {
                var analyseReport = new Analyse
                {
                    LanguageCode = analyse.LanguageCode,
                    Report = analyse.Report
                };
                analyseResults.Add(analyseReport);
            }
            return analyseResults;
        }
    }
}