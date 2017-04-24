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

            var searchResult = (List<Term>) TempData["searchResult"];
            if (searchResult != null)
            {
                _viewModel.SearchResult = searchResult;
                return View(_viewModel);
            }

            return View(_viewModel);
        }

        [HttpPost]
        public async Task<ActionResult> Search(TermbaseViewModel searchedTerm)
        {
            var selectedTb = Request["tbselect"];
            searchedTerm.Search.TermbaseId = selectedTb;
            var searchResult = await _termRepository.Search(searchedTerm.Search);
            TempData["searchResult"] = searchResult.Terms;
            return RedirectToAction("Index","Termbase");
        }

        [HttpPost]
        public void Edit(string termbaseId,string conceptId)
        {
           // return null;
        } 
    }
}