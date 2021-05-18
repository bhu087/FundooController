using FundooManager.Notes;
using Microsoft.AspNetCore.Mvc;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace FundooController.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class NotesController : Controller
    {
        public readonly INotesManager manager;
        public NotesController(INotesManager notesManager)
        {
            manager = notesManager;
        }
        public IActionResult Index()
        {
            return View();
        }
    }
}
