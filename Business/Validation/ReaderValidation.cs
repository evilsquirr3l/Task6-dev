using Business.Models;

namespace Business.Validation
{
    public static class ReaderValidation
    {
        public static void CheckReader(ReaderModel readerModel)
        {
            if (string.IsNullOrEmpty(readerModel.Name))
            {
                throw new ValidationException("Name is empty.");
            }

            if (string.IsNullOrEmpty(readerModel.Email))
            {
                throw new ValidationException("Email is empty.");
            }

            if (string.IsNullOrEmpty(readerModel.Phone))
            {
                throw new ValidationException("Phone is empty.");
            }

            if (string.IsNullOrEmpty(readerModel.Address))
            {
                throw new ValidationException("Address is empty.");
            }
        }
    }
}
