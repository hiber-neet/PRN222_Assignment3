using DataAccess.Repositories;
using Asm03Solution.DataAccess.Repositories;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Microsoft.EntityFrameworkCore;
using DataAccess.Models;
using Asm03Solution.DataAccess.Models;

namespace BusinessObject.Services
{
    public class MemberService
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
            // Lấy entity từ database trước
            var existingUser = await _memberRepository.GetByIdAsync(user.MemberId);
            if (existingUser == null)
            {
                throw new Exception("User not found");
            }

            // Cập nhật giá trị mới từ user
            existingUser.City = user.City;
            existingUser.CompanyName = user.CompanyName;
            existingUser.Country = user.Country;

            var result = await _memberRepository.UpdateAsync(existingUser);
            return result > 0;
        }

    }
}
