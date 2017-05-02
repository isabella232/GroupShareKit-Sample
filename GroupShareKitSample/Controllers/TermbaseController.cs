using System;
using System.Collections.Generic;
using System.Linq;
using System.ServiceModel.Description;
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
            return PartialView("TermDetails",details);
        }

        public async Task<ActionResult> AddTerm(string termbaseId,string conceptId,string newTerm,string languageCode)
        {
            var concept = await _termRepository.GetConcept(termbaseId, conceptId);

            var term = new Sdl.Community.GroupShareKit.Models.Response.TermbaseTerms
            {
                Attributes = null,

                Transactions = new List<Sdl.Community.GroupShareKit.Models.Response.Transactions>
                {
                    new Sdl.Community.GroupShareKit.Models.Response.Transactions
                    {
                        DateTime = DateTime.Now,
                        Id = null,
                        Username = "",
                        Details = new Sdl.Community.GroupShareKit.Models.Response.TransactionsDetails
                        {
                            User = "",
                            Type = "Create"
                        }

                    }
                },
                Text = newTerm
            };
            concept.Concept.Languages.First(l => l.Language.Name == languageCode).Terms.Add(term);
           
            await _termRepository.UpdateConcept(termbaseId,concept);
            var details = await _termRepository.ConceptDetails(termbaseId, conceptId);

            return PartialView("TermDetails", details);
        }

        public async Task<ActionResult> DeleteConcept(string termbaseId, string conceptId, string term, string sourceLanguage, string targetLanguage)
        {
            await _termRepository.DeleteConcept(termbaseId, conceptId);
            return await Search(term, sourceLanguage, targetLanguage, termbaseId);
        }
    }
}