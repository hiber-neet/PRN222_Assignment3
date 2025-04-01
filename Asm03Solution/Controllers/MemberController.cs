using BusinessObject.RequestModel;
using BusinessObject.Services.Instance;
using DataAccess.Models;
using Microsoft.AspNetCore.Mvc;
using System;
using System.Collections.Generic;
using System.Threading.Tasks;

namespace Asm03Solution.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class MemberController : ControllerBase
    {
        private readonly IMemberService _memberService;

        public MemberController(IMemberService memberService)
        {
            _memberService = memberService;
        }

        // GET: api/Member
        [HttpGet]
        public async Task<ActionResult<IEnumerable<Member>>> GetMembers()
        {
            try
            {
                var members = await _memberService.GetAllMembersAsync();
                return Ok(members);
            }
            catch (Exception ex)
            {
                return StatusCode(500, $"Internal server error: {ex.Message}");
            }
        }

        // GET: api/Member/5
        [HttpGet("{id}")]
        public async Task<ActionResult<Member>> GetMember(int id)
        {
            try
            {
                var member = await _memberService.GetMemberByIdAsync(id);

                if (member == null)
                {
                    return NotFound($"Member with ID {id} not found");
                }

                return Ok(member);
            }
            catch (Exception ex)
            {
                return StatusCode(500, $"Internal server error: {ex.Message}");
            }
        }

        // GET: api/Member/5/Orders
        [HttpGet("{id}/Orders")]
        public async Task<ActionResult<IEnumerable<Order>>> GetMemberOrders(int id)
        {
            try
            {
                var orders = await _memberService.GetMemberOrdersAsync(id);
                return Ok(orders);
            }
            catch (KeyNotFoundException ex)
            {
                return NotFound(ex.Message);
            }
            catch (Exception ex)
            {
                return StatusCode(500, $"Internal server error: {ex.Message}");
            }
        }

        // GET: api/Member/email/{email}
        [HttpGet("email/{email}")]
        public async Task<ActionResult<Member>> GetMemberByEmail(string email)
        {
            try
            {
                var member = await _memberService.GetMemberByEmailAsync(email);

                if (member == null)
                {
                    return NotFound($"Member with email {email} not found");
                }

                return Ok(member);
            }
            catch (Exception ex)
            {
                return StatusCode(500, $"Internal server error: {ex.Message}");
            }
        }

        // POST: api/Member
        [HttpPost]
        public async Task<ActionResult<Member>> PostMember(Member member)
        {
            try
            {
                if (!ModelState.IsValid)
                {
                    return BadRequest(ModelState);
                }

                var createdMember = await _memberService.CreateMemberAsync(member);
                return CreatedAtAction(nameof(GetMember), new { id = createdMember.MemberId }, createdMember);
            }
            catch (InvalidOperationException ex)
            {
                return BadRequest(ex.Message);
            }
            catch (Exception ex)
            {
                return StatusCode(500, $"Internal server error: {ex.Message}");
            }
        }

        // PUT: api/Member/5
        [HttpPut("{id}")]
        public async Task<IActionResult> PutMember(int id, Member member)
        {
            try
            {
                if (!ModelState.IsValid)
                {
                    return BadRequest(ModelState);
                }

                await _memberService.UpdateMemberAsync(id, member);
                return NoContent();
            }
            catch (ArgumentException ex)
            {
                return BadRequest(ex.Message);
            }
            catch (KeyNotFoundException ex)
            {
                return NotFound(ex.Message);
            }
            catch (InvalidOperationException ex)
            {
                return BadRequest(ex.Message);
            }
            catch (Exception ex)
            {
                return StatusCode(500, $"Internal server error: {ex.Message}");
            }
        }

        // DELETE: api/Member/5
        [HttpDelete("{id}")]
        public async Task<IActionResult> DeleteMember(int id)
        {
            try
            {
                await _memberService.DeleteMemberAsync(id);
                return NoContent();
            }
            catch (KeyNotFoundException ex)
            {
                return NotFound(ex.Message);
            }
            catch (InvalidOperationException ex)
            {
                return BadRequest(ex.Message);
            }
            catch (Exception ex)
            {
                return StatusCode(500, $"Internal server error: {ex.Message}");
            }
        }

        // POST: api/Member/login
        [HttpPost("login")]
        public async Task<ActionResult<Member>> Login([FromBody] LoginRequestModel loginRequest)
        {
            try
            {
                if (!ModelState.IsValid)
                {
                    return BadRequest(ModelState);
                }

                var member = await _memberService.AuthenticateAsync(loginRequest.Email, loginRequest.Password);
                
                if (member == null)
                {
                    return Unauthorized("Invalid email or password");
                }

                return Ok(member);
            }
            catch (ArgumentException ex)
            {
                return BadRequest(ex.Message);
            }
            catch (Exception ex)
            {
                return StatusCode(500, $"Internal server error: {ex.Message}");
            }
        }
    }
} 