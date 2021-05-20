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
        /// <returns>return notes</returns>
        public async Task<Notes> AddNotes(Notes notes)
        {
            try
            {
                this.context.Notes.Add(notes);
                var result = this.context.SaveChangesAsync();
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
        /// <param name="id">parameter ID</param>
        /// <returns>returns Notes</returns>
        public async Task<Notes> DeleteNotes(int id)
        {
            try
            {
                var note = this.context.Notes.Where(notesId => notesId.NotesId == id).SingleOrDefault();
                if (note != null && !note.IsTrash)
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
        /// <returns>returns notes</returns>
        public async Task<Notes> UpdateNotes(Notes notes)
        {
            try
            {
                var note = this.context.Notes.Where(notesId => notesId.NotesId == notes.NotesId).SingleOrDefault();
                if (note != null && !note.IsTrash && !note.IsArchive)
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
        /// <param name="id">parameter ID</param>
        /// <returns>Returns Notes</returns>
        public async Task<Notes> DeleteFromTrash(int id)
        {
            try
            {
                var note = this.context.Notes.Where(notesId => notesId.NotesId == id).SingleOrDefault();
                if (note != null && note.IsTrash)
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
        /// Get all notes
        /// </summary>
        /// <returns>returns all notes</returns>
        public async Task<IEnumerable<Notes>> GetAllNotes()
        {
            try
            {
                var allNotes = this.context.Notes.ToList();
                if (allNotes != null)
                {
                    return await Task.Run(() => allNotes);
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
        /// <param name="email">parameter email</param>
        /// <returns>returns list of notes</returns>
        public async Task<IEnumerable<Notes>> GetAllNotesByEmail(string email)
        {
            try
            {
                var allNotes = this.context.Notes.ToList();
                List<Notes> list = new List<Notes>();
                if (allNotes != null)
                {
                    foreach (Notes notes in allNotes)
                    {
                        if (notes.Email.Equals(email))
                        {
                            list.Add(notes);
                        }
                    }

                    return await Task.Run(() => list);
                }

                return null;
            }
            catch
            {
                throw new Exception();
            }
        }

        /// <summary>
        /// Set to zero in is Trash column
        /// </summary>
        /// <param name="id">Note ID</param>
        /// <returns>returns status</returns>
        public async Task<bool> ResetIsTrash(int id)
        {
            try
            {
                var note = this.context.Notes.Where(notesId => notesId.NotesId == id).SingleOrDefault();
                if (note != null)
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
        /// <param name="id">Note ID</param>
        /// <returns>returns status</returns>
        public async Task<bool> SetIsTrash(int id)
        {
            try
            {
                var note = this.context.Notes.Where(notesId => notesId.NotesId == id).SingleOrDefault();
                if (note != null)
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
        /// <param name="id">Note ID</param>
        /// <returns>returns status</returns>
        public async Task<bool> ResetArchive(int id)
        {
            try
            {
                var note = this.context.Notes.Where(notesId => notesId.NotesId == id).SingleOrDefault();
                if (note != null)
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
        /// <param name="id">Notes ID</param>
        /// <returns>returns status</returns>
        public async Task<bool> SetArchive(int id)
        {
            try
            {
                var note = this.context.Notes.Where(notesId => notesId.NotesId == id).SingleOrDefault();
                if (note != null)
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
        /// <param name="id">Notes Id</param>
        /// <returns>returns status</returns>
        public async Task<bool> ResetPin(int id)
        {
            try
            {
                var note = this.context.Notes.Where(notesId => notesId.NotesId == id).SingleOrDefault();
                if (note != null)
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
        /// <param name="id">Notes ID</param>
        /// <returns>returns status</returns>
        public bool SetPin(int id)
        {
            try
            {
                var note = this.context.Notes.Where(notesId => notesId.NotesId == id).SingleOrDefault();
                if (note != null)
                {
                    note.IsPin = true;
                    this.context.SaveChangesAsync();
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
        /// <param name="id">Notes ID</param>
        /// <param name="time">parameter time</param>
        /// <returns>returns status</returns>
        public bool AddRemainder(int id, string time)
        {
            try
            {
                var note = this.context.Notes.Where(notesId => notesId.NotesId == id).SingleOrDefault();
                if (note != null)
                {
                    note.Remainder = time;
                    this.context.SaveChangesAsync();
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
        /// <param name="id">Notes ID</param>
        /// <returns>returns status</returns>
        public bool DeleteRemainder(int id)
        {
            try
            {
                var note = this.context.Notes.Where(notesId => notesId.NotesId == id).SingleOrDefault();
                if (note != null)
                {
                    note.Remainder = null;
                    this.context.SaveChangesAsync();
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
        /// Add collaborater
        /// </summary>
        /// <param name="collaborater">parameter collaborater</param>
        /// <returns>return collaborater</returns>
        public async Task<Collaborater> AddCollaborater(Collaborater collaborater)
        {
            try
            {
                var note = this.context.Notes.Where(noteId => noteId.NotesId == collaborater.NotesId).SingleOrDefault();
                if (note != null)
                {
                    Collaborater newCollaborater = new Collaborater
                    {
                        NotesId = collaborater.NotesId,
                        SenderEmail = collaborater.SenderEmail,
                        ReceiverEmail = collaborater.ReceiverEmail
                    };
                    this.context.Collaboraters.Add(newCollaborater);
                    var result = this.context.SaveChangesAsync();
                    if (result != null)
                    {
                        return await Task.Run(() => collaborater);
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
        /// Delete collaborater
        /// </summary>
        /// <param name="collaborater">parameter collaborater</param>
        /// <returns>return collaborater</returns>
        public async Task<Collaborater> DeleteCollaborater(Collaborater collaborater)
        {
            try
            {
                var collabrate = this.context.Collaboraters.Where(notes => notes.NotesId == collaborater.NotesId).ToList();
                if (collabrate != null)
                {
                    foreach (Collaborater list in collabrate)
                    {
                        if (list.ReceiverEmail == collaborater.ReceiverEmail)
                        {
                            this.context.Collaboraters.Remove(list);
                            await Task.Run(() => this.context.SaveChangesAsync());
                            return collaborater;
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
        /// Upload image
        /// </summary>
        /// <param name="noteId">parameter note ID</param>
        /// <param name="imagePath">parameter image path</param>
        /// <returns>returns image upload result</returns>
        public async Task<ImageUploadResult> UploadImage(int noteId, string imagePath)
        {
            try
            {
                string cloudName = this.config["Cloudinary:CloudName"];
                string apiKey = this.config["Cloudinary:APIKey"];
                string apiSecret = this.config["Cloudinary:APISecret"];
                var note = this.context.Notes.Where(noteAtId => noteAtId.NotesId == noteId).SingleOrDefault();
                if (note != null)
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
    }
}
