using BusinessObject.Services.Instance;
using DataAccess.Models;
using DataAccess.Repositories;
using Microsoft.EntityFrameworkCore;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using BusinessObject.RequestModel;
using Microsoft.Extensions.Logging;

namespace BusinessObject.Services
{
    public class MemberService : IMemberService
    {
        private readonly IUnitOfWork _unitOfWork;
        private readonly ILogger<MemberService> _logger;

        public MemberService(IUnitOfWork unitOfWork, ILogger<MemberService> logger)
        {
            _unitOfWork = unitOfWork;
            _logger = logger;
        }

        public async Task<IEnumerable<Member>> GetAllMembersAsync()
        {
            return await _unitOfWork.Members.GetAllAsync();
        }

        public async Task<Member> GetMemberByIdAsync(int id)
        {
            return await _unitOfWork.Members.GetByIdAsync(id);
        }

        public async Task<Member> GetMemberByEmailAsync(string email)
        {
            var members = await _unitOfWork.Members.FindAsync(m => m.Email.Equals(email));
            return members.FirstOrDefault();
        }

        public async Task<IEnumerable<Order>> GetMemberOrdersAsync(int memberId)
        {
            // Check if member exists
            var member = await _unitOfWork.Members.GetByIdAsync(memberId);
            if (member == null)
            {
                throw new KeyNotFoundException($"Member with ID {memberId} not found");
            }

            return await _unitOfWork.Orders.FindAsync(o => o.MemberId == memberId);
        }

        public async Task<Member> CreateMemberAsync(Member member)
        {
            if (member == null)
            {
                throw new ArgumentNullException(nameof(member));
            }

            // Check if email already exists
            if (await EmailExistsAsync(member.Email))
            {
                throw new InvalidOperationException($"Member with email {member.Email} already exists");
            }

            await _unitOfWork.Members.AddAsync(member);
            await _unitOfWork.CompleteAsync();

            return member;
        }

        public async Task UpdateMemberAsync(int id, Member member)
        {
            if (member == null)
            {
                throw new ArgumentNullException(nameof(member));
            }

            if (id != member.MemberId)
            {
                throw new ArgumentException("Member ID mismatch");
            }

            // Check if member exists
            var existingMember = await _unitOfWork.Members.GetByIdAsync(id);
            if (existingMember == null)
            {
                throw new KeyNotFoundException($"Member with ID {id} not found");
            }

            // Check if email is being changed and already taken by another member
            if (!string.IsNullOrEmpty(member.Email) && 
                !member.Email.Equals(existingMember.Email) && 
                await EmailExistsAsync(member.Email, id))
            {
                throw new InvalidOperationException($"Email {member.Email} is already in use by another member");
            }

            // Only update non-null properties
            if (!string.IsNullOrEmpty(member.Email))
            {
                existingMember.Email = member.Email;
            }

            if (!string.IsNullOrEmpty(member.CompanyName))
            {
                existingMember.CompanyName = member.CompanyName;
            }

            if (!string.IsNullOrEmpty(member.City))
            {
                existingMember.City = member.City;
            }

            if (!string.IsNullOrEmpty(member.Country))
            {
                existingMember.Country = member.Country;
            }

            if (!string.IsNullOrEmpty(member.Password))
            {
                existingMember.Password = member.Password;
            }

            // Update the entity using the updated existingMember
            _unitOfWork.Members.Update(existingMember);
            await _unitOfWork.CompleteAsync();
        }

        public async Task DeleteMemberAsync(int id)
        {
            var member = await _unitOfWork.Members.GetByIdAsync(id);
            if (member == null)
            {
                throw new KeyNotFoundException($"Member with ID {id} not found");
            }

            // Check if member has any orders
            if (await MemberHasOrdersAsync(id))
            {
                throw new InvalidOperationException("Cannot delete member because they have associated orders");
            }

            _unitOfWork.Members.Remove(member);
            await _unitOfWork.CompleteAsync();
        }

        public async Task<bool> EmailExistsAsync(string email, int? excludeMemberId = null)
        {
            var query = await _unitOfWork.Members.FindAsync(m => m.Email.Equals(email));

            if (excludeMemberId.HasValue)
            {
                // If we're checking for an existing member (during update), exclude that member from the check
                return query.Any(m => m.MemberId != excludeMemberId.Value);
            }

            return query.Any();
        }

        public async Task<bool> MemberHasOrdersAsync(int memberId)
        {
            var orders = await _unitOfWork.Orders.FindAsync(o => o.MemberId == memberId);
            return orders.Any();
        }

        /// <summary>
        /// Authenticates a member using email and password
        /// </summary>
        /// <param name="loginRequest">Login request model</param>
        /// <returns>Member information if authentication succeeds, null otherwise</returns>
        public async Task<Member?> AuthenticateAsync(LoginRequestModel loginRequest)
        {
            try
            {
                if (loginRequest == null || string.IsNullOrWhiteSpace(loginRequest.Email) || string.IsNullOrWhiteSpace(loginRequest.Password))
                {
                    _logger.LogWarning("Login attempt with null or empty credentials");
                    return null;
                }

                var email = loginRequest.Email.Trim();
                var password = loginRequest.Password;

                // Kiểm tra email tồn tại
                var members = await _unitOfWork.Members.FindAsync(m => m.Email == email);
                var member = members.FirstOrDefault();
                
                if (member == null)
                {
                    _logger.LogWarning("Login attempt with non-existent email: {Email}", email);
                    return null;
                }

                // Kiểm tra mật khẩu
                if (member.Password != password)
                {
                    _logger.LogWarning("Login attempt with incorrect password for email: {Email}", email);
                    return null;
                }

                _logger.LogInformation("Successful login for email: {Email}", email);
                return member;
            }
            catch (Exception ex)
            {
                _logger.LogError(ex, "Error during authentication for email: {Email}", loginRequest?.Email);
                return null;
            }
        }
    }
}