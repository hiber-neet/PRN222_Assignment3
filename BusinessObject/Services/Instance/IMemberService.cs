using BusinessObject.RequestModel;
using DataAccess.Models;
using System;
using System.Collections.Generic;
using System.Threading.Tasks;

namespace BusinessObject.Services.Instance
{
    public interface IMemberService
    {
        // Get all members
        Task<IEnumerable<Member>> GetAllMembersAsync();

        // Get member by id
        Task<Member> GetMemberByIdAsync(int id);

        // Get member by email
        Task<Member> GetMemberByEmailAsync(string email);

        // Get orders for a member
        Task<IEnumerable<Order>> GetMemberOrdersAsync(int memberId);

        //// Create a new member
        //Task<Member> CreateMemberAsync(Member member);

        //// Update an existing member
        //Task UpdateMemberAsync(int id, Member member);

        // Delete a member
        Task DeleteMemberAsync(int id);

        // Check if email exists (excluding a specific member ID)
        Task<bool> EmailExistsAsync(string email, int? excludeMemberId = null);

        // Check if member has orders
        Task<bool> MemberHasOrdersAsync(int memberId);

        // Authenticate a member
        Task<Member> AuthenticateAsync(LoginRequestModel loginRequest);

        // Add a new member
        Task AddMemberAsync(Member member);

        // Update an existing member
        Task UpdateMemberAsync(Member member);

        // Authenticate a member
        Task<Member> AuthenticateAsync(string email, string password);
    }
}