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
    }
}
