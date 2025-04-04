using BusinessObject.Services.Instance;
using DataAccess.Models;
using DataAccess.Repositories;
using System;
using System.Collections.Generic;
using System.Threading.Tasks;
using BusinessObject.RequestModel;
using System.Text.RegularExpressions;

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
            if (user == null)
            {
                throw new ArgumentNullException(nameof(user), "User information cannot be null");
            }

            if (!IsValidEmail(user.Email))
            {
                throw new ArgumentException("Email address is not in a valid format", nameof(user.Email));
            }

            var existingUser = await _memberRepository.GetByIdAsync(user.MemberId);
            if (existingUser == null)
            {
                throw new Exception("User not found");
            }

            if (existingUser.Email != user.Email)
            {
                var memberWithSameEmail = await GetMemberByEmail(user.Email);
                if (memberWithSameEmail != null && memberWithSameEmail.MemberId != user.MemberId)
                {
                    throw new InvalidOperationException($"A member with email {user.Email} already exists");
                }
            }

            existingUser.City = user.City;
            existingUser.CompanyName = user.CompanyName;
            existingUser.Country = user.Country;
            existingUser.Email = user.Email;

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
            if (member == null)
            {
                throw new ArgumentNullException(nameof(member), "Member information cannot be null");
            }

            if (!IsValidEmail(member.Email))
            {
                throw new ArgumentException("Email address is not in a valid format", nameof(member.Email));
            }

            var existingMember = await GetMemberByEmail(member.Email);
            if (existingMember != null)
            {
                throw new InvalidOperationException($"A member with email {member.Email} already exists");
            }

            await _memberRepository.CreateAsync(member);
        }

        private bool IsValidEmail(string email)
        {
            if (string.IsNullOrWhiteSpace(email))
                return false;

            string pattern = @"^[a-zA-Z0-9._%+-]+@[a-zA-Z0-9.-]+\.[a-zA-Z]{2,}$";
            return Regex.IsMatch(email, pattern);
        }

        public async Task UpdateMemberAsync(Member member)
        {
            if (member == null)
            {
                throw new ArgumentNullException(nameof(member), "Member information cannot be null");
            }

            if (!IsValidEmail(member.Email))
            {
                throw new ArgumentException("Email address is not in a valid format", nameof(member.Email));
            }

            var existingMember = await _memberRepository.GetByIdAsync(member.MemberId);
            if (existingMember != null && existingMember.Email != member.Email)
            {
                var memberWithSameEmail = await GetMemberByEmail(member.Email);
                if (memberWithSameEmail != null && memberWithSameEmail.MemberId != member.MemberId)
                {
                    throw new InvalidOperationException($"A member with email {member.Email} already exists");
                }
            }

            await UpdateMemberProfile(member);
        }

        public async Task<Member> AuthenticateAsync(string email, string password)
        {
            return await LogIn(email, password);
        }
    }
}