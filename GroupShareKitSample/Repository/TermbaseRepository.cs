using System;
using System.Collections.Generic;
using System.Linq;
using System.Security.Principal;
using System.Threading.Tasks;
using System.Web;
using System.Web.Script.Serialization;
using GroupShareKitSample.Models;
using Newtonsoft.Json;
using Sdl.Community.GroupShareKit.Clients;
using Sdl.Community.GroupShareKit.Models.Response;
using Termbase = GroupShareKitSample.Models.Termbase;


namespace GroupShareKitSample.Repository
{
    public class TermbaseRepository
    {
        private readonly IPrincipal user;

        public TermbaseRepository(IPrincipal user)
        {
            this.user = user;
        }

        public async Task<List<Termbase>> GetTermbases()
        {
                var gsClient = Helper.HelperMethods.GetCurrentGsClient(user);
                var gsTermbases = await gsClient.Terminology.GetTermbases();

                var termbases = new List<Termbase>();
                foreach (var gsTermbase in gsTermbases.Termbases)
                {
                    List<LanguageDirection> languageDirections;
                    try
                    {
                        languageDirections = await GetLanguagesForTb(gsTermbase.Id);
                    }
                    catch { continue;}

                    var termbase = new Termbase
                    {
                        Id = gsTermbase.Id,
                        Name = gsTermbase.Name,
                        LanguageDirections = languageDirections
                    };
                    termbases.Add(termbase);
                }
                return termbases;
        }

        public async Task<List<LanguageDirection>> GetLanguagesForTb(string tbId)
        {
                var gsClient = Helper.HelperMethods.GetCurrentGsClient(user);
                TermbaseResponse gsTermbase;
                try
                {
                    gsTermbase = await gsClient.Terminology.GetTermbaseById(tbId);
                }
                catch(Exception ex)
                {
                    throw new Exception($"{ex.Message} Source: {ex.Source}");
                }
                var languageDirection = new List<LanguageDirection>();
                foreach (var language in gsTermbase.Termbase.Languages)
                {
                    var lgDirection = new GroupShareKitSample.Models.LanguageDirection
                    {
                        SourceCode = language.Code,
                        SourceDisplayName = language.Name

                    };
                    languageDirection.Add(lgDirection);
                }
                return languageDirection;
        }

        public async Task<SearchResponse> Search(SearchTerm search)
        {
            var gsClient = Helper.HelperMethods.GetCurrentGsClient(user);
            var request = new SearchTermRequest(search.TermbaseId, search.Language, search.SearchedTerm);

            var term = await gsClient.Terminology.SearchTerm(request);
            return term;
        }

        public async Task<KitConcept> ConceptDetails(string termbaseId, string conceptId)
        {
            var gsClient = Helper.HelperMethods.GetCurrentGsClient(user);
            var gsConcept = await gsClient.Terminology.GetConcept(termbaseId, conceptId);

            var json = JsonConvert.SerializeObject(gsConcept.Concept);
            var kitConcept = JsonConvert.DeserializeObject<KitConcept>(json);
            return kitConcept;
        }

        public async Task<ConceptDetails> GetConcept(string termbaseId, string conceptId)
        {
            var gsClient = Helper.HelperMethods.GetCurrentGsClient(user);
            var gsConcept = await gsClient.Terminology.GetConcept(termbaseId, conceptId);
            return gsConcept;
        }

        public async Task UpdateConcept(string termbaseId, ConceptDetails concept)
        {
            var gsClient = Helper.HelperMethods.GetCurrentGsClient(user);
            await gsClient.Terminology.EditConcept(termbaseId, concept);
        }

        public async Task DeleteConcept(string termbaseId, string conceptId)
        {
            var gsClient = Helper.HelperMethods.GetCurrentGsClient(user);
            await gsClient.Terminology.DeleteConcept(termbaseId, conceptId);
        }
    }
}