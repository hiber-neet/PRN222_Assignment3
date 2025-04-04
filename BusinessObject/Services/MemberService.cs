using BusinessObject.Services.Instance;
using DataAccess.Models;
using DataAccess.Repositories;
using System;
using System.Collections.Generic;
using System.Threading.Tasks;
using BusinessObject.RequestModel;

namespace BusinessObject.Services
{
    public class MemberService : IMemberService
    {
        private readonly MemberRepository _memberRepository;

        public MemberService(MemberRepository memberRepository)
        {
            _memberRepository = memberRepository;
        }

        public async Task<Member> LogIn(string userName, string password)
        {
            return await _memberRepository.LogIn(userName, password);
        }

        public async Task<List<Member>> GetAllUser()
        {
            return await _memberRepository.GetAllUser();
        }

        public async Task<Member> GetMemberByEmail(string email)
        {
            return await _memberRepository.GetMemberByEmail(email);
        }

        public async Task<bool> UpdateMemberProfile(Member user)
        {
           
            var existingUser = await _memberRepository.GetByIdAsync(user.MemberId);
            if (existingUser == null)
            {
                throw new Exception("User not found");
            }

          
            existingUser.City = user.City;
            existingUser.CompanyName = user.CompanyName;
            existingUser.Country = user.Country;

            var result = await _memberRepository.UpdateAsync(existingUser);
            return result > 0;
        }

      
        public async Task<IEnumerable<Member>> GetAllMembersAsync()
        {
            return await GetAllUser();
        }

        public async Task<Member> GetMemberByIdAsync(int id)
        {
            return await _memberRepository.GetByIdAsync(id);
        }

        public async Task<Member> GetMemberByEmailAsync(string email)
        {
            return await GetMemberByEmail(email);
        }

        public async Task<IEnumerable<Order>> GetMemberOrdersAsync(int memberId)
        {
            var member = await _memberRepository.GetByIdAsync(memberId);
            return member?.Orders;
        }

        public async Task DeleteMemberAsync(int id)
        {
            var member = await _memberRepository.GetByIdAsync(id);
            if (member != null)
            {
                _memberRepository.Remove(member);
                await _memberRepository.SaveAsync();
            }
        }

        public async Task<bool> EmailExistsAsync(string email, int? excludeMemberId = null)
        {
            var member = await GetMemberByEmail(email);
            if (member == null) return false;
            if (excludeMemberId.HasValue)
            {
                return member.MemberId != excludeMemberId.Value;
            }
            return true;
        }

        public async Task<bool> MemberHasOrdersAsync(int memberId)
        {
            var orders = await GetMemberOrdersAsync(memberId);
            return orders != null && orders.Any();
        }

        public async Task<Member> AuthenticateAsync(LoginRequestModel loginRequest)
        {
            if (loginRequest == null || string.IsNullOrEmpty(loginRequest.Email) || string.IsNullOrEmpty(loginRequest.Password))
            {
                return null;
            }
            return await LogIn(loginRequest.Email, loginRequest.Password);
        }

        public async Task AddMemberAsync(Member member)
        {
            await _memberRepository.CreateAsync(member);
        }

        public async Task UpdateMemberAsync(Member member)
        {
            await UpdateMemberProfile(member);
        }

        public async Task<Member> AuthenticateAsync(string email, string password)
        {
            return await LogIn(email, password);
        }
    }
}