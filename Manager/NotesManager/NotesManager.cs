﻿/////------------------------------------------------------------------------
////<copyright file="NotesManager.cs" company="BridgeLabz">
////author="Bhushan"
////</copyright>
////-------------------------------------------------------------------------

namespace FundooManager.NotesManager
{
    using FundooRepository.Repo.NotesRepository;
    using System;
    using System.Collections.Generic;
    using System.Text;
    using System.Threading.Tasks;
    using CloudinaryDotNet.Actions;
    using FundooModel.Notes;

    /// <summary>
    /// Notes manager class
    /// </summary>
    public class NotesManager : INotesManager
    {
        /// <summary>
        /// Notes repository
        /// </summary>
        public readonly INotesRepo repository;

        /// <summary>
        /// Notes manager constructor
        /// </summary>
        /// <param name="notesRepo">parameter notes repository</param>
        public NotesManager(INotesRepo notesRepo)
        {
            repository = notesRepo;
        }

        /// <summary>
        /// Add notes
        /// </summary>
        /// <param name="notes">parameter notes</param>
        /// <returns>return notes</returns>
        public Task<Notes> AddNotes(Notes notes)
        {
            try
            {
                return this.repository.AddNotes(notes);
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
        public Task<Notes> DeleteNotes(int id)
        {
            try
            {
                return this.repository.DeleteNotes(id);
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
        public Task<Notes> UpdateNotes(Notes notes)
        {
            try
            {
                return this.repository.UpdateNotes(notes);
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
        public Task<IEnumerable<Notes>> GetAllNotes()
        {
            try
            {
                return this.repository.GetAllNotes();
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
        public Task<IEnumerable<Notes>> GetAllNotesByEmail(string email)
        {
            try
            {
                return this.repository.GetAllNotesByEmail(email);
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
        public Task<Notes> DeleteFromTrash(int id)
        {
            try
            {
                return this.repository.DeleteFromTrash(id);
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
        public Task<Collaborater> AddCollaborater(Collaborater collaborater)
        {
            try
            {
                return this.repository.AddCollaborater(collaborater);
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
        public Task<Collaborater> DeleteCollaborater(Collaborater collaborater)
        {
            try
            {
                return this.repository.DeleteCollaborater(collaborater);
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
        public Task<ImageUploadResult> UploadImage(int noteId, string imagePath)
        {
            try
            {
                return this.repository.UploadImage(noteId, imagePath);
            }
            catch
            {
                throw new Exception();
            }
        }

        public Task<bool> ResetIsTrash(int id)
        {
            try
            {
                return this.repository.ResetIsTrash(id);
            }
            catch
            {
                throw new Exception();
            }
        }

        public Task<bool> SetIsTrash(int id)
        {
            try
            {
                return this.repository.SetIsTrash(id);
            }
            catch
            {
                throw new Exception();
            }
        }

        public Task<bool> ResetArchive(int id)
        {
            try
            {
                return this.repository.ResetArchive(id);
            }
            catch
            {
                throw new Exception();
            }
        }

        public Task<bool> SetArchive(int id)
        {
            try
            {
                return this.repository.SetArchive(id);
            }
            catch
            {
                throw new Exception();
            }
        }
    }
}
