using System;
using Business.Models;

namespace Business.Validation
{
    public static class BookValidation
    {
        public static void CheckBook(BookModel bookModel)
        {
            if (string.IsNullOrEmpty(bookModel.Title))
            {
                throw new ValidationException("Title is empty.");
            }

            if (string.IsNullOrEmpty(bookModel.Author))
            {
                throw new ValidationException("Author is empty.");
            }

            if (bookModel.Year > DateTime.Now.Year)
            {
                throw new ValidationException("Year should be valid.");
            }
        }
    }
}