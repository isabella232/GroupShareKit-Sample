using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using System.Web;
using System.Web.Mvc;
using GroupShareKitSample.Models;
using GroupShareKitSample.Repository;

namespace GroupShareKitSample.Controllers
{
    public class TranslationMemoryController : Controller
    {
        private  TranslationMemoryViewModel _tmVm = new TranslationMemoryViewModel();
        private static TranslationMemoryRepository _tmRepository;
        // GET: TranslationMemory
        public async Task<ActionResult> Index()
        {
            _tmRepository = new TranslationMemoryRepository(HttpContext.User);
            var translationMemories = await _tmRepository.GetTms();
            _tmVm.TranslationMemories = translationMemories;
            return View(_tmVm);
        }

        public async Task<ActionResult> SearchTerm(string sourceSearchedUnit,string targetSearchedUnit, 
            string tmId, string sourceCode, string targetCode)
        {
            var terms = await _tmRepository.FilterAsPlainText(sourceSearchedUnit,targetSearchedUnit, tmId,sourceCode,targetCode);
            _tmVm.SearchResult = terms;
            return PartialView("TmSearchResult",_tmVm);
        }

        public async Task<ActionResult> ConcordanceSearch(string sourceSearchedUnit, string targetSearchedUnit,
            string tmId, string sourceCode, string targetCode)
        {
            var concordanceResult = await _tmRepository.ConcordanceSearch(sourceSearchedUnit, targetSearchedUnit, tmId, sourceCode, targetCode);
            _tmVm.SearchResult = concordanceResult;
            return PartialView("TmSearchResult", _tmVm);
        }
    }
}