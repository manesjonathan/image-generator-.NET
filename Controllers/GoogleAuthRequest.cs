using System.ComponentModel.DataAnnotations;

namespace ImageGeneratorApi.Controllers
{
    public class GoogleAuthRequest
    {
        public GoogleAuthRequest(UserProperties user, string[] scopes, string idToken)
        {
            User = user;
            Scopes = scopes;
            IdToken = idToken;
        }

        [Required] public UserProperties User { get; set; }

        public string[] Scopes { get; set; }

        [Required] public string IdToken { get; set; }
    }

    public class UserProperties
    {
        public UserProperties(string id, string name, string email, string photo, string familyName,
            string givenName)
        {
            Id = id;
            Name = name;
            Email = email;
            Photo = photo;
            FamilyName = familyName;
            GivenName = givenName;
        }

        public string Id { get; set; }

        public string Name { get; set; }

        public string Email { get; set; }

        public string Photo { get; set; }

        public string FamilyName { get; set; }

        public string GivenName { get; set; }
    }
}