using System;
using System.CodeDom;
using System.Collections.Generic;
using System.Linq;
using System.Security.Principal;
using System.Threading.Tasks;
using System.Web;
using GroupShareKitSample.Models;
using Sdl.Community.GroupShareKit;
using Sdl.Community.GroupShareKit.Clients;

namespace GroupShareKitSample.Repository
{
    public class ProjectsRepository
    {
        private readonly IPrincipal _user;
        private readonly string _token;

        public ProjectsRepository(IPrincipal user)
        {
            _user = user;
            _token = Helper.HelperMethods.GetToken(user);
        }

        public async Task<List<Project>> GetProjects()
        {
            var gsClient = await Helper.HelperMethods.GetCurrentGsClient(_token, _user);
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

        public async Task<string> CreateProject(string projectName,string templateId,string organizationId,byte[]rawData,DateTime dueDate)
        {
            var gsClient = await Helper.HelperMethods.GetCurrentGsClient(_token, _user);
            var projectRequest = new CreateProjectRequest(projectName,organizationId,"",dueDate, templateId,rawData);
            var id = await gsClient.Project.CreateProject(projectRequest);
            return id;
        }

        public async Task<List<Template>>GetTemplates()
        {
            var gsClient = await Helper.HelperMethods.GetCurrentGsClient(_token, _user);
            var gsTemplates = await gsClient.ProjectsTemplates.GetAllTemplates();
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

        public async  Task<List<Organization>>GetAllOrganizations()
        {
            var gsClient = await Helper.HelperMethods.GetCurrentGsClient(_token, _user);
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
    }
}