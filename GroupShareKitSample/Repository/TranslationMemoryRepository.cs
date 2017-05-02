using System;
using System.Collections.Generic;
using System.Linq;
using System.Security.Principal;
using System.Threading.Tasks;
using Sdl.Community.GroupShareKit.Models.Response;
using Sdl.Community.GroupShareKit.Models.Response.TranslationMemory;
using Filter = GroupShareKitSample.Models.Filter;
using TranslationMemory = GroupShareKitSample.Models.TranslationMemory;


namespace GroupShareKitSample.Repository
{
    public class TranslationMemoryRepository
    {
        private readonly IPrincipal _user;
        private readonly string _token;

        public TranslationMemoryRepository(IPrincipal user)
        {
            _user = user;
            _token = Helper.HelperMethods.GetToken(user);
        }

        public async Task<List<TranslationMemory>>GetTms()
        {
            var gsClient = await Helper.HelperMethods.GetCurrentGsClient(_token, _user);
            var gsTranslationMemories = await gsClient.TranslationMemories.GetTms();
            var kitTm = new List<TranslationMemory>();
            foreach (var tm in gsTranslationMemories.Items)
            {
                var translationMemory = new TranslationMemory
                {
                    Id = tm.TranslationMemoryId,
                    Name = tm.Name
                };
                kitTm.Add(translationMemory);
            }
            return kitTm;
        }

        public async Task<List<Filter>> FilterAsPlainText(string searchedUnit,string tmId, string sourceCode, string targetCode)
        {
            var gsClient = await Helper.HelperMethods.GetCurrentGsClient(_token, _user);
            
            var languageDetails = new LanguageDetailsRequest("", sourceCode, searchedUnit, targetCode);
            var tmDetails = new TranslationMemoryDetailsRequest(new Guid(tmId), 0, 50);
            var gsUnits = await gsClient.TranslationMemories.FilterAsPlainText(languageDetails, tmDetails, true, true);
            var searchResults = new List<Filter>();
            foreach (var unit in gsUnits)
            {
                var result = new Filter
                {
                    SourceText = unit.Source,
                    TargetText = unit.Target
                };
                searchResults.Add(result);
            }
            return searchResults;
        }
    }
}