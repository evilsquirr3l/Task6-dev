using Business.Models;

namespace Business.Validation
{
    public static class ReaderValidation
    {
        public static void CheckReader(ReaderModel readerModel)
        {
            if (string.IsNullOrEmpty(readerModel.Name))
            {
                throw new LibraryException("Name is empty.");
            }

            if (string.IsNullOrEmpty(readerModel.Email))
            {
                throw new LibraryException("Email is empty.");
            }

            if (string.IsNullOrEmpty(readerModel.Phone))
            {
                throw new LibraryException("Phone is empty.");
            }

            if (string.IsNullOrEmpty(readerModel.Address))
            {
                throw new LibraryException("Address is empty.");
            }
        }
    }
}
