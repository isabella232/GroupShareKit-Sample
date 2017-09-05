using System;
using System.Collections.Generic;
using System.Globalization;
using System.Linq;
using System.Security.Principal;
using System.Threading.Tasks;
using Sdl.Community.GroupShareKit.Models.Response;
using Sdl.Community.GroupShareKit.Models.Response.TranslationMemory;
using Filter = GroupShareKitSample.Models.Filter;
using LanguageDirection = GroupShareKitSample.Models.LanguageDirection;
using TranslationMemory = GroupShareKitSample.Models.TranslationMemory;


namespace GroupShareKitSample.Repository
{
    public class TranslationMemoryRepository
    {
        private readonly IPrincipal user;

        public TranslationMemoryRepository(IPrincipal user)
        {
            this.user = user;
        }

        public async Task<List<TranslationMemory>>GetTms()
        {
            var gsClient = Helper.HelperMethods.GetCurrentGsClient(user);
            var gsTranslationMemories = await gsClient.TranslationMemories.GetTms();
            var kitTm = new List<TranslationMemory>();
            
            foreach (var tm in gsTranslationMemories.Items)
            {
                var translationMemory = new TranslationMemory
                {
                    Id = tm.TranslationMemoryId,
                    Name = tm.Name,
                    LanguageDirections = new List<LanguageDirection>()
                };
                foreach (var languageDirection in tm.LanguageDirections)
                {
                    var language = new LanguageDirection
                    {
                        SourceCode = languageDirection.Source,
                        SourceDisplayName = new CultureInfo(languageDirection.Source).DisplayName,
                        TargetCode = languageDirection.Target,
                        TargetDisplayName = new CultureInfo(languageDirection.Target).DisplayName
                    };
                    translationMemory.LanguageDirections.Add(language);
                }
               
                kitTm.Add(translationMemory);
            }
         
            return kitTm;
        }

        public async Task<List<Filter>> ConcordanceSearch(string sourceSearchedUnit, string targetSearchedUnit, string tmId, string sourceCode, string targetCode)
        {
            var gsClient = Helper.HelperMethods.GetCurrentGsClient(user);
            ConcordanceSearchRequest concordanceSearchRequest;

            //check if is target concordance search
            if (!string.IsNullOrEmpty(targetSearchedUnit))
            {
                var concordanceSearchSettings = new ConcordanceSearchSettings
                {
                    IsTargetConcodanceSearch = true
                };
                 concordanceSearchRequest = new ConcordanceSearchRequest(new Guid(tmId), targetSearchedUnit, sourceCode, targetCode,concordanceSearchSettings);
            }
            else
            {
                concordanceSearchRequest = new ConcordanceSearchRequest(new Guid(tmId), sourceSearchedUnit, sourceCode, targetCode);
            }
            

            var concordanceResponse = await gsClient.TranslationMemories.ConcordanceSearchAsPlainText(concordanceSearchRequest);

            var searchResults = new List<Filter>();
            foreach (var unit in concordanceResponse)
            {
                var result = new Filter
                {
                    SourceText = unit.Source,
                    TargetText = unit.Target,
                    Penalty = unit.MatchScore
                };
                searchResults.Add(result);
            }
            return searchResults;


        }

        public async Task<List<Filter>> FilterAsPlainText(string sourceSearchedUnit,string targetSearchedUnit, string tmId, string sourceCode, string targetCode)
        {
            var gsClient = Helper.HelperMethods.GetCurrentGsClient(user);
            
            var languageDetails = new LanguageDetailsRequest(sourceSearchedUnit, sourceCode, targetSearchedUnit, targetCode);
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