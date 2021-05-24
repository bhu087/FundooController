/////------------------------------------------------------------------------
////<copyright file="INotesRepo.cs" company="BridgeLabz">
////author="Bhushan"
////</copyright>
////--------------------------------------------------------------------------

namespace FundooRepository.Repo.NotesRepository
{
    using System;
    using System.Collections.Generic;
    using System.Text;
    using System.Threading.Tasks;
    using CloudinaryDotNet.Actions;
    using FundooModel.Notes;

    /// <summary>
    /// Notes repository interface
    /// </summary>
    public interface INotesRepo
    {
        /// <summary>
        /// Add notes
        /// </summary>
        /// <param name="notes">parameter notes</param>
        /// <returns>return notes</returns>
        Task<Notes> AddNotes(Notes notes, string userEmail);

        /// <summary>
        /// Delete notes
        /// </summary>
        /// <param name="id">parameter ID</param>
        /// <returns>returns Notes</returns>
        Task<Notes> DeleteNotes(int notesId, int userId);

        /// <summary>
        /// Update notes
        /// </summary>
        /// <param name="notes">parameter notes</param>
        /// <returns>returns notes</returns>
        Task<Notes> UpdateNotes(Notes notes, int userId);

        /// <summary>
        /// Get all notes by email address
        /// </summary>
        /// <param name="email">parameter email</param>
        /// <returns>returns list of notes</returns>
        Task<List<Notes>> GetAllNotes(int userId);

        /// <summary>
        /// Delete from trash
        /// </summary>
        /// <param name="id">parameter ID</param>
        /// <returns>Returns Notes</returns>
        Task<Notes> DeleteFromTrash(int noteId, int userId);

        /// <summary>
        /// Add collaborater
        /// </summary>
        /// <param name="collaborater">parameter collaborater</param>
        /// <returns>return collaborater</returns>
        Task<Collaborater> AddCollaborater(Collaborater collaborater);

        /// <summary>
        /// Delete collaborater
        /// </summary>
        /// <param name="collaborater">parameter collaborater</param>
        /// <returns>return collaborater</returns>
        Task<Collaborater> DeleteCollaborater(Collaborater collaborater);

        /// <summary>
        /// Upload image
        /// </summary>
        /// <param name="noteId">parameter note ID</param>
        /// <param name="imagePath">parameter image path</param>
        /// <returns>returns image upload result</returns>
        Task<ImageUploadResult> UploadImage(int noteId, string imagePath, int userId);

        /// <summary>
        /// Reset the trash
        /// </summary>
        /// <param name="id">Note id</param>
        /// <returns>Returns boolean result</returns>
        Task<bool> ResetIsTrash(int noteId, int userId);

        /// <summary>
        /// Set the trash
        /// </summary>
        /// <param name="id">Note id</param>
        /// <returns>Returns boolean result</returns>
        Task<bool> SetIsTrash(int noteId, int userId);

        /// <summary>
        /// Reset the Archive
        /// </summary>
        /// <param name="id">note id</param>
        /// <returns>boolean result</returns>
        Task<bool> ResetArchive(int noteId, int userId);

        /// <summary>
        /// Set the Archive
        /// </summary>
        /// <param name="id">note id</param>
        /// <returns>boolean result</returns>
        Task<bool> SetArchive(int noteId, int userId);

        /// <summary>
        /// Reset the Pin
        /// </summary>
        /// <param name="id">note id</param>
        /// <returns>boolean result</returns>
        Task<bool> ResetPin(int noteId, int userId);

        /// <summary>
        /// Set the Pin
        /// </summary>
        /// <param name="id">note id</param>
        /// <returns>boolean result</returns>
        Task<bool> SetPin(int noteId, int userId);

        /// <summary>
        /// Add remainder
        /// </summary>
        /// <param name="id">note id</param>
        /// <param name="time">Given time</param>
        /// <returns>boolean result</returns>
        Task<bool> AddRemainder(int noteId, string time, int userId);

        /// <summary>
        /// Delete remainder
        /// </summary>
        /// <param name="id">note id</param>
        /// <returns>boolean result</returns>
        Task<bool> DeleteRemainder(int noteId, int userId);
        Task<bool> SetColor(int noteId,int userId, string color);
        Task<bool> DeleteColor(int noteId, int userId);
    }
}
