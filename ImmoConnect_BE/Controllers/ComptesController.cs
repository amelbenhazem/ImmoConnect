using ImmoConnect_BE.Services;
using ImmoConnect_BE.ViewModel;
using ImmoConnect_BE.ViewModels;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;

namespace ImmoConnect_BE.Controllers
{
    [ApiController]
    [Route("api/[controller]")]
    [Authorize]
    public class ComptesController : ControllerBase
    {
        private readonly ICompteService _compteService;

        public ComptesController(ICompteService compteService)
        {
            _compteService = compteService;
        }

        [AllowAnonymous]
        [HttpPost("register")]
        public async Task<IActionResult> Register([FromBody] UtilisateurDto utilisateurDto)
        {
            if (!ModelState.IsValid)
                return BadRequest(ModelState);

            try
            {
                var user = await _compteService.RegisterAsync(utilisateurDto);
                return Ok(user);
            }
            catch (Exception ex)
            {
                return BadRequest(new { message = ex.Message });
            }
        }
        [AllowAnonymous]
        [HttpPost("login")]
        public async Task<IActionResult> Login([FromBody] LoginDto loginDto)
        {
            if (!ModelState.IsValid)
                return BadRequest(ModelState);

            try
            {
                var user = await _compteService.LoginAsync(loginDto);
                return Ok(user);
            }
            catch (Exception ex)
            {
                return Unauthorized(new { message = ex.Message });
            }
        }
        [Authorize(Roles = "Admin")]
        [HttpPut("{id}")]
        public async Task<IActionResult> Update(int id, [FromBody] UtilisateurDto dto)
        {
            if (!ModelState.IsValid)
                return BadRequest(ModelState);

            try
            {
                var updatedUser = await _compteService.UpdateUtilisateurAsync(id, dto);
                return Ok(updatedUser);
            }
            catch (Exception ex)
            {
                return NotFound(new { message = ex.Message });
            }
        }
        [Authorize(Roles = "Admin")]
        [HttpGet]
        public async Task<IActionResult> GetAll()
        {
            return Ok(await _compteService.ObtenirTousAsync());
        }

        [Authorize(Roles = "Admin")]
        [HttpGet("{id}")]
        public async Task<IActionResult> GetById(int id)
        {
            var user = await _compteService.ObtenirParIdAsync(id);
            if (user == null) return NotFound();
            return Ok(user);
        }


    }
}
