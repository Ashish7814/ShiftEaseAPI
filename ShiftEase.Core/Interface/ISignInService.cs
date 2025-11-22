using ShiftEase.Shared.DTOs;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace ShiftEase.Core.Interface
{
    public interface ISignInService
    {
        Task<LoginResponseDto?> LoginAsync(LoginModel model);
    }
}
