using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using System.Web;
using System.Web.Mvc;
using GroupShareKitSample.Models;
using GroupShareKitSample.Repository;
using Sdl.Community.GroupShareKit.Models.Response;

namespace GroupShareKitSample.Controllers
{
    public class TermbaseController : Controller
    {
        private TermbaseViewModel _viewModel=new TermbaseViewModel();
        private static TermbaseRepository _termRepository;
        // GET: Termbase
        public async Task<ActionResult> Index()
        {
            _termRepository = new TermbaseRepository(HttpContext.User);
            var termbases = await _termRepository.GetTermbases();
            _viewModel.Search = new SearchTerm();
            _viewModel.Termbases = termbases;

            return View(_viewModel);
        }

       // [HttpPost]
     //  [HttpGet]
        public async Task<ActionResult> Search(string term,string sourceLanguage, string targetLanguage, string termbaseId)
        {

            var searchedTerm = new SearchTerm
            {
                SearchedTerm = term,
                SourceLanguage = sourceLanguage,
                TargetLanguage = targetLanguage,
                TermbaseId = termbaseId
            };
            var searchResult = await _termRepository.Search(searchedTerm);
            var tbVm = new TermbaseViewModel
            {
                SearchResult = searchResult.Terms
            };
            return PartialView("SearchResult", tbVm);
        }

        public async Task<ActionResult> TermDetails(string termbaseId, string conceptId)
        {
            var details = await _termRepository.ConceptDetails(termbaseId, conceptId);
            return PartialView("TermDetails");
        }
        [HttpPost]
        public void Edit(string termbaseId,string conceptId)
        {
           // return null;
        } 
    }
}