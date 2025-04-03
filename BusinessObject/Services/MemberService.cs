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
            try
            {
                return await _unitOfWork.Repository<Member>().GetAllAsync();
            }
            catch (Exception ex)
            {
                Console.WriteLine($"Error in GetAllMembersAsync: {ex.Message}");
                throw;
            }
        }

        public async Task<Member> GetMemberByIdAsync(int id)
        {
            try
            {
                return await _unitOfWork.Repository<Member>().GetByIdAsync(id);
            }
            catch (Exception ex)
            {
                Console.WriteLine($"Error in GetMemberByIdAsync: {ex.Message}");
                throw;
            }
        }

        public async Task<Member> GetMemberByEmailAsync(string email)
        {
            try
            {
                var members = await _unitOfWork.Repository<Member>().FindAsync(m => m.Email.Equals(email));
                return members.FirstOrDefault();
            }
            catch (Exception ex)
            {
                Console.WriteLine($"Error in GetMemberByEmailAsync: {ex.Message}");
                throw;
            }
        }

        public async Task<IEnumerable<Order>> GetMemberOrdersAsync(int memberId)
        {
            try
            {
                return await _unitOfWork.Repository<Order>().FindAsync(o => o.MemberId == memberId);
            }
            catch (Exception ex)
            {
                Console.WriteLine($"Error in GetMemberOrdersAsync: {ex.Message}");
                throw;
            }
        }

        //public async Task<Member> CreateMemberAsync(Member member)
        //{
        //    try
        //    {
        //        await _unitOfWork.Repository<Member>().AddAsync(member);
        //        await _unitOfWork.CommitAsync();
        //        return member;
        //    }
        //    catch (Exception ex)
        //    {
        //        Console.WriteLine($"Error in CreateMemberAsync: {ex.Message}");
        //        throw;
        //    }
        //}

        public async Task AddMemberAsync(Member member)
        {
            try
            {
                await _unitOfWork.Repository<Member>().AddAsync(member);
                await _unitOfWork.CommitAsync();
            }
            catch (Exception ex)
            {
                Console.WriteLine($"Error in AddMemberAsync: {ex.Message}");
                throw;
            }
        }

        //public async Task UpdateMemberAsync(int id, Member member)
        //{
        //    try
        //    {
        //        var existingMember = await _unitOfWork.Repository<Member>().GetByIdAsync(id);
        //        if (existingMember == null)
        //        {
        //            throw new Exception($"Member with ID {id} not found.");
        //        }

        //        // Update properties
        //        existingMember.Email = member.Email;
        //        existingMember.CompanyName = member.CompanyName;
        //        existingMember.City = member.City;
        //        existingMember.Country = member.Country;
        //        existingMember.Password = member.Password;

        //        _unitOfWork.Repository<Member>().Update(existingMember);
        //        await _unitOfWork.CommitAsync();
        //    }
        //    catch (Exception ex)
        //    {
        //        Console.WriteLine($"Error in UpdateMemberAsync: {ex.Message}");
        //        throw;
        //    }
        //}

        public async Task UpdateMemberAsync(Member member)
        {
            try
            {
                _unitOfWork.Repository<Member>().Update(member);
                await _unitOfWork.CommitAsync();
            }
            catch (Exception ex)
            {
                Console.WriteLine($"Error in UpdateMemberAsync(Member): {ex.Message}");
                throw;
            }
        }

        public async Task DeleteMemberAsync(int id)
        {
            try
            {
                var member = await _unitOfWork.Repository<Member>().GetByIdAsync(id);
                if (member == null)
                {
                    throw new Exception($"Member with ID {id} not found.");
                }

                _unitOfWork.Repository<Member>().Remove(member);
                await _unitOfWork.CommitAsync();
            }
            catch (Exception ex)
            {
                Console.WriteLine($"Error in DeleteMemberAsync: {ex.Message}");
                throw;
            }
        }

        public async Task<bool> EmailExistsAsync(string email, int? excludeMemberId = null)
        {
            try
            {
                var members = await _unitOfWork.Repository<Member>().FindAsync(m => m.Email.Equals(email));
                if (excludeMemberId.HasValue)
                {
                    return members.Any(m => m.MemberId != excludeMemberId.Value);
                }
                return members.Any();
            }
            catch (Exception ex)
            {
                Console.WriteLine($"Error in EmailExistsAsync: {ex.Message}");
                throw;
            }
        }

        public async Task<bool> MemberHasOrdersAsync(int memberId)
        {
            try
            {
                var orders = await _unitOfWork.Repository<Order>().FindAsync(o => o.MemberId == memberId);
                return orders.Any();
            }
            catch (Exception ex)
            {
                Console.WriteLine($"Error in MemberHasOrdersAsync: {ex.Message}");
                throw;
            }
        }

        /// <summary>
        /// Authenticates a member using email and password
        /// </summary>
        /// <param name="loginRequest">Login request model</param>
        /// <returns>Member information if authentication succeeds, null otherwise</returns>
        public async Task<Member> AuthenticateAsync(LoginRequestModel loginRequest)
        {
            if (loginRequest == null || string.IsNullOrEmpty(loginRequest.Email) || string.IsNullOrEmpty(loginRequest.Password))
            {
                return null;
            }

            return await AuthenticateAsync(loginRequest.Email, loginRequest.Password);
        }

        public async Task<Member> AuthenticateAsync(string email, string password)
        {
            try
            {
                if (string.IsNullOrEmpty(email) || string.IsNullOrEmpty(password))
                {
                    return null;
                }

                // Check if email exists
                var members = await _unitOfWork.Repository<Member>().FindAsync(m => m.Email.Equals(email));
                var member = members.FirstOrDefault();
                if (member == null)
                {
                    Console.WriteLine($"Authentication failed: Email {email} not found.");
                    return null;
                }

                // Check if password matches
                if (!member.Password.Equals(password))
                {
                    Console.WriteLine($"Authentication failed: Invalid password for email {email}.");
                    return null;
                }

                return member;
            }
            catch (Exception ex)
            {
                Console.WriteLine($"Error in AuthenticateAsync: {ex.Message}");
                return null;
            }
        }
    }
}