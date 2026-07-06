using Microsoft.AspNetCore.Identity;

namespace IdentityEmailApp.Validator
{
    public class CustomErrorValidator :IdentityErrorDescriber
    {
        public override IdentityError PasswordTooShort(int length)
        {
            return new IdentityError()
            {
                Code = "PasswordTooShort",
                Description = $"Şifreniz en az {length} karakter içermelidir!"
            };
        }

        public override IdentityError PasswordRequiresLower()
        {
            return new IdentityError()
            {
                Code = "PasswordRequiresLower",
                Description = $"Şifreniz en az 1 tane büyük harf içermelidir!"
            };
        }

        public override IdentityError PasswordRequiresDigit()
        {
            return new IdentityError()
            {
                Code = "PasswordRequiresDigit",
                Description = $"Şifreniz en az 1 tane rakam içermelidir!"
            };
        }
        public override IdentityError PasswordRequiresNonAlphanumeric()
        {
            return new IdentityError()
            {
                Code = "PasswordRequiresNonAlphanumeric",
                Description = $"Şifreniz en az 1 tane sembol içermelidir!"
            };
        }
        public override IdentityError DuplicateUserName(string userName)
        {
            return new IdentityError()
            {
                Code = "DuplicateUserName",
                Description = $"{userName} adlı kullancı adı zaten bulunmaktadır,lütfen farklı bir kullanıcı adıyla deneyin!"
            };
        }

        public override IdentityError DuplicateEmail(string email)
        {
            return new IdentityError()
            {
                Code = "DuplicateUserName",
                Description = $"{email} adlı email adresi zaten bulunmaktadır,lütfen farklı bir email adresiyle deneyin!"
            };
        }
        
    }
}
