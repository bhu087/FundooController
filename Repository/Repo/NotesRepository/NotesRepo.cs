/////------------------------------------------------------------------------
////<copyright file="NotesRepo.cs" company="BridgeLabz">
////author="Bhushan"
////</copyright>
////--------------------------------------------------------------------------

namespace FundooRepository.Repo.NotesRepository
{
    using System;
    using System.Collections.Generic;
    using System.Linq;
    using System.Text;
    using System.Threading.Tasks;
    using CloudinaryDotNet;
    using CloudinaryDotNet.Actions;
    using FundooModel.Notes;
    using FundooRepository.DbContexts;
    using Microsoft.Extensions.Configuration;
    using Newtonsoft.Json;
    using StackExchange.Redis;

    /// <summary>
    /// Notes repository class
    /// </summary>
    public class NotesRepo : INotesRepo
    {
        /// <summary>
        /// User DB context
        /// </summary>
        private readonly UserDbContext context;

        /// <summary>
        /// database for cache
        /// </summary>
        private IDatabase database;

        /// <summary>
        /// Connection multiplexer
        /// </summary>
        private ConnectionMultiplexer connectionMultiplexer = ConnectionMultiplexer.Connect("127.0.0.1:6379");

        /// <summary>
        /// Configuration interface
        /// </summary>
        private readonly IConfiguration config;

        /// <summary>
        /// Notes repository constructor
        /// </summary>
        /// <param name="userDbContext">parameter DB context</param>
        /// <param name="configuration">parameter Configuration</param>
        public NotesRepo(UserDbContext userDbContext, IConfiguration configuration)
        {
            this.context = userDbContext;
            this.config = configuration;
        }

        /// <summary>
        /// Add notes
        /// </summary>
        /// <param name="notes">parameter notes</param>
        /// /// <param name="userEmail">parameter user Email</param>
        /// <returns>return notes</returns>
        public async Task<Notes> AddNotes(Notes notes, string userEmail)
        {
            try
            {
                var val = this.context.Notes.Add(notes);
                var result = await this.context.SaveChangesAsync();
                Collaborator coll = new Collaborator
                {
                    NotesId = val.Entity.NotesId,
                    UserID = notes.UserID,
                    ReceiverEmail = userEmail,
                    SenderEmail = userEmail
                };
                var res = this.context.Collaborator.Add(coll);
                var results = this.context.SaveChangesAsync();
                return await Task.Run(() => notes);
            }
            catch
            {
                throw new Exception();
            }
        }

        /// <summary>
        /// Delete notes
        /// </summary>
        /// <param name="noteId">parameter note Id</param>
        /// /// <param name="userId">parameter User Id</param>
        /// <returns>returns Notes</returns>
        public async Task<Notes> DeleteNotes(int noteId, int userId)
        {
            try
            {
                var note = this.GetNoteById(noteId).Result;
                if (note != null && !note.IsTrash && note.UserID == userId)
                {
                    note.IsTrash = true;
                    var result = this.context.SaveChangesAsync();
                    return await Task.Run(() => note);
                }

                return null;
            }
            catch
            {
                throw new Exception();
            }
        }

        /// <summary>
        /// Update notes
        /// </summary>
        /// <param name="notes">parameter notes</param>
        /// <param name="userId">parameter User ID</param>
        /// <returns>returns notes</returns>
        public async Task<Notes> UpdateNotes(Notes notes, int userId)
        {
            try
            {
                var note = this.context.Notes.Where(notesId => notesId.NotesId == userId).SingleOrDefault();
                if (note != null && !note.IsTrash && !note.IsArchive && note.UserID == userId)
                {
                    if (notes.Title != null)
                    {
                        note.Title = notes.Title;
                    }

                    if (notes.Description != null)
                    {
                        note.Description = notes.Description;
                    }

                    if (notes.ModifiedTime != null)
                    {
                        note.ModifiedTime = notes.ModifiedTime;
                    }

                    var result = this.context.SaveChangesAsync();
                    return await Task.Run(() => notes);
                }

                return null;
            }
            catch
            {
                throw new Exception();
            }
        }

        /// <summary>
        /// Delete from trash
        /// </summary>
        /// <param name="noteId">parameter note id</param>
        /// <param name="userId">parameter user ID</param>
        /// <returns>Returns Notes</returns>
        public async Task<Notes> DeleteFromTrash(int noteId, int userId)
        {
            try
            {
                var note = this.GetNoteById(noteId).Result;
                if (note != null && note.IsTrash && note.UserID == userId)
                {
                    this.context.Notes.Remove(note);
                    var result = this.context.SaveChangesAsync();
                    return await Task.Run(() => note);
                }

                return null;
            }
            catch
            {
                throw new Exception();
            }
        }

        /// <summary>
        /// Get all notes by email address
        /// </summary>
        /// <param name="userId">parameter user Id</param>
        /// <returns>returns list of notes</returns>
        public async Task<List<Notes>> GetAllNotes(int userId)
        {
            try
            {
                this.database = this.connectionMultiplexer.GetDatabase();
                string key = userId + "GetAllNotes";
                var cacheList = this.database.StringGet(key);
                if (!cacheList.IsNull)
                {
                    var cacheNotes = JsonConvert.DeserializeObject<List<Notes>>(cacheList);
                    return cacheNotes;
                }
                List<Notes> list = new List<Notes>();
                var allNotes = this.context.Notes.Join(this.context.Collaborator, 
                    Notes => Notes.NotesId, 
                    Collaborater => Collaborater.NotesId,
                        (Notes, Collaborater) => new Notes
                        {
                            NotesId = Collaborater.NotesId,
                            UserID = Collaborater.UserID,
                            Title = Notes.Title,
                            Description = Notes.Description,
                            CreatedTime = Notes.CreatedTime,
                            ModifiedTime = Notes.ModifiedTime,
                            Remainder = Notes.Remainder,
                            Image = Notes.Image,
                            IsArchive = Notes.IsArchive,
                            IsPin = Notes.IsPin,
                            IsTrash = Notes.IsTrash
                        });
                foreach (var data in allNotes)
                {
                    if (data.UserID == userId)
                    {
                        list.Add(data);
                    }
                }
                this.SaveToRedisCache(key, list);
                return await Task.Run(() => list);
            }
            catch
            {
                throw new Exception();
            }
        }

        /// <summary>
        /// Get user by Email
        /// </summary>
        /// <param name="email"></param>
        /// <returns>return the user id</returns>
        public string GetUserIdByEmail(string email)
        {
            var account = this.context.Users.Where(accounts => accounts.Email == email).SingleOrDefault();
            if (account == null)
            {
                return null;
            }

            return account.UserID.ToString();
        }

        /// <summary>
        /// Set to zero in is Trash column
        /// </summary>
        /// <param name="noteId">Note ID</param>
        /// <param name="userId">User ID</param>
        /// <returns>returns status</returns>
        public async Task<bool> ResetIsTrash(int noteId, int userId)
        {
            try
            {
                var note = this.GetNoteById(noteId).Result;
                if (note != null && note.UserID == userId)
                {
                    note.IsTrash = false;
                    await Task.Run(() => this.context.SaveChangesAsync());
                    return true;
                }

                return false;
            }
            catch
            {
                throw new Exception();
            }
        }

        /// <summary>
        /// Set to one in is trash column
        /// </summary>
        /// <param name="noteId">Note ID</param>
        /// <param name="userId">User ID</param>
        /// <returns>returns status</returns>
        public async Task<bool> SetIsTrash(int noteId, int userId)
        {
            try
            {
                var note = this.GetNoteById(noteId).Result;
                if (note != null && note.UserID == userId)
                {
                    note.IsTrash = true;
                    await Task.Run(() => this.context.SaveChangesAsync());
                    return true;
                }

                return false;
            }
            catch
            {
                throw new Exception();
            }
        }

        /// <summary>
        /// Reset archive field
        /// </summary>
        /// <param name="noteId">Note ID</param>
        /// <param name="userId">User ID</param>
        /// <returns>returns status</returns>
        public async Task<bool> ResetArchive(int noteId, int userId)
        {
            try
            {
                var note = this.GetNoteById(noteId).Result;
                if (note != null && note.UserID == userId)
                {
                    note.IsArchive = false;
                    await Task.Run(() => this.context.SaveChangesAsync());
                    return true;
                }

                return false;
            }
            catch
            {
                throw new Exception();
            }
        }

        /// <summary>
        /// Set Archive
        /// </summary>
        /// <param name="noteId">Notes ID</param>
        /// <param name="userId">User ID</param>
        /// <returns>returns status</returns>
        public async Task<bool> SetArchive(int noteId, int userId)
        {
            try
            {
                var note = this.GetNoteById(noteId).Result;
                if (note != null && note.UserID == userId)
                {
                    note.IsArchive = true;
                    await Task.Run(() => this.context.SaveChangesAsync());
                    return true;
                }

                return false;
            }
            catch
            {
                throw new Exception();
            }
        }

        /// <summary>
        /// Reset Pin
        /// </summary>
        /// <param name="noteId">Notes Id</param>
        /// <param name="userId">User Id</param>
        /// <returns>returns status</returns>
        public async Task<bool> ResetPin(int noteId, int userId)
        {
            try
            {
                var note = this.GetNoteById(noteId).Result;
                if (note != null && note.UserID == userId)
                {
                    note.IsPin = false;
                    await Task.Run(() => this.context.SaveChangesAsync());
                    return true;
                }

                return false;
            }
            catch
            {
                throw new Exception();
            }
        }

        /// <summary>
        /// Set Pin
        /// </summary>
        /// <param name="noteId">Notes ID</param>
        /// <param name="userId">User ID</param>
        /// <returns>returns status</returns>
        public async Task<bool> SetPin(int noteId, int userId)
        {
            try
            {
                var note = this.GetNoteById(noteId).Result;
                if (note != null && note.UserID == userId)
                {
                    note.IsPin = true;
                    await Task.Run(() => this.context.SaveChangesAsync());
                    return true;
                }

                return false;
            }
            catch
            {
                throw new Exception();
            }
        }

        /// <summary>
        /// Add Remainder
        /// </summary>
        /// <param name="noteId">Notes ID</param>
        /// <param name="time">parameter time</param>
        /// <param name="userId">User Id</param>
        /// <returns>returns status</returns>
        public async Task<bool> AddRemainder(int noteId, string time, int userId)
        {
            try
            {
                var note = this.GetNoteById(noteId).Result;
                if (note != null && note.UserID == userId)
                {
                    note.Remainder = time;
                    await Task.Run(() => this.context.SaveChangesAsync());
                    return true;
                }

                return false;
            }
            catch
            {
                throw new Exception();
            }
        }

        /// <summary>
        /// Delete Remainder
        /// </summary>
        /// <param name="noteId">Notes ID</param>
        /// <param name="userId">User ID</param>
        /// <returns>returns status</returns>
        public async Task<bool> DeleteRemainder(int noteId, int userId)
        {
            try
            {
                var note = this.GetNoteById(noteId).Result;
                if (note != null && note.UserID == userId)
                {
                    note.Remainder = null;
                    await Task.Run(() => this.context.SaveChangesAsync());
                    return true;
                }

                return false;
            }
            catch
            {
                throw new Exception();
            }
        }

        /// <summary>
        /// Add collaborator
        /// </summary>
        /// <param name="collaborator">parameter collaborator</param>
        /// <returns>return collaborator</returns>
        public async Task<Collaborator> AddCollaborater(Collaborator collaborator)
        {
            try
            {
                string collaboratorUserId = this.GetUserIdByEmail(collaborator.ReceiverEmail);
                int collUserId = int.Parse(collaboratorUserId);
                bool duplicateStatus = this.CollaboratorDuplicatesCheck(collUserId, collaborator.NotesId, collaborator.ReceiverEmail);
                if (collaboratorUserId != null && !duplicateStatus)
                {
                    var note = this.GetNoteById(collaborator.NotesId).Result;

                    if (note != null && note.UserID == collaborator.UserID)
                    {
                        Collaborator newCollaborater = new Collaborator
                        {
                            SenderEmail = collaborator.SenderEmail,
                            ReceiverEmail = collaborator.ReceiverEmail,
                            UserID = collUserId,
                            NotesId = collaborator.NotesId
                        };
                        this.context.Collaborator.Add(newCollaborater);
                        var result = this.context.SaveChangesAsync();
                        if (result != null)
                        {
                            return await Task.Run(() => collaborator);
                        }
                    }
                }

                return null;
            }
            catch
            {
                throw new Exception();
            }
        }

        /// <summary>
        /// Delete collaborator
        /// </summary>
        /// <param name="collaborator">parameter collaborator</param>
        /// <returns>return collaborator</returns>
        public async Task<Collaborator> DeleteCollaborater(Collaborator collaborator)
        {
            try
            {
                var collabrate = this.context.Collaborator.Where(notes => notes.NotesId == collaborator.NotesId).ToList();
                if (collabrate != null)
                {
                    foreach (Collaborator list in collabrate)
                    {
                        if (list.UserID == collaborator.UserID)
                        {
                            this.context.Collaborator.Remove(list);
                            await Task.Run(() => this.context.SaveChangesAsync());
                            return collaborator;
                        }
                    }
                }

                return null;
            }
            catch
            {
                throw new Exception();
            }
        }

        /// <summary>
        /// Set Color
        /// </summary>
        /// <param name="noteId">Note ID</param>
        /// <param name="userId">User ID</param>
        /// <param name="color">Color Name</param>
        /// <returns>return boolean value</returns>
        public async Task<bool> SetColor(int noteId, int userId, string color)
        {
            var note = this.GetNoteById(noteId).Result;
            if (note != null && note.UserID == userId)
            {
                note.Color = color;
                await Task.Run(() => this.context.SaveChangesAsync());
                return true;
            }

            return false;
        }

        /// <summary>
        /// Delete Color
        /// </summary>
        /// <param name="noteId">Note ID</param>
        /// <param name="userId">User ID</param>
        /// <returns>return boolean</returns>
        public async Task<bool> DeleteColor(int noteId, int userId)
        {
            var note = this.GetNoteById(noteId).Result;
            if (note != null && note.UserID == userId)
            {
                note.Color = string.Empty;
                await Task.Run(() => this.context.SaveChangesAsync());
                return true;
            }

            return false;
        }

        /// <summary>
        /// Upload image
        /// </summary>
        /// <param name="noteId">parameter note ID</param>
        /// <param name="imagePath">parameter image path</param>
        /// <param name="userId">parameter user ID</param>
        /// <returns>returns image upload result</returns>
        public async Task<ImageUploadResult> UploadImage(int noteId, string imagePath, int userId)
        {
            try
            {
                string cloudName = this.config["Cloudinary:CloudName"];
                string apiKey = this.config["Cloudinary:APIKey"];
                string apiSecret = this.config["Cloudinary:APISecret"];
                var note = this.GetNoteById(noteId).Result;
                if (note != null && note.UserID == userId)
                {
                    Account account = new Account(cloudName, apiKey, apiSecret);
                    Cloudinary cloudinary = new Cloudinary(account);
                    var uploadFile = new ImageUploadParams
                    {
                        File = new FileDescription(imagePath)
                    };
                    var uploadResult = cloudinary.Upload(uploadFile);
                    string imageString = uploadResult.Url.ToString();
                    note.Image = imageString;
                    note.ModifiedTime = DateTime.Now;
                    var result = this.context.SaveChangesAsync();
                    return await Task.Run(() => uploadResult);
                }

                return null;
            }
            catch
            {
                throw new Exception();
            }
        }
        
        /// <summary>
        /// Get note by Id
        /// </summary>
        /// <param name="noteId">Note ID</param>
        /// <returns>Return Note by note ID</returns>
        public async Task<Notes> GetNoteById(int noteId)
        {
            try
            {
                var resultNote = this.context.Notes.Where(note => note.NotesId == noteId).SingleOrDefault();
                return await Task.Run(() => resultNote);
            }
            catch
            {
                throw new Exception();
            }
        }

        /// <summary>
        /// Collaborator Duplicates Check
        /// </summary>
        /// <param name="userId">User ID</param>
        /// <param name="noteId">Note ID</param>
        /// <param name="receiverEmail">Receiver Email</param>
        /// <returns>return status</returns>
        public bool CollaboratorDuplicatesCheck(int userId, int noteId, string receiverEmail)
        {
            var collaborate = this.context.Collaborator.Where(user => user.UserID == userId).ToList();
            foreach (var collUsers in collaborate)
            {
                if (collUsers.UserID == userId && collUsers.NotesId == noteId && collUsers.ReceiverEmail == receiverEmail)
                {
                    return true;
                }
            }

            return false;
        }

        /// <summary>
        /// Store to cache
        /// </summary>
        /// <param name="key">Key value</param>
        /// <param name="jwtToken">receiving JWT token to store cache</param>
        public void SaveToRedisCache(string key, List<Notes> notes)
        {
            this.database = this.connectionMultiplexer.GetDatabase();
            database.StringSet(key, JsonConvert.SerializeObject(notes));
        }
    }
}
