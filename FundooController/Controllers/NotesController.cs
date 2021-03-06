/////------------------------------------------------------------------------
////<copyright file="NotesController.cs" company="BridgeLabz">
////author="Bhushan"
////</copyright>
////-------------------------------------------------------------------------

namespace FundooController.Controllers
{
    using System;
    using System.Collections.Generic;
    using System.IdentityModel.Tokens.Jwt;
    using System.Linq;
    using System.Security.Claims;
    using System.Threading.Tasks;
    using CloudinaryDotNet.Actions;
    using FundooManager.NotesManager;
    using FundooModel.Notes;
    using LoggerService;
    using Microsoft.AspNetCore.Authentication;
    using Microsoft.AspNetCore.Authorization;
    using Microsoft.AspNetCore.Cors;
    using Microsoft.AspNetCore.Http;
    using Microsoft.AspNetCore.Mvc;

    /// <summary>
    /// Notes Controller
    /// </summary>
    [Route("api/[controller]")]
    [ApiController]
    [Authorize(Roles = "User")]
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
        /// <returns>Action result</returns>archive
        [HttpPost]
        public ActionResult AddNotes(Notes notes)
        {
            var token = HttpContext.Request?.Headers["authorization"];
            string tok = token.ToString();
            string[] tokenArray = tok.Split(" ");
            var handler = new JwtSecurityTokenHandler();
            var jsonToken = handler.ReadToken(tokenArray[1]);
            var tokenS = jsonToken as JwtSecurityToken;
            var email = tokenS.Claims.First(claim => claim.Type == "Email").Value;
            var user = tokenS.Claims.First(claim => claim.Type == "Id").Value;
            //string jwt = HttpContext.Session.GetString("Bearer");
            Task<Notes> result = null;
            try
            {
                if (email != null)

                {
                    notes.UserID = int.Parse(user);
                    //string email = this.TokenEmail();
                    result = this.manager.AddNotes(notes, email);
                    if (result.Result != null)
                    {
                        this.logger.LogInfo("Add Notes Successfully " + result.Result.Title + ", Status : OK");
                        this.logger.LogDebug("Debug Successfull : Add Notes");
                        return this.Ok(new { Status = true, Message = "Added Successfully", Response = result.Result });
                    }
                }

                this.logger.LogError("Notes Not added, Status : Bad Request");
                return this.BadRequest(new { Status = false, Message = "Not added", Response = result });
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
        /// <param name="noteId">Parameter ID</param>
        /// <returns>Action result</returns>
        [HttpPut("{noteId}")]
        public ActionResult DeleteNotes(int noteId)
        {
            try
            {
                int userId = this.TokenUserId();
                Task<Notes> result = this.manager.DeleteNotes(noteId, userId);
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
                int userId = this.GetUserId();
                Task<Notes> result = this.manager.UpdateNotes(notes, userId);
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
        [HttpGet("allNotes")]
        public ActionResult GetAllNotes()
        {
            try
            {
                var token = HttpContext.Request?.Headers["authorization"];
                string tok = token.ToString();
                string[] tokenArray = tok.Split(" ");
                var handler = new JwtSecurityTokenHandler();
                var jsonToken = handler.ReadToken(tokenArray[1]);
                var tokenS = jsonToken as JwtSecurityToken;
                int userId = int.Parse(tokenS.Claims.First(claim => claim.Type == "Id").Value);
                 
                var result = this.manager.GetAllNotes(userId);
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
        /// <param name="notesId">parameter ID</param>
        /// <returns>Action result</returns>
        [HttpDelete]
        [Route("deleteTrash/{notesId}")]
        public ActionResult DeleteFromTrash(int notesId)
        {
            try
            {
                int userId = this.TokenUserId();
                Task<Notes> result = this.manager.DeleteFromTrash(notesId, userId);
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
        public ActionResult AddCollaborater(Collaborator collaborater)
        {
            try
            {
                collaborater.SenderEmail = this.TokenEmail();
                collaborater.UserID = this.TokenUserId();
                Task<Collaborator> result = this.manager.AddCollaborater(collaborater);
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
        public ActionResult DeleteCollaborater(Collaborator collaborater)
        {
            try
            {
                Task<Collaborator> result = this.manager.DeleteCollaborater(collaborater);
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
        [HttpPut("uploadImage/{noteId}")]
        public ActionResult UploadImage(int noteId, string imagePath)
        {
            try
            {
                int userId = this.TokenUserId();
                Task<ImageUploadResult> result = this.manager.UploadImage(noteId, imagePath, userId);
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

        /// <summary>
        /// Set trash field
        /// </summary>
        /// <param name="notesId">input parameter is id</param>
        /// <returns>returns action result</returns>
        [HttpPut("setIsTrash/{notesId}")]
        public ActionResult SetIsTrash(int notesId)
        {
            try
            {
                int userId = this.GetUserId();
                Task<bool> response = this.manager.SetIsTrash(notesId, userId);
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

        /// <summary>
        /// Reset the trash
        /// </summary>
        /// <param name="noteId">Note id</param>
        /// <returns>Returns Action result</returns>
        [HttpPut("resetIsTrash/{noteId}")]
        public ActionResult ResetIsTrash(int noteId)
        {
            try
            {
                int userId = this.TokenUserId();
                Task<bool> response = this.manager.ResetIsTrash(noteId, userId);
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

        [HttpGet("archivedNotes")]
        public ActionResult GetArchive()
        {
            try
            {
                var token = HttpContext.Request?.Headers["authorization"];
                string tok = token.ToString();
                string[] tokenArray = tok.Split(" ");
                var handler = new JwtSecurityTokenHandler();
                var jsonToken = handler.ReadToken(tokenArray[1]);
                var tokenS = jsonToken as JwtSecurityToken;
                int userId = int.Parse(tokenS.Claims.First(claim => claim.Type == "Id").Value);

                var result = this.manager.GetArchivedNotes(userId);
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

        [HttpGet("trashNotes")]
        public ActionResult GetTrashNotes()
        {
            try
            {
                var token = HttpContext.Request?.Headers["authorization"];
                string tok = token.ToString();
                string[] tokenArray = tok.Split(" ");
                var handler = new JwtSecurityTokenHandler();
                var jsonToken = handler.ReadToken(tokenArray[1]);
                var tokenS = jsonToken as JwtSecurityToken;
                int userId = int.Parse(tokenS.Claims.First(claim => claim.Type == "Id").Value);

                var result = this.manager.GetTrashNotes(userId);
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
        /// Reset the Archive
        /// </summary>
        /// <param name="noteId">note id</param>
        /// <returns>Action result</returns>
        [HttpPut("resetArchive/{noteId}")]
        public ActionResult ResetArchive(int noteId)
        {
            try
            {
                int userId = this.TokenUserId();
                Task<bool> response = this.manager.ResetArchive(noteId, userId);
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

        /// <summary>
        /// Set archive
        /// </summary>
        /// <param name="noteId">parameter is id</param>
        /// <returns>returns action result</returns>
        [HttpPut("setArchive/{noteId}")]
        public ActionResult SetArchive(int noteId)
        {
            try
            {
                int userId = this.TokenUserId();
                Task<bool> response = this.manager.SetArchive(noteId, userId);
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

        /// <summary>
        /// Reset pin field in notes
        /// </summary>
        /// <param name="noteId">notes id</param>
        /// <returns>Action result</returns>
        [HttpPut("resetPin/{noteId}")]
        public ActionResult ResetPin(int noteId)
        {
            try
            {
                int userId = this.TokenUserId();
                Task<bool> response = this.manager.ResetPin(noteId, userId);
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

        /// <summary>
        /// Set the Pin
        /// </summary>
        /// <param name="noteId">Notes id</param>
        /// <returns>return action result</returns>
        [HttpPut("setPin/{noteId}")]
        public ActionResult SetPin(int noteId)
        {
            try
            {
                int userId = this.TokenUserId();
                Task<bool> response = this.manager.SetPin(noteId, userId);
                if (response.Result == true)
                {
                    this.logger.LogInfo("Set Pin Successfully, Status : OK");
                    this.logger.LogDebug("Set Pin");
                    return this.Ok(new { Status = true, Message = "Set Pin successfully", Response = response.Result });
                }
                this.logger.LogError("Pin Not setted");
                return this.BadRequest(new { Status = false, Message = "Pin Not setted", Response = response.Result });
            }
            catch (Exception e)
            {
                this.logger.LogWarn("Exception " + e + ", Status : Bad Request");
                return this.BadRequest(new { Status = false, Message = "Exception", Response = e });
            }
        }

        /// <summary>
        /// Add Remainder
        /// </summary>
        /// <param name="noteId">note note id</param>
        /// <param name="time">remainder time</param>
        /// <returns>return Action result</returns>
        [HttpPut("addRemainder/{noteId}")]
        public ActionResult AddRemainder(int noteId, string time)
        {
            try
            {
                int userId = this.TokenUserId();
                Task<bool> response = this.manager.AddRemainder(noteId, time, userId);
                if (response.Result == true)
                {
                    this.logger.LogInfo("Set Remainder Successfully, Status : OK");
                    this.logger.LogDebug("Set Pin");
                    return this.Ok(new { Status = true, Message = "Set Remainder successfully", Response = response.Result });
                }
                this.logger.LogError("Remainder Not setted");
                return this.BadRequest(new { Status = false, Message = "Remainder Not setted", Response = response.Result });
            }
            catch (Exception e)
            {
                this.logger.LogWarn("Exception " + e + ", Status : Bad Request");
                return this.BadRequest(new { Status = false, Message = "Exception", Response = e });
            }
        }

        /// <summary>
        /// Delete Remainder
        /// </summary>
        /// <param name="noteId">notes id</param>
        /// <returns>return action result</returns>
        [HttpPut("deleteRemainder/{noteId}")]
        public ActionResult DeleteRemainder(int noteId)
        {
            try
            {
                int userId = TokenUserId();
                Task<bool> response = this.manager.DeleteRemainder(noteId, userId);
                if (response.Result == true)
                {
                    this.logger.LogInfo("Delete Remainder Successfully, Status : OK");
                    this.logger.LogDebug("Delete Pin");
                    return this.Ok(new { Status = true, Message = "Delete Remainder successfully", Response = response.Result });
                }
                this.logger.LogError("Remainder Not Deleted");
                return this.BadRequest(new { Status = false, Message = "Remainder Not Deleted", Response = response.Result });
            }
            catch (Exception e)
            {
                this.logger.LogWarn("Exception " + e + ", Status : Bad Request");
                return this.BadRequest(new { Status = false, Message = "Exception", Response = e });
            }
        }

        /// <summary>
        /// Set color name to User Id
        /// </summary>
        /// <param name="noteId">Note Id</param>
        /// <param name="color">Note color
        /// </param>
        /// <returns>return action result</returns>
        [HttpPut("setColor/{noteId}/{color}")]
        public ActionResult SetColor(int noteId, string color)
        {
            try
            {
                int userId = TokenUserId();
                Task<bool> response = this.manager.SetColor(noteId, userId, color);
                if (response.Result == true)
                {
                    this.logger.LogInfo("set color Successfully, Status : OK");
                    this.logger.LogDebug("Set color");
                    return this.Ok(new { Status = true, Message = "Set color successfully", Response = response.Result });
                }
                this.logger.LogError("Color not setted");
                return this.BadRequest(new { Status = false, Message = "color not setted", Response = response.Result });
            }
            catch (Exception e)
            {
                this.logger.LogWarn("Exception " + e + ", Status : Bad Request");
                return this.BadRequest(new { Status = false, Message = "Exception", Response = e });
            }
        }

        /// <summary>
        /// Delete color
        /// </summary>
        /// <param name="noteId"></param>
        /// <returns>return action result</returns>
        [HttpPut("deleteColor/{noteId}")]
        public ActionResult DeleteColor(int noteId)
        {
            try
            {
                int userId = TokenUserId();
                Task<bool> response = this.manager.DeleteColor(noteId, userId);
                if (response.Result == true)
                {
                    this.logger.LogInfo("color delete Successfully, Status : OK");
                    this.logger.LogDebug("Deleted color");
                    return this.Ok(new { Status = true, Message = "Color delete successfully", Response = response.Result });
                }
                this.logger.LogError("Color not Deleted");
                return this.BadRequest(new { Status = false, Message = "color not Deleted", Response = response.Result });
            }
            catch (Exception e)
            {
                this.logger.LogWarn("Exception " + e + ", Status : Bad Request");
                return this.BadRequest(new { Status = false, Message = "Exception", Response = e });
            }
        }

        private int GetUserId()
        {
            var token = HttpContext.Request?.Headers["authorization"];
            string tok = token.ToString();
            string[] tokenArray = tok.Split(" ");
            var handler = new JwtSecurityTokenHandler();
            var jsonToken = handler.ReadToken(tokenArray[1]);
            var tokenS = jsonToken as JwtSecurityToken;
            int userId = int.Parse(tokenS.Claims.First(claim => claim.Type == "Id").Value);
            return userId;
        }

        /// <summary>
        /// user Id
        /// </summary>
        /// <returns>user Id</returns>
        private int TokenUserId()
        {
            return Convert.ToInt32(User.FindFirst("Id").Value);
        }
        
        /// <summary>
        /// returns User email
        /// </summary>
        /// <returns>return Email</returns>
        private string TokenEmail()
        {
            return User.FindFirst("Email").Value;
        }

    }
}
