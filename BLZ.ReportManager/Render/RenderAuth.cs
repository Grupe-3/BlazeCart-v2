using BLZ.Client.Services;
using BLZ.ReportManager.Services;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace BLZ.ReportManager.Render
{
    internal class RenderAuth : IRender
    {
        private readonly InputService _input;
        private readonly AuthService _auth;
        public RenderAuth(InputService input, AuthService auth)
        {
            _input = input;
            _auth = auth;
        }

        public async Task Render()
        {
            while (!_auth.IsSignedIn())
            {
                var email = _input.GetStr("Enter email:");
                var password = _input.GetStr("Enter password:");
                await _auth.LoginAsync(email, password);
                if (!_auth.IsSignedIn())
                {
                    _input.Log("Failed to login, try again.");
                }
                else
                {
                    _input.Log("Authenticated!");
                }
            }
        }
    }
}
