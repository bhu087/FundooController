using CloudinaryDotNet;
using CloudinaryDotNet.Actions;
using FundooModel.Notes;
using FundooRepository.DbContexts;
using Microsoft.Extensions.Configuration;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace FundooRepository.Repo.NotesRepository
{
    public class NotesRepo : INotesRepo
    {
        public readonly UserDbContext context;
        public readonly IConfiguration config;
        public NotesRepo(UserDbContext userDbContext, IConfiguration configuration)
        {
            this.context = userDbContext;
            this.config = configuration;
        }

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
        public bool ResetIsTrash(int id)
        {
            try
            {
                var note = this.context.Notes.Where(notesId => notesId.NotesId == id).SingleOrDefault();
                if (note != null)
                {
                    note.IsTrash = false;
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
        public bool SetIsTrash(int id)
        {
            try
            {
                var note = this.context.Notes.Where(notesId => notesId.NotesId == id).SingleOrDefault();
                if (note != null)
                {
                    note.IsTrash = true;
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
        public bool ResetArchieve(int id)
        {
            try
            {
                var note = this.context.Notes.Where(notesId => notesId.NotesId == id).SingleOrDefault();
                if (note != null)
                {
                    note.IsArchive = false;
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
        public bool SetArchieve(int id)
        {
            try
            {
                var note = this.context.Notes.Where(notesId => notesId.NotesId == id).SingleOrDefault();
                if (note != null)
                {
                    note.IsArchive = true;
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
        public bool ResetPin(int id)
        {
            try
            {
                var note = this.context.Notes.Where(notesId => notesId.NotesId == id).SingleOrDefault();
                if (note != null)
                {
                    note.IsPin = false;
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
        public async Task<Collaborater> DeleteCollaborater(Collaborater collaborater)
        {
            try
            {
                var collabrate = this.context.Collaboraters.Where(notes => notes.NotesId == collaborater.NotesId).ToList();
                if (collabrate != null)
                {
                    foreach (Collaborater list in collabrate)
                    {
                        if ( list.ReceiverEmail == collaborater.ReceiverEmail)
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
        public async Task<ImageUploadResult> UploadImage(int noteId, string imagePath)
        {
            try
            {
                string cloudName = this.config["Cloudinary:CloudName"];
                string APIKey = this.config["Cloudinary:APIKey"];
                string APISecret = this.config["Cloudinary:APISecret"];
                var note = this.context.Notes.Where(noteAtId => noteAtId.NotesId == noteId).SingleOrDefault();
                if (note != null)
                {
                    Account account = new Account(cloudName, APIKey, APISecret);
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
