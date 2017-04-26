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
        private readonly IPrincipal _user;
        private readonly string _token;

        public TermbaseRepository(IPrincipal user)
        {
            _user = user;
            _token = Helper.HelperMethods.GetToken(user);
        }

        public async Task<List<Termbase>> GetTermbases()
        {
            var gsClient = await Helper.HelperMethods.GetCurrentGsClient(_token, _user);
            var gsTermbases = await gsClient.TermBase.GetTermbases();
           
            var termbases = new List<Termbase>();
            foreach (var gsTermbase in gsTermbases.Termbases)
            {
                var termbase = new Termbase
                {
                    Id = gsTermbase.Id,
                    Name = gsTermbase.Name
                };
                termbases.Add(termbase);
            }
            return termbases;
        }

        public async Task<SearchResponse> Search(SearchTerm search)
        {
            var gsClient = await Helper.HelperMethods.GetCurrentGsClient(_token, _user);
            var request = new SearchTermRequest(search.TermbaseId, search.SourceLanguage, search.SearchedTerm,
                search.TargetLanguage);

            var term = await gsClient.TermBase.SearchTerm(request);
            return term;
        }

        public async Task<KitConcept> ConceptDetails(string termbaseId, string conceptId)
        {
            var gsClient = await Helper.HelperMethods.GetCurrentGsClient(_token, _user);
            var gsConcept = await gsClient.TermBase.GetConcept(termbaseId, conceptId);

            var json = JsonConvert.SerializeObject(gsConcept.Concept);
            var kitConcept = JsonConvert.DeserializeObject<KitConcept>(json);
            return kitConcept;
        } 
    }
}