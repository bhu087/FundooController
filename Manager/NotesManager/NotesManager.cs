/////------------------------------------------------------------------------
////<copyright file="NotesManager.cs" company="BridgeLabz">
////author="Bhushan"
////</copyright>
////-------------------------------------------------------------------------

namespace FundooManager.NotesManager
{
    using System;
    using System.Collections.Generic;
    using System.Text;
    using System.Threading.Tasks;
    using CloudinaryDotNet.Actions;
    using FundooModel.Notes;
    using FundooRepository.Repo.NotesRepository;

    /// <summary>
    /// Notes manager class
    /// </summary>
    public class NotesManager : INotesManager
    {
        /// <summary>
        /// Notes repository
        /// </summary>
        private readonly INotesRepo repository;

        /// <summary>
        /// Notes manager constructor
        /// </summary>
        /// <param name="notesRepo">parameter notes repository</param>
        public NotesManager(INotesRepo notesRepo)
        {
            this.repository = notesRepo;
        }

        /// <summary>
        /// Add notes
        /// </summary>
        /// <param name="notes">parameter notes</param>
        /// <returns>return notes</returns>
        public Task<Notes> AddNotes(Notes notes, string email)
        {
            try
            {
                return this.repository.AddNotes(notes, email);
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
        public Task<Notes> DeleteNotes(int notesId, int userId)
        {
            try
            {
                return this.repository.DeleteNotes(notesId, userId);
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
        public Task<Notes> UpdateNotes(Notes notes, int userId)
        {
            try
            {
                return this.repository.UpdateNotes(notes, userId);
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
        public Task<List<Notes>> GetAllNotes(int userId)
        {
            try
            {
                return this.repository.GetAllNotes(userId);
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
        public Task<Notes> DeleteFromTrash(int noteId, int userId)
        {
            try
            {
                return this.repository.DeleteFromTrash(noteId, userId);
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
        public Task<ImageUploadResult> UploadImage(int noteId, string imagePath, int userId)
        {
            try
            {
                return this.repository.UploadImage(noteId, imagePath, userId);
            }
            catch
            {
                throw new Exception();
            }
        }

        /// <summary>
        /// Reset the trash
        /// </summary>
        /// <param name="id">Note id</param>
        /// <returns>Returns boolean result</returns>
        public Task<bool> ResetIsTrash(int noteId, int userId)
        {
            try
            {
                return this.repository.ResetIsTrash(noteId, userId);
            }
            catch
            {
                throw new Exception();
            }
        }

        /// <summary>
        /// Set the trash
        /// </summary>
        /// <param name="id">Note id</param>
        /// <returns>Returns boolean result</returns>
        public Task<bool> SetIsTrash(int noteId, int userId)
        {
            try
            {
                return this.repository.SetIsTrash(noteId, userId);
            }
            catch
            {
                throw new Exception();
            }
        }

        /// <summary>
        /// Reset the Archive
        /// </summary>
        /// <param name="id">note id</param>
        /// <returns>boolean result</returns>
        public Task<bool> ResetArchive(int noteId, int userId)
        {
            try
            {
                return this.repository.ResetArchive(noteId, userId);
            }
            catch
            {
                throw new Exception();
            }
        }

        /// <summary>
        /// Set the Archive
        /// </summary>
        /// <param name="id">note id</param>
        /// <returns>boolean result</returns>
        public Task<bool> SetArchive(int noteId, int userId)
        {
            try
            {
                return this.repository.SetArchive(noteId, userId);
            }
            catch
            {
                throw new Exception();
            }
        }

        /// <summary>
        /// Reset the Pin
        /// </summary>
        /// <param name="id">note id</param>
        /// <returns>boolean result</returns>
        public Task<bool> ResetPin(int noteId, int userId)
        {
            try
            {
                return this.repository.ResetPin(noteId, userId);
            }
            catch
            {
                throw new Exception();
            }
        }

        /// <summary>
        /// Set the Pin
        /// </summary>
        /// <param name="id">note id</param>
        /// <returns>boolean result</returns>
        public Task<bool> SetPin(int noteId, int userId)
        {
            try
            {
                return this.repository.SetPin(noteId, userId);
            }
            catch
            {
                throw new Exception();
            }
        }

        /// <summary>
        /// Add remainder
        /// </summary>
        /// <param name="id">note id</param>
        /// <param name="time">Given Time</param>
        /// <returns>boolean result</returns>
        public Task<bool> AddRemainder(int noteId, string time, int userId)
        {
            try
            {
                return this.repository.AddRemainder(noteId, time, userId);
            }
            catch
            {
                throw new Exception();
            }
        }

        /// <summary>
        /// Delete remainder
        /// </summary>
        /// <param name="id">note id</param>
        /// <returns>boolean result</returns>
        public Task<bool> DeleteRemainder(int noteId, int userId)
        {
            try
            {
                return this.repository.DeleteRemainder(noteId, userId);
            }
            catch
            {
                throw new Exception();
            }
        }

        public Task<bool> SetColor(int noteId, int userId, string color)
        {
            try
            {
                return this.repository.SetColor(noteId, userId, color);
            }
            catch
            {
                throw new Exception();
            }
        }

        public Task<bool> DeleteColor(int noteId, int userId)
        {
            try
            {
                return this.repository.DeleteColor(noteId, userId);
            }
            catch
            {
                throw new Exception();
            }
        }
    }
}
