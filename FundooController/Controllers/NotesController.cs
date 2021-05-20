/////------------------------------------------------------------------------
////<copyright file="NotesController.cs" company="BridgeLabz">
////author="Bhushan"
////</copyright>
////-------------------------------------------------------------------------

namespace FundooController.Controllers
{
    using System;
    using System.Collections.Generic;
    using System.Linq;
    using System.Threading.Tasks;
    using CloudinaryDotNet.Actions;
    using FundooManager.NotesManager;
    using FundooModel.Notes;
    using LoggerService;
    using Microsoft.AspNetCore.Mvc;

    /// <summary>
    /// Notes Controller
    /// </summary>
    [Route("api/[controller]")]
    [ApiController]
    public class NotesController : Controller
    {
        /// <summary>
        /// Notes manager interface
        /// </summary>
        private readonly INotesManager manager;

        /// <summary>
        /// logger manager interface
        /// </summary>
        private readonly ILoggerManager logger;

        /// <summary>
        /// Notes controller
        /// </summary>
        /// <param name="notesManager">Notes Manager</param>
        /// <param name="loggerManager">logger manager</param>
        public NotesController(INotesManager notesManager, ILoggerManager loggerManager)
        {
            this.manager = notesManager;
            this.logger = loggerManager;
        }

        /// <summary>
        /// Add notes
        /// </summary>
        /// <param name="notes">Notes parameter</param>
        /// <returns>Action result</returns>
        [HttpPost]
        public ActionResult AddNotes(Notes notes)
        {
            try
            {
                Task<Notes> result = this.manager.AddNotes(notes);
                if (result.Result != null)
                {
                    this.logger.LogInfo("Add Notes Successfully " + result.Result.Title + ", Status : OK");
                    this.logger.LogDebug("Debug Successfull : Add Notes");
                    return this.Ok(new { Status = true, Message = "Added Successfully", Response = result.Result });
                }

                this.logger.LogError("Notes Not added, Status : Bad Request");
                return this.BadRequest(new { Status = false, Message = "Not added", Response = result.Result });
            }
            catch (Exception e)
            {
                this.logger.LogWarn("Exception " + e + ", Status : Bad Request");
                return this.BadRequest(new { Status = false, Message = "Exception", Response = e });
            }
        }

        /// <summary>
        /// Delete notes by Identity number
        /// </summary>
        /// <param name="id">Parameter ID</param>
        /// <returns>Action result</returns>
        [HttpDelete]
        public ActionResult DeleteNotes(int id)
        {
            try
            {
                Task<Notes> result = this.manager.DeleteNotes(id);
                if (result.Result != null)
                {
                    this.logger.LogInfo("Moved to trash Successfully " + result.Result.Title + ", Status : OK");
                    this.logger.LogDebug("Debug Successfull : Delete Notes");
                    return this.Ok(new { Status = true, Message = "Moved to trash Successfully", Response = result.Result });
                }

                this.logger.LogError("Notes Not Moved to trash, Status : Bad Request");
                return this.BadRequest(new { Status = false, Message = "Not Deleted", Response = result.Result });
            }
            catch (Exception e)
            {
                this.logger.LogWarn("Exception " + e + ", Status : Bad Request");
                return this.BadRequest(new { Status = false, Message = "Exception", Response = e });
            }
        }

        /// <summary>
        /// Update notes
        /// </summary>
        /// <param name="notes">Notes parameter</param>
        /// <returns>Action result</returns>
        [HttpPut]
        public ActionResult UpdateNotes(Notes notes)
        {
            try
            {
                Task<Notes> result = this.manager.UpdateNotes(notes);
                if (result.Result != null)
                {
                    this.logger.LogInfo("Notes Updated Successfully " + result.Result.Title + ", Status : OK");
                    this.logger.LogDebug("Debug Successfull : Update Notes");
                    return this.Ok(new { Status = true, Message = "Updated Successfully", Response = result.Result });
                }

                this.logger.LogError("Notes Not Updated, Status : Bad Request");
                return this.BadRequest(new { Status = false, Message = "Not Updated", Response = result.Result });
            }
            catch (Exception e)
            {
                this.logger.LogWarn("Exception " + e + ", Status : Bad Request");
                return this.BadRequest(new { Status = false, Message = "Exception", Response = e });
            }
        }

        /// <summary>
        /// Get all notes
        /// </summary>
        /// <returns>Action result</returns>
        [HttpGet]
        public ActionResult GetAllNotes()
        {
            try
            {
                var result = this.manager.GetAllNotes();
                if (result.Result != null)
                {
                    this.logger.LogInfo("List of notes, Status : OK");
                    this.logger.LogDebug("Debug Successfull : Get all Notes");
                    return this.Ok(new { Status = true, Message = "Lists", Response = result.Result });
                }

                this.logger.LogError("No lists available, Status : Bad Request");
                return this.BadRequest(new { Status = false, Message = "No list available", Response = result.Result });
            }
            catch (Exception e)
            {
                this.logger.LogWarn("Exception " + e + ", Status : Bad Request");
                return this.BadRequest(new { Status = false, Message = "Exception", Response = e });
            }
        }

        /// <summary>
        /// Get all notes by email
        /// </summary>
        /// <param name="email">parameter email</param>
        /// <returns>Action result</returns>
        [HttpGet]
        [Route("{email}")]
        public ActionResult GetAllNotesByEmail(string email)
        {
            try
            {
                var result = this.manager.GetAllNotesByEmail(email);
                if (result.Result != null)
                {
                    this.logger.LogInfo("List of notes by email, Status : OK");
                    this.logger.LogDebug("Debug Successfull : Get all Notes by email");
                    return this.Ok(new { Status = true, Message = "Lists", Response = result.Result });
                }

                this.logger.LogError("No lists available, Status : Bad Request");
                return this.BadRequest(new { Status = false, Message = "No list available", Response = result.Result });
            }
            catch (Exception e)
            {
                this.logger.LogWarn("Exception " + e + ", Status : Bad Request");
                return this.BadRequest(new { Status = false, Message = "Exception", Response = e });
            }
        }

        /// <summary>
        /// Delete from trash
        /// </summary>
        /// <param name="id">parameter ID</param>
        /// <returns>Action result</returns>
        [HttpDelete]
        [Route("deleteTrash/{id}")]
        public ActionResult DeleteFromTrash(int id)
        {
            try
            {
                Task<Notes> result = this.manager.DeleteFromTrash(id);
                if (result.Result != null)
                {
                    this.logger.LogInfo("Deleted from Trash Successfully, Status : OK");
                    this.logger.LogDebug("Debug Successfull : Deleted from Trash");
                    return this.Ok(new { Status = true, Message = "Deleted from Trash Successfully", Response = result.Result });
                }

                this.logger.LogError("Not deleted from trash, Status : Bad Request");
                return this.BadRequest(new { Status = false, Message = "Not Deleted", Response = result.Result });
            }
            catch (Exception e)
            {
                this.logger.LogWarn("Exception " + e + ", Status : Bad Request");
                return this.BadRequest(new { Status = false, Message = "Exception", Response = e });
            }
        }

        /// <summary>
        /// Add collaborater
        /// </summary>
        /// <param name="collaborater">parameter collaborator</param>
        /// <returns>Action result</returns>
        [HttpPost]
        [Route("addCollaborater")]
        public ActionResult AddCollaborater(Collaborater collaborater)
        {
            try
            {
                Task<Collaborater> result = this.manager.AddCollaborater(collaborater);
                if (result.Result != null)
                {
                    this.logger.LogInfo("Collaborater added Successfully, Status : OK");
                    this.logger.LogDebug("Debug Successfull : Collabration");
                    return this.Ok(new { Status = true, Message = "Collaborater added Successfully", Response = result.Result });
                }

                this.logger.LogError("Collaborater not added, Status : Bad Request");
                return this.BadRequest(new { Status = false, Message = "Collaborater not added", Response = result.Result });
            }
            catch (Exception e)
            {
                this.logger.LogWarn("Exception " + e + ", Status : Bad Request");
                return this.BadRequest(new { Status = false, Message = "Exception", Response = e });
            }
        }

        /// <summary>
        /// Delete collaborater
        /// </summary>
        /// <param name="collaborater">parameter collaborater</param>
        /// <returns>Action result</returns>
        [HttpDelete]
        [Route("deleteCollaborater")]
        public ActionResult DeleteCollaborater(Collaborater collaborater)
        {
            try
            {
                Task<Collaborater> result = this.manager.DeleteCollaborater(collaborater);
                if (result.Result != null)
                {
                    this.logger.LogInfo("Delete collabrator Successfully, Status : OK");
                    this.logger.LogDebug("Debug Successfull : Delete collabrator");
                    return this.Ok(new { Status = true, Message = "Collaborater Deleted Successfully", Response = result.Result });
                }

                this.logger.LogError("Collaborater not available, Status : Bad Request");
                return this.BadRequest(new { Status = false, Message = "Collaborater not available", Response = result.Result });
            }
            catch (Exception e)
            {
                this.logger.LogWarn("Exception " + e + ", Status : Bad Request");
                return this.BadRequest(new { Status = false, Message = "Exception", Response = e });
            }
        }

        /// <summary>
        /// Upload image
        /// </summary>
        /// <param name="noteId">parameter note ID</param>
        /// <param name="imagePath">parameter image path</param>
        /// <returns>Action result</returns>
        [HttpPost]
        [Route("uploadImage")]
        public ActionResult UploadImage(int noteId, string imagePath)
        {
            try
            {
                Task<ImageUploadResult> result = this.manager.UploadImage(noteId, imagePath);
                if (result.Result != null)
                {
                    this.logger.LogInfo("Image Uploaded Successfully, Status : OK");
                    this.logger.LogDebug("Debug Successfull : Delete collabrator");
                    return this.Ok(new { Status = true, Message = "Image Uploaded Successfully", Response = result.Result });
                }

                this.logger.LogError("image not uploaded, Status : Bad Request");
                return this.BadRequest(new { Status = false, Message = "image not uploaded", Response = result.Result });
            }
            catch (Exception e)
            {
                this.logger.LogWarn("Exception " + e + ", Status : Bad Request");
                return this.BadRequest(new { Status = false, Message = "Exception", Response = e });
            }
        }

        [HttpPut]
        [Route("setIsTrash/{id}")]
        public ActionResult SetIsTrash(int id)
        {
            try
            {
                Task<bool> response = this.manager.SetIsTrash(id);
                if (response.Result == true)
                {
                    this.logger.LogInfo("Set trash Successfully, Status : OK");
                    this.logger.LogDebug("Set Trash");
                    return this.Ok(new { Status = true, Message = "Set trash successfully", Response = response.Result });
                }
                this.logger.LogError("Trash Not Setted");
                return this.BadRequest(new { Status = false, Message = "Trash Not Setted", Response = response.Result });
            }
            catch (Exception e)
            {
                this.logger.LogWarn("Exception " + e + ", Status : Bad Request");
                return this.BadRequest(new { Status = false, Message = "Exception", Response = e });
            }
        }
        [HttpPut]
        [Route("resetIsTrash/{id}")]
        public ActionResult ResetIsTrash(int id)
        {
            try
            {
                Task<bool> response = this.manager.ResetIsTrash(id);
                if (response.Result == true)
                {
                    this.logger.LogInfo("Reset trash Successfully, Status : OK");
                    this.logger.LogDebug("Reset Trash");
                    return this.Ok(new { Status = true, Message = "Reset trash successfully", Response = response.Result });
                }
                this.logger.LogError("Trash Not resetted");
                return this.BadRequest(new { Status = false, Message = "Trash Not resetted", Response = response.Result });
            }
            catch (Exception e)
            {
                this.logger.LogWarn("Exception " + e + ", Status : Bad Request");
                return this.BadRequest(new { Status = false, Message = "Exception", Response = e });
            }
        }
        [HttpPut]
        [Route("resetArchive/{id}")]
        public ActionResult ResetArchive(int id)
        {
            try
            {
                Task<bool> response = this.manager.ResetArchive(id);
                if (response.Result == true)
                {
                    this.logger.LogInfo("Reset Archive Successfully, Status : OK");
                    this.logger.LogDebug("Reset Archive");
                    return this.Ok(new { Status = true, Message = "Reset Archive successfully", Response = response.Result });
                }
                this.logger.LogError("Archive Not resetted");
                return this.BadRequest(new { Status = false, Message = "Archive Not resetted", Response = response.Result });
            }
            catch (Exception e)
            {
                this.logger.LogWarn("Exception " + e + ", Status : Bad Request");
                return this.BadRequest(new { Status = false, Message = "Exception", Response = e });
            }
        }
        [HttpPut]
        [Route("setArchive/{id}")]
        public ActionResult SetArchive(int id)
        {
            try
            {
                Task<bool> response = this.manager.SetArchive(id);
                if (response.Result == true)
                {
                    this.logger.LogInfo("Set Archive Successfully, Status : OK");
                    this.logger.LogDebug("Set Archive");
                    return this.Ok(new { Status = true, Message = "Set Archive successfully", Response = response.Result });
                }
                this.logger.LogError("Archive Not setted");
                return this.BadRequest(new { Status = false, Message = "Archive Not setted", Response = response.Result });
            }
            catch (Exception e)
            {
                this.logger.LogWarn("Exception " + e + ", Status : Bad Request");
                return this.BadRequest(new { Status = false, Message = "Exception", Response = e });
            }
        }
        [HttpPut]
        [Route("resetPin/{id}")]
        public ActionResult ResetPin(int id)
        {
            try
            {
                Task<bool> response = this.manager.ResetPin(id);
                if (response.Result == true)
                {
                    this.logger.LogInfo("Reset Pin Successfully, Status : OK");
                    this.logger.LogDebug("Reset Pin");
                    return this.Ok(new { Status = true, Message = "Reset Pin successfully", Response = response.Result });
                }
                this.logger.LogError("Pin Not resetted");
                return this.BadRequest(new { Status = false, Message = "Pin Not resetted", Response = response.Result });
            }
            catch (Exception e)
            {
                this.logger.LogWarn("Exception " + e + ", Status : Bad Request");
                return this.BadRequest(new { Status = false, Message = "Exception", Response = e });
            }
        }
    }
}
