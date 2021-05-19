
using FundooManager.NotesManager;
using FundooModel.Notes;
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
        [HttpPost]
        public ActionResult AddNotes(Notes notes)
        {
            try
            {
                Task<Notes> result = this.manager.AddNotes(notes);
                if (result.Result != null)
                {
                    return this.Ok(new { Status = true, Message = "Added Successfully", Response = result.Result});
                }
                return this.BadRequest(new { Status = false, Message = "Not added", Response = result.Result });
            }
            catch(Exception e)
            {
                return this.BadRequest(new { Status = false, Message = "Exception", Response = e });
            }
        }
        [HttpDelete]
        public ActionResult DeleteNotes(int id)
        {
            try
            {
                Task<Notes> result = this.manager.DeleteNotes(id);
                if (result.Result != null)
                {
                    return this.Ok(new { Status = true, Message = "Moved to trash Successfully", Response = result.Result });
                }
                return this.BadRequest(new { Status = false, Message = "Not Deleted", Response = result.Result });
            }
            catch (Exception e)
            {
                return this.BadRequest(new { Status = false, Message = "Exception", Response = e });
            }
        }
        [HttpPut]
        public ActionResult UpdateNotes(Notes notes)
        {
            try
            {
                Task<Notes> result = this.manager.UpdateNotes(notes);
                if (result.Result != null)
                {
                    return this.Ok(new { Status = true, Message = "Updated Successfully", Response = result.Result });
                }
                return this.BadRequest(new { Status = false, Message = "Not Updated", Response = result.Result });
            }
            catch (Exception e)
            {
                return this.BadRequest(new { Status = false, Message = "Exception", Response = e });
            }
        }
        [HttpGet]
        public ActionResult GetAllNotes()
        {
            try
            {
                var result = this.manager.GetAllNotes();
                if (result.Result != null)
                {
                    return this.Ok(new { Status = true, Message = "Lists", Response = result.Result });
                }
                return this.BadRequest(new { Status = false, Message = "No list available", Response = result.Result });
            }
            catch(Exception e)
            {
                return this.BadRequest(new { Status = false, Message = "Exception", Response = e });
            }
        }
        [HttpGet]
        [Route("{email}")]
        public ActionResult GetAllNotesByEmail(string email)
        {
            try
            {
                var result = this.manager.GetAllNotesByEmail(email);
                if (result.Result != null)
                {
                    return this.Ok(new { Status = true, Message = "Lists", Response = result.Result });
                }
                return this.BadRequest(new { Status = false, Message = "No list available", Response = result.Result });
            }
            catch (Exception e)
            {
                return this.BadRequest(new { Status = false, Message = "Exception", Response = e });
            }
        }
        [HttpDelete]
        [Route("deleteTrash/{id}")]
        public ActionResult DeleteFromTrash(int id)
        {
            try
            {
                Task<Notes> result = this.manager.DeleteFromTrash(id);
                if (result.Result != null)
                {
                    return this.Ok(new { Status = true, Message = "Deleted from Trash Successfully", Response = result.Result });
                }
                return this.BadRequest(new { Status = false, Message = "Not Deleted", Response = result.Result });
            }
            catch (Exception e)
            {
                return this.BadRequest(new { Status = false, Message = "Exception", Response = e });
            }
        }
        [HttpPost]
        [Route("addCollaborater")]
        public ActionResult AddCollaborater(Collaborater collaborater)
        {
            try
            {
                Task<Collaborater> result = this.manager.AddCollaborater(collaborater);
                if (result.Result != null)
                {
                    return this.Ok(new { Status = true, Message = "Collaborater added Successfully", Response = result.Result });
                }
                return this.BadRequest(new { Status = false, Message = "Collaborater not added", Response = result.Result });
            }
            catch(Exception e)
            {
                return this.BadRequest(new { Status = false, Message = "Exception", Response = e });
            }
        }
        [HttpDelete]
        [Route("addCollaborater")]
        public ActionResult DeleteCollaborater(Collaborater collaborater)
        {
            try
            {
                Task<Collaborater> result = this.manager.DeleteCollaborater(collaborater);
                if (result.Result != null)
                {
                    return this.Ok(new { Status = true, Message = "Collaborater Deleted Successfully", Response = result.Result });
                }
                return this.BadRequest(new { Status = false, Message = "Collaborater not available", Response = result.Result });
            }
            catch (Exception e)
            {
                return this.BadRequest(new { Status = false, Message = "Exception", Response = e });
            }
        }
    }
}
