using RestfulFirebase.Auth;

namespace BLZ.Client.Services
{
    public class AuthService
    {
        private readonly AuthApp _auth;
        public AuthService(AuthApp auth) => _auth = auth;

        public string UserId => _auth.Session!.LocalId!;

        public string GetToken()
            => _auth.Session!.FirebaseToken;

        public bool IsSignedIn() => _auth.IsAuthenticated;

        public async Task LoginAsync(string email, string password)
            => await _auth.SignInWithEmailAndPassword(email, password);
    }
}
